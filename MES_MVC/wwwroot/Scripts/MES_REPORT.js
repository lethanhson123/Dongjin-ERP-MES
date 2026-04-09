let BaseResult = {};
let currentTab = "T1_P1";

$(document).ready(function () {
    $("#Button1").addClass("mes-nav-active").css("background-color", "#000");
    $("#Button2").css("background-color", "#666");
    $("#TabPage1").show();
    $("#TabPage2").hide();
    $("#T1_P1").show();
    $("#T1_P2, #T1_P3, #T1_P4").hide();
    $("#T1_P1_Tab").addClass("active");

    // Auto load data for the first tab instead of calling Load()
    T1_LOAD();
});

$("#Button1").click(function () {
    $("#Button1").addClass("mes-nav-active").css("background-color", "#000");
    $("#Button2").removeClass("mes-nav-active").css("background-color", "#666");
    $("#TabPage1").show();
    $("#TabPage2").hide();
    $("#T1_P1_Tab").click();
});

$("#Button2").click(function () {
    $("#Button2").addClass("mes-nav-active").css("background-color", "#000");
    $("#Button1").removeClass("mes-nav-active").css("background-color", "#666");
    $("#TabPage2").show();
    $("#TabPage1").hide();
});

$("#T1_P1_Tab").click(function () {
    currentTab = "T1_P1";
    $("#T1_P1").show();
    $("#T1_P2, #T1_P3, #T1_P4").hide();
    $(this).addClass("active").siblings().removeClass("active");
    T1_LOAD();
});

$("#T1_P2_Tab").click(function () {
    currentTab = "T1_P2";
    $("#T1_P2").show();
    $("#T1_P1, #T1_P3, #T1_P4").hide();
    $(this).addClass("active").siblings().removeClass("active");
    T2_LOAD();
});

$("#T1_P3_Tab").click(function () {
    currentTab = "T1_P3";
    $("#T1_P3").show();
    $("#T1_P1, #T1_P2, #T1_P4").hide();
    $(this).addClass("active").siblings().removeClass("active");
    T3_LOAD();
});

$("#T1_P4_Tab").click(function () {
    currentTab = "T1_P4";
    $("#T1_P4").show();
    $("#T1_P1, #T1_P2, #T1_P3").hide();
    $(this).addClass("active").siblings().removeClass("active");
    T4_LOAD();
});

$("#Buttonclose").click(function () {
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify({}));
    fetch("/MES_REPORT/Buttonclose_Click", {
        method: "POST",
        body: formUpload
    }).then(response => response.json())
        .then(data => {
            if (data.Error) {
                showError("Please Check Again.\n" + data.Error);
            } else {
                history.back();
            }
        })
        .catch(err => {
            showError("Error Code: 99990, 인터넷 연결을 할 수 없습니다.\nKhông thể kết nối MES.\n(Sự cố mạng Internet)");
        });
});

// Keep the original Load function for backward compatibility
function Load() {
    $("#BackGround").css("display", "block");
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify({}));
    fetch("/MES_REPORT/Load", {
        method: "POST",
        body: formUpload
    }).then(response => response.json())
        .then(data => {
            if (data.Error) {
                showError("Please Check Again.\n" + data.Error);
            } else {
                BaseResult.DataGridView0 = data.DataGridView0;
                DataGridView0Render();
            }
            $("#BackGround").css("display", "none");
        })
        .catch(err => {
            $("#BackGround").css("display", "none");
            showError("Error Code: 99990, 인터넷 연결을 할 수 없습니다.\nKhông thể kết nối MES.\n(Sự cố mạng Internet)");
        });
}

function T1_LOAD() {
    $("#BackGround").css("display", "block");
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify({}));
    fetch("/MES_REPORT/T1_LOAD", {
        method: "POST",
        body: formUpload
    }).then(response => response.json())
        .then(data => {
            if (data.Error) {
                showError("Please Check Again.\n" + data.Error);
            } else {
                BaseResult.DataGridView0 = data.DataGridView0;
                DataGridView0Render();
            }
            $("#BackGround").css("display", "none");
        })
        .catch(err => {
            $("#BackGround").css("display", "none");
            showError("Error Code: 99990, 인터넷 연결을 làm lại.\nKhông thể kết nối MES.\n(Sự cố mạng Internet)");
        });
}

function T2_LOAD() {
    $("#BackGround").css("display", "block");
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify({}));
    fetch("/MES_REPORT/T2_LOAD", {
        method: "POST",
        body: formUpload
    }).then(response => response.json())
        .then(data => {
            if (data.Error) {
                showError("Please Check Again.\n" + data.Error);
            } else {
                BaseResult.DataGridView1 = data.DataGridView1;
                DataGridView1Render();
            }
            $("#BackGround").css("display", "none");
        })
        .catch(err => {
            $("#BackGround").css("display", "none");
            showError("Error Code: 99990, 인터넷 연결을 làm lại.\nKhông thể kết nối MES.\n(Sự cố mạng Internet)");
        });
}

function T3_LOAD() {
    $("#BackGround").css("display", "block");
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify({}));
    fetch("/MES_REPORT/T3_LOAD", {
        method: "POST",
        body: formUpload
    }).then(response => response.json())
        .then(data => {
            if (data.Error) {
                showError("Please Check Again.\n" + data.Error);
            } else {
                BaseResult.DataGridView3 = data.DataGridView3;
                DataGridView3Render();
            }
            $("#BackGround").css("display", "none");
        })
        .catch(err => {
            $("#BackGround").css("display", "none");
            showError("Error Code: 99990, 인터넷 연결을 làm lại.\nKhông thể kết nối MES.\n(Sự cố mạng Internet)");
        });
}

