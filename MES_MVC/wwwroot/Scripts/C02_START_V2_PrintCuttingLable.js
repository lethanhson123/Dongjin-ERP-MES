// ============== GLOBAL PRINT STATE ==============
let isPrintingNow = false;
let printRetryCount = 0;
const MAX_PRINT_RETRIES = 3;

// ============== MAIN PRINT FUNCTIONS ==============

// in tem nhãn co cutting - Optimized version
function C02_START_V2_Buttonprint_Click() {
    console.log("=== PRINT FLOW START ===");

    try {
        IsC02_SPC_PageLoad = false;
        let IsCheck = true;

        // Kiểm tra barcode
        const barcode = $("#C02_START_V2_BARCODE_TEXT").val();
        if (barcode == "None") {
            showToast("NOT New Barcode. Please Check Again.", "red");
            return Promise.resolve(false);
        }

        C02_START_V2_BARCODE_QR = barcode;
        console.log("Barcode:", barcode);

        // Kiểm tra tool counter
        const TOOLA1 = Number($("#C02_START_V2_TOOL_CONT1").val());
        const TOOLA2 = Number($("#C02_START_V2_TOOL_CONT2").val());
        const TOOLC1 = BaseResult?.C02_START_V2_Search?.[0]?.TOOL_MAX1 ?? 1000000;
        const TOOLC2 = BaseResult?.C02_START_V2_Search?.[0]?.TOOL_MAX2 ?? 1000000;

        if (TOOLA1 >= TOOLC1 || TOOLA2 >= TOOLC2) {
            showToast("Tool counter limit reached. Please check.", "red");
            return Promise.resolve(false);
        }

        C02_START_V2_SPC_EXIT = true;

        // Kiểm tra điều kiện SPC
        if (checkSPCConditions()) {
            return Promise.resolve(false);
        }

        // Gọi hàm in
        console.log("Calling print sub function...");
        return C02_START_V2_Buttonprint_ClickSub();

    } catch (err) {
        console.error("Print main error:", err);
        return Promise.reject(err);
    }
}

// Helper function kiểm tra SPC conditions - Optimized
function checkSPCConditions() {
    try {
        const searchData = BaseResult?.C02_START_V2_Search?.[0];
        if (!searchData) return false;

        const spc1 = document.getElementById("C02_START_V2_SPC1")?.innerHTML;
        const spc2 = document.getElementById("C02_START_V2_SPC2")?.innerHTML;
        const spc3 = document.getElementById("C02_START_V2_SPC3")?.innerHTML;

        let spcCondition = "";

        if (spc1 == "First") {
            spcCondition = "First";
        } else if (spc3 == "End" && searchData.TOT_QTY <= searchData.BUNDLE_SIZE &&
            searchData.WORK_QTY < searchData.TOT_QTY) {
            spcCondition = "End";
        } else if (spc2 == "Middle" && searchData.TOT_QTY > 500 &&
            searchData.TOT_QTY / 2 <= searchData.PERFORMN) {
            spcCondition = "Middle";
        } else if (spc3 == "End") {
            const A_CSPC = searchData.TOT_QTY - searchData.PERFORMN;
            if (searchData.BUNDLE_SIZE >= A_CSPC) {
                spcCondition = "End";
            }
        }

        if (spcCondition) {
            IsC02_SPC_PageLoad = true;
            C02_START_V2_SPC_EXIT = false;
            $("#C02_SPC_Label10").val(spcCondition);
            $("#C02_SPC").modal("open");
            C02_SPC_PageLoad();
            return true;
        }

        return false;
    } catch (error) {
        console.error("Error checking SPC:", error);
        return false;
    }
}

// in tem cho đơn cắt - Optimized
function C02_START_V2_Buttonprint_ClickSub() {
    return new Promise((resolve, reject) => {
        console.log("=== PRINT SUB FUNCTION ===");

        if (!C02_START_V2_SPC_EXIT) {
            showToast("검사 누락. Kiểm tra mất tích.", "red");
            return resolve(false);
        }

        if (isPrintingNow) {
            console.warn("Print in progress");
            showToast("Đang in, vui lòng đợi...", "orange");
            return resolve(false);
        }

        printRetryCount = 0;

        try {
            const barcodePrint = $("#C02_START_V2_BARCODE_TEXT").val();
            console.log("Print barcode:", barcodePrint);

            if (!barcodePrint || barcodePrint.length <= 10) {
                showToast("Invalid barcode", "red");
                return resolve(false);
            }

            // Build và gửi data
            sendPrintData(barcodePrint)
                .then(data => executePrintFlow(data))
                .then(() => resolve(true))
                .catch(reject);

        } catch (e) {
            console.error("Print sub error:", e);
            $("#BackGround").css("display", "none");
            reject(e);
        }
    });
}

