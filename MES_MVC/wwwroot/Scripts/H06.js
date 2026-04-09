let BaseResult;
let productionChart = null;

// ===== Event Bindings =====
$("#Button1").click(function () { Button1_Click(); });
$("#Button2").click(function () { Button2_Click(); });
$("#CheckBox1").change(function () { CheckBox1_Change(); });
$("#chartMonth").change(function () { loadMonthlyChart(); });

// Date inputs change event
$("#fromDate, #toDate").change(function () {
    validateAndReload();
});

// Context menu cho 10 máy
$("#LY01").on("contextmenu", function (e) { e.preventDefault(); showMachineMenu(1, e); });
$("#LY02").on("contextmenu", function (e) { e.preventDefault(); showMachineMenu(2, e); });
$("#LY03").on("contextmenu", function (e) { e.preventDefault(); showMachineMenu(3, e); });
$("#LY04").on("contextmenu", function (e) { e.preventDefault(); showMachineMenu(4, e); });
$("#LY05").on("contextmenu", function (e) { e.preventDefault(); showMachineMenu(5, e); });
$("#LY06").on("contextmenu", function (e) { e.preventDefault(); showMachineMenu(6, e); });
$("#LY07").on("contextmenu", function (e) { e.preventDefault(); showMachineMenu(7, e); });
$("#LY08").on("contextmenu", function (e) { e.preventDefault(); showMachineMenu(8, e); });
$("#LY09").on("contextmenu", function (e) { e.preventDefault(); showMachineMenu(9, e); });
$("#LY10").on("contextmenu", function (e) { e.preventDefault(); showMachineMenu(10, e); });

// ===== Date Functions =====
function setDefaultDates() {
    const today = new Date();
    const formatDate = (date) => {
        const year = date.getFullYear();
        const month = String(date.getMonth() + 1).padStart(2, '0');
        const day = String(date.getDate()).padStart(2, '0');
        return `${year}-${month}-${day}`;
    };

    const todayStr = formatDate(today);
    $("#fromDate").val(todayStr);
    $("#toDate").val(todayStr);

    // Set max date = today
    $("#fromDate").attr("max", todayStr);
    $("#toDate").attr("max", todayStr);
}

function setDefaultMonth() {
    const today = new Date();
    const year = today.getFullYear();
    const month = String(today.getMonth() + 1).padStart(2, '0');
    $("#chartMonth").val(`${year}-${month}`);
}

function validateAndReload() {
    let fromDate = $("#fromDate").val();
    let toDate = $("#toDate").val();

    // Validation 1: Both dates must be selected
    if (!fromDate || !toDate) {
        return;
    }

    // Validation 2: fromDate <= toDate
    if (new Date(fromDate) > new Date(toDate)) {
        alert("Ngày bắt đầu phải nhỏ hơn hoặc bằng ngày kết thúc!");
        // Auto swap
        $("#fromDate").val(toDate);
        $("#toDate").val(fromDate);
        return;
    }

    // Validation 3: Max 31 days range (optional)
    const daysDiff = Math.floor((new Date(toDate) - new Date(fromDate)) / (1000 * 60 * 60 * 24));
    if (daysDiff > 31) {
        alert("Khoảng thời gian không được vượt quá 31 ngày!");
        return;
    }

    // All good, reload data
    Buttonfind_Click();
}

// ===== Machine Context Menu =====
function showMachineMenu(machineId, e) {
    let two = machineId.toString().padStart(2, '0');
    let machineName = $(`#MC_NM${two}`).text();
    let machineColor = $(`#LY${two}`).css("background-color");

    let menuOptions = '';
    if (isOrangeColor(machineColor)) {
        menuOptions = `<div class="machine-menu-item" onclick="handleMachineRun('${machineName}')">Run Machine</div>`;
    } else if (isGreenColor(machineColor)) {
        menuOptions = `<div class="machine-menu-item" onclick="handleMachineStop('${machineName}')">Stop Machine</div>`;
    }

    $('.machine-context-menu').remove();

    let menu = $('<div class="machine-context-menu"></div>').html(menuOptions);
    $('body').append(menu);

    menu.css({
        top: e.pageY + 'px',
        left: e.pageX + 'px'
    });

    $(document).one('click', function () {
        menu.remove();
    });
}

