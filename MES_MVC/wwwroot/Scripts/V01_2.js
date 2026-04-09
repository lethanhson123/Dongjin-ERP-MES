let BaseResult;
let DataGridView1RowIndex;

$(window).focus(function () {
}).blur(function () {
    window.close();
});

$(document).ready(function () {

});

$("#TextID").keydown(function (e) {
    if (e.keyCode == 13) {
        TextID_KeyDown();
    }
});
function TextID_KeyDown() {
    Button1_Click();
}
$("#Button1").click(function () {
    Button1_Click();
});
function Button1_Click() {
    let AAA = $("#TextID").val();
    $("#Label2").val("");
    $("#Label1").val("");
    $("#Label3").val("");
    $("#BackGround").css("display", "block");
  
    let BaseParameter = new Object();
    BaseParameter = {       
        SearchString: AAA,
    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/V01_2/Button1_Click";

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
            alert(localStorage.setItem("ERROR"));
            $("#BackGround").css("display", "none");
        })
    });
}
$("#Button3").click(function () {
    Button3_Click();
});
function Button3_Click() {
    let AAA = BaseResult.DataGridView1[DataGridView1RowIndex].CMPNY_IDX;
    let BBB = BaseResult.DataGridView1[DataGridView1RowIndex].CMPNY_NM;
    let CCC = BaseResult.DataGridView1[DataGridView1RowIndex].CMPNY_DVS;
    let DDD = BaseResult.DataGridView1[DataGridView1RowIndex].CMPNY_ADDR;
    let EEE = BaseResult.DataGridView1[DataGridView1RowIndex].CMPNY_TEL;
    let FFF = BaseResult.DataGridView1[DataGridView1RowIndex].CMPNY_MNGR;

    
    localStorage.setItem("V01_TextBox15", BBB);
    localStorage.setItem("V01_DGV_CMP", BBB);
    localStorage.setItem("V01_DGV_CMP_IDX", AAA);
    localStorage.setItem("V01_DGV_ADDR", DDD);
    localStorage.setItem("V01_DGV_TEL", EEE);
    localStorage.setItem("V01_DGV_Manager", FFF);
    Button2_Click();
}
$("#Button2").click(function () {
    Button2_Click();
});
function Button2_Click() {
    localStorage.setItem("V01_2_Close", 1);
    window.close();
}
function DataGridView1Render() {
    let HTML = "";
    if (BaseResult) {
        if (BaseResult.DataGridView1) {
            if (BaseResult.DataGridView1.length > 0) {
                DataGridView1_SelectionChanged(0);
                for (let i = 0; i < BaseResult.DataGridView1.length; i++) {
                    HTML = HTML + "<tr ondblclick='DataGridView1_CellDoubleClick(" + i + ")' onclick='DataGridView1_SelectionChanged(" + i + ")'>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].CMPNY_IDX + "</td>";
                    HTML = HTML + "<td>" + NameShortByName(BaseResult.DataGridView1[i].CMPNY_NM) + "</td>";
                    HTML = HTML + "<td>" + NameShortByName(BaseResult.DataGridView1[i].CMPNY_DVS) + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].CMPNY_NO + "</td>";
                    HTML = HTML + "<td>" + NameShortByName(BaseResult.DataGridView1[i].CMPNY_ADDR) + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].CMPNY_TEL + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].CMPNY_FAX + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].CMPNY_MNGR + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].CMPNY_RMK + "</td>";
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
    let AAA = BaseResult.DataGridView1[DataGridView1RowIndex].CMPNY_IDX;
    let BBB = BaseResult.DataGridView1[DataGridView1RowIndex].CMPNY_NM;
    let CCC = BaseResult.DataGridView1[DataGridView1RowIndex].CMPNY_DVS;

    $("#Label2").val(BBB);
    $("#Label1").val(AAA);
    $("#Label3").val(CCC);
}
function DataGridView1_CellDoubleClick(i) {
    DataGridView1RowIndex = i;
    Button3_Click();
}