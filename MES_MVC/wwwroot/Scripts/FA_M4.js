let CurrentID = null;
let BaseResult = {};
let LineListData = [];
let ImportedData = [];

// ===== UTILITY FUNCTIONS =====
function GetCookieValue(name) {
    const match = document.cookie.match(new RegExp('(^| )' + name + '=([^;]+)'));
    return match ? match[2] : null;
}

function getCurrentUser() {
    return GetCookieValue('UserID') || 'SYSTEM';
}

function getTodayISO() {
    const today = new Date();
    const year = today.getFullYear();
    const month = String(today.getMonth() + 1).padStart(2, '0');
    const day = String(today.getDate()).padStart(2, '0');
    return `${year}-${month}-${day}`;
}

function ShowLoading(flag = true) {
    $("#BackGround").css("display", flag ? "block" : "none");
}

// ===== DATE FORMATTING =====
function formatDate(dateStr) {
    if (!dateStr) return "";
    const date = new Date(dateStr);
    if (isNaN(date.getTime())) return dateStr;

    const day = String(date.getDate()).padStart(2, '0');
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const year = date.getFullYear();
    return `${day}/${month}/${year}`;
}

function formatDateTime(dateTimeStr) {
    if (!dateTimeStr) return "";
    const date = typeof dateTimeStr === 'string' ? new Date(dateTimeStr) : dateTimeStr;
    if (isNaN(date.getTime())) return String(dateTimeStr).substring(0, 16).replace('T', ' ');

    const day = String(date.getDate()).padStart(2, '0');
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const year = date.getFullYear();
    const hours = String(date.getHours()).padStart(2, '0');
    const minutes = String(date.getMinutes()).padStart(2, '0');
    const seconds = String(date.getSeconds()).padStart(2, '0');
    return `${day}/${month}/${year} ${hours}:${minutes}:${seconds}`;
}

function toISODate(vnDate) {
    if (!vnDate) return "";
    if (/^\d{4}-\d{2}-\d{2}$/.test(vnDate)) return vnDate;

    const parts = vnDate.split('/');
    if (parts.length === 3) return `${parts[2]}-${parts[1]}-${parts[0]}`;
    return vnDate;
}

// ===== API CALL =====
function ApiCall(action, param = {}) {
    ShowLoading(true);
    if (!param.USER_IDX) param.USER_IDX = getCurrentUser();

    const formData = new FormData();
    formData.append("BaseParameter", JSON.stringify(param));

    return fetch(`/FA_M4/${action}`, { method: "POST", body: formData })
        .then(r => r.json())
        .finally(() => ShowLoading(false));
}

// ===== INITIALIZE =====
$(document).ready(function () {
    $('.modal').modal();

    // Set ngày hiện tại
    $("#FilterDate").val(getTodayISO());

    LoadLines();
    Buttonfind_Click();

    // Button Events
    $("#Buttonfind").click(Buttonfind_Click);
    $("#Buttonadd").click(Buttonadd_Click);
    $("#Buttonsave").click(Buttonsave_Click);
    $("#Buttondelete").click(Buttondelete_Click);
    $("#Buttoncancel").click(Buttoncancel_Click);
    $("#Buttonexport").click(Buttonexport_Click);
    $("#Buttoninport").click(Buttoninport_Click);
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

    $("#DataGridView1").on("click", ".btn-expand", function (e) {
        e.stopPropagation();
        const $icon = $(this);
        const id = $icon.data("id");
        const $row = $icon.closest("tr");
        const $nextRow = $row.next();

        if ($nextRow.hasClass("history-row")) {
            $nextRow.remove();
            $icon.text("add_circle_outline");
        } else {
            $icon.text("remove_circle_outline");
            LoadHistory(id, $row);
        }
    });

    // Modal & Import Events
    $("#ModalSaveBtn").click(Buttonsave_Click);
    $("#FileImportFA_M4").on("change", HandleFileSelect);
    $("#BtnSaveImportFA_M4").click(SaveImport);

    // Filter Events
    $("#SearchBox").on("keypress", function (e) {
        if (e.which === 13) Buttonfind_Click();
    });
    $("#FilterLineID").on("change", Buttonfind_Click);
    $("#FilterDate").on("change", Buttonfind_Click);
});

