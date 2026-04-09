let CurrentID = null;
let BaseResult = {};
let ImportedData = [];
let CurrentImageFile = null;

function GetCookieValue(name) {
    const match = document.cookie.match(new RegExp('(^| )' + name + '=([^;]+)'));
    return match ? match[2] : null;
}

function getCurrentUser() {
    return GetCookieValue('UserID') || 'SYSTEM';
}

// ============================================
// NEW: Format number with comma separator
// ============================================
function formatNumber(num) {
    if (!num && num !== 0) return "";
    return parseFloat(num).toLocaleString('en-US');
}

$(document).ready(function () {
    $('.modal').modal();
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

    $("#FilterDept, #FilterLine, #FilterStatus").on("change", function () {
        Buttonfind_Click();
    });

    $("#Buttoninport").click(Buttoninport_Click);
    $("#FileImport").on("change", HandleFileSelect);
    $("#BtnSaveImport").click(SaveImport);

    $("#ImageFile").on("change", HandleImageSelect);
    $("#BtnRemoveImage").click(RemoveImage);
});

function ShowLoading(flag = true) {
    $("#BackGround").css("display", flag ? "block" : "none");
}

function ApiCall(action, param = {}) {
    ShowLoading(true);
    if (!param.USER_IDX) param.USER_IDX = getCurrentUser();

    let formData = new FormData();
    formData.append("BaseParameter", JSON.stringify(param));
    return fetch(`/E03/${action}`, { method: "POST", body: formData })
        .then(r => r.json())
        .finally(() => ShowLoading(false));
}

function Buttonfind_Click() {
    let search = $("#SearchBox").val() || "";
    let filterDept = $("#FilterDept").val() || "";
    let filterLine = $("#FilterLine").val() || "";
    let filterStatus = $("#FilterStatus").val() || "";

    ApiCall("Buttonfind_Click", {
        SearchString: search,
        Dept: filterDept,
        ProductionLine: filterLine,
        Status: filterStatus
    })
        .then(res => {
            BaseResult = res;
            DataGridView1Render(res.ToolShopList || []);
            PopulateFilters(res.ToolShopList || []);
        })
        .catch(err => {
            console.error("Error in search:", err);
            alert("Có lỗi xảy ra khi tìm kiếm: " + err.message);
        });
}

function PopulateFilters(data) {
    let depts = [...new Set(data.map(x => x.Dept).filter(Boolean))].sort();
    let $deptFilter = $("#FilterDept");
    let currentDept = $deptFilter.val();
    $deptFilter.empty().append('<option value="">-- Tất cả --</option>');
    depts.forEach(dept => {
        $deptFilter.append(`<option value="${dept}">${dept}</option>`);
    });
    if (currentDept) $deptFilter.val(currentDept);

    let lines = [...new Set(data.map(x => x.ProductionLine).filter(Boolean))].sort();
    let $lineFilter = $("#FilterLine");
    let currentLine = $lineFilter.val();
    $lineFilter.empty().append('<option value="">-- Tất cả --</option>');
    lines.forEach(line => {
        $lineFilter.append(`<option value="${line}">${line}</option>`);
    });
    if (currentLine) $lineFilter.val(currentLine);
}

function DataGridView1Render(data) {
    let $tbody = $("#DataGridView1 tbody");
    $tbody.empty();

    if (!data || !data.length) {
        $tbody.append('<tr><td colspan="20" class="center-align">Không có dữ liệu</td></tr>');
        $("#TotalRecords").text("0");
        return;
    }

    data.forEach(item => {
        let imageCell = '';
        if (item.Image) {
            imageCell = `<i class="material-icons image-icon" data-image="${item.Image}" style="cursor:pointer;color:#2196F3;font-size:28px;" title="Xem ảnh">image</i>`;
        }

        $tbody.append(`
            <tr data-id="${item.ID}">
                <td><button class="btn btn-small btn-edit"><i class="material-icons">edit</i></button></td>
                <td class="center-align">${imageCell}</td>
                <td>${item.ID ?? ""}</td>
                <td>${item.Code ?? ""}</td>
                <td>${item.Sub_Code ?? ""}</td>
                <td>${item.NameVN ?? ""}</td>
                <td>${item.NameEN ?? ""}</td>
                <td>${item.Serial ?? ""}</td>
                <td class="right-align">${formatNumber(item.Quantity)}</td>
                <td>${item.Unit ?? ""}</td>
                <td class="right-align">${formatNumber(item.UnitPrice)}</td>
                <td>${item.Currency ?? ""}</td>
                <td>${item.Type ?? ""}</td>
                <td>${item.Dept ?? ""}</td>
                <td>${item.ProductionLine ?? ""}</td>
                <td><span class="badge ${getStatusColor(item.Status)}">${item.Status ?? ""}</span></td>
                <td>${formatDate(item.UsingDate)}</td>
                <td>${formatDate(item.ExpiryDate)}</td>
              
                <td>${formatDateTime(item.CreateDate)}</td>
                <td>${item.CreateUser ?? ""}</td>
            </tr>
        `);
    });
    $("#TotalRecords").text(data.length);

    $(".image-icon").click(function () {
        let imagePath = $(this).data("image");
        ShowImageModal(imagePath);
    });
}

