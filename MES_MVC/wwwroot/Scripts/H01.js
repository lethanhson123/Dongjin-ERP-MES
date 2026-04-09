let BaseResult;
let productionChart = null;
$(document).ready(function () {
    setDefaultDates();
    $("#machineTypeFilter").val("CUTTING");
    $("#check_AutoReload").prop("checked", true);
    Timer1_Start();
    $("#Button1").css("display", "none");

    RELOAD();
    $('.modal').modal();

    $("#check_AutoReload").change(function () {
        if (this.checked) {
            Timer1_Start();
            $("#Button1").css("display", "none");
        } else {
            Timer1_Stop();
            $("#Button1").css("display", "block");
        }
    });

    setupMachineMenus();

    $("#machineTypeFilter").change(function () {
        RELOAD();
    });

    $("#fromDate, #toDate").change(function () {
        RELOAD();
    });

    $("#Button1").click(function () {
        RELOAD();
    });

    $("#Button2").click(function () {
        Dgv_Reload_ShowDialog();
    });
    setDefaultChartMonth();
    initProductionChart();
    loadMonthlyChart();
    $("#chartMonth").change(function () {
        loadMonthlyChart();
    });
});
function setDefaultChartMonth() {
    const today = new Date();
    const year = today.getFullYear();
    const month = String(today.getMonth() + 1).padStart(2, '0');
    const monthStr = `${year}-${month}`;

    $("#chartMonth").val(monthStr);
    $("#chartMonth").attr("max", monthStr);
}

