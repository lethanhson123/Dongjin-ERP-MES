let CurrentID = null;
let BaseResult = {};
let LineListData = [];
let ShiftTimesData = [];
let ImportedData = [];

function parseHireDateFromEmpCode(empCode) {
    if (!empCode || empCode.length < 8) return null;

    try {
        let yy = empCode.substring(0, 2);
        let mm = empCode.substring(2, 4);
        let dd = empCode.substring(4, 6);

        let year = parseInt(yy) + 2000;
        let month = parseInt(mm);
        let day = parseInt(dd);


        if (month < 1 || month > 12 || day < 1 || day > 31) {
            return null;
        }
        let yyyy = year.toString();
        let mm_str = month.toString().padStart(2, '0');
        let dd_str = day.toString().padStart(2, '0');

        return `${yyyy}-${mm_str}-${dd_str}`;

    } catch (e) {
        console.error('Parse hire date error:', e);
        return null;
    }
}

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
    if (!param.USER_IDX) {
        param.USER_IDX = getCurrentUser();
    }
    let formData = new FormData();
    formData.append("BaseParameter", JSON.stringify(param));
    return fetch(`/HR1/${action}`, {
        method: "POST",
        body: formData
    })
        .then(r => r.json())
        .finally(() => ShowLoading(false));
}

function formatDateTime(dateTimeStr) {
    if (!dateTimeStr) return "";
    const date = typeof dateTimeStr === 'string' ? new Date(dateTimeStr) : dateTimeStr;
    if (isNaN(date.getTime())) {
        return dateTimeStr.substring(0, 16).replace('T', ' ');
    }
    return date.toISOString().substring(0, 16).replace('T', ' ');
}

$(document).ready(function () {
    $('.modal').modal();
    LoadInitialData();
    Buttonfind_Click();
    $("#Buttonfind").click(Buttonfind_Click);
    $("#Buttonadd").click(Buttonadd_Click);
    $("#Buttonsave").click(Buttonsave_Click);
    $("#Buttondelete").click(Buttondelete_Click);
    $("#Buttoncancel").click(Buttoncancel_Click);
    $("#Buttoninport").click(Buttoninport_Click);
    $("#Buttonexport").click(Buttonexport_Click);
    $("#Buttonprint").click(Buttonprint_Click);
    $("#Buttonhelp").click(Buttonhelp_Click);
    $("#Buttonclose").click(Buttonclose_Click);
    $("#EmpCode").on("input", function () {
        let empCode = $(this).val().trim();
        if (empCode.length >= 8 && !$("#HireDate").val()) {
            let parsedDate = parseHireDateFromEmpCode(empCode);
            if (parsedDate) {
                $("#HireDate").val(parsedDate);
                $("#HireDate").css("border-color", "#4CAF50");
                setTimeout(() => $("#HireDate").css("border-color", ""), 800);
            }
        }
    });
    $("#DataGridView1").on("click", ".btn-edit", function () {
        let id = $(this).closest("tr").data("id");
        let data = (BaseResult.EmployeeFAList || []).find(x => x.ID == id);
        if (!data) return;

        FillForm(data);
        CurrentID = id;
        $("#modal-title").text("Edit Employee");
        $("#modal-add").modal("open");
    });
    $("#DataGridView1").on("click", "tr", function (e) {
        if ($(e.target).is('input[type="checkbox"]') ||
            $(e.target).is('label') ||
            $(e.target).closest('label').length) {
            return;
        }
        $("#DataGridView1 tr.selected").removeClass("selected");
        $(this).addClass("selected");
    });
    $("#SelectAllCheckbox").on("change", function () {
        let isChecked = $(this).prop("checked");
        $(".employee-checkbox").prop("checked", isChecked);
    });
    $("#DataGridView1").on("change", ".employee-checkbox", function () {
        let totalCheckboxes = $(".employee-checkbox").length;
        let checkedCheckboxes = $(".employee-checkbox:checked").length;
        $("#SelectAllCheckbox").prop("checked", totalCheckboxes === checkedCheckboxes);
    });
    $("#ModalSaveBtn").click(Buttonsave_Click);
    $("#SearchBox").on("keypress", function (e) {
        if (e.which === 13) {
            Buttonfind_Click();
        }
    });
    $("#FilterLine").on("change", function () {
        Buttonfind_Click();
    });
    $("#FileImportHR1").on("change", HandleFileSelect);
    $("#BtnSaveImportHR1").click(SaveImport);
    $("#FilterLine, #BulkShiftID").on("change", function () {
        updateBulkShiftButton();
    });

    $("#BtnBulkAssignShift").click(BulkAssignShiftByLine);
});

