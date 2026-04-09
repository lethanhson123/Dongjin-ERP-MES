let LEAD_CONT_OUT = 0;
let TempData = [];
let LEAD_CONT_IN = 0;
let TempDataIn = [];
let currentGroup = "";
let currentGroupOut = "";
let currentTab = "#TabPage6";
let IsTableSort2 = false;
let IsTableSort1 = false;
let IsTableSort3 = false;
let IsTableSort4 = false;
let NoGroupLock = false;
let isProcessingInput = false;
let isProcessingOutput = false;
let barcodeQueue = [];
let isProcessingQueue = false;
let C03_IN_TrolleyData = [];
let CompanyID = 16;
let CategoryDepartmentID = 0;
let CategoryDepartmentDisplay = "";
let CategoryDepartmentCode = "";
let BaseResult = new Object();

$(document).ready(function () {

    var IPAddress = location.host;
    if (IPAddress.includes("192.168.1.240")) {
        CompanyID = 17;
    }

    $('select').formSelect();
    $('.modal').modal();

    let today = new Date();
    let todayFormatted = formatDateForInput(today);
    let yesterday = new Date(today);
    yesterday.setDate(yesterday.getDate() - 1);
    let yesterdayFormatted = formatDateForInput(yesterday);

    $("#DateTimePicker2").val(todayFormatted);
    $("#DateTimePicker1").val(yesterdayFormatted);

    CategoryDepartmentGetByCompanyID_ActiveToList();
});

