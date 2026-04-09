let BaseResult;
let IsTableSort = false;
let CurrentActiveTab = "TabPage1";
let toolResetStatus = [];

$(document).ready(function () {
    $('.modal').modal({
        onOpenEnd: function (modal) {
            // Kiểm tra ID của modal đang mở và focus vào textbox tương ứng
            if ($(modal).attr('id') === 'ScanINModal') {
                $('#txtScanINQR').focus();
            } else if ($(modal).attr('id') === 'ScanOUTModal') {
                $('#txtScanOUTQR').focus();
            }
        }
    });
    // Initialize tabs
    $('.tabs').tabs();

    // Tab click handlers
    $('a[href="#TabPage1"]').on('click', function () {
        CurrentActiveTab = "TabPage1";
    });

    $('a[href="#TabPage2"]').on('click', function () {
        CurrentActiveTab = "TabPage2";
    });
    $('a[href="#TabPage3"]').on('click', function () {
        loadScanInHistory();
    });

    $('a[href="#TabPage4"]').on('click', function () {
        loadScanOutHistory();
    });

    // Đăng ký sự kiện tìm kiếm lịch sử
    $("#DT_SCAN_FROM, #DT_SCAN_TO, #TXT_SCAN_SEARCH").on('change', function () {
        loadScanInHistory();
    });

    $("#DT_SCAN_OUT_FROM, #DT_SCAN_OUT_TO, #TXT_SCAN_OUT_SEARCH").on('change', function () {
        loadScanOutHistory();
    });

    Buttonfind_Click();
});
$("#txtScanINQR").keypress(function (e) {
    if (e.which === 13) {
        processScanIn();
    }
});

// Xử lý quét mã QR Scan Out
$("#txtScanOUTQR").keypress(function (e) {
    if (e.which === 13) {
        processScanOut();
    }
});
$("#ButtonScanIn").click(function () {
    $('#ScanINModal').modal('open');
});
$("#ButtonScanOut").click(function () {
    $('#ScanOUTModal').modal('open');
});

$('#DataGridView1Table').on('click', 'td:nth-child(2)', function () {
    let rowIndex = $(this).parent().index();
    let checkbox = $(`input[name="toolReset"][data-row-index="${rowIndex}"]`);
    let statusCell = $(`#status_${rowIndex}`);

    if (checkbox.prop('checked')) {
        checkbox.prop('checked', false);
        statusCell.text('');
    } else {
        checkbox.prop('checked', true);
        statusCell.text('Reset count');
    }
});
// Button event handlers
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

// Tab2 specific buttons
$("#Button13").click(function () {
    Button13_Click(); // ADD SAVE
});
$("#Button16").click(function () {
    Button16_Click(); // Cancel/Clear
});

// Main search function
function Buttonfind_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        ListSearchString: [],
    }

    if (CurrentActiveTab == "TabPage1") {
        // TabPage1 parameters
        let STB1 = $('#STB1').val() || "";        // APP
        let STB2 = $('#STB2').val() || "ALL";     // CUSTOMER  
        let STB3 = $('#STB3').val() || "";        // TYPE
        let ComboBox2 = $('#ComboBox2').val() || "ALL"; // SEQ

        BaseParameter.ListSearchString.push("TabPage1");
        BaseParameter.ListSearchString.push(STB1);
        BaseParameter.ListSearchString.push(STB2);
        BaseParameter.ListSearchString.push(STB3);
        BaseParameter.ListSearchString.push(ComboBox2);
    } else {
        // TabPage2 parameters
        let TextBox1 = $('#TextBox1').val() || "";     // APPLICATOR
        let ComboBox3 = $('#ComboBox3').val() || "ALL"; // SEQ

        BaseParameter.ListSearchString.push("TabPage2");
        BaseParameter.ListSearchString.push(TextBox1);
        BaseParameter.ListSearchString.push(ComboBox3);
    }

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/E01/Buttonfind_Click";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            BaseResult = data;
            if (BaseResult.Error) {
                alert("Error: " + BaseResult.Error);
                $("#BackGround").css("display", "none");
                return;
            }

            // Render appropriate tables based on active tab
            if (CurrentActiveTab == "TabPage1") {
                DataGridView1Render();
                ApplyToolUsageColorCoding(); // Color coding for usage ratio
            } else {
                DataGridView5Render();
                ClearToolInfo(); // Clear tool info when new search
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            console.error("Error parsing response:", err);
            $("#BackGround").css("display", "none");
        })
    }).catch((err) => {
        console.error("Network error:", err);
        $("#BackGround").css("display", "none");
    });
}

