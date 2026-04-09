let BaseResult;
let DataGridView1RowIndex = -1;

$(document).ready(function () {
    LoadData();
});

$("#Button1").click(function () {
    Button1_Click();
});

$("#Buttonclose").click(function () {
    Buttonclose_Click();
});

$("#ComboBox1").change(function () {
    ComboBox1_Changed();
});

function LoadData() {
    $("#BackGround").css("display", "block");

    let BaseParameter = new Object();
    BaseParameter = {}

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/V03_3/Load";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {}
    }).then((response) => {
        response.json().then((data) => {
            BaseResult = data;
            RenderComboBox1();
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            console.error("Error parsing JSON:", err);
            $("#BackGround").css("display", "none");
        })
    }).catch((err) => {
        console.error("Fetch error:", err);
        $("#BackGround").css("display", "none");
    });
}

function RenderComboBox1() {
    let HTML = "<option value=''>-- Select Order No --</option>";

    if (BaseResult && BaseResult.ComboBox1 && BaseResult.ComboBox1.length > 0) {
        for (let i = 0; i < BaseResult.ComboBox1.length; i++) {
            HTML = HTML + "<option value='" + BaseResult.ComboBox1[i].PDP_NO + "'>" + BaseResult.ComboBox1[i].PDP_NO + "</option>";
        }
    }

    document.getElementById("ComboBox1").innerHTML = HTML;
}

function ComboBox1_Changed() {
    $("#BackGround").css("display", "block");

    let selectedValue = $("#ComboBox1").val();

    let BaseParameter = new Object();
    BaseParameter = {
        ListSearchString: []
    }
    BaseParameter.ListSearchString.push(selectedValue);

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/V03_3/ComboBox1_Changed";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {}
    }).then((response) => {
        response.json().then((data) => {
            BaseResult = data;
            DataGridView1Render();
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            console.error("Error parsing JSON:", err);
            $("#BackGround").css("display", "none");
        })
    }).catch((err) => {
        console.error("Fetch error:", err);
        $("#BackGround").css("display", "none");
    });
}

function DataGridView1Render() {
    let HTML = "";

    if (BaseResult) {
        if (BaseResult.DataGridView1) {
            if (BaseResult.DataGridView1.length > 0) {
                for (let i = 0; i < BaseResult.DataGridView1.length; i++) {
                    HTML = HTML + "<tr onclick='DataGridView1_SelectionChanged(" + i + ")' ondblclick='DataGridView1_CellDoubleClick()'>";
                    HTML = HTML + "<td>" + (BaseResult.DataGridView1[i].PDP_CONF || "") + "</td>";
                    HTML = HTML + "<td>" + (BaseResult.DataGridView1[i].PDP_NO || "") + "</td>";
                    HTML = HTML + "<td>" + (BaseResult.DataGridView1[i].PDP_DATE1 || "") + "</td>";
                    HTML = HTML + "<td>" + (BaseResult.DataGridView1[i].DEPARTMENT || "") + "</td>";
                    HTML = HTML + "<td>" + (BaseResult.DataGridView1[i].PN_NM || "") + "</td>";
                    HTML = HTML + "<td>" + (BaseResult.DataGridView1[i].PN_V || "") + "</td>";
                    HTML = HTML + "<td>" + (BaseResult.DataGridView1[i].PSPEC_V || "") + "</td>";
                    HTML = HTML + "<td>" + (BaseResult.DataGridView1[i].UNIT_VN || "") + "</td>";
                    HTML = HTML + "<td>" + (BaseResult.DataGridView1[i].PN_K || "") + "</td>";
                    HTML = HTML + "<td>" + (BaseResult.DataGridView1[i].PSPEC_K || "") + "</td>";
                    HTML = HTML + "<td>" + (BaseResult.DataGridView1[i].UNIT_KR || "") + "</td>";
                    HTML = HTML + "<td>" + (BaseResult.DataGridView1[i].PQTY || "") + "</td>";
                    HTML = HTML + "<td>" + (BaseResult.DataGridView1[i].PDP_QTY || "") + "</td>";
                    HTML = HTML + "<td>" + (BaseResult.DataGridView1[i].PDP_MEMO || "") + "</td>";
                    HTML = HTML + "<td>" + (BaseResult.DataGridView1[i].CREATE_DTM || "") + "</td>";
                    HTML = HTML + "<td>" + (BaseResult.DataGridView1[i].CREATE_USER || "") + "</td>";
                    HTML = HTML + "<td>" + (BaseResult.DataGridView1[i].USER_NAME || "") + "</td>";
                    HTML = HTML + "<td>" + (BaseResult.DataGridView1[i].PDP_PART || "") + "</td>";
                    HTML = HTML + "<td>" + (BaseResult.DataGridView1[i].STOCK || "") + "</td>";
                    HTML = HTML + "<td>" + (BaseResult.DataGridView1[i].PDPUSCH_IDX || "") + "</td>";
                    HTML = HTML + "<td>" + (BaseResult.DataGridView1[i].PDP_COST || "") + "</td>";
                    HTML = HTML + "<td>" + (BaseResult.DataGridView1[i].SUM_COST || "") + "</td>";
                    HTML = HTML + "<td>" + (BaseResult.DataGridView1[i].PDP_VAT || "") + "</td>";
                    HTML = HTML + "<td>" + (BaseResult.DataGridView1[i].PDP_ECTCOST || "") + "</td>";
                    HTML = HTML + "<td>" + (BaseResult.DataGridView1[i].PDP_TOTCOST || "") + "</td>";
                    HTML = HTML + "<td>" + (BaseResult.DataGridView1[i].PDP_BE_COST || "") + "</td>";
                    HTML = HTML + "<td>" + (BaseResult.DataGridView1[i].PDP_CMPY || "") + "</td>";
                    HTML = HTML + "<td>" + (BaseResult.DataGridView1[i].COMP_NM || "") + "</td>";
                    HTML = HTML + "</tr>";
                }
            }
        }
    }

    document.getElementById("DataGridView1").innerHTML = HTML;
}

function DataGridView1_SelectionChanged(i) {
    DataGridView1RowIndex = i;
}

function DataGridView1_CellDoubleClick() {
    Button1_Click();
}

function Button1_Click() {
    if (window.opener && !window.opener.closed) {
        window.opener.$("#TextBox1").val($("#ComboBox1").val());
    }
    window.close();
}

function Buttonclose_Click() {
    window.close();
}