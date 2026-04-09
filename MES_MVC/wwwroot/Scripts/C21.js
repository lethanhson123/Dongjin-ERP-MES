let CurrentID = null;
let BaseResult = {};
let ImportedData = [];

// ============================================
// UTILITY FUNCTIONS
// ============================================
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
    return fetch(`/C21/${action}`, { method: "POST", body: formData })
        .then(r => r.json())
        .finally(() => ShowLoading(false));
}

function formatDateTime(dateTimeStr) {
    if (!dateTimeStr) return "";
    const date = new Date(dateTimeStr);
    if (isNaN(date.getTime())) return "";
    return date.toISOString().substring(0, 16).replace('T', ' ');
}

// ============================================
// DOCUMENT READY
// ============================================
$(document).ready(function () {
    $('.modal').modal();
    Buttonfind_Click();

    // Button Events
    $("#Buttonfind").click(Buttonfind_Click);
    $("#Buttonadd").click(Buttonadd_Click);
    $("#Buttonsave, #ModalSaveBtn").click(Buttonsave_Click);
    $("#Buttondelete").click(Buttondelete_Click);
    $("#Buttoncancel").click(Buttoncancel_Click);
    $("#Buttoninport").click(Buttoninport_Click);
    $("#Buttonexport").click(Buttonexport_Click);
    $("#Buttonprint").click(Buttonprint_Click);
    $("#Buttonhelp").click(Buttonhelp_Click);
    $("#Buttonclose").click(Buttonclose_Click);

    // Table Events
    $("#DataGridView1").on("click", ".btn-edit", function () {
        EditRow_Click($(this).closest("tr").data("id"));
    });

    $("#DataGridView1").on("click", "tr", function () {
        $("#DataGridView1 tr.selected").removeClass("selected");
        $(this).addClass("selected");
    });

    // Search Events
    $("#SearchBox").on("keypress", function (e) {
        if (e.which === 13) Buttonfind_Click();
    });

    // Import Events
    $("#FileImportC21").on("change", HandleFileSelect);
    $("#BtnSaveImportC21").click(SaveImport);

    // Checkbox Events
    $("#checkAll").on("change", function () {
        $(".trolley-checkbox").prop("checked", $(this).is(":checked"));
    });

    $("#DataGridView1").on("change", ".trolley-checkbox", function () {
        if (!$(this).is(":checked")) {
            $("#checkAll").prop("checked", false);
        } else {
            $("#checkAll").prop("checked", $(".trolley-checkbox").length === $(".trolley-checkbox:checked").length);
        }
    });
});

// ============================================
// CRUD OPERATIONS
// ============================================
function Buttonfind_Click() {
    ApiCall("Buttonfind_Click", { SearchString: $("#SearchBox").val() || "" })
        .then(res => {
            BaseResult = res;
            DataGridView1Render(res.TrolleyList || []);
        })
        .catch(err => alert("Lỗi tìm kiếm: " + err.message));
}

function DataGridView1Render(data) {
    let $tbody = $("#DataGridView1 tbody");
    $tbody.empty();

    if (!data || !data.length) {
        $tbody.append('<tr><td colspan="9" class="center-align">Không có dữ liệu</td></tr>');
        $("#TotalRecords").text("0");
        return;
    }

    data.forEach(item => {
        $tbody.append(`
            <tr data-id="${item.ID}">
                <td>
                    <label>
                        <input type="checkbox" class="trolley-checkbox"
                               data-trolleycode="${item.TrolleyCode}"
                               data-location="${item.Location}" />
                        <span></span>
                    </label>
                </td>
                <td><button class="btn btn-small btn-edit"><i class="material-icons">edit</i></button></td>
                <td>${item.ID ?? ""}</td>
                <td>${item.TrolleyCode ?? ""}</td>
                <td>${item.Location ?? ""}</td>
                <td>${formatDateTime(item.CreatedDate)}</td>
                <td>${item.CreatedBy ?? ""}</td>
                <td>${formatDateTime(item.UpdateDate)}</td>
                <td>${item.UpdateBy ?? ""}</td>
            </tr>
        `);
    });
    $("#TotalRecords").text(data.length);
    $("#checkAll").prop("checked", false);
}

function Buttonadd_Click() {
    ResetForm();
    CurrentID = null;
    $("#modal-title").text("Add Trolley");
    $("#modal-add").modal("open");
}