function Buttonadd_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/E01/Buttonadd_Click";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}

function Buttonsave_Click() {
    if (CurrentActiveTab != "TabPage1") {
        alert("Save function only available in Tool Status tab");
        return;
    }

    // Collect selected tools based on toolResetStatus array
    let selectedTools = [];
    for (let i = 0; i < toolResetStatus.length; i++) {
        if (toolResetStatus[i] === true) {  // Checked
            if (BaseResult && BaseResult.DataGridView1 && BaseResult.DataGridView1[i]) {
                selectedTools.push({
                    TOOLMASTER_IDX: BaseResult.DataGridView1[i].TOOLMASTER_IDX,
                    WK_CNT: BaseResult.DataGridView1[i].WK_CNT,
                    TOT_WK_CNT: BaseResult.DataGridView1[i].TOT_WK_CNT
                });
            }
        }
    }

    if (selectedTools.length == 0) {
        alert("Please select tools to reset");
        return;
    }

    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        ListSearchString: [getCurrentUser()]

    }
    BaseParameter.ListSearchString.push(JSON.stringify(selectedTools));

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/E01/Buttonsave_Click";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            if (data.Error) {
                alert("Save Error: " + data.Error);
            } else {
                alert("Save Completed.");
                toolResetStatus = [];  // Reset status array
                Buttonfind_Click();    // Refresh data
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}

