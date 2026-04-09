let BaseResult = {};
let EmployeeList = [];
let LineListData = [];
let DowntimeInterval = null;
let DowntimeStartTime = null;
let CurrentDowntimeLine = null;
let IsDowntimeRunning = false;
let EditingAttendanceID = null;
function GetCookieValue(name) {
    const match = document.cookie.match(new RegExp('(^| )' + name + '=([^;]+)'));
    return match ? match[2] : null;
}

function getCurrentUser() {
    return GetCookieValue('UserID') || 'SYSTEM';
}

function PlaySuccessSound() {
    try {
        new Audio('/audio/HOOK_S.mp3').play().catch(e => console.log('Cannot play success sound:', e));
    } catch (e) {
        console.log('Success sound not available');
    }
}

function PlayErrorSound() {
    try {
        new Audio('/audio/Sash_brk.mp3').play().catch(e => console.log('Cannot play error sound:', e));
    } catch (e) {
        console.log('Error sound not available');
    }
}

function detectEntryMethod(input) {
    return input.endsWith('$') ? "QR" : "Manual";
}

function cleanEmployeeCode(input) {
    return input.replace('$', '').trim();
}

$(document).ready(function () {
    // ✅ Set ngày hiện tại
    SetTodayDate();

    LoadLines();
    LoadStaffList();

    $("#Buttonexport").click(Buttonexport_Click);
    $("#Buttonclose").click(Buttonclose_Click);
    $("#BtnStopLine").click(BtnStopLine_Click);
    $("#BtnRefresh").click(LoadStaffList); // ✅ Refresh data
    $("#BtnToday").click(BackToToday_Click); // ✅ Nút hôm nay
    $("#BtnModalStart").click(ModalStart_Click);
    $("#BtnModalClose").click(ModalClose_Click);

    $('.modal').modal({ dismissible: false });

    $("#ScanQR").on("keypress", function (e) {
        if (e.which === 13) {
            $("#ChkScanOut").is(":checked") ? ScanOut_Click() : ScanIn_Click();
        }
    });

    // ✅ Khi chọn ngày khác → load lại data
    $("#SelectedDate").on("change", function () {
        LoadStaffList();
    });

    $("#LineFilter").on("change", function () {
        let selectedLine = $(this).val();

        if (IsDowntimeRunning && CurrentDowntimeLine && selectedLine !== CurrentDowntimeLine) {
            M.toast({
                html: `Line '${CurrentDowntimeLine}' đang downtime! Không thể chuyển line.`,
                classes: 'red',
                displayLength: 4000
            });
            PlayErrorSound();
            $(this).val(CurrentDowntimeLine);
            return;
        }

        LoadStaffList();
    });

    $("#ChkScanIn").on("change", function () {
        if (this.checked) {
            $("#ChkScanOut").prop("checked", false);
            $("#ScanQR").attr("placeholder", "Scan QR Code for Scan In");
        }
    });

    $("#ChkScanOut").on("change", function () {
        if (this.checked) {
            $("#ChkScanIn").prop("checked", false);
            $("#ScanQR").attr("placeholder", "Scan QR Code for Scan Out");
        }
    });
    $("#WorkerListTable").on("click", ".btn-edit-shift", function () {
        let id = $(this).closest("tr").data("id");
        let attendanceData = BaseResult.AttendanceRecordsList?.find(x => x.ID == id);

        if (!attendanceData) {
            alert("Không tìm thấy dữ liệu!");
            return;
        }

        OpenEditShiftModal(id, attendanceData.ShiftID);
    });
    $("#BtnSaveEditShift").click(SaveEditShift);
});
function OpenEditShiftModal(attendanceID, currentShiftID) {
    EditingAttendanceID = attendanceID;
    fetch('/FA_M5/Load', {
        method: "POST",
        body: new FormData()
    })
        .then(r => r.json())
        .then(res => {
            // Populate dropdown
            let $select = $("#EditShiftSelect");
            $select.empty();
            $select.append('<option value="">-- Chọn ca --</option>');

            if (res.ShiftTimes && res.ShiftTimes.length > 0) {
                res.ShiftTimes.forEach(shift => {
                    let selected = shift.ID == currentShiftID ? 'selected' : '';
                    $select.append(`<option value="${shift.ID}" ${selected}>${shift.ShiftName}</option>`);
                });
                BaseResult.ShiftTimes = res.ShiftTimes;
            } else {
                $select.append('<option value="">Không có ca nào</option>');
            }
            if ($select.length) {
                M.FormSelect.init($select[0]);
            }
            $('#ModalEditShift').modal('open');
        })
        .catch(err => {
            console.error('Error loading shifts:', err);
            M.toast({
                html: 'Lỗi khi tải danh sách ca: ' + err.message,
                classes: 'red',
                displayLength: 3000
            });
        });
}
function SaveEditShift() {
    let shiftID = $("#EditShiftSelect").val();

    if (!shiftID) {
        M.toast({
            html: 'Vui lòng chọn ca!',
            classes: 'orange',
            displayLength: 3000
        });
        return;
    }

    ApiCall('EditShift', {
        ID: EditingAttendanceID,
        ShiftID: parseInt(shiftID)
    })
        .then(res => {
            if (res.Success) {
                PlaySuccessSound();
                M.toast({
                    html: res.Message,
                    classes: 'green',
                    displayLength: 3000
                });
                $('#ModalEditShift').modal('close');
                LoadStaffList(); 
            } else {
                PlayErrorSound();
                M.toast({
                    html: res.Error,
                    classes: 'red',
                    displayLength: 3000
                });
            }
        })
        .catch(err => {
            PlayErrorSound();
            M.toast({
                html: "Lỗi: " + err.message,
                classes: 'red',
                displayLength: 3000
            });
        });
}