function updateBulkShiftButton() {
    const lineSelected = $("#FilterLine").val();
    const shiftSelected = $("#BulkShiftID").val();
    $("#BtnBulkAssignShift").prop("disabled", !(lineSelected && shiftSelected));
}

function BulkAssignShiftByLine() {
    const line = $("#FilterLine").val();
    const shiftID = parseInt($("#BulkShiftID").val());

    if (!line) {
        M.toast({ html: '⚠️ Vui lòng chọn Line!', classes: 'orange darken-2' });
        return;
    }

    if (!shiftID) {
        M.toast({ html: '⚠️ Vui lòng chọn ca!', classes: 'orange darken-2' });
        return;
    }

    const shiftInfo = ShiftTimesData.find(s => s.ID === shiftID);
    const shiftName = shiftInfo ? shiftInfo.ShiftName : shiftID;

    if (!confirm(`Bạn chắc chắn muốn phân ca "${shiftName}" cho TẤT CẢ nhân viên trong Line "${line}"?`)) {
        return;
    }

    ApiCall("BulkAssignShift", {
        LineToAssign: line,
        ShiftID: shiftID
    })
        .then(res => {
            if (res.Success) {
                M.toast({ html: '✅ ' + (res.Message || 'Phân ca thành công!'), classes: 'green darken-1' });
                Buttonfind_Click();
                $("#BulkShiftID").val("");
                updateBulkShiftButton();
            } else {
                M.toast({ html: '❌ ' + (res.Error || 'Có lỗi xảy ra!'), classes: 'red darken-2' });
            }
        })
        .catch(err => {
            M.toast({ html: '❌ Có lỗi xảy ra: ' + err.message, classes: 'red darken-2' });
        });
}

function LoadInitialData() {
    fetch('/HR1/Load', {
        method: "POST",
        body: new FormData()
    })
        .then(r => r.json())
        .then(res => {
            // Load Lines
            if (res.LineList && res.LineList.length > 0) {
                LineListData = res.LineList;

                let $selectLine = $("#Line");
                $selectLine.empty();
                res.LineList.forEach(line => {
                    let text = line.LineGroup
                        ? `${line.LineGroup} - ${line.LineName} - ${line.Family}`
                        : line.LineName;
                    $selectLine.append(`<option value="${text}">${text}</option>`);
                });

                let $filterLine = $("#FilterLine");
                $filterLine.empty();
                $filterLine.append('<option value="">-- Tất cả Line --</option>');
                res.LineList.forEach(line => {
                    let text = line.LineGroup
                        ? `${line.LineGroup} - ${line.LineName} - ${line.Family}`
                        : line.LineName;
                    $filterLine.append(`<option value="${text}">${text}</option>`);
                });
            }

            // Load Shifts
            if (res.ShiftTimes && res.ShiftTimes.length > 0) {
                ShiftTimesData = res.ShiftTimes;

                // Dropdown modal
                let $selectShift = $("#DefaultShiftID");
                $selectShift.empty();
                $selectShift.append('<option value="">-- Chọn ca --</option>');
                res.ShiftTimes.forEach(shift => {
                    $selectShift.append(`<option value="${shift.ID}">${shift.ShiftName}</option>`);
                });

                // Dropdown bulk assign
                let $bulkShift = $("#BulkShiftID");
                $bulkShift.empty();
                $bulkShift.append('<option value="">-- Chọn ca để phân --</option>');
                res.ShiftTimes.forEach(shift => {
                    $bulkShift.append(`<option value="${shift.ID}">${shift.ShiftName}</option>`);
                });
            }
        })
        .catch(err => {
            console.error('Error loading data:', err);
            M.toast({ html: '❌ Lỗi tải dữ liệu: ' + err.message, classes: 'red darken-2' });
        });
}

