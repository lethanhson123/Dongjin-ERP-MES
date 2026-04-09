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
    var activeTab = document.querySelector('.tabs .tab a.active');
    if (activeTab) {
        var currentTabId = activeTab.getAttribute('href').replace('#', '');

        if (currentTabId === 'Tag001') {
           
        } else if (currentTabId === 'Tag002') {
            SearchHistory();
        } else if (currentTabId === 'Tag003') {


        }
    }
}

async function SearchHistory() {
    const fromDate = $("#tab2_fromDate").val()?.trim();
    const toDate = $("#tab2_toDate").val()?.trim();
    const selectedValue = document.querySelector('input[name="STATUS"]:checked')?.value || "all";
    const PartCode = $("#tab2_PartCode").val()?.trim() || "";
    const partName = $("#tab2_PartName").val()?.trim() || "";
    const PackingLot = $("#tab2_PackingCode").val()?.trim() || "";
    const Stage = $("#tab2_Stage").val()?.trim() || "";

    const BaseParameter = {
        FilterType: selectedValue,
        LeadData: Stage,
        PartNo: PartCode,
        PartName: partName,
        PackingLotCode: PackingLot,
        FromDate: fromDate,
        ToDate: toDate,
        Code: "LoadHistory"
    };

    const formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    $("#BackGround").fadeIn();

    try {

        const response = await fetch( "/D02/Buttonfind_Click", {
            method: "POST",
            body: formUpload
        });

        const data = await response.json();
        const tbody = document.getElementById("DataGridView2");
        tbody.innerHTML = "";

        if (data.ListtspartTranfer && data.ListtspartTranfer.length > 0) {

            const fragment = document.createDocumentFragment();
            const stats = {
                totalY: 0,
                totalN: 0,
                totalD: 0,
                uniqueSet: new Set(),
                deletedSet: new Set()
            };

            for (let i = 0; i < data.ListtspartTranfer.length; i++) {
                const rowData = data.ListtspartTranfer[i];
                const tr = document.createElement("tr");

                if (rowData.DSCN_YN === "D") {
                    tr.classList.add("red-text");
                    stats.totalD++;
                    stats.deletedSet.add(rowData.PKG_GRP);
                } else if (rowData.DSCN_YN === "Y") {
                    stats.totalY++;
                } else {
                    stats.totalN++;
                }

                stats.uniqueSet.add(rowData.PKG_GRP);

                tr.innerHTML = `
                                <td>${i + 1}</td> 
                                <td>${rowData.PART_NO || ""}</td>
                                <td>${rowData.PART_NM || ""}</td>
                                <td>${rowData.PART_CAR || ""}</td>
                                <td>${rowData.PART_FML || ""}</td>
                                <td>${rowData.SNP_QTY ?? ""}</td>
                                <td>${rowData.PKG_GRP || ""}</td>
                                <td>${rowData.QTY ?? ""}</td>
                                <td>${FormatDate(rowData.MTIN_DTM)}</td>
                                <td>${FormatDateTime(rowData.CREATE_DTM)}</td>
                                <td class="preserve-space">${rowData.LOT_CODE || ""}</td>
                                <td>${rowData.REMARK || ""}</td>
                                <td>${rowData.CREATE_USER || ""}</td>
                                <td>${rowData.DSCN_YN || ""}</td>
                                <td>${FormatDateTime(rowData.DeleteTime)}</td>
                                <td>${rowData.DeleteBy || ""}</td>
                            `;

                fragment.appendChild(tr);
            }

            tbody.appendChild(fragment);

            // Gán giá trị thống kê
            $("#tab2_TotalQty").val(stats.totalY + stats.totalN);
            $("#tab2_DistinctPartNo").val(stats.uniqueSet.size - stats.deletedSet.size);
            $("#tab2_TotalY").val(stats.totalY);
            $("#tab2_totalN").val(stats.totalN);

            ShowMessage(localizedText.Success, 'OK');
        }
        else if (data.ErrorNumber === -1) {
            ShowMessage(localizedText.Empty, 'error');
        }
        else {
            ShowMessage(localizedText.Empty, 'error');
        }
    } catch (err) {
        ShowMessage(err.message, "error");
    } finally {
        $("#BackGround").fadeOut();
    }
}

function Buttonadd_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/D02/Buttonadd_Click";

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
    let url = "/D02/Buttonsave_Click";

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
    let url = "/D02/Buttondelete_Click";

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
    let url = "/D02/Buttoncancel_Click";

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
    let url = "/D02/Buttoninport_Click";

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
    let url = "/D02/Buttonexport_Click";

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
    let url = "/D02/Buttonprint_Click";

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

async function LoadSelector() {
    const select = document.getElementById("tab2_POCode");
    const select1 = document.getElementById("tab2_POCode1");

    // Xóa các option cũ
    select.innerHTML = "";
    select1.innerHTML = "";

    // Thêm option mặc định
    const defaultOption = document.createElement("option");
    defaultOption.value = "";
    defaultOption.text = "--- Chọn PO ---";
    select.appendChild(defaultOption);

    try {
        const response = await fetch('/D02/Load', {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json'
            }
        });

        const result = await response.json();

        if (result && result.ComboBox1) {
            result.ComboBox1.forEach(item => {
                const option = document.createElement("option");
                option.value = item.PO_CODE;
                option.text = item.PO_CODE; // Có thể thêm ngày tạo nếu cần
                select.appendChild(option);
                const option1 = document.createElement("option");
                option1.value = item.PO_CODE;
                option1.text = item.PO_CODE; // Có thể thêm ngày tạo nếu cần
                select1.appendChild(option1);                
            });
        } else {
            console.warn("Không có dữ liệu từ API.");
        }
    } catch (error) {
        console.error("Lỗi khi load selector:", error);
    }
}


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


document.addEventListener('DOMContentLoaded', function () {
    document.querySelector('a[href="#Tag002"]').addEventListener('click', function () {
        setTimeout(() => {
            LoadSelector();
        }, 100);
    });
});