function Buttondelete_Click() {
    if (CurrentActiveTab != "TabPage1") {
        alert("Delete function only available in Tool Status tab");
        return;
    }

    let selectedRowIndex = getSelectedRowIndex();
    if (selectedRowIndex < 0) {
        alert("Please select a tool to delete");
        return;
    }

    let toolMasterIdx = BaseResult.DataGridView1[selectedRowIndex].TOOLMASTER_IDX;
    if (!toolMasterIdx) {
        alert("Invalid tool selection");
        return;
    }

    if (!confirm("Are you sure you want to delete this tool?")) {
        return;
    }

    $("#BackGround").css("display", "block");
    let BaseParameter = {
        ListSearchString: [toolMasterIdx]
    };

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/E01/Buttondelete_Click";

    fetch(url, {
        method: "POST",
        body: formUpload
    }).then((response) => {
        response.json().then((data) => {
            if (data.Error) {
                alert("Delete Error: " + data.Error);
            } else {
                alert(data.Message || "Delete completed successfully");
                Buttonfind_Click();
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        });
    }).catch((err) => {
        $("#BackGround").css("display", "none");
    });
}

function Buttoncancel_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/E01/Buttoncancel_Click";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}

function Buttoninport_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/E01/Buttoninport_Click";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}

function Buttonexport_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        ListSearchString: [],
    }

    // Same parameters as Buttonfind_Click for export
    if (CurrentActiveTab == "TabPage1") {
        let STB1 = $('#STB1').val() || "";
        let STB2 = $('#STB2').val() || "ALL";
        let STB3 = $('#STB3').val() || "";
        let ComboBox2 = $('#ComboBox2').val() || "ALL";

        BaseParameter.ListSearchString.push("TabPage1");
        BaseParameter.ListSearchString.push(STB1);
        BaseParameter.ListSearchString.push(STB2);
        BaseParameter.ListSearchString.push(STB3);
        BaseParameter.ListSearchString.push(ComboBox2);
    } else {
        let TextBox1 = $('#TextBox1').val() || "";
        let ComboBox3 = $('#ComboBox3').val() || "ALL";

        BaseParameter.ListSearchString.push("TabPage2");
        BaseParameter.ListSearchString.push(TextBox1);
        BaseParameter.ListSearchString.push(ComboBox3);
    }

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/E01/Buttonexport_Click";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            let BaseResultButtonexport = data;
            if (BaseResultButtonexport) {
                if (BaseResultButtonexport.Code) {
                    let url = BaseResultButtonexport.Code;
                    window.open(url, "_blank");
                } else if (BaseResultButtonexport.Error) {
                    alert("Export Error: " + BaseResultButtonexport.Error);
                }
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
document.getElementById("Buttonexport").addEventListener("click", function () {
    let tableID, fileName, sheetName;

    switch (CurrentActiveTab) {
        case "TabPage1":
            tableID = "DataGridView1Table";
            fileName = "ToolStatus.xlsx";
            sheetName = "Tool Status";
            break;
        case "TabPage2":
            tableID = "DataGridView5Table";
            fileName = "ToolDetail.xlsx";
            sheetName = "Tool Detail";
            break;
        case "TabPage3":
            tableID = "DataGridView3Table";
            fileName = "ScanInHistory.xlsx";
            sheetName = "ScanIn History";
            break;
        case "TabPage4":
            tableID = "DataGridView4Table";
            fileName = "ScanOutHistory.xlsx";
            sheetName = "ScanOut History";
            break;
        default:
            alert("Không xác định tab để xuất Excel.");
            return;
    }
    TableHTMLToExcel(tableID, fileName, sheetName);
});

function Buttonprint_Click() {
    if (CurrentActiveTab != "TabPage1") {
        alert("Chức năng in chỉ khả dụng trong tab Tool Status");
        return;
    }

    let selectedRowIndex = getSelectedRowIndex();
    if (selectedRowIndex < 0) {
        alert("Vui lòng chọn một công cụ để in mã QR");
        return;
    }

    let applicator = BaseResult.DataGridView1[selectedRowIndex].APPLICATOR;
    let seq = BaseResult.DataGridView1[selectedRowIndex].SEQ;

    if (!applicator || !seq) {
        alert("Công cụ được chọn thiếu dữ liệu applicator hoặc sequence");
        return;
    }

    $("#BackGround").css("display", "block");
    let BaseParameter = {
        ListSearchString: [applicator, seq, getCurrentUser()]

    };

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/E01/Buttonprint_Click";

    fetch(url, {
        method: "POST",
        body: formUpload
    }).then((response) => {
        response.json().then((data) => {
            if (data.Error) {
                alert("Lỗi in: " + data.Error);
            } else if (data.QRCodeData) {
                // In trực tiếp mã QR
                printQRCode(data.QRCodeData, data.QRCodeText);
            }
            $("#BackGround").css("display", "none");
        });
    }).catch((err) => {
        console.error("Lỗi mạng:", err);
        alert("Đã xảy ra lỗi mạng khi tạo mã QR");
        $("#BackGround").css("display", "none");
    });
}

// Helper function để tìm selected row
function getSelectedRowIndex() {
    let selectedIndex = -1;

    // Tìm row được highlight hoặc checked
    $('#DataGridView1Table tbody tr').each(function (index) {
        if ($(this).hasClass('selected') || $(this).find('input[name="toolReset"]:checked').length > 0) {
            selectedIndex = index;
            return false; // Break
        }
    });

    // Nếu không có row nào được chọn, lấy row đầu tiên có checkbox checked
    if (selectedIndex < 0) {
        $('input[name="toolReset"]:checked').each(function () {
            selectedIndex = $(this).data('row-index');
            return false; // Break after first
        });
    }

    return selectedIndex;
}


function printQRCode(qrCodeBase64, qrText) {
    let printWindow = window.open('', '', 'width=400,height=300');

    printWindow.document.write(`
        <!DOCTYPE html>
        <html>
        <head>
            <title>QR Print</title>
            <style>
                @page { margin: 0; }
                body { 
                    margin: 0; 
                    padding: 0; 
                    font-family: Arial; 
                    text-align: center;
                    display: flex;
                    flex-direction: column;
                    justify-content: center;
                    align-items: center;
                    height: 100vh;
                }
                img { 
                    width: 40px; 
                    height: 40px; 
                    display: block; 
                    margin: 0 auto; 
                }
                .text { 
                    font-size: 10pt; 
                    font-weight: bold;
                    margin-top: 5px;
                }
            </style>
        </head>
        <body>
            <img src="data:image/png;base64,${qrCodeBase64}">
            <div class="text">${qrText}</div>
        </body>
        </html>
    `);
    printWindow.document.close();
    printWindow.focus();
    printWindow.onload = function () {
        printWindow.print();
        printWindow.close();
    };
}



function Buttonhelp_Click() {
    let url = "/WMP_PLAY";
    OpenWindowByURL(url, 800, 460);
}

function Buttonclose_Click() {
    history.back();
}

// Tab2 specific functions
function Button13_Click() {
    // ADD SAVE - Manual count addition
    let toolIdx = $('#Label114').val();
    let seq = $('#ComboBox1').val();
    let count = $('#TextBox5').val();

    if (!toolIdx || toolIdx == "") {
        alert("Please select a tool first");
        return;
    }

    if (!count || count <= 0) {
        alert("Please enter a valid count");
        return;
    }

    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        ListSearchString: [getCurrentUser()]

    }
    BaseParameter.ListSearchString.push(toolIdx);
    BaseParameter.ListSearchString.push(seq);
    BaseParameter.ListSearchString.push(count);

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/E01/Button13_Click";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            if (data.Error) {
                alert("Save Error: " + data.Error);
            } else {
                alert("SAVE Completed.");
                // Clear inputs and refresh
                Button16_Click();
                Buttonfind_Click();
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}

