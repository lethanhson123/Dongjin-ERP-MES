let BaseResult = {};
let MachineListData = [];

function GetCookieValue(name) {
    const match = document.cookie.match(new RegExp('(^| )' + name + '=([^;]+)'));
    return match ? match[2] : null;
}

function getCurrentUser() {
    return GetCookieValue('UserID') || 'SYSTEM';
}

$(document).ready(function () {
    // Set default dates (today)
    let today = new Date().toISOString().split('T')[0];
    $("#FromDate").val(today);
    $("#ToDate").val(today);

    // Load initial data
    LoadMachines();
    Buttonfind_Click();

    // Bind button events
    $("#Buttonfind").click(Buttonfind_Click);
    $("#Buttonexport").click(Buttonexport_Click);
    $("#Buttonhelp").click(Buttonhelp_Click);
    $("#Buttonclose").click(Buttonclose_Click);

    // Search on Enter
    $("#SearchBox").on("keypress", function (e) {
        if (e.which === 13) Buttonfind_Click();
    });

    // Filter on change
    $("#FilterMachine").on("change", function () {
        Buttonfind_Click();
    });

    $("#FromDate").on("change", function () {
        Buttonfind_Click();
    });

    $("#ToDate").on("change", function () {
        Buttonfind_Click();
    });

    // Row click to select
    $("#DataGridView1").on("click", "tr", function () {
        $("#DataGridView1 tr.selected").removeClass("selected");
        $(this).addClass("selected");
    });
});

function ShowLoading(flag = true) {
    $("#BackGround").css("display", flag ? "block" : "none");
}

function ApiCall(action, param = {}) {
    ShowLoading(true);
    if (!param.USER_IDX) param.USER_IDX = getCurrentUser();

    let formData = new FormData();
    formData.append("BaseParameter", JSON.stringify(param));
    return fetch(`/HR3/${action}`, { method: "POST", body: formData })
        .then(r => r.json())
        .finally(() => ShowLoading(false));
}

function LoadMachines() {
    fetch('/HR3/Load', { method: "POST", body: new FormData() })
        .then(r => r.json())
        .then(res => {
            if (res.MachineList && res.MachineList.length > 0) {
                MachineListData = res.MachineList;
                let $select = $("#FilterMachine");
                $select.empty().append('<option value="">-- All Machines --</option>');

                res.MachineList.forEach(machine => {
                    $select.append(`<option value="${machine}">${machine}</option>`);
                });
            }
        })
        .catch(err => console.error("Error loading machines:", err));
}

function Buttonfind_Click() {
    let search = $("#SearchBox").val() || "";
    let filterMachine = $("#FilterMachine").val() || "";
    let fromDate = $("#FromDate").val() || "";
    let toDate = $("#ToDate").val() || "";

    ApiCall("Buttonfind_Click", {
        SearchString: search,
        MachineName: filterMachine,
        FromDate: fromDate ? new Date(fromDate).toISOString() : null,
        ToDate: toDate ? new Date(toDate).toISOString() : null
    })
        .then(res => {
            BaseResult = res;
            if (res.MachineList && res.MachineList.length > 0) {
                MachineListData = res.MachineList;
            }
            DataGridView1Render(res.AttendanceList || []);
        })
        .catch(err => {
            console.error("Error in search:", err);
            alert("An error occurred during search: " + err.message);
        });
}

function DataGridView1Render(data) {
    let $tbody = $("#DataGridView1 tbody");
    $tbody.empty();

    if (!data || !data.length) {
        $tbody.append('<tr><td colspan="7" class="center-align">No data</td></tr>');
        $("#TotalRecords").text("0");
        return;
    }

    data.forEach((item, index) => {
        let checkTypeText = "";
        let checkTypeClass = "";

        if (item.CheckType === 0) {
            checkTypeText = "In";
            checkTypeClass = "green-text";
        } else if (item.CheckType === 1) {
            checkTypeText = "Out";
            checkTypeClass = "red-text";
        } else {
            checkTypeText = "-";
            checkTypeClass = "grey-text";
        }

        $tbody.append(`
            <tr data-id="${item.ID}">
                <td>${index + 1}</td>
                <td>${item.ID ?? ""}</td>
                <td>${item.MachineName ?? ""}</td>
                <td><strong>${item.EmployeeCode ?? ""}</strong></td>
                <td>${formatDateTime(item.CheckTime)}</td>
                <td class="${checkTypeClass}"><strong>${checkTypeText}</strong></td>
                <td>${formatDateTime(item.CreatedDate)}</td>
            </tr>
        `);
    });

    $("#TotalRecords").text(data.length);
}

function formatDateTime(dateTimeStr) {
    if (!dateTimeStr) return "";

    const date = new Date(dateTimeStr);
    if (isNaN(date.getTime())) return "";

    // Format: dd/MM/yyyy HH:mm:ss
    const day = String(date.getDate()).padStart(2, '0');
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const year = date.getFullYear();
    const hours = String(date.getHours()).padStart(2, '0');
    const minutes = String(date.getMinutes()).padStart(2, '0');
    const seconds = String(date.getSeconds()).padStart(2, '0');

    return `${day}/${month}/${year} ${hours}:${minutes}:${seconds}`;
}

function Buttonexport_Click() {
    TableHTMLToExcel('DataGridView1', 'Attendance.xls', 'Attendance');
}

function Buttonhelp_Click() {
    window.open("/WMP_PLAY", "_blank", "width=800,height=460");
}

function Buttonclose_Click() {
    history.back();
}