function Buttonfind_Click() {
    let search = $("#SearchBox").val() || "";
    let filterLine = $("#FilterLine").val() || "";
    ApiCall("Buttonfind_Click", { SearchString: search })
        .then(res => {
            BaseResult = res;
            if (res.LineList && res.LineList.length > 0) {
                LineListData = res.LineList;
            }
            if (res.ShiftTimes && res.ShiftTimes.length > 0) {
                ShiftTimesData = res.ShiftTimes;
            }
            let employees = res.EmployeeFAList || [];

            if (filterLine) {
                employees = employees.filter(emp => {
                    return (emp.Line || "").trim() === filterLine.trim();
                });
            }
            DataGridView1Render(employees);
        })
        .catch(err => {
            M.toast({ html: '❌ Có lỗi xảy ra khi tìm kiếm: ' + err.message, classes: 'red darken-2' });
        });
}

function Buttonadd_Click() {
    ResetForm();
    CurrentID = null;
    $("#modal-title").text("Add Employee");
    $("#modal-add").modal("open");
}

function Buttonsave_Click() {
    if (!ValidateForm()) return;
    let data = GetFormData();

    let action = data.EmployeeFA.ID > 0 ? "Buttonsave_Click" : "Buttonadd_Click";
    ApiCall(action, data)
        .then(res => {
            if (res.Success) {
                Buttonfind_Click();
                $("#modal-add").modal("close");
                M.toast({ html: '✅ ' + (res.Message || 'Thành công'), classes: 'green darken-1' });
            } else {
                M.toast({ html: '❌ ' + (res.Error || 'Có lỗi xảy ra'), classes: 'red darken-2' });
            }
        })
        .catch(err => {
            M.toast({ html: '❌ Có lỗi xảy ra khi lưu: ' + err.message, classes: 'red darken-2' });
        });
}

function Buttondelete_Click() {
    let id = $("#DataGridView1 tr.selected").data("id");
    if (!id) {
        M.toast({ html: '⚠️ Vui lòng chọn dòng muốn xóa!', classes: 'orange darken-2' });
        return;
    }
    if (!confirm("Bạn chắc chắn muốn xóa nhân viên này?")) return;
    ApiCall("Buttondelete_Click", { ID: id })
        .then(res => {
            if (res.Success) {
                Buttonfind_Click();
                M.toast({ html: '✅ ' + (res.Message || 'Xóa thành công'), classes: 'green darken-1' });
            } else {
                M.toast({ html: '❌ ' + (res.Error || 'Có lỗi xảy ra'), classes: 'red darken-2' });
            }
        })
        .catch(err => {
            M.toast({ html: '❌ Có lỗi xảy ra khi xóa: ' + err.message, classes: 'red darken-2' });
        });
}

function Buttoncancel_Click() {
    ResetForm();
    $("#modal-add").modal("close");
}

