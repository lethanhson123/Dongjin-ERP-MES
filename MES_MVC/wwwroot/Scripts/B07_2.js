let IsTableSort = false;
let BaseResult;
let DataGridView1RowIndex;
let FROM_NM;
$(window).focus(function () {
}).blur(function () {
    window.close();
});
$(document).ready(function () {
});
$("#TextID").keydown(function (e) {
    if (e.keyCode == 13) {
        Button1_Click();
    }
});
$("#Button1").click(function () {
    Button1_Click();
});
$("#Button2").click(function () {
    Button2_Click();
});
function Button2_Click() {
    window.close();
}
$("#Button3").click(function () {
    Button3_Click();
});
function Button3_Click() {
    let AAA = BaseResult.DataGridView1[DataGridView1RowIndex].PART_IDX;
    let BBB = BaseResult.DataGridView1[DataGridView1RowIndex].PART_NO;
    let CCC = BaseResult.DataGridView1[DataGridView1RowIndex].PART_NM;
    localStorage.setItem("B07_TextBox5", BBB);
    window.close();
}
function Button1_Click() {
    $("#BackGround").css("display", "block");
    let AAA = $("#TextBox1").val();
    $("#Label2").val("");
    $("#Label1").val("");
    $("#Label3").val("");
    let BaseParameter = new Object();
    BaseParameter = {
        SearchString: AAA,
    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/B07_2/Button1_Click";

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
function DataGridView1Render() {
    let HTML = "";
    if (BaseResult) {
        if (BaseResult.DataGridView1) {
            if (BaseResult.DataGridView1.length > 0) {
                DataGridView1_SelectionChanged(0);
                for (let i = 0; i < BaseResult.DataGridView1.length; i++) {
                    HTML = HTML + "<tr ondblclick='DataGridView1_CellDoubleClick(" + i + ")' onclick='DataGridView1_SelectionChanged(" + i + ")'>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].PART_IDX + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].PART_NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].PART_NM + "</td>";
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
    let AAA = BaseResult.DataGridView1[DataGridView1RowIndex].PART_IDX;
    let BBB = BaseResult.DataGridView1[DataGridView1RowIndex].PART_NO;
    let CCC = BaseResult.DataGridView1[DataGridView1RowIndex].PART_NM;
    $("#Label2").val(BBB);
    $("#Label1").val(AAA);
    $("#Label3").val(CCC);
}
function DataGridView1_CellDoubleClick(i) {
    DataGridView1RowIndex = i;
    Button3_Click();
}