function CategoryDepartmentGetByCompanyID_ActiveToList() {
    BaseResult.ListCategoryDepartment = new Object();
    BaseResult.ListCategoryDepartment = [];
    //$("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        CompanyID: CompanyID,
    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C03/CategoryDepartmentGetByCompanyID_ActiveToList";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            console.log(data.ListCategoryDepartment);
            BaseResult.ListCategoryDepartment = data.ListCategoryDepartment;
            for (let i = 0; i < BaseResult.ListCategoryDepartment.length; i++) {
                if (BaseResult.ListCategoryDepartment[i].ParentID == 86 || BaseResult.ListCategoryDepartment[i].ParentID == 195) {
                    $("#CategoryDepartmentINPUT").append("<option value='" + BaseResult.ListCategoryDepartment[i].ID + "' style='color: #000000;'>" + BaseResult.ListCategoryDepartment[i].ParentName + " - " + BaseResult.ListCategoryDepartment[i].Code + "</option>");
                    $("#CategoryDepartmentOUTPUT").append("<option value='" + BaseResult.ListCategoryDepartment[i].ID + "' style='color: #000000;'>" + BaseResult.ListCategoryDepartment[i].ParentName + " - " + BaseResult.ListCategoryDepartment[i].Code + "</option>");
                }
                else {
                    $("#CategoryDepartmentReINPUT").append("<option value='" + BaseResult.ListCategoryDepartment[i].ID + "'>" + BaseResult.ListCategoryDepartment[i].ParentName + " - " + BaseResult.ListCategoryDepartment[i].Code + "</option>");
                    $("#CategoryDepartmentINPUT").append("<option value='" + BaseResult.ListCategoryDepartment[i].ID + "'>" + BaseResult.ListCategoryDepartment[i].ParentName + " - " + BaseResult.ListCategoryDepartment[i].Code + "</option>");
                    $("#CategoryDepartmentOUTPUT").append("<option value='" + BaseResult.ListCategoryDepartment[i].ID + "'>" + BaseResult.ListCategoryDepartment[i].ParentName + " - " + BaseResult.ListCategoryDepartment[i].Code + "</option>");
                }
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
$('#CategoryDepartmentINPUT').on('change', function () {
    $('#C03_IN_TextBox1').focus();
    CategoryDepartmentID = $("#CategoryDepartmentINPUT").val();
    SetCategoryDepartmentDisplay();
});
$('#CategoryDepartmentOUTPUT').on('change', function () {
    $('#TextBox4').focus();
    CategoryDepartmentID = $("#CategoryDepartmentOUTPUT").val();
    SetCategoryDepartmentDisplay();
});

$('#CategoryDepartmentReINPUT').on('change', function () {
    $('#C03_RE_IN_TextBox1').focus();
    CategoryDepartmentID = $("#CategoryDepartmentReINPUT").val();
    SetCategoryDepartmentDisplay();
});
function SetCategoryDepartmentDisplay() {
    let ListCategoryDepartmentFilter = BaseResult.ListCategoryDepartment.filter(x => x.ID == CategoryDepartmentID);
    if (ListCategoryDepartmentFilter != null && ListCategoryDepartmentFilter.length > 0) {
        let CategoryDepartment = ListCategoryDepartmentFilter[0];
        CategoryDepartmentDisplay = CategoryDepartment.Display;
        CategoryDepartmentCode = CategoryDepartment.Code;
        if (CategoryDepartmentDisplay == null) {
            CategoryDepartmentDisplay = "";
        }
    }
}


$('input[name="transactionType"]').change(function () {
    let type = $(this).val();

    $("#listContainer").css("display", "none");
    $("#leadContainer").css("display", "none");
    $("#inoutContainer").css("display", "none");

    switch (type) {
        case "IN":
        case "OUT":
            $("#listContainer").css("display", "block");
            break;
        case "LEAD":
            $("#leadContainer").css("display", "block");
            break;
        case "INOUT":
            $("#inoutContainer").css("display", "block");
            break;
    }
});

$("#Tab5_LeadNoFilter").keydown(function (e) {
    if (e.keyCode === 13) {
        Buttonfind_Click();
        e.preventDefault();
    }
});

$("input[name='Tab5_leadType']").change(function () {
    if ($("#Tab5_leadRadio").prop("checked")) {
        $("#Tab5_SubPartPanel").prop("enabled", false);
    } else {
        $("#Tab5_SubPartPanel").prop("enabled", true);
    }
});

$("#Tab5_wirePNo").blur(function () {
    checkPartExists($(this).val(), "Tab5_wirePNo");
});

$("#Tab5_term1").blur(function () {
    checkPartExists($(this).val(), "Tab5_term1");
});

$("#Tab5_seal1").blur(function () {
    checkPartExists($(this).val(), "Tab5_seal1");
});

$("#Tab5_term2").blur(function () {
    checkPartExists($(this).val(), "Tab5_term2");
});

$("#Tab5_seal2").blur(function () {
    checkPartExists($(this).val(), "Tab5_seal2");
});

$("#ButtonReset_IN").click(function () {
    C03_IN_Load();
});

$("#ButtonReset_OUT").click(function () {
    C03_OUT_Load();
});

$("#Buttonadd").click(function () {
    Buttonadd_Click();
});

$("#Buttoncancel").click(function () {
    Buttoncancel_Click();
});

$(document).keydown(function (e) {
    if (e.keyCode === 32) {
        if ($('#C03_IN_Modal').hasClass('open')) {
            C03_IN_Load();
            e.preventDefault();
        } else if ($('#C03_OUT_Modal').hasClass('open')) {
            C03_OUT_Load();
            e.preventDefault();
        }
    }
});

$("#C03_IN_Buttonfind").click(function () {
    Buttonfind_Click_IN();
});

$("#C03_IN_Buttonclose").click(function () {
    Buttonclose_Click_IN();
});

$("#C03_IN_TextBox1").keydown(function (e) {
    if (e.keyCode === 13) {
        DATA_ADD_IN();
        e.preventDefault();
    }
});

$("#Buttonfind").click(function () {
    Buttonfind_Click();
});

$("#Buttonclose").click(function () {
    Buttonclose_Click();
});

$("#TextBox4").keydown(function (e) {
    if (e.keyCode === 13) {
        DATA_ADD();
        e.preventDefault();
    }
});

$('#C03_IN_Modal').on('modal:close', function () {
    $("#C03_IN_TextBox1").val("");
    $("#C03_IN_Label4").text("");
    $("#C03_IN_Label5").text("");
    $("#C03_IN_Label12").text("");
    $("#C03_IN_Label37").text("0");
    LEAD_CONT_IN = 0;
    TempDataIn = [];
    DataGridViewInRender();
});

$('#C03_OUT_Modal').on('modal:close', function () {
    $("#TextBox4").val("");
    $("#Label15").text("");
    $("#Label11").text("");
    $("#Label38").text("0");
    LEAD_CONT_OUT = 0;
    TempData = [];
    currentGroupOut = "";
    DataGridView1Render();
});
$(document).on('keydown.confirmYes', function (e) {
    if (e.keyCode === 13 && $('#C03_IN_ConfirmModal').hasClass('open')) {
        $("#C03_IN_ConfirmYes").trigger('click');
    }
});
$('#C03_IN_ConfirmModal').on('modal:close', function () {
    $(document).off('keydown.confirmYes');
});
initNoGroupF2();
$("#C03_RE_IN_Buttonclose").click(function () {
    $('#C03_RE_IN_Modal').modal('close');
});

$("#C03_RE_IN_Buttonfind").click(function () {
    C03_RE_IN_Load();
});

$("#C03_RE_IN_Buttonsave").click(function () {
    C03_RE_IN_DATA_ADD();
});

$("#C03_RE_IN_TextBox1").keydown(function (e) {
    if (e.keyCode === 13) {
        $("#C03_RE_IN_NumericUpDown1").focus();
        e.preventDefault();
    }
});

$("#C03_RE_IN_NumericUpDown1").keydown(function (e) {
    if (e.keyCode === 13) {
        C03_RE_IN_DATA_ADD();
        e.preventDefault();
    }
});

$('#C03_RE_IN_Modal').on('modal:close', function () {
    $("#C03_RE_IN_TextBox1").val("");
    $("#C03_RE_IN_Label4").text("");
    $("#C03_RE_IN_Label5").text("");
    $("#C03_RE_IN_Label12").text("");
    $("#C03_RE_IN_Label37").text("0");
    $("#C03_RE_IN_NumericUpDown1").val(1);
});
$("#ButtonMiddle").click(function () {
    $('#C03_RE_IN_Modal').modal('open');
    setTimeout(function () {
        C03_RE_IN_Load();
        $("#C03_RE_IN_TextBox1").focus();
    }, 300);
});
$("#ButtonInputTrolley").click(C03_IN_TrolleyModal_Open);
$("#C03_IN_TrolleyConfirm").click(C03_IN_TrolleyConfirm_Click);

let trolleyTimer = null;
$("#C03_IN_TrolleyInput").on("input", function () {
    clearTimeout(trolleyTimer);
    trolleyTimer = setTimeout(() => C03_IN_TrolleyValidate($(this).val()), 500);
}).on("keypress", function (e) {
    if (e.which === 13) { clearTimeout(trolleyTimer); C03_IN_TrolleyValidate($(this).val()); }
});

$("#Button1").click(function () {
    $('#C03_LT_Modal').modal('open');
});

$("#ModalButtonFind").click(function () {
    getLongTermDetail();
});

$("#ButtonexportDetail").click(function () {
    exportLongtimeTableToExcel();
});

$("#ButtonRight1").click(function () {
    $('#C03_OUT_Modal').modal('open');
    setTimeout(function () {
        C03_OUT_Load();
        $("#TextBox4").focus();
    }, 300);
});

$("#ButtonRightLeft").click(function () {
    $('#C03_IN_Modal').modal('open');
    setTimeout(function () {
        C03_IN_Load();
        $("#C03_IN_TextBox1").focus();
    }, 300);
});
$('#TabControl1').on('click', 'a', function () {
    currentTab = $(this).attr('href');
});

$('#TabControl1').on('click', 'a', function () {
    let tabHref = $(this).attr('href');
    if (tabHref === "#TabPage4") {
        $("#RadioButton3").prop("checked", true);
    }
});

$("#Buttonfind").click(function () {
    Buttonfind_Click();
});

$("#TextBox2").keydown(function (e) {
    if (e.keyCode === 13) {
        Buttonfind_Click();
        e.preventDefault();
    }
});

$("#TextBox3").keydown(function (e) {
    if (e.keyCode === 13) {
        Buttonfind_Click();
        e.preventDefault();
    }
});
function DataGridView2Sort() {
    IsTableSort2 = !IsTableSort2;
    DataGridViewSort(BaseResult.DataGridView, IsTableSort2);
    DataGridView2Render();
}
function DataGridView1Sort() {
    IsTableSort1 = !IsTableSort1;
    DataGridViewSort(BaseResult.DataGridView, IsTableSort1);
    DataGridViewTab1Render();
}
function DataGridView3Sort() {
    IsTableSort3 = !IsTableSort3;
    DataGridViewSort(BaseResult.DataGridView3, IsTableSort3);
    DataGridView3Render();
}
function DataGridView4Sort() {
    IsTableSort4 = !IsTableSort4;

    if (BaseResult.DataGridView4 && BaseResult.DataGridView4.length > 1) {
        BaseResult.DataGridView4.sort(function (a, b) {
            let stageA = a.STAGE || '';
            let stageB = b.STAGE || '';

            let dateA = a.STAGE === 'OUTPUT' ? a.RACKOUT_DTM : a.inputdate;
            let dateB = b.STAGE === 'OUTPUT' ? b.RACKOUT_DTM : b.inputdate;

            dateA = dateA ? new Date(dateA).getTime() : 0;
            dateB = dateB ? new Date(dateB).getTime() : 0;

            let qtyA = parseFloat(a.QUANTITY || 0);
            let qtyB = parseFloat(b.QUANTITY || 0);

            if (IsTableSort4) {
                if (stageA !== stageB) {
                    return stageA.localeCompare(stageB);
                }

                if (dateA !== dateB) {
                    return dateA - dateB;
                }

                return qtyA - qtyB;
            } else {
                if (stageA !== stageB) {
                    return stageB.localeCompare(stageA);
                }

                if (dateA !== dateB) {
                    return dateB - dateA;
                }

                return qtyB - qtyA;
            }
        });
    }

    DataGridView4Render(BaseResult);
}
function validateBarcodeFormat(barcode) {
    if (!barcode || barcode.trim() === "") {
        return { isValid: false, message: "Mã vạch không được để trống\nBarcode is required" };
    }
    const dollarCount = (barcode.match(/\$/g) || []).length;
    if (dollarCount < 8) {
        return { isValid: false, message: "BARCODE SCAN sai\nScan sai Barcode." };
    }
    if (!barcode.includes("$$")) {
        return { isValid: false, message: "BARCODE SCAN sai\nScan sai Barcode." };
    }
    return { isValid: true, message: "" };
}

function formatDateForInput(date) {
    let month = '' + (date.getMonth() + 1);
    let day = '' + date.getDate();
    let year = date.getFullYear();

    if (month.length < 2) month = '0' + month;
    if (day.length < 2) day = '0' + day;

    return [year, month, day].join('-');
}

function ShowError(message) {
    M.toast({ html: message, classes: 'red', displayLength: 6000 });
}

function PlaySuccessSound() {
    try {
        let audio = new Audio('/audio/HOOK_S.mp3');
        audio.play().catch(e => console.log('Cannot play success sound:', e));
    } catch (e) {
        console.log('Success sound not available');
    }
}

function PlayErrorSound() {
    try {
        let audio = new Audio('/audio/Sash_brk.mp3');
        audio.play().catch(e => console.log('Cannot play error sound:', e));
    } catch (e) {
        console.log('Error sound not available');
    }
}

function getCurrentUser() {
    // Lấy USER_ID từ cookie
    return GetCookieValue('UserID') || 'SYSTEM';
}

function getGroupFromBarcode(barcode) {
    if (barcode && barcode.includes("$$")) {
        return barcode.substring(0, barcode.indexOf("$$"));
    }
    return "";
}

function checkBarcodeGroup(barcode) {
    if (!currentGroup) {
        return true;
    }
    const group = getGroupFromBarcode(barcode);
    return group === currentGroup;
}

function checkBarcodeGroupOut(barcode) {
    if (!currentGroupOut) {
        return true;
    }
    const group = getGroupFromBarcode(barcode);
    return group === currentGroupOut;
}

function checkPartExists(partId, fieldId) {
    if (!partId) return;

    let BaseParameter = {
        ListSearchString: [partId]
    };

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));

    fetch("/C03/CheckPartExists", {
        method: "POST",
        body: formUpload
    })
        .then(response => {
            if (!response.ok) {
                throw new Error(`Server responded with status: ${response.status}`);
            }
            return response.json();
        })
        .then(data => {
            if (data.Code !== "SUCCESS") {
                M.toast({ html: data.Error, classes: 'red', displayLength: 6000 });
                $("#" + fieldId).val("");
                M.updateTextFields();
            }
        })
        .catch(err => {
            M.toast({ html: "Lỗi kết nối: " + err.message, classes: 'red', displayLength: 6000 });
        });
}