function SetTodayDate() {
    const today = new Date();
    const formatted = today.getFullYear() + '-' +
        String(today.getMonth() + 1).padStart(2, '0') + '-' +
        String(today.getDate()).padStart(2, '0');
    $("#SelectedDate").val(formatted);
}
function BackToToday_Click() {
    SetTodayDate();
    LoadStaffList();
}
function IsToday() {
    const selectedDate = $("#SelectedDate").val();
    const today = new Date();
    const todayStr = today.getFullYear() + '-' +
        String(today.getMonth() + 1).padStart(2, '0') + '-' +
        String(today.getDate()).padStart(2, '0');
    return selectedDate === todayStr;
}
function ShowLoading(flag = true) {
    $("#BackGround").css("display", flag ? "block" : "none");
}

function ApiCall(action, param = {}) {
    ShowLoading(true);
    if (!param.USER_IDX) param.USER_IDX = getCurrentUser();

    let formData = new FormData();
    formData.append("BaseParameter", JSON.stringify(param));
    return fetch(`/FA_M5/${action}`, { method: "POST", body: formData })
        .then(r => r.json())
        .finally(() => ShowLoading(false));
}

function LoadLines() {
    ApiCall("Load")
        .then(res => {
            if (res.Success && res.LineList && res.LineList.length > 0) {
                LineListData = res.LineList;
                let $select = $("#LineFilter");
                $select.empty().append('<option value="">-- Tất cả Line --</option>');

                res.LineList.forEach(line => {
                    let text = line.LineGroup
                        ? `${line.LineGroup} - ${line.LineName} - ${line.Family}`
                        : line.LineName;
                    $select.append(`<option value="${text}">${text}</option>`);
                });
            }
        })
        .catch(err => console.error("Error loading lines:", err));
}