// Gửi data để in - Optimized
function sendPrintData(barcodePrint) {
    const BaseParameter = {
        ListSearchString: [
            $("#C02_START_V2_Label5").val() || "",
            barcodePrint,
            SHIELDWIRE_CHK || "",
            $("#C02_START_V2_VLA1").val() || "",
            $("#C02_START_V2_TOOL1_IDX").val() || "",
            BaseResult?.C02_START_V2_Search?.[0]?.T1_CONT || "",
            $("#C02_START_V2_Label4").val() || "",
            $("#C02_START_V2_VLA2").val() || "",
            $("#C02_START_V2_TOOL2_IDX").val() || "",
            BaseResult?.C02_START_V2_Search?.[0]?.T2_CONT || "",
            $("#C02_START_V2_Label8").val() || "",
            BaseResult?.C02_START_V2_Search?.[0]?.PERFORMN || "",
            $("#MCbox").val() || "",
            $("#C02_START_V2_ProductCode").val() || ""
        ],
        USER_ID: GetCookieValue("UserID"),
        USER_IDX: GetCookieValue("USER_IDX")
    };

    const formUpload = new FormData();
    formUpload.append("BaseParameter", JSON.stringify(BaseParameter));

    console.log("Saving to server...");

    return fetch("/C02_START_V2/Buttonprint_Click", {
        method: "POST",
        body: formUpload
    })
        .then(handleResponse)
        .catch(err => {
            console.error("Server error:", err);
            throw err;
        });
}

// Xử lý response - Optimized
function handleResponse(response) {
    if (!response.ok) {
        throw new Error(`HTTP ${response.status}`);
    }

    const contentType = response.headers.get('content-type');
    if (!contentType?.includes('application/json')) {
        throw new Error("Invalid response format");
    }

    return response.json();
}

// Thực thi luồng in - Optimized
function executePrintFlow(data) {
    return new Promise((resolve) => {
        console.log("Executing print flow...");

        // 1. In tem
        C02_START_V2_PrintDocument1_PrintPage().catch(console.error);

        // 2. Cập nhật data
        if (BaseResult) {
            BaseResult.C02_START_V2_DataGridView5 = data?.DataGridView5 || [];
        }

        const dataCount = BaseResult?.C02_START_V2_DataGridView5?.length || 0;

        if (dataCount === 0) {
            C02_START_V2_Buttonclose_Click();
        } else {
            C02_START_V2_TI_CONT = 1;
            if (typeof C02_START_V2_Timer1StartInterval === 'function') {
                C02_START_V2_Timer1StartInterval();
            }

            try {
                C02_START_V2_ORDER_LOAD(1);
            } catch (e) {
                console.error("Order load error:", e);
            }
        }

        showToast("정상처리 되었습니다. Đã được lưu.", "green");
        $("#BackGround").css("display", "none");
        resolve();
    });
}

// ============== PRINT LABEL FUNCTION - Optimized ==============

function C02_START_V2_PrintDocument1_PrintPage() {
    return new Promise((resolve, reject) => {
        console.log("=== PRINT LABEL ===");

        // Check printing state
        if (isPrintingNow && printRetryCount < MAX_PRINT_RETRIES) {
            printRetryCount++;
            console.log(`Retry ${printRetryCount}/${MAX_PRINT_RETRIES}`);
            setTimeout(() => C02_START_V2_PrintDocument1_PrintPage().then(resolve).catch(reject), 1000);
            return;
        }

        if (isPrintingNow) {
            console.warn("Max retries reached");
            return resolve();
        }

        isPrintingNow = true;
        setTimeout(() => { isPrintingNow = false; }, 10000);

        const BARCODE_QR = $("#C02_START_V2_BARCODE_TEXT").val();
        if (!BARCODE_QR || BARCODE_QR.length <= 10) {
            isPrintingNow = false;
            showToast("Invalid barcode", "red");
            return resolve();
        }

        // Get label data và in
        fetchLabelData(BARCODE_QR)
            .then(openPrintWindow)
            .then(() => {
                isPrintingNow = false;
                $("#BackGround").css("display", "none");
                resolve();
            })
            .catch(err => {
                console.error("Label print error:", err);
                isPrintingNow = false;
                $("#BackGround").css("display", "none");
                reject(err);
            });
    });
}

