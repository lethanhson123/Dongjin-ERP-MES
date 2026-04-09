let windowHeight = Math.floor(screen.height);
let windowWidth = Math.floor(screen.width);
let DimTop = (windowHeight - 300) / 2;
let DimLeft = (windowWidth - 300) / 2;
let protocol = window.location.protocol;
let hostname = window.location.hostname;
let port = window.location.port;
let DomainSite = protocol + "://" + hostname + ":" + port;
localStorage.setItem("DomainSite", DomainSite);

document.getElementById("BackGroundDim").style.top = DimTop + "px";
document.getElementById("BackGroundDim").style.left = DimLeft + "px";

localStorage.setItem("SaveComplete", "Save database complete");
localStorage.setItem("SaveSuccess", "정상처리 되었습니다. Đã được lưu.");
localStorage.setItem("SaveNotSuccess", "오류가 발생 하였습니다. Một lỗi đã xảy ra.");
localStorage.setItem("DeleteConfirm", "Do you want delete this data?");
localStorage.setItem("DeleteSuccess", "Delete Success");
localStorage.setItem("DeleteNotSuccess", "Delete Not Success");
localStorage.setItem("ERROR", "Unable to connect to database. Please Check Again.");
localStorage.setItem("ERRORNOTDATA", "NOT DATA. Please Check Again.");
localStorage.setItem("ERRORPleaseCheckAgain", "Please Check Again.");
localStorage.setItem("ERRORAnErrorOccurred", "오류가 발생 하였습니다. Một lỗi đã xảy ra.");
localStorage.setItem("DeleteConfirm", "Confirm Delete?");
localStorage.setItem("ERRORCheck", "다시 확인 바랍니다. Vui lòng kiểm tra lại.");
localStorage.setItem("ExcelImportSuccess", "Completed. Import to Excel.");
localStorage.setItem("ERROR_B08_Add_001", "입고 담당자가 없습니다. Người nhập kho. Một lỗi đã xảy ra.");
localStorage.setItem("ERROR_B08_Add_002", "BARCODE SCAN 을 잘못했습니다. Scan sai Barcode.");
localStorage.setItem("ERROR_B08_Add_003", "이미 처리된 바코드. Đã xử lý Barcode trước đó");
localStorage.setItem("ERROR_B08_Add_004", "출고 담당자가 없습니다. Người xuất kho. Một lỗi đã xảy ra.");
localStorage.setItem("Notification_B08_001", "Do you want to process the goods issue?");
localStorage.setItem("Notification_A01_001", "품목 선택이 잘 못 되었습니다. Lựa chọn mục sai.");
localStorage.setItem("Notification_B09_001", "Can Not Delete. Please Check Again.");
localStorage.setItem("Notification_B10_001", "Sheet NO. 오류가 발생 하였습니다. Sheet NO.  Một lỗi đã xảy ra.");
localStorage.setItem("Notification_B10_002", "출력 데이터 없음. không có dữ liệu.  Một lỗi đã xảy ra.");
localStorage.setItem("Notification_G02_001", "PART NO Please Check Again.");
localStorage.setItem("Notification_G02_002", "Required(Reason) no data. Please Check Again.");
localStorage.setItem("Notification_G02_003", "Required(Change quantity) no data. Please Check Again.");
localStorage.setItem("Notification_G02_004", "Required(After) no data. Please Check Again.");
localStorage.setItem("Notification_V01_001", "제품명(VN)을 입력하여 주시기 바랍니다. Vui lòng nhập tên sản phẩm của bạn(VN)");
localStorage.setItem("Notification_V01_002", "제품명(KR)을 입력하여 주시기 바랍니다. Vui lòng nhập tên sản phẩm của bạn(KR)");
localStorage.setItem("Notification_V01_003", "수량을 입력하여 주시기 바랍니다. Vui lòng nhập số lượng");
localStorage.setItem("Notification_V01_004", "제품번호를 입력하여 주시기 바랍니다. Vui lòng nhập tên sản phẩm của bạn(Sản phẩm số)");
localStorage.setItem("Notification_V01_005", "Part Name을 입력하여 주시기 바랍니다. Vui lòng nhập tên sản phẩm của bạn(Part Name)");
localStorage.setItem("Notification_V01_006", "Description을 입력하여 주시기 바랍니다. Vui lòng nhập tên sản phẩm của bạn(Description)");
localStorage.setItem("Notification_V01_007", "Type을 입력하여 주시기 바랍니다. Vui lòng nhập tên sản phẩm của bạn(Type)");
localStorage.setItem("Notification_V01_008", "업체 이름을 입력 누락");
localStorage.setItem("Notification_V01_009", "회사이름 오류가 발생 하였습니다. Company Name Một lỗi đã xảy ra.");
localStorage.setItem("Notification_V01_010", "Data 오류가 발생 하였습니다. Data Một lỗi đã xảy ra.");
localStorage.setItem("Notification_V01_4_001", "FILE NAME 오류가 발생 하였습니다. FILE NAME Một lỗi đã xảy ra.");


