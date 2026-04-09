$(document).ready(function () {
    $('.modal').modal();
    initializeDatePickers();
    loadData();

    $("#Buttonfind").click(function () {
        Buttonfind_Click();
    });

    $("#Buttonadd").click(function () {
        Buttonadd_Click();
    });

    $("#Buttonsave").click(function () {
        Buttonsave_Click();
    });

    $("#Buttondelete").click(function () {
        Buttondelete_Click();
    });

    $("#Buttoncancel").click(function () {
        Buttoncancel_Click();
    });

    $("#Buttoninport").click(function () {
        Buttoninport_Click();
    });

    $("#Buttonexport").click(function () {
        Buttonexport_Click();
    });

    $("#Buttonprint").click(function () {
        Buttonprint_Click();
    });

    $("#Buttonhelp").click(function () {
        Buttonhelp_Click();
    });

    $("#Buttonclose").click(function () {
        Buttonclose_Click();
    });

    $("#BtnSaveAssignment").click(function () {
        saveAssignment();
    });

    $("#BtnDeleteAssignment").click(function () {
        deleteAssignment();
    });

    $("#LineName").change(function () {
        Buttonfind_Click();
    });

    $("#SearchBox").on('input', function () {
        clearTimeout(window.searchTimeout);
        window.searchTimeout = setTimeout(function () {
            Buttonfind_Click();
        }, 500);
    });
});

function isPrimaryShift(shiftName) {
    if (!shiftName) return false;

    const lowerName = shiftName.toLowerCase();
    if (lowerName.includes("tăng ca") || lowerName.includes("tc")) {
        return false;
    }
    return true;
}

function initializeDatePickers() {
    var today = new Date();
    $("#StartDate").val(today.toISOString().split('T')[0]);

    var nextMonth = new Date();
    nextMonth.setMonth(nextMonth.getMonth() + 1);
    $("#EndDate").val(nextMonth.toISOString().split('T')[0]);
}

function loadData() {
    $("#BackGround").css("display", "block");

    let BaseParameter = {
        Code: "",
        SearchString: ""
    };

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));

    fetch("/FA_M2/Buttonfind_Click", {
        method: "POST",
        body: formUpload,
        cache: "no-store"
    })
        .then(response => response.json())
        .then(data => {
            if (!data.Success) {
                showError(data.Error || "Không thể tải dữ liệu");
                $("#BackGround").css("display", "none");
                return;
            }

            refreshStatisticsTable(data, "");
            $("#BackGround").css("display", "none");
        })
        .catch(err => {
            showError("Lỗi khi tải dữ liệu: " + err.message);
            console.error("Error:", err);
            $("#BackGround").css("display", "none");
        });
}

function Buttonfind_Click() {
    $("#BackGround").css("display", "block");

    let selectedLineId = $("#LineName").val();

    let BaseParameter = {
        Code: selectedLineId,
        SearchString: $("#SearchBox").val()
    };

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));

    fetch("/FA_M2/Buttonfind_Click", {
        method: "POST",
        body: formUpload,
        cache: "no-store"
    })
        .then(response => response.json())
        .then(data => {
            if (!data.Success) {
                showError(data.Error || "Tìm kiếm không thành công");
                $("#BackGround").css("display", "none");
                return;
            }

            refreshStatisticsTable(data, selectedLineId);
            $("#BackGround").css("display", "none");
        })
        .catch(err => {
            showError("Lỗi khi tìm kiếm: " + err.message);
            console.error("Error:", err);
            $("#BackGround").css("display", "none");
        });
}

function Buttonadd_Click() {
    showError("Vui lòng chọn Line từ bảng để xem chi tiết");
}

function Buttonsave_Click() {
    showError("Chức năng này không áp dụng. Vui lòng sử dụng các thao tác phân công trong bảng.");
}

function Buttondelete_Click() {
    showError("Chức năng này không áp dụng. Vui lòng sử dụng các thao tác phân công trong bảng.");
}

function Buttoncancel_Click() {
    $("#LineName").val("");
    $("#SearchBox").val("");

    Buttonfind_Click();
}

function Buttoninport_Click() {
    showError("Chức năng nhập dữ liệu chưa được triển khai");
}

function Buttonexport_Click() {
    showError("Chức năng xuất dữ liệu chưa được triển khai");
}

function Buttonprint_Click() {
    window.print();
}

function Buttonhelp_Click() {
    let url = "/WMP_PLAY";
    OpenWindowByURL(url, 800, 460);
}

