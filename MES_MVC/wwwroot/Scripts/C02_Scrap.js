let ScrapTable;
let ScrapTotalTable;

$(document).ready(function () {
    loadScrapTable();
});

function loadScrapTable() {
    ScrapTable = $("#ScrapList").DataTable({
        scrollX: true,
        scrollY: "15vh",
        scrollCollapse: true,
        paging: true,
        pageLength: 50,
        lengthChange: false,
        autoWidth: false,
        responsive: false,
        ordering: true,
        searching: false,
        fixedHeader: true,
    });

    ScrapTotalTable = $("#ScrapListTotal").DataTable({
        scrollX: true,
        scrollY: "15vh",
        scrollCollapse: true,
        paging: true,
        pageLength: 50,
        lengthChange: false,
        autoWidth: false,
        responsive: false,
        ordering: true,
        searching: false,
        fixedHeader: true,
    });

    $(".dataTables_scrollBody").css("min-height", "15vh");


    $("#Scrap_WireLength").val();
    $("#Scrap_WireCount").val();
}

// Hàm load dữ liệu
function loadScrapData(ScrapDetail) {
    let sampleData = [];

    let totalSeal1 = 0;
    let totalSeal2 = 0;
    let totalTerm1 = 0;
    let totalTerm2 = 0;
    let totalLengt = 0;

    ScrapDetail.forEach(item => {

        totalTerm1 += item.TERM1 ? 1 : 0;
        totalTerm2 += item.TERM2 ? 1 : 0;
        totalSeal1 += item.SEAL1 ? 1 : 0;
        totalSeal2 += item.SEAL2 ? 1 : 0;
        totalLengt += parseInt(item.WIRE_LENGTH) || 0;
        priceTerm1 = parseFloat(item.PriceTerm1) || 0;
        priceTerm2 = parseFloat(item.PriceTerm2) || 0;
        priceSeal1 = parseFloat(item.PriceSeal1) || 0;
        priceSeal2 = parseFloat(item.PriceSeal2) || 0;
        priceWire = parseFloat(item.WirePrice) || 0;

        sampleData.push([
            item.WireNumber,
            1,
            item.TERM1 ? 1 : 0,
            item.SEAL1 ? 1 : 0,
            parseInt(item.WIRE_LENGTH) || 0,
            item.TERM2 ? 1 : 0,
            item.SEAL2 ? 1 : 0, 
            item.MC,
            item.CREATE_DTM,
            item.CREATE_USER,
        ]);
    });
            
    var term1Code = $("#C02_START_V2_Label10").val();
    var term2Code = $("#C02_START_V2_Label14").val();
    var seal1Code = $("#C02_START_V2_Label12").val();
    var seal2Code = $("#C02_START_V2_Label16").val();
    var wireCode = $("#C02_START_V2_WireCode").val();
    var totalComplete = parseFloat($("#C02_START_V2_Label59").val()) || 0;
    var BOMLenght = parseInt($("#C02_START_V2_WIRE_Length").val().replace(/,/g, "")) || 1;

    let totalLenghtBOM = totalComplete * BOMLenght;

    let tyleTerm1 = (totalTerm1 / totalComplete) * 100;
    let tyleTerm2 = (totalTerm2 / totalComplete) * 100;
    let tyleSeal1 = (totalSeal1 / totalComplete) * 100;
    let tyleSeal2 = (totalSeal2 / totalComplete) * 100;
    let tyleDay = (totalLengt / totalLenghtBOM) * 100;

    let sampleTotal = [];

    // Sửa dấu thập phân: dùng dấu chấm
    if (totalTerm1 > 0) {
        sampleTotal.push([term1Code, "EA", totalTerm1, tyleTerm1.toFixed(2) + "%", 0, (totalTerm1 * priceTerm1).toFixed(5)]);
    }
    if (totalSeal1 > 0) {
        sampleTotal.push([seal1Code, "EA", totalSeal1, tyleSeal1.toFixed(2) + "%", 0, (totalSeal1 * priceSeal1).toFixed(5)]);
    }

    if (totalTerm2 > 0) {
        sampleTotal.push([term2Code, "EA", totalTerm2, tyleTerm2.toFixed(2) + "%", 0, (totalTerm2 * priceTerm2).toFixed(5)]);
    }
    if (totalSeal2 > 0) {
        sampleTotal.push([seal2Code, "EA", totalSeal2, tyleSeal2.toFixed(2) + "%", 0, (totalSeal2 * priceSeal2).toFixed(5)]);
    }

    if (totalLengt > 0) {
        sampleTotal.push([wireCode, "mm", totalLengt, tyleDay.toFixed(2) + "%", 0, (totalLengt * priceWire).toFixed(5)]);
    }
    

    // Clear table trước khi bind
    ScrapTable.clear();
    ScrapTable.rows.add(sampleData);
    ScrapTable.draw();

    ScrapTotalTable.clear();
    ScrapTotalTable.rows.add(sampleTotal);
    ScrapTotalTable.draw();
}