var SettingsFNM = 0;
var SettingsFNMName = "Factory 1";
var SettingsMC_NM = "---";
var SettingsNON_OPER_CHK = false;
var SettingsNON_OPER_IDX = 0;
var ToolStripLOGIDX;
var SettingsMC_LIST_GRP = "";
var SettingsMASTER_BC = "";
var WORING_IDX = 0;


$(document).ready(function () {

    // Materialize chỉ init select thường
    $('select').not('.select2-only').formSelect();

});

function GetCookieValue(name) {
    const regex = new RegExp(`(^| )${name}=([^;]+)`)
    const match = document.cookie.match(regex)
    if (match) {
        return match[2]
    }
}

function OpenWindowByURL(URL, w = 200, h = 200, level = 1) {
    w = 200;
    h = 200;
    if (level > 1) {
        w = w * 2;
        h = h * 2;
    }
    if (level == 1) {
        w = 0;
        h = 0;
    }
    var width = windowWidth;
    var height = windowHeight;
    var width01 = width - w;
    var height01 = height - h;
    var left = (width - width01) / 2;
    var top = (height - height01) / 2;
    var url = URL;
    window.open(url, '_blank', 'location=yes,height=' + height01 + ',width=' + width01 + ',scrollbars=yes,status=yes,toolbar=yes,top=' + top + ',left=' + left + '');
}

function TableHTMLToExcel(table, sheetName, fileName) {


    var ua = window.navigator.userAgent;
    var msie = ua.indexOf("MSIE ");
    if (msie > 0 || !!navigator.userAgent.match(/Trident.*rv\:11\./))      // If Internet Explorer
    {
        return fnExcelReport(table, fileName);
    }

    var uri = 'data:application/vnd.ms-excel;base64,',
        templateData = '<html xmlns:o="urn:schemas-microsoft-com:office:office" xmlns:x="urn:schemas-microsoft-com:office:excel" xmlns="http://www.w3.org/TR/REC-html40"><head><!--[if gte mso 9]><xml><x:ExcelWorkbook><x:ExcelWorksheets><x:ExcelWorksheet><x:Name>{worksheet}</x:Name><x:WorksheetOptions><x:DisplayGridlines/></x:WorksheetOptions></x:ExcelWorksheet></x:ExcelWorksheets></x:ExcelWorkbook></xml><![endif]--><meta http-equiv="content-type" content="text/plain; charset=UTF-8"/></head><body><table>{table}</table></body></html>',
        base64Conversion = function (s) { return window.btoa(unescape(encodeURIComponent(s))) },
        formatExcelData = function (s, c) { return s.replace(/{(\w+)}/g, function (m, p) { return c[p]; }) }

    $("tbody > tr[data-level='0']").show();

    if (!table.nodeType)
        table = document.getElementById(table)

    var ctx = { worksheet: sheetName || 'Worksheet', table: table.innerHTML }

    var element = document.createElement('a');
    element.setAttribute('href', 'data:application/vnd.ms-excel;base64,' + base64Conversion(formatExcelData(templateData, ctx)));
    element.setAttribute('download', fileName);
    element.style.display = 'none';
    document.body.appendChild(element);
    element.click();
    document.body.removeChild(element);

    $("tbody > tr[data-level='0']").hide();
}

function DataTableToExcel(dataTable, sheetName, fileName) {

    // Lấy toàn bộ data từ DataTable (không phân trang)
    const data = dataTable
        .rows({ search: 'applied' })
        .data()
        .toArray();

    if (!data || data.length === 0) {
        alert("No data to export");
        return;
    }

    // Convert sang HTML table tạm
    let html = "<table><thead><tr>";

    // Header
    dataTable.columns().every(function () {
        html += `<th>${this.header().innerText}</th>`;
    });
    html += "</tr></thead><tbody>";

    // Body
    data.forEach(row => {
        html += "<tr>";
        row.forEach(col => {
            html += `<td>${col ?? ""}</td>`;
        });
        html += "</tr>";
    });

    html += "</tbody></table>";

    // Tạo table ảo để reuse hàm cũ
    const tempTable = document.createElement("table");
    tempTable.innerHTML = html;

    TableHTMLToExcel(tempTable, sheetName, fileName);
}