// ===== LOAD DATA =====
function LoadLines() {
    fetch('/FA_M4/Load', { method: "POST", body: new FormData() })
        .then(r => r.json())
        .then(res => {
            if (res.LineList && res.LineList.length > 0) {
                LineListData = res.LineList;

                const $selectModal = $("#LineID");
                const $selectFilter = $("#FilterLineID");

                $selectModal.empty().append('<option value="">-- Select Line --</option>');
                $selectFilter.empty().append('<option value="">-- Tất cả Line --</option>');

                res.LineList.forEach(line => {
                    const text = line.LineGroup
                        ? `${line.LineGroup} - ${line.LineName} - ${line.Family}`
                        : line.LineName;
                    const option = `<option value="${line.ID}">${text}</option>`;
                    $selectModal.append(option);
                    $selectFilter.append(option);
                });
            }
        })
        .catch(err => console.error("Error loading lines:", err));
}

function Buttonfind_Click() {
    const search = $("#SearchBox").val() || "";
    const lineId = $("#FilterLineID").val() || "";
    const date = $("#FilterDate").val() || "";

    ApiCall("Buttonfind_Click", { SearchString: search, LineID: lineId, Date: date })
        .then(res => {
            BaseResult = res;
            if (res.LineList && res.LineList.length > 0) LineListData = res.LineList;
            DataGridView1Render(res.FAWorkOrderList || []);
        })
        .catch(err => {
            console.error("Error in search:", err);
            alert("Có lỗi xảy ra khi tìm kiếm: " + err.message);
        });
}

// ===== RENDER TABLE =====
function DataGridView1Render(data) {
    const $tbody = $("#DataGridView1 tbody");
    $tbody.empty();

    if (!data || !data.length) {
        $tbody.append('<tr><td colspan="15" class="center-align">Không có dữ liệu</td></tr>');
        UpdateTotalQuantity(0);
        return;
    }

    let totalQuantity = 0;

    data.forEach(item => {
        const lineName = getLineName(item.LineID);
        const quantity = parseInt(item.Quantity) || 0;
        totalQuantity += quantity;

        $tbody.append(`
            <tr data-id="${item.ID}">
                <td class="center-align">
                    <i class="material-icons btn-expand" style="cursor:pointer; color:#26a69a;" data-id="${item.ID}">add_circle_outline</i>
                </td>
                <td><button class="btn btn-small btn-edit"><i class="material-icons">edit</i></button></td>
                <td>${item.ID ?? ""}</td>
                <td>${lineName}</td>
                <td>${item.PartNumber ?? ""}</td>
                <td>${item.ECN ?? ""}</td>
                <td>${item.New_ECN ?? ""}</td>
                <td>${item.SNP ?? ""}</td>
                <td>${formatDate(item.Date)}</td>
                <td>${quantity}</td>
                <td>${item.Remark ?? ""}</td>
                <td>${formatDateTime(item.CreatedDate)}</td>
                <td>${item.CreatedBy ?? ""}</td>
                <td>${formatDateTime(item.ModifiedDate)}</td>
                <td>${item.ModifiedBy ?? ""}</td>
            </tr>
        `);
    });

    UpdateTotalQuantity(totalQuantity);
}

function UpdateTotalQuantity(total) {
    $("#TotalQuantity").text(total.toLocaleString('vi-VN'));
}

function getLineName(lineId) {
    if (!lineId || !LineListData || LineListData.length === 0) return "";
    const line = LineListData.find(x => x.ID == lineId);
    if (!line) return "";
    return line.LineGroup ? `${line.LineGroup} - ${line.LineName} - ${line.Family}` : line.LineName;
}

// ===== HISTORY =====
function LoadHistory(workOrderId, $row) {
    ApiCall("GetHistory", { ID: workOrderId })
        .then(res => {
            if (res.Success && res.FAWorkOrderHistoryList && res.FAWorkOrderHistoryList.length > 0) {
                RenderHistory(res.FAWorkOrderHistoryList, $row);
            } else {
                $row.after(`
                    <tr class="history-row">
                        <td colspan="15" style="background:#f5f5f5; padding:10px; text-align:center;">
                            <em class="grey-text">Không có lịch sử</em>
                        </td>
                    </tr>
                `);
            }
        })
        .catch(err => {
            console.error("Error loading history:", err);
            alert("Lỗi khi tải lịch sử: " + err.message);
        });
}