function initProductionChart() {
    const ctx = document.getElementById('productionChart').getContext('2d');

    productionChart = new Chart(ctx, {
        type: 'bar',
        data: {
            labels: [],
            datasets: [{
                label: '생산량 (Production)',
                data: [],
                backgroundColor: 'rgba(54, 162, 235, 0.6)',
                borderColor: 'rgb(54, 162, 235)',
                borderWidth: 2,
                borderRadius: 5,
                hoverBackgroundColor: 'rgba(54, 162, 235, 0.8)'
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            plugins: {
                legend: {
                    display: true,
                    position: 'top',
                    labels: {
                        font: {
                            size: 14,
                            weight: 'bold'
                        }
                    }
                },
                tooltip: {
                    callbacks: {
                        label: function (context) {
                            let label = context.dataset.label || '';
                            if (label) {
                                label += ': ';
                            }
                            label += formatNumber(context.parsed.y);
                            return label;
                        }
                    },
                    backgroundColor: 'rgba(0, 0, 0, 0.8)',
                    titleFont: {
                        size: 14
                    },
                    bodyFont: {
                        size: 13
                    },
                    padding: 10
                },
                datalabels: {
                    anchor: 'end',
                    align: 'top',
                    formatter: function (value) {
                        if (value === null || value === 0) return '';
                        return formatNumber(value);
                    },
                    font: {
                        weight: 'bold',
                        size: 14
                    },
                    color: '#333'
                }
            },
            scales: {
                x: {
                    grid: {
                        display: false
                    },
                    ticks: {
                        font: {
                            size: 11
                        }
                    }
                },
                y: {
                    beginAtZero: true,
                    grid: {
                        color: 'rgba(0, 0, 0, 0.05)'
                    },
                    ticks: {
                        callback: function (value) {
                            return formatNumber(value);
                        },
                        font: {
                            size: 12
                        }
                    }
                }
            }
        },
        plugins: [ChartDataLabels]
    });
}

function loadMonthlyChart() {
    $("#BackGround").css("display", "block");

    let month = $("#chartMonth").val();
    let filterType = $("#machineTypeFilter").val() || "ALL";
    let machineList = getMachineList(filterType);

    let BaseParameter = {
        USER_IDX: GetCookieValue("USER_IDX"),
        MONTH: month,
        MACHINE_LIST: machineList
    };

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/H01/GetMonthlyProduction";

    fetch(url, {
        method: "POST",
        body: formUpload
    })
        .then(response => response.json())
        .then(data => {
            updateChart(data);
            $("#BackGround").css("display", "none");
        })
        .catch(err => {
            $("#BackGround").css("display", "none");
            M.toast({ html: 'Lỗi tải dữ liệu biểu đồ', classes: 'red' });
        });
}

function updateChart(data) {
    let monthStr = $("#chartMonth").val();
    let selectedDate = new Date(monthStr + "-01");
    let year = selectedDate.getFullYear();
    let month = selectedDate.getMonth();
    let daysInMonth = new Date(year, month + 1, 0).getDate();

    // Map data từ server
    let dataMap = {};
    if (data && data.DataGridView1 && data.DataGridView1.length > 0) {
        data.DataGridView1.forEach(row => {
            if (row.DATE) {
                let dateObj = new Date(row.DATE);
                let day = dateObj.getDate();
                dataMap[day] = row.TOTAL_QTY || 0;
            }
        });
    }

    // Tạo labels và data
    let labels = [];
    let quantities = [];
    let today = new Date();
    let isCurrentMonth = (year === today.getFullYear() && month === today.getMonth());
    let currentDay = today.getDate();

    for (let day = 1; day <= daysInMonth; day++) {
        let dayStr = String(day).padStart(2, '0');
        let monthNum = String(month + 1).padStart(2, '0');
        labels.push(`${dayStr}/${monthNum}`);

        if (isCurrentMonth && day > currentDay) {
            quantities.push(null);
        } else {
            quantities.push(dataMap[day] || 0);
        }
    }

    productionChart.data.labels = labels;
    productionChart.data.datasets[0].data = quantities;
    productionChart.update();
}
function getMachineList(filterType) {
    if (filterType === "ALL") {
        return "A801,A802,A803,A804,A805,A806,A807,A808,A809,A810,A811,A812,A813,A814,A815,A816,A501,A502,A830,A831,A832,A833";
    } else if (filterType === "CUTTING") {
        return "A801,A802,A803,A804,A805,A806,A807,A808,A809,A810,A811,A812,A813,A814,A815,A816,A501,A502";
    } else if (filterType === "FA") {
        return "A830,A831,A832,A833";
    }
    return "A801,A802,A803,A804,A805,A806,A807,A808,A809,A810,A811,A812,A813,A814,A815,A816,A501,A502,A830,A831,A832,A833";
}

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
    $("#fromDate").attr("max", todayStr);
    $("#toDate").attr("max", todayStr);
}

function displayFAMachinesInRow1() {
    $(".machine-cell.clone").remove();

    let a830Clone = $("#LY_A830").clone(true);
    let a831Clone = $("#LY_A831").clone(true);
    let a832Clone = $("#LY_A832").clone(true);
    let a833Clone = $("#LY_A833").clone(true);

    a830Clone.attr("id", "LY_A830_clone").addClass("clone fa-machine");
    a831Clone.attr("id", "LY_A831_clone").addClass("clone fa-machine");
    a832Clone.attr("id", "LY_A832_clone").addClass("clone fa-machine");
    a833Clone.attr("id", "LY_A833_clone").addClass("clone fa-machine");

    $("#LY01, #LY02, #LY03, #LY04, #LY05, #LY06").hide();

    a830Clone.insertAfter($("#grid-row1 .note-cell"));
    a831Clone.insertAfter(a830Clone);
    a832Clone.insertAfter(a831Clone);
    a833Clone.insertAfter(a832Clone);

    $("#grid-row2, #grid-row3, #grid-row4").hide();

    setupFACloneContextMenus();
}

function setupFACloneContextMenus() {
    const faMachines = [
        { cloneId: "LY_A830_clone", nameId: "MC_NM_A830" },
        { cloneId: "LY_A831_clone", nameId: "MC_NM_A831" },
        { cloneId: "LY_A832_clone", nameId: "MC_NM_A832" },
        { cloneId: "LY_A833_clone", nameId: "MC_NM_A833" }
    ];

    faMachines.forEach(machine => {
        $(`#${machine.cloneId}`).contextmenu(function (e) {
            e.preventDefault();
            let mcName = $(`#${machine.nameId}`).text();
            let currentColor = $(this).css("background-color");

            if (currentColor === "rgb(255, 165, 0)" || currentColor === "orange") {
                MC_STOP_RUN(mcName);
            } else if (currentColor === "rgb(0, 128, 0)" || currentColor === "green") {
                MC_STOPN(mcName);
            }
        });
    });
}

function resetMachineDisplay() {
    $(".machine-cell.clone").remove();
    $(".production-grid").show();
    $(".machine-cell").show();
}

function filterMachines(filterType) {
    resetMachineDisplay();

    if (filterType === "ALL") {
        return;
    }

    if (filterType === "CUTTING") {
        $("#LY_A830, #LY_A831, #LY_A832, #LY_A833").hide();
        $("#grid-row4").hide();
    }
    else if (filterType === "FA") {
        displayFAMachinesInRow1();
    }
}

function RELOAD(isAuto = false) {
    if (isAuto) {
        const today = new Date().toISOString().split("T")[0];
        $("#fromDate").val(today);
        $("#toDate").val(today);
    }

    if (!isAuto) {
        $("#BackGround").css("display", "block");
    }

    let filterType = $("#machineTypeFilter").val() || "ALL";
    let startDate = $("#fromDate").val();
    let endDate = $("#toDate").val();
    let machineList = getMachineList(filterType);

    let BaseParameter = {
        USER_IDX: GetCookieValue("USER_IDX"),
        START_DATE: startDate,
        END_DATE: endDate,
        MACHINE_LIST: machineList
    };

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/H01/RELOAD";

    fetch(url, {
        method: "POST",
        body: formUpload
    }).then((response) => {
        response.json().then((data) => {
            BaseResult = data;
            updateMachineDisplay();
            filterMachines(filterType);
            if (!isAuto) {
                loadMonthlyChart();
            }
            if (!isAuto) $("#BackGround").css("display", "none");
        }).catch((err) => {
            if (!isAuto) $("#BackGround").css("display", "none");
        });
    });
}

function updateMachineDisplay() {
    if (!BaseResult) return;

    if (BaseResult.DataGridView1 && BaseResult.DataGridView1.length > 0) {
        let lastIdx = BaseResult.DataGridView1.length - 1;
        $("#Label2").text(formatNumber(BaseResult.DataGridView1[lastIdx].SUM));
        $("#Label3").text(formatPercent(BaseResult.DataGridView1[lastIdx].RUN_RAT));
        $("#Label4").text(formatNumber(BaseResult.DataGridView1[lastIdx].UPH));

        for (let i = 0; i < lastIdx; i++) {
            let machine = BaseResult.DataGridView1[i];
            let mcName = machine.MC_NO;

            if (mcName) {
                updateMachineData(mcName, machine);
            }
        }
    }

    let today = new Date();
    today.setHours(0, 0, 0, 0);

    let selectedDate = new Date($("#toDate").val());
    selectedDate.setHours(0, 0, 0, 0);

    if (selectedDate < today) {
        setAllMachinesToGreen();
    } else {
        initializeMachineStatus();

        if (BaseResult.DataGridView2 && BaseResult.DataGridView2.length > 0) {
            for (let i = 0; i < BaseResult.DataGridView2.length; i++) {
                let machine = BaseResult.DataGridView2[i];
                let mcName = machine.tsnon_oper_mitor_MCNM;
                let status = machine.tsnon_oper_mitor_NOIC;
                let runYN = machine.tsnon_oper_mitor_RUNYN;

                updateMachineStatus(mcName, status, runYN);
            }
        }
    }
}

function setAllMachinesToGreen() {
    let machines = [
        "01", "02", "03", "04", "05", "06", "07", "08", "09", "10",
        "11", "12", "13", "14", "15", "16", "_A501", "_A502", "_A830", "_A831", "_A832", "_A833"
    ];

    machines.forEach(id => {
        $(`#LY${id}`).css("background-color", "Lime");
        $(`#STOP${id}`).text("--");

        if ($(`#LY${id}_clone`).length) {
            $(`#LY${id}_clone`).css("background-color", "Lime");
        }
    });
}

function updateMachineData(mcName, machine) {
    let numericId = mcName.substring(1);  
    let prefix = mcName.substring(0, 1);  
    let id = "";

    if (prefix === "A" && numericId >= 801 && numericId <= 816) { 
        id = numericId.substring(1); 
    } else if (prefix === "A" && (numericId === "501" || numericId === "502")) {
        id = `_${numericId}`;
    } else if (mcName === "A830" || mcName === "A831" || mcName === "A832" || mcName === "A833") {
        id = `_${mcName}`;
    }

    if (id) {
        $(`#${mcName}1`).text(formatNumber(machine.SUM));

        let ratData = parseFloat(machine.RUN_RAT);
        if (ratData >= 100) ratData = 100;
        $(`#${mcName}2`).text(formatPercent(ratData));

        $(`#${mcName}3`).text(formatNumber(machine.UPH));
    }
}

function initializeMachineStatus() {
    let machines = [
        "01", "02", "03", "04", "05", "06", "07", "08", "09", "10",
        "11", "12", "13", "14", "15", "16", "_A501", "_A502", "_A830", "_A831", "_A832", "_A833"
    ];

    machines.forEach(id => {
        $(`#LY${id}`).css("background-color", "Lime");
        $(`#STOP${id}`).text("--");

        let mcId;
        if (id === "_A501") mcId = "A501";
        else if (id === "_A502") mcId = "A502";
        else if (id === "_A830") mcId = "A830";
        else if (id === "_A831") mcId = "A831";
        else if (id === "_A832") mcId = "A832";
        else if (id === "_A833") mcId = "A833";
        else mcId = "A8" + id;

        let production = parseInt($(`#${mcId}1`).text().replace(/,/g, ''));
        if (production === 0 || isNaN(production)) {
            $(`#LY${id}`).css("background-color", "Silver");
        }
    });
}

function updateMachineStatus(mcName, status, runYN) {
    let id = "";

    if (mcName === "A501") {
        id = "_A501";
    } else if (mcName === "A502") {
        id = "_A502";
    } else if (mcName === "A830") {
        id = "_A830";
    } else if (mcName === "A831") {
        id = "_A831";
    } else if (mcName === "A832") {
        id = "_A832";
    } else if (mcName === "A833") {
        id = "_A833";
    } else if (mcName.startsWith("A8")) {
        id = mcName.substring(2);
    }

    if (id) {
        $(`#STOP${id}`).text(status || "--");

        if (status) {
            if (runYN === "W") {
                $(`#LY${id}`).css("background-color", "Orange");
                $(`#LY${id}_clone`).css("background-color", "Orange");
            } else {
                $(`#LY${id}`).css("background-color", "Red");
                $(`#LY${id}_clone`).css("background-color", "Red");
            }
        }
    }
}

function MC_STOPN(mcName) {
    $("#BackGround").css("display", "block");
    let BaseParameter = {
        MC_NAME: mcName,
        USER_NM: GetCookieValue("USER_NM") || "MES"
    };

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/H01/MC_STOPN";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {}
    }).then((response) => {
        response.json().then((data) => {
            RELOAD();
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        });
    });
}