function ShowImageModal(imagePath) {
    $("#ImageView").attr("src", imagePath);
    $("#ModalImageView").modal("open");
}

function getStatusColor(status) {
    switch (status) {
        case 'Active': return 'green white-text';
        case 'Inactive': return 'grey white-text';
        case 'Maintenance': return 'orange white-text';
        case 'Broken': return 'red white-text';
        default: return '';
    }
}

function formatDate(dateStr) {
    if (!dateStr) return "";
    const date = new Date(dateStr);
    if (isNaN(date.getTime())) return "";
    return date.toISOString().substring(0, 10);
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
    CurrentImageFile = null;
    $("#modal-title").text("Add Tool Shop");
    $("#modal-add").modal("open");
}

function EditRow_Click(id) {
    let data = (BaseResult.ToolShopList || []).find(x => x.ID == id);
    if (!data) return;
    FillForm(data);
    CurrentID = id;

    CurrentImageFile = null;
    $("#modal-title").text("Edit Tool Shop");
    $("#modal-add").modal("open");
}

function FillForm(data) {
    $("#ToolShopID").val(data.ID ?? "");
    $("#Code").val(data.Code ?? "");
    $("#Sub_Code").val(data.Sub_Code ?? "");
    $("#NameVN").val(data.NameVN ?? "");
    $("#NameEN").val(data.NameEN ?? "");
    $("#NameKO").val(data.NameKO ?? "");
    $("#Serial").val(data.Serial ?? "");
    $("#Quantity").val(data.Quantity ?? "");
    $("#Unit").val(data.Unit ?? "");
    $("#UnitPrice").val(data.UnitPrice ?? "");
    $("#Currency").val(data.Currency ?? "");
    $("#Type").val(data.Type ?? "");
    $("#Owner").val(data.Owner ?? "");
    $("#Supplier").val(data.Supplier ?? "");
    $("#Dept").val(data.Dept ?? "");
    $("#ProductionLine").val(data.ProductionLine ?? "");
    $("#Status").val(data.Status ?? "");
    $("#UsingDate").val(formatDate(data.UsingDate));
    $("#ExpiryDate").val(formatDate(data.ExpiryDate));
    $("#InvNumber").val(data.InvNumber ?? "");
    $("#CustomDeclaration").val(data.CustomDeclaration ?? "");
    $("#Note").val(data.Note ?? "");

    if (data.Image) {
        $("#ImagePreview").html(`
            <img src="${data.Image}" style="max-width:200px;max-height:200px;border:1px solid #ddd;padding:5px;">
            <p class="green-text"><i class="material-icons tiny">check_circle</i> Đã có ảnh</p>
        `);
        $("#BtnRemoveImage").show();
    } else {
        $("#ImagePreview").html('<p class="grey-text">Chưa có ảnh</p>');
        $("#BtnRemoveImage").hide();
    }
}

function ResetForm() {
    $("#ToolShopForm")[0].reset();
    $("#ToolShopID").val("");
    $("#ImagePreview").html('<p class="grey-text">Chưa có ảnh</p>');
    $("#ImageFile").val("");
    $("#BtnRemoveImage").hide();
    CurrentImageFile = null;
}

function ValidateForm() {
    let errors = [];
    if (!$("#Code").val().trim()) errors.push("Code không được để trống");

    if (errors.length > 0) {
        alert(errors.join("\n"));
        return false;
    }
    return true;
}

function GetFormData() {
    return {
        ToolShop: {
            ID: parseInt($("#ToolShopID").val()) || 0,
            Code: $("#Code").val(),
            Sub_Code: $("#Sub_Code").val(),
            NameVN: $("#NameVN").val(),
            NameEN: $("#NameEN").val(),
            NameKO: $("#NameKO").val(),
            Serial: $("#Serial").val(),
            Quantity: parseFloat($("#Quantity").val()) || null,
            Unit: $("#Unit").val(),
            UnitPrice: parseFloat($("#UnitPrice").val()) || null,
            Currency: $("#Currency").val(),
            Type: $("#Type").val(),
            Owner: $("#Owner").val(),
            Supplier: $("#Supplier").val(),
            Dept: $("#Dept").val(),
            ProductionLine: $("#ProductionLine").val(),
            Status: $("#Status").val(),
            UsingDate: $("#UsingDate").val() || null,
            ExpiryDate: $("#ExpiryDate").val() || null,
            InvNumber: $("#InvNumber").val(),
            CustomDeclaration: $("#CustomDeclaration").val(),
            Note: $("#Note").val()
        },
        USER_IDX: getCurrentUser()
    };
}