function Button16_Click() {
    // Cancel/Clear inputs
    $('#TextBox5').val("");
    $('#ComboBox1').prop('selectedIndex', 0);
}

// =====================================================
// DATA RENDERING FUNCTIONS
// =====================================================

// DataGridView1 - TabPage1 Tool Status
function DataGridView1Render() {
    let HTML = "";
    if (BaseResult && BaseResult.DataGridView1) {
        if (BaseResult.DataGridView1.length > 0) {
            for (let i = 0; i < BaseResult.DataGridView1.length; i++) {
                let item = BaseResult.DataGridView1[i];
                HTML += `<tr onclick="selectDataGridView1Row(${i})" style="cursor: pointer;">`;
                HTML += `<td><button class="btn btn-small blue" onclick="toggleToolReset(${i}); event.stopPropagation();">Check</button></td>`;
                HTML += `<td onclick="event.stopPropagation();"><span id='status_${i}'></span></td>`;           
                let statusClass = "";
                if (item.STATUS === "Working") {
                    statusClass = "red white-text"; 
                } else if (item.STATUS === "Available") {
                    statusClass = "green white-text"; 
                }
                HTML += `<td class="${statusClass}">${item.STATUS || ""}</td>`;
                HTML += `<td>${item.APPLICATOR || ""}</td>`;
                HTML += `<td>${item.SEQ || ""}</td>`;
                HTML += `<td>${item.MAX_CNT || "0"}</td>`;
                HTML += `<td>${item.CD_SYS_NOTE || ""}</td>`;
                HTML += `<td>${item.TOT_WK_CNT || "0"}</td>`;
                HTML += `<td>${item.WK_CNT || "0"}</td>`;
                HTML += `<td>${item.SPP_NO || ""}</td>`;
                HTML += `<td>${item.TYPE || ""}</td>`;
                HTML += `<td>${item.LCount || "0"}</td>`;
                HTML += `<td id='rat_${i}'>${Math.round((item.RAT || 0) * 100)}%</td>`;
                HTML += `<td style='display: none;'>${item.TOOLMASTER_IDX || ""}</td>`;
                HTML += `<td style='display: none;'>${item.TOOL_IDX || ""}</td>`;
                HTML += "</tr>";
            }
        }
    }
    document.getElementById("DataGridView1").innerHTML = HTML;
}
function selectDataGridView1Row(rowIndex) {
    // Remove previous selection
    $('#DataGridView1Table tbody tr').removeClass('selected');

    // Add selection to clicked row
    $('#DataGridView1Table tbody tr:eq(' + rowIndex + ')').addClass('selected');
}
// Global array để track checked status


function toggleToolReset(rowIndex) {
    // Toggle status
    toolResetStatus[rowIndex] = !toolResetStatus[rowIndex];

    let statusCell = document.getElementById(`status_${rowIndex}`);
    let button = event.target;

    if (toolResetStatus[rowIndex]) {
        statusCell.textContent = "Reset count";
        statusCell.style.color = "red";
        button.textContent = "Checked";
        button.className = "btn btn-small red";
    } else {
        statusCell.textContent = "";
        statusCell.style.color = "";
        button.textContent = "Check";
        button.className = "btn btn-small blue";
    }
}

