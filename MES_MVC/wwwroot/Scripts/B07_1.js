let IsTableSort = false;
let BaseResult;
let DataGridView1RowIndex;
let FROM_NM;
$(window).focus(function () {
}).blur(function () {
    Buttonclose_Click();
    window.close();
});
$(document).ready(function () {
    FROM_NM = localStorage.getItem("B07_1_FROM_NM");
});
$("#TextBox1").keydown(function (e) {
    if (e.keyCode == 13) {
        Button1_Click();
    }
});
$("#Button1").click(function () {
    Button1_Click();
});
function Button1_Click() {
    $("#BackGround").css("display", "block");
    let TextBox1 = $("#TextBox1").val();
    let BaseParameter = new Object();
    BaseParameter = {
        SearchString: TextBox1,
    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/B07_1/Button1_Click";

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
            $("#BackGround").css("display", "none");
        })
    });
}
function Buttonclose_Click() {
    let TNAME = FROM_NM;
    let Label2 = $("#Label2").val();
    if (TNAME == "TAB_1") {
        localStorage.setItem("B07_TextBox1", Label2);
    }
    if (TNAME == "TAB_3") {
        localStorage.setItem("B07_Text4", Label2);
    }
}
function DataGridView1Render() {
    let HTML = "";
    if (BaseResult) {
        if (BaseResult.DataGridView1) {
            if (BaseResult.DataGridView1.length > 0) {
                DataGridView1_SelectionChanged(0);
                for (let i = 0; i < BaseResult.DataGridView1.length; i++) {
                    HTML = HTML + "<tr ondblclick='DataGridView1_CellContentDoubleClick(" + i + ")' onclick='DataGridView1_SelectionChanged(" + i + ")'>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].TC_PART_NM + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].TC_DESC + "</td>";
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
    $("#Label2").val(BaseResult.DataGridView1[DataGridView1RowIndex].TC_PART_NM);
    $("#Label3").val(BaseResult.DataGridView1[DataGridView1RowIndex].TC_DESC);
    Buttonclose_Click();    
}
function DataGridView1_CellContentDoubleClick(i) {
    DataGridView1_SelectionChanged(i);
    window.close();
}