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
    let url = "/H13/Buttonfind_Click";

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
    let url = "/H13/Buttonadd_Click";

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
    let url = "/H13/Buttonsave_Click";

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
    let url = "/H13/Buttondelete_Click";

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
    let url = "/H13/Buttoncancel_Click";

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
    let url = "/H13/Buttoninport_Click";

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
    let url = "/H13/Buttonexport_Click";

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
    let url = "/H13/Buttonprint_Click";

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
    let url = "/H13/Button1_Click";

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
    let url = "/H13/Button2_Click";

    fetch(url, {
        method: "POST",
        body: formUpload
    }).then((response) => {
        return response.json().then((data) => {
            $("#BackGround").css("display", "none");
            if (data && data.DataGridView3 && data.DataGridView3.length > 0) {
                populateH00_LISTModal(data);
                $('#H00_LISTModal').modal('open');
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
    $('#DataGridView1').html(html);
}

function updateDashboard(data) {
    updateSummaryData(data);
    updateMachineData(data);
    updateStopInfo(data);
}

function updateSummaryData(data) {
    if (!data || !data.DataGridView1) return;

    let twistTotalQty = 0;
    let twistTotalRatCount = 0;
    let twistTotalRat = 0;
    let twistTotalUph = 0;

    let weldingTotalQty = 0;
    let weldingTotalRatCount = 0;
    let weldingTotalRat = 0;
    let weldingTotalUph = 0;

    for (let i = 0; i < data.DataGridView1.length; i++) {
        let machine = data.DataGridView1[i];
        let mcNo = machine.MC_NO;

        if (!mcNo) continue;

        if (mcNo.startsWith("ZZ1")) {
            twistTotalQty += parseInt(machine.SUM || 0);
            twistTotalRatCount++;
            twistTotalRat += parseFloat(machine.RUN_RAT || 0);
            twistTotalUph += parseInt(machine.UPH || 0);
        }
        else if (mcNo.startsWith("ZS1")) {
            weldingTotalQty += parseInt(machine.SUM || 0);
            weldingTotalRatCount++;
            weldingTotalRat += parseFloat(machine.RUN_RAT || 0);
            weldingTotalUph += parseInt(machine.UPH || 0);
        }
    }

    $("#Label2").text(formatNumber(twistTotalQty));
    $("#Label3").text(twistTotalRatCount > 0 ? Math.round((twistTotalRat / twistTotalRatCount) * 100) + "%" : "0%");
    $("#Label4").text(formatNumber(twistTotalRatCount > 0 ? Math.round(twistTotalUph / twistTotalRatCount) : 0));

    $("#Label12").text(formatNumber(weldingTotalQty));
    $("#Label11").text(weldingTotalRatCount > 0 ? Math.round((weldingTotalRat / weldingTotalRatCount) * 100) + "%" : "0%");
    $("#Label9").text(formatNumber(weldingTotalRatCount > 0 ? Math.round(weldingTotalUph / weldingTotalRatCount) : 0));
}

function updateMachineData(data) {
    if (!data || !data.DataGridView1) return;
    $(".machine-data-box").css("background-color", "silver");

    for (let i = 0; i < data.DataGridView1.length; i++) {
        let machine = data.DataGridView1[i];
        let mcNo = machine.MC_NO;
        if (!mcNo) continue;

        let dataBox;
        if (mcNo.startsWith("ZZ1")) {
            let machineNum = mcNo.substring(2);
            dataBox = $(`#TW_LY${machineNum.substring(1)} .machine-data-box`);
            $(`#Z${machineNum}1`).text(formatNumber(machine.SUM || 0));
            $(`#Z${machineNum}2`).text((parseFloat(machine.RUN_RAT || 0) * 100).toFixed(0) + "%");
            $(`#Z${machineNum}3`).text(formatNumber(machine.UPH || 0));
            dataBox.css("background-color", "lime");
        }
        else if (mcNo.startsWith("ZS1")) {
            let machineNum = mcNo.substring(2);
            dataBox = $(`#WD_LY${machineNum.substring(1)} .machine-data-box`);
            $(`#S${machineNum}1`).text(formatNumber(machine.SUM || 0));
            $(`#S${machineNum}2`).text((parseFloat(machine.RUN_RAT || 0) * 100).toFixed(0) + "%");
            $(`#S${machineNum}3`).text(formatNumber(machine.UPH || 0));
            dataBox.css("background-color", "lime");
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

        let dataBox;
        if (mcName.startsWith("ZZ1")) {
            let machineNum = mcName.substring(2);
            dataBox = $(`#TW_LY${machineNum.substring(1)} .machine-data-box`);
            if (status === "W") {
                dataBox.css("background-color", "orange");
                $(`#TW_STOP${machineNum.substring(1)}`).text(reason || "No Worker");
            } else if (status !== "N") {
                dataBox.css("background-color", "red");
                $(`#TW_STOP${machineNum.substring(1)}`).text(reason || "Stopped");
            }
        }
        else if (mcName.startsWith("ZS1")) {
            let machineNum = mcName.substring(2);
            dataBox = $(`#WD_LY${machineNum.substring(1)} .machine-data-box`);
            if (status === "W") {
                dataBox.css("background-color", "orange");
                $(`#WD_STOP${machineNum.substring(1)}`).text(reason || "No Worker");
            } else if (status !== "N") {
                dataBox.css("background-color", "red");
                $(`#WD_STOP${machineNum.substring(1)}`).text(reason || "Stopped");
            }
        }
    }
}

function setupTwistContextMenus() {
    for (let i = 1; i <= 8; i++) {
        const contextMenuId = `TW0${i}ContextMenu`;
        $(`#TW_LY0${i}`).on("contextmenu", function (e) {
            e.preventDefault();
            $(`.dropdown-content`).removeClass('active');
            $(`#${contextMenuId}`).addClass('active');
            $(`#${contextMenuId}`).css({
                display: "block",
                left: e.pageX + "px",
                top: e.pageY + "px"
            });
            $(document).one("click", function () {
                $(`#${contextMenuId}`).css("display", "none");
            });
        });
    }
    $("#ToolStripMenuItem1").click(function () { startMachine("ZZ101"); });
    $("#ToolStripMenuItem3").click(function () { startMachine("ZZ102"); });
    $("#ToolStripMenuItem5").click(function () { startMachine("ZZ103"); });
    $("#ToolStripMenuItem7").click(function () { startMachine("ZZ104"); });
    $("#ToolStripMenuItem9").click(function () { startMachine("ZZ105"); });
    $("#ToolStripMenuItem11").click(function () { startMachine("ZZ106"); });
    $("#ToolStripMenuItem13").click(function () { startMachine("ZZ107"); });
    $("#ToolStripMenuItem15").click(function () { startMachine("ZZ108"); });

    $("#ToolStripMenuItem2").click(function () { stopMachineNoWorker("ZZ101"); });
    $("#ToolStripMenuItem4").click(function () { stopMachineNoWorker("ZZ102"); });
    $("#ToolStripMenuItem6").click(function () { stopMachineNoWorker("ZZ103"); });
    $("#ToolStripMenuItem8").click(function () { stopMachineNoWorker("ZZ104"); });
    $("#ToolStripMenuItem10").click(function () { stopMachineNoWorker("ZZ105"); });
    $("#ToolStripMenuItem12").click(function () { stopMachineNoWorker("ZZ106"); });
    $("#ToolStripMenuItem14").click(function () { stopMachineNoWorker("ZZ107"); });
    $("#ToolStripMenuItem16").click(function () { stopMachineNoWorker("ZZ108"); });
}

function setupWeldingContextMenus() {
    for (let i = 1; i <= 8; i++) {
        const contextMenuId = `WD0${i}ContextMenu`;
        $(`#WD_LY0${i}`).on("contextmenu", function (e) {
            e.preventDefault();
            $(`.dropdown-content`).removeClass('active');
            $(`#${contextMenuId}`).addClass('active');
            $(`#${contextMenuId}`).css({
                display: "block",
                left: e.pageX + "px",
                top: e.pageY + "px"
            });
            $(document).one("click", function () {
                $(`#${contextMenuId}`).css("display", "none");
            });
        });
    }
    $("#ToolStripMenuItem17").click(function () { startMachine("ZS101"); });
    $("#ToolStripMenuItem19").click(function () { startMachine("ZS102"); });
    $("#ToolStripMenuItem21").click(function () { startMachine("ZS103"); });
    $("#ToolStripMenuItem23").click(function () { startMachine("ZS104"); });
    $("#ToolStripMenuItem25").click(function () { startMachine("ZS105"); });
    $("#ToolStripMenuItem27").click(function () { startMachine("ZS106"); });
    $("#ToolStripMenuItem29").click(function () { startMachine("ZS107"); });
    $("#ToolStripMenuItem31").click(function () { startMachine("ZS108"); });

    $("#ToolStripMenuItem18").click(function () { stopMachineNoWorker("ZS101"); });
    $("#ToolStripMenuItem20").click(function () { stopMachineNoWorker("ZS102"); });
    $("#ToolStripMenuItem22").click(function () { stopMachineNoWorker("ZS103"); });
    $("#ToolStripMenuItem24").click(function () { stopMachineNoWorker("ZS104"); });
    $("#ToolStripMenuItem26").click(function () { stopMachineNoWorker("ZS105"); });
    $("#ToolStripMenuItem28").click(function () { stopMachineNoWorker("ZS106"); });
    $("#ToolStripMenuItem30").click(function () { stopMachineNoWorker("ZS107"); });
    $("#ToolStripMenuItem32").click(function () { stopMachineNoWorker("ZS108"); });
}

function startMachine(mcName) {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter.ListSearchString = [mcName, getCurrentUser()];
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));

    fetch("/H13/MachineStopRun", {
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

    fetch("/H13/MachineStopNoWorker", {
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

function startAutoRefresh() {
    setInterval(function () {
        Button1_Click();
    }, 30000);
}

function OpenWindowByURL(url, width, height) {
    window.open(url, "_blank", `width=${width},height=${height}`);
}

$(document).ready(function () {
    $('.modal').modal();
    setupTwistContextMenus();
    setupWeldingContextMenus();
    Button1_Click();
    startAutoRefresh();
});