// DataGridView5 - TabPage2 Tool Detail
function DataGridView5Render() {
    let HTML = "";
    if (BaseResult && BaseResult.DataGridView5) {
        if (BaseResult.DataGridView5.length > 0) {
            for (let i = 0; i < BaseResult.DataGridView5.length; i++) {
                let item = BaseResult.DataGridView5[i];
                HTML += "<tr onclick='selectToolDetailRow(" + i + ")'>";
                HTML += "<td>" + (item.APPLICATOR || "") + "</td>";
                HTML += "<td>" + (item.SEQ || "") + "</td>";
                HTML += "<td>" + (item.MAX_CNT || "0") + "</td>";
                HTML += "<td>" + (item.TOT_WK_CNT || "0") + "</td>";
                HTML += "<td>" + (item.WK_CNT || "0") + "</td>";
                HTML += "<td>" + (item.SPP_NO || "") + "</td>";
                HTML += "<td>" + (item.TYPE || "") + "</td>";
                HTML += "<td>" + (item.GAUGE || "") + "</td>";
                HTML += "<td>" + (item.COPL_NOR || "") + "</td>";
                HTML += "<td>" + (item.COPL_SPE || "") + "</td>";
                HTML += "<td>" + (item.INSPL_SEALTYPE || "") + "</td>";
                HTML += "<td>" + (item.INSPL_NONSEAL || "") + "</td>";
                HTML += "<td>" + (item.INSPL_XTYPE || "") + "</td>";
                HTML += "<td>" + (item.INSPL_KTYPE || "") + "</td>";
                HTML += "<td>" + (item.INSPL_SPE || "") + "</td>";
                HTML += "<td>" + (item.ANVIL_NOR || "") + "</td>";
                HTML += "<td>" + (item.ANVIL_SPE || "") + "</td>";
                HTML += "<td>" + (item.CMU_NOR || "") + "</td>";
                HTML += "<td>" + (item.CMU_SPE || "") + "</td>";
                HTML += "<td>" + (item.IMU_NOR || "") + "</td>";
                HTML += "<td>" + (item.IMU_NONSEAL || "") + "</td>";
                HTML += "<td>" + (item.IMU_SPE || "") + "</td>";
                HTML += "<td>" + (item.CUTPL_ONE || "") + "</td>";
                HTML += "<td>" + (item.CUTPL_DET || "") + "</td>";
                HTML += "<td>" + (item.CUTAN_ONE || "") + "</td>";
                HTML += "<td>" + (item.CUTAN_DET || "") + "</td>";
                HTML += "<td>" + (item.CUTHO_ONE || "") + "</td>";
                HTML += "<td>" + (item.CUTHO_DET || "") + "</td>";
                HTML += "<td>" + (item.RRBLK_ONE || "") + "</td>";
                HTML += "<td>" + (item.RRBLK_DET || "") + "</td>";
                HTML += "<td>" + (item.RRCUTHO_ONE || "") + "</td>";
                HTML += "<td>" + (item.RRCUTHO_DET || "") + "</td>";
                HTML += "<td>" + (item.FRCUTHO_ONE || "") + "</td>";
                HTML += "<td>" + (item.FRCUTHO_DET || "") + "</td>";
                HTML += "<td>" + (item.RRCUTAN_ONE || "") + "</td>";
                HTML += "<td>" + (item.RRCUTAN_DET || "") + "</td>";
                HTML += "<td>" + (item.WRDN_ONE || "") + "</td>";
                HTML += "<td>" + (item.WRDN_DET || "") + "</td>";
                HTML += "<td>" + (item.DESC || "") + "</td>";
                HTML += "<td>" + (item.COMB_CODE || "") + "</td>";
                HTML += "<td>" + (item.TOOL_IDX || "") + "</td>";
                HTML += "<td>" + (item.TOOLMASTER_IDX || "") + "</td>";
                HTML += "</tr>";
            }
        }
    }
    document.getElementById("DataGridView5").innerHTML = HTML;
}

// DataGridView6 - Tool History
function DataGridView6Render(toolIdx) {
    let HTML = "";
    if (BaseResult && BaseResult.DataGridView6) {
        if (BaseResult.DataGridView6.length > 0) {
            for (let i = 0; i < BaseResult.DataGridView6.length; i++) {
                let item = BaseResult.DataGridView6[i];
                HTML += "<tr>";
                HTML += "<td>" + (item.APPLICATOR || "") + "</td>";
                HTML += "<td>" + (item.SEQ || "") + "</td>";
                HTML += "<td>" + formatDate(item.WORK_DTM) + "</td>";
                HTML += "<td>" + (item.WK_QTY || "0") + "</td>";
                HTML += "<td>" + (item.TOT_QTY || "0") + "</td>";
                HTML += "<td>" + (item.CONTENT || "") + "</td>";
                HTML += "<td>" + formatDate(item.CREATE_DTM) + "</td>";
                HTML += "<td>" + (item.CREATE_USER || "") + "</td>";
                HTML += "</tr>";
            }
        }
    }
    document.getElementById("DataGridView6").innerHTML = HTML;
}

// =====================================================
// TOOL SPECIFIC FUNCTIONS
// =====================================================

