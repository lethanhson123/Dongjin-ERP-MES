let CurrentID = null;
let BaseResult = {};
let LineListData = [];
let ImportedData = [];

function GetCookieValue(name) {
    const match = document.cookie.match(new RegExp('(^| )' + name + '=([^;]+)'));
    return match ? match[2] : null;
}

function getCurrentUser() {
    return GetCookieValue('UserID') || 'SYSTEM';
}

$(document).ready(function () {
    $('.modal').modal();
    LoadLines();
    Buttonfind_Click();

    $("#Buttonfind").click(Buttonfind_Click);
    $("#Buttonadd").click(Buttonadd_Click);
    $("#Buttonsave").click(Buttonsave_Click);
    $("#Buttondelete").click(Buttondelete_Click);
    $("#Buttoncancel").click(Buttoncancel_Click);
    $("#Buttonexport").click(Buttonexport_Click);
    $("#Buttonprint").click(Buttonprint_Click);
    $("#Buttonhelp").click(Buttonhelp_Click);
    $("#Buttonclose").click(Buttonclose_Click);

    $("#DataGridView1").on("click", ".btn-edit", function () {
        let id = $(this).closest("tr").data("id");
        EditRow_Click(id);
    });

    $("#DataGridView1").on("click", "tr", function () {
        $("#DataGridView1 tr.selected").removeClass("selected");
        $(this).addClass("selected");
    });

    $("#ModalSaveBtn").click(Buttonsave_Click);

    $("#SearchBox").on("keypress", function (e) {
        if (e.which === 13) Buttonfind_Click();
    });
    $("#FilterLineID").on("change", function () {
        Buttonfind_Click();
    });

    $("#Buttoninport").click(Buttoninport_Click);
    $("#FileImportP04").on("change", HandleFileSelect);
    $("#BtnSaveImportP04").click(SaveImport);
});

function ShowLoading(flag = true) {
    $("#BackGround").css("display", flag ? "block" : "none");
}

function ApiCall(action, param = {}) {
    ShowLoading(true);
    if (!param.USER_IDX) param.USER_IDX = getCurrentUser();

    let formData = new FormData();
    formData.append("BaseParameter", JSON.stringify(param));
    return fetch(`/P04/${action}`, { method: "POST", body: formData })
        .then(r => r.json())
        .finally(() => ShowLoading(false));
}

function LoadLines() {
    fetch('/P04/Load', { method: "POST", body: new FormData() })
        .then(r => r.json())
        .then(res => {
            if (res.LineList && res.LineList.length > 0) {
                LineListData = res.LineList;
                let $selectModal = $("#LineID");
                $selectModal.empty().append('<option value="">-- Select Line --</option>');
                let $selectFilter = $("#FilterLineID");
                $selectFilter.empty().append('<option value="">-- Tất cả Line --</option>');

                res.LineList.forEach(line => {
                    let text = line.LineGroup ? `${line.LineGroup} - ${line.LineName} - ${line.Family}` : line.LineName;
                    $selectModal.append(`<option value="${line.ID}">${text}</option>`);
                    $selectFilter.append(`<option value="${line.ID}">${text}</option>`);
                });
            }
        })
        .catch(err => console.error("Error loading lines:", err));
}


function Buttonfind_Click() {
    let search = $("#SearchBox").val() || "";
    let filterLineID = $("#FilterLineID").val() || "";

    ApiCall("Buttonfind_Click", {
        SearchString: search,
        ID: filterLineID ? parseInt(filterLineID) : null  
    })
        .then(res => {
            BaseResult = res;
            if (res.LineList && res.LineList.length > 0) LineListData = res.LineList;
            DataGridView1Render(res.TaskTimeFAList || []);
        })
        .catch(err => {
            console.error("Error in search:", err);
            alert("Có lỗi xảy ra khi tìm kiếm: " + err.message);
        });
}

