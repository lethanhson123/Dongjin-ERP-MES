let BaseResult;

// ==================== INIT ====================

$(document).ready(function () {
    $('.modal').modal();

    // Mở modal tìm kiếm LEAD
    $("#Button1").click(function () {
        $('#admin6USER1Modal').modal('open');
        $("#ModalTextBox1").focus();
    });

    $("#CloseModalButton").click(function () {
        $('#admin6USER1Modal').modal('close');
    });

    // Cập nhật datetime mỗi giây
    updateDateTime();
    setInterval(updateDateTime, 1000);

    // Focus vào barcode input
    $("#BC").focus();
});

// ==================== MODAL SEARCH LEAD ====================

$("#ModalButton1").click(function () {
    searchLeadData();
});

$("#ModalTextBox1").keydown(function (e) {
    if (e.keyCode === 13) {
        searchLeadData();
    }
});

function searchLeadData() {
    const leadPN = $("#ModalTextBox1").val().trim();

    $("#BackGround").css("display", "block");

    const baseParameter = {
        ListSearchString: [leadPN]
    };

    let formUpload = new FormData();
    formUpload.append("BaseParameter", JSON.stringify(baseParameter));

    fetch("/Admin6User/Button1_Click", {
        method: "POST",
        body: formUpload
    })
        .then(response => response.json())
        .then(data => {
            if (data.Success) {
                renderLeadData(data.DataGridView1);
            } else {
                alert(data.Error || "Unable to fetch LEAD data");
            }
            $("#BackGround").css("display", "none");
        })
        .catch(err => {
            alert("Connection error: " + err.message);
            $("#BackGround").css("display", "none");
        });
}

function renderLeadData(data) {
    let html = "";

    if (data && data.length > 0) {
        for (let i = 0; i < data.length; i++) {
            const row = data[i];
            html += `<tr data-index="${i}">
                <td class="center-align">${row.LEAD_PN || ""}</td>
                <td class="right-align">${row.BUNDLE_SIZE || ""}</td>
                <td class="center-align">${row.HOOK_RACK || ""}</td>
            </tr>`;
        }
    } else {
        html = "<tr><td colspan='3' class='center-align'>No data found</td></tr>";
    }

    $("#ModalDataGridView1").html(html);

    // Click để select
    $("#ModalDataGridView1 tr").click(function () {
        $("#ModalDataGridView1 tr").removeClass("selected");
        $(this).addClass("selected");

        const leadPN = $(this).find("td:eq(0)").text();
        const bundleSize = $(this).find("td:eq(1)").text();
        const hookRack = $(this).find("td:eq(2)").text();

        $("#Label2").text(leadPN);
        $("#Label3").text(bundleSize);
        $("#Label4").text(hookRack);
    });

    // Double click để chọn và đóng modal
    $("#ModalDataGridView1 tr").dblclick(function () {
        const leadPN = $(this).find("td:eq(0)").text();
        $("#TextBox1").val(leadPN);
        $('#admin6USER1Modal').modal('close');
        STK_BACODE();
        STK_BACODE_DATA();
    });
}

// Nút Select trong modal
$("#SelectButton").click(function () {
    if ($("#Label2").text() !== "-") {
        $("#TextBox1").val($("#Label2").text());
        $('#admin6USER1Modal').modal('close');
        STK_BACODE();
        STK_BACODE_DATA();
    }
});

// ==================== STOCK FUNCTIONS ====================

function STK_BACODE() {
    const leadNo = $("#TextBox1").val().trim();

    if (!leadNo) {
        $("#DataGridView1").empty();
        $("#Button3").text("STOCK_QTY = 0");
        return;
    }

    $("#BackGround").css("display", "block");

    const baseParameter = {
        ListSearchString: [leadNo]
    };

    let formUpload = new FormData();
    formUpload.append("BaseParameter", JSON.stringify(baseParameter));

    fetch("/Admin6User/STK_BACODE", {
        method: "POST",
        body: formUpload
    })
        .then(response => response.json())
        .then(data => {
            if (data.Success) {
                // Cập nhật STOCK_QTY button
                if (data.DataGridView1 && data.DataGridView1.length > 0) {
                    const stockQty = data.DataGridView1[0].STOCK_QTY || 0;
                    $("#Button3").text("STOCK_QTY = " + formatNumber(stockQty));
                } else {
                    $("#Button3").text("STOCK_QTY = 0");
                }

                renderStockGrid(data.DataGridView1);
            } else {
                alert(data.Error || "Unable to fetch stock information");
            }
            $("#BackGround").css("display", "none");
        })
        .catch(err => {
            alert("Connection error: " + err.message);
            $("#BackGround").css("display", "none");
        });
}