// Lấy data label - Optimized
function fetchLabelData(BARCODE_QR) {
    const searchData = BaseResult?.C02_START_V2_Search?.[0];
    const params = {
        ListSearchString: [
            BARCODE_QR,
            $("#C02_START_V2_Label43").val() || "",
            $("#C02_START_V2_Label54").val() || "",
            $("#C02_START_V2_Label4").val() || "",
            $("#C02_START_V2_Label18").val() || "",
            $("#C02_START_V2_Label48").val() || "",
            $("#C02_START_V2_Label50").val() || "",
            $("#C02_START_V2_VLT1").val() || "",
            $("#C02_START_V2_VLT2").val() || "",
            $("#C02_START_V2_VLS1").val() || "",
            $("#C02_START_V2_VLS2").val() || "",
            $("#C02_START_V2_ST_DCC1").val() || "",
            $("#C02_START_V2_ST_DCC2").val() || "",
            $("#C02_START_V2_ST_DIC1").val() || "",
            $("#C02_START_V2_ST_DIC2").val() || "",
            $("#C02_START_V2_Label42").val() || "",
            $("#C02_START_V2_barsq").val() || "",
            $("#C02_START_V2_ST_DSTR1").val() || "",
            $("#C02_START_V2_Label34").val() || "",
            $("#C02_START_V2_Label77").val() || "",
            searchData?.OR_NO || "",
            $("#C02_START_V2_ProductCode").val() || "",
            $("#C02_START_V2_L_MCNM").val() || ""
        ],
        USER_ID: GetCookieValue("UserID"),
        USER_IDX: GetCookieValue("USER_IDX")
    };

    const formData = new FormData();
    formData.append('BaseParameter', JSON.stringify(params));

    return fetch("/C02_START_V2/PrintDocument1_PrintPage", {
        method: "POST",
        body: formData
    })
        .then(handleResponse)
        .then(data => data.DataGridView1?.[0] || Promise.reject("No label data"));
}

// Mở cửa sổ in - Optimized
function openPrintWindow(labelData) {
    console.log("Opening print window...");

    const params = new URLSearchParams();
    const fields = [
        'Barcode', 'D01', 'D02', 'D03', 'D04', 'D05', 'D06', 'D07', 'D08', 'D09',
        'D10', 'D11', 'D12', 'D13', 'D14', 'D15', 'D16', 'D17', 'D18', 'D19',
        'D20', 'D21', 'D22', 'D23', 'D24', 'D25', 'D26', 'D27'
    ];

    fields.forEach((field, index) => {
        const value = labelData[field];
        if (value !== undefined && value !== null) {
            const paramName = index === 0 ? 'QRCodeString' : `PR${index}`;
            params.append(paramName, value);
        }
    });

    const printUrl = `/html/C02Lable.html?${params.toString()}`;
    const printWindow = window.open(printUrl, '_blank',
        'width=800,height=600,scrollbars=yes,resizable=yes');

    if (!printWindow) {
        console.warn("Popup blocked");
        showPopupWarning();
        openPrintWithIframe(printUrl);
    }

    return Promise.resolve();
}

// Phương pháp backup - Optimized
function openPrintWithIframe(printUrl) {
    const iframe = document.createElement('iframe');
    iframe.style.display = 'none';
    iframe.src = printUrl;

    iframe.onload = function () {
        setTimeout(() => {
            try {
                iframe.contentWindow.print();
            } catch (e) {
                console.error("Iframe print failed:", e);
            }
            setTimeout(() => iframe.remove(), 5000);
        }, 500);
    };

    document.body.appendChild(iframe);
}

// ============== HELPER FUNCTIONS ==============

function showToast(message, color = "blue") {
    console.log(`Toast: ${message}`);
    if (typeof M !== 'undefined' && M.toast) {
        M.toast({ html: message, classes: color, displayLength: 6000 });
    } else {
        console.warn("Toast:", message);
    }
}

function showPopupWarning() {
    const warning = `
        <div class="alert alert-warning alert-dismissible fade show" role="alert" style="position: fixed; top: 20px; right: 20px; z-index: 9999; max-width: 400px;">
            <strong>Popup Blocked!</strong> Please allow popups for this site to print labels.
            <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
        </div>
    `;
    $('body').append(warning);
    setTimeout(() => $('.alert').alert('close'), 5000);
}

// ============== EVENT HANDLER ==============

$(document).ready(function () {
    console.log("Print script loaded");

    // Initialize print button
    $("#C02_START_V2_Buttonprint").off("click.print").on("click.print", function (e) {
        e.preventDefault();
        const $btn = $(this);

        if ($btn.prop("disabled")) return;

        $btn.prop("disabled", true).html('<i class="fas fa-spinner fa-spin"></i> Printing...');

        C02_START_V2_Buttonprint_Click()
            .finally(() => {
                $btn.prop("disabled", false).html('<i class="fas fa-print"></i> Print');
                setTimeout(() => $btn.focus(), 100);
            });
    });
});