/* ==================== MODERN PRODUCTION DASHBOARD JS ==================== */

let lineCharts = {};
let isLiveMode = true;
let refreshInterval = null;
let selectedDate = null;
let availableLines = [];
let currentViewBy = 'hour';
let currentFullscreen = null;

// ==================== PRODUCTIVITY PLAN MAPPING ====================
const PRODUCTIVITY_PLAN = {
    'PKI-9BUX-BAT': 100,
    'PKI-9B ENG-AWD': 86,
    'PKI-9B ENG-FWD': 95,
    'PKI-LIH-LIH': 100,
    'JEM-U100-U100': 95,
    'DH-DH-DH': 98,
    'PKI-HDI-HDI': 85,
    'PKI-HDI-BAT': 100
};

// Helper function to get plan productivity for a line
function getPlanProductivity(lineName) {
    // Try exact match first
    if (PRODUCTIVITY_PLAN[lineName]) {
        return PRODUCTIVITY_PLAN[lineName];
    }

    // Try normalized (remove extra spaces, case-insensitive)
    const normalized = lineName.trim().replace(/\s+/g, ' ');
    for (const [key, value] of Object.entries(PRODUCTIVITY_PLAN)) {
        if (key.replace(/\s+/g, ' ').toLowerCase() === normalized.toLowerCase()) {
            return value;
        }
    }

    // Default to 100% if not found
    return 100;
}

// ==================== INITIALIZATION ====================
document.addEventListener('DOMContentLoaded', async () => {
    initializeDatePicker();
    await loadAvailableLines();
    await refreshData();
    if (isLiveMode) startAutoRefresh();
    initializeFullscreenListeners();
    updateClock();
});

window.addEventListener('beforeunload', stopAutoRefresh);

// ==================== CLOCK ====================
function updateClock() {
    setInterval(() => {
        document.getElementById('currentTime').textContent = new Date().toLocaleTimeString('en-US', {
            hour12: false,
            hour: '2-digit',
            minute: '2-digit',
            second: '2-digit'
        });
    }, 1000);
}

// ==================== DATE PICKER ====================
function initializeDatePicker() {
    const dateInput = document.getElementById('selectedDate');
    const today = new Date();

    dateInput.value = today.toISOString().split('T')[0];
    selectedDate = today;
    updateDateLabel();

    dateInput.addEventListener('change', function () {
        selectedDate = new Date(this.value);
        updateDateLabel();
        if (isLiveMode) toggleLiveMode();
        refreshData();
    });

    // Fix date picker click
    document.querySelector('.date-picker-wrapper').addEventListener('click', function () {
        dateInput.showPicker();
    });
}

function updateDateLabel() {
    const today = new Date();
    const base = selectedDate || today;
    const isToday = base.toDateString() === today.toDateString();

    let labelText = '';

    switch (currentViewBy) {
        case 'hour':
            labelText = isToday ? 'Today' : base.toLocaleDateString('en-US', { month: 'short', day: 'numeric', year: 'numeric' });
            break;
        case 'day':
            labelText = base.toLocaleDateString('en-US', { month: 'long', year: 'numeric' });
            break;
        case 'week':
            labelText = 'Last 7 Weeks';
            break;
        case 'month':
            labelText = base.getFullYear().toString();
            break;
        case 'year':
            const fromYear = base.getFullYear() - 5;
            const toYear = base.getFullYear();
            labelText = `${fromYear} - ${toYear}`;
            break;
    }

    document.getElementById('dateLabel').textContent = labelText;
}

// ==================== VIEW CHANGE ====================
function onViewByChange() {
    const selector = document.getElementById('viewBySelector');
    currentViewBy = selector.value;
    updateDateLabel();
    refreshData();
}