document.querySelectorAll("#Scrap_WireLength, #Scrap_WireCount").forEach(input => {
    input.addEventListener("input", function () {
        let value = this.value;

        // Nếu không phải số
        if (isNaN(value) || value === "") {
            M.toast({ html: "Vui lòng nhập số hợp lệ >0", classes: "red" });
            this.value = null;
            return;
        }

        // Ép giá trị >= 0
        if (Number(value) < 0) {
            M.toast({ html: "Chỉ được nhập số >0", classes: "red" });
            this.value = null;
        }
    });
});


//load giao dien ẩn hiện theo thông tin đơn cắt
function updateScrapCheckboxes() {
    const rules = [
        { input: "#C02_START_V2_Label10", checkbox: "#Scrap_checkTerm1" },
        { input: "#C02_START_V2_Label12", checkbox: "#Scrap_checkSeal1" },
        { input: "#C02_START_V2_Label14", checkbox: "#Scrap_checkTerm2" },
        { input: "#C02_START_V2_Label16", checkbox: "#Scrap_checkSeal2" }
    ];

    rules.forEach(x => {
        let val = $(x.input).val();
        $(x.checkbox).prop("checked", false);

        if (!val || (val.includes("(") && val.includes(")"))) {
            $(x.checkbox).prop("disabled", true).hide();
        } else {
            $(x.checkbox).prop("disabled", false).show();
        }
    });

    // Reset lại các input số
    $("#Scrap_WireLength").val('');  // rỗng
    $("#Scrap_WireCount").val(1);    // 1

}

$("#C02_START_V2_ButtonScrap_Save").click(function (e) {

    var term1Code = $("#C02_START_V2_Label10").val().trim();
    var term2Code = $("#C02_START_V2_Label14").val().trim();
    var seal1Code = $("#C02_START_V2_Label12").val().trim();
    var seal2Code = $("#C02_START_V2_Label16").val().trim();
    var wireName = $("#C02_START_V2_Label18").val().trim();
    var wireCode = $("#C02_START_V2_WireCode").val().trim();
    var MCName = $("#C02_START_V2_L_MCNM").val().trim();
    var OrderIDX = $("#C02_START_V2_Label8").val().trim();

    // bỏ dấu phẩy, chuyển sang số
    var wireBOMLenght = parseFloat($("#C02_START_V2_WIRE_Length").val().replace(/,/g, "")) || 0;

    var wireCount = parseFloat($("#Scrap_WireCount").val().replace(/,/g, "")) || 0;
    var wireLenght = parseFloat($("#Scrap_WireLength").val().replace(/,/g, "")) || 0;

    // checkbox
    var checkTerm1 = $("#Scrap_checkTerm1").prop("checked");
    var checkTerm2 = $("#Scrap_checkTerm2").prop("checked");
    var checkSeal1 = $("#Scrap_checkSeal1").prop("checked");
    var checkSeal2 = $("#Scrap_checkSeal2").prop("checked");

    // validate
    if (wireCount <= 0) {
        ShowMessage("Chưa nhập số lượng dây!", "error");
        return;
    }

    if (wireLenght < 0) {
        ShowMessage("Chiều dài dây không hợp lệ! (0 → " + (wireBOMLenght + 5) + " mm)", "error");
        return;
    }

    // nếu checkbox không được check → xóa code
    if (!checkTerm1) term1Code = "";
    if (!checkTerm2) term2Code = "";
    if (!checkSeal1) seal1Code = "";
    if (!checkSeal2) seal2Code = "";

    var UserID = GetCookieValue("UserID");

    $("#BackGround").css("display", "block");

    let BaseParameter = {
        ImportData:[ {
            WIRE: wireCode,
            WIRE_LENGTH: wireLenght,
            TERM1: term1Code,
            TERM2: term2Code,
            SEAL1: seal1Code,
            SEAL2: seal2Code,
            ORDER_IDX: OrderIDX,
            MC: MCName,
            COUNT: wireCount
        }],
        USER_ID: UserID,
        USER_IDX: GetCookieValue("USER_IDX")
    };

    let formUpload = new FormData();
    formUpload.append("BaseParameter", JSON.stringify(BaseParameter));

    fetch("/C02_START_V2/ScrapSave", {
        method: "POST",
        body: formUpload
    }).then(response => {
        return response.json();
    }).then(data => {
        $("#BackGround").css("display", "none");
        // xử lý kết quả backend
        if (data.DataGridView && Array.isArray(data.DataGridView)) { 
            loadScrapData(data.DataGridView);
        }
    }).catch(err => {
        $("#BackGround").css("display", "none");
        console.error(err);
    });

    updateScrapCheckboxes();
});