function Buttonprint_Click() {
    let selectedEmployees = [];
    $(".employee-checkbox:checked").each(function () {
        selectedEmployees.push({
            empCode: $(this).data("empcode") + "",
            name: $(this).data("name") + ""
        });
    });

    if (selectedEmployees.length === 0) {
        M.toast({ html: '⚠️ Vui lòng chọn ít nhất một nhân viên để in QR Code!', classes: 'orange darken-2' });
        return;
    }

    let printWindow = window.open('', '_blank', 'width=900,height=700');

    let htmlContent = `
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title>Print QR Codes</title>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/qrcodejs/1.0.0/qrcode.min.js"></script>
    <style>
        * { margin: 0; padding: 0; box-sizing: border-box; }
        body { font-family: Arial, sans-serif; padding: 20px; }
        .qr-container { 
            display: flex; 
            flex-wrap: wrap; 
            gap: 15px; 
        }
        .qr-item { 
            width: 100px; 
            text-align: center; 
            page-break-inside: avoid; 
        }
        .qr-code { 
            width: 80px; 
            height: 80px; 
            margin: 0 auto 4px; 
        }
        .emp-name { 
            font-size: 9px; 
            font-weight: bold; 
            margin-bottom: 2px; 
        }
        .emp-code { 
            font-size: 10px; 
            color: #2196F3; 
            font-weight: bold; 
        }
        .no-print { 
            text-align: center; 
            margin-bottom: 20px; 
        }
        .btn { 
            padding: 10px 20px; 
            font-size: 16px; 
            cursor: pointer; 
            border: none; 
            border-radius: 4px; 
            color: white; 
            margin: 0 5px;
        }
        .btn-print { background: #4CAF50; }
        .btn-close { background: #f44336; }
        @media print {
            .no-print { display: none; }
        }
    </style>
</head>
<body>
    <div class="no-print">
        <h2>QR Codes - Employee FA (${selectedEmployees.length} nhân viên)</h2>
        <button class="btn btn-print" onclick="window.print()">🖨️ Print</button>
        <button class="btn btn-close" onclick="window.close()">✖ Close</button>
    </div>
    <div class="qr-container" id="container"></div>
    
    <script>
        const employees = ${JSON.stringify(selectedEmployees)};
        const container = document.getElementById('container');
        
        employees.forEach((emp, i) => {
            const item = document.createElement('div');
            item.className = 'qr-item';
            
            const qrDiv = document.createElement('div');
            qrDiv.className = 'qr-code';
            qrDiv.id = 'qr' + i;
            
            const name = document.createElement('div');
            name.className = 'emp-name';
            name.textContent = emp.name;
            
            const code = document.createElement('div');
            code.className = 'emp-code';
            code.textContent = emp.empCode;
            
            item.appendChild(qrDiv);
            item.appendChild(name);
            item.appendChild(code);
            container.appendChild(item);
            
            new QRCode(qrDiv, {
                text: emp.empCode + '$',
                width: 80,
                height: 80
            });
        });
    </script>
</body>
</html>`;
    printWindow.document.open();
    printWindow.document.write(htmlContent);
    printWindow.document.close();
}

function Buttoninport_Click() {
    ImportedData = [];
    $("#ImportPreviewTableHR1").html('<tr><td colspan="5" class="center-align grey-text">Chưa có dữ liệu</td></tr>');
    $("#BtnSaveImportHR1").prop("disabled", true);
    $("#ImportCount").text("0");
    $("#FileImportHR1").val("");
    $("#ModalImportHR1").modal("open");
}

function Buttonexport_Click() {
    TableHTMLToExcel('DataGridView1', 'EmployeeFA.xls', 'EmployeeFA');
}

function Buttonhelp_Click() {
    window.open("/WMP_PLAY", "_blank", "width=800,height=460");
}

function Buttonclose_Click() {
    history.back();
}

function DataGridView1Render(data) {
    let $tbody = $("#DataGridView1 tbody");
    $tbody.empty();
    if (!data || !data.length) {
        $tbody.append('<tr><td colspan="14" class="center-align">Không có dữ liệu</td></tr>');
        $("#TotalRecordsHR1").text("0");
        return;
    }
    data.forEach(item => {
        let lineInfo = LineListData.find(l => l.LineName === item.Line);
        let lineDisplay = item.Line ?? "";
        if (lineInfo && lineInfo.LineGroup && lineInfo.Family) {
            lineDisplay = `${lineInfo.LineGroup} - ${lineInfo.LineName} - ${lineInfo.Family}`;
        } else if (lineInfo) {
            lineDisplay = lineInfo.LineName;
        }
        let shiftInfo = ShiftTimesData.find(s => s.ID === item.DefaultShiftID);
        let shiftDisplay = shiftInfo ? shiftInfo.ShiftName : "";

        let hireDateDisplay = "";
        if (item.HireDate) {
            let date = new Date(item.HireDate);
            hireDateDisplay = date.toLocaleDateString('vi-VN');
        }

        $tbody.append(`
            <tr data-id="${item.ID}">
                <td>
                    <label>
                        <input type="checkbox" class="employee-checkbox"
                               data-empcode="${item.EmpCode}"
                               data-name="${item.Name}" />
                        <span></span>
                    </label>
                </td>
                <td>
                    <button class="btn btn-small btn-edit">
                        <i class="material-icons">edit</i>
                    </button>
                </td>
                <td>${item.ID ?? ""}</td>
                <td>${item.EmpCode ?? ""}</td>
                <td>${item.Name ?? ""}</td>
                <td>${item.Dept ?? ""}</td>
                <td>${lineDisplay}</td>
                <td>${item.Process ?? ""}</td>
                <td>${shiftDisplay}</td>
                <td>${hireDateDisplay}</td>
                <td>${formatDateTime(item.CreateDate)}</td>
                <td>${item.CreateUser ?? ""}</td>
                <td>${formatDateTime(item.UpdateDate)}</td>
                <td>${item.UpdateUser ?? ""}</td>
            </tr>
        `);
    });
    $("#TotalRecordsHR1").text(data.length);
}