function ScanIn_Click() {
    // ✅ Chỉ cho scan nếu đang xem hôm nay
    if (!IsToday()) {
        M.toast({
            html: 'Chỉ có thể scan cho ngày hôm nay!',
            classes: 'orange',
            displayLength: 3000
        });
        PlayErrorSound();
        return;
    }

    let input = $("#ScanQR").val().trim();
    let line = $("#LineFilter").val().trim();

    if (!input) {
        M.toast({ html: 'Vui lòng nhập mã nhân viên!', classes: 'red' });
        PlayErrorSound();
        return;
    }

    if (!line) {
        M.toast({ html: 'Vui lòng chọn Line!', classes: 'red' });
        PlayErrorSound();
        return;
    }

    if (IsDowntimeRunning && CurrentDowntimeLine === line) {
        M.toast({
            html: `⚠️ Cảnh báo: Line '${line}' đang downtime!`,
            classes: 'orange',
            displayLength: 3000
        });
    }

    let entryMethod = detectEntryMethod(input);
    let empCode = cleanEmployeeCode(input);

    ApiCall("ScanIn_Click", {
        SearchString: empCode,
        Line: line,
        EntryMethod: entryMethod
    })
        .then(res => {
            if (res.Success) {
                PlaySuccessSound();
                M.toast({ html: res.Message, classes: 'green', displayLength: 3000 });
                $("#ScanQR").val("").focus();
                LoadStaffList();
            } else {
                PlayErrorSound();
                M.toast({ html: res.Error, classes: 'red', displayLength: 4000 });
                $("#ScanQR").val("").focus();
            }
        })
        .catch(err => {
            PlayErrorSound();
            M.toast({ html: "Lỗi: " + err.message, classes: 'red' });
        });
}

function ScanOut_Click() {
    // ✅ Chỉ cho scan nếu đang xem hôm nay
    if (!IsToday()) {
        M.toast({
            html: 'Chỉ có thể scan cho ngày hôm nay!',
            classes: 'orange',
            displayLength: 3000
        });
        PlayErrorSound();
        return;
    }

    let rawInput = $("#ScanQR").val().trim();

    if (!rawInput) {
        M.toast({ html: 'Vui lòng nhập mã nhân viên!', classes: 'red' });
        PlayErrorSound();
        return;
    }

    let entryMethod = detectEntryMethod(rawInput);
    let empCode = cleanEmployeeCode(rawInput);

    ApiCall("ScanOut_Click", {
        SearchString: empCode,
        EntryMethod: entryMethod
    })
        .then(res => {
            if (res.Success) {
                PlaySuccessSound();
                M.toast({ html: res.Message, classes: 'green', displayLength: 3000 });
                $("#ScanQR").val("").focus();
                LoadStaffList();
            } else {
                PlayErrorSound();
                M.toast({ html: res.Error, classes: 'red', displayLength: 4000 });
                $("#ScanQR").val("").focus();
            }
        })
        .catch(err => {
            PlayErrorSound();
            M.toast({ html: "Lỗi: " + err.message, classes: 'red' });
        });
}


function LoadStaffList() {
    let line = $("#LineFilter").val().trim();
    let selectedDate = $("#SelectedDate").val();

    ApiCall("GetStaffList", {
        Line: line,
        SelectedDate: selectedDate
    })
        .then(res => {
            if (res.Success) {
                BaseResult = res;
                EmployeeList = res.EmployeeFAList || [];
                RenderStaffList(res.AttendanceRecordsList || []);
            }
        })
        .catch(err => console.error("Error loading staff list:", err));
}

