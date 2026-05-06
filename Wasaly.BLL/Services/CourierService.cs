using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wasaly.BLL.Services.Interfaces;
using Wasaly.BLL.ViewModels;
using Wasaly.DAL.Configuration;
using Wasaly.DAL.Enums;
using Wasaly.DAL.Models;
using Wasaly.DAL.Repositories.IRepositories;

namespace Wasaly.BLL.Services
{
    public class CourierService : ICourierService
    {
        private readonly ICourierRepository _courierRepository;
        private readonly IEmailService _emailService;

        public CourierService(ICourierRepository courierRepository,IEmailService emailService)
        {
            _courierRepository = courierRepository;
            _emailService = emailService;
        }

        public async Task<List<AvailableShipmentVM>> GetAvailableShipmentsAsync()
        {
            List<Shipment> shipments=await _courierRepository.GetAvailableShipmentsAsync();

            return shipments.Select(s => new AvailableShipmentVM
            {
                Id = s.Id,
                TrackingNumber = s.TrackingNumber,
                Description = s.Description,
                Weight = s.Weight,
                Price = s.Price,
                MerchantName = s.Merchant.WasalyIdentityUser.FullName,
                PickupAddress = s.PickupLocation.Address,
                PickupLatitude = s.PickupLocation.Latitude,
                PickupLongitude = s.PickupLocation.Longitude,
                DropAddress = s.DropLocation.Address,
                DropLatitude = s.DropLocation.Latitude,
                DropLongitude = s.DropLocation.Longitude,
                CreatedAt = s.CreatedAt
            }).ToList();
        }
        public async Task<bool> AcceptShipmentAsync(int shipmentId, string courierId)
        {
            Shipment shipment =await _courierRepository.GetShipmentWithDetailsAsync(shipmentId);
            if (shipment == null || shipment.Status != ShipmentStatus.Created) return false;

            var alreadyAccepted = shipment.CourierAssignments.Any(a => a.CourierId == courierId);

            if (alreadyAccepted)
                return false;

            var assignment = new CourierAssignment
            {
                CourierId = courierId,
                ShipmentId = shipmentId,
                Status = CourierStatus.Accepted,
                AssignedAt = DateTime.Now
            };
            var courierName = await _courierRepository.GetCourierNameAsync(courierId);

            var tracking = new ShipmentTracking
            {
                ShipmentId = shipmentId,
                Status = ShipmentStatus.Accepted,
                TimeStamp = DateTime.Now,
                Note = $"تم قبول الشحنة من قبل المندوب {courierName}"
            };

            await _courierRepository.AddAssignmentAsync(assignment);
            await _courierRepository.AddTrackingAsync(tracking);
            await _courierRepository.UpdateShipmentStatusAsync(shipmentId, ShipmentStatus.Accepted);

            await _courierRepository.SaveAsync();
            return true;
        }

        public async Task<bool> PickupShipmentAsync(int shipmentId, string courierId)
        {
            Shipment? shipment = await _courierRepository.GetShipmentWithDetailsAsync(shipmentId);
            if (shipment == null || shipment.Status != ShipmentStatus.Accepted) return false;
            var isAssigned =  shipment.CourierAssignments
                            .Any(a => a.CourierId == courierId && a.Status == CourierStatus.Accepted);

            if (!isAssigned)
                return false;

            var courierName = await _courierRepository.GetCourierNameAsync(courierId);

            var tracking = new ShipmentTracking
            {
                ShipmentId = shipmentId,
                Status = ShipmentStatus.PickedUp,
                TimeStamp = DateTime.Now,
                Note = $"تم استلام الشحنة من التاجر بواسطة {courierName}"
            };
            await _courierRepository.AddTrackingAsync(tracking);
            await _courierRepository.UpdateShipmentStatusAsync(shipmentId, ShipmentStatus.PickedUp);
            await _courierRepository.SaveAsync();
            return true;
        }

        public async Task<List<CourierShipmentVM>> GetCourierShipmentsAsync(string courierId)
        {
            var assignments = await _courierRepository.GetCourierAssignmentsAsync(courierId);

            return assignments.Select(a => new CourierShipmentVM
            {
                ShipmentId = a.Shipment.Id,
                AssignmentId = a.Id,
                TrackingNumber = a.Shipment.TrackingNumber,
                Description = a.Shipment.Description,
                Weight = a.Shipment.Weight,
                Price = a.Shipment.Price,
                ShipmentStatus = a.Shipment.Status,
                AssignmentStatus = a.Status,
                PickupAddress = a.Shipment.PickupLocation.Address,
                DropAddress = a.Shipment.DropLocation.Address,
                AssignedAt = a.AssignedAt,
                TrackingHistory = a.Shipment.Trackings
                    .OrderBy(t => t.TimeStamp)
                    .Select(t => new TrackingHistoryVM
                    {
                        Status = t.Status,
                        StatusArabic = GetStatusArabic(t.Status),
                        TimeStamp = t.TimeStamp,
                        Note = t.Note
                    }).ToList()
            }).ToList();
        }


        private static string GetStatusArabic(ShipmentStatus status) => status switch
        {
            ShipmentStatus.Created => "تم الإنشاء",
            ShipmentStatus.Accepted => "تم القبول",
            ShipmentStatus.PickedUp => "تم الاستلام",
            ShipmentStatus.Delivered => "تم التسليم",
            _ => status.ToString()
        };