function isOrangeColor(color) {
    return color && (
        color.includes('rgb(255, 165, 0)') ||
        color.includes('rgb(255,165,0)') ||
        color.includes('orange')
    );
}

function isGreenColor(color) {
    return color && (
        color.includes('rgb(0, 255, 0)') ||
        color.includes('rgb(0,255,0)') ||
        color.includes('lime')
    );
}

function handleMachineRun(machineName) {
    $("#BackGround").css("display", "block");
    let BaseParameter = {
        MC_NAME: machineName,
        USER_ID: $("#user_id").val() || "System",
        Action: 1
    };
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/H06/Buttonsave_Click";

    fetch(url, { method: "POST", body: formUpload }).then((response) => {
        response.json().then(() => {
            $("#BackGround").css("display", "none");
            Buttonfind_Click();
        }).catch(() => { $("#BackGround").css("display", "none"); });
    });
}

function handleMachineStop(machineName) {
    $("#BackGround").css("display", "block");
    let BaseParameter = {
        MC_NAME: machineName,
        USER_ID: $("#user_id").val() || "System",
        Action: 2
    };
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/H06/Buttonsave_Click";

    fetch(url, { method: "POST", body: formUpload }).then((response) => {
        response.json().then(() => {
            $("#BackGround").css("display", "none");
            Buttonfind_Click();
        }).catch(() => { $("#BackGround").css("display", "none"); });
    });
}

// ===== Main Functions =====
function Buttonfind_Click() {
    $("#BackGround").css("display", "block");

    let BaseParameter = {
        START_DATE: $("#fromDate").val(),
        END_DATE: $("#toDate").val()
    };

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/H06/Buttonfind_Click";

    fetch(url, { method: "POST", body: formUpload }).then((response) => {
        response.json().then((data) => {
            BaseResult = data;
            updateDashboard(data);
            $("#BackGround").css("display", "none");
        }).catch(() => { $("#BackGround").css("display", "none"); });
    });
}

function Button1_Click() {
    validateAndReload();
}

function Button2_Click() {
    if (BaseResult && BaseResult.DataGridView1) {
        displayDetailData(BaseResult.DataGridView1);
    } else {
        Dgv_reload();
    }
}

function displayDetailData(data) {
    let tableBody = '';
    if (data && data.length > 0) {
        data.forEach((row, index) => {
            tableBody += '<tr>';
            tableBody += `<td>${index + 1}</td>`;
            tableBody += `<td>${row.DATE || ''}</td>`;
            tableBody += `<td>${row.MC_NO || ''}</td>`;
            tableBody += `<td>${row.TS_TIME_ST || ''}</td>`;
            tableBody += `<td>${row.TS_TIME_END || ''}</td>`;
            tableBody += `<td>${formatNumber(row.SUM || 0)}</td>`;
            tableBody += `<td>${row.STOP_TIME || 0}</td>`;
            tableBody += `<td>${row.T_STOP || 0}</td>`;
            tableBody += `<td>${row.WORK_TIME || 0}</td>`;
            tableBody += `<td>${row.RUN_RAT ? (row.RUN_RAT * 100).toFixed(0) + '%' : '0%'}</td>`;
            tableBody += `<td>${formatNumber(row.UPH || 0)}</td>`;
            tableBody += '</tr>';
        });
    }
    $('#DataGridView1').html(tableBody);
    $('#H00_LISTModal').modal('open');
}

function Dgv_reload() {
    $("#BackGround").css("display", "block");
    let url = "/H06/Dgv_reload";

    fetch(url, { method: "POST" }).then((response) => {
        response.json().then((data) => {
            BaseResult = data;
            displayDetailData(data.DataGridView1);
            $("#BackGround").css("display", "none");
        }).catch(() => { $("#BackGround").css("display", "none"); });
    });
}

// ===== Auto Reload =====
function CheckBox1_Change() {
    if ($("#CheckBox1").prop("checked")) {
        startAutoReload();
        $("#Button1").css("display", "none");
    } else {
        stopAutoReload();
        $("#Button1").css("display", "inline-block");
    }
}

let autoReloadTimer;

function startAutoReload() {
    autoReloadTimer = setInterval(function () {
        setDefaultDates();
        Buttonfind_Click();
    }, 30000);
}

function stopAutoReload() {
    if (autoReloadTimer) {
        clearInterval(autoReloadTimer);
    }
}

