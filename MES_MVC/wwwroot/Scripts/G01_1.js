let BaseResult;
let RowIndex;
let LOC_INDEX;
$(window).focus(function () {
}).blur(function () {
    window.close();
});
$(document).ready(function () {
    $("#Label1").val("-");
    $("#Label3").val("-");
    let G01_1_LOC_INDEX = localStorage.getItem("G01_1_LOC_INDEX");
    if (G01_1_LOC_INDEX.length == 0) {
        alert(localStorage.getItem("ERROR"));
        window.close();
    }
    else {
        LOC_INDEX = Number(G01_1_LOC_INDEX);
        if (LOC_INDEX == 1) {
            $("#Label1").val("Material inventory");
            $("#Label3").val("MT_STOCK");
        }
        if (LOC_INDEX == 2) {
            $("#Label1").val("Finished goods stock");
            $("#Label3").val("FG_STOCK");
        }
    }
    let Label1 = $("#Label1").val();
    if (Label1.length < 0) {
        alert(localStorage.getItem("ERROR"));
        window.close();
    }
});
$("#Buttonsave").click(function () {
    Buttonsave_Click();
});
$("#Buttoninport").click(function () {
    Buttoninport_Click();
});
$("#Buttonclose").click(function () {
    Buttonclose_Click();
});

function Buttonsave_Click() {
    if (BaseResult) {
        if (BaseResult.DataGridView1) {
            if (BaseResult.DataGridView1.length > 0) {
                $("#BackGround").css("display", "block");
                let BaseParameter = new Object();
                BaseParameter = {
                    SearchString: localStorage.getItem("G01_1_LOC_INDEX"),
                }
                BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
                BaseParameter.DataGridView1 = BaseResult.DataGridView1;
                let formUpload = new FormData();
                formUpload.append("BaseParameter", JSON.stringify(BaseParameter));
                let url = "/G01_1/Buttonsave_Click";

                fetch(url, {
                    method: "POST",
                    body: formUpload,
                    headers: {
                    }
                }).then((response) => {
                    response.json().then((data) => {          
                        alert(localStorage.getItem("SaveSuccess"));
                        $("#BackGround").css("display", "none");
                    }).catch((err) => {
                        alert(localStorage.getItem("ERROR"));
                        $("#BackGround").css("display", "none");
                    })
                });
            }
            else {
                alert(localStorage.getItem("ERRORNOTDATA"));
            }
        }
    }
}
function Buttoninport_Click() {
    $("#FileToUpload").click();
}
function Buttonclose_Click() {
    window.close();
}
$("#FileToUpload").change(function () {
    Buttoninport_ClickSub();
});
function Buttoninport_ClickSub() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        SearchString: localStorage.getItem("G01_1_LOC_INDEX"),
    }
    let formUpload = new FormData();
    formUpload.append("BaseParameter", JSON.stringify(BaseParameter));
    let url = "/G01_1/Buttoninport_Click";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            BaseResult = data;
            if (BaseResult) {
                if (BaseResult.DataGridView1) {
                    DataGridView1Render();
                    $("#Label57").val(BaseResult.DataGridView1.length);
                    $("#Label61").val(BaseResult.DataGridView1.length - BaseResult.CHK_PM);
                    $("#Label55").val(BaseResult.QTY_SUM);
                }
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            alert(localStorage.getItem("ERROR"));
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
                HTML += "<tbody>";
                for (let i = 0; i < BaseResult.DataGridView1.length; i++) {
                    HTML = HTML + "<tr onclick='DataGridView1_SelectionChanged(" + i + ")'>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].Description + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].GROUP + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].PART_NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].QTY + "</td>";
                    HTML = HTML + "</tr>";
                }
                HTML += "</tbody>";
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
}