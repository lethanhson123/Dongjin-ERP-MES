let BaseResult;
let IsTableSort = false;
let CurrentActiveTab = "TabPage1";

$(document).ready(function () {
    // Initialize tabs
    $('.tabs').tabs();

    // Set default dates to today
    var now = new Date();
    var day = ("0" + now.getDate()).slice(-2);
    var month = ("0" + (now.getMonth() + 1)).slice(-2);
    var today = now.getFullYear() + "-" + (month) + "-" + (day);

    $("#DateTimePicker1").val(today);
    $("#DateTimePicker2").val(today);
    $("#DateTimePicker3").val(today);
    $("#DateTimePicker4").val(today);

    // Tab click handlers
    $('a[href="#TabPage1"]').on('click', function () {
        CurrentActiveTab = "TabPage1";
    });

    $('a[href="#TabPage2"]').on('click', function () {
        CurrentActiveTab = "TabPage2";
    });
});

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

function Buttonfind_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        ListSearchString: [],
    }

    // Determine which tab is active and get appropriate date range
    let FromDate, ToDate;
    if (CurrentActiveTab == "TabPage1") {
        FromDate = $('#DateTimePicker3').val();
        ToDate = $('#DateTimePicker4').val();
    } else {
        FromDate = $('#DateTimePicker1').val();
        ToDate = $('#DateTimePicker2').val();
    }

    BaseParameter.ListSearchString.push(CurrentActiveTab); // Tab selection
    BaseParameter.ListSearchString.push(FromDate);          // From date
    BaseParameter.ListSearchString.push(ToDate);            // To date

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C12/Buttonfind_Click";

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
                DataGridView3Render();
            } else {
                DataGridView2Render();
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
    let url = "/C12/Buttonadd_Click";

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
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C12/Buttonsave_Click";

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

function Buttondelete_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C12/Buttondelete_Click";

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

function Buttoncancel_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C12/Buttoncancel_Click";

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
    let url = "/C12/Buttoninport_Click";

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

    // Get current tab and date range for export
    let FromDate, ToDate;
    if (CurrentActiveTab == "TabPage1") {
        FromDate = $('#DateTimePicker3').val();
        ToDate = $('#DateTimePicker4').val();
    } else {
        FromDate = $('#DateTimePicker1').val();
        ToDate = $('#DateTimePicker2').val();
    }

    BaseParameter.ListSearchString.push(CurrentActiveTab);
    BaseParameter.ListSearchString.push(FromDate);
    BaseParameter.ListSearchString.push(ToDate);

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C12/Buttonexport_Click";

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

function Buttonprint_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C12/Buttonprint_Click";

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

function Buttonhelp_Click() {
    let url = "/WMP_PLAY";
    OpenWindowByURL(url, 800, 460);
}

function Buttonclose_Click() {
    history.back();
}

// =====================================================
// DATA RENDERING FUNCTIONS
// =====================================================

// DataGridView1 - TabPage1 Detail Records
function DataGridView1Render() {
    let HTML = "";
    if (BaseResult && BaseResult.DataGridView1) {
        if (BaseResult.DataGridView1.length > 0) {
            for (let i = 0; i < BaseResult.DataGridView1.length; i++) {
                let item = BaseResult.DataGridView1[i];
                HTML += "<tr>";
                HTML += "<td>" + (item.TORDER_IDX || "") + "</td>";
                HTML += "<td>" + (item.LEAD_NO || "") + "</td>";
                HTML += "<td>" + (item.MC_NO || "") + "</td>";
                HTML += "<td>" + formatDate(item.DATE) + "</td>";
                HTML += "<td>" + formatNumber(item.NON_WORK_TIME, 1) + "</td>";
                HTML += "<td>" + formatNumber(item.WORKG_TIME, 1) + "</td>";
                HTML += "<td>" + formatNumber(item.WG_RUS, 2) + "</td>";
                HTML += "<td>" + formatNumber(item.WORK_TIME, 1) + "</td>";
                HTML += "<td>" + formatNumber(item.WG_RUS2, 2) + "</td>";
                HTML += "<td>" + formatNumber(item.SUM_QTY, 0) + "</td>";
                HTML += "<td>" + formatNumber(item.TOT_QTY, 0) + "</td>";
                HTML += "<td>" + (item.WIRE || "") + "</td>";
                HTML += "<td>" + formatNumber(item.WIRE_L, 0) + "</td>";
                HTML += "<td>" + (item.TERM1 || "") + "</td>";
                HTML += "<td>" + (item.TERM2 || "") + "</td>";
                HTML += "<td>" + (item.SEAL1 || "") + "</td>";
                HTML += "<td>" + (item.SEAL2 || "") + "</td>";
                HTML += "<td>" + formatNumber(item.RUS, 0) + "</td>";
               
                HTML += "</tr>";
            }
        }
    }
    document.getElementById("DataGridView1").innerHTML = HTML;
}

// DataGridView2 - TabPage2 Summary by MC
function DataGridView2Render() {
    let HTML = "";
    if (BaseResult && BaseResult.DataGridView2) {
        if (BaseResult.DataGridView2.length > 0) {
            for (let i = 0; i < BaseResult.DataGridView2.length; i++) {
                let item = BaseResult.DataGridView2[i];
                HTML += "<tr>";
                HTML += "<td>" + formatDate(item.DATE) + "</td>";
                HTML += "<td>" + (item.MC_NO || "") + "</td>";
                HTML += "<td>" + formatNumber(item.PO_QTY, 0) + "</td>";
                HTML += "<td>" + formatNumber(item.WK_QTY, 0) + "</td>";
                HTML += "<td>" + formatNumber(item.NON_WORK_TIME, 1) + "</td>";
                HTML += "<td>" + formatNumber(item.WORKG_TIME, 1) + "</td>";
                HTML += "<td>" + formatNumber(item.WG_RUS, 2) + "</td>";
                HTML += "<td>" + formatNumber(item.WORK_TIME, 1) + "</td>";
                HTML += "<td>" + formatNumber(item.WG_RUS2, 2) + "</td>";
                HTML += "</tr>";
            }
        }
    }
    document.getElementById("DataGridView2").innerHTML = HTML;
}

// DataGridView3 - TabPage1 Summary Report
function DataGridView3Render() {
    let HTML = "";
    if (BaseResult && BaseResult.DataGridView3) {
        if (BaseResult.DataGridView3.length > 0) {
            for (let i = 0; i < BaseResult.DataGridView3.length; i++) {
                let item = BaseResult.DataGridView3[i];
                HTML += "<tr>";
                HTML += "<td>" + formatNumber(item.NON_WORK_TIME, 1) + "</td>";
                HTML += "<td>" + formatNumber(item.WORKG_TIME, 1) + "</td>";
                HTML += "<td>" + formatNumber(item.WG_RUS, 2) + "</td>";
                HTML += "<td>" + formatNumber(item.WORK_TIME, 1) + "</td>";
                HTML += "<td>" + formatNumber(item.WG_RUS2, 2) + "</td>";
                HTML += "<td>" + formatNumber(item.PO_QTY, 0) + "</td>";
                HTML += "<td>" + formatNumber(item.ACT_QTY, 0) + "</td>";
                HTML += "</tr>";
            }
        }
    }
    document.getElementById("DataGridView3").innerHTML = HTML;
}

// =====================================================
// SORTING FUNCTIONS
// =====================================================

function DataGridView1Sort() {
    IsTableSort = !IsTableSort;
    if (BaseResult && BaseResult.DataGridView1) {
        DataGridViewSort(BaseResult.DataGridView1, IsTableSort);
        DataGridView1Render();
    }
}

function DataGridView2Sort() {
    IsTableSort = !IsTableSort;
    if (BaseResult && BaseResult.DataGridView2) {
        DataGridViewSort(BaseResult.DataGridView2, IsTableSort);
        DataGridView2Render();
    }
}

function DataGridView3Sort() {
    IsTableSort = !IsTableSort;
    if (BaseResult && BaseResult.DataGridView3) {
        DataGridViewSort(BaseResult.DataGridView3, IsTableSort);
        DataGridView3Render();
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
        // Handle various date formats
        let date;
        if (typeof dateString === 'string') {
            // Remove time part if exists (2025-05-19T00:00:00 -> 2025-05-19)
            if (dateString.includes('T')) {
                date = new Date(dateString);
            } else {
                date = new Date(dateString);
            }
        } else {
            date = new Date(dateString);
        }

        // Format as YYYY-MM-DD
        const year = date.getFullYear();
        const month = String(date.getMonth() + 1).padStart(2, '0');
        const day = String(date.getDate()).padStart(2, '0');

        return `${year}-${month}-${day}`;
    } catch (e) {
        console.error("Date formatting error:", e, "Input:", dateString);
        // Fallback: if it's already in YYYY-MM-DD format or similar, just return first 10 chars
        if (typeof dateString === 'string' && dateString.length >= 10) {
            return dateString.substring(0, 10);
        }
        return dateString || "";
    }
}