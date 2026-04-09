let IsTableSort = false;
let BaseResult = new Object();
let TagIndex = 1;
let DataGridView1RowIndex = 0;
let DataGridView2RowIndex = 0;
let DataGridView3RowIndex = 0;
let DGV_01RowIndex = 0;
let Now;
let IntervalTimer;

$(document).ready(function () {
    $('.modal').modal();
    var now = new Date();
    var day = ("0" + now.getDate()).slice(-2);
    var month = ("0" + (now.getMonth() + 1)).slice(-2);
    var today = now.getFullYear() + "-" + (month) + "-" + (day);
    Now = today;
    $("#DateTimePicker1").val(today);
    document.getElementById("TBA01").readOnly = true;
    $("#TBA08").val("-");
    document.getElementById("RadioButton4").checked = true;
    document.getElementById("RadioButton14").checked = true;
    document.getElementById("RadioButton7").checked = true;
    document.getElementById("RadioButton8").checked = true;
    document.getElementById("RadioButton16").checked = true;

    localStorage.setItem("V01_DGV_Manager", "");
    localStorage.setItem("V01_DGV_TEL", "");
    localStorage.setItem("V01_DGV_ADDR", "");

    BaseResult.DataGridView3 = new Object();
    BaseResult.DataGridView3 = [];
});
$("#ATag001").click(function (e) {
    TagIndex = 1;
});
$("#ATag002").click(function (e) {
    TagIndex = 2;
});
$("#ATag003").click(function (e) {
    TagIndex = 3;
});
$("#ATag004").click(function (e) {
    TagIndex = 4;
});
$("#PART_ADD04").keypress(function (e) {
    PART_ADD01_KeyPress();
});
$("#PART_ADD03").keypress(function (e) {
    PART_ADD01_KeyPress();
});
$("#PART_ADD02").keypress(function (e) {
    PART_ADD01_KeyPress();
});
$("#PART_ADD01").keypress(function (e) {
    PART_ADD01_KeyPress();
});
function PART_ADD01_KeyPress() {

}
$("#PART_ADD07").keypress(function (e) {
    PART_ADD08_KeyPress();
});
function PART_ADD08_KeyPress() {

}
$("#TextBox15").click(function (e) {
    TextBox15_Click();
});
function TextBox15_Click() {
    let url = "/V01_2";
    OpenWindowByURL(url, 800, 600);
    localStorage.setItem("V01_2_Close", 0);
    StartInterval();
}
function StartInterval() {
    IntervalTimer = setInterval(function () {
        let IsClose = localStorage.getItem("V01_2_Close");
        if (IsClose == 1) {
            clearInterval(IntervalTimer);
            IsClose = 0;
            localStorage.setItem("V01_2_Close", IsClose);
            let BBB = localStorage.getItem("V01_TextBox15");
            $("#TextBox15").val(BBB);
            Buttonfind_Click();
        }
    }, 100);
}