function STK_BACODE_DATA() {
    const leadNo = $("#TextBox1").val().trim();

    if (!leadNo) {
        $("#DataGridView2").empty();
        return;
    }

    $("#BackGround").css("display", "block");

    const baseParameter = {
        ListSearchString: [leadNo]
    };

    let formUpload = new FormData();
    formUpload.append("BaseParameter", JSON.stringify(baseParameter));

    fetch("/Admin6User/STK_BACODE_DATA", {
        method: "POST",
        body: formUpload
    })
        .then(response => response.json())
        .then(data => {
            if (data.Success) {
                renderTransactionGrid(data.DataGridView2);
            } else {
                alert(data.Error || "Unable to fetch transaction data");
            }
            $("#BackGround").css("display", "none");
        })
        .catch(err => {
            alert("Connection error: " + err.message);
            $("#BackGround").css("display", "none");
        });
}

function renderStockGrid(data) {
    let html = "";

    if (data && data.length > 0) {
        for (let i = 0; i < data.length; i++) {
            const row = data[i];
            const rowClass = row.Stock_status === "Bad" ? "class='red-text'" : "";

            html += `<tr ${rowClass}>
                <td>${row.HOOK_RACK || ""}</td>
                <td>${row.LEAD_NM || ""}</td>
                <td class="right-align">${formatNumber(row.STOCK_QTY)}</td>
                <td class="right-align">${formatNumber(row.IN_QTY)}</td>
                <td class="right-align">${formatNumber(row.OUT_QTY)}</td>
                <td class="center-align">${row.Stock_status || ""}</td>
                <td class="right-align">${formatNumber(row.CHK_QTY)}</td>
            </tr>`;
        }
    } else {
        html = "<tr><td colspan='7' class='center-align'>No data found</td></tr>";
    }

    $("#DataGridView1").html(html);
}

function renderTransactionGrid(data) {
    let html = "";

    if (data && data.length > 0) {
        for (let i = 0; i < data.length; i++) {
            const row = data[i];

            html += `<tr>
                <td class="center-align">${row.TABLE_NM || ""}</td>
                <td class="center-align">${row.RACKCODE || ""}</td>
                <td class="right-align">${formatNumber(row.QTY)}</td>
                <td class="center-align">${row.RACKDTM || ""}</td>
                <td class="center-align">${row.RACKOUT_DTM || ""}</td>
                <td>${row.BARCODE_NM || ""}</td>
            </tr>`;
        }
    } else {
        html = "<tr><td colspan='6' class='center-align'>No transaction history found</td></tr>";
    }

    $("#DataGridView2").html(html);
}

// Auto reload khi TextBox1 thay đổi
$("#TextBox1").on("change", function () {
    STK_BACODE();
    STK_BACODE_DATA();
});

$("#TextBox1").keydown(function (e) {
    if (e.keyCode === 13) {
        STK_BACODE();
        STK_BACODE_DATA();
    }
});

// Button STOCK_Search
$("#Button4").click(function () {
    STK_BACODE();
    STK_BACODE_DATA();
});

// ==================== BARCODE PROCESSING ====================

$("#BC").keydown(function (e) {
    if (e.keyCode === 13) {
        BC_KeyDown();
    }
});

