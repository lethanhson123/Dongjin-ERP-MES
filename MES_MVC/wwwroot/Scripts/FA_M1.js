let BaseResult = {};
let EmployeeList = [];
let ShiftList = [];

// ========== HELPER FUNCTIONS ==========

function GetCookieValue(name) {
    const match = document.cookie.match(new RegExp('(^| )' + name + '=([^;]+)'));
    return match ? match[2] : null;
}

function getCurrentUser() {
    return GetCookieValue('UserID') || 'SYSTEM';
}

function ShowLoading(flag = true) {
    $("#BackGround").css("display", flag ? "block" : "none");
}

function ApiCall(action, param = {}) {
    ShowLoading(true);
    if (!param.USER_IDX) param.USER_IDX = getCurrentUser();

    let formData = new FormData();
    formData.append("BaseParameter", JSON.stringify(param));
    return fetch(`/FA_M1/${action}`, { method: "POST", body: formData })
        .then(r => r.json())
        .finally(() => ShowLoading(false));
}

// ========================================
// ✅ FORMAT DATETIME - Chuẩn Việt Nam
// ========================================
function formatDateTime(dateTimeStr) {
    if (!dateTimeStr) return "";

    const date = new Date(dateTimeStr);
    if (isNaN(date.getTime())) return "";

    const day = String(date.getDate()).padStart(2, '0');
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const year = date.getFullYear();
    const hours = String(date.getHours()).padStart(2, '0');
    const minutes = String(date.getMinutes()).padStart(2, '0');
    const seconds = String(date.getSeconds()).padStart(2, '0');

    return `${day}/${month}/${year} ${hours}:${minutes}:${seconds}`;
}

// ========================================
// ✅ FORMAT DATE - Chỉ ngày
// ========================================
function formatDate(dateStr) {
    if (!dateStr) return "";

    const date = new Date(dateStr);
    if (isNaN(date.getTime())) return "";

    const day = String(date.getDate()).padStart(2, '0');
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const year = date.getFullYear();

    return `${day}/${month}/${year}`;
}

// ========================================
// ✅ FORMAT TIME - Chỉ giờ:phút
// ========================================
function formatTime(dateTimeStr) {
    if (!dateTimeStr) return "";

    const date = new Date(dateTimeStr);
    if (isNaN(date.getTime())) return "";

    const hours = String(date.getHours()).padStart(2, '0');
    const minutes = String(date.getMinutes()).padStart(2, '0');

    return `${hours}:${minutes}`;
}

// ========== INITIALIZATION ==========

$(document).ready(function () {
    LoadInitialData();
    Buttonfind_Click();

    // Button events
    $("#Buttonfind").click(Buttonfind_Click);
    $("#Buttonexport").click(Buttonexport_Click);
    $("#Buttonprint").click(Buttonprint_Click);
    $("#Buttonhelp").click(Buttonhelp_Click);
    $("#Buttonclose").click(Buttonclose_Click);

    // Enter key on filters
    $("#StartDate, #EndDate, #EmployeeFilter, #LineFilter").on("keypress", function (e) {
        if (e.which === 13) Buttonfind_Click();
    });
});

// ========================================
// LOAD INITIAL DATA
// ========================================
function LoadInitialData() {
    fetch('/FA_M1/Load', { method: "POST", body: new FormData() })
        .then(r => r.json())
        .then(res => {
            if (res.EmployeeFAList) {
                EmployeeList = res.EmployeeFAList;
            }
            if (res.ShiftTimes) {
                ShiftList = res.ShiftTimes;
            }
        })
        .catch(err => console.error("Error loading data:", err));
}

