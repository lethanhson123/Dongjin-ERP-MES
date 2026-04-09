let BaseResult;
let table1;
let table2;
let selectedFile = null;
let isScanning = false;
let CurrentHistoryData = null;

// ==================== SOUND FUNCTIONS ====================
function PlaySuccessSound() {
    try {
        let audio = new Audio('/audio/HOOK_S.mp3');
        audio.play().catch(e => console.log('Cannot play success sound:', e));
    } catch (e) {
        console.log('Success sound not available');
    }
}

function PlayErrorSound() {
    try {
        let audio = new Audio('/audio/Sash_brk.mp3');
        audio.play().catch(e => console.log('Cannot play error sound:', e));
    } catch (e) {
        console.log('Error sound not available');
    }
}


function showLoading(show) {
    $("#BackGround").css("display", show ? "block" : "none");
}

async function callApi(url, param) {
    const formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(param));
    const response = await fetch(url, { method: "POST", body: formUpload });
    return await response.json();
}

function GetCookieValue(name) {
    const match = document.cookie.match(new RegExp('(^| )' + name + '=([^;]+)'));
    return match ? match[2] : null;
}

function initTable1() {
    table1 = $("#Table1").DataTable({
        scrollX: true,
        scrollY: "42vh",
        scrollCollapse: true,
        paging: true,
        pageLength: 1000,
        lengthChange: false,
        autoWidth: false,
        responsive: false,
        ordering: true,
        searching: false,
        fixedHeader: false,
        columnDefs: [
            { targets: 0, orderable: false, className: "dt-center" },
            { targets: "_all", className: "dt-nowrap" }
        ]
    });
    $(".dataTables_scrollBody").css("min-height", "42vh");
}

function initTable2() {
    table2 = $("#Table2").DataTable({
        scrollX: true,
        scrollY: "52vh",
        scrollCollapse: true,
        paging: true,
        pageLength: 1000,
        lengthChange: false,
        autoWidth: false,
        responsive: false,
        ordering: true,
        searching: false,
        fixedHeader: false,
        columnDefs: [
            { targets: 0, orderable: false, className: "dt-center" },
            { targets: 1, orderable: false, className: "dt-center" },
            { targets: "_all", className: "dt-nowrap" }
        ]
    });
    $(".dataTables_scrollBody").css("min-height", "52vh");

    $('#Table2').on('click', '.btn-edit-history', function () {
        let id = $(this).data("id");
        EditHistory(id);
    });
}

$(document).ready(function () {
    $('.modal').modal();

    const today = new Date().toISOString().split('T')[0];
    $("#tab2_fromDate").val(today);
    $("#tab2_toDate").val(today);

    initTable1();
    initTable2();

    $('a[href="#Tag002"]').on('click', function () {
        setTimeout(() => Buttonfind_Click(), 300);
    });

    $("#tab2_Search").on('keydown', function (e) {
        if (e.key === 'Enter' || e.keyCode === 13) {
            e.preventDefault();
            Buttonfind_Click();
        }
    });

    $("#tab1_QRCodeScan").on('keydown', async function (e) {
        if ((e.key === 'Enter' || e.keyCode === 13) && !isScanning) {
            e.preventDefault();
            isScanning = true;
            await ScanLotcode();
            isScanning = false;
        }
    });

    $(".upload-btn").on('click', () => $("#fileInput").click());

    $("#fileInput").on('change', function () {
        const file = this.files[0];
        if (!file) return;
        selectedFile = file;
        const reader = new FileReader();
        reader.onload = (e) => $("#pictureUpload").attr('src', e.target.result);
        reader.readAsDataURL(file);
    });

    $("#modalClose").on('click', () => $("#pictureModal").hide());

    $(window).on('click', function (e) {
        if (e.target.id === "pictureModal") $("#pictureModal").hide();
    });

    $("#BtnSaveHistory").on('click', SaveHistory);

    $('#SelectAllCheckbox').on('change', function () {
        const isChecked = $(this).is(':checked');
        $('#Table2 .row-checkbox').prop('checked', isChecked);
    });

    $('#Table2').on('change', '.row-checkbox', function () {
        const total = $('#Table2 .row-checkbox').length;
        const checked = $('#Table2 .row-checkbox:checked').length;
        $('#SelectAllCheckbox').prop('checked', total === checked);
    });
});