function Buttonclose_Click() {
    history.back();
}

function OpenWindowByURL(url, width, height) {
    window.open(url, "_blank", "width=" + width + ",height=" + height);
}

function refreshStatisticsTable(data, selectedLineId) {
    let table = $("#StatisticsTable tbody");
    table.empty();

    if (!data.LineList || data.LineList.length === 0) {
        table.append("<tr><td colspan='5' class='center-align'>Không có dữ liệu</td></tr>");
        return;
    }

    let filteredLines = data.LineList;
    if (selectedLineId && selectedLineId !== "") {
        filteredLines = data.LineList.filter(l => l.ID == selectedLineId);
    }

    filteredLines.forEach(line => {
        let lineAssignments = [];
        if (data.LineAssignments) {
            lineAssignments = data.LineAssignments.filter(a =>
                a.LineID === line.ID && a.Active === true
            );
        }

        let shiftCounts = {};
        lineAssignments.forEach(assignment => {
            let shift = data.ShiftTimes.find(s => s.ID === assignment.ShiftID);
            if (shift) {
                if (!shiftCounts[shift.ShiftName]) {
                    shiftCounts[shift.ShiftName] = 0;
                }
                shiftCounts[shift.ShiftName]++;
            }
        });

        let workerNumber = line.WorkerNumber || 0;
        let assigned = lineAssignments.length;
        let remaining = workerNumber - assigned;
        let status = "";
        let statusColor = "";

        if (remaining > 0) {
            status = `Thiếu ${remaining}`;
            statusColor = "#FFCCBC";
        } else if (remaining < 0) {
            status = `Dư ${Math.abs(remaining)}`;
            statusColor = "#FFF9C4";
        } else {
            status = "Đủ";
            statusColor = "#C8E6C9";
        }

        let shiftCountsText = Object.keys(shiftCounts).length > 0
            ? Object.keys(shiftCounts).map(shiftName =>
                `${shiftName}: ${shiftCounts[shiftName]}`
            ).join(" | ")
            : "Chưa có phân công";

        let lineDisplayName = line.LineGroup
            ? `${line.LineGroup} - ${line.LineName}${line.Family ? ' - ' + line.Family : ''}`
            : line.LineName;

        let row = `<tr class="line-row" data-line="${line.ID}" style="cursor: pointer; background-color: #e3f2fd;">
            <td style="font-weight: bold;">
                <i class="material-icons">factory</i> ${lineDisplayName}
            </td>
            <td style="text-align: center; font-weight: bold;">${workerNumber}</td>
            <td style="text-align: center; font-weight: bold;">${assigned}</td>
            <td style="text-align: center; font-weight: bold;">${remaining}</td>
            <td style="background-color: ${statusColor}; font-weight: bold; text-align: center;">${status}</td>
        </tr>`;

        table.append(row);
    });

    $(".line-row").click(function () {
        const lineId = $(this).data("line");
        viewLineDetails(lineId, data);
    });
}

function viewLineDetails(lineId, allData) {
    $("#BackGround").css("display", "block");

    let BaseParameter = {
        ID: lineId
    };

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));

    fetch("/FA_M2/GetLineHistory", {
        method: "POST",
        body: formUpload
    })
        .then(response => response.json())
        .then(data => {
            if (!data.Success) {
                showError(data.Error || "Lỗi không xác định");
                $("#BackGround").css("display", "none");
                return;
            }

            const line = allData.LineList.find(l => l.ID == lineId);
            const lineDisplayName = line
                ? (line.LineGroup
                    ? `${line.LineGroup} - ${line.LineName}${line.Family ? ' - ' + line.Family : ''}`
                    : line.LineName)
                : "Line";

            $("#line-history-title").text(`Danh sách nhân viên: ${lineDisplayName}`);
            refreshLineHistoryTable(data);
            $("#modal-line-history").modal("open");
            $("#BackGround").css("display", "none");
        })
        .catch(err => {
            showError("Lỗi khi lấy dữ liệu: " + err.message);
            console.error("Error:", err);
            $("#BackGround").css("display", "none");
        });
}

