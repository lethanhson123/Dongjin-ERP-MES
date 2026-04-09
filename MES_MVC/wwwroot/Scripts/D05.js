let BaseResult;

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
    let dateSelector = $("#tab1_DateTime").val();
    let tab1_PartNo = $("#tab1_PartNo").val();
    let tab1_PartName = $("#tab1_PartName").val();
    BaseParameter = {
        DateTimePicker1: dateSelector,
        PartNo: tab1_PartNo,
        PartName: tab1_PartName
    }

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/D05/Buttonfind_Click";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            if (data.ListSuperResultTranfer.length > 0) {
                const tbody = document.getElementById("DataGridView1");
                tbody.innerHTML = "";
                const fragment = document.createDocumentFragment();

                for (let i = 0; i < data.ListSuperResultTranfer.length; i++) {
                    const rowData = data.ListSuperResultTranfer[i];
                    const tr = document.createElement("tr");   

                    tr.innerHTML = `                           
                                <td>${i+1}</td>
                                <td>${rowData.PART_NO || ""}</td>
                                <td>${rowData.PART_NAME || ""}</td>
                                <td>${rowData.DVC || ""}</td>
                                <td>${rowData.W1 || ""}</td>
                                <td>${rowData.W2 || ""}</td>                            
                                <td>${rowData.D00 || ""}</td>
                                <td>${rowData.D01 || ""}</td>
                                <td>${rowData.D02 || ""}</td>
                                <td>${rowData.D03 || ""}</td>
                                <td>${rowData.D04 || ""}</td>
                                <td>${rowData.D05 || ""}</td>
                                <td>${rowData.D06 || ""}</td>
                                <td>${rowData.D07 || ""}</td>
                                <td>${rowData.D08 || ""}</td>
                                <td>${rowData.D09 || ""}</td>
                                <td>${rowData.D10 || ""}</td>
                                <td>${rowData.D11 || ""}</td>
                                <td>${rowData.D12 || ""}</td>
                                <td>${rowData.D13 || ""}</td>                                
                            `;

                    fragment.appendChild(tr);
                }

                tbody.appendChild(fragment);
                ShowMessage("Successfuly", "ok");
            }
            else {
                ShowMessage("No data", "error");
            }

            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
    DGV2_DT_CHG();
}




function Buttonadd_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/D05/Buttonadd_Click";

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
    let url = "/D05/Buttonsave_Click";

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
    let url = "/D05/Buttondelete_Click";

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
    let url = "/D05/Buttoncancel_Click";

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
    let url = "/D05/Buttoninport_Click";

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
    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/D05/Buttonexport_Click";

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
function Buttonprint_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/D05/Buttonprint_Click";

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

function DGV2_DT_CHG() {
    const dateInput = document.getElementById("tab1_DateTime");
    if (!dateInput || !dateInput.value) return;

    updateWeekNumber();

    const baseDate = new Date(dateInput.value);
    const wCount = 1 - baseDate.getDay(); // tương tự VB: tính ngày đầu tuần (Chủ Nhật)

    const formatDate = (date) => {
        const year = date.getFullYear();
        const month = String(date.getMonth() + 1).padStart(2, '0');
        const day = String(date.getDate()).padStart(2, '0');
        return `${year}-${month}-${day}`;
    };

    for (let i = 1; i <= 14; i++) {
        const d = new Date(baseDate);
        d.setDate(baseDate.getDate() + wCount + i - 1); // tính D+1 đến D+14
        const formatted = formatDate(d);

        const th = document.querySelector(`#Table1 th[data-col="DD_${i}"]`);
        if (th) {
            th.textContent = formatted;
        }
    }
}


function getISOWeekNumber(date) {
    const tempDate = new Date(date);
    tempDate.setHours(0, 0, 0, 0);

    // Đưa ngày về thứ Năm của tuần ISO
    tempDate.setDate(tempDate.getDate() + 3 - ((tempDate.getDay() + 6) % 7));

    const firstThursday = new Date(tempDate.getFullYear(), 0, 4);
    firstThursday.setDate(firstThursday.getDate() + 3 - ((firstThursday.getDay() + 6) % 7));

    const weekNumber = 1 + Math.round((tempDate - firstThursday) / (7 * 24 * 60 * 60 * 1000));
    return weekNumber;
}

function updateWeekNumber() {
    const dateInput = document.getElementById("tab1_DateTime");
    if (!dateInput || !dateInput.value) return;

    const selectedDate = new Date(dateInput.value);
    const weekNumber = getISOWeekNumber(selectedDate);

    $("#tab1_WeekNumber").val(weekNumber); // Gán số tuần vào input readonly
}






