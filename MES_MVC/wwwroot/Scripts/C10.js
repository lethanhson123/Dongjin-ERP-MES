let BaseResult;

$(document).ready(function () {
    loadInitialData();

    setInterval(function () {
        Buttonfind_Click();
    }, 30000);

    $("#DateTimePicker1").change(function () {
        Buttonfind_Click();
    });

    $("#Button2").click(function () {
        Button2_Click();
    });
});

function loadInitialData() {
    Buttonfind_Click();
}

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
    let BaseParameter = {
        ListSearchString: [$("#DateTimePicker1").val()]
    };
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C10/Buttonfind_Click";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {}
    }).then((response) => {
        response.json().then((data) => {
            BaseResult = data;
            updateMachineDisplay(data);
            updateMachineStatus(data);
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            console.error("Error processing data:", err);
            $("#BackGround").css("display", "none");
        })
    });
}

function Button2_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = {
        ListSearchString: [$("#DateTimePicker1").val()]
    };
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C10/Button2_Click";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {}
    }).then((response) => {
        response.json().then((data) => {
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            console.error("Error processing data:", err);
            $("#BackGround").css("display", "none");
        })
    });
}

function updateMachineDisplay(data) {
    resetMachineValues();

    if (!data || !data.DataGridView1 || data.DataGridView1.length === 0) {
        return;
    }

    const totalRow = data.DataGridView1[data.DataGridView1.length - 1];
    if (totalRow) {
        $("#Label2").text(formatNumber(totalRow.SUM));
        $("#Label3").text(formatPercent(totalRow.RUN_RAT));
        $("#Label4").text(formatNumber(totalRow.UPH));
    }

    data.DataGridView1.forEach(row => {
        const machineName = row.MC_NO;
        if (!machineName) return;
        if (machineName.startsWith('A8') && machineName.length === 4) {
            const machineNumber = machineName.substring(1);
            $(`#A${machineNumber}1`).text(formatNumber(row.SUM));
            let ratData = row.RUN_RAT;
            if (ratData > 1) ratData = 1;
            $(`#A${machineNumber}2`).text(formatPercent(ratData));
            $(`#A${machineNumber}3`).text(formatNumber(row.UPH));
        }
    });
}

function updateMachineStatus(data) {
    resetMachineStatus();
    markInactiveMachines();

    if (!data || !data.DataGridView2 || data.DataGridView2.length === 0) {
        return;
    }

    data.DataGridView2.forEach(row => {
        const machineName = row.tsnon_oper_mitor_MCNM;
        if (!machineName) return;
        if (machineName.startsWith('A8') && machineName.length === 4) {
            const machineNumber = machineName.substring(1);
            const machineIndex = parseInt(machineNumber) - 800;
            $(`#STOP${machineIndex.toString().padStart(2, '0')}`).text(row.tsnon_oper_mitor_NOIC);
            if (row.tsnon_oper_mitor_RUNYN === 'W') {
                $(`#LY${machineIndex.toString().padStart(2, '0')}`).css("background-color", "orange");
            } else {
                $(`#LY${machineIndex.toString().padStart(2, '0')}`).css("background-color", "red");
            }
        }
    });
}

function resetMachineValues() {
    for (let i = 1; i <= 16; i++) {
        const index = i.toString().padStart(2, '0');
        $(`#A80${index}1`).text('0');
        $(`#A80${index}2`).text('0');
        $(`#A80${index}3`).text('0');
        $(`#STOP${index}`).text('--');
    }
}

function resetMachineStatus() {
    for (let i = 1; i <= 16; i++) {
        const index = i.toString().padStart(2, '0');
        $(`#LY${index}`).css("background-color", "lime");
    }
}

function markInactiveMachines() {
    for (let i = 1; i <= 16; i++) {
        const index = i.toString().padStart(2, '0');
        if ($(`#A80${index}1`).text() === '0') {
            $(`#LY${index}`).css("background-color", "silver");
        }
    }
}

function formatNumber(value) {
    if (!value && value !== 0) return '';
    return new Intl.NumberFormat().format(parseInt(value));
}

function formatPercent(value) {
    if (!value && value !== 0) return '';
    return new Intl.NumberFormat(undefined, {
        style: 'percent',
        minimumFractionDigits: 0,
        maximumFractionDigits: 0
    }).format(parseFloat(value));
}

function Buttonadd_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = {};
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C10/Buttonadd_Click";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {}
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
    let BaseParameter = {};
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C10/Buttonsave_Click";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {}
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
    let BaseParameter = {};
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C10/Buttondelete_Click";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {}
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
    let BaseParameter = {};
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C10/Buttoncancel_Click";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {}
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
    let BaseParameter = {};
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C10/Buttoninport_Click";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {}
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
    let BaseParameter = {};
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C10/Buttonexport_Click";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {}
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
    let BaseParameter = {};
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C10/Buttonprint_Click";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {}
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

function OpenWindowByURL(url, width, height) {
    let left = (screen.width - width) / 2;
    let top = (screen.height - height) / 2;
    window.open(url, '_blank', `width=${width},height=${height},left=${left},top=${top}`);
}
function machineStopRun(machineName) {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        ListSearchString: [machineName]
    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C10/MC_STOP_RUN";

    fetch(url, {
        method: "POST",
        body: formUpload
    }).then((response) => {
        response.json().then((data) => {
           
            Buttonfind_Click();
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            showError(err.message);
            $("#BackGround").css("display", "none");
        })
    });
}
function machineStopN(machineName) {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        ListSearchString: [machineName]
    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C10/MC_STOPN";

    fetch(url, {
        method: "POST",
        body: formUpload
    }).then((response) => {
        response.json().then((data) => {
         
            Buttonfind_Click();
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            showError(err.message);
            $("#BackGround").css("display", "none");
        })
    });
}
function showError(message) {
    alert("Lỗi: " + message);
}