function refreshLineHistoryTable(data) {
    let table = $("#LineHistoryTable tbody");
    table.empty();

    if (!data.LineAssignments || data.LineAssignments.length === 0) {
        table.append("<tr><td colspan='8' class='center-align'>Chưa có nhân viên phân công</td></tr>");
        return;
    }

    data.LineAssignments.forEach(assignment => {
        let employee = data.EmployeeFAList.find(e => e.ID === assignment.EmployeeID);
        let shift = data.ShiftTimes.find(s => s.ID === assignment.ShiftID);

        if (!employee || !shift) return;

        let empCode = employee.EmpCode || "-";
        let empName = employee.Name || "-";
        let shiftName = shift.ShiftName || "-";

        let isPrimary = isPrimaryShift(shift.ShiftName);
        let shiftType = isPrimary ? "Chính" : "Tăng ca";
        let shiftTypeColor = isPrimary ? "#4CAF50" : "#FF9800";

        let startDate = new Date(assignment.StartDate).toLocaleDateString('vi-VN');
        let endDate = assignment.EndDate
            ? new Date(assignment.EndDate).toLocaleDateString('vi-VN')
            : "Không giới hạn";

        let now = new Date();
        now.setHours(0, 0, 0, 0);

        let start = new Date(assignment.StartDate);
        start.setHours(0, 0, 0, 0);

        let end = assignment.EndDate ? new Date(assignment.EndDate) : null;
        if (end) {
            end.setHours(23, 59, 59, 999);
        }

        let status = "";
        let statusColor = "";

        if (now < start) {
            status = "Sắp bắt đầu";
            statusColor = "#FFF9C4";
        } else if (!end || now <= end) {
            status = "Đang làm";
            statusColor = "#C8E6C9";
        } else {
            status = "Đã kết thúc";
            statusColor = "#FFCCBC";
        }

        let actionButtons = "";
        if (assignment.Active) {
            actionButtons = `
                <button class="btn-small blue edit-line-assignment" data-id="${assignment.ID}" data-emp="${assignment.EmployeeID}">
                    <i class="material-icons">edit</i>
                </button>
            `;
        }

        let row = `<tr>
            <td>${actionButtons}</td>
            <td>${empCode}</td>
            <td>${empName}</td>
            <td>
                <span style="background-color: ${shiftTypeColor}; color: white; padding: 2px 8px; border-radius: 3px; font-size: 11px;">
                    ${shiftType}
                </span>
            </td>
            <td>${shiftName}</td>
            <td>${startDate}</td>
            <td>${endDate}</td>
            <td style="background-color: ${statusColor}; font-weight: bold;">${status}</td>
        </tr>`;

        table.append(row);
    });

    $(".edit-line-assignment").click(function () {
        const empId = $(this).data("emp");
        const assignId = $(this).data("id");
        $("#modal-line-history").modal("close");
        openAssignmentModal(empId, assignId);
    });
}

function openAssignmentModal(employeeId, assignmentId) {
    $("#AssignmentForm")[0].reset();
    $("#BtnDeleteAssignment").hide();
    $("#BackGround").css("display", "block");

    let BaseParameter = {
        ID: employeeId
    };

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));

    fetch("/FA_M2/Buttonadd_Click", {
        method: "POST",
        body: formUpload
    })
        .then(response => response.json())
        .then(data => {
            if (!data.Success) {
                showError(data.Error || "Lỗi không xác định");
                $("#BackGround").css("display", "none");
                return;
            }

            const employee = data.Data;
            $("#EmployeeID").val(employee.ID);
            $("#EmpCode").val(employee.EmpCode);
            $("#Name").val(employee.Name);
            $("#CurrentLine").val(employee.Line || "-");

            $("#AssignedLine").empty().append('<option value="">-- Chọn dây chuyền --</option>');
            if (data.LineList) {
                data.LineList.forEach(line => {
                    let text = line.LineGroup
                        ? `${line.LineGroup} - ${line.LineName} - ${line.Family}`
                        : line.LineName;
                    $("#AssignedLine").append(`<option value="${line.ID}">${text}</option>`);
                });
            }

            $("#Shift").empty().append('<option value="">-- Chọn ca làm việc --</option>');
            if (data.ShiftTimes) {
                data.ShiftTimes.forEach(shift => {
                    $("#Shift").append(`<option value="${shift.ID}">${shift.ShiftName}</option>`);
                });
            }

            $("#AssignmentID").val(assignmentId || "");
            $("#modal-title").text(assignmentId ? "Sửa phân công" : "Thêm mới phân công");

            if (assignmentId) {
                loadAssignmentData(assignmentId);
                $("#BtnDeleteAssignment").show();
            } else {
                var today = new Date();
                $("#StartDate").val(today.toISOString().split('T')[0]);

                var nextMonth = new Date();
                nextMonth.setMonth(nextMonth.getMonth() + 1);
                $("#EndDate").val(nextMonth.toISOString().split('T')[0]);

                $("#modal-assignment").modal("open");
                $("#BackGround").css("display", "none");
            }
        })
        .catch(err => {
            showError("Lỗi khi lấy dữ liệu nhân viên: " + err.message);
            console.error("Error:", err);
            $("#BackGround").css("display", "none");
        });
}