// Apply color coding for tool usage ratio (like VB.NET)
function ApplyToolUsageColorCoding() {
    if (BaseResult && BaseResult.DataGridView1) {
        for (let i = 0; i < BaseResult.DataGridView1.length; i++) {
            let rat = parseFloat(BaseResult.DataGridView1[i].RAT || 0);
            let ratCell = document.getElementById('rat_' + i);

            if (ratCell) {
                if (rat > 0.9) {
                    ratCell.style.backgroundColor = 'red';
                    ratCell.style.color = 'white';
                } else if (rat > 0.8) {
                    ratCell.style.backgroundColor = 'orange';
                    ratCell.style.color = 'white';
                } else if (rat > 0.7) {
                    ratCell.style.backgroundColor = 'yellow';
                    ratCell.style.color = 'black';
                }
            }
        }
    }
}

// Handle tool detail row selection (like VB.NET DataGridView5_SelectionChanged)
function selectToolDetailRow(rowIndex) {
    if (BaseResult && BaseResult.DataGridView5 && BaseResult.DataGridView5[rowIndex]) {
        let item = BaseResult.DataGridView5[rowIndex];

        // Update tool info display
        $('#DGV1_D1').val(item.APPLICATOR || "");
        $('#DGV1_D2').val(item.SEQ || "");
        $('#DGV1_D3').val(item.MAX_CNT || "");
        $('#DGV1_D4').val(item.WK_CNT || "");

        // ✅ SỬA: Hiển thị APPLICATOR ở APP_NM và TOOL_IDX ở APP
        $('#Label115_APP_NM').text(item.APPLICATOR || "");   // NEW: APPLICATOR vào APP_NM
        $('#Label115_APP').text(item.TOOL_IDX || "");        // NEW: TOOL_IDX vào APP

        $('#Label114').val(item.TOOL_IDX || "");

        // Load tool history for selected tool
        loadToolHistory(item.TOOLMASTER_IDX);

        // Highlight selected row
        $('#DataGridView5Table tbody tr').removeClass('selected');
        $('#DataGridView5Table tbody tr:eq(' + rowIndex + ')').addClass('selected');
    }
}

// Load tool history for selected tool
function loadToolHistory(toolMasterIdx) {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        ListSearchString: [],
    }
    BaseParameter.ListSearchString.push(toolMasterIdx);

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/E01/LoadToolHistory";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            if (data.DataGridView6) {
                BaseResult.DataGridView6 = data.DataGridView6;
                DataGridView6Render();
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}

function ClearToolInfo() {
    $('#DGV1_D1').val("");
    $('#DGV1_D2').val("");
    $('#DGV1_D3').val("");
    $('#DGV1_D4').val("");

    // ✅ SỬA: Reset về giá trị mặc định
    $('#Label115_APP_NM').text("APP_NM");
    $('#Label115_APP').text("APP");

    $('#Label114').val("");
    document.getElementById("DataGridView6").innerHTML = "";
}

// =====================================================
// SORTING FUNCTIONS
// =====================================================

function DataGridView1Sort() {
    IsTableSort = !IsTableSort;
    if (BaseResult && BaseResult.DataGridView1) {
        DataGridViewSort(BaseResult.DataGridView1, IsTableSort);
        DataGridView1Render();
        ApplyToolUsageColorCoding();
    }
}

function DataGridView5Sort() {
    IsTableSort = !IsTableSort;
    if (BaseResult && BaseResult.DataGridView5) {
        DataGridViewSort(BaseResult.DataGridView5, IsTableSort);
        DataGridView5Render();
    }
}

// =====================================================
// UTILITY FUNCTIONS
// =====================================================

function formatNumber(value, decimals = 2) {
    if (!value || isNaN(value)) return "0";
    return parseFloat(value).toFixed(decimals);
}