function Buttonfind_Click() {
    let activeTab = $('#TabControl1 .active').attr('href') || currentTab;
    let tabName = activeTab.replace("#", "");

    $("#BackGround").css("display", "block");

    let BaseParameter = {
        ListSearchString: [tabName]
    };

    if (tabName === "TabPage3") {
        BaseParameter.ListSearchString.push($("#TextBox2").val() || "");
    }
    else if (tabName === "TabPage4") {
        BaseParameter.ListSearchString.push($("#TextBox3").val() || "");
        let startDate = $("#DateTimePicker1").val();
        let endDate = $("#DateTimePicker2").val();
        if (!startDate) {
            let yesterday = new Date();
            yesterday.setDate(yesterday.getDate() - 1);
            startDate = formatDateForInput(yesterday);
        }
        if (!endDate) {
            let today = new Date();
            endDate = formatDateForInput(today);
        }
        BaseParameter.ListSearchString.push(startDate);
        BaseParameter.ListSearchString.push(endDate);
        let selectedRadio = "";
        if ($("#RadioButton1").prop("checked")) selectedRadio = "RadioButton1";
        else if ($("#RadioButton2").prop("checked")) selectedRadio = "RadioButton2";
        else if ($("#RadioButton3").prop("checked")) selectedRadio = "RadioButton3";
        else if ($("#RadioButton8").prop("checked")) selectedRadio = "RadioButton8";
        BaseParameter.ListSearchString.push(selectedRadio);
    }
    else if (tabName === "TabPage5") {
        $("#Tab5_leadNo").val("");
        $("#Tab5_index").val("");
        $("#Tab5_hookRack").val("");
        $("#Tab5_safetyStock").val("");
        $("#Tab5_wirePNo").val("");
        $("#Tab5_wireName").val("");
        $("#Tab5_diameter").val("");
        $("#Tab5_wLink").val("");
        $("#Tab5_color").val("");
        $("#Tab5_wrNo").val("");
        $("#Tab5_length").val("");
        $("#Tab5_term1").val("");
        $("#Tab5_seal1").val("");
        $("#Tab5_strip1").val("");
        $("#Tab5_t1No").val("");
        $("#Tab5_cchW1").val("");
        $("#Tab5_ichW1").val("");
        $("#Tab5_term2").val("");
        $("#Tab5_seal2").val("");
        $("#Tab5_strip2").val("");
        $("#Tab5_t2No").val("");
        $("#Tab5_cchW2").val("");
        $("#Tab5_ichW2").val("");
        $("#Tab5_bundleQty").val("");

        $("#Tab5_leadNo").prop("readonly", true);
        $("#Tab5_SubPartPanel").prop("enabled", false);
        BaseParameter.ListSearchString.push($("#Tab5_LeadNoFilter").val() || "");
    }

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));

    let url = "/C03/Buttonfind_Click";

    fetch(url, {
        method: "POST",
        body: formUpload
    })
        .then(response => {
            if (!response.ok) {
                throw new Error(`Server responded with status: ${response.status}`);
            }
            return response.json();
        })
        .then(data => {
            BaseResult = data;

            if (tabName === "TabPage1") {
                DataGridView2Render();
            } else if (tabName === "TabPage2") {
                DataGridViewTab1Render();
            } else if (tabName === "TabPage3") {
                DataGridView3Render();
            } else if (tabName === "TabPage4") {
                DataGridView5Render();
            } else if (tabName === "TabPage5") {
                DataGridView6Render();
            } else if (tabName === "TabPage7") {
                DataGridView8Render();
            }
            $("#BackGround").css("display", "none");
        })
        .catch(err => {
            ShowError("Lỗi kết nối: " + err.message);
            $("#BackGround").css("display", "none");
        });
}

function DataGridView2Render() {
    let tbody = $("#DataGridView2");
    tbody.empty();

    if (!BaseResult.DataGridView || BaseResult.DataGridView.length === 0) {
        tbody.append('<tr><td colspan="9" class="center-align">Không có dữ liệu</td></tr>');
        return;
    }

    for (let i = 0; i < BaseResult.DataGridView.length; i++) {
        let item = BaseResult.DataGridView[i];
        let row = `<tr>
            <td>${item.TABLE_NM || ''}</td>
            <td>${item.Location || ''}</td>
            <td>${item.LEAD_NO || ''}</td>
            <td>${item.STOCK_QTY || '0'}</td>
            <td>${item.Barcode || ''}</td>
            <td>${item.inputdate || ''}</td>
            <td>${item.QUANTITY || '0'}</td>
            <td>${item.CREATE_DTM || ''}</td>
            <td>${item.CREATE_USER || ''}</td>
        </tr>`;

        tbody.append(row);
    }
}

function DataGridViewTab1Render() {
    let tbody = $("#DataGridView1");
    tbody.empty();

    if (!BaseResult.DataGridView || BaseResult.DataGridView.length === 0) {
        tbody.append('<tr><td colspan="9" class="center-align">Không có dữ liệu</td></tr>');
        return;
    }

    for (let i = 0; i < BaseResult.DataGridView.length; i++) {
        let item = BaseResult.DataGridView[i];
        let row = `<tr>
            <td>${item.TABLE_NM || ''}</td>
            <td>${item.Location || ''}</td>
            <td>${item.LEAD_NO || ''}</td>
            <td>${item.STOCK_QTY || '0'}</td>
            <td>${item.Barcode || ''}</td>
            <td>${item.inputdate || ''}</td>
            <td>${item.QUANTITY || '0'}</td>
            <td>${item.CREATE_DTM || ''}</td>
            <td>${item.CREATE_USER || ''}</td>
        </tr>`;

        tbody.append(row);
    }
}

function DataGridView3Render() {
    let tbody = $("#DataGridView3");
    tbody.empty();

    if (!BaseResult.DataGridView3 || BaseResult.DataGridView3.length === 0) {
        tbody.append('<tr><td colspan="7" class="center-align">Không có dữ liệu</td></tr>');
        return;
    }

    for (let i = 0; i < BaseResult.DataGridView3.length; i++) {
        let item = BaseResult.DataGridView3[i];
        let isBad = item.STAGE === 'Bad';

        let row = $("<tr>");

        if (isBad) {
            row.addClass("red-text");
        }

        //row.append(`<td class="center-align">${item.PO_CODE || ''}</td>`);
        //row.append(`<td class="center-align">${item.FG_PART_NO || ''}</td>`);
        //row.append(`<td class="center-align">${item.ECNNo || ''}</td>`);
        row.append(`<td class="center-align">${item.HOOK_RACK || ''}</td>`);
        row.append(`<td class="center-align">${item.LEAD_NM || ''}</td>`);
        row.append(`<td class="right-align">${formatNumber(item.STOCK_QTY)}</td>`);
        row.append(`<td class="right-align">${formatNumber(item.IN_QTY)}</td>`);
        row.append(`<td class="right-align">${formatNumber(item.OUT_QTY)}</td>`);
        row.append(`<td class="center-align">${item.STAGE || ''}</td>`);
        row.append(`<td class="right-align">${formatNumber(item.CHK_QTY)}</td>`);

        row.click(function () {
            DGV_CellSEL(item.LEAD_NM);
        });

        tbody.append(row);
    }

    function formatNumber(value) {
        if (!value) return '0';
        return parseFloat(value).toLocaleString('en-US');
    }
}

function DGV_CellSEL(leadNm) {
    $("#BackGround").css("display", "block");

    let BaseParameter = {
        ListSearchString: [leadNm]
    };

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));

    let url = "/C03/DGV_CellSEL";

    fetch(url, {
        method: "POST",
        body: formUpload
    })
        .then(response => {
            if (!response.ok) {
                throw new Error(`Server responded with status: ${response.status}`);
            }
            return response.json();
        })
        .then(data => {
            BaseResult.DataGridView4 = data.DataGridView4 || [];
            IsTableSort4 = false;
            DataGridView4Render(data);
            $("#BackGround").css("display", "none");
        })
        .catch(err => {
            ShowError("Lỗi kết nối: " + err.message);
            $("#BackGround").css("display", "none");
        });
}

function DataGridView4Render(data) {
    let tbody = $("#DataGridView4");
    tbody.empty();

    if (!data.DataGridView4 || data.DataGridView4.length === 0) {
        tbody.append('<tr><td colspan="8" class="center-align">Không có dữ liệu chi tiết</td></tr>');
        return;
    }

    for (let i = 0; i < data.DataGridView4.length; i++) {
        let item = data.DataGridView4[i];
        let row = `<tr>
            <td class="center-align">${item.PO_CODE || ''}</td>
            <td class="center-align">${item.FG_PART_NO || ''}</td>
            <td class="center-align">${item.ECNNo || ''}</td>
            <td class="center-align">${item.Location || ''}</td>
            <td class="center-align">${item.LEAD_NO || ''}</td>
            <td class="center-align">${item.Barcode || ''}</td>
            <td class="center-align">${item.MC_NO || ''}</td>
            <td class="right-align">${item.SEQ || ''}</td>
            <td class="right-align">${formatNumber(item.QUANTITY)}</td>
            <td class="center-align">${item.STAGE || ''}</td>
            <td class="center-align">${formatDate(item.inputdate)}</td>
            <td class="center-align">${formatDate(item.RACKOUT_DTM)}</td>
        </tr>`;

        tbody.append(row);
    }

    function formatNumber(value) {
        if (!value) return '0';
        return parseFloat(value).toLocaleString('en-US');
    }

    function formatDate(dateStr) {
        if (!dateStr) return '';
        try {
            const date = new Date(dateStr);
            if (isNaN(date.getTime())) return dateStr;
            return date.toLocaleDateString() + ' ' + date.toLocaleTimeString();
        } catch (e) {
            return dateStr;
        }
    }
}

