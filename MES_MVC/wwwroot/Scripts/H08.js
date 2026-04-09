let BaseResult = {};
let refreshInterval = null;
let timerInterval = null;
let currentMachineStates = {};

$(document).ready(function () {
    LoadDashboard();
    refreshInterval = setInterval(LoadDashboard, 5000);
    StartGlobalTimer();

    $("#Buttonclose").click(function () {
        history.back();
    });

    $("#modalClose, #modalOverlay, #btnModalClose").click(CloseMaintenanceModal);
    $("#modalContent").click(function (e) {
        e.stopPropagation();
    });
});

function LoadDashboard() {
    fetch('/H08/Load', { method: "POST", body: new FormData() })
        .then(r => r.json())
        .then(res => {
            BaseResult = res;
            if (res.Success && res.MachineStatusList) {
                UpdateMachineStates(res.MachineStatusList);
            }
        })
        .catch(err => {
            console.error("Error:", err);
        });
}

function UpdateMachineStates(machineList) {
    let needRerender = false;

    machineList.forEach(m => {
        let oldState = currentMachineStates[m.Sub_Code];

        if (!oldState || oldState.Status !== m.Status) {
            needRerender = true;
        }

        let newTimerSeconds;
        if (!oldState || oldState.Status !== m.Status) {
            newTimerSeconds = Math.max(0, m.DowntimeSeconds || 0);
        } else {
            newTimerSeconds = oldState.TimerSeconds || 0;
        }

        currentMachineStates[m.Sub_Code] = {
            ...m,
            TimerSeconds: newTimerSeconds
        };
    });

    if (needRerender) {
        RenderDashboard();
    }
}

function RenderDashboard() {
    const groupedByLine = {};
    Object.values(currentMachineStates).forEach(m => {
        if (!groupedByLine[m.ProductionLine]) groupedByLine[m.ProductionLine] = [];
        groupedByLine[m.ProductionLine].push(m);
    });

    if (groupedByLine['CUTTING']) {
        groupedByLine['CUTTING'].sort((a, b) => {
            const isA_Special = (a.Sub_Code === 'A501' || a.Sub_Code === 'A502');
            const isB_Special = (b.Sub_Code === 'A501' || b.Sub_Code === 'A502');

            if (isA_Special && !isB_Special) return 1;
            if (!isA_Special && isB_Special) return -1;

            return a.Sub_Code.localeCompare(b.Sub_Code);
        });
    }

    $("#DashboardContainer").empty();

    ['CUTTING', 'CRIMPING', 'WELDING', 'TWIST'].forEach(line => {
        if (groupedByLine[line]) {
            const lineHtml = `<div class="dashboard-section">
                <div class="line-header">
                    <h5 class="line-title">${line}</h5>
                    ${line === 'CUTTING' ? `
                    <div class="status-legend-inline">
                        <div class="legend-box-inline status-green">
                            <span>Work</span>
                        </div>
                        <div class="legend-box-inline status-red">
                            <span>Call</span>
                        </div>
                        <div class="legend-box-inline status-yellow">
                            <span>Repair</span>
                        </div>
                    </div>
                    ` : ''}
                </div>
                <div class="machines-container" id="line-${line}"></div>
            </div>`;
            $("#DashboardContainer").append(lineHtml);

            const container = $(`#line-${line}`);
            groupedByLine[line].forEach(m => {
                const statusClass = m.Status === 0 ? 'status-green' : m.Status === 1 ? 'status-yellow' : 'status-red';

                let timerHtml = '';
                if (m.Status === 1 || m.Status === 2) {
                    const seconds = Math.max(0, m.TimerSeconds || 0);
                    timerHtml = `<span class="downtime-timer" data-subcode="${m.Sub_Code}">${FormatSeconds(seconds)}</span>`;
                }

                container.append(`<div class="machine-card ${statusClass}" data-subcode="${m.Sub_Code}" data-status="${m.Status}">
                    <span class="machine-code">${m.Sub_Code}</span>
                    ${timerHtml}
                </div>`);
            });
        }
    });

    $(".machine-card.status-yellow").click(function () {
        ShowMaintenanceDetail($(this).data("subcode"));
    });
}

function StartGlobalTimer() {
    if (timerInterval) {
        clearInterval(timerInterval);
    }

    timerInterval = setInterval(function () {
        Object.keys(currentMachineStates).forEach(subCode => {
            let machine = currentMachineStates[subCode];
            if (machine.Status === 1 || machine.Status === 2) {
                machine.TimerSeconds = (machine.TimerSeconds || 0) + 1;

                let timerElement = $(`.downtime-timer[data-subcode="${subCode}"]`);
                if (timerElement.length > 0) {
                    timerElement.text(FormatSeconds(machine.TimerSeconds));
                }
            }
        });
    }, 1000);
}

function FormatSeconds(totalSeconds) {
    if (isNaN(totalSeconds) || totalSeconds < 0) {
        totalSeconds = 0;
    }

    const hours = Math.floor(totalSeconds / 3600);
    const minutes = Math.floor((totalSeconds % 3600) / 60);
    const seconds = totalSeconds % 60;

    const pad = (n) => String(n).padStart(2, '0');

    if (hours > 0) {
        return `${pad(hours)}:${pad(minutes)}:${pad(seconds)}`;
    } else {
        return `${pad(minutes)}:${pad(seconds)}`;
    }
}

function ShowMaintenanceDetail(subCode) {
    $("#maintenanceModal").show();
    $("#modalTitle").text("🔧 ĐANG TẢI...");

    let formData = new FormData();
    formData.append('BaseParameter', JSON.stringify({ Sub_Code: subCode }));

    fetch('/H08/GetMaintenanceDetail', { method: "POST", body: formData })
        .then(r => r.json())
        .then(res => {
            if (res.Success && res.MaintenanceDetail) {
                const d = res.MaintenanceDetail;
                $("#modalTitle").text(`🔧 ĐANG BẢO TRÌ - ${d.Sub_Code}`);
                $("#mdSubCode").text(d.Sub_Code);
                $("#mdNameVN").text(d.NameVN || "");
                $("#mdProductionLine").text(d.ProductionLine);
                $("#mdMaintenedBy").text(d.MaintenedBy);
                $("#mdStartDate").text(FormatDateTime(d.StartDate));
                $("#mdDuration").text(FormatMinutes(d.MinutesWorked));
                $("#mdCurrentStatus").text(d.CurrentStatus);
                $("#mdReason").text(d.Reason);
                $("#mdSolution").text(d.Solution);
            } else {
                alert("Lỗi: " + (res.Error || "Không tìm thấy thông tin"));
                CloseMaintenanceModal();
            }
        })
        .catch(err => {
            console.error("Error:", err);
            alert("Có lỗi xảy ra: " + err.message);
            CloseMaintenanceModal();
        });
}

function CloseMaintenanceModal() {
    $("#maintenanceModal").hide();
}

function FormatDateTime(dateStr) {
    if (!dateStr) return "";
    const d = new Date(dateStr);
    if (isNaN(d.getTime())) return "";
    const pad = n => String(n).padStart(2, '0');
    return `${pad(d.getHours())}:${pad(d.getMinutes())} (${pad(d.getDate())}/${pad(d.getMonth() + 1)}/${d.getFullYear()})`;
}

function FormatMinutes(min) {
    if (!min || min <= 0) return "0 phút";
    const h = Math.floor(min / 60);
    const m = min % 60;
    return h > 0 ? `${h}h ${m}m` : `${m}m`;
}