let BaseResult = {};
let controller = "/D17/";

function GetCookieValue(name) {
    const match = document.cookie.match(new RegExp('(^| )' + name + '=([^;]+)'));
    return match ? match[2] : null;
}
function PlaySuccessSound() {
    try {
        let audio = new Audio('/audio/ok.mp3');
        audio.play().catch(e => console.log('Cannot play success sound:', e));
    } catch (e) {
        console.log('Success sound not available');
    }
}

function PlayErrorSound() {
    try {
        let audio = new Audio('/audio/ng.mp3');
        audio.play().catch(e => console.log('Cannot play error sound:', e));
    } catch (e) {
        console.log('Error sound not available');
    }
}
function getCurrentUser() {
    return GetCookieValue('UserID') || 'SYSTEM';
}

$(document).ready(function () {
    $('.modal').modal();
    $('.tabs').tabs();

    // Auto focus scan input when tab becomes active
    $('#Tab_Receive').on('click', function () {
        setTimeout(() => $("#ScanBarcode").focus(), 200);
    });

    // Button handlers
    $("#Buttonfind").click(Buttonfind_Click);
    $("#Buttonexport").click(Buttonexport_Click);
    $("#Buttonprint").click(Buttonprint_Click);
    $("#Buttonhelp").click(Buttonhelp_Click);
    $("#Buttonclose").click(Buttonclose_Click);

    // Tab-specific search triggers
    $("#Filter_PartNo, #Filter_PackingLot, #Filter_CustomerCode").on("keypress", function (e) {
        if (e.which === 13) SearchInventory();
    });

    $("#History_FromDate, #History_ToDate, #History_FilterType, #History_PartNo, #History_PackingLot").on("change keypress", function (e) {
        if (e.type === 'change' || e.which === 13) SearchHistory();
    });

    // Scan barcode handler
    $("#ScanBarcode").on('keydown', function (e) {
        if (e.keyCode === 13) {
            e.preventDefault();
            const barcode = $(this).val().trim().toUpperCase();
            if (barcode) {
                ScanReceiveBarcode(barcode);
            }
        }
    });

    // Initial focus
    $("#ScanBarcode").focus();

    // Click packing lot to view detail
    $("#Table_Inventory").on('click', '.packing-lot-link', function (e) {
        e.preventDefault();
        OpenPackingDetailModal($(this).data('packing-lot'));
    });
    $('#Tab_Report').on('click', function () {
        SearchReport();
    });
});

function ShowLoading(flag = true) {
    $("#BackGround").css("display", flag ? "block" : "none");
}

function ShowMessage(message, type) {
    if (type === 'error') {
        M.toast({ html: message, classes: 'red' });
        PlayErrorSound();
    } else if (type === 'warning') {
        M.toast({ html: message, classes: 'orange' });
        PlayErrorSound();
    } else {
        M.toast({ html: message, classes: 'green' });
        PlaySuccessSound();
    }
}

function ApiCall(action, param = {}) {
    ShowLoading(true);
    if (!param.USER_ID) param.USER_ID = getCurrentUser();

    let formData = new FormData();
    formData.append("BaseParameter", JSON.stringify(param));

    return fetch(`${controller}${action}`, { method: "POST", body: formData })
        .then(r => r.json())
        .finally(() => ShowLoading(false));
}

function Buttonfind_Click() {
    const activeTab = $('.tabs .tab a.active').attr('href');

    if (activeTab === '#Tab_Receive') {
        ClearReceiveForm();
        $("#ScanBarcode").focus();
    } else if (activeTab === '#Tab_Inventory') {
        SearchInventory();
    } else if (activeTab === '#Tab_History') {
        SearchHistory();
    }
}

function Buttonexport_Click() {
    const activeTab = $('.tabs .tab a.active').attr('href');

    if (activeTab === '#Tab_Inventory') {
        TableHTMLToExcel("Table_Inventory", "FG_Inventory", "D17_Inventory.xls");
    } else if (activeTab === '#Tab_History') {
        TableHTMLToExcel("Table_History", "FG_History", "D17_History.xls");
    }
}

function Buttonprint_Click() {
    const activeTab = $('.tabs .tab a.active').attr('href');

    if (activeTab === '#Tab_Receive') {
        const packingLot = $("#Info_PackingLot").val();
        if (packingLot) {
            window.print();
        } else {
            ShowMessage("No packing lot to print", "warning");
        }
    }
}

function Buttonhelp_Click() {
    window.open("/WMP_PLAY", "_blank", "width=800,height=460");
}

function Buttonclose_Click() {
    history.back();
}

// ========== TAB 1: FG RECEIVE ==========

function ClearReceiveForm() {
    $("#ScanBarcode").val("");
    $("#TBody_LotDetail").html('<tr><td colspan="9" class="center-align grey-text">No data</td></tr>');
}