function DataGridView5Render() {
    let displayType = BaseResult.DATEString || "";

    $("#Label1").text("IN : " + formatNumber(BaseResult.SUM_QTY || 0) + " EA");
    $("#Label2").text("OUT : " + formatNumber(BaseResult.OUT_QTY || 0) + " EA");

    $("#listContainer").css("display", "none");
    $("#leadContainer").css("display", "none");
    $("#inoutContainer").css("display", "none");

    if (!BaseResult.DataGridView5 || BaseResult.DataGridView5.length === 0) {
        switch (displayType) {
            case "LIST":
                $("#listContainer").css("display", "block");
                $("#DataGridView5List").html('<tr><td colspan="8" class="center-align">Không có dữ liệu</td></tr>');
                break;
            case "LEAD":
                $("#leadContainer").css("display", "block");
                $("#DataGridView5Lead").html('<tr><td colspan="6" class="center-align">Không có dữ liệu</td></tr>');
                break;
            case "INOUT":
                $("#inoutContainer").css("display", "block");
                $("#DataGridView5InOut").html('<tr><td colspan="9" class="center-align">Không có dữ liệu</td></tr>');
                break;
        }
        return;
    }

    switch (displayType) {
        case "LIST":
            renderListType();
            $("#listContainer").css("display", "block");
            break;

        case "LEAD":
            renderLeadType();
            $("#leadContainer").css("display", "block");
            break;

        case "INOUT":
            renderInOutType();
            $("#inoutContainer").css("display", "block");
            break;
    }

    function renderListType() {
        let tbody = $("#DataGridView5List");
        tbody.empty();

        for (let i = 0; i < BaseResult.DataGridView5.length; i++) {
            let item = BaseResult.DataGridView5[i];
            let row = `<tr>
                <td class="center-align">${item.Location || ''}</td>
                <td class="center-align">${item.LEAD_NO || ''}</td>
                <td class="center-align">${item.Barcode || ''}</td>
                <td class="right-align">${item.SEQ || ''}</td>
                <td class="right-align">${formatNumber(item.QUANTITY)}</td>
                <td class="center-align">${item.STAGE || ''}</td>
                <td class="center-align">${formatDate(item.inputdate)}</td>
                <td class="center-align">${formatDate(item.RACKOUT_DTM)}</td>
            </tr>`;

            tbody.append(row);
        }
    }

    function renderLeadType() {
        let tbody = $("#DataGridView5Lead");
        tbody.empty();

        for (let i = 0; i < BaseResult.DataGridView5.length; i++) {
            let item = BaseResult.DataGridView5[i];
            let row = `<tr>
                <td class="center-align">${item.LEAD_NO || ''}</td>
                <td class="center-align">${item.STAGE || ''}</td>
                <td class="center-align">${formatDate(item.inputdate)}</td>
                <td class="right-align">${formatNumber(item.IN_QTY)}</td>
                <td class="center-align">${formatDate(item.RACKOUT_DTM)}</td>
                <td class="right-align">${formatNumber(item.OUT_QTY)}</td>
            </tr>`;

            tbody.append(row);
        }
    }

    function renderInOutType() {
        let tbody = $("#DataGridView5InOut");
        tbody.empty();

        for (let i = 0; i < BaseResult.DataGridView5.length; i++) {
            let item = BaseResult.DataGridView5[i];
            let row = `<tr>
                <td class="center-align">${item.Location || ''}</td>
                <td class="center-align">${item.LEAD_NO || ''}</td>
                <td class="center-align">${item.STAGE || ''}</td>
                <td class="left-align">${item.Barcode || ''}</td>
                <td class="right-align">${formatNumber(item.QUANTITY)}</td>
                <td class="center-align">${formatDate(item.inputdate)}</td>
                <td class="right-align">${formatNumber(item.IN_QTY)}</td>
                <td class="center-align">${formatDate(item.RACKOUT_DTM)}</td>
                <td class="right-align">${formatNumber(item.OUT_QTY)}</td>
            </tr>`;

            tbody.append(row);
        }
    }

    function formatNumber(value) {
        if (!value) return '0';
        return parseFloat(value).toLocaleString('en-US');
    }

    function formatDate(dateStr) {
        if (!dateStr) return '';
        try {
            const date = new Date(dateStr);
            if (isNaN(date.getTime())) return dateStr;
            return date.toLocaleDateString() + ' ' + date.toLocaleTimeString();
        } catch (e) {
            return dateStr;
        }
    }
}
function DataGridView6Render() {
    let tbody = $("#Tab5_LeadListBody");
    tbody.empty();

    if (!BaseResult.DataGridView6 || BaseResult.DataGridView6.length === 0) {
        tbody.append('<tr><td class="center-align">Không có dữ liệu</td></tr>');
        return;
    }

    for (let i = 0; i < BaseResult.DataGridView6.length; i++) {
        let item = BaseResult.DataGridView6[i];
        let row = `<tr class="lead-item">
            <td class="center-align">${item.LEAD_PN}</td>
        </tr>`;
        tbody.append(row);
    }

    $(".lead-item").click(function () {
        $(".lead-item").removeClass("selected-row");
        $(this).addClass("selected-row");
        let leadPN = $(this).find("td:first").text();
        loadLeadDetail(leadPN);
    });
}

function loadLeadDetail(leadPN) {
    $("#BackGround").css("display", "block");

    let BaseParameter = {
        ListSearchString: [leadPN]
    };

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));

    fetch("/C03/DGV_LOAD", {
        method: "POST",
        body: formUpload
    })
        .then(response => {
            if (!response.ok) {
                throw new Error(`Server responded with status: ${response.status}`);
            }
            return response.json();
        })
        .then(data => {
            $("#BackGround").css("display", "none");



            if (data.Error) {
                M.toast({ html: data.Error, classes: 'red', displayLength: 6000 });
                return;
            }

            displayLeadDetail(data.LeadDetail);
            renderSubPartList(data.DataGridView7);
        })
        .catch(err => {
            $("#BackGround").css("display", "none");
            M.toast({ html: "Lỗi kết nối: " + err.message, classes: 'red', displayLength: 6000 });
        });
}

function displayLeadDetail(detail) {
    if (!detail) return;

    $("#Tab5_leadNo").val(detail.LEAD_PN || "");
    $("#Tab5_index").val(detail.LEAD_INDEX || "");
    $("#Tab5_hookRack").val(detail.HOOK_RACK || "");
    $("#Tab5_safetyStock").val(detail.Safety_Stock || "0");
    $("#Tab5_wirePNo").val(detail.WIRE || "");
    $("#Tab5_term1").val(detail.TERM1 || "");
    $("#Tab5_seal1").val(detail.SEAL1 || "");
    $("#Tab5_strip1").val(detail.STRIP1 || "0");
    $("#Tab5_cchW1").val(detail.CCH_W1 || "");
    $("#Tab5_ichW1").val(detail.ICH_W1 || "");
    $("#Tab5_term2").val(detail.TERM2 || "");
    $("#Tab5_seal2").val(detail.SEAL2 || "");
    $("#Tab5_strip2").val(detail.STRIP2 || "0");
    $("#Tab5_cchW2").val(detail.CCH_W2 || "");
    $("#Tab5_ichW2").val(detail.ICH_W2 || "");
    $("#Tab5_wLink").val(detail.W_LINK || "");
    $("#Tab5_wrNo").val(detail.WR_NO || "");
    $("#Tab5_wireName").val(detail.WIRE_NM || "");
    $("#Tab5_diameter").val(detail.W_Diameter || "");
    $("#Tab5_color").val(detail.W_Color || "");
    $("#Tab5_length").val(detail.W_Length || "0");
    $("#Tab5_t1No").val(detail.T1NO || "");
    $("#Tab5_t2No").val(detail.T2NO || "");
    $("#Tab5_bundleQty").val(detail.BUNDLE_QTY || "0");

    if (detail.DSCN_YN === "Y") {
        $("#Tab5_dsYRadio").prop("checked", true);
    } else {
        $("#Tab5_dsNRadio").prop("checked", true);
    }

    if (detail.LEAD_SCN === "LEAD") {
        $("#Tab5_leadRadio").prop("checked", true);
        $("#Tab5_SubPartPanel").prop("enabled", false);
    } else {
        $("#Tab5_spstRadio").prop("checked", true);
        $("#Tab5_SubPartPanel").prop("enabled", true);
    }

    M.updateTextFields();
}