// ===== Update Dashboard =====
function updateDashboard(data) {
    if (data.Label2) $("#Label2").text(formatNumber(data.Label2));
    if (data.Label3) $("#Label3").text(data.Label3);
    if (data.Label4) $("#Label4").text(formatNumber(data.Label4));

    if (data.DataGridView1) { updateMachineData(data.DataGridView1); }
    if (data.DataGridView2) { updateMachineStatus(data.DataGridView2); }
}

function updateMachineData(machineData) {
    if (!machineData || !machineData.length) return;

    // Reset tất cả về 0
    for (let i = 1; i <= 10; i++) {
        const two = i.toString().padStart(2, '0');
        $(`#A8${two}1`).text("0");
        $(`#A8${two}2`).text("0%");
        $(`#A8${two}3`).text("0");
    }

    // Update data từ server
    machineData.forEach(machine => {
        if (!machine.MC_NO) return;
        if (!machine.MC_NO.startsWith('ZA8')) return;

        const two = machine.MC_NO.slice(-2);
        $(`#A8${two}1`).text(formatNumber(machine.SUM || 0));
        const opRate = machine.RUN_RAT ? (machine.RUN_RAT * 100).toFixed(0) + '%' : '0%';
        $(`#A8${two}2`).text(opRate);
        $(`#A8${two}3`).text(formatNumber(machine.UPH || 0));
    });
}

function updateMachineStatus(statusData) {
    let selectedDate = new Date($("#toDate").val());
    let today = new Date();
    selectedDate.setHours(0, 0, 0, 0);
    today.setHours(0, 0, 0, 0);

    // Initialize: Set color based on production
    for (let i = 1; i <= 10; i++) {
        const two = i.toString().padStart(2, '0');

        if (selectedDate < today) {
            $(`#LY${two}`).css("background-color", "lime");
            $(`#STOP${two}`).text("--");
        } else {
            if ($(`#A8${two}1`).text() !== "0") {
                $(`#LY${two}`).css("background-color", "lime");
            } else {
                $(`#LY${two}`).css("background-color", "silver");
            }
            $(`#STOP${two}`).text("--");
        }
    }

    // Only apply stop status if current date
    if (selectedDate.getTime() === today.getTime()) {
        if (!statusData || !statusData.length) return;

        statusData.forEach(status => {
            if (!status.tsnon_oper_mitor_MCNM) return;
            if (!status.tsnon_oper_mitor_MCNM.startsWith('ZA8')) return;

            const two = status.tsnon_oper_mitor_MCNM.slice(-2);

            if (status.tsnon_oper_mitor_RUNYN === 'W') {
                $(`#LY${two}`).css("background-color", "orange");
                $(`#STOP${two}`).text(status.tsnon_oper_mitor_NOIC || "No Worker");
            } else if (status.tsnon_oper_mitor_RUNYN !== 'N') {
                $(`#LY${two}`).css("background-color", "red");
                $(`#STOP${two}`).text(status.tsnon_oper_mitor_NOIC || "--");
            }
        });
    }
}

// ===== Chart Functions =====
function loadMonthlyChart() {
    let BaseParameter = {
        MONTH: $("#chartMonth").val()
    };

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/H06/GetMonthlyChart";

    fetch(url, { method: "POST", body: formUpload })
        .then(response => response.json())
        .then(data => {
            if (data.DataGridView1) {
                renderChart(data.DataGridView1);
            }
        })
        .catch(err => console.error(err));
}