async function ScanReceiveBarcode(barcode) {
    if (!barcode) return;

    const cleanBarcode = CleanString(barcode);

    if (cleanBarcode.includes("3S")) {
        await ReceivePackingLot(cleanBarcode);
    } else {
        ShowMessage("Invalid packing lot format", "error");
    }

    $("#ScanBarcode").val("").focus();
}

async function LoadPackingDetail(packingLot) {
    try {
        const data = await ApiCall("Buttonfind_Click", {
            Code: "LoadPackingDetail",
            Barcode: packingLot
        });

        if (data.ErrorNumber === 0 && data.tfg_inventory) {
            if (data.tfg_packing_detailList && data.tfg_packing_detailList.length > 0) {
                RenderLotDetail(data.tfg_packing_detailList);
            }
            ShowMessage("Packing lot loaded successfully", "ok");
        } else {
            ShowMessage(data.Message || "Packing lot not found", "error");
            ClearReceiveForm();
        }
    } catch (err) {
        ShowMessage("Error: " + err.message, "error");
    }
}

async function ReceivePackingLot(packingLot) {
    try {
        const data = await ApiCall("Buttonfind_Click", {
            Code: "ScanReceive",
            Barcode: packingLot,
            USER_ID: getCurrentUser(),
            SkipSNPCheck: $("#SkipSNPCheck").is(':checked')
        });

        if (data.ErrorNumber === 0) {
            ShowMessage(data.Message || "Received successfully", "ok");
            await LoadPackingDetail(packingLot);
        } else if (data.ErrorNumber === -2) {
            ShowMessage(data.Message || "Already received", "warning");
            await LoadPackingDetail(packingLot);
        } else if (data.ErrorNumber === -3) {
            ShowMessage(data.Message || "Packing lot not found in QC", "error");
        } else if (data.ErrorNumber === -4) {
            ShowMessage(data.Message || "Quantity mismatch", "error");
        } else if (data.ErrorNumber === -5) {
            ShowMessage(data.Message || "Already shipped", "error");
        } else {
            ShowMessage(data.Message || data.Error || "Error occurred", "error");
        }
    } catch (err) {
        ShowMessage("Error: " + err.message, "error");
    }
}

function RenderLotDetail(data) {
    const tbody = $("#TBody_LotDetail");
    tbody.empty();

    if (!data || data.length === 0) {
        tbody.append('<tr><td colspan="9" class="center-align grey-text">No data</td></tr>');
        return;
    }

    data.forEach((item, index) => {
        const reworkDisplay = item.REWORK_YN === "Y"
            ? '<span style="color:red; font-weight:bold;">Y</span>'
            : 'N';

        tbody.append(`
            <tr>
                <td>${index + 1}</td>
                <td class="preserve-space">${item.LOT_CODE || ""}</td>
                <td>${item.PART_NO || ""}</td>
                <td>${item.ECN_NO || ""}</td>
                <td>${reworkDisplay}</td>
                <td>${FormatDateTime(item.QC_SCAN_DATE)}</td>
                <td>${item.QC_USER || ""}</td>
                <td>${FormatDateTime(item.FG_IN_DATE)}</td>
                <td>${item.FG_IN_USER || ""}</td>
            </tr>
        `);
    });
}

// ========== TAB 2: INVENTORY (GROUPED BY PART_NO) ==========

async function SearchInventory() {
    try {
        const data = await ApiCall("Buttonfind_Click", {
            Code: "SearchInventory",
            PartNo: $("#Filter_PartNo").val().trim(),
            PackingLotCode: $("#Filter_PackingLot").val().trim(),
            CustomerCode: $("#Filter_CustomerCode").val().trim()
        });

        if (data.Success && data.tfg_inventoryList) {
            RenderInventory(data.tfg_inventoryList);
            $("#TotalInventory").text(data.tfg_inventoryList.length);
            ShowMessage(data.Message || "Search completed", "ok");
        } else {
            RenderInventory([]);
            $("#TotalInventory").text("0");
            ShowMessage(data.Error || "No data found", "warning");
        }
    } catch (err) {
        ShowMessage("Error: " + err.message, "error");
    }
}