function EditRow_Click(id) {
    let data = (BaseResult.TrolleyList || []).find(x => x.ID == id);
    if (!data) return;
    
    $("#TrolleyID").val(data.ID);
    $("#TrolleyCode").val(data.TrolleyCode);
    $("#Location").val(data.Location);
    
    CurrentID = id;
    $("#modal-title").text("Edit Trolley");
    $("#modal-add").modal("open");
}

function Buttonsave_Click() {
    if (!$("#TrolleyCode").val().trim() || !$("#Location").val().trim()) {
        alert("Trolley Code và Location không được để trống!");
        return;
    }

    let data = {
        Trolley: {
            ID: parseInt($("#TrolleyID").val()) || 0,
            TrolleyCode: $("#TrolleyCode").val().trim(),
            Location: $("#Location").val().trim()
        }
    };

    let action = data.Trolley.ID > 0 ? "Buttonsave_Click" : "Buttonadd_Click";

    ApiCall(action, data)
        .then(res => {
            if (res.Success) {
                alert(res.Message || "Thành công");
                $("#modal-add").modal("close");
                Buttonfind_Click();
            } else {
                alert(res.Error || "Có lỗi xảy ra");
            }
        })
        .catch(err => alert("Lỗi lưu: " + err.message));
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
                alert(res.Message || "Xóa thành công");
                Buttonfind_Click();
            } else {
                alert(res.Error || "Có lỗi xảy ra");
            }
        })
        .catch(err => alert("Lỗi xóa: " + err.message));
}

function Buttoncancel_Click() {
    ResetForm();
    $("#modal-add").modal("close");
}

function ResetForm() {
    $("#TrolleyForm")[0].reset();
    $("#TrolleyID").val("");
}

// ============================================
// IMPORT EXCEL
// ============================================
function Buttoninport_Click() {
    ImportedData = [];
    $("#ImportPreviewTableC21").html('<tr><td colspan="2" class="center-align grey-text">Chưa có dữ liệu. Vui lòng chọn file Excel.</td></tr>');
    $("#BtnSaveImportC21").prop("disabled", true);
    $("#ImportCount").text("0");
    $("#FileImportC21").val("");
    $("#ModalImportC21").modal("open");
}

function HandleFileSelect(e) {
    let file = e.target.files[0];
    if (!file) return;

    $("#importProgressC21").show();

    let reader = new FileReader();
    reader.onload = function (e) {
        try {
            let data = new Uint8Array(e.target.result);
            let workbook = XLSX.read(data, { type: 'array' });
            let firstSheet = workbook.Sheets[workbook.SheetNames[0]];
            let jsonData = XLSX.utils.sheet_to_json(firstSheet);

            ImportedData = ProcessExcelData(jsonData);
            RenderImportPreview(ImportedData);
        } catch (err) {
            alert("Không thể đọc file Excel: " + err.message);
        } finally {
            $("#importProgressC21").hide();
        }
    };
    reader.readAsArrayBuffer(file);
}

function ProcessExcelData(jsonData) {
    if (!jsonData || jsonData.length === 0) {
        alert("File Excel không có dữ liệu!");
        return [];
    }

    return jsonData.map(row => ({
        TrolleyCode: (row['Trolley Code'] || "").toString().trim(),
        Location: (row['Location'] || "").toString().trim()
    })).filter(item => item.TrolleyCode && item.Location);
}

function RenderImportPreview(data) {
    let $tbody = $("#ImportPreviewTableC21");
    $tbody.empty();

    if (!data || data.length === 0) {
        $tbody.append('<tr><td colspan="2" class="center-align red-text">Không có dữ liệu hợp lệ!</td></tr>');
        $("#BtnSaveImportC21").prop("disabled", true);
        $("#ImportCount").text("0");
        return;
    }

    data.forEach(item => {
        $tbody.append(`
            <tr>
                <td>${item.TrolleyCode}</td>
                <td>${item.Location}</td>
            </tr>
        `);
    });

    $("#BtnSaveImportC21").prop("disabled", false);
    $("#ImportCount").text(data.length);
}

function SaveImport() {
    if (!ImportedData || ImportedData.length === 0) {
        alert("Không có dữ liệu để import!");
        return;
    }

    if (!confirm(`Bạn chắc chắn muốn import ${ImportedData.length} records?`)) return;

    ApiCall("Buttoninport_Click", { TrolleyList: ImportedData })
        .then(res => {
            if (res.Success) {
                alert(res.Message || "Import thành công!");
                $("#ModalImportC21").modal("close");
                Buttonfind_Click();
            } else {
                alert(res.Error || "Có lỗi xảy ra khi import!");
            }
        })
        .catch(err => alert("Lỗi import: " + err.message));
}