function RenderHistory(histories, $row) {
    let html = `
        <tr class="history-row">
            <td colspan="15" style="background:#f0f0f0; padding:5px 10px;">
                <table class="striped" style="background:white; margin:0; border:1px solid #ddd;">
                    <thead>
                        <tr style="background:#e0f2f1;">
                            <th style="padding:8px;">Part Number</th>
                            <th style="padding:8px;">Ngày cũ</th>
                            <th style="padding:8px;">Ngày mới</th>
                            <th style="padding:8px;">SL cũ</th>
                            <th style="padding:8px;">SL mới</th>
                            <th style="padding:8px;">Thời gian sửa</th>
                            <th style="padding:8px;">Người sửa</th>
                        </tr>
                    </thead>
                    <tbody>`;

    histories.forEach(h => {
        const dateOldClass = h.OldDate !== h.NewDate ? 'red-text' : '';
        const dateNewClass = h.OldDate !== h.NewDate ? 'green-text' : '';
        const qtyOldClass = h.OldQuantity !== h.NewQuantity ? 'red-text' : '';
        const qtyNewClass = h.OldQuantity !== h.NewQuantity ? 'green-text' : '';

        html += `
            <tr>
                <td style="padding:8px;"><strong>${h.PartNumber}</strong></td>
                <td style="padding:8px;" class="${dateOldClass}">${formatDate(h.OldDate)}</td>
                <td style="padding:8px;" class="${dateNewClass}">${formatDate(h.NewDate)}</td>
                <td style="padding:8px; text-align:right;" class="${qtyOldClass}">${h.OldQuantity}</td>
                <td style="padding:8px; text-align:right;" class="${qtyNewClass}">${h.NewQuantity}</td>
                <td style="padding:8px; white-space:nowrap;">${formatDateTime(h.ModifiedDate)}</td>
                <td style="padding:8px;">${h.ModifiedBy ?? ""}</td>
            </tr>`;
    });

    html += `</tbody></table></td></tr>`;
    $row.after(html);
}

// ===== ADD/EDIT/DELETE =====
function Buttonadd_Click() {
    ResetForm();
    CurrentID = null;
    $("#modal-title").text("Add Work Order");
    $("#modal-add").modal("open");
}

function EditRow_Click(id) {
    const data = (BaseResult.FAWorkOrderList || []).find(x => x.ID == id);
    if (!data) return;

    FillForm(data);
    CurrentID = id;
    $("#modal-title").text("Edit Work Order");
    $("#modal-add").modal("open");
}

function FillForm(data) {
    $("#WorkOrderID").val(data.ID ?? "");
    $("#LineID").val(data.LineID ?? "");
    $("#PartNumber").val(data.PartNumber ?? "");
    $("#ECN").val(data.ECN ?? "");
    $("#New_ECN").val(data.New_ECN ?? "");
    $("#SNP").val(data.SNP ?? "");
    $("#Date").val(data.Date ? toISODate(formatDate(data.Date)) : "");
    $("#Quantity").val(data.Quantity ?? "");
    $("#Remark").val(data.Remark ?? "");
}

function ResetForm() {
    $("#FAWorkOrderForm")[0].reset();
    $("#WorkOrderID").val("");
}

function ValidateForm() {
    const errors = [];
    if (!$("#LineID").val()) errors.push("Line không được để trống");
    if (!$("#PartNumber").val().trim()) errors.push("Part Number không được để trống");
    if (!$("#Date").val()) errors.push("Ship Date không được để trống");
    if (!$("#Quantity").val() || $("#Quantity").val() < 0) errors.push("Quantity phải >= 0");

    if (errors.length > 0) {
        alert(errors.join("\n"));
        return false;
    }
    return true;
}