function RenderInventory(data) {
    const tbody = $("#TBody_Inventory");
    tbody.empty();

    if (!data || data.length === 0) {
        tbody.append('<tr><td colspan="11" class="center-align grey-text">No data</td></tr>');
        return;
    }

    // Group by PART_NO
    const grouped = {};
    data.forEach(item => {
        const key = item.PART_NO;
        if (!grouped[key]) {
            grouped[key] = {
                PART_NO: item.PART_NO,
                PART_NM: item.PART_NM,
                CUSTOMER_CODE: item.CUSTOMER_CODE,
                SNP_QTY: 0,
                ACTUAL_QTY: 0,
                REWORK_QTY: 0,
                PACKING_LOTS: [],
                LATEST_DATE: item.FG_IN_DATE,
                LATEST_USER: item.FG_IN_USER,
                QC_DATE: item.QC_DATE
            };
        }
        grouped[key].SNP_QTY += item.SNP_QTY || 0;
        grouped[key].ACTUAL_QTY += item.ACTUAL_QTY || 0;
        grouped[key].REWORK_QTY += item.REWORK_QTY || 0;
        grouped[key].PACKING_LOTS.push(item.PACKING_LOT);

        // Keep latest date
        if (new Date(item.FG_IN_DATE) > new Date(grouped[key].LATEST_DATE)) {
            grouped[key].LATEST_DATE = item.FG_IN_DATE;
            grouped[key].LATEST_USER = item.FG_IN_USER;
        }
    });

    // Render grouped data
    Object.values(grouped).forEach((item) => {
        const reworkDisplay = item.REWORK_QTY > 0
            ? `<span style="color:red; font-weight:bold;">${item.REWORK_QTY}</span>`
            : item.REWORK_QTY || 0;

        const packingLotDisplay = item.PACKING_LOTS.length === 1
            ? `<a href="#" class="packing-lot-link" data-packing-lot="${item.PACKING_LOTS[0]}" style="font-weight:bold; color:blue; text-decoration:underline;">${item.PACKING_LOTS[0]}</a>`
            : `<span style="font-weight:bold; color:blue;">${item.PACKING_LOTS.length} boxes</span>`;

        tbody.append(`
            <tr>
                <td>${packingLotDisplay}</td>
                <td>${item.PART_NO || ""}</td>
                <td>${item.PART_NM || ""}</td>
                <td class="right-align">${item.SNP_QTY || ""}</td>
                <td class="right-align" style="font-weight:bold; color:green;">${item.ACTUAL_QTY || ""}</td>
                <td class="right-align">${reworkDisplay}</td>
                <td style="font-weight:bold; color:red;">${item.CUSTOMER_CODE || ""}</td>
                <td>${FormatDateTime(item.LATEST_DATE)}</td>
                <td>${item.LATEST_USER || ""}</td>
                <td>${FormatDateTime(item.QC_DATE)}</td>
                <td><span style="color:green; font-weight:bold;">IN</span></td>
            </tr>
        `);
    });
}

// ========== TAB 3: HISTORY ==========

async function SearchHistory() {
    try {
        const fromDate = $("#History_FromDate").val();
        const toDate = $("#History_ToDate").val();

        if (!fromDate || !toDate) {
            ShowMessage("Please select date range", "warning");
            return;
        }
        const fromDateTime = fromDate + " 00:00:00";
        const toDateTime = toDate + " 23:59:59";

        const data = await ApiCall("Buttonfind_Click", {
            Code: "SearchHistory",
            FromDate: fromDateTime,
            ToDate: toDateTime,
            FilterType: $("#History_FilterType").val(),
            PartNo: $("#History_PartNo").val().trim(),
            PackingLotCode: $("#History_PackingLot").val().trim()
        });

        if (data.Success && data.tfg_historyList) {
            RenderHistory(data.tfg_historyList);
            $("#TotalHistory").text(data.tfg_historyList.length);
            ShowMessage(data.Message || "Search completed", "ok");
        } else {
            RenderHistory([]);
            $("#TotalHistory").text("0");
            ShowMessage(data.Error || "No data found", "warning");
        }
    } catch (err) {
        ShowMessage("Error: " + err.message, "error");
    }
}

function RenderHistory(data) {
    const tbody = $("#TBody_History");
    tbody.empty();

    if (!data || data.length === 0) {
        tbody.append('<tr><td colspan="14" class="center-align grey-text">No data</td></tr>');
        return;
    }

    data.forEach((item, index) => {
        const transTypeColor = item.TRANS_TYPE === 'IN' ? 'green' : 'orange';
        const transTypeDisplay = `<span style="color:${transTypeColor}; font-weight:bold;">${item.TRANS_TYPE}</span>`;

        const reworkDisplay = item.REWORK_QTY > 0
            ? `<span style="color:red; font-weight:bold;">${item.REWORK_QTY}</span>`
            : item.REWORK_QTY || 0;

        tbody.append(`
            <tr>
                <td>${index + 1}</td>
                <td>${transTypeDisplay}</td>
                <td style="font-weight:bold; color:blue;">${item.PACKING_LOT || ""}</td>
                <td>${item.PART_NO || ""}</td>
                <td>${item.PART_NM || ""}</td>
                <td class="right-align">${item.SNP_QTY || ""}</td>
                <td class="right-align" style="font-weight:bold; color:green;">${item.ACTUAL_QTY || ""}</td>
                <td class="right-align">${reworkDisplay}</td>
                <td style="font-weight:bold; color:red;">${item.CUSTOMER_CODE || ""}</td>
                <td>${item.SHIPPING_NO || ""}</td>
                <td>${item.DESTINATION || ""}</td>
                <td>${FormatDateTime(item.TRANS_DATE)}</td>
                <td>${item.TRANS_USER || ""}</td>
                <td>${item.REMARK || ""}</td>
            </tr>
        `);
    });
}
// ========== TAB 4: REPORT ==========