function renderSubPartList(subParts) {
    let tbody = $("#Tab5_SubPartBody");
    tbody.empty();

    if (!subParts || subParts.length === 0) {
        tbody.append('<tr><td colspan="3" class="center-align">Không có dữ liệu</td></tr>');
        return;
    }

    for (let i = 0; i < subParts.length; i++) {
        let item = subParts[i];
        let row = `<tr>
            <td class="center-align">${item.NO}</td>
            <td class="center-align">${item.LEAD_NO}</td>
            <td class="center-align">${item.S_LR}</td>
        </tr>`;
        tbody.append(row);
    }
}

function DataGridView8Render() {
    let tbody = $("#DataGridView8");
    tbody.empty();

    if (!BaseResult.DataGridView8 || BaseResult.DataGridView8.length === 0) {
        tbody.append('<tr><td colspan="12" class="center-align">Không có dữ liệu</td></tr>');
        return;
    }

    for (let i = 0; i < BaseResult.DataGridView8.length; i++) {
        let item = BaseResult.DataGridView8[i];
        let row = $("<tr>");

        row.append(`<td class="center-align">${item.YEAR || ''}</td>`);
        row.append(`<td class="center-align">${item.MONTH || ''}</td>`);
        row.append(`<td class="right-align">${formatNumber(item.LEAD_COUNT)}</td>`);
        row.append(`<td class="right-align">${formatNumber(item['ADD_1'])}</td>`);
        row.append(`<td class="right-align">${formatNumber(item['ADD_2'])}</td>`);
        row.append(`<td class="right-align">${formatNumber(item['ADD_3'])}</td>`);
        row.append(`<td class="right-align">${formatNumber(item['ADD_4'])}</td>`);
        row.append(`<td class="right-align">${formatNumber(item['ADD_5'])}</td>`);
        row.append(`<td class="right-align">${formatNumber(item['ADD_6'])}</td>`);
        row.append(`<td class="right-align">${formatNumber(item['OVER_9'])}</td>`);
        row.append(`<td class="right-align">${formatNumber(item['OVER_10'])}</td>`);
        row.append(`<td class="right-align">${formatNumber(item.SUM)}</td>`);

        tbody.append(row);
    }

    function formatNumber(value) {
        if (!value) return '0';
        return parseFloat(value).toLocaleString('en-US');
    }
}

function exportToExcel() {
    if (!$("#ModalLongTermDetailBody tr").length || $("#ModalLongTermDetailBody tr td").length <= 1) {
        ShowError("Không có dữ liệu để xuất");
        return;
    }

    let wb = XLSX.utils.book_new();
    let table = document.querySelector("table");
    let ws = XLSX.utils.table_to_sheet(table);
    XLSX.utils.book_append_sheet(wb, ws, "Long Term Details");
    XLSX.writeFile(wb, "LongTermDetails_" + new Date().toISOString().slice(0, 10) + ".xlsx");
}

function exportLongtimeTableToExcel() {
    // Kiểm tra dữ liệu
    if ($("#ModalLongTermDetailBody tr td[colspan='9']").length > 0) {
        ShowError("Không có dữ liệu để xuất");
        return;
    }

    try {
        // Lấy table bằng jQuery
        let $table = $("#LongtimeTable");

        // Tạo workbook
        let wb = XLSX.utils.book_new();

        // Chuyển table thành worksheet - CÁCH NÀY GIỮ NGUYÊN TIÊU ĐỀ
        let ws = XLSX.utils.table_to_sheet($table[0]);

        // Điều chỉnh độ rộng cột (tùy chọn)
        ws['!cols'] = [
            { wch: 15 }, // LEAD_NM
            { wch: 20 }, // TABLE_NM
            { wch: 15 }, // LOCATION
            { wch: 20 }, // BARCODE_NM
            { wch: 10 }, // QTY
            { wch: 15 }, // LAST_INSPECT
            { wch: 8 },  // YEAR
            { wch: 8 },  // MONTH
            { wch: 12 }  // MONTHA
        ];

        // Thêm sheet và xuất file
        XLSX.utils.book_append_sheet(wb, ws, "Long Term Details");
        XLSX.writeFile(wb, `LongTermDetails_${new Date().toISOString().slice(0, 10)}.xlsx`);

    } catch (error) {
        ShowError("Lỗi xuất Excel: " + error.message);
    }
}

function getLongTermDetail() {
    $("#BackGround").css("display", "block");

    let BaseParameter = {
        ListSearchString: [
            $("#ComboBox1").val() || "ALL",
            $("#TextBox1").val() || "",
            $("#TextBox2").val() || ""
        ]
    };

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));

    let url = "/C03/GET_LONG_TERM_DETAIL";

    fetch(url, {
        method: "POST",
        body: formUpload
    })
        .then(response => {
            if (!response.ok) {
                throw new Error(`Server responded with status: ${response.status}`);
            }
            return response.json();
        })
        .then(data => {
            renderModalDetail(data);
            $("#BackGround").css("display", "none");
        })
        .catch(err => {
            ShowError("Lỗi kết nối: " + err.message);
            $("#BackGround").css("display", "none");
        });
}

function renderModalDetail(data) {
    let tbody = $("#ModalLongTermDetailBody");
    tbody.empty();

    if (!data.DataGridView9 || data.DataGridView9.length === 0) {
        tbody.append('<tr><td colspan="9" class="center-align">Không có dữ liệu</td></tr>');
        return;
    }

    // Cache các function format
    function formatNumber(value) {
        if (!value) return '0';
        return parseFloat(value).toLocaleString('en-US');
    }

    function formatDate(dateStr) {
        if (!dateStr) return '';
        try {
            const date = new Date(dateStr);
            if (isNaN(date.getTime())) return dateStr;
            return date.toLocaleDateString() + ' ' + date.toLocaleTimeString();
        } catch (e) {
            return dateStr;
        }
    }

    // Sử dụng DocumentFragment hoặc string concatenation
    let html = '';
    const items = data.DataGridView9;

    for (let i = 0; i < items.length; i++) {
        let item = items[i];
        html += `<tr>
            <td class="center-align">${item.LEAD_NO || ''}</td>
            <td class="center-align">${item.TABLE_NM || ''}</td>
            <td class="center-align">${item.COLN_LOC || ''}</td>
            <td class="center-align">${item.BARCODE_NM || ''}</td>
            <td class="right-align">${formatNumber(item.QTY)}</td>
            <td class="center-align">${formatDate(item.Coln1)}</td>
            <td class="center-align">${item.YEAR || ''}</td>
            <td class="center-align">${item.MONTH || ''}</td>
            <td class="center-align">${item.Coln7 || ''}</td>
        </tr>`;
    }

    // Chỉ append DOM một lần duy nhất
    tbody.append(html);
}

function C03_IN_Load() {
    $("#C03_IN_Label4").text("");
    $("#C03_IN_Label5").text("");
    $("#C03_IN_Label12").text("");
    $("#C03_IN_Label37").text("0");
    $("#C03_IN_TextBox1").val("");
    LEAD_CONT_IN = 0;
    TempDataIn = [];
    currentGroup = "";
    barcodeQueue = [];
    isProcessingQueue = false;
    DataGridViewInRender();
    updateQueueCount();
    $("#C03_IN_TextBox1").focus();
}
function updateQueueCount() {
    $("#queueCount").text(barcodeQueue.length);
}
function C03_OUT_Load() {
    $("#Label15").text("");
    $("#Label11").text("");
    $("#Label38").text("0");
    $("#TextBox4").val("");
    LEAD_CONT_OUT = 0;
    currentGroupOut = "";
    DataGridView1Render();
    $("#TextBox4").focus();
}
function DataGridViewInRender() {
    let tbody = $("#C03_IN_TempTableBody");
    tbody.empty();

    if (TempDataIn.length === 0) {
        tbody.append('<tr><td colspan="4" class="center-align">Chưa có dữ liệu quét</td></tr>');
        return;
    }

    let displayData = TempDataIn.slice().reverse();

    displayData.forEach(function (item) {
        let row = `<tr>
            <td>${item.leadNo}</td>
            <td>${item.barcode}</td>
            <td class="center-align">${item.hookRack}</td>
            <td class="center-align">${item.quantity}</td>
        </tr>`;
        tbody.append(row);
    });
}


function Buttonfind_Click_IN() {
    $("#BackGround").css("display", "block");
    let BaseParameter = {};
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C03/Buttonfind_Click_IN";

    fetch(url, {
        method: "POST",
        body: formUpload
    }).then((response) => {
        response.json().then(() => {
            $("#BackGround").css("display", "none");
        }).catch(() => {
            $("#BackGround").css("display", "none");
        })
    });
}