$("#Buttonfind").click(() => Buttonfind_Click());
$("#Buttonadd").click(() => Buttonadd_Click());
$("#Buttonsave").click(() => Buttonsave_Click());
$("#Buttondelete").click(() => Buttondelete_Click());
$("#Buttoncancel").click(() => Buttoncancel_Click());
$("#Buttonhelp").click(() => Buttonhelp_Click());
$("#Buttonclose").click(() => Buttonclose_Click());
$("#Buttonexport").click(() => Buttonexport_Click());

function Buttonexport_Click() {
    if (!table2 || table2.data().count() === 0) {
        ShowMessage("No data to export", "warning");
        return;
    }

    try {
        const $exportTable = $('<table id="TempExport" border="1"></table>');

        // ✅ Header TÙY CHỈNH
        const customHeaders = [
            'No', 'Supplier', 'Invoice', 'QRCode', 'Part No', 'Sample',
            'Total Invoice', 'NG Qty', 'Unit', '%', 'Input Date', 'Issue Date',
            'Week', 'Month', 'Error Info', 'Picture', 'Status', 'Note',
            'Create User', 'Create Date'
        ];

        const headerRow = $('<tr></tr>');
        customHeaders.forEach(h => headerRow.append('<th>' + h + '</th>'));
        $exportTable.append($('<thead></thead>').append(headerRow));

        // Body
        const tbody = $('<tbody></tbody>');
        let no = 1;

        table2.rows().every(function () {
            const row = $('<tr></tr>');
            row.append('<td>' + no++ + '</td>');

            $(this.node()).find('td').slice(2).each(function () {
                row.append('<td>' + $(this).text().trim() + '</td>');
            });

            tbody.append(row);
        });
        $exportTable.append(tbody);

        // Export
        $('body').append($exportTable.hide());
        TableHTMLToExcel('TempExport', 'IQC_NG_Customer2.xls', 'IQC_NG_Customer2');
        $exportTable.remove();

        ShowMessage("Export successfully!", "ok");
    } catch (err) {
        ShowMessage("Export error: " + err.message, "error");
    }
}

async function Buttonfind_Click() {
    try {
        showLoading(true);

        const data = await callApi("/F03/Buttonfind_Click", {
            FromDate: $("#tab2_fromDate").val(),
            ToDate: $("#tab2_toDate").val(),
            SearchString: $("#tab2_Search").val().trim()
        });

        if (data.Error) {
            ShowMessage(data.Error, "error");
            table2.clear().draw();
        } else {
            BaseResult = data;
            RenderTable2(data.IQCNGCustomer2List || []);
            ShowMessage(`Found ${data.IQCNGCustomer2List?.length || 0} records`, "ok");
        }
    } catch (err) {
        ShowMessage("Error: " + err.message, "error");
        console.error("Buttonfind_Click:", err);
    } finally {
        showLoading(false);
    }
}