        public async Task<bool> GenerateAndSendOtpAsync(int shipmentId)
        {
            // 1. تأكد إن الشحنة PickedUp
            var shipment = await _courierRepository.GetShipmentWithDetailsAsync(shipmentId);

            if (shipment == null || shipment.Status != ShipmentStatus.PickedUp)
                return false;

            // 2. لو فيه OTP قديم امسحه
            var oldOtp = await _courierRepository.GetOtpByShipmentAsync(shipmentId);
            if (oldOtp != null)
                await _courierRepository.DeleteOtpAsync(oldOtp);

            // 3. توليد OTP بـ Cryptographically Secure Random
            var otpCode = GenerateSecureOtp();

            // 4. حفظ الـ OTP في الداتابيز
            var otp = new DeliveryOTP
            {
                ShipmentId = shipmentId,
                OTPCode = otpCode,
                CreatedAt = DateTime.Now,
                ExpiryTime = DateTime.Now.AddMinutes(10),
                IsUsed = false
            };

            await _courierRepository.AddOtpAsync(otp);
            await _courierRepository.SaveAsync();

            await _emailService.SendOtpAsync(shipment.RecipientEmail,shipment.RecipientName,otpCode);

            return true;
        }

        private static string GenerateSecureOtp()
        {
            // Cryptographically Secure Random
            var bytes = new byte[4];
            using var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
            rng.GetBytes(bytes);
            var number = Math.Abs(BitConverter.ToInt32(bytes, 0)) % 1000000;
            return number.ToString("D6");
        }



        public async Task<(bool Success, string Message)> VerifyOtpAndDeliverAsync(VerifyOtpVM model, string courierId)
        {
            // 1. جيب الـ OTP
            var otp = await _courierRepository.GetOtpByShipmentAsync(model.ShipmentId);

            // 2. موجود؟
            if (otp == null)
                return (false, "لم يتم إرسال كود لهذه الشحنة");

            // 3. اتستخدم قبل كده؟
            if (otp.IsUsed)
                return (false, "تم استخدام هذا الكود من قبل");

            // 4. انتهت صلاحيته؟
            if (otp.ExpiryTime < DateTime.Now)
                return (false, "انتهت صلاحية الكود، اطلب كود جديد");

            // 5. الكود صح؟
            if (otp.OTPCode != model.OtpCode)
                return (false, "الكود غير صحيح ❌");

            // 6. جيب الشحنة وتأكد إن المندوب مخول
            var shipment = await _courierRepository.GetShipmentWithDetailsAsync(model.ShipmentId);
            if (shipment == null)
                return (false, "الشحنة غير موجودة");

            var isAssigned = shipment.CourierAssignments
                .Any(a => a.CourierId == courierId && a.Status == CourierStatus.Accepted);

            if (!isAssigned)
                return (false, "أنت غير مسئول بتسليم هذه الشحنة");

            // 7. جيب اسم المندوب
            var courierName = await _courierRepository.GetCourierNameAsync(courierId);

            // 8. كل حاجة تمام — نسلم الشحنة
            otp.IsUsed = true;

            await _courierRepository.UpdateShipmentStatusAsync(model.ShipmentId, ShipmentStatus.Delivered);

            var tracking = new ShipmentTracking
            {
                ShipmentId = model.ShipmentId,
                Status = ShipmentStatus.Delivered,
                TimeStamp = DateTime.Now,
                Note = $"تم تسليم الشحنة للعميل بواسطة {courierName} ✅"
            };

            await _courierRepository.UpdateCourierBalanceAsync(courierId, shipment.Price);
            await _courierRepository.AddTrackingAsync(tracking);
            await _courierRepository.SaveAsync();

            return (true, "تم تسليم الشحنة بنجاح ");
        }
        public async Task<CourierDashboardVM> GetDashboardAsync(string courierId)
        {
            var courier = await _courierRepository.GetCourierWithDetailsAsync(courierId);
            if (courier == null) return new CourierDashboardVM();

            var allAssignments = await _courierRepository.GetAllCourierAssignmentsAsync(courierId);

            var today = DateTime.Today;
            var weekAgo = DateTime.Today.AddDays(-7);

            return new CourierDashboardVM
            {
                CourierName = courier.WasalyIdentityUser.FullName,
                Balance = courier.Balance,

                // توصيلات اليوم
                TodayDeliveries = allAssignments
                    .Count(a => a.Shipment.Status == ShipmentStatus.Delivered
                             && a.Shipment.DeliveredAt?.Date == today),

                // أرباح الأسبوع
                WeekEarnings = allAssignments
                    .Where(a => a.Shipment.Status == ShipmentStatus.Delivered
                             && a.Shipment.DeliveredAt >= weekAgo)
                    .Sum(a => a.Shipment.Price),

                // نسبة القبول
                AcceptanceRate = allAssignments.Any()
                    ? (int)(allAssignments.Count(a => a.Status == CourierStatus.Accepted)
                      * 100.0 / allAssignments.Count)
                    : 0,

                // آخر الشحنات
                RecentShipments = allAssignments
                    .Where(a => a.Shipment.Status == ShipmentStatus.Delivered)
                    .OrderByDescending(a => a.Shipment.DeliveredAt)
                    .Take(3)
                    .Select(a => new CourierShipmentVM
                    {
                        ShipmentId = a.Shipment.Id,
                        TrackingNumber = a.Shipment.TrackingNumber,
                        Description = a.Shipment.Description,
                        Price = a.Shipment.Price,
                        ShipmentStatus = a.Shipment.Status,
                        DropAddress = a.Shipment.DropLocation.Address
                    }).ToList()
            };
        }

    }
}
