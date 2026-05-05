using Day9Demo.Models;

namespace Wasaly.DAL.Repositories.IRepositories
{
    public interface IUserRepository
    {

        // 1. لجلب قائمة المناديب الذين ينتظرون التوثيق (لجدول الأدمن)
        // نستخدم IEnumerable لأننا نعرض قائمة
        public Task<IEnumerable<Courier>> GetPendingCouriersAsync();

        // 2. تحديث حالة المندوب (قبول أو رفض التوثيق)
        // نمرر الـ ID والحالة الجديدة، ونرجع bool للتأكيد
        public Task<bool> UpdateCourierStatusAsync(string courierId, bool status);

        //// 3. جلب بيانات إحصائية مجمعة للأدمن
        //// هنا نستخدم الـ DTO اللي بيشيل الأرقام الخام من الداتابيز
        //public Task<AdminStatsVM> GetDashboardStatsAsync();

        // 4. جلب بيانات مندوب محدد للمراجعة
        // عشان لما الأدمن يدوس "مراجعة" يشوف صور البطاقة والرخصة
        public Task<Courier> GetCourierByIdAsync(string id);



    }
}