function RenderTable2(dataList) {
    table2.clear();

    if (!dataList || dataList.length === 0) {
        $("#sum_Sample").text("0");
        $("#sum_NG").text("0");
        $("#sum_Percentage").text("0%");
        table2.draw();
        return;
    }

    let totalSample = 0;
    let totalNG = 0;

    // ✅ Dùng Set để track MaterialCode đã tính totalInvoice
    let totalInvoice = 0;
    const processedMaterialCodes = new Set();

    dataList.forEach((item) => {
        totalSample += parseFloat(item.Quantity) || 0;
        totalNG += parseFloat(item.NGQty) || 0;

        // ✅ Chỉ cộng totalInvoice nếu MaterialCode chưa được tính
        const materialCode = item.MaterialCode || '';
        if (materialCode && !processedMaterialCodes.has(materialCode)) {
            totalInvoice += parseFloat(item.TotalInvoice) || 0;
            processedMaterialCodes.add(materialCode);
        }

        // ... phần render table giữ nguyên ...
        const pictureHtml = item.Picture && item.Picture !== '-'
            ? `<img src="${item.Picture}" style="width:50px;height:50px;cursor:pointer;object-fit:cover;border-radius:4px;" onclick="showPictureModal('${item.Picture}')" />`
            : '-';

        const percentage = item.Percentage ? item.Percentage.toFixed(2) + '%' :
            (item.NGQty && item.Quantity) ? ((item.NGQty / item.Quantity) * 100).toFixed(2) + '%' : '0%';

        let improvementRaw = (item.ImprovementPlans || 'waiting feedback').toLowerCase().trim();
        let improvementClass = 'orange-text';
        let improvementText = 'Waiting Feedback';

        if (improvementRaw === 'return') {
            improvementClass = 'red-text';
            improvementText = 'Return';
        } else if (improvementRaw === 'use') {
            improvementClass = 'green-text';
            improvementText = 'Use';
        }

        let noteRaw = (item.Note || 'On going').toLowerCase().trim();
        let noteClass = noteRaw === 'closed' ? 'blue-text' : 'teal-text';
        let noteText = noteRaw === 'closed' ? 'Closed' : 'On Going';

        const checkboxHtml = `<label>
            <input type="checkbox" class="row-checkbox" data-id="${item.ID}" />
            <span></span>
        </label>`;

        const editBtn = `<button class="btn btn-small btn-edit-history cyan" data-id="${item.ID}" title="Edit">
            <i class="material-icons">edit</i>
        </button>`;

        table2.row.add([
            checkboxHtml,
            editBtn,
            item.SupplierCode || '-',
            item.InvoiceName || '-',
            item.Barcode || '-',
            item.MaterialCode || '-',
            item.Quantity || 0,
            item.TotalInvoice || 0,
            item.NGQty || 0,
            item.Unit || 'PCS',
            percentage,
            item.InputDate ? new Date(item.InputDate).toLocaleDateString() : '-',
            item.IssueDate ? new Date(item.IssueDate).toLocaleDateString() : '-',
            item.Week || '-',
            item.Month || '-',
            item.ErrorInfo || '-',
            pictureHtml,
            `<span class="${improvementClass}"><b>${improvementText}</b></span>`,
            `<span class="${noteClass}"><b>${noteText}</b></span>`,
            item.CreateUser || '-',
            item.CreateDate ? new Date(item.CreateDate).toLocaleString() : '-'
        ]).node().dataset.item = JSON.stringify(item);
    });

    const totalPercentage = totalInvoice > 0 ? ((totalNG / totalInvoice) * 100).toFixed(2) : 0;
    $("#sum_Sample").text(totalSample.toLocaleString());
    $("#sum_TotalInvoice").text(totalInvoice.toLocaleString());
    $("#sum_NG").text(totalNG.toLocaleString());
    $("#sum_Percentage").text(totalPercentage + "%");

    table2.draw(false);
}

function showPictureModal(picturePath) {
    if (!picturePath || picturePath === '-') return;
    $("#modalImage").attr("src", picturePath);
    $("#pictureModal").show();
}

function EditHistory(id) {
    let row = table2.rows().nodes().toArray().find(r =>
        $(r).find('.btn-edit-history').data('id') == id
    );

    if (!row) return;

    CurrentHistoryData = JSON.parse(row.dataset.item);

    $("#view_Barcode").text(CurrentHistoryData.Barcode || '-');
    $("#view_MaterialCode").text(CurrentHistoryData.MaterialCode || '-');
    $("#view_Quantity").text(CurrentHistoryData.Quantity || '-');
    $("#edit_ID").val(CurrentHistoryData.ID);
    $("#edit_Quantity").val(CurrentHistoryData.Quantity);
    $("#edit_NGQty").val(CurrentHistoryData.NGQty);

    let unit = (CurrentHistoryData.Unit || 'PC').toUpperCase();
    if (unit !== 'PC' && unit !== 'MT') {
        unit = 'PC'; 
    }
    $("#edit_Unit").val(unit);

    $("#edit_ImprovementPlans").val((CurrentHistoryData.ImprovementPlans || 'waiting feedback').toLowerCase());
    $("#edit_Note").val(CurrentHistoryData.Note || 'On going');
    $("#edit_ErrorInfo").val(CurrentHistoryData.ErrorInfo || '');

    $("#modal-edit-history").modal('open');

    setTimeout(() => {
        $("#edit_Quantity").focus().select();
    }, 300);
}

async function SaveHistory() {
    const quantity = parseInt($("#edit_Quantity").val());
    const ngQty = parseInt($("#edit_NGQty").val());
    const unit = $("#edit_Unit").val();
    const errorInfo = $("#edit_ErrorInfo").val().trim();

    if (!quantity || quantity < 0) {
        ShowMessage("Sample quantity invalid!", "error");
        return;
    }

    if (!ngQty || ngQty < 0) {
        ShowMessage("NG Qty invalid!", "error");
        return;
    }

    if (ngQty > quantity) {
        ShowMessage("NG Qty cannot exceed Sample quantity!", "error");
        return;
    }

    if (!unit) {
        ShowMessage("Unit is required!", "error");
        $("#edit_Unit").focus();
        return;
    }

    if (!errorInfo) {
        ShowMessage("Error Information required!", "error");
        return;
    }

    try {
        showLoading(true);

        const data = await callApi("/F03/Buttonsave_Click", {
            IQCNGCustomer2: {
                ID: parseInt($("#edit_ID").val()),
                Quantity: quantity,
                NGQty: ngQty,
                Unit: unit, 
                ImprovementPlans: $("#edit_ImprovementPlans").val(),
                Note: $("#edit_Note").val(),
                ErrorInfo: errorInfo
            },
            USER_IDX: GetCookieValue("UserID") || ""
        });

        if (data.Error) {
            ShowMessage(data.Error, "error");
        } else {
            ShowMessage("Update OK!", "ok");
            $("#modal-edit-history").modal('close');
            Buttonfind_Click();
        }
    } catch (err) {
        ShowMessage("Error: " + err.message, "error");
    } finally {
        showLoading(false);
    }
}