function FillForm(data) {
    $("#EmployeeID").val(data.ID ?? "");
    $("#EmpCode").val(data.EmpCode ?? "");
    $("#Name").val(data.Name ?? "");
    $("#Dept").val(data.Dept ?? "");
    $("#Process").val(data.Process ?? "");
    $("#DefaultShiftID").val(data.DefaultShiftID ?? "");
    if (data.HireDate) {
        let date = new Date(data.HireDate);
        let formatted = date.toISOString().split('T')[0];
        $("#HireDate").val(formatted);
    } else {
        $("#HireDate").val("");
    }

    const lineFromDB = (data.Line ?? "").trim();
    $("#Line").val(lineFromDB);
}

function ResetForm() {
    $("#EmployeeFAForm")[0].reset();
    $("#EmployeeID").val("");
    $("#DefaultShiftID").val("");
    $("#HireDate").val("");
}

function ValidateForm() {
    let errors = [];
    if (!$("#EmpCode").val().trim()) {
        errors.push("Employee Code không được để trống");
    }
    if (!$("#Name").val().trim()) {
        errors.push("Name không được để trống");
    }
    if (!$("#Dept").val()) {
        errors.push("Department không được để trống");
    }
    if (!$("#Line").val()) {
        errors.push("Line không được để trống");
    }
    if (!$("#DefaultShiftID").val()) {
        errors.push("Shift không được để trống");
    }
    if (errors.length > 0) {
        M.toast({ html: '⚠️ ' + errors.join('<br>'), classes: 'orange darken-2', displayLength: 4000 });
        return false;
    }
    return true;
}

function GetFormData() {
    let defaultShiftID = $("#DefaultShiftID").val();
    const selectedLineName = $("#Line").val();
    const selectedLineText = $("#Line option:selected").text();
    const lineToSave = selectedLineText;

    let hireDate = $("#HireDate").val();

    return {
        EmployeeFA: {
            ID: parseInt($("#EmployeeID").val()) || 0,
            EmpCode: $("#EmpCode").val().trim(),
            Name: $("#Name").val().trim(),
            Dept: $("#Dept").val(),
            Line: lineToSave,
            Process: $("#Process").val() || null,
            DefaultShiftID: defaultShiftID ? parseInt(defaultShiftID) : null,
            HireDate: hireDate || null
        },
        USER_IDX: getCurrentUser()
    };
}

function HandleFileSelect(e) {
    let file = e.target.files[0];
    if (!file) return;
    $("#importProgressHR1").show();
    let reader = new FileReader();
    reader.onload = function (e) {
        try {
            let data = new Uint8Array(e.target.result);
            let workbook = XLSX.read(data, { type: 'array' });
            let firstSheet = workbook.Sheets[workbook.SheetNames[0]];
            let jsonData = XLSX.utils.sheet_to_json(firstSheet);
            ImportedData = ProcessExcelData(jsonData);
            RenderImportPreview(ImportedData);
            $("#importProgressHR1").hide();
        } catch (err) {
            M.toast({ html: '❌ Không thể đọc file Excel: ' + err.message, classes: 'red darken-2' });
            $("#importProgressHR1").hide();
        }
    };
    reader.readAsArrayBuffer(file);
}

