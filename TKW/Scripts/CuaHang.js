document.getElementById("selectStore").addEventListener("change", function () {

    var map = document.getElementById("mapFrame");

    // Ẩn cả hai khối
    document.getElementById("storeInfo").classList.add("d-none");
    document.getElementById("storeInfo2").classList.add("d-none");

    // Hiện đúng cửa hàng
    if (this.value === "store1") {
        document.getElementById("storeInfo").classList.remove("d-none");
        map.src = "https://maps.google.com/maps?q=162%20HT17%20Tan%20Thoi%20Hiep%20Quan%2012&t=&z=15&ie=UTF8&iwloc=&output=embed";
    }

    if (this.value === "store2") {
        document.getElementById("storeInfo2").classList.remove("d-none");
        map.src = "https://maps.google.com/maps?q=Vinhomes%20Grand%20Park&t=&z=15&ie=UTF8&iwloc=&output=embed";
    }
});