// ==================== LIVE MODE ====================
function toggleLiveMode() {
    const btn = document.getElementById('liveModeBtn');
    isLiveMode = !isLiveMode;

    if (isLiveMode) {
        btn.className = 'live-btn active';
        btn.innerHTML = '<span class="live-indicator">●</span> Live';

        const today = new Date();
        document.getElementById('selectedDate').value = today.toISOString().split('T')[0];
        selectedDate = today;
        updateDateLabel();
        startAutoRefresh();
    } else {
        btn.className = 'live-btn inactive';
        btn.innerHTML = '<span class="live-indicator">●</span> Manual';
        stopAutoRefresh();
    }
    refreshData();
}

function startAutoRefresh() {
    if (refreshInterval) clearInterval(refreshInterval);
    refreshInterval = setInterval(() => isLiveMode && refreshData(), 30000);
}

function stopAutoRefresh() {
    if (refreshInterval) {
        clearInterval(refreshInterval);
        refreshInterval = null;
    }
}

// ==================== DATE RANGE ====================
function getDateRange() {
    const base = isLiveMode ? new Date() : (selectedDate || new Date());

    switch (currentViewBy) {
        case 'hour':
            return {
                fromDate: new Date(base.getFullYear(), base.getMonth(), base.getDate()),
                toDate: isLiveMode ? base : new Date(base.getFullYear(), base.getMonth(), base.getDate(), 23, 59, 59)
            };
        case 'day':
            return {
                fromDate: new Date(base.getFullYear(), base.getMonth(), 1),
                toDate: new Date(base.getFullYear(), base.getMonth() + 1, 0, 23, 59, 59)
            };
        case 'week':
            return {
                fromDate: base,
                toDate: base
            };
        case 'month':
            return {
                fromDate: new Date(base.getFullYear(), 0, 1),
                toDate: new Date(base.getFullYear(), 11, 31, 23, 59, 59)
            };
        case 'year':
            return {
                fromDate: new Date(base.getFullYear() - 5, 0, 1),
                toDate: new Date(base.getFullYear(), 11, 31, 23, 59, 59)
            };
    }
}

function formatDateLocal(date) {
    return `${date.getFullYear()}-${String(date.getMonth() + 1).padStart(2, '0')}-${String(date.getDate()).padStart(2, '0')}T${String(date.getHours()).padStart(2, '0')}:${String(date.getMinutes()).padStart(2, '0')}:${String(date.getSeconds()).padStart(2, '0')}`;
}

// ==================== API CALL ====================
async function apiCall(endpoint, fromDate, toDate, lineNumber = null, viewType = null) {
    const formData = new FormData();
    const params = {
        FromDate: formatDateLocal(fromDate),
        ToDate: formatDateLocal(toDate)
    };

    if (lineNumber !== null) {
        params.ComboBox1 = lineNumber;
    }
    if (viewType !== null) {
        params.ComboBox2 = viewType;
    }

    formData.append('BaseParameter', JSON.stringify(params));

    const response = await fetch(`/H15/${endpoint}`, {
        method: 'POST',
        body: formData
    });
    return response.json();
}

// ==================== LOAD AVAILABLE LINES ====================
async function loadAvailableLines() {
    try {
        const { fromDate, toDate } = getDateRange();
        const result = await apiCall('GetDailyLineProduction', fromDate, toDate, 'all');

        if (result.Data && result.Data.Lines) {
            availableLines = result.Data.LineNumbers.map((lineNum, index) => ({
                value: lineNum,
                text: result.Data.Lines[index]
            }));

            // ✅ ĐƠN GIẢN: Ưu tiên 3 line lên đầu
            availableLines.sort((a, b) => {
                const priority = ['AWD', 'FWD', '9BUX'];
                const aP = priority.findIndex(p => a.text.includes(p));
                const bP = priority.findIndex(p => b.text.includes(p));

                if (aP !== -1 && bP !== -1) return aP - bP;
                if (aP !== -1) return -1;
                if (bP !== -1) return 1;
                return 0;
            });
        }
    } catch (error) {
        console.error('Error loading lines:', error);
        showError('Failed to load production lines');
    }
}