function ProcessResultIn(data, originalBarcode) {
    if (data.Error) {
        if (data.Code === "CONFIRM_REINPUT") {
            ShowError(data.Error);
            try { new Audio('/audio/warning.mp3').play().catch(() => { }); } catch { }
            $("#C03_IN_ConfirmMessage").text(data.Error);
            $('#C03_IN_ConfirmModal').modal('open');
            $("#C03_IN_ConfirmYes").off('click').on('click', function () {
                ReInputBarcode(originalBarcode);
                $('#C03_IN_ConfirmModal').modal('close');
            });
            $("#C03_IN_ConfirmNo").off('click').on('click', function () {
                $("#C03_IN_TextBox1").val("").focus();
            });
        } else {
            ShowError(data.Error);
            $("#C03_IN_Label4, #C03_IN_Label5, #C03_IN_Label12").text("ERROR");
            $("#C03_IN_TextBox1").val("");
            if (data.Code === "SOUND_ERROR") PlayErrorSound();
        }
        return;
    }

    if (data.Code === "SOUND_SUCCESS" && !currentGroup && !NoGroupLock) {
        currentGroup = getGroupFromBarcode(originalBarcode);
    }

    $("#C03_IN_Label4").text(data.Label4 || currentGroup);
    $("#C03_IN_Label5").text(data.Label5 || "");
    $("#C03_IN_Label12").text(data.Label12 || "");
    $("#C03_IN_TextBox1").val("");

    if (data.Code === "SOUND_SUCCESS") {
        PlaySuccessSound();
        AddToTempTableIn({
            leadNo: data.Label4 || currentGroup,
            barcode: originalBarcode,
            hookRack: data.Label5 || "",
            quantity: getQuantityFromBarcode(originalBarcode)
        });
        $("#C03_IN_Label37").text(TempDataIn.length.toString());
        M.toast({ html: data.Message || 'Nhập kho thành công!', classes: 'green' });
    }
}


function getQuantityFromBarcode(barcode) {
    if (!barcode || !barcode.includes("$$")) {
        return "1";
    }

    try {
        const firstDoubleDollar = barcode.indexOf("$$");
        const secondDoubleDollar = barcode.indexOf("$$", firstDoubleDollar + 2);

        if (firstDoubleDollar >= 0 && secondDoubleDollar >= 0) {
            const quantityStr = barcode.substring(firstDoubleDollar + 2, secondDoubleDollar);

            if (!isNaN(quantityStr) && quantityStr.trim() !== "") {
                return quantityStr;
            }
        }

        return "1";
    } catch (e) {
        return "1";
    }
}
function AddToTempTableIn(item) {
    const quantity = item.quantity || getQuantityFromBarcode(item.barcode);
    TempDataIn.push({
        leadNo: item.leadNo,
        barcode: item.barcode,
        hookRack: item.hookRack,
        quantity: quantity,
        timestamp: new Date().toLocaleString()
    });
    DataGridViewInRender();
}


function ReInputBarcode(barcode) {
    $("#BackGround").css("display", "block");

    let BaseParameter = {
        ListSearchString: [barcode, getCurrentUser()]
    };

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C03/ReInputBarcode";

    fetch(url, { method: "POST", body: formUpload })
        .then(response => response.json())
        .then(data => {
            $("#BackGround").css("display", "none");

            if (data.Error) {
                ShowError(data.Error);
                if (data.Code === "SOUND_ERROR") PlayErrorSound();
                $("#C03_IN_TextBox1").val("").focus();
                return;
            }

            let leadNo = "";
            if (barcode.includes("$$")) {
                leadNo = barcode.substring(0, barcode.indexOf("$$"));
            }

            $("#C03_IN_Label4").text(leadNo);
            $("#C03_IN_Label5").text(data.Label5 || "");
            $("#C03_IN_Label12").text(data.Label12 || "");

            if (!currentGroup) currentGroup = leadNo;

            if (data.Code === "SOUND_SUCCESS") {
                PlaySuccessSound();
                AddToTempTableIn({
                    leadNo: leadNo,
                    barcode: barcode,
                    hookRack: data.Label5 || "",
                    quantity: "1"
                });
                LEAD_CONT_IN = data.Label37 ? parseInt(data.Label37) : TempDataIn.length;
                $("#C03_IN_Label37").text(TempDataIn.length.toString());
                M.toast({ html: data.Message || 'Tái nhập kho thành công!', classes: 'green' });
                $("#C03_IN_TextBox1").val("").focus();
            }
        })
        .catch(err => {
            $("#BackGround").css("display", "none");
            ShowError("Lỗi kết nối server: " + err.message);
            PlayErrorSound();
        });
}

function Buttonclose_Click_IN() {
    $('#C03_IN_Modal').modal('close');
}

function DATA_ADD_IN() {

    CategoryDepartmentID = $("#CategoryDepartmentINPUT").val();
    if (CategoryDepartmentID == 0) {
        $("#C03_IN_TextBox1").val("");
        ShowError("Chọn bộ phận / Select department");
        PlayErrorSound();
    }
    if (CategoryDepartmentID > 0) {
        let textBox1Text = $("#C03_IN_TextBox1").val().trim();
        if (!textBox1Text) return;

        const validation = validateBarcodeFormat(textBox1Text);
        if (!validation.isValid) {
            ShowError(validation.message);
            PlayErrorSound();
            $("#C03_IN_TextBox1").val("").focus();
            return;
        }

        if (!NoGroupLock && currentGroup && !checkBarcodeGroup(textBox1Text)) {
            ShowError(`Mã vạch khác nhóm hiện tại (${currentGroup})\nBarcode from different group (${currentGroup})`);
            PlayErrorSound();
            $("#C03_IN_TextBox1").val("").focus();
            return;
        }

        barcodeQueue.push(textBox1Text);
        $("#C03_IN_TextBox1").val("").focus();
        updateQueueCount();

        if (!isProcessingQueue) {
            processNextInQueue();
        }
    }
}

function processNextInQueue() {
    if (barcodeQueue.length === 0) {
        isProcessingQueue = false;
        return;
    }

    isProcessingQueue = true;
    const barcode = barcodeQueue[0];

    $("#processingStatus").text(`Đang xử lý: ${barcode}`);
    $("#BackGround").css("display", "block");

    let BaseParameter = {
        ListSearchString: [barcode, getCurrentUser()],
        CompanyID: CompanyID,
        CategoryDepartmentID: CategoryDepartmentID
    };
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));

    fetch("/C03/DATA_ADD_IN", { method: "POST", body: formUpload })
        .then(r => r.json())
        .then(data => {
            ProcessResultIn(data, barcode);
            barcodeQueue.shift();
            updateQueueCount();
            $("#BackGround").css("display", "none");
            processNextInQueue();
        })
        .catch(err => {
            ShowError("Lỗi kết nối server: " + err.message);
            PlayErrorSound();
            barcodeQueue.shift();
            updateQueueCount();
            $("#BackGround").css("display", "none");
            processNextInQueue();
        });
}
function C03_IN_TrolleyModal_Open() {
    C03_IN_TrolleyData = [];
    $("#C03_IN_TrolleyInput").val("");
    $("#C03_IN_TrolleyMsg").html("");
    $("#C03_IN_TrolleySummary, #C03_IN_TrolleyProgress").hide();
    $("#C03_IN_TrolleyBody").empty();
    $("#C03_IN_TrolleyConfirm").prop("disabled", true);

    $("#C03_IN_TrolleyModal").modal({ dismissible: false, onOpenEnd: () => $("#C03_IN_TrolleyInput").focus() });
    $("#C03_IN_TrolleyModal").modal("open");
}

function C03_IN_TrolleyValidate(trolleyCode) {
    if (!trolleyCode?.trim()) {
        $("#C03_IN_TrolleyMsg").html("");
        $("#C03_IN_TrolleySummary").hide();
        $("#C03_IN_TrolleyConfirm").prop("disabled", true);
        return;
    }

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify({ ListSearchString: [trolleyCode.trim()] }));

    fetch("/C03/GetBarcodesByTrolley", { method: "POST", body: formUpload })
        .then(r => r.json())
        .then(data => {
            if (data.Success && data.TrolleyBarcodes?.length > 0) {
                C03_IN_TrolleyData = data.TrolleyBarcodes;
                C03_IN_TrolleyRenderPreview(data.TrolleyBarcodes, data.TrolleySummary);
                $("#C03_IN_TrolleyMsg").html(`<span class="green-text">✓ ${trolleyCode.trim()}</span>`);
                $("#C03_IN_TrolleyConfirm").prop("disabled", false);
            } else {
                C03_IN_TrolleyData = [];
                $("#C03_IN_TrolleySummary").hide();
                $("#C03_IN_TrolleyMsg").html(`<span class="red-text">✗ ${data.Error || "Không có barcode"}</span>`);
                $("#C03_IN_TrolleyConfirm").prop("disabled", true);
            }
        })
        .catch(() => {
            $("#C03_IN_TrolleyMsg").html('<span class="red-text">✗ Lỗi kết nối</span>');
            $("#C03_IN_TrolleyConfirm").prop("disabled", true);
        });
}