// ========================================
// BUTTONFIND - Tìm kiếm
// ========================================
function Buttonfind_Click() {
    let startDate = $("#StartDate").val();
    let endDate = $("#EndDate").val();
    let employeeCode = $("#EmployeeFilter").val(); // TextBox - mã hoặc tên
    let line = $("#LineFilter").val();

    let param = {
        StartDate: startDate,
        EndDate: endDate,
        EmployeeCode: employeeCode, // Gửi EmployeeCode thay vì ID
        SearchString: line
    };

    ApiCall("Buttonfind_Click", param)
        .then(res => {
            BaseResult = res;
            if (res.Success) {
                if (res.EmployeeFAList) EmployeeList = res.EmployeeFAList;
                if (res.ShiftTimes) ShiftList = res.ShiftTimes;
                RenderTable(res.AttendanceSessionList || []);
            } else {
                alert(res.Error || "Có lỗi xảy ra");
            }
        })
        .catch(err => {
            console.error("Error in search:", err);
            alert("Có lỗi xảy ra khi tìm kiếm: " + err.message);
        });
}

// ========================================
// RENDER TABLE
// ========================================
function RenderTable(data) {
    let $tbody = $("#DataGridView1 tbody");
    $tbody.empty();

    if (!data || data.length === 0) {
        $tbody.append('<tr><td colspan="11" class="center-align">Không có dữ liệu</td></tr>');
        return;
    }

    data.forEach(item => {
        // Tìm tên nhân viên
        let employee = EmployeeList.find(e => e.ID === item.EmployeeID);
        let employeeName = employee ? `${employee.EmpCode} - ${employee.Name}` : item.EmployeeID;

        // Tìm tên ca
        let shift = ShiftList.find(s => s.ID === item.ShiftID);
        let shiftName = shift ? shift.ShiftName : item.ShiftID;

        // Tính giờ làm
        let workingHours = item.WorkingMinutes ? (item.WorkingMinutes / 60).toFixed(2) : "0";

        // Trạng thái
        let statusClass = item.Status === "Complete" ? "green-text" : "orange-text";
        let statusText = item.Status === "Complete" ? "Hoàn thành" : "Chưa hoàn thành";

        // Late indicator
        let lateClass = item.IsLate ? "red-text" : "";
        let lateText = item.LateMinutes > 0 ? `${item.LateMinutes} phút` : "-";

        // Overtime
        let overtimeText = item.OvertimeMinutes > 0 ? `${item.OvertimeMinutes} phút` : "-";

        $tbody.append(`
            <tr>
                <td class="center-align">${item.ID}</td>
                <td class="center-align">${formatDate(item.WorkDate)}</td>
                <td>${employeeName}</td>
                <td class="center-align">${shiftName}</td>
                <td class="center-align">${item.Line || "-"}</td>
                <td class="center-align">${formatTime(item.CheckInTime)}</td>
                <td class="center-align">${item.CheckOutTime ? formatTime(item.CheckOutTime) : "-"}</td>
                <td class="center-align">${item.WorkingMinutes || 0} phút (${workingHours}h)</td>
                <td class="center-align ${lateClass}"><b>${lateText}</b></td>
                <td class="center-align">${overtimeText}</td>
                <td class="center-align ${statusClass}"><b>${statusText}</b></td>
            </tr>
        `);
    });
}

// ========================================
// BUTTONEXPORT - Export Excel
// ========================================
function Buttonexport_Click() {
    if (!BaseResult.AttendanceSessionList || BaseResult.AttendanceSessionList.length === 0) {
        alert("Không có dữ liệu để export!");
        return;
    }
    TableHTMLToExcel('DataGridView1', 'BaoCaoChamCong.xls', 'AttendanceSession');
}

// ========================================
// BUTTONPRINT - In Report
// ========================================
function Buttonprint_Click() {
    window.print();
}

// ========================================
// BUTTONHELP
// ========================================
function Buttonhelp_Click() {
    window.open("/WMP_PLAY", "_blank", "width=800,height=460");
}

// ========================================
// BUTTONCLOSE
// ========================================
function Buttonclose_Click() {
    history.back();
}

// ========================================
// TABLE TO EXCEL (Helper function)
// ========================================
function TableHTMLToExcel(tableID, filename, sheetName) {
    let table = document.getElementById(tableID);
    if (!table) {
        alert("Không tìm thấy bảng dữ liệu!");
        return;
    }

    let html = table.outerHTML;
    let url = 'data:application/vnd.ms-excel,' + encodeURIComponent(html);
    let link = document.createElement("a");
    link.href = url;
    link.download = filename;
    link.click();
}