function GetFormData() {
    return {
        FAWorkOrder: {
            ID: parseInt($("#WorkOrderID").val()) || 0,
            LineID: parseInt($("#LineID").val()) || null,
            PartNumber: $("#PartNumber").val(),
            ECN: $("#ECN").val(),
            New_ECN: $("#New_ECN").val(),
            SNP: parseInt($("#SNP").val()) || null,
            Date: $("#Date").val(),
            Quantity: parseInt($("#Quantity").val()) || 0,
            Remark: $("#Remark").val()
        },
        USER_IDX: getCurrentUser()
    };
}

function Buttonsave_Click() {
    if (!ValidateForm()) return;

    const data = GetFormData();
    const action = data.FAWorkOrder.ID > 0 ? "Buttonsave_Click" : "Buttonadd_Click";

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
    const id = $("#DataGridView1 tr.selected").data("id");
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

function Buttoncancel_Click() {
    ResetForm();
    $("#modal-add").modal("close");
}

// ===== IMPORT EXCEL =====
function Buttoninport_Click() {
    ImportedData = [];
    $("#ImportPreviewTableFA_M4").html('<tr><td colspan="7" class="center-align grey-text">Chưa có dữ liệu. Vui lòng chọn file Excel.</td></tr>');
    $("#ImportSummary").hide();
    $("#BtnSaveImportFA_M4").prop("disabled", true);
    $("#ImportCount").text("0");
    $("#FileImportFA_M4").val("");
    $("#ModalImportFA_M4").modal("open");
}

function HandleFileSelect(e) {
    const file = e.target.files[0];
    if (!file) return;

    $("#importProgressFA_M4").show();

    const reader = new FileReader();
    reader.onload = function (e) {
        try {
            const data = new Uint8Array(e.target.result);
            const workbook = XLSX.read(data, { type: 'array' });
            const firstSheet = workbook.Sheets[workbook.SheetNames[0]];
            const jsonData = XLSX.utils.sheet_to_json(firstSheet, { header: 1 });

            ImportedData = ProcessExcelData(jsonData);
            RenderImportPreview(ImportedData);
        } catch (err) {
            console.error("Error reading file:", err);
            alert("Không thể đọc file Excel: " + err.message);
        } finally {
            $("#importProgressFA_M4").hide();
        }
    };
    reader.readAsArrayBuffer(file);
}

function ProcessExcelData(jsonData) {
    const workOrders = [];
    if (jsonData.length < 2) return workOrders;

    const headers = jsonData[0];
    const dateColumns = [];

    // Parse date columns
    for (let i = 5; i < headers.length; i++) {
        if (headers[i]) {
            const formattedDate = parseExcelDate(headers[i]);
            if (formattedDate) {
                dateColumns.push({ index: i, date: formattedDate });
            }
        }
    }

    // Process rows
    for (let i = 1; i < jsonData.length; i++) {
        const row = jsonData[i];
        if (!row || row.length === 0) continue;

        const lineStr = row[0] ? row[0].toString().trim() : "";
        const partNumber = row[1] ? row[1].toString().trim() : "";
        const ecnRaw = row[2] ? row[2].toString().trim() : "";
        const newEcn = row[3] ? row[3].toString().trim() : "";
        const snp = row[4] ? parseInt(row[4]) : null;
        const ecn = ecnRaw.toUpperCase().startsWith("ECN") ? ecnRaw.substring(3).trim() : ecnRaw;

        if (!partNumber) continue;

        const lineId = FindLineIDByName(lineStr);
        if (!lineId) continue;

        dateColumns.forEach(col => {
            const quantity = row[col.index];
            if (quantity && parseInt(quantity) > 0) {
                workOrders.push({
                    LineID: lineId,
                    LineName: lineStr,
                    PartNumber: partNumber,
                    ECN: ecn,
                    New_ECN: newEcn,
                    SNP: snp,
                    Date: col.date,
                    Quantity: parseInt(quantity)
                });
            }
        });
    }

    return workOrders;
}

function parseExcelDate(dateValue) {
    // String formats
    if (typeof dateValue === 'string') {
        if (/^\d{4}-\d{2}-\d{2}$/.test(dateValue)) return dateValue;

        if (/^\d{1,2}\/\d{1,2}\/\d{4}$/.test(dateValue)) {
            const parts = dateValue.split('/');
            const month = parts[0].padStart(2, '0');
            const day = parts[1].padStart(2, '0');
            return `${parts[2]}-${month}-${day}`;
        }

        if (/^\d{2}\/\d{2}\/\d{4}$/.test(dateValue)) {
            const parts = dateValue.split('/');
            return `${parts[2]}-${parts[1]}-${parts[0]}`;
        }
    }

    // Excel number format
    if (typeof dateValue === 'number') {
        const date = XLSX.SSF.parse_date_code(dateValue);
        const year = date.y;
        const month = String(date.m).padStart(2, '0');
        const day = String(date.d).padStart(2, '0');
        return `${year}-${month}-${day}`;
    }

    // Date object
    if (dateValue instanceof Date) {
        const year = dateValue.getFullYear();
        const month = String(dateValue.getMonth() + 1).padStart(2, '0');
        const day = String(dateValue.getDate()).padStart(2, '0');
        return `${year}-${month}-${day}`;
    }

    // Fallback
    try {
        const date = new Date(dateValue);
        if (!isNaN(date.getTime())) {
            const year = date.getFullYear();
            const month = String(date.getMonth() + 1).padStart(2, '0');
            const day = String(date.getDate()).padStart(2, '0');
            return `${year}-${month}-${day}`;
        }
    } catch (e) {
        console.error("Parse date error:", dateValue, e);
    }

    return null;
}

function FindLineIDByName(lineName) {
    if (!lineName || !LineListData || LineListData.length === 0) return null;

    let line = LineListData.find(x =>
        x.LineName === lineName ||
        `${x.LineGroup} - ${x.LineName} - ${x.Family}` === lineName ||
        `${x.LineGroup} - ${x.LineName}` === lineName
    );

    if (line) return line.ID;

    line = LineListData.find(x =>
        x.LineName.includes(lineName) || lineName.includes(x.LineName)
    );

    return line ? line.ID : null;
}

function RenderImportPreview(data) {
    const $tbody = $("#ImportPreviewTableFA_M4");
    $tbody.empty();

    if (!data || data.length === 0) {
        $tbody.append('<tr><td colspan="7" class="center-align red-text">Không có dữ liệu hợp lệ!</td></tr>');
        $("#ImportSummary").hide();
        $("#BtnSaveImportFA_M4").prop("disabled", true);
        return;
    }

    data.forEach(item => {
        $tbody.append(`
            <tr>
                <td>${item.LineName}</td>
                <td><strong>${item.PartNumber}</strong></td>
                <td>${item.ECN || ""}</td>
                <td>${item.New_ECN || ""}</td>
                <td class="center-align">${item.SNP || ""}</td>
                <td><strong class="blue-text">${formatDate(item.Date)}</strong></td>
                <td class="right-align"><strong class="green-text">${item.Quantity}</strong></td>
            </tr>
        `);
    });

    $("#BtnSaveImportFA_M4").prop("disabled", false);
    $("#ImportCount").text(data.length);
}

function SaveImport() {
    if (!ImportedData || ImportedData.length === 0) {
        alert("Không có dữ liệu!");
        return;
    }

    if (!confirm(`Bạn chắc chắn muốn import ${ImportedData.length} Work Orders?`)) return;

    ApiCall("Buttoninport_Click", { FAWorkOrderList: ImportedData })
        .then(res => {
            if (res.Success) {
                alert(res.Message || "Import thành công!");
                $("#ModalImportFA_M4").modal("close");
                Buttonfind_Click();
            } else {
                alert(res.Error || "Lỗi: " + res.Error);
            }
        })
        .catch(err => {
            console.error("Error in import:", err);
            alert("Có lỗi xảy ra khi import: " + err.message);
        });
}

// ===== OTHER ACTIONS =====
function Buttonexport_Click() {
    TableHTMLToExcel('DataGridView1', 'FAWorkOrder.xls', 'FAWorkOrder');
}

function Buttonprint_Click() {
    window.open(`/FA_M4/Buttonprint_Click`, "_blank");
}

function Buttonhelp_Click() {
    window.open("/WMP_PLAY", "_blank", "width=800,height=460");
}

function Buttonclose_Click() {
    history.back();
}