function C03_IN_TrolleyRenderPreview(barcodeData, summaryData) {
    let tbody = $("#C03_IN_TrolleyBody").empty();
    let totalBarcode = 0, totalQty = 0;

    barcodeData.forEach((item, index) => {
        tbody.append(`
            <tr>
                <td>${index + 1}</td>
                <td><b>${item.BARCODE}</b></td>
                <td>${item.LEAD_NO}</td>
                <td class="center-align">${item.HOOK_RACK || "-"}</td>
                <td class="center-align">${item.QTY}</td>
            </tr>
        `);
        totalBarcode++;
        totalQty += item.QTY;
    });

    let uniqueLeads = [...new Set(barcodeData.map(x => x.LEAD_NO))].length;

    $("#C03_IN_TotalBarcode").text(totalBarcode);
    $("#C03_IN_TotalLead").text(uniqueLeads);
    $("#C03_IN_TotalQty").text(totalQty);
    $("#C03_IN_TrolleySummary").show();

    let leadSummary = {};
    barcodeData.forEach(item => {
        if (!leadSummary[item.LEAD_NO]) {
            leadSummary[item.LEAD_NO] = 0;
        }
        leadSummary[item.LEAD_NO] += item.QTY;
    });
    const colors = ['blue', 'green', 'orange', 'purple', 'teal', 'pink', 'indigo', 'deep-orange'];

    let leadHtml = Object.keys(leadSummary).sort().map((leadNo, index) => {
        let color = colors[index % colors.length];
        return `<span class="chip ${color} white-text">${leadNo}: <b>${leadSummary[leadNo]} EA</b></span>`;
    }).join(' ');

    $("#C03_IN_LeadDetail").html(leadHtml);

    $("#C03_IN_TrolleySummary").show();
}

function C03_IN_TrolleyConfirm_Click() {
    if (!C03_IN_TrolleyData.length) return;

    $("#C03_IN_TrolleyConfirm, #C03_IN_TrolleyCancel").prop("disabled", true);
    $("#C03_IN_TrolleyProgress").show();
    $("#C03_IN_ProgressBar").css("width", "50%");
    $("#C03_IN_ProgressText").text("Đang xử lý...");

    let barcodes = C03_IN_TrolleyData.map(x => x.BARCODE);

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify({
        ListSearchString: barcodes,
        USER_ID: GetCookieValue("UserID"),
        USER_IDX: GetCookieValue("USER_IDX"),
        CompanyID: CompanyID,        
    }));

    fetch("/C03/InputTrolleyBarcodes", { method: "POST", body: formUpload })
        .then(r => r.json())
        .then(data => {
            $("#C03_IN_ProgressBar").css("width", "100%");

            if (data.Success) {
                $("#C03_IN_ProgressText").text(`Hoàn thành: ${data.SuccessCount}/${data.TotalCount}`);
                M.toast({ html: `Đã nhập kho ${data.SuccessCount} barcode`, classes: "green" });
                PlaySuccessSound();

                C03_IN_TrolleyData.forEach(item => {
                    AddToTempTableIn({
                        leadNo: item.LEAD_NO,
                        barcode: item.BARCODE,
                        hookRack: item.HOOK_RACK || "",
                        quantity: item.QTY
                    });
                });
                $("#C03_IN_Label37").text(TempDataIn.length.toString());
            } else {
                $("#C03_IN_ProgressText").text("Lỗi: " + (data.Error || "Không xác định"));
                M.toast({ html: data.Error || "Có lỗi xảy ra", classes: "red" });
                PlayErrorSound();
            }

            $("#C03_IN_TrolleyCancel").prop("disabled", false);
            setTimeout(() => $("#C03_IN_TrolleyModal").modal("close"), 1500);
        })
        .catch(() => {
            $("#C03_IN_ProgressText").text("Lỗi kết nối!");
            M.toast({ html: "Lỗi kết nối server", classes: "red" });
            $("#C03_IN_TrolleyCancel").prop("disabled", false);
        });
}
function C03_OUT_Load() {
    $("#Label15").text("");
    $("#Label11").text("");
    $("#Label38").text("0");
    $("#TextBox4").val("");
    LEAD_CONT_OUT = 0;
    TempData = [];
    currentGroupOut = "";
    DataGridView1Render();
    $("#TextBox4").focus();
}

function DataGridView1Render() {
    let tbody = $("#DataGridView1Body");
    tbody.empty();

    if (TempData.length === 0) {
        tbody.append('<tr><td colspan="4" class="center-align">Chưa có dữ liệu quét</td></tr>');
        return;
    }

    // Hiển thị tất cả dữ liệu thay vì chỉ 10 mục gần nhất
    let displayData = TempData.slice().reverse();

    displayData.forEach(function (item) {
        let row = `<tr>
            <td>${item.leadNo}</td>
            <td>${item.barcode}</td>
            <td class="center-align">${item.quantity}</td>
            <td class="center-align">
                <span class="chip ${item.status.includes('FA') ? 'blue' : 'green'} white-text">
                    ${item.status}
                </span>
            </td>
        </tr>`;
        tbody.append(row);
    });
}

function Buttonclose_Click() {
    $('#C03_OUT_Modal').modal('close');
}

function DATA_ADD() {

    CategoryDepartmentID = $("#CategoryDepartmentOUTPUT").val();
    if (CategoryDepartmentID == 0) {
        $("#TextBox4").val("");
        ShowError("Chọn bộ phận / Select department");
        PlayErrorSound();
    }
    if (CategoryDepartmentID > 0) {
        let IsCheck = true;

        let textBox4Text = $("#TextBox4").val().trim();
        if (CategoryDepartmentDisplay.length > 0) {
            if (textBox4Text.length > 0) {
                let BarcodeArray = textBox4Text.split("$$");
                let LeadNo = BarcodeArray[0];
                let CategoryDepartmentDisplayArray = CategoryDepartmentDisplay.split(";");
                if (CategoryDepartmentDisplayArray.length > 0) {
                    for (let i = 0; i < CategoryDepartmentDisplayArray.length; i++) {
                        let CategoryDepartmentDisplayCheck = CategoryDepartmentDisplayArray[i];
                        IsCheck = LeadNo.includes(CategoryDepartmentDisplayCheck);
                        if (IsCheck == false) {
                            ShowError(LeadNo + " xuất sai Line " + CategoryDepartmentCode);
                            PlayErrorSound();
                            $("#TextBox4").val("").focus();
                            i = CategoryDepartmentDisplayArray.length;
                        }
                        if (IsCheck == true) {
                            i = CategoryDepartmentDisplayArray.length;
                        }
                    }
                }

            }
        }
        if (IsCheck == true) {
            if (isProcessingOutput) {
                M.toast({ html: "Đang xử lý mã vạch, vui lòng đợi...", classes: 'orange' });
                return;
            }
            if (!textBox4Text) return;

            const validation = validateBarcodeFormat(textBox4Text);
            if (!validation.isValid) {
                ShowError(validation.message);
                PlayErrorSound();
                $("#TextBox4").val("").focus();
                return;
            }

            // CHẶN KHÁC NHÓM khi chưa bật NoGroupLock
            if (!NoGroupLock && currentGroupOut && !checkBarcodeGroupOut(textBox4Text)) {
                ShowError(`Mã vạch khác nhóm hiện tại (${currentGroupOut})\nBarcode from different group (${currentGroupOut})`);
                PlayErrorSound();
                $("#TextBox4").val("").focus();
                return;
            }

            isProcessingOutput = true;
            $("#TextBox4").prop("disabled", true);
            $("#Label15, #Label11").text("");
            $("#BackGround").css("display", "block");

            let BaseParameter = {
                ListSearchString: [textBox4Text, getCurrentUser()],
                CompanyID: CompanyID,
                CategoryDepartmentID: CategoryDepartmentID
            };
            let formUpload = new FormData();
            formUpload.append('BaseParameter', JSON.stringify(BaseParameter));

            fetch("/C03/DATA_ADD", { method: "POST", body: formUpload })
                .then(r => r.json())
                .then(data => {
                    isProcessingOutput = false;
                    $("#TextBox4").prop("disabled", false);
                    $("#BackGround").css("display", "none");
                    ProcessResult(data, textBox4Text);
                })
                .catch(err => {
                    isProcessingOutput = false;
                    $("#TextBox4").prop("disabled", false);
                    $("#BackGround").css("display", "none");
                    ShowError("Lỗi kết nối server: " + err.message);
                    PlayErrorSound();
                    $("#TextBox4").focus();
                });
        }
    }
}