function loadAssignmentData(assignmentId) {
    $("#BackGround").css("display", "block");

    let BaseParameter = {
        ID: assignmentId
    };

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));

    fetch("/FA_M2/GetAssignment", {
        method: "POST",
        body: formUpload
    })
        .then(response => response.json())
        .then(data => {
            if (!data.Success) {
                showError(data.Error || "Lỗi không xác định");
                $("#BackGround").css("display", "none");
                return;
            }

            const assignment = data.Data;
            $("#AssignedLine").val(assignment.LineID);
            $("#Shift").val(assignment.ShiftID);

            if (assignment.StartDate) {
                $("#StartDate").val(assignment.StartDate);
            }

            if (assignment.EndDate) {
                $("#EndDate").val(assignment.EndDate);
            }

            $("#Description").val(assignment.Description || "");

            $("#modal-assignment").modal("open");
            $("#BackGround").css("display", "none");
        })
        .catch(err => {
            showError("Lỗi khi lấy dữ liệu phân công: " + err.message);
            console.error("Error:", err);
            $("#BackGround").css("display", "none");
        });
}

function saveAssignment() {
    if (!validateAssignmentForm()) {
        return;
    }

    $("#BackGround").css("display", "block");

    let BaseParameter = {
        ID: $("#AssignmentID").val() || 0,
        UserID: $("#EmployeeID").val(),
        Code: $("#AssignedLine").val(),
        GroupCode: $("#Shift").val(),
        StartDate: $("#StartDate").val(),
        EndDate: $("#EndDate").val(),
        Remark: $("#Description").val(),
        USER_IDX: $("input[name='__RequestVerificationToken']").val()
    };

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));

    fetch("/FA_M2/Buttonsave_Click", {
        method: "POST",
        body: formUpload
    })
        .then(response => response.json())
        .then(data => {
            if (!data.Success) {
                showError(data.Error || "Lưu không thành công");
                $("#BackGround").css("display", "none");
                return;
            }

            $("#modal-assignment").modal("close");
            M.toast({ html: data.Message || 'Lưu phân công thành công', classes: 'green' });

            setTimeout(function () {
                Buttonfind_Click();
            }, 500);
        })
        .catch(err => {
            showError("Lỗi khi lưu phân công: " + err.message);
            console.error("Error:", err);
            $("#BackGround").css("display", "none");
        });
}

function deleteAssignment() {
    if (!confirm("Bạn có chắc chắn muốn xóa phân công này?")) {
        return;
    }

    $("#BackGround").css("display", "block");

    let BaseParameter = {
        ID: $("#AssignmentID").val(),
        USER_IDX: $("input[name='__RequestVerificationToken']").val()
    };

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));

    fetch("/FA_M2/Buttondelete_Click", {
        method: "POST",
        body: formUpload
    })
        .then(response => response.json())
        .then(data => {
            if (!data.Success) {
                showError(data.Error || "Xóa không thành công");
                $("#BackGround").css("display", "none");
                return;
            }

            $("#modal-assignment").modal("close");
            M.toast({ html: data.Message || 'Xóa phân công thành công', classes: 'green' });

            setTimeout(function () {
                Buttonfind_Click();
            }, 500);
        })
        .catch(err => {
            showError("Lỗi khi xóa phân công: " + err.message);
            console.error("Error:", err);
            $("#BackGround").css("display", "none");
        });
}

function validateAssignmentForm() {
    if (!$("#AssignedLine").val()) {
        showError("Vui lòng chọn dây chuyền");
        return false;
    }

    if (!$("#Shift").val()) {
        showError("Vui lòng chọn ca làm việc");
        return false;
    }

    if (!$("#StartDate").val()) {
        showError("Vui lòng nhập ngày bắt đầu");
        return false;
    }

    if ($("#EndDate").val() && new Date($("#StartDate").val()) > new Date($("#EndDate").val())) {
        showError("Ngày kết thúc phải sau ngày bắt đầu");
        return false;
    }

    return true;
}

function showError(message) {
    $("#error-message").text(message);
    $("#error-modal").modal("open");
}