// ==================== RENDER LINE CARDS ====================
function renderLineCards() {
    const grid = document.getElementById('linesGrid');
    if (availableLines.length === 0) {
        grid.innerHTML = '<div class="loading">⚠️ No production lines available</div>';
        return;
    }

    grid.innerHTML = '';

    availableLines.forEach(line => {
        const planProductivity = getPlanProductivity(line.text);

        const card = document.createElement('div');
        card.className = 'line-card';
        card.id = `line-card-${line.value}`;
        card.style.cursor = 'pointer';
        card.innerHTML = `
            <div class="stats-section">
                <div class="line-name">${line.text}</div>
                
                <div class="stats-grid">
                    <div class="stat-box">
                        <div class="stat-header">Quantity</div>
                        <div class="stat-row">
                            <span class="stat-label">Plan</span>
                            <span class="stat-value plan" id="plan-qty-${line.value}">--</span>
                        </div>
                        <div class="stat-row">
                            <span class="stat-label">Actual</span>
                            <span class="stat-value actual" id="actual-qty-${line.value}">--</span>
                        </div>
                    </div>
                    
                    <div class="stat-box">
                        <div class="stat-header">UPH</div>
                        <div class="stat-row">
                            <span class="stat-label">Plan</span>
                            <span class="stat-value plan" id="plan-uph-${line.value}">--</span>
                        </div>
                        <div class="stat-row">
                            <span class="stat-label">Actual</span>
                            <span class="stat-value actual" id="actual-uph-${line.value}">--</span>
                        </div>
                    </div>
                    
                    <div class="stat-box">
                        <div class="stat-header">Productivity</div>
                        <div class="stat-row">
                            <span class="stat-label">Plan</span>
                            <span class="stat-value plan" id="plan-prod-${line.value}">${planProductivity}%</span>
                        </div>
                        <div class="stat-row">
                            <span class="stat-label">Actual</span>
                            <span class="stat-value actual" id="actual-prod-${line.value}">--</span>
                        </div>
                    </div>
                    
                    <div class="stat-box">
                        <div class="stat-header">Staff & Time</div>
                        <div class="stat-row">
                            <span class="stat-label">👥 Workers</span>
                            <span class="stat-value actual" id="staff-count-${line.value}">--</span>
                        </div>
                        <div class="stat-row">
                            <span class="stat-label">⏱️ Time</span>
                            <span class="stat-value actual" id="working-time-${line.value}">--</span>
                        </div>
                    </div>
                </div>
            </div>
            
            <div class="chart-section">
                <canvas id="chart-${line.value}"></canvas>
            </div>
        `;

        card.addEventListener('click', () => openFullscreen(line.value));
        grid.appendChild(card);
    });
}

// ==================== REFRESH DATA ====================
async function refreshData() {
    showLoadingState();
    renderLineCards();

    for (const line of availableLines) {
        await loadLineData(line.value);
    }
}

function showLoadingState() {
    availableLines.forEach(line => {
        const elements = [
            `actual-qty-${line.value}`,
            `actual-uph-${line.value}`,
            `actual-prod-${line.value}`,
            `staff-count-${line.value}`,
            `working-time-${line.value}`
        ];

        elements.forEach(id => {
            const el = document.getElementById(id);
            if (el) el.textContent = '...';
        });
    });
}