function HandleImageSelect(e) {
    let file = e.target.files[0];
    if (!file) {
        CurrentImageFile = null;
        return;
    }

    const allowedTypes = ['image/jpeg', 'image/jpg', 'image/png', 'image/gif', 'image/bmp'];
    if (!allowedTypes.includes(file.type)) {
        alert("Chỉ chấp nhận file ảnh (jpg, png, gif, bmp)!");
        $("#ImageFile").val("");
        return;
    }

    if (file.size > 5 * 1024 * 1024) {
        alert("Kích thước file không được vượt quá 5MB!");
        $("#ImageFile").val("");
        return;
    }

    CurrentImageFile = file;

    let reader = new FileReader();
    reader.onload = function (e) {
        $("#ImagePreview").html(`
            <img src="${e.target.result}" style="max-width:200px;max-height:200px;border:1px solid #ddd;padding:5px;">
            <p class="blue-text"><i class="material-icons tiny">info</i> Ảnh mới (chưa lưu)</p>
        `);
    };
    reader.readAsDataURL(file);
}

async function UploadImage(toolShopId) {
    if (!CurrentImageFile) return null;

    ShowLoading(true);
    try {
        let formData = new FormData();
        formData.append("file", CurrentImageFile);
        formData.append("toolShopId", toolShopId);

        let response = await fetch("/E03/UploadImage", {
            method: "POST",
            body: formData
        });

        let result = await response.json();
        return result;
    } catch (err) {
        console.error("Error uploading image:", err);
        return { Error: err.message };
    } finally {
        ShowLoading(false);
    }
}

async function RemoveImage() {
    if (!CurrentID) {
        alert("Chưa có Tool Shop để xóa ảnh!");
        return;
    }

    if (!confirm("Bạn chắc chắn muốn xóa ảnh này?")) return;

    ShowLoading(true);
    try {
        let formData = new FormData();
        formData.append("toolShopId", CurrentID);

        let response = await fetch("/E03/DeleteImage", {
            method: "POST",
            body: formData
        });

        let result = await response.json();

        if (result.Success) {
            $("#ImagePreview").html('<p class="grey-text">Chưa có ảnh</p>');
            $("#BtnRemoveImage").hide();
            $("#ImageFile").val("");
            CurrentImageFile = null;
            alert("Xóa ảnh thành công!");
        } else {
            alert(result.Error || "Có lỗi xảy ra khi xóa ảnh!");
        }
    } catch (err) {
        console.error("Error deleting image:", err);
        alert("Có lỗi xảy ra: " + err.message);
    } finally {
        ShowLoading(false);
    }
}

async function Buttonsave_Click() {
    if (!ValidateForm()) return;

    let data = GetFormData();
    let action = data.ToolShop.ID > 0 ? "Buttonsave_Click" : "Buttonadd_Click";

    try {
        let result = await ApiCall(action, data);

        if (result.Success) {
            let savedToolShop = result.ToolShopList[0];
            let toolShopId = savedToolShop.ID;

            if (CurrentImageFile) {
                let uploadResult = await UploadImage(toolShopId);
                if (uploadResult && uploadResult.Error) {
                    alert("Lưu thành công nhưng có lỗi khi upload ảnh: " + uploadResult.Error);
                }
            }

            Buttonfind_Click();
            $("#modal-add").modal("close");
            alert(result.Message || "Thành công");
        } else {
            alert(result.Error || "Có lỗi xảy ra");
        }
    } catch (err) {
        console.error("Error in save:", err);
        alert("Có lỗi xảy ra khi lưu: " + err.message);
    }
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
    $("#ImportPreviewTable").html('<tr><td colspan="13" class="center-align grey-text">Chưa có dữ liệu. Vui lòng chọn file Excel.</td></tr>');
    $("#BtnSaveImport").prop("disabled", true);
    $("#ImportCount").text("0");
    $("#FileImport").val("");
    $("#ModalImport").modal("open");
}