// ============================================
// PRINT QR CODE - SIMPLE VERSION
// ============================================
function Buttonprint_Click() {
    let selected = [];

    $(".trolley-checkbox:checked").each(function () {
        let trolleyCode = $(this).data("trolleycode");
        let displayNumber = String(trolleyCode).replace(/trolley\s*/gi, '').trim();

        selected.push({
            qrContent: trolleyCode,
            displayText: displayNumber
        });
    });

    if (selected.length === 0) {
        alert('⚠️ Vui lòng chọn ít nhất một Trolley để in QR Code!');
        return;
    }

    let win = window.open('', '_blank', 'width=900,height=700');
    win.document.write(`<!DOCTYPE html>
<html>
<head>
<meta charset="UTF-8">
<title>QR Codes</title>
<script src="https://cdnjs.cloudflare.com/ajax/libs/qrcodejs/1.0.0/qrcode.min.js"><\/script>
<style>
* { margin: 0; padding: 0; box-sizing: border-box; }
body { font-family: Arial; }
.no-print { text-align: center; margin-bottom: 15px; padding: 10px; }
.btn { padding: 10px 20px; margin: 5px; cursor: pointer; border: none; 
       border-radius: 4px; color: white; font-size: 14px; }
.btn-print { background: #4CAF50; }
.btn-close { background: #f44336; }
.page { height: 297mm; position: relative; page-break-after: always; }
.page:last-child { page-break-after: auto; }
.divider { position: absolute; top: 50%; left: 0; right: 0; height: 0; 
           border-top: 2px dashed #000; z-index: 10; }
.page-content { display: flex; flex-direction: column; height: 100%; }
.item { height: 50%; text-align: center; page-break-inside: avoid; 
        display: flex; align-items: center; justify-content: center; gap: 25px;
        padding-top: 20px; }
.item:nth-child(2) { padding-top: 30px; }
.qr { width: 120px; height: 120px; flex-shrink: 0; }
.code { font-size: 350px; font-weight: bold; letter-spacing: 2px; }
@media print { 
    .no-print { display: none; }
    @page { size: A4 portrait; margin: 0; }
}
</style>
</head>
<body>
<div class="no-print">
    <h3>QR Codes (${selected.length})</h3>
    <button class="btn btn-print" onclick="window.print()">🖨️ In</button>
    <button class="btn btn-close" onclick="window.close()">✖ Đóng</button>
</div>
<div id="qr-container"></div>
<script>
const data = ${JSON.stringify(selected)};
const container = document.getElementById('qr-container');

for (let i = 0; i < data.length; i += 2) {
    const page = document.createElement('div');
    page.className = 'page';
    
    const divider = document.createElement('div');
    divider.className = 'divider';
    page.appendChild(divider);
    
    const pageContent = document.createElement('div');
    pageContent.className = 'page-content';
    
    // Item 1
    const div1 = document.createElement('div');
    div1.className = 'item';
    div1.innerHTML = '<div class="code">' + data[i].displayText + '</div><div class="qr" id="qr' + i + '"></div>';
    pageContent.appendChild(div1);
    
    // Item 2 (if exists)
    if (i + 1 < data.length) {
        const div2 = document.createElement('div');
        div2.className = 'item';
        div2.innerHTML = '<div class="code">' + data[i + 1].displayText + '</div><div class="qr" id="qr' + (i + 1) + '"></div>';
        pageContent.appendChild(div2);
    }
    
    page.appendChild(pageContent);
    container.appendChild(page);
}

// Generate QR codes
data.forEach((item, i) => {
    new QRCode(document.getElementById('qr' + i), {
        text: item.qrContent,
        width: 120,
        height: 120
    });
});
<\/script>
</body>
</html>`);
    win.document.close();
}

// ============================================
// OTHER BUTTONS
// ============================================
function Buttonexport_Click() {
    if (typeof TableHTMLToExcel === 'function') {
        TableHTMLToExcel('DataGridView1', 'Trolley.xls', 'Trolley');
    } else {
        alert("Chức năng export chưa được cài đặt!");
    }
}

function Buttonhelp_Click() {
    window.open("/WMP_PLAY", "_blank", "width=800,height=460");
}

function Buttonclose_Click() {
    history.back();
}