async function Buttondelete_Click() {
    const checkedIds = [];
    $('#Table2 .row-checkbox:checked').each(function () {
        checkedIds.push(parseInt($(this).data('id')));
    });

    if (checkedIds.length === 0) {
        ShowMessage("Please select at least 1 record to delete!", "warning");
        return;
    }

    if (!confirm(`Are you sure you want to delete ${checkedIds.length} selected record(s)?`)) {
        return;
    }

    try {
        showLoading(true);

        const data = await callApi("/F03/Buttondelete_Click", {
            Ids: checkedIds
        });

        if (data.Error) {
            ShowMessage(data.Error, "error");
        } else {
            ShowMessage("Delete successfully!", "ok");
            $('#SelectAllCheckbox').prop('checked', false);
            Buttonfind_Click();
        }
    } catch (err) {
        ShowMessage("Error: " + err.message, "error");
    } finally {
        showLoading(false);
    }
}

function Buttonadd_Click() {
}

async function Buttonsave_Click() {
    const lotCode = $("#txt_LotCode").text().trim();
    const ngQty = parseFloat($("#error_Qty").val());
    const errorInfo = $("#error_Infor").val().trim();

    // ✅ CHỈ CHECK 3 FIELD CHÍNH
    if (!lotCode || lotCode === "-") {
        ShowMessage("Please scan lot code first!", "error");
        $("#tab1_QRCodeScan").focus();
        return;
    }

    if (!errorInfo) {
        ShowMessage("Please enter error information!", "error");
        $("#error_Infor").focus();
        return;
    }

    // ✅ Confirm
    if (!confirm(`Save NG?\nLot: ${lotCode}\nNG Qty: ${ngQty}`)) {
        return;
    }

    showLoading(true);
    await UploadPicture();
    await SaveNG();
    showLoading(false);
}
async function SaveNG() {
    const id = $('#txt_ID').text().trim();
    const lotCode = $('#txt_LotCode').text().trim();
    const productCode = $('#txt_ProductCode').text().trim();
    const productName = $('#txt_ProductName').text().trim();
    const invoice = $('#txt_Invoice').text().trim();
    const supplier = $('#txt_Supplier').text().trim();
    const total = parseFloat($('#txt_Total').val().trim()) || 0;
    const totalInvoice = parseFloat($('#txt_TotalInvoice').text().trim()) || 0;
    const totalNG = parseFloat($('#error_Qty').val().trim()) || 0;
    const errorInfo = $('#error_Infor').val().trim();
    const picture = $('#txt_Picture').text().trim();
    const unit = $('#txt_Unit').text().trim();

    try {
        const data = await callApi("/F03/Buttonadd_Click", {
            IQCNGCustomer2: {
                WarehouseInputDetailBarcodeID: (id && id !== '-') ? parseInt(id) : null,
                Barcode: lotCode,
                MaterialCode: (productCode !== '-') ? productCode : '',
                MaterialName: (productName !== '-') ? productName : '',
                InvoiceName: (invoice !== '-') ? invoice : '',
                SupplierCode: (supplier !== '-') ? supplier : '',
                Quantity: total > 0 ? total : totalNG,
                TotalInvoice: totalInvoice > 0 ? totalInvoice : (total > 0 ? total : totalNG),
                NGQty: totalNG,
                Unit: (unit !== '-') ? unit : 'PC',
                ErrorInfo: errorInfo,
                Picture: (picture !== '-') ? picture : ''
            },
            USER_IDX: GetCookieValue("UserID") || ""
        });

        if (data.Error) {
            ShowMessage(data.Error, "error");
        } else {
            ShowMessage("Save successfully!", "ok");
            clearForm();
        }
    } catch (err) {
        ShowMessage("Error: " + err.message, "error");
    }
}