function ProcessResult(data, originalBarcode) {
    if (data.Error) {
        ShowError(data.Error);
        $("#Label15").text("ERROR");
        $("#Label11").text("ERROR");
        $("#TextBox4").val("");
        if (data.Code === "SOUND_ERROR") PlayErrorSound();
        $("#TextBox4").focus();
        return;
    }

    // CHỈ tự bắt nhóm khi chưa bật NoGroupLock
    if ((data.Code === "SUCCESS" || data.Code === "SUCCESS_FA") && !currentGroupOut && !NoGroupLock) {
        currentGroupOut = getGroupFromBarcode(originalBarcode);
    }

    $("#Label15").text(data.Label15 || data.Data?.LeadNo || currentGroupOut);
    $("#Label11").text(data.Label11 || data.Data?.Barcode || originalBarcode);
    $("#TextBox4").val("");

    if (data.Code === "SUCCESS" || data.Code === "SUCCESS_FA") {
        PlaySuccessSound();
        AddToTempTable({
            leadNo: data.Label15 || data.Data?.LeadNo || currentGroupOut,
            barcode: originalBarcode,
            quantity: getQuantityFromBarcode(originalBarcode),
            status: data.Code === "SUCCESS_FA" ? "FA Process" : "Normal Process"
        });
        $("#Label38").text(TempData.length.toString());
        M.toast({ html: data.Message || 'Xuất kho thành công!', classes: 'green' });
    }

    $("#TextBox4").focus();
}
function initNoGroupF2() {
    const $cbs = $("#ChkNoGroupF2_IN, #ChkNoGroupF2_OUT");
    $cbs.prop("checked", NoGroupLock);
    $cbs.off("change").on("change", function () {
        NoGroupLock = $(this).is(":checked");
        $cbs.prop("checked", NoGroupLock);
    });
    $(document).off("keydown.noGroupF2").on("keydown.noGroupF2", function (e) {
        if (e.key === "F2") {
            e.preventDefault();
            NoGroupLock = !NoGroupLock;
            $cbs.prop("checked", NoGroupLock).trigger("change");
        }
    });
    $('#C03_IN_Modal').on('modal:open', function () {
        $("#ChkNoGroupF2_IN").prop("checked", NoGroupLock);
    });
    $('#C03_OUT_Modal').on('modal:open', function () {
        $("#ChkNoGroupF2_OUT").prop("checked", NoGroupLock);
    });
}


function AddToTempTable(item) {
    const quantity = getQuantityFromBarcode(item.barcode);

    TempData.push({
        leadNo: item.leadNo,
        barcode: item.barcode,
        quantity: quantity,
        status: item.status,
        timestamp: new Date().toLocaleString()
    });

    DataGridView1Render();
}
function Buttonadd_Click() {
    let activeTab = $('#TabControl1 .active').attr('href') || currentTab;

    if (activeTab === "#TabPage5") {
        $("#Tab5_leadNo").val("");
        $("#Tab5_index").val("NEW");
        $("#Tab5_hookRack").val("");
        $("#Tab5_safetyStock").val("");
        $("#Tab5_wirePNo").val("");
        $("#Tab5_wireName").val("");
        $("#Tab5_diameter").val("");
        $("#Tab5_wLink").val("");
        $("#Tab5_color").val("");
        $("#Tab5_wrNo").val("");
        $("#Tab5_length").val("");
        $("#Tab5_term1").val("");
        $("#Tab5_seal1").val("");
        $("#Tab5_strip1").val("");
        $("#Tab5_t1No").val("");
        $("#Tab5_cchW1").val("");
        $("#Tab5_ichW1").val("");
        $("#Tab5_term2").val("");
        $("#Tab5_seal2").val("");
        $("#Tab5_strip2").val("");
        $("#Tab5_t2No").val("");
        $("#Tab5_cchW2").val("");
        $("#Tab5_ichW2").val("");
        $("#Tab5_bundleQty").val("");

        $("#Tab5_leadRadio").prop("checked", true);
        $("#Tab5_dsYRadio").prop("checked", true);
        $("#Tab5_leadNo").prop("readonly", false);

        M.updateTextFields();

        $("#Tab5_SubPartBody").empty();
        $("#Tab5_SubPartBody").append('<tr><td colspan="3" class="center-align">Không có dữ liệu</td></tr>');
    }
}

function Buttoncancel_Click() {
    let activeTab = $('#TabControl1 .active').attr('href') || currentTab;

    if (activeTab === "#TabPage5") {
        $("#Tab5_leadNo").val("");
        $("#Tab5_index").val("");
        $("#Tab5_hookRack").val("");
        $("#Tab5_safetyStock").val("");
        $("#Tab5_wirePNo").val("");
        $("#Tab5_wireName").val("");
        $("#Tab5_diameter").val("");
        $("#Tab5_wLink").val("");
        $("#Tab5_color").val("");
        $("#Tab5_wrNo").val("");
        $("#Tab5_length").val("");
        $("#Tab5_term1").val("");
        $("#Tab5_seal1").val("");
        $("#Tab5_strip1").val("");
        $("#Tab5_t1No").val("");
        $("#Tab5_cchW1").val("");
        $("#Tab5_ichW1").val("");
        $("#Tab5_term2").val("");
        $("#Tab5_seal2").val("");
        $("#Tab5_strip2").val("");
        $("#Tab5_t2No").val("");
        $("#Tab5_cchW2").val("");
        $("#Tab5_ichW2").val("");
        $("#Tab5_bundleQty").val("");

        $("#Tab5_leadNo").prop("readonly", true);

        M.updateTextFields();

        $("#Tab5_SubPartBody").empty();
        $("#Tab5_SubPartBody").append('<tr><td colspan="3" class="center-align">Không có dữ liệu</td></tr>');
    }
}
function C03_RE_IN_Load() {
    $("#C03_RE_IN_Label4").text("");
    $("#C03_RE_IN_Label5").text("");
    $("#C03_RE_IN_Label12").text("");
    $("#C03_RE_IN_Label37").text("0");
    $("#C03_RE_IN_TextBox1").val("");
    $("#C03_RE_IN_NumericUpDown1").val(1);
    $("#C03_RE_IN_TextBox1").focus();
}
function C03_RE_IN_DATA_ADD() {

    CategoryDepartmentID = $("#CategoryDepartmentReINPUT").val();
    if (CategoryDepartmentID == 0) {
        ShowError("Chọn bộ phận / Select department");
        PlayErrorSound();
    }
    if (CategoryDepartmentID > 0) {

        let textBox1Text = $("#C03_RE_IN_TextBox1").val().trim();
        let numericUpDown1Value = $("#C03_RE_IN_NumericUpDown1").val();
        if (!textBox1Text) return;

        const validation = validateBarcodeFormat(textBox1Text);
        if (!validation.isValid) {
            ShowError(validation.message);
            PlayErrorSound();
            $("#C03_RE_IN_TextBox1").val("").focus();
            return;
        }

        $("#BackGround").css("display", "block");

        let BaseParameter = {
            ListSearchString: [textBox1Text, numericUpDown1Value, getCurrentUser()],
            CompanyID: CompanyID,
            CategoryDepartmentID: CategoryDepartmentID,
        };

        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));

        fetch("/C03/DATA_ADD_RE_IN", {
            method: "POST",
            body: formUpload
        })
            .then(response => response.json())
            .then(data => {
                $("#BackGround").css("display", "none");

                if (data.Error) {
                    ShowError(data.Error);
                    $("#C03_RE_IN_Label4").text("ERROR");
                    $("#C03_RE_IN_Label5").text("ERROR");
                    $("#C03_RE_IN_Label12").text("ERROR");
                    if (data.Code === "SOUND_ERROR") {
                        PlayErrorSound();
                    }
                    $("#C03_RE_IN_TextBox1").val("").focus();
                    return;
                }

                $("#C03_RE_IN_Label4").text(data.Label4 || "");
                $("#C03_RE_IN_Label5").text(data.Label5 || "");
                $("#C03_RE_IN_Label12").text(data.Label12 || "");
                $("#C03_RE_IN_Label37").text(data.Label37 || "0");

                if (data.Code === "SOUND_SUCCESS") {
                    PlaySuccessSound();
                    M.toast({ html: data.Message || 'Tái nhập kho thành công!', classes: 'green' });
                    if (typeof AddToTempTableIn === 'function') {
                        AddToTempTableIn({
                            leadNo: data.Label4 || "",
                            barcode: textBox1Text + "FA",
                            hookRack: data.Label5 || "",
                            quantity: numericUpDown1Value
                        });
                    }
                }
                $("#C03_RE_IN_TextBox1").val("").focus();
            })
            .catch(err => {
                $("#BackGround").css("display", "none");
                ShowError("Lỗi kết nối server: " + err.message);
                PlayErrorSound();
                $("#C03_RE_IN_TextBox1").focus();
            });
    }
}




