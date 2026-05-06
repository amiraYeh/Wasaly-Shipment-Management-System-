//$(document).ready(function () {
//    // فتح السايد بار
//    $('#sidebarToggle').on('click', function () {
//        $('#sidebar').addClass('active');
//        $('#sidebarOverlay').addClass('show');
//    });

//    // إغلاق السايد بار
//    $('#closeSidebar, #sidebarOverlay').on('click', function () {
//        $('#sidebar').removeClass('active');
//        $('#sidebarOverlay').removeClass('show');
//    });
//});
document.addEventListener("DOMContentLoaded", function () {
    const sidebar = document.getElementById('sidebar');
    const toggleBtn = document.getElementById('sidebarToggle');
    const closeBtn = document.getElementById('closeSidebar');
    const overlay = document.getElementById('sidebarOverlay');

    if (toggleBtn) {
        toggleBtn.addEventListener('click', function () {
            sidebar.classList.toggle('active');
            overlay.classList.toggle('show');
        });
    }

    if (closeBtn) {
        closeBtn.addEventListener('click', function () {
            sidebar.classList.remove('active');
            overlay.classList.remove('show');
        });
    }

    if (overlay) {
        overlay.addEventListener('click', function () {
            sidebar.classList.remove('active');
            overlay.classList.remove('show');
        });
    }
});