function DataGridView1Render(data) {
    let $tbody = $("#DataGridView1 tbody");
    $tbody.empty();

    if (!data || !data.length) {
        $tbody.append('<tr><td colspan="15" class="center-align">Không có dữ liệu</td></tr>'); 
        $("#TotalRecords").text("0");
        return;
    }

    data.forEach(item => {
        let lineName = getLineName(item.LineID);
        let gap = calculateGap(item); 
        $tbody.append(`
            <tr data-id="${item.ID}">
                <td><button class="btn btn-small btn-edit"><i class="material-icons">edit</i></button></td>
                <td>${item.ID ?? ""}</td>
                <td>${lineName}</td>
                <td>${item.PartNo ?? ""}</td>
                <td>${item.ECN ?? ""}</td>
                <td>${item.TaskTimeCus ?? ""}</td>
                <td>${item.TaskTimeIE ?? ""}</td>
                <td>${item.TaskTimeIE2 ?? ""}</td>
                <td>${item.TaskTimeIE3 ?? ""}</td>
                <td>${item.TaskTimeIE4 ?? ""}</td>
                <td class="right-align">${gap}</td>  <!-- ✅ Thêm cột Gap (căn phải) -->
                <td>${formatDateTime(item.CreateDate)}</td>
                <td>${item.CreateUserName ?? ""}</td>
                <td>${formatDateTime(item.UpdateDate)}</td>
                <td>${item.UpdateUserName ?? ""}</td>
            </tr>
        `);
    });
    $("#TotalRecords").text(data.length);
}
function calculateGap(item) {
    let ttIE = item.TaskTimeIE2 || item.TaskTimeIE3 || item.TaskTimeIE4 || item.TaskTimeIE;
    let ttCus = item.TaskTimeCus;
    if (!ttIE || !ttCus || ttCus === 0) {
        return "";
    }
    let gap = (1 - (ttIE / ttCus)) * 100;
    return gap.toFixed(2) + "%";
}
function getLineName(lineId) {
    if (!lineId || !LineListData || LineListData.length === 0) return "";
    let line = LineListData.find(x => x.ID == lineId);
    if (!line) return "";
    return line.LineGroup ? `${line.LineGroup} - ${line.LineName} - ${line.Family}` : line.LineName;
}

function formatDateTime(dateTimeStr) {
    if (!dateTimeStr) return "";
    const date = typeof dateTimeStr === 'string' ? new Date(dateTimeStr) : dateTimeStr;
    if (isNaN(date.getTime())) return dateTimeStr.substring(0, 16).replace('T', ' ');
    return date.toISOString().substring(0, 16).replace('T', ' ');
}

function Buttonadd_Click() {
    ResetForm();
    CurrentID = null;
    $("#modal-title").text("Add Task Time");
    $("#modal-add").modal("open");
}

function EditRow_Click(id) {
    let data = (BaseResult.TaskTimeFAList || []).find(x => x.ID == id);
    if (!data) return;
    FillForm(data);
    CurrentID = id;
    $("#modal-title").text("Edit Task Time");
    $("#modal-add").modal("open");
}

// ✅ UPDATE: Thêm fill 3 fields mới
function FillForm(data) {
    $("#TaskTimeID").val(data.ID ?? "");
    $("#LineID").val(data.LineID ?? "");
    $("#PartNo").val(data.PartNo ?? "");
    $("#ECN").val(data.ECN ?? "");
    $("#TaskTimeIE").val(data.TaskTimeIE ?? "");
    $("#TaskTimeIE2").val(data.TaskTimeIE2 ?? "");  // ✅ Thêm
    $("#TaskTimeIE3").val(data.TaskTimeIE3 ?? "");  // ✅ Thêm
    $("#TaskTimeIE4").val(data.TaskTimeIE4 ?? "");  // ✅ Thêm
    $("#TaskTimeCus").val(data.TaskTimeCus ?? "");
}

function ResetForm() {
    $("#TaskTimeFAForm")[0].reset();
    $("#TaskTimeID").val("");
}

function ValidateForm() {
    let errors = [];
    if (!$("#LineID").val()) errors.push("Line không được để trống");
    if (!$("#PartNo").val().trim()) errors.push("Part No không được để trống");

    if (errors.length > 0) {
        alert(errors.join("\n"));
        return false;
    }
    return true;
}

// ✅ UPDATE: Thêm 3 fields mới vào data
function GetFormData() {
    return {
        TaskTimeFA: {
            ID: parseInt($("#TaskTimeID").val()) || 0,
            LineID: parseInt($("#LineID").val()) || null,
            PartNo: $("#PartNo").val(),
            ECN: $("#ECN").val(),
            TaskTimeIE: parseFloat($("#TaskTimeIE").val()) || null,
            TaskTimeIE2: parseFloat($("#TaskTimeIE2").val()) || null,  // ✅ Thêm
            TaskTimeIE3: parseFloat($("#TaskTimeIE3").val()) || null,  // ✅ Thêm
            TaskTimeIE4: parseFloat($("#TaskTimeIE4").val()) || null,  // ✅ Thêm
            TaskTimeCus: parseFloat($("#TaskTimeCus").val()) || null
        },
        USER_IDX: getCurrentUser()
    };
}