$("#Button1").click(function (e) {
    Button1_Click();
});
function Button1_Click() {
    let IsCheck = true;
    let CHK_T1 = false;
    localStorage.setItem("V01_1_URL_PHOTO", "");
    localStorage.setItem("V01_1_Label3", "");
    localStorage.setItem("V01_1_Label2", "");
    localStorage.setItem("V01_1_Label4", "");
    localStorage.setItem("V01_1_Label8", "");
    let PART_ADD05 = $("#PART_ADD05").val();
    if (PART_ADD05 == "") {
        IsCheck = false;
    }
    if (PART_ADD05 == "DJG-???") {
        IsCheck = false;
    }
    if (IsCheck == true) {
        let PART_ADD08 = $("#PART_ADD08").val();
        if (PART_ADD08 == "") {
            let URL_PHOTO = "http://113.161.129.118:5240/MES_DATE/PHOTO/V00/DJG_IMG_NULL.png";
            localStorage.setItem("V01_1_Label3", $("#PART_ADD05").val());
            localStorage.setItem("V01_1_Label2", $("#PART_ADD08").val());
            localStorage.setItem("V01_1_URL_PHOTO", URL_PHOTO);
            localStorage.setItem("V01_1_CHK_V", "V00");
            let url = "/V01_1";
            OpenWindowByURL(url, 600, 200);
        }
        else {
            let PART_ADD08 = $("#PART_ADD08").val();
            let URL_PHOTO = "http://113.161.129.118:5240/MES_DATE/PHOTO/V00/" + PART_ADD08;
            localStorage.setItem("V01_1_Label3", $("#PART_ADD05").val());
            localStorage.setItem("V01_1_Label2", $("#PART_ADD08").val());
            localStorage.setItem("V01_1_URL_PHOTO", URL_PHOTO);
            localStorage.setItem("V01_1_CHK_V", "V00");
            let url = "/V01_1";
            OpenWindowByURL(url, 600, 200);
        }
        Buttonfind_Click();
    }
}
$("#Button2").click(function (e) {
    Button2_Click();
});
function Button2_Click() {
    let CHK_T1 = false;
    localStorage.setItem("V01_1_URL_PHOTO", "");
    localStorage.setItem("V01_1_Label3", "");
    localStorage.setItem("V01_1_Label2", "");
    localStorage.setItem("V01_1_Label4", "");
    localStorage.setItem("V01_1_Label8", "");
    let T2_PN_NM = $("#T2_PN_NM").val();
    if (T2_PN_NM == "") {
        IsCheck = false;
    }
    if (T2_PN_NM == "DJG-???") {
        IsCheck = false;
    }
    if (IsCheck == true) {
        let T2_PHOTO = $("#T2_PHOTO").val();
        if (T2_PHOTO == "") {
            let URL_PHOTO = "http://113.161.129.118:5240/MES_DATE/PHOTO/V01/DJG_IMG_NULL.png";
            localStorage.setItem("V01_1_Label3", $("#T2_PN_NM").val());
            localStorage.setItem("V01_1_Label2", $("#T2_PHOTO").val());
            localStorage.setItem("V01_1_URL_PHOTO", URL_PHOTO);
            localStorage.setItem("V01_1_CHK_V", "V01");
            let url = "/V01_1";
            OpenWindowByURL(url, 600, 200);
        }
        else {
            let T2_PHOTO = $("#T2_PHOTO").val();
            let URL_PHOTO = "http://113.161.129.118:5240/MES_DATE/PHOTO/V01/" + PART_ADD08;
            localStorage.setItem("V01_1_Label3", $("#T2_PN_NM").val());
            localStorage.setItem("V01_1_Label2", $("#T2_PHOTO").val());
            localStorage.setItem("V01_1_URL_PHOTO", URL_PHOTO);
            localStorage.setItem("V01_1_CHK_V", "V01");
            let url = "/V01_1";
            OpenWindowByURL(url, 600, 200);
        }
        Buttonfind_Click();
    }
}
$("#Button3").click(function (e) {
    Button3_Click();
});
function Button3_Click() {
    let IsCheck = true;
    let TextBox15 = $("#TextBox15").val();
    if (TextBox15 == "") {
        alert(localStorage.getItem("Notification_V01_009"));
        IsCheck = false;
    }
    if (BaseResult.DataGridView3.length <= 0) {
        alert("Data 오류가 발생 하였습니다. Data Một lỗi đã xảy ra.");
        IsCheck = false;
    }
    if (IsCheck == true) {
        $("#BackGround").css("display", "block");
        let DGV_Manager = localStorage.getItem("V01_DGV_Manager");
        let DGV_TEL = localStorage.getItem("V01_DGV_TEL");
        let DGV_ADDR = localStorage.getItem("V01_DGV_ADDR");
        let BaseParameter = new Object();
        BaseParameter = {
            Action: TagIndex,
            ListSearchString: [],
        }
        BaseParameter.USER_ID = GetCookieValue("UserID");
        BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
        BaseParameter.ListSearchString.push(TextBox15);
        BaseParameter.ListSearchString.push(DGV_Manager);
        BaseParameter.ListSearchString.push(DGV_TEL);
        BaseParameter.ListSearchString.push(DGV_ADDR);
        BaseParameter.V01_DMPS_COST_DataGridView3 = BaseResult.DataGridView3;
        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        let url = "/V01/Button3_Click";

        fetch(url, {
            method: "POST",
            body: formUpload,
            headers: {
            }
        }).then((response) => {
            response.json().then((data) => {
                let BaseResultButton3 = data;
                if (BaseResultButton3) {
                    if (BaseResultButton3.Code) {
                        let url = BaseResultButton3.Code;
                        window.location.href = url;
                    }
                }
                $("#BackGround").css("display", "none");
            }).catch((err) => {
                alert(localStorage.setItem("ERRORAnErrorOccurred"));
                $("#BackGround").css("display", "none");
            })
        });
    }
}
$("#RadioButton1").click(function () {
    RadioButton1_CheckedChanged();
});
$("#RadioButton2").click(function () {
    RadioButton1_CheckedChanged();
});
$("#RadioButton3").click(function () {
    RadioButton1_CheckedChanged();
});
function RadioButton1_CheckedChanged() {
    DataGridView1Render();
}
$("#RadioButton12").click(function () {
    RadioButton16_CheckedChanged();
});
$("#RadioButton15").click(function () {
    RadioButton16_CheckedChanged();
});
$("#RadioButton16").click(function () {
    RadioButton16_CheckedChanged();
});
function RadioButton16_CheckedChanged() {
    DataGridView3Render();

}
$("#T2_PN_K").keypress(function (e) {
    T2_PN_K_KeyPress();
});
function T2_PN_K_KeyPress() {
}
$("#T2_PSPEC_V").keypress(function (e) {
    T2_PSPEC_V_KeyPress();
});
function T2_PSPEC_V_KeyPress() {
}
$("#T2_PN_V").keypress(function (e) {
    T2_PN_V_KeyPress();
});
function T2_PN_V_KeyPress() {
}
$("#T2_PSPEC_K").keypress(function (e) {
    T2_PSPEC_K_KeyPress();
});
function T2_PSPEC_K_KeyPress() {
}
$("#SUCH1").keydown(function (e) {
    if (e.keyCode == 13) {
        SUCH1_KeyDown();
    }
});
function SUCH1_KeyDown() {
    Buttonfind_Click();
}
$("#SUCH2").keydown(function (e) {
    if (e.keyCode == 13) {
        SUCH2_KeyDown();
    }
});
function SUCH2_KeyDown() {
    Buttonfind_Click();
}
$("#SUCH3").keydown(function (e) {
    if (e.keyCode == 13) {
        SUCH3_KeyDown();
    }
});
function SUCH3_KeyDown() {
    Buttonfind_Click();
}
$("#TBA01").keypress(function (e) {
    TBA01_KeyPress();
});
$("#TBA02").keypress(function (e) {
    TBA01_KeyPress();
});
$("#TBA03").keypress(function (e) {
    TBA01_KeyPress();
});
$("#TBA04").keypress(function (e) {
    TBA01_KeyPress();
});
$("#TBA05").keypress(function (e) {
    TBA01_KeyPress();
});
function TBA01_KeyPress() {
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
    if (TagIndex == 1) {
        let S_AAA = $("#TextBoxS1").val();
        let S_CCC = $("#TextBoxS3").val();
        let S_DDD = "";
        let RadioButton7 = document.getElementById("RadioButton7").checked;
        if (RadioButton7 == true) {
            S_DDD = "Y"
        }
        let RadioButton6 = document.getElementById("RadioButton6").checked;
        if (RadioButton6 == true) {
            S_DDD = "N"
        }
        $("#BackGround").css("display", "block");
        let BaseParameter = new Object();
        BaseParameter = {
            Action: TagIndex,
            ListSearchString: [],
        }
        BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
        BaseParameter.RadioButton7 = document.getElementById("RadioButton7").checked;
        BaseParameter.RadioButton6 = document.getElementById("RadioButton6").checked;
        BaseParameter.ListSearchString.push(S_AAA);
        BaseParameter.ListSearchString.push(S_CCC);
        BaseParameter.ListSearchString.push(S_DDD);
        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        let url = "/V01/Buttonfind_Click";

        fetch(url, {
            method: "POST",
            body: formUpload,
            headers: {
            }
        }).then((response) => {
            response.json().then((data) => {
                document.getElementById("RadioButton1").checked = true;
                let BaseResultButtonfind = data;
                BaseResult.DataGridView1 = BaseResultButtonfind.DataGridView1;
                DataGridView1Render();
                $("#BackGround").css("display", "none");
            }).catch((err) => {
                $("#BackGround").css("display", "none");
            })
        });
    }
    if (TagIndex == 2) {
        let S_AAA = $("#TextBox2").val();
        let S_BBB = $("#TextBox1").val();
        let S_FFF = $("#TextBox10").val();
        let S_DDD = "";
        let RadioButton8 = document.getElementById("RadioButton8").checked;
        if (RadioButton8 == true) {
            S_DDD = "Y"
        }
        let RadioButton9 = document.getElementById("RadioButton9").checked;
        if (RadioButton9 == true) {
            S_DDD = "N"
        }
        $("#BackGround").css("display", "block");
        let BaseParameter = new Object();
        BaseParameter = {
            Action: TagIndex,
            ListSearchString: [],
        }
        BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
        BaseParameter.RadioButton8 = document.getElementById("RadioButton8").checked;
        BaseParameter.RadioButton9 = document.getElementById("RadioButton9").checked;
        BaseParameter.ListSearchString.push(S_AAA);
        BaseParameter.ListSearchString.push(S_BBB);
        BaseParameter.ListSearchString.push(S_FFF);
        BaseParameter.ListSearchString.push(S_DDD);
        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        let url = "/V01/Buttonfind_Click";

        fetch(url, {
            method: "POST",
            body: formUpload,
            headers: {
            }
        }).then((response) => {
            response.json().then((data) => {
                let BaseResultButtonfind = data;
                BaseResult.DataGridView2 = BaseResultButtonfind.DataGridView2;
                DataGridView2Render();
                document.getElementById("RadioButton1").checked = true;
                $("#BackGround").css("display", "none");
            }).catch((err) => {
                $("#BackGround").css("display", "none");
            })
        });
    }
    if (TagIndex == 3) {
        let AAA = $("#SUCH1").val();
        let BBB = $("#SUCH2").val();
        let CCC = $("#SUCH3").val();
        document.getElementById("TBA01").readOnly = true;
        $("#TBA08").val("-");
        $("#BackGround").css("display", "block");
        let BaseParameter = new Object();
        BaseParameter = {
            Action: TagIndex,
            ListSearchString: [],
        }
        BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
        BaseParameter.RadioButton7 = document.getElementById("RadioButton7").checked;
        BaseParameter.RadioButton6 = document.getElementById("RadioButton6").checked;
        BaseParameter.ListSearchString.push(AAA);
        BaseParameter.ListSearchString.push(BBB);
        BaseParameter.ListSearchString.push(CCC);
        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        let url = "/V01/Buttonfind_Click";

        fetch(url, {
            method: "POST",
            body: formUpload,
            headers: {
            }
        }).then((response) => {
            response.json().then((data) => {
                let BaseResultButtonfind = data;
                BaseResult.DGV_01 = BaseResultButtonfind.DGV_01;
                DGV_01Render();
                $("#BackGround").css("display", "none");
            }).catch((err) => {
                $("#BackGround").css("display", "none");
            })
        });
    }
    if (TagIndex == 4) {
        let S_AAA = $("#TextBox3").val();
        let S_BBB = $("#TextBox15").val();
        let S_CCC = $("#TextBox4").val();
        let CM = document.getElementById("ComboBox1").selectedIndex;
        $("#BackGround").css("display", "block");
        let BaseParameter = new Object();
        BaseParameter = {
            Action: TagIndex,
            ListSearchString: [],
        }
        BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
        BaseParameter.ListSearchString.push(S_AAA);
        BaseParameter.ListSearchString.push(S_BBB);
        BaseParameter.ListSearchString.push(S_CCC);
        BaseParameter.ListSearchString.push(CM);
        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        let url = "/V01/Buttonfind_Click";

        fetch(url, {
            method: "POST",
            body: formUpload,
            headers: {
            }
        }).then((response) => {
            response.json().then((data) => {
                let BaseResultButtonfind = data;
                BaseResult.DataGridView3 = BaseResultButtonfind.V01_DMPS_COST_DataGridView3;
                DataGridView3Render();
                document.getElementById("RadioButton16").checked = true;
                $("#BackGround").css("display", "none");
            }).catch((err) => {
                $("#BackGround").css("display", "none");
            })
        });
    }
}
function Buttonadd_Click() {
    if (TagIndex == 1) {
        Buttoncancel_Click();
        $("#PART_ADD05").val("DJG-???");
    }
    if (TagIndex == 2) {
        Buttoncancel_Click();
        $("#T2_PN_NM").val("DJGMC-???");
    }
    if (TagIndex == 3) {
        $("#TBA01").val("");
        $("#TBA02").val("");
        $("#TBA03").val("");
        $("#TBA04").val("");
        $("#TBA05").val("");
        $("#TBA06").val("");
        $("#TBA07").val("");
        $("#TBA08").val("New");
        document.getElementById("TBA01").readOnly = false;
    }
}
function Buttonsave_Click() {
    if (TagIndex == 1) {
        let IsSave = false;
        let PART_ADD01 = $("#PART_ADD01").val();
        if (PART_ADD01.length > 0) {
            IsSave = true;
        }
        else {
            IsSave = false;
            alert(localStorage.getItem("Notification_V01_001"));
        }
        let PART_ADD03 = $("#PART_ADD03").val();
        if (PART_ADD03.length > 0) {
            IsSave = true;
        }
        else {
            IsSave = false;
            alert(localStorage.getItem("Notification_V01_002"));
        }
        let PART_ADD07 = Number($("#PART_ADD07").val());
        if (PART_ADD07 > 0) {
            IsSave = true;
        }
        else {
            IsSave = false;
            alert(localStorage.getItem("Notification_V01_003"));
        }
        let PART_ADD05 = $("#PART_ADD05").val();
        if (PART_ADD05.length >= 6) {
            IsSave = true;
        }
        else {
            IsSave = false;
            alert(localStorage.getItem("Notification_V01_004"));
        }
        if (IsSave == true) {
            $("#BackGround").css("display", "block");
            let BaseParameter = new Object();
            BaseParameter = {
                Action: TagIndex,
                ListSearchString: [],
            }
            BaseParameter.USER_ID = GetCookieValue("UserID");
            BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
            BaseParameter.RadioButton4 = document.getElementById("RadioButton4").checked;
            BaseParameter.RadioButton5 = document.getElementById("RadioButton5").checked;
            BaseParameter.ListSearchString.push($("#PART_ADD01").val());
            BaseParameter.ListSearchString.push($("#PART_ADD02").val());
            BaseParameter.ListSearchString.push($("#PART_ADD03").val());
            BaseParameter.ListSearchString.push($("#PART_ADD04").val());
            BaseParameter.ListSearchString.push($("#PART_ADD05").val());
            BaseParameter.ListSearchString.push($("#PART_ADD06").val());
            BaseParameter.ListSearchString.push($("#PART_ADD07").val());
            BaseParameter.ListSearchString.push($("#PART_ADD08").val());
            let formUpload = new FormData();
            formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
            let url = "/V01/Buttonsave_Click";

            fetch(url, {
                method: "POST",
                body: formUpload,
                headers: {
                }
            }).then((response) => {
                response.json().then((data) => {
                    DataGridView1RowIndex = 0;
                    Buttonfind_Click();
                    DataGridView1_SelectionChanged(DataGridView1RowIndex);
                    alert(localStorage.getItem("SaveSuccess"));
                    $("#BackGround").css("display", "none");
                }).catch((err) => {
                    Buttonfind_Click();
                    alert(localStorage.getItem("SaveNotSuccess"));
                    $("#BackGround").css("display", "none");
                })
            });
        }
    }
    if (TagIndex == 2) {
        let IsSave = false;
        let T2_PN_K = $("#T2_PN_K").val();
        if (T2_PN_K.length > 0) {
            IsSave = true;
        }
        else {
            IsSave = false;
            alert(localStorage.getItem("Notification_V01_005"));
        }
        let T2_PSPEC_K = $("#T2_PSPEC_K").val();
        if (T2_PSPEC_K.length > 0) {
            IsSave = true;
        }
        else {
            IsSave = false;
            alert(localStorage.getItem("Notification_V01_006"));
        }
        let T2_PSPEC_V = $("#T2_PSPEC_V").val();
        if (T2_PSPEC_V.length > 0) {
            IsSave = true;
        }
        else {
            IsSave = false;
            alert(localStorage.getItem("Notification_V01_007"));
        }
        if (IsSave == true) {
            $("#BackGround").css("display", "block");
            let BaseParameter = new Object();
            BaseParameter = {
                Action: TagIndex,
                ListSearchString: [],
            }
            BaseParameter.USER_ID = GetCookieValue("UserID");
            BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
            BaseParameter.RadioButton13 = document.getElementById("RadioButton13").checked;
            BaseParameter.RadioButton14 = document.getElementById("RadioButton14").checked;
            BaseParameter.ListSearchString.push($("#T2_PN_V").val());
            BaseParameter.ListSearchString.push($("#T2_PSPEC_V").val());
            BaseParameter.ListSearchString.push($("#T2_PN_K").val());
            BaseParameter.ListSearchString.push($("#T2_PSPEC_K").val());
            BaseParameter.ListSearchString.push($("#T2_PN_NM").val());
            BaseParameter.ListSearchString.push($("#T2_PHOTO").val());
            let formUpload = new FormData();
            formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
            let url = "/V01/Buttonsave_Click";

            fetch(url, {
                method: "POST",
                body: formUpload,
                headers: {
                }
            }).then((response) => {
                response.json().then((data) => {
                    DataGridView1RowIndex = 0;
                    Buttonfind_Click();
                    DataGridView1_SelectionChanged(DataGridView1RowIndex);
                    alert(localStorage.getItem("SaveSuccess"));
                    $("#BackGround").css("display", "none");
                }).catch((err) => {
                    Buttonfind_Click();
                    alert(localStorage.getItem("SaveNotSuccess"));
                    $("#BackGround").css("display", "none");
                })
            });
        }
    }
    if (TagIndex == 3) {
        let IsSave = false;
        let TBA01 = $("#TBA01").val();
        if (TBA01.length > 0) {
            IsSave = true;
        }
        else {
            IsSave = false;
            alert(localStorage.getItem("Notification_V01_008"));
        }
        let TBA02 = $("#TBA02").val();
        if (TBA02.length > 0) {
            IsSave = true;
        }
        else {
            IsSave = false;
            alert(localStorage.getItem("Notification_V01_008"));
        }
        if (IsSave == true) {
            document.getElementById("Buttonsave").disabled = true;
            $("#BackGround").css("display", "block");
            let BaseParameter = new Object();
            BaseParameter = {
                Action: TagIndex,
                ListSearchString: [],
            }
            BaseParameter.USER_ID = GetCookieValue("UserID");
            BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
            BaseParameter.ListSearchString.push($("#TBA08").val());
            BaseParameter.ListSearchString.push($("#TBA01").val());
            BaseParameter.ListSearchString.push($("#TBA02").val());
            BaseParameter.ListSearchString.push($("#TBA03").val());
            BaseParameter.ListSearchString.push($("#TBA06").val());
            BaseParameter.ListSearchString.push($("#TBA07").val());
            BaseParameter.ListSearchString.push($("#TBA04").val());
            BaseParameter.ListSearchString.push($("#TBA05").val());
            BaseParameter.ListSearchString.push($("#TBA09").val());
            let formUpload = new FormData();
            formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
            let url = "/V01/Buttonsave_Click";

            fetch(url, {
                method: "POST",
                body: formUpload,
                headers: {
                }
            }).then((response) => {
                response.json().then((data) => {
                    document.getElementById("TBA01").readOnly = true;
                    alert(localStorage.getItem("SaveSuccess"));
                    $("#BackGround").css("display", "none");
                }).catch((err) => {
                    alert(localStorage.getItem("SaveNotSuccess"));
                    $("#BackGround").css("display", "none");
                })
            });
            document.getElementById("Buttonsave").disabled = false;
        }
    }
}
function Buttondelete_Click() {

}
function Buttoncancel_Click() {
    if (TagIndex == 1) {
        $("#PART_ADD01").val("");
        $("#PART_ADD02").val("");
        $("#PART_ADD03").val("");
        $("#PART_ADD04").val("");
        $("#PART_ADD05").val("");
        $("#PART_ADD08").val("");
        $("#PART_ADD07").val("");
        document.getElementById("PART_ADD06").selectedIndex = "0";
    }
    if (TagIndex == 2) {
        $("#T2_PN_K").val("");
        $("#T2_PSPEC_V").val("");
        $("#T2_PSPEC_K").val("");
        $("#T2_PN_V").val("");
        $("#T2_PN_NM").val("");
        $("#T2_PHOTO").val("");
        $("#TAB4_TB01").val("");
        $("#TAB4_TB02").val("");
        $("#TAB4_TB03").val("");
        $("#TAB4_TB04").val("");
        $("#TextBox6").val("");
        $("#TAB4_TB00").val("");
        $("#TextBox14").val("");
        $("#TextBox5").val("");
        $("#DateTimePicker1").val(Now);
        document.getElementById("PictureBox1").src = "";
    }
    if (TagIndex == 3) {
        $("#TBA01").val("");
        $("#TBA02").val("");
        $("#TBA03").val("");
        $("#TBA04").val("");
        $("#TBA05").val("");
        $("#TBA06").val("");
        $("#TBA07").val("");
        $("#TBA08").val("-");

        document.getElementById("TBA01").readOnly = false;
    }
    if (TagIndex == 4) {
        $("#TextBox15").val("");
    }
}
function Buttoninport_Click() {
    if (TagIndex == 1) {
        $("#ModalImportNameKR").modal('open');
        $("#ImportPreviewTable").empty();
        $("#FileImportNameKR").val('');
    }
    else if (TagIndex == 4) {
        let IsImport = true;
        let TextBox15 = $("#TextBox15").val();
        if (TextBox15 == "") {
            IsImport = false;
            alert(localStorage.getItem("Notification_V01_009"));
        }
        if (IsImport == true) {
            $("#FileToUpload").click();

        }
    }
}
$("#FileImportNameKR").change(function () {
    var file = this.files[0];
    if (file) {
        var reader = new FileReader();
        reader.onload = function (e) {
            try {
                var data = new Uint8Array(e.target.result);
                var workbook = XLSX.read(data, { type: 'array' });
                var firstSheet = workbook.SheetNames[0];
                var worksheet = workbook.Sheets[firstSheet];
                var jsonData = XLSX.utils.sheet_to_json(worksheet);
                var html = '';
                var uniqueData = {};
                jsonData.forEach(function (row) {
                    if (row.P_NAME_VN && row.P_NAME_KR) {
                        uniqueData[row.P_NAME_VN] = row.P_NAME_KR;
                    }
                });
                Object.keys(uniqueData).forEach(function (key) {
                    html += '<tr>';
                    html += '<td>' + key + '</td>';
                    html += '<td>' + uniqueData[key] + '</td>';
                    html += '</tr>';
                });

                if (html === '') {
                    html = '<tr><td colspan="2">No valid data found. Make sure your file has P_NAME_VN and P_NAME_KR columns.</td></tr>';
                }

                $("#ImportPreviewTable").html(html);
            } catch (e) {
                console.error(e);
                alert("Error reading file. Make sure it's a valid Excel file.");
            }
        };
        reader.readAsArrayBuffer(file);
    }
});