function clearForm() {
    $('#txt_ID, #txt_LotCode, #txt_ProductCode, #txt_ProductName, #txt_Invoice, #txt_Supplier, #txt_Unit, #txt_Picture, #txt_TotalInvoice').text('-');
    $('#error_Qty, #error_Infor, #tab1_QRCodeScan, #txt_Total').val('');
    $('#pictureUpload').attr('src', '');
    selectedFile = null;
    $('#tab1_QRCodeScan').focus();
}

async function ScanLotcode() {
    const lotcode = $("#tab1_QRCodeScan").val().trim();

    if (!lotcode) {
        PlayErrorSound();
        ShowMessage("Please scan a lot code", "warning");
        return;
    }

    try {
        showLoading(true);

        const response = await fetch("/F01/Buttonadd_Click", {
            method: "POST",
            headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
            body: `BaseParameter=${encodeURIComponent(JSON.stringify({ LotCode: lotcode }))}`
        });

        if (!response.ok) {
            throw new Error(`Server error: ${response.status}`);
        }

        const data = await response.json();
        const dataArray = findDataArray(data);
        if (dataArray && dataArray.length > 0) {
            PlaySuccessSound();
            ShowMessage("✅ Lot code found in system", "ok");
            updateUIWithLotcodeInfo(getDisplayItem(dataArray[0]));
        }
        else {
            ShowMessage("⚠️ Lot code not in system. Please input manually", "warning");
            $("#txt_ID").text('-');
            $("#txt_LotCode").text(lotcode); 
            $("#txt_ProductCode").text('-');
            $("#txt_ProductName").text('-');
            $("#txt_Invoice").text('-');
            $("#txt_Supplier").text('-');
            $("#txt_Unit").text('PC'); 
            $("#txt_TotalInvoice").text('-');
        }
        $("#tab1_QRCodeScan").val("");
        $("#error_Infor").focus();

    } catch (error) {
        PlayErrorSound();
        ShowMessage(`Error: ${error.message}`, "error");
        console.error("ScanLotcode error:", error);
        $("#tab1_QRCodeScan").select(); 
    } finally {
        showLoading(false);
    }
}

function findDataArray(data) {
    if (Array.isArray(data)) return data;
    const keys = ['NGDataList', 'Data', 'List', 'Items', 'Results'];
    for (const key of keys) {
        if (data[key] && Array.isArray(data[key])) return data[key];
    }
    return null;
}

function getDisplayItem(item) {
    if (!item) return {};
    if (Array.isArray(item) && item.length > 0) return item[0];
    if (typeof item === 'object') return item;
    return {};
}

function updateUIWithLotcodeInfo(item) {
    $("#txt_ID").text(item.ID || '-');
    $("#txt_ProductCode").text(item.MaterialCode || '-');
    $("#txt_LotCode").text(item.Barcode || item.LotCode || '-');
    $("#txt_ProductName").text(item.MaterialName || item.Name || '-');
    $("#txt_Invoice").text(item.InvoiceName || item.InvoiceNo || '-');
    $("#txt_Supplier").text(item.SupplierName || item.VendorName || '-');
    $("#txt_Total").val(item.Quantity || item.TotalQty || item.Qty || '');
    $("#txt_TotalInvoice").text(item.QuantityInvoice || '-');
    $("#txt_Unit").text(item.CategoryUnitName || 'PC'); 
}

function clearLotcodeFields() {
    $("#txt_ID, #txt_ProductCode, #txt_LotCode, #txt_ProductName, #txt_Invoice, #txt_Supplier, #txt_TotalInvoice, #txt_Unit").text('-');
    $("#txt_Total").val('');
    $("#tab1_QRCodeScan").focus();
}

async function UploadPicture() {
    if (!selectedFile) {
        $("#txt_Picture").text('-');
        return;
    }

    const formData = new FormData();
    formData.append("file", selectedFile, `IQCNG_C2_${Date.now()}.jpg`);

    try {
        const response = await fetch("/F03/UploadPhoto", {
            method: "POST",
            body: formData
        });
        const result = await response.json();

        if (result.success) {
            ShowMessage("Upload picture successfully", "ok");
            $("#pictureUpload").attr('src', result.path);
            $("#txt_Picture").text(result.path);
            selectedFile = null;
        } else {
            ShowMessage("Upload picture failed", "error");
            $("#txt_Picture").text('-');
        }
    } catch (err) {
        ShowMessage("Upload error: " + err.message, "error");
        $("#txt_Picture").text('-');
    }
}

function Buttoncancel_Click() {
    clearForm();
}

function Buttonhelp_Click() {
    OpenWindowByURL("/WMP_PLAY", 800, 460);
}

function Buttonclose_Click() {
    history.back();
}