async function SearchReport() {
    try {
        const now = new Date();
        const year = now.getFullYear();
        const month = now.getMonth() + 1; // 1-12

        const data = await ApiCall("Buttonfind_Click", {
            Code: "SearchReport",
            Year: year,
            Month: month
        });

        if (data.Success && data.ReportData) {
            RenderReport(data.ReportData);
            ShowMessage(`Found ${data.ReportData.length} records`, "ok");
        } else {
            RenderReport([]);
            ShowMessage(data.Error || "No data found", "warning");
        }
    } catch (err) {
        ShowMessage("Error: " + err.message, "error");
    }
}

function RenderReport(data) {
    const tbody = $("#TBody_Report");
    tbody.empty();

    if (!data || data.length === 0) {
        tbody.append('<tr><td colspan="35" class="center-align grey-text">No data</td></tr>');
        return;
    }

    data.forEach((item) => {
        let row = `
            <tr>
                <td>${item.PART_NO || ""}</td>
                <td>${item.PART_NM || ""}</td>
                <td class="right-align">${item.SNP_QTY || 0}</td>
        `;

        let rowTotal = 0;

        // Generate 31 day columns
        for (let day = 1; day <= 31; day++) {
            const dayKey = "DAY_" + day.toString().padStart(2, '0');
            const qty = item[dayKey] || 0;
            rowTotal += qty;

            const cellStyle = qty > 0
                ? 'style="background-color: #e8f5e9; font-weight: bold; color: #2e7d32;"'
                : '';

            row += `<td ${cellStyle}>${qty > 0 ? qty : '-'}</td>`;
        }

        // Total column
        row += `<td style="background-color: #fff3e0; font-weight: bold; color: #e65100;">${rowTotal}</td>`;
        row += `</tr>`;

        tbody.append(row);
    });
}
// ========== PACKING DETAIL MODAL ==========

async function OpenPackingDetailModal(packingLot) {
    $("#Detail_PackingLot").text(packingLot);
    $("#TBody_PackingDetail").html('<tr><td colspan="9" class="center-align">Loading...</td></tr>');
    $("#Modal_PackingDetail").modal('open');

    const data = await ApiCall("Buttonfind_Click", { Code: "LoadPackingDetail", Barcode: packingLot });

    const list = data.tfg_packing_detailList || [];
    $("#Detail_LotCount").text(list.length);

    const tbody = $("#TBody_PackingDetail").empty();
    if (!list.length) {
        tbody.append('<tr><td colspan="9" class="center-align grey-text">No data</td></tr>');
        return;
    }
    list.forEach((item, i) => {
        tbody.append(`<tr>
            <td>${i + 1}</td>
            <td class="preserve-space">${item.LOT_CODE || ""}</td>
            <td>${item.PART_NO || ""}</td>
            <td>${item.ECN_NO || ""}</td>
            <td>${item.REWORK_YN === "Y" ? '<span style="color:red;font-weight:bold;">Y</span>' : 'N'}</td>
            <td>${FormatDateTime(item.QC_SCAN_DATE)}</td>
            <td>${item.QC_USER || ""}</td>
            <td>${FormatDateTime(item.FG_IN_DATE)}</td>
            <td>${item.FG_IN_USER || ""}</td>
        </tr>`);
    });
}

// ========== UTILITY FUNCTIONS ==========

function CleanString(input) {
    if (!input) return "";
    return input.replace(/[^\x00-\x7F]/g, '').trim();
}

function FormatDateTime(dateTimeStr) {
    if (!dateTimeStr) return "";
    const date = typeof dateTimeStr === 'string' ? new Date(dateTimeStr) : dateTimeStr;
    if (isNaN(date.getTime())) return dateTimeStr;
    return date.toISOString().substring(0, 16).replace('T', ' ');
}

function FormatDate(dateTimeStr) {
    if (!dateTimeStr) return "";
    const date = typeof dateTimeStr === 'string' ? new Date(dateTimeStr) : dateTimeStr;
    if (isNaN(date.getTime())) return dateTimeStr;
    return date.toISOString().substring(0, 10);
}