$("#BtnSaveImport").click(function () {
    var rows = $("#ImportPreviewTable tr");
    $("#BackGround").css("display", "block");
    var importData = [];
    rows.each(function () {
        var cells = $(this).find("td");
        if (cells.length >= 2) {
            importData.push({
                PN_V: $(cells[0]).text(),
                PN_K: $(cells[1]).text()
            });
        }
    });
    var BaseParameter = new Object();
    BaseParameter = {
        Action: 1,
        ImportData: importData
    };
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));

    let url = "/V01/UpdateKoreanNames";

    fetch(url, {
        method: "POST",
        body: formUpload
    })
        .then(function (response) {
            if (!response.ok) {
                throw new Error("Network response was not ok");
            }
            return response.json();
        })
        .then(function (data) {
            if (!data.Error) {  // Kiểm tra lỗi theo cấu trúc BaseResult
                alert("Successfully updated Korean names");
                $("#ModalImportNameKR").modal('close');
                Buttonfind_Click();
            } else {
                alert("Error: " + data.Error);
            }
            $("#BackGround").css("display", "none");
        })
        .catch(function (error) {
            console.error("Error:", error);
            alert("Error saving data: " + error.message);
            $("#BackGround").css("display", "none");
        });
});
$("#FileToUpload").change(function () {
    var FileToUpload = $('#FileToUpload').prop('files');
    if (FileToUpload) {
        if (FileToUpload.length > 0) {
            for (var i = 0; i < FileToUpload.length; i++) {
            }
            let url = "/V01_3";
            OpenWindowByURL(url, 800, 400);
        }
    }
});
function Buttonexport_Click() {
    if (TagIndex == 1) {
        let RadioButton1 = document.getElementById("RadioButton1").checked;
        let RadioButton2 = document.getElementById("RadioButton2").checked;
        let RadioButton3 = document.getElementById("RadioButton3").checked;
        let TableName = "";
        if (RadioButton1 == true) {
            TableName = "DataGridView1Table";
        }
        if (RadioButton2 == true) {
            TableName = "DataGridView1TableRadioButton2";
        }
        if (RadioButton3 == true) {
            TableName = "DataGridView1TableRadioButton3";
        }
        TableHTMLToExcel(TableName, "V01_ListManagement", "V01_ListManagement");
    }
    if (TagIndex == 2) {
        TableHTMLToExcel("DataGridView2Table", "V01_MachinePartsList", "V01_MachinePartsList");
    }
    if (TagIndex == 3) {
        TableHTMLToExcel("DGV_01Table", "V01_Company", "V01_Company");
    }
}
function Buttonprint_Click() {

}
function Buttonhelp_Click() {
    let url = "/WMP_PLAY";
    OpenWindowByURL(url, 800, 460);
}
function Buttonclose_Click() {
    history.back();
}