function renderChart(chartData) {
    const labels = [];
    const qtyValues = [];
    const uphValues = [];

    const month = $("#chartMonth").val();
    const firstDay = new Date(month + "-01");
    const lastDay = new Date(firstDay.getFullYear(), firstDay.getMonth() + 1, 0);

    // Map data từ server
    const dataMap = {};
    if (chartData && chartData.length > 0) {
        chartData.forEach(item => {
            const dateValue = item.DATE || item.Date || item.date;
            const qtyValue = item.TOTAL_QTY || item.Total_QTY || item.total_qty || 0;
            const uphValue = item.UPH || item.uph || 0;

            if (dateValue) {
                const dateOnly = dateValue.substring(0, 10);
                dataMap[dateOnly] = {
                    qty: parseInt(qtyValue) || 0,
                    uph: parseInt(uphValue) || 0
                };
            }
        });
    }

    // Fill tất cả ngày
    for (let d = 1; d <= lastDay.getDate(); d++) {
        const dateStr = `${month}-${String(d).padStart(2, '0')}`;
        labels.push(`${d}`);
        qtyValues.push(dataMap[dateStr]?.qty || 0);
        uphValues.push(dataMap[dateStr]?.uph || 0);
    }

    // Tính trung bình sản lượng (chỉ tính ngày có dữ liệu > 0)
    const validQty = qtyValues.filter(v => v > 0);
    const avgQty = validQty.length > 0 ? Math.round(validQty.reduce((a, b) => a + b, 0) / validQty.length) : 0;
    const avgLine = qtyValues.map(() => avgQty);

    // Destroy chart cũ
    if (productionChart) {
        productionChart.destroy();
    }

    // Đăng ký plugin
    Chart.register(ChartDataLabels);

    // Tạo chart mới
    const ctx = document.getElementById('productionChart').getContext('2d');
    productionChart = new Chart(ctx, {
        type: 'bar',
        data: {
            labels: labels,
            datasets: [
                {
                    type: 'bar',
                    label: '생산량 (Sản lượng)',
                    data: qtyValues,
                    backgroundColor: 'rgba(54, 162, 235, 0.6)',
                    borderColor: 'rgba(54, 162, 235, 1)',
                    borderWidth: 1,
                    yAxisID: 'y',
                    order: 3
                },
                {
                    type: 'line',
                    label: `평균 (TB): ${formatNumber(avgQty)}`,
                    data: avgLine,
                    borderColor: 'rgba(75, 192, 92, 1)',
                    borderWidth: 2,
                    borderDash: [5, 5],
                    pointRadius: 0,
                    fill: false,
                    yAxisID: 'y',
                    order: 2,
                    datalabels: {
                        display: false
                    }
                },
                {
                    type: 'line',
                    label: 'UPH',
                    data: uphValues,
                    borderColor: 'rgba(255, 99, 132, 1)',
                    backgroundColor: 'rgba(255, 99, 132, 0.2)',
                    borderWidth: 2,
                    pointRadius: 3,
                    pointBackgroundColor: 'rgba(255, 99, 132, 1)',
                    fill: false,
                    yAxisID: 'y1',
                    order: 1,
                    datalabels: {
                        display: false
                    }
                }
            ]
        },
        plugins: [ChartDataLabels],
        options: {
            responsive: true,
            maintainAspectRatio: false,
            interaction: {
                mode: 'index',
                intersect: false
            },
            plugins: {
                legend: {
                    display: true,
                    position: 'top'
                },
                tooltip: {
                    callbacks: {
                        label: function (context) {
                            let label = context.dataset.label || '';
                            let value = formatNumber(context.parsed.y);
                            return `${label}: ${value}`;
                        }
                    }
                },
                datalabels: {
                    anchor: 'end',
                    align: 'top',
                    formatter: function (value, context) {
                        if (context.dataset.type === 'line') return '';
                        return value > 0 ? formatNumber(value) : '';
                    },
                    font: {
                        weight: 'bold',
                        size: 14
                    },
                    color: '#333'
                }
            },
            scales: {
                y: {
                    type: 'linear',
                    position: 'left',
                    beginAtZero: true,
                    title: {
                        display: true,
                        text: '생산량 (Sản lượng)',
                        font: { weight: 'bold' }
                    },
                    ticks: {
                        callback: function (value) {
                            return formatNumber(value);
                        }
                    }
                },
                y1: {
                    type: 'linear',
                    position: 'right',
                    beginAtZero: true,
                    title: {
                        display: true,
                        text: 'UPH',
                        font: { weight: 'bold' }
                    },
                    grid: {
                        drawOnChartArea: false
                    },
                    ticks: {
                        callback: function (value) {
                            return formatNumber(value);
                        }
                    }
                },
                x: {
                    grid: {
                        display: false
                    }
                }
            }
        }
    });
}

function formatNumber(num) {
    return new Intl.NumberFormat().format(num);
}

// ===== Document Ready =====
$(document).ready(function () {
    $('.modal').modal();
    setDefaultDates();
    setDefaultMonth();
    Buttonfind_Click();
    loadMonthlyChart();
});