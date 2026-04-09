let SAVE_BUTCHK = false;
let BaseResult;
let DataGridView1RowIndex;
let CMP_CODE = "";

$(window).focus(function () {
}).blur(function () {
    window.close();
});

$(document).ready(function () {
    ComboBox1_SelectedIndexChanged();
});
$("#ComboBox1").change(function () {
    ComboBox1_SelectedIndexChanged();
});
function ComboBox1_SelectedIndexChanged() {
    $("#BackGround").css("display", "block");
    let ComboBox1 = $("#ComboBox1").val();
    BaseParameter = {
        SearchString: ComboBox1,
    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/V01_3/ComboBox1_SelectedIndexChanged";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            BaseResult = data;
            DataGridView1Render();
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            alert(localStorage.getItem("SaveNotSuccess"));
            $("#BackGround").css("display", "none");
        })
    });
}
$("#Button1").click(function () {
    Button1_Click();
});
function Button1_Click() {
    localStorage.setItem("V01_TextBox1", $("#ComboBox1").val());
    window.close();
}
$("#Buttonclose").click(function () {
    Buttonclose_Click();
});
function Buttonclose_Click() {
    window.close();
}
function DataGridView1Render() {
    let HTML = "";
    if (BaseResult) {
        if (BaseResult.DataGridView1) {
            if (BaseResult.DataGridView1.length > 0) {
                DataGridView1_SelectionChanged(0);
                for (let i = 0; i < BaseResult.DataGridView1.length; i++) {
                    HTML = HTML + "<tr onclick='DataGridView1_SelectionChanged(" + i + ")'>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].PDP_CONF + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].PDP_NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].PDP_DATE1 + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].DEP + "</td>";
                    HTML = HTML + "</tr>";
                }
            }
        }
    }
    document.getElementById("DataGridView1").innerHTML = HTML;
}
function DataGridView1Sort() {
    IsTableSort = !IsTableSort;
    DataGridViewSort(BaseResult.DataGridView1, IsTableSort);
    DataGridView1Render();
}
function DataGridView1_SelectionChanged(i) {
    DataGridView1RowIndex = i;
    localStorage.setItem("V01_TextBox1", $("#ComboBox1").val());
    window.close();
}