function BC_KeyDown() {
    const BC = $("#BC").val().trim();

    if (!BC) {
        return;
    }

    try {
        // 1. Đếm số dấu $
        const dollarCount = (BC.match(/\$/g) || []).length;
        if (dollarCount < 8) {
            alert("BARCODE SCAN 을 잘못했습니다.\nScan sai Barcode.");
            $("#BC").val("").focus();
            return;
        }

        // 2. Parse LEAD_NO (phần trước dấu $$ đầu tiên)
        const leadNoIndex = BC.indexOf("$$");
        if (leadNoIndex === -1) {
            alert("BARCODE SCAN 을 잘못했습니다.\nScan sai Barcode.");
            $("#BC").val("").focus();
            return;
        }

        const LEAD_NO = BC.substring(0, leadNoIndex);

        // 3. Parse QTY (phần giữa $$ thứ 1 và $$ thứ 2)
        const afterFirst = BC.substring(leadNoIndex + 2);
        const secondIndex = afterFirst.indexOf("$$");
        if (secondIndex === -1) {
            alert("BARCODE SCAN 을 잘못했습니다.\nScan sai Barcode.");
            $("#BC").val("").focus();
            return;
        }

        const QTY = afterFirst.substring(0, secondIndex);

        // 4. Set TextBox1
        $("#TextBox1").val(LEAD_NO);

        // 5. Gọi API xử lý barcode
        $("#BackGround").css("display", "block");

        const baseParameter = {
            ListSearchString: [BC, LEAD_NO, QTY]
        };

        let formUpload = new FormData();
        formUpload.append("BaseParameter", JSON.stringify(baseParameter));

        fetch("/Admin6User/BC_KeyDown", {
            method: "POST",
            body: formUpload
        })
            .then(response => response.json())
            .then(data => {
                if (data.Success) {
                    // Reload data
                    STK_BACODE();
                    STK_BACODE_DATA();
                } else {
                    alert(data.Error || "Error processing barcode");
                }
                $("#BackGround").css("display", "none");
                $("#BC").val("").focus();
            })
            .catch(err => {
                alert("Connection error: " + err.message);
                $("#BackGround").css("display", "none");
                $("#BC").val("").focus();
            });

    } catch (error) {
        alert("Error: " + error.message);
        $("#BC").val("").focus();
    }
}

// ==================== STOCK ADJUSTMENT ====================

$("#Button3").click(function () {
    Button3_Click();
});

function Button3_Click() {
    const leadNo = $("#TextBox1").val().trim();
    const dateTime = $("#DateTimePicker1").val().trim();
    const qty = $("#TextBox2").val().trim();

    if (!leadNo) {
        alert("Please Check Again.");
        return;
    }

    if (dateTime.length < 18) {
        alert("Please Check Again.");
        return;
    }

    if (!qty) {
        alert("Please Check Again.");
        return;
    }

    if (!confirm("Select LEAD NO : " + leadNo + " Stock = 0")) {
        return;
    }

    $("#BackGround").css("display", "block");

    const baseParameter = {
        ListSearchString: [leadNo, dateTime, qty]
    };

    let formUpload = new FormData();
    formUpload.append("BaseParameter", JSON.stringify(baseParameter));

    fetch("/Admin6User/Button3_Click", {
        method: "POST",
        body: formUpload
    })
        .then(response => response.json())
        .then(data => {
            if (data.Success) {
                alert("Saving is complete.");
                STK_BACODE();
                STK_BACODE_DATA();
            } else {
                alert(data.Error || "Unknown error Please Check Again.");
            }
            $("#BackGround").css("display", "none");
            $("#BC").val("").focus();
        })
        .catch(err => {
            alert("Connection error: " + err.message);
            $("#BackGround").css("display", "none");
            $("#BC").val("").focus();
        });
}

// ==================== RESET BUTTON ====================

$("#Button2").click(function () {
    $("#DataGridView1").empty();
    $("#DataGridView2").empty();
    $("#TextBox1").val("");
    $("#TextBox2").val("0");
    $("#DateTimePicker1").val(getCurrentDateTime());
    $("#Button3").text("STOCK_QTY = 0");
    $("#BC").val("").focus();
});

// ==================== UTILITY FUNCTIONS ====================

function getCurrentDateTime() {
    const now = new Date();
    return now.getFullYear() + '-' +
        padZero(now.getMonth() + 1) + '-' +
        padZero(now.getDate()) + ' ' +
        padZero(now.getHours()) + ':' +
        padZero(now.getMinutes()) + ':' +
        padZero(now.getSeconds());
}

function padZero(num) {
    return num < 10 ? '0' + num : num;
}

function updateDateTime() {
    $("#DateTimePicker1").val(getCurrentDateTime());
}

function formatNumber(num) {
    if (num === null || num === undefined || num === "") return "0";
    return parseFloat(num).toLocaleString('en-US', { minimumFractionDigits: 0, maximumFractionDigits: 0 });
}

function OpenWindowByURL(url, width, height) {
    window.open(url, "_blank", "width=" + width + ",height=" + height + ",toolbar=no,menubar=no,scrollbars=yes,resizable=yes,location=no,status=no");
}