function formatDate(dateString) {
    if (!dateString) return "";
    try {
        let date;
        if (typeof dateString === 'string') {
            if (dateString.includes('T')) {
                date = new Date(dateString);
            } else {
                date = new Date(dateString);
            }
        } else {
            date = new Date(dateString);
        }

        const year = date.getFullYear();
        const month = String(date.getMonth() + 1).padStart(2, '0');
        const day = String(date.getDate()).padStart(2, '0');

        return `${year}-${month}-${day}`;
    } catch (e) {
        console.error("Date formatting error:", e, "Input:", dateString);
        if (typeof dateString === 'string' && dateString.length >= 10) {
            return dateString.substring(0, 10);
        }
        return dateString || "";
    }
}
function processScanIn() {
    let qrCode = $("#txtScanINQR").val();
    if (!qrCode) {
        M.toast({ html: 'Vui lòng nhập mã QR', classes: 'red' });
        return;
    }

    $("#BackGround").css("display", "block");
    let BaseParameter = {
        ListSearchString: [qrCode, getCurrentUser()]
    };

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/E01/ScanIn";

    fetch(url, {
        method: "POST",
        body: formUpload
    }).then((response) => {
        response.json().then((data) => {
            $("#BackGround").css("display", "none");
            handleScanResponse(data);
            $("#txtScanINQR").val("");
            $("#txtScanINQR").focus();
            if (data.ErrorNumber === 1) {
                loadScanInHistory();
            }
        }).catch((err) => {
            $("#BackGround").css("display", "none");
            alert("Lỗi xử lý dữ liệu: " + err);
            $("#txtScanINQR").val("");
            $("#txtScanINQR").focus();
        });
    }).catch((err) => {
        $("#BackGround").css("display", "none");
        alert("Lỗi kết nối: " + err);
        $("#txtScanINQR").val("");
        $("#txtScanINQR").focus();
    });
}
function clearScanOutFields() {
    $("#txtScanOUTQR").val("");
    $("#txt_DeviceQRScan").val("");
    $("#select_Device_Group").val("");
    $("#select_Device_Code").val("");
}
function processScanOut() {
    let qrCode = $("#txtScanOUTQR").val().trim();
    let deviceQR = $("#txt_DeviceQRScan").val().trim();
    let deviceGroup = $("#select_Device_Group").val().trim();
    let deviceCode = $("#select_Device_Code").val().trim();

    if (!qrCode) {
        M.toast({ html: 'Vui lòng quét mã Applicator', classes: 'red' });
        $("#txtScanOUTQR").focus();
        return;
    }

    if (!deviceQR || !deviceGroup || !deviceCode) {
        M.toast({ html: 'Vui lòng quét mã thiết bị', classes: 'red' });
        $("#txt_DeviceQRScan").val("");
        $("#select_Device_Group").val("");
        $("#select_Device_Code").val("");
        $("#txt_DeviceQRScan").focus();
        return;
    }

    $("#BackGround").css("display", "block");
    let BaseParameter = {
        ListSearchString: [qrCode, deviceQR, deviceGroup, deviceCode, getCurrentUser()]
    };

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/E01/ScanOut";

    fetch(url, {
        method: "POST",
        body: formUpload
    }).then((response) => {
        response.json().then((data) => {
            $("#BackGround").css("display", "none");
            handleScanResponse(data);

            if (data.ErrorNumber === 1) {
                // Chỉ khi thành công mới xóa hết
                $("#txtScanOUTQR").val("");
                $("#txt_DeviceQRScan").val("");
                $("#select_Device_Group").val("");
                $("#select_Device_Code").val("");
                $("#txtScanOUTQR").focus();
                loadScanOutHistory();
            } else {
                $("#txtScanOUTQR").val("");
                // Khi lỗi chỉ xóa phần thiết bị
                $("#txt_DeviceQRScan").val("");
                $("#select_Device_Group").val("");
                $("#select_Device_Code").val("");
                $("#txtScanOUTQR").focus();
            }
        }).catch((err) => {
            $("#BackGround").css("display", "none");
            alert("Lỗi xử lý dữ liệu: " + err);
            // Khi lỗi chỉ xóa phần thiết bị
            $("#txtScanOUTQR").val("");
            $("#txt_DeviceQRScan").val("");
            $("#select_Device_Group").val("");
            $("#select_Device_Code").val("");
            $("#txtScanOUTQR").focus();
        });
    }).catch((err) => {
        $("#BackGround").css("display", "none");
        alert("Lỗi kết nối: " + err);
        // Khi lỗi chỉ xóa phần thiết bị
        $("#txtScanOUTQR").val("");
        $("#txt_DeviceQRScan").val("");
        $("#select_Device_Group").val("");
        $("#select_Device_Code").val("");
        $("#txtScanOUTQR").focus();
    });
}
function handleScanResponse(data) {
    if (data.ErrorNumber === 1) {
        // Thành công
        M.toast({ html: data.Message || 'Thao tác thành công', classes: 'green' });
    } else if (data.ErrorNumber === 0) {
        // Đã ở trạng thái mong muốn
        M.toast({ html: data.Message || 'Đã ở trạng thái mong muốn', classes: 'orange' });
    } else if (data.ErrorNumber === -1) {
        // Lỗi đầu vào
        M.toast({ html: data.Message || 'Vui lòng quét mã QR', classes: 'red' });
    } else if (data.ErrorNumber === -2) {
        // Mã không tồn tại
        M.toast({ html: data.Message || 'Mã QR không tồn tại', classes: 'red' });
    } else {
        // Lỗi khác
        M.toast({ html: data.Message || data.Error || 'Lỗi xử lý', classes: 'red' });
    }
}
function loadScanInHistory() {
    $("#BackGround").css("display", "block");
    let fromDate = $("#DT_SCAN_FROM").val();
    let toDate = $("#DT_SCAN_TO").val();
    let searchText = $("#TXT_SCAN_SEARCH").val();

    let BaseParameter = {
        ListSearchString: [fromDate, toDate, searchText, getCurrentUser()]

    };

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/E01/GetScanInHistory";

    fetch(url, {
        method: "POST",
        body: formUpload
    }).then((response) => {
        response.json().then((data) => {
            renderScanInHistory(data);
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
            alert("Lỗi xử lý dữ liệu: " + err);
        });
    }).catch((err) => {
        $("#BackGround").css("display", "none");
        alert("Lỗi kết nối: " + err);
    });
}
function loadScanOutHistory() {
    $("#BackGround").css("display", "block");
    let fromDate = $("#DT_SCAN_OUT_FROM").val();
    let toDate = $("#DT_SCAN_OUT_TO").val();
    let searchText = $("#TXT_SCAN_OUT_SEARCH").val();

    let BaseParameter = {
        ListSearchString: [fromDate, toDate, searchText, getCurrentUser()]

    };

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/E01/GetScanOutHistory";

    fetch(url, {
        method: "POST",
        body: formUpload
    }).then((response) => {
        response.json().then((data) => {
            renderScanOutHistory(data);
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
            alert("Lỗi xử lý dữ liệu: " + err);
        });
    }).catch((err) => {
        $("#BackGround").css("display", "none");
        alert("Lỗi kết nối: " + err);
    });
}
function renderScanInHistory(data) {
    let HTML = "";
    if (data && data.DataGridView3) {
        if (data.DataGridView3.length > 0) {
            for (let i = 0; i < data.DataGridView3.length; i++) {
                let item = data.DataGridView3[i];
                HTML += "<tr>";
                HTML += "<td>" + (item.CODE || "") + "</td>";
                HTML += "<td>" + (item.APPLICATOR || "") + "</td>";
                HTML += "<td>" + (item.SEQ || "") + "</td>";
                HTML += "<td>" + (item.APPLICATOR_LOT || "") + "</td>";
                HTML += "<td>" + (item.CREATE_DTM) + "</td>";
                HTML += "<td>" + (item.CREATE_USER || "") + "</td>";
                HTML += "</tr>";
            }
        }
    }
    document.getElementById("DataGridView3").innerHTML = HTML;
}
// Render lịch sử Scan Out
function renderScanOutHistory(data) {
    let HTML = "";
    if (data && data.DataGridView4) {
        if (data.DataGridView4.length > 0) {
            for (let i = 0; i < data.DataGridView4.length; i++) {
                let item = data.DataGridView4[i];
                HTML += "<tr>";
                HTML += "<td>" + (item.CODE || "") + "</td>";
                HTML += "<td>" + (item.APPLICATOR || "") + "</td>";
                HTML += "<td>" + (item.SEQ || "") + "</td>";
                HTML += "<td>" + (item.APPLICATOR_LOT || "") + "</td>";
                HTML += "<td>" + (item.DeviceGroup || "") + "</td>";
                HTML += "<td>" + (item.DeviceCode || "") + "</td>";
                HTML += "<td>" + (item.CREATE_DTM) + "</td>";
                HTML += "<td>" + (item.CREATE_USER || "") + "</td>";
                HTML += "</tr>";
            }
        }
    }
    document.getElementById("DataGridView4").innerHTML = HTML;
}
$("#txt_DeviceQRScan").keypress(function (e) {
    if (e.which === 13) {
        let deviceQR = $(this).val().trim();

        if (deviceQR) {
            let parts = deviceQR.split('-');

            if (parts.length === 2) {
                let deviceGroup = parts[0];
                let deviceCode = parts[1];

                $("#select_Device_Group").val(deviceGroup);
                $("#select_Device_Code").val(deviceCode);

                processScanOut();
            } else {
                M.toast({ html: 'Định dạng mã thiết bị không hợp lệ (phải là Group-Code)', classes: 'red' });
                $("#txt_DeviceQRScan").val("");
                $("#select_Device_Group").val("");
                $("#select_Device_Code").val("");

                $(this).focus();
            }
        } else {
            M.toast({ html: 'Vui lòng quét mã thiết bị', classes: 'red' });
            $(this).focus();
        }
    }
});
function getCurrentUser() {
    // Lấy USER_ID từ cookie
    return GetCookieValue('UserID') || 'SYSTEM';
}




