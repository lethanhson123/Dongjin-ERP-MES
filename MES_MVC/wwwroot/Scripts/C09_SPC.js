let IsTableSort = false;
let BaseResult;
let DataGridView1RowIndex;
let Timer;


$(window).focus(function () {
}).blur(function () {
    Buttonclose_Click();
});
$(document).ready(function () {
    localStorage.setItem("C09_SPC_Close", 0);

    $("#INSP_TEXT").val(localStorage.getItem("C09_SPC_INSP_TEXT"));
    localStorage.setItem("C09_START_V3_SPC_EXIT", false);

    $("#RUS_01").val("");
    $("#RUS_02").val("");
    $("#MRUS").val("");
    document.getElementById("MRUS").style.backgroundColor = "gray";
    C09_SPC_Load();
});
function C09_SPC_Load() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        SearchString: $("#MCbox").val(),
    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C09_SPC/C09_SPC_Load";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            BaseResult = data;
            DataGridView1Render();
            $("#RUS_TEXT_01").val("1");
            $("#RUS_TEXT_01").val(BaseResult.Search[0].Coln2);
            $("#RUS_TEXT_02").val("1");
            $("#RUS_TEXT_02").val(BaseResult.Search[0].Coln3);

            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
$("#Buttonclose").click(function (e) {
    Buttonclose_Click();
});
function Buttonclose_Click() {
    localStorage.setItem("C09_SPC_Close", 1);
    window.close();
}
$("#Button1").click(function (e) {
    Button1_Click();
});
function Button1_Click() {
    let IsCheck = true;
    let MRUS = $("#MRUS").val();
    if (MRUS == "NG") {
        IsCheck = false;
        alert("The test result is an error.");
    }
    if (MRUS == "") {
        IsCheck = false;
        alert("The test result is an error.");
    }
    if (IsCheck == true) {
        if (MRUS == "OK") {
            let C09_START_V3_ORDER_NO_TEXT = localStorage.getItem("C09_START_V3_ORDER_NO_TEXT")
            let RUS_01 = $("#RUS_01").val();
            let RUS_02 = $("#RUS_02").val();
            let INSP_TEXT = $("#INSP_TEXT").val();
            $("#BackGround").css("display", "block");
            let BaseParameter = new Object();
            BaseParameter = {
                ListSearchString: [],
            }
            BaseParameter.USER_ID = GetCookieValue("UserID");
            BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
            BaseParameter.ListSearchString.push(C09_START_V3_ORDER_NO_TEXT);
            BaseParameter.ListSearchString.push(RUS_01);
            BaseParameter.ListSearchString.push(RUS_02);
            BaseParameter.ListSearchString.push(INSP_TEXT);
            BaseParameter.ListSearchString.push(MRUS);
            let formUpload = new FormData();
            formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
            let url = "/C09_SPC/Button1_Click";

            fetch(url, {
                method: "POST",
                body: formUpload,
                headers: {
                }
            }).then((response) => {
                response.json().then((data) => {
                    localStorage.setItem("C09_START_V3_SPC_EXIT", true);
                    $("#BackGround").css("display", "none");
                }).catch((err) => {
                    $("#BackGround").css("display", "none");
                })
            });
        }
    }
}
$("#RUS_01").change(function (e) {
    TextBox10_TextChanged();
});
function TextBox10_TextChanged() {
    let RUS_01 = $("#RUS_01").val();
    if (RUS_01 == "") {

    }
    else {
        Form_RUS();
    }
}
$("#RUS_02").change(function (e) {
    TextBox1_TextChanged();
});
function TextBox1_TextChanged() {
    let RUS_02 = $("#RUS_02").val();
    if (RUS_02 == "") {

    }
    else {
        Form_RUS();
    }
}
function Form_RUS() {
    let Val1 = false;
    let Val2 = false;
    let RUS_TEXT_01 = Number($("#RUS_TEXT_01").val());
    let RUS_01 = Number($("#RUS_01").val());
    if (RUS_TEXT_01 < RUS_01) {
        Val1 = true;
    }
    let RUS_TEXT_02 = Number($("#RUS_TEXT_02").val());
    let RUS_02 = Number($("#RUS_02").val());
    if (RUS_TEXT_02 < RUS_02) {
        Val2 = true;
    }
    if (Val1 == true && Val2 == true) {
        $("#MRUS").val("OK");
        document.getElementById("MRUS").style.backgroundColor = "lime";
    }
    else {
        $("#MRUS").val("NG");
        document.getElementById("MRUS").style.backgroundColor = "red";
    }
}
$("#RUS_01").keydown(function (e) {
    if (e.keyCode == 13) {
        RUS_01_KeyDown();
    }
});
function RUS_01_KeyDown() {
    $("#RUS_02").focus();
}
$("#RUS_02").keydown(function (e) {
    if (e.keyCode == 13) {
        RUS_02_KeyDown();
    }
});
function RUS_02_KeyDown() {
    $("#Button1").focus();
}
function DataGridView1Render() {
    let HTML = "";
    if (BaseResult) {
        if (BaseResult.DataGridView1) {
            if (BaseResult.DataGridView1.length > 0) {
                DataGridView1_SelectionChanged(0);
                for (let i = 0; i < BaseResult.DataGridView1.length; i++) {
                    HTML = HTML + "<tr onclick='DataGridView1_SelectionChanged(" + i + ")'>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].LEAD_PN + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].W_Diameter + "</td>";
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
let DataGridView1Table = document.getElementById("DataGridView1Table");
DataGridView1Table.addEventListener('click', function (event) {
    if (event.target.tagName === 'TH') {
        let text = event.target.innerText;
        let key = "LEAD_PN";
        IsTableSort = !IsTableSort;
        ListSort(BaseResult.DataGridView1, key, text, IsTableSort);
        DataGridView1Render();
    }
});
