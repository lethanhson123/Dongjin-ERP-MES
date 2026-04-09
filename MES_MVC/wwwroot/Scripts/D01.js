let BaseResult;

function ShowMessage(message, type) {

    if (type === 'error') {
        M.toast({ html: message, classes: 'red' });
    } else if (type === 'warning') {
        M.toast({ html: message, classes: 'orange' });
    }
    else {
        M.toast({ html: message, classes: 'green' });
    }
    $("#BackGround").css("display", "none");
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
    var activeTab = document.querySelector('.tabs .tab a.active');
    if (activeTab) {
        var currentTabId = activeTab.getAttribute('href').replace('#', '');

        if (currentTabId === 'Tag001') {

            SearchByDate();

        } else if (currentTabId === 'Tag002') {

            SearchByYear();

        } else if (currentTabId === 'Tag003') {


        }
    }
}

function SearchByDate() {
    let fromDate = $('#tab1_fromDate').val();
    let toDate = $('#tab1_toDate').val();
    let poCode = $('#tab1_poCode').val();
    let partNo = $('#tab1_PartNo').val();
    let model = $('#tab1_model').val();

    if (fromDate && toDate) {
        $("#BackGround").css("display", "block");

        let BaseParameter = {
            StartDate: fromDate,
            EndDate: toDate,
            PartNo: partNo,
            PartEncno: model,
            LeadNo: poCode,
            SearchString: "1"
        };

        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        let url = "/D01/Buttonfind_Click";

        fetch(url, {
            method: "POST",
            body: formUpload
        }).then(response => response.json())
            .then(data => {
                const tbody = document.getElementById("DataGridView1");
                tbody.innerHTML = ""; // Clear old rows
                const fragment = document.createDocumentFragment();

                for (let i = 0; i < data.List_D01.length; i++) {
                    const row = data.List_D01[i];
                    const newRow = document.createElement("tr");

                    newRow.innerHTML = `
                    <td>${i + 1}</td>
                    <td>${row.PO_CODE}</td>
                    <td>${row.PO_DATE}</td>
                    <td>${row.PART_NO}</td>
                    <td>${row.PART_NAME}</td>
                    <td>${row.MODEL}</td>
                    <td>${row.GROUP}</td>
                    <td>${row.PART_SNP}</td>
                    <td>${row.STOCK}</td>
                    <td>${row.PO_QTY}</td>
                    <td>${row.Sales_QTY}</td>
                    <td>${row.COST}</td>
                    <td>${row.Sales}</td>
                `;
                    fragment.appendChild(newRow);
                }

                tbody.appendChild(fragment);
                ShowMessage(localizedText.Success, 'OK');
                $("#BackGround").css("display", "none");
            }).catch(err => {
                $("#BackGround").css("display", "none");
                ShowMessage("Lỗi tải dữ liệu: " + err.message, "error");
            });

    } else {
        ShowMessage("Không có thông tin hợp lệ", "error");
    }
}


function SearchByYear() {
    let year = $('#tab2_year').val();
    if (year) {
        $("#BackGround").css("display", "block");
        let BaseParameter = new Object();
        BaseParameter = {
            Year: year,
            SearchString: "2" //loại timd kiếm cho tab2
        }
        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        let url = "/D01/Buttonfind_Click";
        fetch(url, {
            method: "POST",
            body: formUpload,
            headers: {
                // Nếu không có headers đặc biệt thì không cần thêm gì ở đây
            }
        }).then((response) => {
            response.json().then((data) => {
                const tbody = document.getElementById("DataGridView2");
                tbody.innerHTML = ""; // Xóa dữ liệu cũ nếu có
                const fragment = document.createDocumentFragment(); // Khai báo fragment

                for (let i = 0; i < data.List_D01.length; i++) {
                    const rowData = data.List_D01[i];
                    const newRow = document.createElement("tr");

                    // Tạo dòng HTML tương ứng với các cột đã mở rộng (ví dụ: MODEL, P_COUNT, PO_QTY, ACT_QTY, RAT, Sales và các cột theo tháng)
                    newRow.innerHTML = `
                <td>${i + 1}</td>
                <td>${rowData.MODEL}</td>
                <td>${rowData.P_COUNT}</td>
                <td>${rowData.PO_QTY}</td>
                <td>${rowData.ACT_QTY}</td>
                <td>${rowData.RAT}</td>
                <td>${rowData.Sales != null ? Number(rowData.Sales).toFixed(2) : ""}</td>

                ${generateMonthColumns(rowData)}
            `;

                    fragment.appendChild(newRow);
                }

                tbody.appendChild(fragment);
                ShowMessage(localizedText.Success, 'OK');
                $("#BackGround").css("display", "none");
            }).catch((err) => {
                $("#BackGround").css("display", "none");
            });
        });
    } else {
        ShowMessage("Không có thông tin hợp lệ", "error");
    }
}

function generateMonthColumns(rowData) {
    let html = "";
    for (let i = 1; i <= 12; i++) {
        const mm = i.toString().padStart(2, '0');
        html += `
            <td>${rowData["PO_" + mm] || ""}</td>
            <td>${rowData["ACT_" + mm] || ""}</td>       
            <td>${rowData["Sales_" + mm] != null ? Number(rowData["Sales_" + mm]).toFixed(2) : ""}</td>
        `;
    }
    return html;
}

function Buttonadd_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/D01/Buttonadd_Click";

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
    let url = "/D01/Buttonsave_Click";

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
    let url = "/D01/Buttondelete_Click";

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
    let url = "/D01/Buttoncancel_Click";

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
    let url = "/D01/Buttoninport_Click";

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
    let url = "/D01/Buttonexport_Click";

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
    let url = "/D01/Buttonprint_Click";

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