document.addEventListener("DOMContentLoaded", function () {
    // Tìm tất cả các bảng trên trang
    const tables = document.querySelectorAll("table");

    tables.forEach(table => {
        const tbody = table.querySelector("tbody");
        if (!tbody) return;

        // Gắn sự kiện click cho từng tbody
        tbody.addEventListener("click", function (e) {
            const clickedRow = e.target.closest("tr");
            if (!clickedRow || !tbody.contains(clickedRow)) return;

            // Xóa class 'selected' khỏi tất cả các dòng trong cùng bảng
            tbody.querySelectorAll("tr").forEach(r => r.classList.remove("selected"));

            // Gán class 'selected' cho dòng được click
            clickedRow.classList.add("selected");
        });
    });
});

function FormatDate(dateStr) {
    if (!dateStr || String(dateStr).trim() === "") return ""; // Kiểm tra null, undefined, hoặc rỗng

    const date = new Date(dateStr);
    if (isNaN(date)) return ""; // Không phải ngày hợp lệ → trả chuỗi rỗng

    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const day = String(date.getDate()).padStart(2, '0');
    return `${year}-${month}-${day}`;
}

function FormatDateTime(dateStr) {
    if (!dateStr || String(dateStr).trim() === "") return ""; // Kiểm tra null, undefined, hoặc rỗng

    const date = new Date(dateStr);
    if (isNaN(date)) return ""; // Không phải ngày hợp lệ → trả chuỗi rỗng

    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const day = String(date.getDate()).padStart(2, '0');

    const hours = String(date.getHours()).padStart(2, '0');
    const minutes = String(date.getMinutes()).padStart(2, '0');
    const seconds = String(date.getSeconds()).padStart(2, '0');

    return `${year}-${month}-${day} ${hours}:${minutes}:${seconds}`;
}
function ConvertStringToSecond(input, char) {
    let hh_time = Number(Text_time.split(char)[0]) * 60 * 60;
    let mm_time = Number(Text_time.split(char)[1]) * 60;
    let ss_time = Number(Text_time.split(char)[2]);
    let result = hh_time + mm_time + ss_time;
    return result;
}
function ConvertSecondToString(input, char) {
    let result = "";
    if (input < 0) {
        input = input * (-1);
    }
    let days = Math.floor(input / (24 * 60 * 60));
    input = input - (days * (24 * 60 * 60));
    let hours = Math.floor(input / (60 * 60));
    input = input - (hours * (60 * 60));
    let minutes = Math.floor(input / (60));
    let seconds = input - (minutes * (60));
    let resultSub = "";
    if (days > 0) {
        if (days < 10) {
            resultSub = String(days).padStart(1, '0') + String(hours).padStart(2, '0') + String(minutes).padStart(2, '0') + String(seconds).padStart(2, '0');
        }
        else {
            if (days < 100) {
                resultSub = String(days).padStart(2, '0') + String(hours).padStart(2, '0') + String(minutes).padStart(2, '0') + String(seconds).padStart(2, '0');
            }
            else {
                if (days < 1000) {
                    resultSub = String(days).padStart(3, '0') + String(hours).padStart(2, '0') + String(minutes).padStart(2, '0') + String(seconds).padStart(2, '0');
                }
            }
        }
    }
    else {
        resultSub = String(hours).padStart(2, '0') + String(minutes).padStart(2, '0') + String(seconds).padStart(2, '0');
    }
    let hourString = resultSub.substr(0, 2);
    let minuteString = resultSub.substr(2, 2);
    let secondString = resultSub.substr(4, 2);
    result = hourString + char + minuteString + char + secondString;
    return result;
}


function DataGridViewSort(List, IsTableSort) {
    if (List) {
        if (IsTableSort == true) {
            List.sort((a, b) => (a > b ? 1 : -1));
        }
        else {
            List.sort((a, b) => (a < b ? 1 : -1));
        }
    }
}
function ListSort(List, key, text, IsSort) {
    if (List) {
        if (List.length > 0) {
            let obj = List[0];
            text = text.toLowerCase();
            text = text.split(" ")[0];
            let IsCheck = false;
            for (let objkey of Object.keys(obj)) {
                let objkeySub = objkey.toLowerCase();
                if (objkeySub == text) {
                    key = objkey;
                    IsCheck = true;
                    break;
                }
            }
            if (IsCheck == false) {
                for (let objkey of Object.keys(obj)) {
                    let objkeySub = objkey.toLowerCase();
                    if (objkeySub.includes(text) == true) {
                        key = objkey;
                        break;
                    }
                }
            }
            if (IsSort == true) {
                List.sort((a, b) => (a[key] > b[key] ? 1 : -1));
            }
            else {
                List.sort((a, b) => (a[key] < b[key] ? 1 : -1));
            }
        }
    }
}