// ==================== LOAD LINE DATA ====================
async function loadLineData(lineNumber) {
    try {
        const { fromDate, toDate } = getDateRange();

        // Get Productivity Metrics
        const metricsResult = await apiCall('GetLineProductivityMetrics', fromDate, toDate, lineNumber);

        // Get chart data
        const chartEndpoints = {
            'hour': 'GetHourlyProduction',
            'day': 'GetDailyProduction',
            'week': 'GetWeeklyProduction',
            'month': 'GetMonthlyProduction',
            'year': 'GetYearlyProduction'
        };

        const chartResult = await apiCall(chartEndpoints[currentViewBy], fromDate, toDate, lineNumber);

        // Update UI
        if (metricsResult.Success && metricsResult.Data) {
            updateLineStats(lineNumber, metricsResult.Data);
        }

        if (chartResult.Data) {
            renderLineChart(lineNumber, chartResult.Data);
        }
    } catch (error) {
        console.error(`Error loading data for line ${lineNumber}:`, error);
    }
}

// ==================== UPDATE LINE STATS ====================
function updateLineStats(lineNumber, metricsData) {
    const actualQty = metricsData.Actual || 0;
    const planQty = metricsData.PlanQty || 0;
    const totalWorkers = metricsData.TotalWorkers || 0;
    const workingTimeMinutes = metricsData.TotalWorkingTimeMinutes || 0;
    const workingTimeHours = workingTimeMinutes > 0 ? (workingTimeMinutes / 60) : 0;
    const productivity = metricsData.Productivity || 0;

    const lineInfo = availableLines.find(l => l.value === lineNumber);
    const lineName = lineInfo ? lineInfo.text : '';
    const planProductivity = getPlanProductivity(lineName);

    // ✅ ACTUAL UPH (theo công thức của bạn)
    const avgHoursPerWorker = totalWorkers > 0 ? workingTimeHours / totalWorkers : 0;
    const actualUph = avgHoursPerWorker > 0 ? Math.round(actualQty / avgHoursPerWorker) : 0;

    // ✅ PLAN UPH (tiêu chuẩn 8h)
    const plannedHours = 8;
    const planUph = planQty > 0 ? Math.round(planQty / plannedHours) : 0;

    // Update DOM
    const updates = {
        [`actual-qty-${lineNumber}`]: actualQty.toLocaleString(),    
        [`plan-qty-${lineNumber}`]: planQty.toLocaleString(),        
        [`plan-uph-${lineNumber}`]: planUph,                          
        [`actual-uph-${lineNumber}`]: actualUph,                      
        [`staff-count-${lineNumber}`]: totalWorkers,                 
        [`working-time-${lineNumber}`]: workingTimeHours.toFixed(1) + 'h' 
    };

    Object.entries(updates).forEach(([id, value]) => {
        const el = document.getElementById(id);
        if (el) el.textContent = value;
    });

    // Update Plan Productivity
    const planProdEl = document.getElementById(`plan-prod-${lineNumber}`); 
    if (planProdEl) {
        planProdEl.textContent = planProductivity + '%';
    }

    // Update Actual Productivity with color coding
    const prodEl = document.getElementById(`actual-prod-${lineNumber}`); 
    if (prodEl) {
        prodEl.textContent = productivity.toFixed(1) + '%';
        prodEl.className = 'stat-value actual';

        const performanceRatio = (productivity / planProductivity) * 100;

        if (performanceRatio >= 90) {
            prodEl.classList.add('productivity-good');
        } else if (performanceRatio >= 70) {
            prodEl.classList.add('productivity-warning');
        } else {
            prodEl.classList.add('productivity-bad');
        }
    }
}