function ProcessExcelData(jsonData) {
    let employees = [];
    if (!jsonData || jsonData.length === 0) {
        M.toast({ html: '⚠️ File Excel không có dữ liệu!', classes: 'orange darken-2' });
        return employees;
    }
    jsonData.forEach(row => {
        let name = row['Họ & Tên'] ? row['Họ & Tên'].toString().trim() : "";
        let dept = row['Bộ phận'] ? row['Bộ phận'].toString().trim() : "";
        let line = row['Line'] ? row['Line'].toString().trim() : "";
        let process = row['Công đoạn'] ? row['Công đoạn'].toString().trim() : "";
        let empCode = row['Mã số nhân viên'] ? row['Mã số nhân viên'].toString().trim() : "";

        let hireDate = null;
        if (row['Ngày nhận việc']) {
            let rawDate = row['Ngày nhận việc'];
            if (rawDate instanceof Date) {
                hireDate = rawDate.toISOString().split('T')[0];
            } else if (typeof rawDate === 'string') {
                let parts = rawDate.split(/[\/\-]/);
                if (parts.length === 3) {
                    let d = new Date(parts[2], parts[1] - 1, parts[0]);
                    if (!isNaN(d.getTime())) {
                        hireDate = d.toISOString().split('T')[0];
                    }
                }
            } else if (typeof rawDate === 'number') {
                let d = new Date((rawDate - 25569) * 86400 * 1000);
                hireDate = d.toISOString().split('T')[0];
            }
        }
        if (!hireDate && empCode) {
            hireDate = parseHireDateFromEmpCode(empCode);
        }

        if (!name || !dept || !line || !empCode) {
            return;
        }
        employees.push({
            Name: name,
            Dept: dept,
            Line: line,
            Process: process || null,
            EmpCode: empCode,
            DefaultShiftID: null,
            HireDate: hireDate
        });
    });
    return employees;
}

function RenderImportPreview(data) {
    let $tbody = $("#ImportPreviewTableHR1");
    $tbody.empty();
    if (!data || data.length === 0) {
        $tbody.append('<tr><td colspan="6" class="center-align red-text">Không có dữ liệu hợp lệ!</td></tr>');
        $("#BtnSaveImportHR1").prop("disabled", true);
        $("#ImportCount").text("0");
        return;
    }
    data.forEach(item => {
        let hireDateDisplay = "";
        if (item.HireDate) {
            let date = new Date(item.HireDate);
            hireDateDisplay = date.toLocaleDateString('vi-VN');
        }

        $tbody.append(`
            <tr>
                <td>${item.Name ?? ""}</td>
                <td>${item.Dept ?? ""}</td>
                <td>${item.Line ?? ""}</td>
                <td>${item.Process ?? ""}</td>
                <td>${item.EmpCode ?? ""}</td>
                <td>${hireDateDisplay}</td> 
            </tr>
        `);
    });
    $("#BtnSaveImportHR1").prop("disabled", false);
    $("#ImportCount").text(data.length);
}

function SaveImport() {
    if (!ImportedData || ImportedData.length === 0) {
        M.toast({ html: '⚠️ Không có dữ liệu để import!', classes: 'orange darken-2' });
        return;
    }
    if (!confirm(`Bạn chắc chắn muốn import ${ImportedData.length} nhân viên?`)) return;
    ApiCall("Buttoninport_Click", { EmployeeFAList: ImportedData })
        .then(res => {
            if (res.Success) {
                M.toast({ html: '✅ ' + (res.Message || 'Import thành công!'), classes: 'green darken-1' });
                $("#ModalImportHR1").modal("close");
                Buttonfind_Click();
            } else {
                M.toast({ html: '❌ ' + (res.Error || 'Có lỗi xảy ra khi import!'), classes: 'red darken-2' });
            }
        })
        .catch(err => {
            M.toast({ html: '❌ Có lỗi xảy ra khi import: ' + err.message, classes: 'red darken-2' });
        });
}