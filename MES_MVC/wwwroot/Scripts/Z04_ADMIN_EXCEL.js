let IsTableSort = false;
let BaseResult = new Object();
let TagIndex = 1;
let Now;
$(document).ready(function () {
    var now = new Date();
    var day = ("0" + now.getDate()).slice(-2);
    var month = ("0" + (now.getMonth() + 1)).slice(-2);
    var today = now.getFullYear() + "-" + (month) + "-" + (day);
    Now = today;
    $('.modal').modal();
    BaseResult.DataGridView1 = new Object();
    BaseResult.DataGridView1 = [];
    document.getElementById("RadioButton1").checked = true;
    MES_CDD();
    $("#Label3").val(BaseResult.DataGridView1.length);
});

$(window).focus(function () {
}).blur(function () {
    localStorage.setItem("Z04_ADMIN_EXCEL_Close", 1);
    window.close();
});

$("#FileToUpload").change(function () {
    Buttoninport_Click();
});
$("#RadioButton1").click(function () {
    RadioButton1_Click();
});
$("#RadioButton2").click(function () {
    RadioButton2_Click();
});
$("#RadioButton3").click(function () {
    RadioButton3_Click();
});
$("#Z04_ADMIN_EXCELRadioButton3OK").click(function () {
    let INPUT_A = $("#Z04_ADMIN_EXCELRadioButtonTextBox1").val();
    let INPUT_B = $("#Z04_ADMIN_EXCELRadioButtonTextBox15").val();
    $("#TextBox1").val(INPUT_A);
    $("#TextBox15").val(INPUT_B);
    $('#Z04_ADMIN_EXCELRadioButton3').modal('close');
});

function RadioButton1_Click() {
    MES_CDD();
}
function RadioButton2_Click() {
    MES_CDD();
}
function RadioButton3_Click() {
    MES_CDD();
}

function MES_CDD() {
    let RadioButton3 = document.getElementById("RadioButton3").checked;

    if (RadioButton3 == true) {
        $('#Z04_ADMIN_EXCELRadioButton3').modal('open');
        return;
    }

    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        Action: TagIndex,
        ListSearchString: [],
    };
    BaseParameter.RadioButton1 = document.getElementById("RadioButton1").checked;
    BaseParameter.RadioButton2 = document.getElementById("RadioButton2").checked;
    BaseParameter.RadioButton3 = false;

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/Z04_ADMIN_EXCEL/MES_CDD";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {}
    }).then((response) => {
        response.json().then((data) => {
            let BaseResultMES_CDD = data;
            $("#TextBox1").val(BaseResultMES_CDD.Error);
            $("#TextBox15").val(BaseResultMES_CDD.ErrorNumber);
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            alert(localStorage.getItem("ERROR"));
            $("#BackGround").css("display", "none");
        });
    });
}

$("#Buttoninport").click(function () {
    $("#FileToUpload").click();
});
$("#Buttonsave").click(function () {
    Buttonsave_Click();
});
$("#Buttondelete").click(function () {
    Buttondelete_Click();
});
$("#Buttonclose").click(function () {
    Buttonclose_Click();
});

function Buttoninport_Click() {
    var FileToUpload = $('#FileToUpload').prop('files');
    //if (!FileToUpload || FileToUpload.length <= 0) {
    //    return;
    //}

    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {};
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    for (var i = 0; i < FileToUpload.length; i++) {
        formUpload.append('file[]', FileToUpload[i]);
    }

    let url = "/Z04_ADMIN_EXCEL/Buttoninport_Click";
    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {}
    }).then((response) => {
        response.json().then((data) => {
            BaseResult = data;
            DataGridView1Render();
            $("#Label3").val(BaseResult.DataGridView1.length);
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            alert(localStorage.getItem("ERROR"));
            $("#BackGround").css("display", "none");
        });
    });
}

function Buttonsave_Click() {
    let IsSave = true;
    if (BaseResult.DataGridView1.length <= 0) {
        IsSave = false;
    }
    let TextBox1 = $("#TextBox1").val();
    if (TextBox1 == "") {
        IsSave = false;
    }
    let TextBox15 = $("#TextBox15").val();
    if (TextBox15 == "") {
        IsSave = false;
    }
    if (IsSave == true) {
        let AA = $("#TextBox1").val();
        let BB = $("#TextBox15").val();
        $("#BackGround").css("display", "block");
        let BaseParameter = new Object();
        BaseParameter = {
            Action: TagIndex,
            ListSearchString: [],
        };
        BaseParameter.RadioButton3 = document.getElementById("RadioButton3").checked;
        BaseParameter.DataGridView1 = BaseResult.DataGridView1;
        BaseParameter.USER_ID = GetCookieValue("UserID");
        BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
        BaseParameter.ListSearchString.push(AA);
        BaseParameter.ListSearchString.push(BB);

        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        let url = "/Z04_ADMIN_EXCEL/Buttonsave_Click";

        fetch(url, {
            method: "POST",
            body: formUpload,
            headers: {}
        }).then((response) => {
            response.json().then((data) => {
                alert(localStorage.getItem("SaveSuccess"));
                window.close();
                $("#BackGround").css("display", "none");
            }).catch((err) => {
                alert(localStorage.getItem("ERROR"));
                $("#BackGround").css("display", "none");
            });
        });
    }
}

function Buttondelete_Click() {
    BaseResult.DataGridView1 = [];
    DataGridView1Render();
    $("#Label3").val(0);
}

function Buttonclose_Click() {
    window.close();
}

function DataGridView1Render() {
    let HTML = "";
    if (BaseResult) {
        if (BaseResult.DataGridView1) {
            if (BaseResult.DataGridView1.length > 0) {
                DataGridView1_DataBindingComplete();
                DataGridView1_SelectionChanged(0);
                for (let i = 0; i < BaseResult.DataGridView1.length; i++) {
                    HTML = HTML + "<tr onclick='DataGridView1_SelectionChanged(" + i + ")'>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].TSYEAR_MESNO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].TSYEAR_DEPART + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].TSYEAR_PKILOC + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].TSYEAR_INPUTER + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].TSYEAR_SERIAL_NO1 + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].TSYEAR_SERIAL_NO2 + "</td>";
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
}

function DataGridView1_DataBindingComplete() {
    if (!BaseResult.DataGridView1 || BaseResult.DataGridView1.length <= 0) return;

    for (let i = 0; i < BaseResult.DataGridView1.length; i++) {
        let MES_NO_CHK = BaseResult.DataGridView1[i].TSYEAR_MESNO;

        for (let j = 0; j < BaseResult.DataGridView1.length; j++) {
            if (j == i) continue;

            let S_MES_NO_CHK = BaseResult.DataGridView1[j].TSYEAR_MESNO;
            if (MES_NO_CHK == S_MES_NO_CHK) {
                document.getElementById("Buttonsave").disabled = true;
                alert("MES NO 중복 발생. Phát sinh trùng MES NO. (Sự cố mạng Internet)");
                return;
            }
        }
    }
    document.getElementById("Buttonsave").disabled = false;
}