let BaseResult = {};
let TabIndex = 1;
let DataGridView4RowIndex = 0;
let DataGridView5RowIndex = 0;
let chartInstance = null;

function formatNumber(value) {
    return (isNaN(value) || value === "" || value === null) ? "0" : parseFloat(value).toLocaleString("vi-VN", { maximumFractionDigits: 0 });
}

function costAuto() {
    try {
        let qty = parseFloat($("#TextBox1").val()) || 0;
        let cost = parseFloat($("#TextBox_C1").val()) || 0;
        let vat = parseFloat($("#TextBox_C3").val()) || 0;
        let ectCost = parseFloat($("#TextBox_C4").val()) || 0;

        let sumCost = qty * cost;
        $("#TextBox_C2").val(sumCost.toFixed(0));

        if ($("#TextBox_C3").val() === "") {
            vat = sumCost * 0.1;
            $("#TextBox_C3").val(vat.toFixed(0));
        }

        let totalCost = sumCost + vat + ectCost;
        $("#TextBox_C5").val(totalCost.toFixed(0));

        $("#Label33").text(formatNumber(cost));
        $("#Label35").text(formatNumber(sumCost));
        $("#Label46").text(formatNumber(vat));
        $("#Label47").text(formatNumber(ectCost));
        $("#Label48").text(formatNumber(totalCost));
    } catch (e) {
        $("#TextBox_C1, #TextBox_C2, #TextBox_C3, #TextBox_C4, #TextBox_C5").val("0");
        $("#Label33, #Label35, #Label46, #Label47, #Label48").text("0");
    }
}

