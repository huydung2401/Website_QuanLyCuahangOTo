// Mở popup giỏ hàng
$(document).on("click", "#btnCart", function (e) {
    e.preventDefault();

    $("#cartOverlay").addClass("active");
    $("#cartSide").addClass("active");

    LoadPopupGioHang();
});

// Đóng popup khi click vào overlay
$(document).on("click", "#cartOverlay", function () {
    $("#cartOverlay").removeClass("active");
    $("#cartSide").removeClass("active");
});

// Load nội dung popup cart
function LoadPopupGioHang() {
    $("#cartContent").load("/GioHang/Popup");
}

// Thêm sản phẩm vào giỏ
function ThemGio(id) {
    $.post("/GioHang/ThemGioHang", { id: id }, function (res) {

        // 🔥 SHOW THÔNG BÁO
        showAddToCartToast();

        // 🔥 MỞ POPUP GIỎ HÀNG
        $("#cartOverlay").addClass("active");
        $("#cartSide").addClass("active");

        // Reload giỏ
        LoadPopupGioHang();

        // Cập nhật badge
        UpdateCartBadge();
    });
}

// Xóa sản phẩm
function removeItem(id) {
    $.post("/GioHang/Xoa/" + id, function (res) {
        if (res.success) {
            LoadPopupGioHang();
            UpdateCartBadge();
        }
    });
}

// Tăng giảm số lượng
function updateQuantity(id, type) {
    $.post("/GioHang/CapNhatSoLuong", { id: id, type: type }, function (res) {
        if (res.success) {
            LoadPopupGioHang();
            UpdateCartBadge();
        }
    });
}

// Badge số lượng
function UpdateCartBadge() {
    $.get("/GioHang/SoLuong", function (res) {
        $("#cartBadge").text(res.count);
    });
}

// Cập nhật số lượng khi load trang
$(document).ready(function () {
    UpdateCartBadge();
});

// 🔥 HÀM HIỂN THỊ TOAST THÊM VÀO GIỎ
function showAddToCartToast() {
    let toast = `
        <div id="cartToast"
             style="position: fixed; top: 20px; right: 20px; 
                    background: #28a745; color: white; 
                    padding: 12px 20px; border-radius: 6px; 
                    font-size: 15px; z-index: 99999; 
                    box-shadow: 0 3px 10px rgba(0,0,0,0.2);
                    animation: fadeOut 2s forwards;">
            ✓ Đã thêm vào giỏ hàng
        </div>

        <style>
            @keyframes fadeOut {
                0% { opacity: 1; }
                70% { opacity: 1; }
                100% { opacity: 0; transform: translateY(-20px); }
            }
        </style>
    `;

    $("body").append(toast);

    setTimeout(() => {
        $("#cartToast").remove();
    }, 2500);
}
