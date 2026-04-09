let BaseResult;

$("#Buttonfind").click(function () { Buttonfind_Click(); });
$("#Buttonadd").click(function () { Buttonadd_Click(); });
$("#Buttonsave").click(function () { Buttonsave_Click(); });
$("#Buttondelete").click(function () { Buttondelete_Click(); });
$("#Buttoncancel").click(function () { Buttoncancel_Click(); });
$("#Buttoninport").click(function () { Buttoninport_Click(); });
$("#Buttonexport").click(function () { Buttonexport_Click(); });
$("#Buttonprint").click(function () { Buttonprint_Click(); });
$("#Buttonhelp").click(function () { Buttonhelp_Click(); });
$("#Buttonclose").click(function () { Buttonclose_Click(); });
$("#Button1").click(function () { Button1_Click(); });
$("#Button2").click(function () { Button2_Click(); });

function Buttonfind_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/H09/Buttonfind_Click";

    fetch(url, {
        method: "POST",
        body: formUpload
    }).then((response) => {
        response.json().then((data) => {
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}

function Buttonadd_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/H09/Buttonadd_Click";

    fetch(url, {
        method: "POST",
        body: formUpload
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
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/H09/Buttonsave_Click";

    fetch(url, {
        method: "POST",
        body: formUpload
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
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/H09/Buttondelete_Click";

    fetch(url, {
        method: "POST",
        body: formUpload
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
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/H09/Buttoncancel_Click";

    fetch(url, {
        method: "POST",
        body: formUpload
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
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/H09/Buttoninport_Click";

    fetch(url, {
        method: "POST",
        body: formUpload
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
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/H09/Buttonexport_Click";

    fetch(url, {
        method: "POST",
        body: formUpload
    }).then((response) => {
        response.json().then((data) => {
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}

function Buttonprint_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/H09/Buttonprint_Click";

    fetch(url, {
        method: "POST",
        body: formUpload
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

function Button1_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/H09/Button1_Click";

    fetch(url, {
        method: "POST",
        body: formUpload
    }).then((response) => {
        response.json().then((data) => {
            updateDashboard(data);
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}

function Button2_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/H09/Button2_Click";

    fetch(url, {
        method: "POST",
        body: formUpload
    }).then((response) => {
        return response.json().then((data) => {
            $("#BackGround").css("display", "none");
            if (data && data.DataGridView3 && data.DataGridView3.length > 0) {
                populateH00_LISTModal(data);
                $('#H00_LISTModal').modal('open'); // Mở modal sau khi render dữ liệu
            } else {
                alert("Không có dữ liệu để hiển thị.");
            }
        }).catch((err) => {
            $("#BackGround").css("display", "none");
            alert("Lỗi khi tải dữ liệu: " + err.message);
        });
    }).catch((err) => {
        $("#BackGround").css("display", "none");
        alert("Lỗi kết nối: " + err.message);
    });
}
function populateH00_LISTModal(data) {
   
    let html = '';
    for (let i = 0; i < data.DataGridView3.length; i++) {
        let row = data.DataGridView3[i];
        html += '<tr>';
        html += `<td>${row.DATE || ""}</td>`;
        html += `<td>${row.MC_NO || ""}</td>`;
        html += `<td>${row.TS_TIME_ST || ""}</td>`;
        html += `<td>${row.TS_TIME_END || ""}</td>`;
        html += `<td>${formatNumber(row.SUM || 0)}</td>`;
        html += `<td>${row.STOP_TIME || "0"}</td>`;
        html += `<td>${row.T_STOP || "0"}</td>`;
        html += `<td>${row.WORK_TIME || "0"}</td>`;
        html += `<td>${(parseFloat(row.RUN_RAT || 0) * 100).toFixed(0)}%</td>`;
        html += `<td>${formatNumber(row.UPH || 0)}</td>`;
        html += '</tr>';
    }

    // Cập nhật bảng trong modal
    $('#DataGridView1').html(html);
}

function updateDashboard(data) {
    updateSummaryData(data);
    updateMachineData(data);
    updateStopInfo(data);
}

function updateSummaryData(data) {
    if (!data || !data.DataGridView1) return;

    // Khởi tạo biến tổng hợp
    let twistTotalQty = 0;
    let twistTotalRatCount = 0;
    let twistTotalRat = 0;
    let twistTotalUph = 0;

    let weldingTotalQty = 0;
    let weldingTotalRatCount = 0;
    let weldingTotalRat = 0;
    let weldingTotalUph = 0;

    let crimpingTotalQty = 0;
    let crimpingTotalRatCount = 0;
    let crimpingTotalRat = 0;
    let crimpingTotalUph = 0;

    // Duyệt qua dữ liệu của từng máy
    for (let i = 0; i < data.DataGridView1.length; i++) {
        let machine = data.DataGridView1[i];
        let mcNo = machine.MC_NO;

        if (!mcNo) continue;

        // Tính tổng cho Twist (Z1*)
        if (mcNo.startsWith("Z1")) {
            twistTotalQty += parseInt(machine.SUM || 0);
            twistTotalRatCount++;
            twistTotalRat += parseFloat(machine.RUN_RAT || 0);
            twistTotalUph += parseInt(machine.UPH || 0);
        }
        // Tính tổng cho Welding (S1*)
        else if (mcNo.startsWith("S1")) {
            weldingTotalQty += parseInt(machine.SUM || 0);
            weldingTotalRatCount++;
            weldingTotalRat += parseFloat(machine.RUN_RAT || 0);
            weldingTotalUph += parseInt(machine.UPH || 0);
        }
        // Tính tổng cho Crimping (B2*)
        else if (mcNo.startsWith("B2")) {
            crimpingTotalQty += parseInt(machine.SUM || 0);
            crimpingTotalRatCount++;
            crimpingTotalRat += parseFloat(machine.RUN_RAT || 0);
            crimpingTotalUph += parseInt(machine.UPH || 0);
        }
    }

    // Cập nhật UI với dữ liệu tổng hợp
    // Twist
    $("#Label2").text(formatNumber(twistTotalQty));
    $("#Label3").text(twistTotalRatCount > 0 ? Math.round((twistTotalRat / twistTotalRatCount) * 100) + "%" : "0%");
    $("#Label4").text(formatNumber(twistTotalRatCount > 0 ? Math.round(twistTotalUph / twistTotalRatCount) : 0));

    // Welding
    $("#Label12").text(formatNumber(weldingTotalQty));
    $("#Label11").text(weldingTotalRatCount > 0 ? Math.round((weldingTotalRat / weldingTotalRatCount) * 100) + "%" : "0%");
    $("#Label9").text(formatNumber(weldingTotalRatCount > 0 ? Math.round(weldingTotalUph / weldingTotalRatCount) : 0));

    // Crimping
    $("#Label19").text(formatNumber(crimpingTotalQty));
    $("#Label18").text(crimpingTotalRatCount > 0 ? Math.round((crimpingTotalRat / crimpingTotalRatCount) * 100) + "%" : "0%");
    $("#Label17").text(formatNumber(crimpingTotalRatCount > 0 ? Math.round(crimpingTotalUph / crimpingTotalRatCount) : 0));
}

function updateMachineData(data) {
    if (!data || !data.DataGridView1) return;
    $(".machine-data").css("background-color", "#ccc");

    for (let i = 0; i < data.DataGridView1.length; i++) {
        let machine = data.DataGridView1[i];
        let mcNo = machine.MC_NO;
        if (!mcNo) continue;

        let container;
        if (mcNo.startsWith("Z1")) {
            container = $(`#FLP_Z [id^="TW_NM"]:contains("${mcNo}")`).closest(".machine-container");
        } else if (mcNo.startsWith("S1")) {
            container = $(`#FLP_S [id^="WD_NM"]:contains("${mcNo}")`).closest(".machine-container");
        } else if (mcNo.startsWith("B2")) {
            container = $(`#PLP_CR [id^="CR_NM"]:contains("${mcNo}")`).closest(".machine-container");
        }

        if (container && container.length) {
            let dataElements = container.find(".machine-data div");
            $(dataElements[0]).text(formatNumber(machine.SUM));
            $(dataElements[1]).text((parseFloat(machine.RUN_RAT) * 100) + "%");
            $(dataElements[2]).text(formatNumber(machine.UPH));
            container.find(".machine-data").css("background-color", "lime");
        }
    }
}

function updateStopInfo(data) {
    if (!data || !data.DataGridView2) return;

    for (let i = 0; i < data.DataGridView2.length; i++) {
        let stopInfo = data.DataGridView2[i];
        let mcName = stopInfo.tsnon_oper_mitor_MCNM;
        let status = stopInfo.tsnon_oper_mitor_RUNYN;
        let reason = stopInfo.tsnon_oper_mitor_NOIC;

        if (!mcName) continue;

        let container;
        if (mcName.startsWith("Z1")) {
            container = $(`#FLP_Z [id^="TW_NM"]:contains("${mcName}")`).closest(".machine-container");
        } else if (mcName.startsWith("S1")) {
            container = $(`#FLP_S [id^="WD_NM"]:contains("${mcName}")`).closest(".machine-container");
        } else if (mcName.startsWith("B2")) {
            container = $(`#PLP_CR [id^="CR_NM"]:contains("${mcName}")`).closest(".machine-container");
        }

        if (container && container.length) {
            if (status === "W" || status === "Y") {
                container.find(".machine-data").css("background-color", "red");
                container.find(".machine-status").text(reason || "Không có người vận hành");
            }
        }
    }
}

function displayProductionData(data) {
    if (!data || !data.DataGridView3) return;

    let dataHtml = "<div class='production-data-container'>";
    dataHtml += "<h3>Chi tiết sản xuất</h3>";
    dataHtml += "<table class='production-table'>";
    dataHtml += "<thead><tr><th>Máy</th><th>Sản lượng</th><th>Tỷ lệ hoạt động</th><th>UPH</th><th>Mã dừng</th><th>Thời gian</th></tr></thead>";
    dataHtml += "<tbody>";

    for (let i = 0; i < data.DataGridView3.length; i++) {
        let item = data.DataGridView3[i];
        dataHtml += "<tr>";
        dataHtml += `<td>${item.MC_NO}</td>`;
        dataHtml += `<td>${formatNumber(item.PRODUCTION)}</td>`;
        dataHtml += `<td>${item.OPERATION_RATE}%</td>`;
        dataHtml += `<td>${formatNumber(item.UPH)}</td>`;
        dataHtml += `<td>${item.STOP_CODE || ""}</td>`;
        dataHtml += `<td>${item.TIME_STAMP || ""}</td>`;
        dataHtml += "</tr>";
    }

    dataHtml += "</tbody></table></div>";

    let detailDialog = $("#productionDetailDialog");
    if (detailDialog.length === 0) {
        $("body").append("<div id='productionDetailDialog'></div>");
        detailDialog = $("#productionDetailDialog");
    }

    detailDialog.html(dataHtml);
    detailDialog.dialog({
        title: "Chi tiết sản xuất",
        width: 800,
        height: 600,
        modal: true
    });
}

function setupMachineContextMenus() {
    $("#FLP_Z .machine-container").on("contextmenu", function (e) {
        e.preventDefault();
        let mcName = $(this).find("[id^='TW_NM']").text();
        let currentColor = $(this).find(".machine-data").css("background-color");
        if (isGreen(currentColor)) {
            showStopMenu(mcName, e.pageX, e.pageY);
        } else {
            showStartMenu(mcName, e.pageX, e.pageY);
        }
    });

    $("#FLP_S .machine-container").on("contextmenu", function (e) {
        e.preventDefault();
        let mcName = $(this).find("[id^='WD_NM']").text();
        let currentColor = $(this).find(".machine-data").css("background-color");
        if (isGreen(currentColor)) {
            showStopMenu(mcName, e.pageX, e.pageY);
        } else {
            showStartMenu(mcName, e.pageX, e.pageY);
        }
    });

    $("#PLP_CR .machine-container").on("contextmenu", function (e) {
        e.preventDefault();
        let mcName = $(this).find("[id^='CR_NM']").text();
        let currentColor = $(this).find(".machine-data").css("background-color");
        if (isGreen(currentColor)) {
            showStopMenu(mcName, e.pageX, e.pageY);
        } else {
            showStartMenu(mcName, e.pageX, e.pageY);
        }
    });
}

function showStartMenu(mcName, x, y) {
    let menuHtml = `<div id="contextMenu" class="context-menu" style="left:${x}px;top:${y}px;">`;
    menuHtml += `<div class="menu-item" onclick="startMachine('${mcName}')">Bắt đầu hoạt động</div>`;
    menuHtml += "</div>";

    $("body").append(menuHtml);

    $(document).on("click", function () {
        $("#contextMenu").remove();
        $(document).off("click");
    });
}

function showStopMenu(mcName, x, y) {
    let menuHtml = `<div id="contextMenu" class="context-menu" style="left:${x}px;top:${y}px;">`;
    menuHtml += `<div class="menu-item" onclick="stopMachineNoWorker('${mcName}')">Dừng - Không có người vận hành</div>`;
    menuHtml += "</div>";

    $("body").append(menuHtml);

    $(document).on("click", function () {
        $("#contextMenu").remove();
        $(document).off("click");
    });
}

function startMachine(mcName) {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter.ListSearchString = [mcName, getCurrentUser()];
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));

    fetch("/H09/MachineStopRun", {
        method: "POST",
        body: formUpload
    }).then((response) => {
        response.json().then((data) => {
            Button1_Click();
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}

function stopMachineNoWorker(mcName) {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter.ListSearchString = [mcName, getCurrentUser()];
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));

    fetch("/H09/MachineStopNoWorker", {
        method: "POST",
        body: formUpload
    }).then((response) => {
        response.json().then((data) => {
            Button1_Click();
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}

function getCurrentUser() {
    return $("#currentUser").val() || "SYSTEM";
}

function startAutoRefresh() {
    setInterval(function () {
        location.reload();
    }, 30000);
}

function formatNumber(num) {
    if (num === undefined || num === null) return "0";
    return new Intl.NumberFormat().format(num);
}

function isGreen(color) {
    if (color.includes("rgb")) {
        let rgbValues = color.match(/\d+/g);
        if (rgbValues && rgbValues.length >= 3) {
            return rgbValues[0] < 150 && rgbValues[1] > 200 && rgbValues[2] < 150;
        }
    }
    return color.includes("lime") || color.includes("green");
}

function OpenWindowByURL(url, width, height) {
    window.open(url, "_blank", `width=${width},height=${height}`);
}

$(document).ready(function () {
    $('.modal').modal();
    setupMachineContextMenus();
    Button1_Click();
    startAutoRefresh();
});