function renderChart() {
    if (chartInstance) chartInstance.destroy();
    const ctx = document.getElementById("ChartCanvas").getContext("2d");
    const data = BaseResult.DataGridView5;
    if (!data || data.length === 0) return;

    const lastRow = data[data.length - 1];
    const labels = ["01M", "02M", "03M", "04M", "05M", "06M", "07M", "08M", "09M", "10M", "11M", "12M"];
    const values = labels.map((_, i) => Math.round(parseFloat(lastRow[`D${String(i + 1).padStart(2, '0')}`] || 0)));

    chartInstance = new Chart(ctx, {
        type: "bar",
        data: {
            labels: labels,
            datasets: [{
                label: "Tổng chi phí theo tháng",
                data: values,
                backgroundColor: "rgba(54, 162, 235, 0.6)",
                borderColor: "rgba(54, 162, 235, 1)",
                borderWidth: 1
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            scales: {
                y: {
                    beginAtZero: true,
                    title: { display: true, text: "Tổng chi phí" },
                    ticks: { callback: value => formatNumber(value) }
                },
                x: { title: { display: true, text: "Tháng" } }
            }
        }
    });
}

function RenderComboBox1() {
    let HTML = "";
    HTML += "<option value='0'>All</option>";
    HTML += "<option value='1'>Wait</option>";
    HTML += "<option value='2'>Ship</option>";
    HTML += "<option value='3'>Complete</option>";
    HTML += "<option value='4'>Cencel</option>";
    HTML += "<option value='5'>Partical Complete</option>";
    HTML += "<option value='6'>-</option>";
    document.getElementById("ComboBox1").innerHTML = HTML;
}

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
    $("#BackGround").css("display", "block");
    let BaseParameter = {};
    if (TabIndex === 1) {
        BaseParameter.TabName = "TabPage3";
        BaseParameter.RadioButton1 = $("#RadioButton1").is(":checked");
        BaseParameter.RadioButton2 = $("#RadioButton2").is(":checked");
        BaseParameter.RadioButton3 = $("#RadioButton3").is(":checked");
        BaseParameter.RadioButton4 = $("#RadioButton4").is(":checked");
        BaseParameter.ListSearchString = [
            $("#DateTimePicker1").val() || "",
            $("#DateTimePicker2").val() || "",
            $("#TextBox10").val() || "",
            $("#TextBox11").val() || "",
            $("#TextBox12").val() || "",
            $("#TextBox13").val() || "",
            $("#TextBox14").val() || "",
            $("#TextBox15").val() || "",
            $("#ComboBox1").val() || "0"
        ];
    } else {
        BaseParameter.TabName = "TabPage4";
        BaseParameter.ListSearchString = [
            $("#ComboBox2").val() || ""
        ];
    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    fetch("/V06/Buttonfind_Click", {
        method: "POST",
        body: formUpload
    }).then(response => response.json()).then(data => {
        BaseResult = data;
        if (TabIndex === 1) {
            DataGridView4Render();
        } else {
            DataGridView5Render();
        }
        $("#BackGround").css("display", "none");
    }).catch(() => {
        $("#BackGround").css("display", "none");
    });
}

function Buttonsave_Click() {
    if (TabIndex !== 1 || !BaseResult || !BaseResult.DataGridView4 || BaseResult.DataGridView4.length === 0) return;
    let idx = DataGridView4RowIndex;
    let row = BaseResult.DataGridView4[idx];
    if (!row.PDPUSCH_IDX) return;
    let PDP_QTY = $("#TextBox1").val() || row.PDP_QTY || "";
    let PDP_COST = $("#TextBox_C1").val() || row.PDP_COST || "";
    let PDP_VAT = $("#TextBox_C3").val() || row.PDP_VAT || "";
    let PDP_ECTCOST = $("#TextBox_C4").val() || row.PDP_ECTCOST || "";
    let PDP_TOTCOST = $("#TextBox_C5").val() || row.PDP_TOTCOST || "";
    let BaseParameter = {
        ListSearchString: [PDP_QTY, PDP_COST, PDP_VAT, PDP_ECTCOST, PDP_TOTCOST, row.PDPUSCH_IDX]
    };
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    fetch("/V06/Buttonsave_Click", {
        method: "POST",
        body: formUpload
    }).then(response => response.json()).then(data => {
        if (data.Success) {
            alert("Save Success");
            Buttonfind_Click();
        } else {
            alert("Save Fail");
        }
    });
}

function Buttondelete_Click() {
    if (TabIndex !== 1 || !BaseResult || !BaseResult.DataGridView4 || BaseResult.DataGridView4.length === 0) return;
    let idx = DataGridView4RowIndex;
    let row = BaseResult.DataGridView4[idx];
    if (!row.PDPUSCH_IDX) return;
    if (confirm("Are you sure you want to delete?")) {
        let BaseParameter = {
            ListSearchString: [row.PDPUSCH_IDX],
            USER_ID: GetCookieValue("USER_ID") || ""
        };
        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        fetch("/V06/Buttondelete_Click", {
            method: "POST",
            body: formUpload
        }).then(response => response.json()).then(data => {
            if (data.Success) {
                alert("Delete Success");
                Buttonfind_Click();
            } else {
                alert("Delete Fail");
            }
        });
    }
}

function Buttoncancel_Click() {
    if (TabIndex === 1) {
        $("#TextBox1, #TextBox_C1, #TextBox_C3, #TextBox_C4, #TextBox_C5").val("");
        DataGridView4RowIndex = 0;
    }
}

function Buttonadd_Click() { }
function Buttoninport_Click() { }
function Buttonexport_Click() {
    if (TabIndex === 1 && BaseResult && BaseResult.DataGridView4 && BaseResult.DataGridView4.length > 0) {
        let data = BaseResult.DataGridView4;
        let HTML = "<table border='1'>";
        HTML += "<tr>";
        HTML += "<th>Request</th><th>Cancel</th><th>Shipping</th><th>Complete</th><th>ORDER</th><th>State</th>";
        HTML += "<th>ORDER_NO</th><th>ORDER_DATE</th><th>Department</th><th>Approval date</th>";
        HTML += "<th>Product_NO</th><th>PART_NAME</th><th>PART_SPEC</th><th>MC_NO</th><th>UNIT</th>";
        HTML += "<th>Bundles_QTY</th><th>STOCK</th><th>LAST_COST</th><th>PO_QTY</th><th>COST</th>";
        HTML += "<th>TOTAL_COST</th><th>Other_COST</th><th>VAT</th><th>TOTAL_COST(VAT)</th>";
        HTML += "<th>PDP_MEMO</th><th>Company_CODE</th><th>Company_NM</th><th>CREATE_DTM</th>";
        HTML += "<th>CREATE_USER</th><th>USER_NAME</th><th>FIFO</th>";
        HTML += "</tr>";
        for (let i = 0; i < data.length; i++) {
            let orderStatus = data[i].ORDER_ST || "0";
            let statusText = {
                "0": "-",
                "1": "Waiting",
                "2": "Shipping",
                "3": "Complete",
                "4": "Cancel",
                "5": "Ing..."
            }[orderStatus] || "-";
            HTML += "<tr>";
            HTML += `<td>${orderStatus === "1" ? "TRUE" : "FALSE"}</td>`;
            HTML += `<td>${orderStatus === "4" ? "TRUE" : "FALSE"}</td>`;
            HTML += `<td>${orderStatus === "2" || orderStatus === "5" ? "TRUE" : "FALSE"}</td>`;
            HTML += `<td>${orderStatus === "3" || orderStatus === "5" ? "TRUE" : "FALSE"}</td>`;
            HTML += `<td>${data[i].PDP_CONF || ""}</td>`;
            HTML += `<td>${statusText}</td>`;
            HTML += `<td>${data[i].PDP_NO || ""}</td>`;
            HTML += `<td>${data[i].PDP_DATE1 || ""}</td>`;
            HTML += `<td>${data[i].DEP || ""}</td>`;
            HTML += `<td>${data[i].PDP_CNF_DATE || "----"}</td>`;
            HTML += `<td>${data[i].PN_NM || ""}</td>`;
            HTML += `<td>${(data[i].PN_V || "") + "\n" + (data[i].PN_K || "")}</td>`;
            HTML += `<td>${(data[i].PSPEC_V || "") + "\n" + (data[i].PSPEC_K || "")}</td>`;
            HTML += `<td>${data[i].MCNO || ""}</td>`;
            HTML += `<td>${(data[i].UNIT_VN || "") + "\n" + (data[i].UNIT_KR || "")}</td>`;
            HTML += `<td>${data[i].PQTY || "0"}</td>`;
            HTML += `<td>${data[i].STOCK || "0"}</td>`;
            HTML += `<td>${data[i].PDP_BE_COST || "0"}</td>`;
            HTML += `<td>${data[i].PDP_QTY || "0"}</td>`;
            HTML += `<td>${data[i].PDP_COST || "0"}</td>`;
            HTML += `<td>${data[i].SUM_COST || "0"}</td>`;
            HTML += `<td>${data[i].PDP_ECTCOST || "0"}</td>`;
            HTML += `<td>${data[i].PDP_VAT || "0"}</td>`;
            HTML += `<td>${data[i].PDP_TOTCOST || "0"}</td>`;
            HTML += `<td>${data[i].PDP_MEMO || ""}</td>`;
            HTML += `<td>${data[i].PDP_CMPY || ""}</td>`;
            HTML += `<td>${data[i].COMP_NM || ""}</td>`;
            HTML += `<td>${data[i].CREATE_DTM || ""}</td>`;
            HTML += `<td>${data[i].CREATE_USER || ""}</td>`;
            HTML += `<td>${data[i].NAME || ""}</td>`;
            HTML += `<td>${data[i].PDP_FIFO || "N"}</td>`;
            HTML += "</tr>";
        }
        HTML += "</table>";
        let blob = new Blob([HTML], { type: "application/vnd.ms-excel" });
        let url = window.URL.createObjectURL(blob);
        let a = document.createElement("a");
        a.href = url;
        a.download = `V06_${new Date().toISOString().slice(0, 10).replace(/-/g, "")}.xls`;
        document.body.appendChild(a);
        a.click();
        document.body.removeChild(a);
        window.URL.revokeObjectURL(url);
    }
}

function Buttonprint_Click() { }
function Buttonhelp_Click() {
    window.open("/WMP_PLAY", "_blank", "width=800,height=460");
}
function Buttonclose_Click() {
    history.back();
}

function DataGridView4Render() {
    let HTML = "";
    if (BaseResult && BaseResult.DataGridView4 && BaseResult.DataGridView4.length > 0) {
        for (let i = 0; i < BaseResult.DataGridView4.length; i++) {
            let orderStatus = BaseResult.DataGridView4[i].ORDER_ST || "0";
            let statusText = {
                "0": "-",
                "1": "Waiting",
                "2": "Shipping",
                "3": "Complete",
                "4": "Cancel",
                "5": "Ing..."
            }[orderStatus] || "-";
            HTML += "<tr onclick='DataGridView4_SelectionChanged(" + i + ")'>";
            HTML += "<td><input type='checkbox' " + (orderStatus === "1" ? "checked" : "") + " disabled><span></span></td>";
            HTML += "<td><input type='checkbox' " + (orderStatus === "4" ? "checked" : "") + " disabled><span></span></td>";
            HTML += "<td><input type='checkbox' " + (orderStatus === "2" || orderStatus === "5" ? "checked" : "") + " disabled><span></span></td>";
            HTML += "<td><input type='checkbox' " + (orderStatus === "3" || orderStatus === "5" ? "checked" : "") + " disabled><span></span></td>";
            HTML += "<td>" + (BaseResult.DataGridView4[i].PDP_CONF || "") + "</td>";
            HTML += "<td>" + statusText + "</td>";
            HTML += "<td>" + (BaseResult.DataGridView4[i].PDP_NO || "") + "</td>";
            HTML += "<td>" + (BaseResult.DataGridView4[i].PDP_DATE1 || "") + "</td>";
            HTML += "<td>" + (BaseResult.DataGridView4[i].DEP || "") + "</td>";
            HTML += "<td>" + (BaseResult.DataGridView4[i].PDP_CNF_DATE || "----") + "</td>";
            HTML += "<td>" + (BaseResult.DataGridView4[i].PN_NM || "") + "</td>";
            HTML += "<td>" + (BaseResult.DataGridView4[i].PN_V || "") + "<br>" + (BaseResult.DataGridView4[i].PN_K || "") + "</td>";
            HTML += "<td>" + (BaseResult.DataGridView4[i].PSPEC_V || "") + "<br>" + (BaseResult.DataGridView4[i].PSPEC_K || "") + "</td>";
            HTML += "<td>" + (BaseResult.DataGridView4[i].MCNO || "") + "</td>";
            HTML += "<td>" + (BaseResult.DataGridView4[i].UNIT_VN || "") + "<br>" + (BaseResult.DataGridView4[i].UNIT_KR || "") + "</td>";
            HTML += "<td>" + (BaseResult.DataGridView4[i].PQTY || "0") + "</td>";
            HTML += "<td>" + (BaseResult.DataGridView4[i].STOCK || "0") + "</td>";
            HTML += "<td>" + (BaseResult.DataGridView4[i].PDP_BE_COST || "0") + "</td>";
            HTML += "<td>" + (BaseResult.DataGridView4[i].PDP_IN_QTY || "0") + "</td>";
            HTML += "<td>" + (BaseResult.DataGridView4[i].PDP_QTY || "0") + "</td>";
            HTML += "<td>" + (BaseResult.DataGridView4[i].PDP_COST || "0") + "</td>";
            HTML += "<td>" + (BaseResult.DataGridView4[i].SUM_COST || "0") + "</td>";
            HTML += "<td>" + (BaseResult.DataGridView4[i].PDP_ECTCOST || "0") + "</td>";
            HTML += "<td>" + (BaseResult.DataGridView4[i].PDP_VAT || "0") + "</td>";
            HTML += "<td>" + (BaseResult.DataGridView4[i].PDP_TOTCOST || "0") + "</td>";
            HTML += "<td>" + (BaseResult.DataGridView4[i].PDP_MEMO || "") + "</td>";
            HTML += "<td>" + (BaseResult.DataGridView4[i].PDP_CMPY || "") + "</td>";
            HTML += "<td>" + (BaseResult.DataGridView4[i].COMP_NM || "") + "</td>";
            HTML += "<td>" + (BaseResult.DataGridView4[i].CREATE_DTM || "") + "</td>";
            HTML += "<td>" + (BaseResult.DataGridView4[i].CREATE_USER || "") + "</td>";
            HTML += "<td>" + (BaseResult.DataGridView4[i].NAME || "") + "</td>";
            HTML += "<td>" + (BaseResult.DataGridView4[i].PDP_FIFO || "N") + "</td>";
            HTML += "</tr>";
        }
    }
    document.getElementById("DataGridView4").innerHTML = HTML;
}

function DataGridView4_SelectionChanged(idx) {
    DataGridView4RowIndex = idx;
    let row = BaseResult.DataGridView4[idx];
    $("#Label49").text(row.PDP_NO || "-");
    $("#Label9").text(row.PN_NM || "-");
    $("#Label10").text(row.PDPUSCH_IDX || "-");
    $("#TextBox2").val(row.PKRVN || "");
    $("#TextBox3").val(row.SKRVN || "");
    $("#TextBox4").val(row.COMP_NM || "");
    $("#TextBox1").val(row.PDP_QTY || "");
    $("#TextBox_C1").val(row.PDP_COST || "");
    $("#TextBox_C2").val(row.SUM_COST || "");
    $("#TextBox_C3").val(row.PDP_VAT || "");
    $("#TextBox_C4").val(row.PDP_ECTCOST || "");
    $("#TextBox_C5").val(row.PDP_TOTCOST || "");
    let isReadOnly = row.ORDER_ST !== "0";
    $("#TextBox1, #TextBox_C1, #TextBox_C3, #TextBox_C4").prop("readonly", isReadOnly);
    costAuto();
}

function DataGridView5Render() {
    let HTML = "";
    if (BaseResult && BaseResult.DataGridView5 && BaseResult.DataGridView5.length > 0) {
        for (let i = 0; i < BaseResult.DataGridView5.length; i++) {
            HTML += "<tr>";
            HTML += "<td>" + (BaseResult.DataGridView5[i].PDP_FACT || BaseResult.DataGridView5[i].CD_SYS_NOTE || "") + "</td>";
            HTML += "<td>" + formatNumber(BaseResult.DataGridView5[i].D01 || "0") + "</td>";
            HTML += "<td>" + formatNumber(BaseResult.DataGridView5[i].D02 || "0") + "</td>";
            HTML += "<td>" + formatNumber(BaseResult.DataGridView5[i].D03 || "0") + "</td>";
            HTML += "<td>" + formatNumber(BaseResult.DataGridView5[i].D04 || "0") + "</td>";
            HTML += "<td>" + formatNumber(BaseResult.DataGridView5[i].D05 || "0") + "</td>";
            HTML += "<td>" + formatNumber(BaseResult.DataGridView5[i].D06 || "0") + "</td>";
            HTML += "<td>" + formatNumber(BaseResult.DataGridView5[i].D07 || "0") + "</td>";
            HTML += "<td>" + formatNumber(BaseResult.DataGridView5[i].D08 || "0") + "</td>";
            HTML += "<td>" + formatNumber(BaseResult.DataGridView5[i].D09 || "0") + "</td>";
            HTML += "<td>" + formatNumber(BaseResult.DataGridView5[i].D10 || "0") + "</td>";
            HTML += "<td>" + formatNumber(BaseResult.DataGridView5[i].D11 || "0") + "</td>";
            HTML += "<td>" + formatNumber(BaseResult.DataGridView5[i].D12 || "0") + "</td>";
            HTML += "</tr>";
        }
    }
    document.getElementById("DataGridView5").innerHTML = HTML;
    renderChart();
}

function GetCookieValue(name) {
    let matches = document.cookie.match(new RegExp(
        "(?:^|; )" + name.replace(/([\.$?*|{}\(\)\[\]\\\/\+^])/g, '\\$1') + "=([^;]*)"
    ));
    return matches ? decodeURIComponent(matches[1]) : "";
}

$(document).ready(function () {
    let today = new Date();
    let day = ("0" + today.getDate()).slice(-2);
    let month = ("0" + (today.getMonth() + 1)).slice(-2);
    let dateStr = today.getFullYear() + "-" + month + "-" + day;
    $("#DateTimePicker1, #DateTimePicker2").val(dateStr);
    $("#ComboBox2").val(today.getFullYear());
    $(".tabs .tab a").click(function () {
        TabIndex = $(this).attr("id") === "TabPage3" ? 1 : 2;
        if (TabIndex === 2) Buttonfind_Click();
    });
    BaseResult.DataGridView4 = [];
    BaseResult.DataGridView5 = [];
    RenderComboBox1();
    Buttonfind_Click();
    $("#TextBox1, #TextBox_C1, #TextBox_C3, #TextBox_C4").on("input", costAuto);
});