function RenderStaffList(data) {
    let scanInList = data.filter(x => x.Status && x.Status.startsWith("ScanIn"));
    let scanOutList = data.filter(x => x.Status && x.Status.startsWith("ScanOut"));

    // ==================== SCAN IN TABLE ====================
    let $scanInTable = $("#StaffListTable");
    $scanInTable.empty();

    if (!scanInList || scanInList.length === 0) {
        $scanInTable.append('<tr><td colspan="5" class="center-align">Chưa có nhân viên scan in</td></tr>');
        $("#TotalScanIn").text("0");
    } else {
        scanInList.forEach((item) => {
            let employee = GetEmployeeByID(item.EmployeeFAID);
            let shift = GetShiftByID(item.ShiftID);
            let isManual = item.Status.includes("Manual");
            let badge = isManual
                ? '<span class="badge orange">✏️ Manual</span>'
                : '<span class="badge green">📱 QR</span>';

            $scanInTable.append(`
                <tr class="${isManual ? 'orange lighten-5' : ''}">
                    <td>${employee ? employee.EmpCode : ""}</td>
                    <td>${employee ? employee.Name : ""}</td>
                    <td>${shift ? shift.ShiftName : ""}</td>
                    <td>${formatDateTime(item.ScanIn)}</td>
                    <td>${badge}</td>
                </tr>
            `);
        });
        $("#TotalScanIn").text(scanInList.length);
    }

    // ==================== SCAN OUT TABLE (với Edit Button) ====================
    let $scanOutTable = $("#WorkerListTable");
    $scanOutTable.empty();

    if (!scanOutList || scanOutList.length === 0) {
        $scanOutTable.append('<tr><td colspan="9" class="center-align">Chưa có nhân viên scan out</td></tr>');
        $("#TotalScanOut").text("0");
    } else {
        scanOutList.forEach((item) => {
            let employee = GetEmployeeByID(item.EmployeeFAID);
            let shift = GetShiftByID(item.ShiftID);
            let isManual = item.Status.includes("Manual");
            let badge = isManual
                ? '<span class="badge orange">✏️ Manual</span>'
                : '<span class="badge green">📱 QR</span>';

            let downtimeDisplay = "";
            if (item.TotalDownTime && item.TotalDownTime > 0) {
                downtimeDisplay = `<span style="color: #f44336; font-weight: bold;">${item.TotalDownTime.toFixed(2)}</span>`;
            } else {
                downtimeDisplay = "0";
            }

            // ✅ THÊM CỘT EDIT SHIFT BUTTON
            $scanOutTable.append(`
                <tr class="${isManual ? 'orange lighten-5' : ''}" data-id="${item.ID}">
                    <td>${employee ? employee.EmpCode : ""}</td>
                    <td>${employee ? employee.Name : ""}</td>
                    <td>${shift ? shift.ShiftName : ""}</td>
                    <td>${formatDateTime(item.ScanIn)}</td>
                    <td>${formatDateTime(item.ScanOut)}</td>
                    <td>${item.WorkingTime ? item.WorkingTime.toFixed(2) : ""}</td>
                    <td>${downtimeDisplay}</td>
                    <td>${badge}</td>
                    <td>
                        <button class="btn btn-small blue btn-edit-shift" title="Sửa ca làm việc">
                            <i class="material-icons">edit</i>
                        </button>
                    </td>
                </tr>
            `);
        });
        $("#TotalScanOut").text(scanOutList.length);
    }
}


function BtnStopLine_Click() {
    // ✅ Chỉ cho stop line nếu đang xem hôm nay
    if (!IsToday()) {
        M.toast({
            html: 'Chỉ có thể stop line cho ngày hôm nay!',
            classes: 'orange',
            displayLength: 3000
        });
        PlayErrorSound();
        return;
    }

    let line = $("#LineFilter").val().trim();

    if (!line) {
        M.toast({ html: 'Vui lòng chọn Line!', classes: 'red' });
        PlayErrorSound();
        return;
    }

    if (IsDowntimeRunning && CurrentDowntimeLine === line) {
        M.toast({
            html: `Line '${line}' đang downtime rồi!`,
            classes: 'orange',
            displayLength: 3000
        });
        $('#ModalStopLine').modal('open');
        return;
    }

    CurrentDowntimeLine = line;
    $("#ModalLineName").text(line);
    $("#ModalTimer").text("00:00");
    $("#DowntimeReason").val("").prop("disabled", false);

    $("#BtnModalStart").show();
    $("#BtnModalClose").text("Cancel").show();

    $('#ModalStopLine').modal('open');
}