function T4_LOAD() {
    $("#BackGround").css("display", "block");
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify({}));
    fetch("/MES_REPORT/T4_LOAD", {
        method: "POST",
        body: formUpload
    }).then(response => response.json())
        .then(data => {
            if (data.Error) {
                showError("Please Check Again.\n" + data.Error);
            } else {
                BaseResult.DataGridView2 = data.DataGridView2;
                DataGridView2Render();
            }
            $("#BackGround").css("display", "none");
        })
        .catch(err => {
            $("#BackGround").css("display", "none");
            showError("Error Code: 99990, 인터넷 연결을 làm lại.\nKhông thể kết nối MES.\n(Sự cố mạng Internet)");
        });
}

function DataGridView0Render() {
    let HTML = "";
    if (BaseResult.DataGridView0 && BaseResult.DataGridView0.length > 0) {
        for (let i = 0; i < BaseResult.DataGridView0.length; i++) {
            let item = BaseResult.DataGridView0[i];
            let rowClass = item.FCTRY_NM === "ALL" ? "all-row" : "";
            HTML += `<tr class="${rowClass}">
                <td>${item.FCTRY_NM || ''}</td>
                <td>${item.WEEK || 0}</td>
                <td>${item.MIN_DT || ''}</td>
                <td>${item.MAX_DT || ''}</td>
                <td>${formatNumber(item.LEAD_COUNT)}</td>
                <td>${formatNumber(item.WORKING)}</td>
                <td>${formatNumber(item.NOT_WORK)}</td>
                <td class="highlight-yellow">${formatNumber(item.LEAD_RAT)}</td>
                <td>${formatNumber(item.PO_SUM)}</td>
                <td>${formatNumber(item.ACT_SUM)}</td>
                <td class="highlight-yellow">${formatNumber(item.ACT_RAT)}</td>
            </tr>`;
        }
    }
    $("#DataGridView0").html(HTML);
}

function DataGridView1Render() {
    let HTML = "";
    if (BaseResult.DataGridView1 && BaseResult.DataGridView1.length > 0) {
        for (let i = 0; i < BaseResult.DataGridView1.length; i++) {
            let item = BaseResult.DataGridView1[i];
            let rowClass = item.FCTRY_NM === "ALL KOMAX" ? "all-row" : "";
            HTML += `<tr class="${rowClass}">
                <td>${item.FCTRY_NM || ''}</td>
                <td>${item.WEEK || 0}</td>
                <td>${item.MIN_DT || ''}</td>
                <td>${item.MAX_DT || ''}</td>
                <td>${formatNumber(item.LEAD_COUNT)}</td>
                <td>${formatNumber(item.WORKING)}</td>
                <td>${formatNumber(item.NOT_WORK)}</td>
                <td class="highlight-yellow">${formatNumber(item.LEAD_RAT)}</td>
                <td>${formatNumber(item.PO_SUM)}</td>
                <td>${formatNumber(item.ACT_SUM)}</td>
                <td class="highlight-yellow">${formatNumber(item.ACT_RAT)}</td>
            </tr>`;
        }
    }
    $("#DataGridView1").html(HTML);
}

function DataGridView2Render() {
    let HTML = "";
    if (BaseResult.DataGridView2 && BaseResult.DataGridView2.length > 0) {
        for (let i = 0; i < BaseResult.DataGridView2.length; i++) {
            let item = BaseResult.DataGridView2[i];
            let rowClass = item.FCTRY_NM === "ALL SPST" ? "all-row" : "";
            HTML += `<tr class="${rowClass}">
                <td>${item.FCTRY_NM || ''}</td>
                <td>${item.WEEK || 0}</td>
                <td>${item.MIN_DT || ''}</td>
                <td>${item.MAX_DT || ''}</td>
                <td>${formatNumber(item.LEAD_COUNT)}</td>
                <td>${formatNumber(item.WORKING)}</td>
                <td>${formatNumber(item.NOT_WORK)}</td>
                <td class="highlight-yellow">${formatNumber(item.LEAD_RAT)}</td>
                <td>${formatNumber(item.PO_SUM)}</td>
                <td>${formatNumber(item.ACT_SUM)}</td>
                <td class="highlight-yellow">${formatNumber(item.ACT_RAT)}</td>
            </tr>`;
        }
    }
    $("#DataGridView2").html(HTML);
}

function DataGridView3Render() {
    let HTML = "";
    if (BaseResult.DataGridView3 && BaseResult.DataGridView3.length > 0) {
        for (let i = 0; i < BaseResult.DataGridView3.length; i++) {
            let item = BaseResult.DataGridView3[i];
            let rowClass = item.FCTRY_NM === "ALL LP" ? "all-row" : "";
            HTML += `<tr class="${rowClass}">
                <td>${item.FCTRY_NM || ''}</td>
                <td>${item.WEEK || 0}</td>
                <td>${item.MIN_DT || ''}</td>
                <td>${item.MAX_DT || ''}</td>
                <td>${formatNumber(item.LEAD_COUNT)}</td>
                <td>${formatNumber(item.WORKING)}</td>
                <td>${formatNumber(item.NOT_WORK)}</td>
                <td class="highlight-yellow">${formatNumber(item.LEAD_RAT)}</td>
                <td>${formatNumber(item.PO_SUM)}</td>
                <td>${formatNumber(item.ACT_SUM)}</td>
                <td class="highlight-yellow">${formatNumber(item.ACT_RAT)}</td>
            </tr>`;
        }
    }
    $("#DataGridView3").html(HTML);
}

function showError(message) {
    alert(message);
}

function formatNumber(number) {
    return (number || 0).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
}