function MC_STOP_RUN(mcName) {
    $("#BackGround").css("display", "block");
    let BaseParameter = {
        MC_NAME: mcName,
        USER_NM: GetCookieValue("USER_NM") || "MES"
    };

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/H01/MC_STOP_RUN";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {}
    }).then((response) => {
        response.json().then((data) => {
            RELOAD();
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        });
    });
}

function Dgv_Reload_ShowDialog() {
    $("#BackGround").css("display", "block");

    let fromDate = $("#fromDate").val();
    let toDate = $("#toDate").val();
    let filterType = $("#machineTypeFilter").val() || "ALL";
    let machineList = getMachineList(filterType);

    let BaseParameter = {
        USER_IDX: GetCookieValue("USER_IDX"),
        START_DATE: fromDate,
        END_DATE: toDate,
        MACHINE_LIST: machineList
    };

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/H01/RELOAD";

    fetch(url, {
        method: "POST",
        body: formUpload
    })
        .then(response => response.json())
        .then(data => {
            showDataGridDialog(data, fromDate, toDate);
            $("#BackGround").css("display", "none");
        })
        .catch(err => {
            $("#BackGround").css("display", "none");
            M.toast({ html: 'Lỗi tải dữ liệu', classes: 'red' });
        });
}

function showDataGridDialog(data, fromDate, toDate) {
    $('#DataGridView1').empty();

    if (fromDate && toDate) {
        let dateRangeTitle = `<div class="date-range-info" style="font-weight: bold; margin-bottom: 10px;">
            Dữ liệu từ ngày ${fromDate} đến ngày ${toDate}
        </div>`;

        $('.date-range-info').remove();
        $('.modal-header-info').prepend(dateRangeTitle);
    }

    if (data && data.DataGridView1) {
        data.DataGridView1.forEach((row, index) => {
            let html = `<tr>
                <td>${index + 1}</td>
                <td>${row.DATE || ''}</td>
                <td>${row.MC_NO || ''}</td>
                <td>${row.TS_TIME_ST || ''}</td>
                <td>${row.TS_TIME_END || ''}</td>
                <td>${formatNumber(row.SUM)}</td>
                <td>${row.STOP_TIME?.toFixed(3) || '0.000'}</td>
                <td>${row.WORK_TIME?.toFixed(3) || '0.000'}</td>
                <td>${formatPercent(row.RUN_RAT)}</td>
                <td>${formatNumber(row.UPH)}</td>
            </tr>`;
            $('#DataGridView1').append(html);
        });
    }

    $('#H00_LISTModal').modal('open');
}