function Buttonsave_Click() {
    if (!ValidateForm()) return;
    let data = GetFormData();
    let action = data.TaskTimeFA.ID > 0 ? "Buttonsave_Click" : "Buttonadd_Click";

    ApiCall(action, data)
        .then(res => {
            if (res.Success) {
                Buttonfind_Click();
                $("#modal-add").modal("close");
                alert(res.Message || "Thành công");
            } else {
                alert(res.Error || "Có lỗi xảy ra");
            }
        })
        .catch(err => {
            console.error("Error in save:", err);
            alert("Có lỗi xảy ra khi lưu: " + err.message);
        });
}

function Buttondelete_Click() {
    let id = $("#DataGridView1 tr.selected").data("id");
    if (!id) {
        alert("Vui lòng chọn dòng muốn xóa!");
        return;
    }
    if (!confirm("Bạn chắc chắn muốn xóa?")) return;

    ApiCall("Buttondelete_Click", { ID: id })
        .then(res => {
            if (res.Success) {
                Buttonfind_Click();
                alert(res.Message || "Xóa thành công");
            } else {
                alert(res.Error || "Có lỗi xảy ra");
            }
        })
        .catch(err => {
            console.error("Error in delete:", err);
            alert("Có lỗi xảy ra khi xóa: " + err.message);
        });
}

function Buttoninport_Click() {
    ImportedData = [];
    $("#ImportPreviewTableP04").html('<tr><td colspan="8" class="center-align grey-text">Chưa có dữ liệu. Vui lòng chọn file Excel.</td></tr>');  // ✅ Sửa colspan từ 5 thành 8
    $("#ImportSummary").hide();
    $("#BtnSaveImportP04").prop("disabled", true);
    $("#ImportCount").text("0");
    $("#FileImportP04").val("");
    $("#ModalImportP04").modal("open");
}

function HandleFileSelect(e) {
    let file = e.target.files[0];
    if (!file) return;

    $("#importProgressP04").show();

    let reader = new FileReader();
    reader.onload = function (e) {
        try {
            let data = new Uint8Array(e.target.result);
            let workbook = XLSX.read(data, { type: 'array' });
            let firstSheet = workbook.Sheets[workbook.SheetNames[0]];
            let jsonData = XLSX.utils.sheet_to_json(firstSheet);

            ImportedData = ProcessExcelData(jsonData);
            RenderImportPreview(ImportedData);
            $("#importProgressP04").hide();
        } catch (err) {
            console.error("Error reading file:", err);
            alert("Không thể đọc file Excel: " + err.message);
            $("#importProgressP04").hide();
        }
    };
    reader.readAsArrayBuffer(file);
}

// ✅ UPDATE: Đọc thêm 3 cột TaskTimeIE2, TaskTimeIE3, TaskTimeIE4 từ Excel
function ProcessExcelData(jsonData) {
    let taskTimes = [];

    if (!jsonData || jsonData.length === 0) {
        alert("File Excel không có dữ liệu!");
        return taskTimes;
    }

    jsonData.forEach(row => {
        let lineStr = row['Line'] ? row['Line'].toString().trim() : "";
        let partNo = row['Part No'] ? row['Part No'].toString().trim() : "";
        let ecn = row['ECN'] ? row['ECN'].toString().trim() : "";
        let taskTimeIE = row['Task Time IE'] ? parseFloat(row['Task Time IE']) : null;
        let taskTimeIE2 = row['Task Time IE2'] ? parseFloat(row['Task Time IE2']) : null;  // ✅ Thêm
        let taskTimeIE3 = row['Task Time IE3'] ? parseFloat(row['Task Time IE3']) : null;  // ✅ Thêm
        let taskTimeIE4 = row['Task Time IE4'] ? parseFloat(row['Task Time IE4']) : null;  // ✅ Thêm
        let taskTimeCus = row['Task Time Cus'] ? parseFloat(row['Task Time Cus']) : null;

        if (!partNo || !lineStr) return;

        let lineId = FindLineIDByName(lineStr);
        if (!lineId) {
            console.warn(`Line không tìm thấy: ${lineStr}`);
            return;
        }

        taskTimes.push({
            LineID: lineId,
            LineName: lineStr,
            PartNo: partNo,
            ECN: ecn,
            TaskTimeIE: taskTimeIE,
            TaskTimeIE2: taskTimeIE2,  // ✅ Thêm
            TaskTimeIE3: taskTimeIE3,  // ✅ Thêm
            TaskTimeIE4: taskTimeIE4,  // ✅ Thêm
            TaskTimeCus: taskTimeCus
        });
    });

    return taskTimes;
}