function ModalStart_Click() {
    let reason = $("#DowntimeReason").val();

    if (!reason) {
        M.toast({ html: 'Vui lòng chọn lý do dừng line!', classes: 'orange' });
        return;
    }

    ApiCall("StartDowntime", { Line: CurrentDowntimeLine })
        .then(res => {
            if (res.Success) {
                PlaySuccessSound();
                M.toast({ html: 'Bắt đầu downtime!', classes: 'green' });

                IsDowntimeRunning = true;
                DowntimeStartTime = new Date();

                $("#BtnModalStart").hide();
                $("#BtnModalClose").text("Stop");
                $("#DowntimeReason").prop("disabled", true);
                $("#LineFilter").prop("disabled", true);
                $("#BtnStopLine").prop("disabled", true);

                if (DowntimeInterval) clearInterval(DowntimeInterval);

                DowntimeInterval = setInterval(function () {
                    let now = new Date();
                    let diff = Math.floor((now - DowntimeStartTime) / 1000);
                    let minutes = Math.floor(diff / 60);
                    let seconds = diff % 60;
                    let display = String(minutes).padStart(2, '0') + ':' + String(seconds).padStart(2, '0');
                    $("#ModalTimer").text(display);
                }, 1000);

            } else {
                PlayErrorSound();
                M.toast({ html: res.Error, classes: 'red' });
            }
        })
        .catch(err => {
            PlayErrorSound();
            M.toast({ html: "Lỗi: " + err.message, classes: 'red' });
        });
}

function ModalClose_Click() {
    if (!IsDowntimeRunning) {
        $('#ModalStopLine').modal('close');
        return;
    }

    if (confirm("Dừng downtime?")) {
        ApiCall("EndDowntime", { Line: CurrentDowntimeLine })
            .then(res => {
                if (res.Success) {
                    PlaySuccessSound();
                    M.toast({ html: res.Message, classes: 'green' });

                    if (DowntimeInterval) {
                        clearInterval(DowntimeInterval);
                        DowntimeInterval = null;
                    }

                    IsDowntimeRunning = false;
                    DowntimeStartTime = null;
                    CurrentDowntimeLine = null;

                    $("#DowntimeReason").prop("disabled", false);
                    $("#LineFilter").prop("disabled", false);
                    $("#BtnStopLine").prop("disabled", false);

                    $('#ModalStopLine').modal('close');
                    LoadStaffList();

                } else {
                    PlayErrorSound();
                    M.toast({ html: res.Error, classes: 'red' });
                }
            })
            .catch(err => {
                PlayErrorSound();
                M.toast({ html: "Lỗi: " + err.message, classes: 'red' });
            });
    }
}

function GetEmployeeByID(empId) {
    if (!empId || !EmployeeList || EmployeeList.length === 0) return null;
    return EmployeeList.find(x => x.ID == empId);
}

function GetShiftByID(shiftId) {
    if (!shiftId || !BaseResult.ShiftTimes || BaseResult.ShiftTimes.length === 0) return null;
    return BaseResult.ShiftTimes.find(x => x.ID == shiftId);
}

function formatDateTime(dateTimeStr) {
    if (!dateTimeStr) return "";

    let date = new Date(dateTimeStr);
    if (isNaN(date.getTime())) return "";

    const day = String(date.getDate()).padStart(2, '0');
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const year = date.getFullYear();
    const hours = String(date.getHours()).padStart(2, '0');
    const minutes = String(date.getMinutes()).padStart(2, '0');
    const seconds = String(date.getSeconds()).padStart(2, '0');

    return `${day}/${month}/${year} ${hours}:${minutes}:${seconds}`;
}

function Buttonexport_Click() {
    M.toast({ html: 'Export functionality to be implemented', classes: 'blue' });
}

function Buttonclose_Click() {
    history.back();
}