let timerInterval;

function Timer1_Start() {
    Timer1_Stop();
    timerInterval = setInterval(function () {
        RELOAD(true);
    }, 30000);
}

function Timer1_Stop() {
    clearInterval(timerInterval);
}

function formatNumber(value) {
    if (!value) return "0";
    return new Intl.NumberFormat().format(value);
}

function formatPercent(value) {
    if (!value) return "0%";
    return new Intl.NumberFormat(undefined, {
        style: 'percent',
        minimumFractionDigits: 0,
        maximumFractionDigits: 0
    }).format(value);
}

function setupMachineMenus() {
    const machines = [];

   
    for (let i = 1; i <= 16; i++) { 
        let machineId = (i < 10) ? "0" + i : i.toString();
        machines.push({ lyId: `LY${machineId}`, nameId: `MC_NM${machineId}` });
    }
    const specialMachines = [
        { lyId: "LY_A501", nameId: "MC_NM_A501" },
        { lyId: "LY_A502", nameId: "MC_NM_A502" },
        { lyId: "LY_A830", nameId: "MC_NM_A830" },
        { lyId: "LY_A831", nameId: "MC_NM_A831" },
        { lyId: "LY_A832", nameId: "MC_NM_A832" },
        { lyId: "LY_A833", nameId: "MC_NM_A833" }
    ];

    machines.push(...specialMachines);

    machines.forEach(machine => {
        $(`#${machine.lyId}`).contextmenu(function (e) {
            e.preventDefault();
            let mcName = $(`#${machine.nameId}`).text();
            let currentColor = $(this).css("background-color");

            if (currentColor === "rgb(255, 165, 0)" || currentColor === "orange") {
                MC_STOP_RUN(mcName);
            } else if (currentColor === "rgb(0, 128, 0)" || currentColor === "green") {
                MC_STOPN(mcName);
            }
        });
    });
}