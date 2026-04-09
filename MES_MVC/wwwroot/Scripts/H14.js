let BaseResult;

$("#Button1").click(function () { Button1_Click(); });
$("#Button2").click(function () { Button2_Click(); });
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

function Buttonfind_Click() {
    $("#BackGround").css("display", "flex");
    let BaseParameter = new Object();
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/H14/Buttonfind_Click";

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
    $("#BackGround").css("display", "flex");
    let BaseParameter = new Object();
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/H14/Buttonadd_Click";

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
    $("#BackGround").css("display", "flex");
    let BaseParameter = new Object();
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/H14/Buttonsave_Click";

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
    $("#BackGround").css("display", "flex");
    let BaseParameter = new Object();
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/H14/Buttondelete_Click";

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
    $("#BackGround").css("display", "flex");
    let BaseParameter = new Object();
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/H14/Buttoncancel_Click";

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
    $("#BackGround").css("display", "flex");
    let BaseParameter = new Object();
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/H14/Buttoninport_Click";

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
    $("#BackGround").css("display", "flex");
    let BaseParameter = new Object();
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/H14/Buttonexport_Click";

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
    $("#BackGround").css("display", "flex");
    let BaseParameter = new Object();
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/H14/Buttonprint_Click";

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
    $("#BackGround").css("display", "flex");
    let BaseParameter = new Object();
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/H14/Button1_Click";

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
    $("#BackGround").css("display", "flex");
    let BaseParameter = new Object();
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/H14/Button2_Click";

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

function updateDashboard(data) {
    updateSummaryData(data);
    updateMachineData(data);
    updateStopInfo(data);
    updateKPIs(data);
}

function updateSummaryData(data) {
    if (!data || !data.DataGridView1) return;

    let crimpingTotalQty = 0;
    let crimpingTotalRatCount = 0;
    let crimpingTotalRat = 0;
    let crimpingTotalUph = 0;

    for (let i = 0; i < data.DataGridView1.length; i++) {
        let machine = data.DataGridView1[i];
        let mcNo = machine.MC_NO;

        if (!mcNo) continue;

        if (mcNo === 'B218' || mcNo === 'B219' || mcNo === 'B220' || mcNo === 'B221') {
            crimpingTotalQty += parseInt(machine.SUM || 0);
            crimpingTotalRatCount++;
            crimpingTotalRat += parseFloat(machine.RUN_RAT || 0);
            crimpingTotalUph += parseInt(machine.UPH || 0);
        }
    }

    // Update header stats
    $("#Label19").text(formatNumber(crimpingTotalQty));

    let avgEfficiency = crimpingTotalRatCount > 0 ?
        Math.round((crimpingTotalRat / crimpingTotalRatCount) * 100) : 0;
    $("#Label18").text(avgEfficiency + "%");

    let avgUph = crimpingTotalRatCount > 0 ?
        Math.round(crimpingTotalUph / crimpingTotalRatCount) : 0;
    $("#Label17").text(formatNumber(avgUph));
}

function updateMachineData(data) {
    if (!data || !data.DataGridView1) return;

    // Reset all machine statuses to stopped
    $(".machine-card .machine-status").removeClass("status-running status-warning status-stopped").addClass("status-stopped");
    $(".progress-fill").css("width", "0%").css("background", "var(--danger-color)");

    for (let i = 0; i < data.DataGridView1.length; i++) {
        let machine = data.DataGridView1[i];
        let mcNo = machine.MC_NO;
        if (!mcNo) continue;

        if (mcNo === 'B218' || mcNo === 'B219' || mcNo === 'B220' || mcNo === 'B221') {
            let machineNum = mcNo.substring(1);
            let container = $(`#CR_NM${machineNum}`).closest(".machine-card");

            if (container && container.length) {
                // Update production value
                container.find(".metric-row:nth-child(1) .metric-value").text(formatNumber(machine.SUM));

                // Update efficiency
                let efficiency = (parseFloat(machine.RUN_RAT) * 100).toFixed(0);
                container.find(".metric-row:nth-child(2) .metric-value").text(efficiency + "%");

                // Update UPH
                container.find(".metric-row:nth-child(3) .metric-value").text(formatNumber(machine.UPH));

                // Update progress bar
                container.find(".progress-fill").css("width", efficiency + "%");

                // Update status color based on efficiency
                if (efficiency >= 80) {
                    container.find(".machine-status").removeClass("status-warning status-stopped").addClass("status-running");
                    container.find(".progress-fill").css("background", "var(--success-color)");
                } else if (efficiency >= 50) {
                    container.find(".machine-status").removeClass("status-running status-stopped").addClass("status-warning");
                    container.find(".progress-fill").css("background", "var(--warning-color)");
                } else {
                    container.find(".machine-status").removeClass("status-running status-warning").addClass("status-stopped");
                    container.find(".progress-fill").css("background", "var(--danger-color)");
                }
            }
        }
    }
}

function updateStopInfo(data) {
    if (!data || !data.DataGridView2) return;
   
    $(".machine-status-text").text("");

    for (let i = 0; i < data.DataGridView2.length; i++) {
        let stopInfo = data.DataGridView2[i];
        let mcName = stopInfo.tsnon_oper_mitor_MCNM;
        let status = stopInfo.tsnon_oper_mitor_RUNYN;
        let reason = stopInfo.tsnon_oper_mitor_NOIC;

        if (!mcName) continue;

        if (mcName === 'B218' || mcName === 'B219' || mcName === 'B220' || mcName === 'B221') {
            let machineNum = mcName.substring(1);
            let container = $(`#CR_NM${machineNum}`).closest(".machine-card");

            if (container && container.length) {
                if (status === "W" || status === "Y") {
                    container.find(".machine-status").removeClass("status-running status-warning").addClass("status-stopped");
                    container.find(".progress-fill").css("background", "var(--danger-color)");

                    // Show status reason in the dedicated status text area
                    container.find(".machine-status-text").text(reason || "No Worker").css("color", "var(--danger-color)");
                }
            }
        }
    }
}

function updateKPIs(data) {
    if (!data || !data.DataGridView1) return;

    let totalRat = 0;
    let totalMachines = 0;

    for (let i = 0; i < data.DataGridView1.length; i++) {
        let machine = data.DataGridView1[i];
        let mcNo = machine.MC_NO;

        if (!mcNo) continue;

        if (mcNo === 'B218' || mcNo === 'B219' || mcNo === 'B220' || mcNo === 'B221') {
            totalRat += parseFloat(machine.RUN_RAT || 0);
            totalMachines++;
        }
    }

    // Calculate KPI values
    let availability = totalMachines > 0 ? Math.round((totalRat / totalMachines) * 100) : 0;
    let performance = totalMachines > 0 ? Math.round((totalRat / totalMachines) * 85) : 0; // Simplified calculation
    let quality = 99.2; // Example fixed value, could be calculated from data if available
    let oee = Math.round((availability * performance * quality) / 10000);

    // Update KPI cards
    $(".kpi-card:nth-child(1) .kpi-value").text(availability + "%");
    $(".kpi-card:nth-child(2) .kpi-value").text(performance + "%");
    $(".kpi-card:nth-child(3) .kpi-value").text(quality + "%");
    $(".kpi-card:nth-child(4) .kpi-value").text(oee + "%");

    // Add trend indicators (simplified, would need historical data for real comparison)
    if (availability > 80) {
        $(".kpi-card:nth-child(1)").addClass("success").removeClass("warning danger");
        $(".kpi-card:nth-child(1) .kpi-trend").addClass("trend-up").removeClass("trend-down").text("↑ vs last week");
    } else if (availability > 50) {
        $(".kpi-card:nth-child(1)").addClass("warning").removeClass("success danger");
        $(".kpi-card:nth-child(1) .kpi-trend").removeClass("trend-up trend-down").text("― vs last week");
    } else {
        $(".kpi-card:nth-child(1)").addClass("danger").removeClass("success warning");
        $(".kpi-card:nth-child(1) .kpi-trend").addClass("trend-down").removeClass("trend-up").text("↓ vs last week");
    }

    if (performance > 80) {
        $(".kpi-card:nth-child(2)").addClass("success").removeClass("warning danger");
        $(".kpi-card:nth-child(2) .kpi-trend").addClass("trend-up").removeClass("trend-down").text("↑ vs last week");
    } else if (performance > 50) {
        $(".kpi-card:nth-child(2)").addClass("warning").removeClass("success danger");
        $(".kpi-card:nth-child(2) .kpi-trend").removeClass("trend-up trend-down").text("― vs last week");
    } else {
        $(".kpi-card:nth-child(2)").addClass("danger").removeClass("success warning");
        $(".kpi-card:nth-child(2) .kpi-trend").addClass("trend-down").removeClass("trend-up").text("↓ vs last week");
    }

    if (oee > 80) {
        $(".kpi-card:nth-child(4)").addClass("success").removeClass("warning danger");
        $(".kpi-card:nth-child(4) .kpi-trend").addClass("trend-up").removeClass("trend-down").text("↑ vs last week");
    } else if (oee > 50) {
        $(".kpi-card:nth-child(4)").addClass("warning").removeClass("success danger");
        $(".kpi-card:nth-child(4) .kpi-trend").removeClass("trend-up trend-down").text("― vs last week");
    } else {
        $(".kpi-card:nth-child(4)").addClass("danger").removeClass("success warning");
        $(".kpi-card:nth-child(4) .kpi-trend").addClass("trend-down").removeClass("trend-up").text("↓ vs last week");
    }
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

function setupMachineContextMenus() {
    $(".machine-card").on("contextmenu", function (e) {
        e.preventDefault();
        let mcName = $(this).find("[id^='CR_NM']").text();
        let currentStatus = $(this).find(".machine-status").hasClass("status-running");

        if (currentStatus) {
            showStopMenu(mcName, e.pageX, e.pageY);
        } else {
            showStartMenu(mcName, e.pageX, e.pageY);
        }
    });
}

function showStartMenu(mcName, x, y) {
    // Remove any existing context menu
    $("#contextMenu").remove();

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
    // Remove any existing context menu
    $("#contextMenu").remove();

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
    $("#BackGround").css("display", "flex");
    let BaseParameter = new Object();
    BaseParameter.ListSearchString = [mcName, getCurrentUser()];
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));

    fetch("/H14/MachineStopRun", {
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
    $("#BackGround").css("display", "flex");
    let BaseParameter = new Object();
    BaseParameter.ListSearchString = [mcName, getCurrentUser()];
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));

    fetch("/H14/MachineStopNoWorker", {
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
function loadChartData() {
    if (typeof Chart === 'undefined') {
        return;
    }

    $("#BackGround").css("display", "flex");
    let BaseParameter = new Object();
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/H14/GetChartData";

    fetch(url, {
        method: "POST",
        body: formUpload
    }).then((response) => {
        if (!response.ok) {
            throw new Error(`HTTP error! Status: ${response.status}`);
        }
        return response.json();
    }).then((data) => {
        if (data.ProductionTrend && data.ProductionTrend.length > 0) {
            createProductionChart(data.ProductionTrend);
        }

        if (data.DowntimeAnalysis && data.DowntimeAnalysis.length > 0) {
            createDowntimeChart(data.DowntimeAnalysis);
        }

        $("#BackGround").css("display", "none");
    }).catch((err) => {
        $("#BackGround").css("display", "none");
    });
}

function createProductionChart(data) {
    const canvas = document.getElementById('productionChart');
    if (!canvas) {
        return;
    }

    const ctx = canvas.getContext('2d');

    if (window.productionChart instanceof Chart) {
        window.productionChart.destroy();
    }

    window.productionChart = new Chart(ctx, {
        type: 'line',
        data: {
            labels: data.map(item => item.Hour || ""),
            datasets: [
                {
                    label: 'B218',
                    data: data.map(item => item.B218 || 0),
                    borderColor: '#10b981',
                    backgroundColor: 'rgba(16, 185, 129, 0.1)',
                    borderWidth: 2,
                    tension: 0.3,
                    fill: true
                },
                {
                    label: 'B219',
                    data: data.map(item => item.B219 || 0),
                    borderColor: '#2563eb',
                    backgroundColor: 'rgba(37, 99, 235, 0.1)',
                    borderWidth: 2,
                    tension: 0.3,
                    fill: true
                },
                {
                    label: 'B220',
                    data: data.map(item => item.B220 || 0),
                    borderColor: '#f59e0b',
                    backgroundColor: 'rgba(245, 158, 11, 0.1)',
                    borderWidth: 2,
                    tension: 0.3,
                    fill: true
                },
                {
                    label: 'B221',
                    data: data.map(item => item.B221 || 0),
                    borderColor: '#ef4444',
                    backgroundColor: 'rgba(239, 68, 68, 0.1)',
                    borderWidth: 2,
                    tension: 0.3,
                    fill: true
                }
            ]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            plugins: {
                legend: {
                    position: 'top',
                    labels: {
                        color: '#f1f5f9',
                        padding: 15
                    }
                },
                tooltip: {
                    mode: 'index',
                    intersect: false,
                    backgroundColor: 'rgba(51, 65, 85, 0.9)'
                }
            },
            scales: {
                x: {
                    grid: {
                        color: 'rgba(255, 255, 255, 0.1)'
                    },
                    ticks: {
                        color: '#94a3b8'
                    }
                },
                y: {
                    grid: {
                        color: 'rgba(255, 255, 255, 0.1)'
                    },
                    ticks: {
                        color: '#94a3b8'
                    },
                    beginAtZero: true
                }
            }
        }
    });
}

function createDowntimeChart(data) {
    const canvas = document.getElementById('downtimeChart');
    if (!canvas) {
        return;
    }

    const ctx = canvas.getContext('2d');

    if (window.downtimeChart instanceof Chart) {
        window.downtimeChart.destroy();
    }

    const colors = data.map(item => {
        switch (item.PART_CODE) {
            case 'L': return '#10b981';
            case 'T': return '#2563eb';
            case 'M': return '#ef4444';
            case 'S': return '#f59e0b';
            case 'I': return '#7c3aed';
            case 'Q': return '#ec4899';
            default: return '#94a3b8';
        }
    });

    window.downtimeChart = new Chart(ctx, {
        type: 'doughnut',
        data: {
            labels: data.map(item => `${item.PART_CODE}: ${item.Reason || ""}`),
            datasets: [{
                data: data.map(item => item.DURATION || 0),
                backgroundColor: colors,
                borderWidth: 1,
                borderColor: '#1e293b'
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            plugins: {
                legend: {
                    position: 'right',
                    labels: {
                        color: '#f1f5f9',
                        padding: 15
                    }
                },
                tooltip: {
                    backgroundColor: 'rgba(51, 65, 85, 0.9)',
                    callbacks: {
                        label: function (context) {
                            const value = context.raw;
                            const total = context.chart.data.datasets[0].data.reduce((a, b) => a + b, 0);
                            const percent = Math.round((value / total) * 100);

                            const hours = Math.floor(value / 60);
                            const minutes = value % 60;
                            return `${hours}h ${minutes}m (${percent}%)`;
                        }
                    }
                }
            },
            cutout: '50%'
        }
    });
}
function getCurrentUser() {
    return $("#currentUser").val() || "SYSTEM";
}

function formatNumber(num) {
    if (num === undefined || num === null) return "0";
    return new Intl.NumberFormat().format(num);
}

function OpenWindowByURL(url, width, height) {
    window.open(url, "_blank", `width=${width},height=${height}`);
}

function startAutoRefresh() {
    setInterval(function () {
        Button1_Click();
    }, 30000);
}

$(document).ready(function () {
    $('.modal').modal();
    setupMachineContextMenus();
    Button1_Click();
    loadChartData(); 
    startAutoRefresh();
    $("#Button1").click(function () {
        Button1_Click();
        loadChartData();
    });
});