function FindLineIDByName(lineName) {
    if (!lineName || !LineListData || LineListData.length === 0) return null;

    let line = LineListData.find(x =>
        x.LineName === lineName ||
        (x.LineGroup && x.Family && `${x.LineGroup} - ${x.LineName} - ${x.Family}` === lineName)
    );

    return line ? line.ID : null;
}

// ✅ REORDERED: Render thêm 3 cột TaskTimeIE2, TaskTimeIE3, TaskTimeIE4 (with Cus moved)
function RenderImportPreview(data) {
    let $tbody = $("#ImportPreviewTableP04");
    $tbody.empty();

    if (!data || data.length === 0) {
        $tbody.append('<tr><td colspan="8" class="center-align red-text">Không có dữ liệu hợp lệ!</td></tr>'); 
        $("#BtnSaveImportP04").prop("disabled", true);
        $("#ImportCount").text("0");
        return;
    }

    data.forEach(item => {
        $tbody.append(`
            <tr>
                <td>${item.LineName ?? ""}</td>
                <td>${item.PartNo ?? ""}</td>
                <td>${item.ECN ?? ""}</td>
                <td class="right-align">${item.TaskTimeCus ?? ""}</td>  <!-- ✅ REORDERED: Moved here -->
                <td class="right-align">${item.TaskTimeIE ?? ""}</td>
                <td class="right-align">${item.TaskTimeIE2 ?? ""}</td>
                <td class="right-align">${item.TaskTimeIE3 ?? ""}</td>
                <td class="right-align">${item.TaskTimeIE4 ?? ""}</td>
            </tr>
        `);
    });

    $("#BtnSaveImportP04").prop("disabled", false);
    $("#ImportCount").text(data.length);
}

// ✅ UPDATE: Gửi thêm 3 fields mới khi import
function SaveImport() {
    if (!ImportedData || ImportedData.length === 0) {
        alert("Không có dữ liệu để import!");
        return;
    }

    if (!confirm(`Bạn chắc chắn muốn import ${ImportedData.length} records?`)) return;

    let cleanData = ImportedData.map(item => ({
        LineID: item.LineID,
        PartNo: item.PartNo,
        ECN: item.ECN,
        TaskTimeIE: item.TaskTimeIE,
        TaskTimeIE2: item.TaskTimeIE2,  // ✅ Thêm
        TaskTimeIE3: item.TaskTimeIE3,  // ✅ Thêm
        TaskTimeIE4: item.TaskTimeIE4,  // ✅ Thêm
        TaskTimeCus: item.TaskTimeCus
    }));

    ApiCall("Buttoninport_Click", { TaskTimeFAList: cleanData })
        .then(res => {
            if (res.Success) {
                alert(res.Message || "Import thành công!");
                $("#ModalImportP04").modal("close");
                Buttonfind_Click();
            } else {
                alert(res.Error || "Có lỗi xảy ra khi import!");
            }
        })
        .catch(err => {
            console.error("Error in import:", err);
            alert("Có lỗi xảy ra khi import: " + err.message);
        });
}

function Buttoncancel_Click() {
    ResetForm();
    $("#modal-add").modal("close");
}

function Buttonexport_Click() {
    TableHTMLToExcel('DataGridView1', 'TaskTimeFA.xls', 'TaskTimeFA');
}

function Buttonprint_Click() {
    window.open(`/P04/Buttonprint_Click`, "_blank");
}

function Buttonhelp_Click() {
    window.open("/WMP_PLAY", "_blank", "width=800,height=460");
}

function Buttonclose_Click() {
    history.back();
}