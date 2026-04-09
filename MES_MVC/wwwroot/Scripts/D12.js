let BaseResult;
let controller = "/D12/";

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
    // ShowMessage("tim kieem", "ok");

    var activeTab = document.querySelector('.tabs .tab a.active');
    if (activeTab) {
        var currentTabId = activeTab.getAttribute('href').replace('#', '');

        if (currentTabId === 'Tag002') {
            SearchFG();
        } else if (currentTabId === 'Tag003') {
              SearchProductAbility();
        } else if (currentTabId === 'Tag004') {
             SearchRework();

        }
    }
}

async function SearchFG() { 

    const fromDate = $("#tab2_fromDate").val()?.trim();
    const toDate = $("#tab2_toDate").val()?.trim();
    const selectedValue = document.querySelector('input[name="STATUS2"]:checked')?.value || "all";
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

        const response = await fetch(controller + "Buttonfind_Click", {
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

async function SearchProductAbility() {
    const fromDate = $("#tab3_fromDate").val()?.trim();
    const toDate = $("#tab3_toDate").val()?.trim();
    const selectedValue = document.querySelector('input[name="STATUS2"]:checked')?.value || "all";
    const Catalogy = $("#tab3_Catalogy").val()?.trim() || "";
    const partName = $("#tab3_PartName").val()?.trim() || "";
    const Stage = $("#tab2_Stage").val()?.trim() || "";

    const BaseParameter = {
        LeadData: Stage,   
        PartName: partName,
        FilterType: Catalogy,
        FromDate: fromDate,
        ToDate: toDate,
        Code: "LoadAbility"
    };

    const formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    $("#BackGround").fadeIn();

    try {

        const response = await fetch(controller + "Buttonfind_Click", {
            method: "POST",
            body: formUpload
        });

        const data = await response.json();
        const tbody = document.getElementById("DataGridView3");
        tbody.innerHTML = "";

        if (data.ListtspartTranfer && data.ListtspartTranfer.length > 0) {

            const fragment = document.createDocumentFragment();   
            const stats = {
                totalC: 0,
                totalB: 0,               
            };

            for (let i = 0; i < data.ListtspartTranfer.length; i++) {
                const rowData = data.ListtspartTranfer[i];
                const tr = document.createElement("tr");
                stats.totalC = stats.totalC + (rowData.SUM_QTY ?? 0);
                stats.totalB = stats.totalB + (rowData.SUM_BOX ?? 0);

                tr.innerHTML = `
                                <td>${i + 1}</td> 
                                <td>${rowData.CATALOG || ""}</td>
                                <td>${rowData.STAGE || ""}</td>
                                <td>${rowData.PART_NM || ""}</td>
                                <td>${rowData.PN_COUNT ?? ""}</td>
                                <td>${rowData.PART_SNP ?? ""}</td>
                                <td>${rowData.SUM_QTY ?? ""}</td>
                                <td>${rowData.SUM_BOX ?? ""}</td>
                                <td>${rowData.MIN || ""}</td>
                                <td>${rowData.MAX || ""}</td>
                                <td>${rowData.TIME ?? ""}</td>                              
                            `;

                fragment.appendChild(tr);
            }

            tbody.appendChild(fragment);

            // Gán giá trị thống kê
            $("#tab3_itemCount").val(stats.totalC);
            $("#tab3_boxCount").val(stats.totalB);     

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

async function SearchRework() {

    const fromDate = $("#tab4_fromDate").val()?.trim();
    const toDate = $("#tab4_toDate").val()?.trim();
    const selectedValue = document.querySelector('input[name="STATUS4"]:checked')?.value || "all";
    const PartCode = $("#tab4_PartCode").val()?.trim() || "";
    const PackingLot = $("#tab4_PackingCode").val()?.trim() || "";
    const partName = $("#tab4_PartName").val()?.trim() || "";
    const LotCode = $("#tab4_LotCode").val()?.trim() || "";
    const Stage = $("#tab4_Stage").val()?.trim() || "";

    const BaseParameter = {
        FilterType: selectedValue,
        LeadData: Stage,
        PartNo: PartCode,
        PartName: partName,
        PackingLotCode: PackingLot, 
        LotCode: LotCode,
        FromDate: fromDate,
        ToDate: toDate,
        Code: "LoadRework"
    };

    const formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    $("#BackGround").fadeIn();

    try {

        const response = await fetch(controller + "Buttonfind_Click", {
            method: "POST",
            body: formUpload
        });

        const data = await response.json();
        const tbody = document.getElementById("DataGridView4");
        tbody.innerHTML = "";

        if (data.ListtspartTranfer && data.ListtspartTranfer.length > 0) {

            const fragment = document.createDocumentFragment();
            const stats = {
                totalC: 0,
                totalB: 0,
            };

            for (let i = 0; i < data.ListtspartTranfer.length; i++) {
                const rowData = data.ListtspartTranfer[i];
                const tr = document.createElement("tr");
                stats.totalC = stats.totalC + (rowData.SUM_QTY ?? 0);
                stats.totalB = stats.totalB + (rowData.SUM_BOX ?? 0);
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
                            `;

                fragment.appendChild(tr);
            }

            tbody.appendChild(fragment);

            // Gán giá trị thống kê
            $("#tab3_itemCount").val(stats.totalC);
            $("#tab3_boxCount").val(stats.totalB);

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

document.addEventListener("DOMContentLoaded", function () {
    const radios2 = document.querySelectorAll('input[name="STATUS2"]');
    radios2.forEach(function (radio) {
        radio.addEventListener("change", function () {
            SearchFG();
        });
    });

    const radios3 = document.querySelectorAll('input[name="STATUS3"]');
    radios3.forEach(function (radio) {
        radio.addEventListener("change", function () {
            SearchProductAbility();
        });
    });

    const radios4 = document.querySelectorAll('input[name="STATUS4"]');
    radios4.forEach(function (radio) {
        radio.addEventListener("change", function () {
            SearchRework();
        });
    });

});


function Buttonadd_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/D12/Buttonadd_Click";

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
    let url = "/D12/Buttonsave_Click";

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
    let url = "/D12/Buttondelete_Click";

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
    let url = "/D12/Buttoncancel_Click";

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
    let url = "/D12/Buttoninport_Click";

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
    var activeTab = document.querySelector('.tabs .tab a.active');
    if (activeTab) {
        var currentTabId = activeTab.getAttribute('href').replace('#', '');

        if (currentTabId === 'Tag001') {
            TableHTMLToExcel("Table1", "ScanList", "D12_Scan_list");

        } else if (currentTabId === 'Tag002') {
            TableHTMLToExcel("Table2", "HistoryList", "D12_History_list");
        } else if (currentTabId === 'Tag003') {
            TableHTMLToExcel("Table3", "HistoryList", "D12_History_list");
        }
        else if (currentTabId === 'Tag004') {
            TableHTMLToExcel("Table4", "HistoryList", "D12_History_list");
        }
    }
}
function Buttonprint_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/D12/Buttonprint_Click";

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