function GetWeekNumberByDate(date) {
    const currentDate =
        (typeof date === 'object') ? date : new Date();
    const januaryFirst =
        new Date(currentDate.getFullYear(), 0, 1);
    const daysToNextMonday =
        (januaryFirst.getDay() === 1) ? 0 :
            (7 - januaryFirst.getDay()) % 7;
    const nextMonday =
        new Date(currentDate.getFullYear(), 0,
            januaryFirst.getDate() + daysToNextMonday);

    let result = (currentDate < nextMonday) ? 52 :
        (currentDate > nextMonday ? Math.ceil(
            (currentDate - nextMonday) / (24 * 3600 * 1000) / 7) : 1);

    result = result + 1

    return result;
}
function NameShortByName(name) {
    if (name) {
        if (name.length > 20) {
            name = name.substring(0, 20) + "...";
        }
    }
    return name;
}
function CounterByBegin_EndToString(Begin, End) {
    let time = Math.floor((End.getTime() - Begin.getTime()) / 1000);
    let Second01 = time % 60;
    let Minute01 = Math.floor((time % 3660) / 60);
    let Hour01 = Math.floor(time / 3600);
    let Second = "" + Second01;
    let Minute = "" + Minute01;
    let Hour = "" + Hour01;
    if (Second01 < 10) {
        Second = "0" + Second01;
    }
    if (Minute01 < 10) {
        Minute = "0" + Minute01;
    }
    if (Hour01 < 10) {
        Hour = "0" + Hour01;
    }
    let result = Hour + " : " + Minute + " : " + Second;
    return result;
}
function DateToString(date) {
    var day = ("0" + date.getDate()).slice(-2);
    var month = ("0" + (date.getMonth() + 1)).slice(-2);
    var today = date.getFullYear() + "-" + (month) + "-" + (day);
    return today;
}

function showCustomAlert(message, timeout = 3000) {
    const alertBox = document.getElementById('custom-alert');
    const alertMessage = document.getElementById('custom-alert-message');
    alertMessage.textContent = message;
    alertBox.style.display = 'block';

    setTimeout(() => {
        alertBox.style.display = 'none';
    }, timeout);
}
function SPCValuesNormalizeNumber(input) {
    if (((input.includes(",") == true) && (input.includes(",") == true))) {
        input = input.replace(",", "");
    }
    else {
        if (input.includes(",") == true) {
            input = input.Replace(",", ".");
        }
    }
    return input;
}
function SPCValuesCheckValue(input, minValue, maxValue) {
    let result = false;

    let normalizedInput = Number(input);
    let normalizedMin = Number(minValue);
    let normalizedMax = Number(maxValue);

    if (normalizedInput >= normalizedMin) {
        if (normalizedInput <= normalizedMax) {
            result = true;
        }
    }
    return result;
}

function ShowMessage(message, type) {

    if (type === 'error') {
        M.toast({ html: message, classes: 'red' });
    } else if (type === 'warning') {
        M.toast({ html: message, classes: 'orange' });
    }
    else {
        M.toast({ html: message, classes: 'green' });
    }
    $("#BackGround").css("display", "none");
}


function ExportJSONToExcel(data, sheetMame, fileName = "Export.xlsx") {
    if (!data || data.length === 0) {
        ShowMessage("No Data to Export", "error");
        return;
    }

    // Chuyển dữ liệu sang định dạng sheet
    const worksheet = XLSX.utils.json_to_sheet(data);

    // Tạo workbook
    const workbook = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(workbook, worksheet, sheetMame);

    // Xuất file
    XLSX.writeFile(workbook, fileName);
}


function autoCompleteSelector(selector, placeholder = 'Select...', allowClear = true) {
    const $el = $(selector);

    if ($el.length === 0) return;

    // Nếu đang bị Materialize wrap → tháo ra
    if ($el.parent().hasClass('select-wrapper')) {
        $el.unwrap();
    }

    // Xóa các element thừa do Materialize sinh ra
    $el
        .siblings('input.select-dropdown').remove()
        .siblings('ul.dropdown-content').remove();

    // Destroy nếu đã khởi tạo trước đó
    if ($el.hasClass("select2-hidden-accessible")) {
        $el.select2('destroy');
    }

    // Khởi tạo Select2
    $el.select2({
        placeholder: placeholder,
        allowClear: allowClear,
        width: '100%'
    });
}