function DataGridView1Render() {
    let RadioButton1 = document.getElementById("RadioButton1").checked;
    let RadioButton2 = document.getElementById("RadioButton2").checked;
    let RadioButton3 = document.getElementById("RadioButton3").checked;

    let HTML = "";
    let HTMLRadioButton2 = "";
    let HTMLRadioButton3 = "";
    if (BaseResult) {
        if (BaseResult.DataGridView1) {
            if (BaseResult.DataGridView1.length > 0) {
                DataGridView1_SelectionChanged(0);
                for (let i = 0; i < BaseResult.DataGridView1.length; i++) {
                    if (RadioButton1 == true) {
                        HTML = HTML + "<tr onclick='DataGridView1_SelectionChanged(" + i + ")'>";
                        HTML = HTML + "<td>" + BaseResult.DataGridView1[i].PDPART_IDX + "</td>";                
                        HTML = HTML + "<td>" + BaseResult.DataGridView1[i].PN_V + "</td>";
                        HTML = HTML + "<td>" + BaseResult.DataGridView1[i].PSPEC_V + "</td>";
                        HTML = HTML + "<td>" + BaseResult.DataGridView1[i].UNIT_VN + "</td>";
                        HTML = HTML + "<td>" + BaseResult.DataGridView1[i].PN_K + "</td>";
                        HTML = HTML + "<td>" + BaseResult.DataGridView1[i].PSPEC_K + "</td>";
                        HTML = HTML + "<td>" + BaseResult.DataGridView1[i].UNIT_KR + "</td>";
                        HTML = HTML + "<td>" + BaseResult.DataGridView1[i].PQTY + "</td>";
                        HTML = HTML + "<td>" + BaseResult.DataGridView1[i].PN_NM + "</td>";
                        HTML = HTML + "<td>" + BaseResult.DataGridView1[i].CREATE_DTM + "</td>";
                        HTML = HTML + "<td>" + BaseResult.DataGridView1[i].CREATE_USER + "</td>";
                        HTML = HTML + "<td>" + BaseResult.DataGridView1[i].UPDATE_DTM + "</td>";
                        HTML = HTML + "<td>" + BaseResult.DataGridView1[i].UPDATE_USER + "</td>";
                        HTML = HTML + "<td>" + BaseResult.DataGridView1[i].PN_DSCN_YN + "</td>";
                        HTML = HTML + "<td>" + BaseResult.DataGridView1[i].PN_PHOTO + "</td>";
                        HTML = HTML + "</tr>";
                    }

                    if (RadioButton2 == true) {
                        HTMLRadioButton2 = HTMLRadioButton2 + "<tr onclick='DataGridView1_SelectionChanged(" + i + ")'>";
                        HTMLRadioButton2 = HTMLRadioButton2 + "<td>" + BaseResult.DataGridView1[i].PDPART_IDX + "</td>";
                        HTML = HTML + "<td>" + BaseResult.DataGridView1[i].PN_V + "</td>";
                        HTML = HTML + "<td>" + BaseResult.DataGridView1[i].PSPEC_V + "</td>";
                        HTMLRadioButton2 = HTMLRadioButton2 + "<td>" + BaseResult.DataGridView1[i].UNIT_VN + "</td>";
                        HTMLRadioButton2 = HTMLRadioButton2 + "<td>" + BaseResult.DataGridView1[i].PQTY + "</td>";
                        HTMLRadioButton2 = HTMLRadioButton2 + "<td>" + BaseResult.DataGridView1[i].PN_NM + "</td>";
                        HTMLRadioButton2 = HTMLRadioButton2 + "<td>" + BaseResult.DataGridView1[i].CREATE_DTM + "</td>";
                        HTMLRadioButton2 = HTMLRadioButton2 + "<td>" + BaseResult.DataGridView1[i].CREATE_USER + "</td>";
                        HTMLRadioButton2 = HTMLRadioButton2 + "<td>" + BaseResult.DataGridView1[i].UPDATE_DTM + "</td>";
                        HTMLRadioButton2 = HTMLRadioButton2 + "<td>" + BaseResult.DataGridView1[i].UPDATE_USER + "</td>";
                        HTMLRadioButton2 = HTMLRadioButton2 + "<td>" + BaseResult.DataGridView1[i].PN_DSCN_YN + "</td>";
                        HTMLRadioButton2 = HTMLRadioButton2 + "<td>" + BaseResult.DataGridView1[i].PN_PHOTO + "</td>";
                        HTMLRadioButton2 = HTMLRadioButton2 + "</tr>";
                    }

                    if (RadioButton3 == true) {
                        HTMLRadioButton3 = HTMLRadioButton3 + "<tr onclick='DataGridView1_SelectionChanged(" + i + ")'>";
                        HTMLRadioButton3 = HTMLRadioButton3 + "<td>" + BaseResult.DataGridView1[i].PDPART_IDX + "</td>";
                        HTML = HTML + "<td>" + BaseResult.DataGridView1[i].PN_K + "</td>";
                        HTML = HTML + "<td>" + BaseResult.DataGridView1[i].PSPEC_K + "</td>";
                        HTMLRadioButton3 = HTMLRadioButton3 + "<td>" + BaseResult.DataGridView1[i].UNIT_KR + "</td>";
                        HTMLRadioButton3 = HTMLRadioButton3 + "<td>" + BaseResult.DataGridView1[i].PQTY + "</td>";
                        HTMLRadioButton3 = HTMLRadioButton3 + "<td>" + BaseResult.DataGridView1[i].PN_NM + "</td>";
                        HTMLRadioButton3 = HTMLRadioButton3 + "<td>" + BaseResult.DataGridView1[i].CREATE_DTM + "</td>";
                        HTMLRadioButton3 = HTMLRadioButton3 + "<td>" + BaseResult.DataGridView1[i].CREATE_USER + "</td>";
                        HTMLRadioButton3 = HTMLRadioButton3 + "<td>" + BaseResult.DataGridView1[i].UPDATE_DTM + "</td>";
                        HTMLRadioButton3 = HTMLRadioButton3 + "<td>" + BaseResult.DataGridView1[i].UPDATE_USER + "</td>";
                        HTMLRadioButton3 = HTMLRadioButton3 + "<td>" + BaseResult.DataGridView1[i].PN_DSCN_YN + "</td>";
                        HTMLRadioButton3 = HTMLRadioButton3 + "<td>" + BaseResult.DataGridView1[i].PN_PHOTO + "</td>";
                        HTMLRadioButton3 = HTMLRadioButton3 + "</tr>";
                    }
                }
            }
        }
    }
    document.getElementById("DataGridView1").innerHTML = HTML;
    document.getElementById("DataGridView1RadioButton2").innerHTML = HTMLRadioButton2;
    document.getElementById("DataGridView1RadioButton3").innerHTML = HTMLRadioButton3;


    document.getElementById("DataGridView1Table").style.display = "none";
    document.getElementById("DataGridView1TableRadioButton2").style.display = "none";
    document.getElementById("DataGridView1TableRadioButton3").style.display = "none";
    if (RadioButton1 == true) {
        document.getElementById("DataGridView1Table").style.display = "block";
    }
    if (RadioButton2 == true) {
        document.getElementById("DataGridView1TableRadioButton2").style.display = "block";
    }
    if (RadioButton3 == true) {
        document.getElementById("DataGridView1TableRadioButton3").style.display = "block";
    }
}
function DataGridView1Sort() {
    IsTableSort = !IsTableSort;
    DataGridViewSort(BaseResult.DataGridView1, IsTableSort);
    DataGridView1Render();
}
function DataGridView1_SelectionChanged(i) {
    DataGridView1RowIndex = i;
    $("#PART_ADD01").val(BaseResult.DataGridView1[DataGridView1RowIndex].PN_V);
    $("#PART_ADD02").val(BaseResult.DataGridView1[DataGridView1RowIndex].PSPEC_V);
    $("#PART_ADD03").val(BaseResult.DataGridView1[DataGridView1RowIndex].PN_K);
    $("#PART_ADD04").val(BaseResult.DataGridView1[DataGridView1RowIndex].PSPEC_K);
    $("#PART_ADD05").val(BaseResult.DataGridView1[DataGridView1RowIndex].PN_NM);
    $("#PART_ADD07").val(BaseResult.DataGridView1[DataGridView1RowIndex].PQTY);
    $("#PART_ADD08").val(BaseResult.DataGridView1[DataGridView1RowIndex].PN_PHOTO);

    let PART_ADD06 = BaseResult.DataGridView1[DataGridView1RowIndex].UNIT_KR;
    let Language = GetCookieValue("Language");
    if (Language == "vi") {
        PART_ADD06 = BaseResult.DataGridView1[DataGridView1RowIndex].UNIT_VN;
    }
    x = document.getElementById("PART_ADD06");
    x.options[x.selectedIndex].text = PART_ADD06;

    if (BaseResult.DataGridView1[DataGridView1RowIndex].PN_DSCN_YN == "Y") {
        document.getElementById("RadioButton4").checked = true;
    }
    if (BaseResult.DataGridView1[DataGridView1RowIndex].PN_DSCN_YN == "N") {
        document.getElementById("RadioButton5").checked = true;
    }
}
function DataGridView2Render() {
    let HTML = "";
    if (BaseResult) {
        if (BaseResult.DataGridView2) {
            if (BaseResult.DataGridView2.length > 0) {
                DataGridView2_SelectionChanged(0);
                for (let i = 0; i < BaseResult.DataGridView2.length; i++) {
                    HTML = HTML + "<tr onclick='DataGridView2_SelectionChanged(" + i + ")'>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].PDPART_IDX + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].PN_V + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].PN_K + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].PSPEC_V + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].PSPEC_K + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].UNIT_EN + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].PQTY + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].PN_NM + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].CREATE_DTM + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].CREATE_USER + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].UPDATE_DTM + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].UPDATE_USER + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].PN_DSCN_YN + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].PN_PHOTO + "</td>";
                    HTML = HTML + "</tr>";
                }
            }
        }
    }
    document.getElementById("DataGridView2").innerHTML = HTML;
}
function DataGridView2Sort() {
    IsTableSort = !IsTableSort;
    DataGridViewSort(BaseResult.DataGridView2, IsTableSort);
    DataGridView2Render();
}
function DataGridView2_SelectionChanged(i) {
    DataGridView2RowIndex = i;
    $("#T2_PN_K").val(BaseResult.DataGridView2[DataGridView2RowIndex].PN_K);
    $("#T2_PSPEC_V").val(BaseResult.DataGridView2[DataGridView2RowIndex].PSPEC_V);
    $("#T2_PN_V").val(BaseResult.DataGridView2[DataGridView2RowIndex].PN_V);
    $("#T2_PSPEC_K").val(BaseResult.DataGridView2[DataGridView2RowIndex].PSPEC_K);
    $("#T2_PN_NM").val(BaseResult.DataGridView2[DataGridView2RowIndex].PN_NM);
    $("#T2_PHOTO").val(BaseResult.DataGridView2[DataGridView2RowIndex].PN_PHOTO);

    if (BaseResult.DataGridView2[DataGridView2RowIndex].PN_DSCN_YN == "Y") {
        document.getElementById("RadioButton14").checked = true;
    }
    if (BaseResult.DataGridView2[DataGridView2RowIndex].PN_DSCN_YN == "N") {
        document.getElementById("RadioButton13").checked = true;
    }
    let T2_PHOTO = $("#T2_PHOTO").val();
    if (T2_PHOTO == "") {
        let URL_PHOTO = "http://113.161.129.118:5240/MES_DATE/PHOTO/V01/DJG_IMG_NULL.png";
        document.getElementById("PictureBox1").src = URL_PHOTO;
    }
    else {
        let URL_PHOTO = "http://113.161.129.118:5240/MES_DATE/PHOTO/V01/" + T2_PHOTO;
        document.getElementById("PictureBox1").src = URL_PHOTO;
    }
}
function DataGridView3Render() {
    let HTML = "";
    if (BaseResult) {
        if (BaseResult.DataGridView3) {
            if (BaseResult.DataGridView3.length > 0) {
                DataGridView3_SelectionChanged(0);
                for (let i = 0; i < BaseResult.DataGridView3.length; i++) {
                    HTML = HTML + "<tr onclick='DataGridView3_SelectionChanged(" + i + ")'>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView3[i].PN_NM + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView3[i].PN_V + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView3[i].PSPEC_V + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView3[i].UNIT_V + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView3[i].PN_K + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView3[i].PSPEC_K + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView3[i].UNIT_K + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView3[i].PQTY + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView3[i].PD_COST_DATE + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView3[i].COST + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView3[i].Company + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView3[i].CREATE_DTM + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView3[i].CREATE_USER + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView3[i].UPDATE_DTM + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView3[i].UPDATE_USER + "</td>";
                    HTML = HTML + "</tr>";
                }
            }
        }
    }
    document.getElementById("DataGridView3").innerHTML = HTML;
}
function DataGridView3Sort() {
    IsTableSort = !IsTableSort;
    DataGridViewSort(BaseResult.DataGridView3, IsTableSort);
    DataGridView3Render();
}
function DataGridView3_SelectionChanged(i) {
    DataGridView3RowIndex = i;
    $("#TAB4_TB01").val(BaseResult.DataGridView3[DataGridView3RowIndex].PN_V);
    $("#TAB4_TB02").val(BaseResult.DataGridView3[DataGridView3RowIndex].PSPEC_V);
    $("#TAB4_TB03").val(BaseResult.DataGridView3[DataGridView3RowIndex].PN_K);
    $("#TAB4_TB04").val(BaseResult.DataGridView3[DataGridView3RowIndex].PSPEC_K);
    $("#TAB4_TB00").val(BaseResult.DataGridView3[DataGridView3RowIndex].PN_NM);
    $("#TAB4_TB05").val(BaseResult.DataGridView3[DataGridView3RowIndex].Company);
    $("#TextBox6").val(BaseResult.DataGridView3[DataGridView3RowIndex].PQTY);
    $("#TextBox14").val(BaseResult.DataGridView3[DataGridView3RowIndex].COST);
    $("#Label26").val(BaseResult.DataGridView3[DataGridView3RowIndex].COST);
    let PD_COST_DATE = "";
    if (BaseResult.DataGridView3[DataGridView3RowIndex].PD_COST_DATE != null) {
        PD_COST_DATE = BaseResult.DataGridView3[DataGridView3RowIndex].PD_COST_DATE.split("T")[0];
    }

    $("#DateTimePicker1").val(PD_COST_DATE);
    $("#TextBox5").val(BaseResult.DataGridView3[DataGridView3RowIndex].UNIT_V + "(" + BaseResult.DataGridView3[DataGridView3RowIndex].UNIT_K + ")");
}
function DGV_01Render() {
    let HTML = "";
    if (BaseResult) {
        if (BaseResult.DGV_01) {
            if (BaseResult.DGV_01.length > 0) {
                DGV_01_SelectionChanged(0);
                for (let i = 0; i < BaseResult.DGV_01.length; i++) {
                    HTML = HTML + "<tr onclick='DGV_01_SelectionChanged(" + i + ")'>";
                    if (BaseResult.DGV_01[i].CHK) {
                        HTML = HTML + "<td><input id='DGV_01CHK" + i + "' class='form-check-input' type='checkbox' checked onclick='DGV_01CHKChanged(" + i + ")'><span></span></td>";
                    }
                    else {
                        HTML = HTML + "<td><input id='DGV_01CHK" + i + "' class='form-check-input' type='checkbox' onclick='DGV_01CHKChanged(" + i + ")'><span></span></td>";
                    }
                    HTML = HTML + "<td onclick='DGV_01_CellClick(" + i + ")'><button class='btn waves-effect waves-light grey darken-1' style='width: 100px;'>" + BaseResult.DGV_01[i].CMPNY_IDX + "</button></td>";
                    HTML = HTML + "<td>" + BaseResult.DGV_01[i].CMPNY_NM + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DGV_01[i].CMPNY_DVS + "</td>";                                        
                    HTML = HTML + "<td>" + BaseResult.DGV_01[i].CMPNY_ADDR + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DGV_01[i].CMPNY_NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DGV_01[i].CMPNY_TEL + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DGV_01[i].CMPNY_FAX + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DGV_01[i].CMPNY_MNGR + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DGV_01[i].CMPNY_RMK + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DGV_01[i].CREATE_DTM + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DGV_01[i].CREATE_USER + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DGV_01[i].UPDATE_DTM + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DGV_01[i].UPDATE_USER + "</td>";
                    HTML = HTML + "</tr>";
                }
            }
        }
    }
    document.getElementById("DGV_01").innerHTML = HTML;
}
function DGV_01Sort() {
    IsTableSort = !IsTableSort;
    DataGridViewSort(BaseResult.DGV_01, IsTableSort);
    DGV_01Render();
}
function DGV_01_SelectionChanged(i) {
    DGV_01RowIndex = i;
    document.getElementById("TBA01").readOnly = true;
    $("#TBA08").val(BaseResult.DGV_01[DGV_01RowIndex].CMPNY_IDX);
    $("#TBA01").val(BaseResult.DGV_01[DGV_01RowIndex].CMPNY_NM);
    $("#TBA02").val(BaseResult.DGV_01[DGV_01RowIndex].CMPNY_DVS);
    $("#TBA03").val(BaseResult.DGV_01[DGV_01RowIndex].CMPNY_ADDR);
    $("#TBA06").val(BaseResult.DGV_01[DGV_01RowIndex].CMPNY_TEL);
    $("#TBA07").val(BaseResult.DGV_01[DGV_01RowIndex].CMPNY_FAX);
    $("#TBA04").val(BaseResult.DGV_01[DGV_01RowIndex].CMPNY_MNGR);
    $("#TBA05").val(BaseResult.DGV_01[DGV_01RowIndex].CMPNY_RMK);
    $("#TBA09").val(BaseResult.DGV_01[DGV_01RowIndex].CMPNY_NO);
}
function DGV_01CHKChanged(i) {
    let id = "DGV_01CHK" + i;
    DGV_01RowIndex = i;
    BaseResult.DGV_01[DGV_01RowIndex].CHK = !BaseResult.DGV_01[DGV_01RowIndex].CHK;
    DGV_01Render();
}
function DGV_01_CellClick(i) {
    DGV_01RowIndex = i;
    localStorage.setItem("V01_4_CMP_CODE", "");
    localStorage.setItem("V01_4_CMP_CODE", BaseResult.DGV_01[DGV_01RowIndex].CMPNY_IDX);
    localStorage.setItem("V01_4_Label3", BaseResult.DGV_01[DGV_01RowIndex].CMPNY_NM);
    localStorage.setItem("V01_4_Label1", BaseResult.DGV_01[DGV_01RowIndex].CMPNY_DVS);
    let url = "/V01_4";
    OpenWindowByURL(url, 400, 400);
}