function HandleFileSelect(e) {
    let file = e.target.files[0];
    if (!file) return;

    $("#importProgress").show();

    let reader = new FileReader();
    reader.onload = function (e) {
        try {
            let data = new Uint8Array(e.target.result);
            let workbook = XLSX.read(data, { type: 'array' });
            let firstSheet = workbook.Sheets[workbook.SheetNames[0]];
            let jsonData = XLSX.utils.sheet_to_json(firstSheet);

            ImportedData = ProcessExcelData(jsonData);
            RenderImportPreview(ImportedData);
            $("#importProgress").hide();
        } catch (err) {
            console.error("Error reading file:", err);
            alert("Không thể đọc file Excel: " + err.message);
            $("#importProgress").hide();
        }
    };
    reader.readAsArrayBuffer(file);
}

function ProcessExcelData(jsonData) {
    let toolShops = [];

    if (!jsonData || jsonData.length === 0) {
        alert("File Excel không có dữ liệu!");
        return toolShops;
    }

    jsonData.forEach(row => {
        let code = row['Code'] ? row['Code'].toString().trim() : "";
        if (!code) return;

        toolShops.push({
            Code: code,
            Sub_Code: row['Sub Code'] ? row['Sub Code'].toString().trim() : "",
            NameVN: row['Name VN'] ? row['Name VN'].toString().trim() : "",
            NameEN: row['Name EN'] ? row['Name EN'].toString().trim() : "",
            NameKO: row['Name KO'] ? row['Name KO'].toString().trim() : "",
            Serial: row['Serial'] ? row['Serial'].toString().trim() : "",
            Quantity: row['Quantity'] ? parseFloat(row['Quantity']) : null,
            Unit: row['Unit'] ? row['Unit'].toString().trim() : "",
            UnitPrice: row['Unit Price'] ? parseFloat(row['Unit Price']) : null,
            Currency: row['Currency'] ? row['Currency'].toString().trim() : "",
            Type: row['Type'] ? row['Type'].toString().trim() : "",
            Owner: row['Owner'] ? row['Owner'].toString().trim() : "",
            Supplier: row['Supplier'] ? row['Supplier'].toString().trim() : "",
            Dept: row['Dept'] ? row['Dept'].toString().trim() : "",
            ProductionLine: row['Line'] ? row['Line'].toString().trim() : "",
            Status: row['Status'] ? row['Status'].toString().trim() : "",
            InvNumber: row['Inv Number'] ? row['Inv Number'].toString().trim() : "",
            CustomDeclaration: row['Custom Declaration'] ? row['Custom Declaration'].toString().trim() : "",
            Note: row['Note'] ? row['Note'].toString().trim() : ""
        });
    });

    return toolShops;
}

function RenderImportPreview(data) {
    let $tbody = $("#ImportPreviewTable");
    $tbody.empty();

    if (!data || data.length === 0) {
        $tbody.append('<tr><td colspan="13" class="center-align red-text">Không có dữ liệu hợp lệ!</td></tr>');
        $("#BtnSaveImport").prop("disabled", true);
        $("#ImportCount").text("0");
        return;
    }

    data.forEach(item => {
        $tbody.append(`
            <tr>
                <td>${item.Code ?? ""}</td>
                <td>${item.Sub_Code ?? ""}</td>
                <td>${item.NameVN ?? ""}</td>
                <td>${item.NameEN ?? ""}</td>
                <td>${item.Serial ?? ""}</td>
                <td class="right-align">${formatNumber(item.Quantity)}</td>
                <td>${item.Unit ?? ""}</td>
                <td class="right-align">${formatNumber(item.UnitPrice)}</td>
                <td>${item.Currency ?? ""}</td>
                <td>${item.Type ?? ""}</td>
                <td>${item.Dept ?? ""}</td>
                <td>${item.ProductionLine ?? ""}</td>
                <td>${item.Status ?? ""}</td>
            </tr>
        `);
    });

    $("#BtnSaveImport").prop("disabled", false);
    $("#ImportCount").text(data.length);
}

function SaveImport() {
    if (!ImportedData || ImportedData.length === 0) {
        alert("Không có dữ liệu để import!");
        return;
    }

    if (!confirm(`Bạn chắc chắn muốn import ${ImportedData.length} records?`)) return;

    ApiCall("Buttoninport_Click", { ToolShopList: ImportedData })
        .then(res => {
            if (res.Success) {
                alert(res.Message || "Import thành công!");
                $("#ModalImport").modal("close");
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
    TableHTMLToExcel('DataGridView1', 'ToolShop.xls', 'ToolShop');
}

function Buttonprint_Click() {
    window.open(`/E03/Buttonprint_Click`, "_blank");
}

function Buttonhelp_Click() {
    window.open("/WMP_PLAY", "_blank", "width=800,height=460");
}

function Buttonclose_Click() {
    history.back();
}