// ==================== RENDER LINE CHART ====================
function renderLineChart(lineNumber, data) {
    const ctx = document.getElementById(`chart-${lineNumber}`);
    if (!ctx) return;

    if (lineCharts[lineNumber]) {
        lineCharts[lineNumber].destroy();
    }

    const labels = data.Labels || [];
    const values = data.Values || [];

    const canvas = ctx.getContext('2d');
    const gradient = canvas.createLinearGradient(0, 0, 0, 300);
    gradient.addColorStop(0, 'rgba(102, 126, 234, 0.8)');
    gradient.addColorStop(1, 'rgba(118, 75, 162, 0.2)');

    lineCharts[lineNumber] = new Chart(canvas, {
        type: 'bar',
        data: {
            labels: labels,
            datasets: [{
                label: 'Production',
                data: values,
                backgroundColor: gradient,
                borderColor: '#667eea',
                borderWidth: 2,
                borderRadius: 8,
                borderSkipped: false
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            plugins: {
                legend: {
                    display: false
                },
                tooltip: {
                    backgroundColor: 'rgba(15, 23, 42, 0.95)',
                    titleColor: '#fff',
                    bodyColor: '#cbd5e1',
                    borderColor: 'rgba(102, 126, 234, 0.5)',
                    borderWidth: 1,
                    padding: 12,
                    displayColors: false,
                    titleFont: {
                        size: 16,        // ✅ Tăng từ 14 → 16
                        weight: 'bold'
                    },
                    bodyFont: {
                        size: 15         // ✅ Tăng từ 13 → 15
                    },
                    callbacks: {
                        title: function (context) {
                            return `📅 ${context[0].label}`;
                        },
                        label: function (context) {
                            return `📦 Production: ${context.parsed.y.toLocaleString()} units`;
                        }
                    }
                },
                datalabels: {
                    display: function (context) {
                        return values.length <= 31;
                    },
                    color: '#fff',
                    anchor: 'end',
                    align: 'top',
                    offset: 2,
                    font: {
                        size: 12,        // ✅ Tăng từ 10 → 12
                        weight: '700'
                    },
                    formatter: function (value) {
                        return value > 0 ? value.toLocaleString() : '';
                    }
                }
            },
            scales: {
                y: {
                    beginAtZero: true,
                    grid: {
                        color: 'rgba(255, 255, 255, 0.05)',
                        drawBorder: false
                    },
                    ticks: {
                        color: '#94a3b8',
                        font: {
                            size: 13,    // ✅ Tăng từ 11 → 13
                            weight: '500'
                        },
                        padding: 8,
                        callback: function (value) {
                            return value.toLocaleString();
                        }
                    }
                },
                x: {
                    grid: {
                        display: false,
                        drawBorder: false
                    },
                    ticks: {
                        color: '#94a3b8',
                        font: {
                            size: 12,    // ✅ Tăng từ 10 → 12
                            weight: '500'
                        },
                        padding: 8,
                        maxRotation: 45,
                        minRotation: 0
                    }
                }
            }
        },
        plugins: [ChartDataLabels]
    });
}

// ==================== FULLSCREEN ====================
function initializeFullscreenListeners() {
    document.getElementById('fullscreenBackdrop').addEventListener('click', closeFullscreen);
    document.getElementById('closeFullscreen').addEventListener('click', closeFullscreen);
    document.addEventListener('keydown', function (e) {
        if (e.key === 'Escape') closeFullscreen();
    });
}

function openFullscreen(lineNumber) {
    const card = document.getElementById(`line-card-${lineNumber}`);
    if (!card || currentFullscreen) return;

    currentFullscreen = card;
    card.classList.add('fullscreen');

    document.getElementById('fullscreenBackdrop').classList.add('active');
    document.getElementById('closeFullscreen').classList.add('active');
    document.body.style.overflow = 'hidden';

    setTimeout(() => {
        if (lineCharts[lineNumber]) {
            lineCharts[lineNumber].resize();
        }
    }, 300);
}

function closeFullscreen() {
    if (!currentFullscreen) return;

    currentFullscreen.classList.remove('fullscreen');
    document.getElementById('fullscreenBackdrop').classList.remove('active');
    document.getElementById('closeFullscreen').classList.remove('active');
    document.body.style.overflow = '';

    currentFullscreen = null;
}

// ==================== ERROR HANDLING ====================
function showError(message) {
    const grid = document.getElementById('linesGrid');
    grid.innerHTML = `<div class="loading" style="color: var(--accent-red);">⚠️ ${message}</div>`;
}