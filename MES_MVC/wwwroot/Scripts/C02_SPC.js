let IsTableSort = false;
let BaseResult;
let OR_IDX
$(window).focus(function () {
}).blur(function () {
    Buttonclose_Click();
});

$(document).ready(function () {
    localStorage.setItem("C02_SPC_Close", 0);

    $("#Label10").val(localStorage.getItem("C02_SPC_Label10"));
    $("#STS1").val("");
    $("#STS2").val("");
    $("#STS3").val("");
    $("#STS4").val("");
    $("#STS5").val("");
    $("#STS6").val("");
    $("#STS7").val("");
    $("#STS8").val("");

    $("#STL1").val("");
    $("#STL2").val("");
    $("#STL3").val("");
    $("#STL4").val("");
    $("#STL5").val("");
    $("#STL6").val("");
    $("#STL7").val("");
    $("#STL8").val("");

    $("#STM1").val("");
    $("#STM2").val("");
    $("#STM3").val("");
    $("#STM4").val("");
    $("#STM5").val("");
    $("#STM6").val("");
    $("#STM7").val("");
    $("#STM8").val("");

    document.getElementById("TextBox1").disabled = false;
    document.getElementById("TextBox2").disabled = false;
    document.getElementById("TextBox3").disabled = false;
    document.getElementById("TextBox4").disabled = false;
    document.getElementById("TextBox5").disabled = false;
    document.getElementById("TextBox6").disabled = false;
    document.getElementById("TextBox7").disabled = false;
    document.getElementById("TextBox8").disabled = false;

    $("#ST11").val(localStorage.getItem("C02_START_V2_ST_DCC1"));
    $("#ST12").val(localStorage.getItem("C02_START_V2_ST_DIC1"));
    $("#ST21").val(localStorage.getItem("C02_START_V2_ST_DCC2"));
    $("#ST22").val(localStorage.getItem("C02_START_V2_ST_DIC2"));
    $("#Label8").val(localStorage.getItem("C02_START_V2_VLW"));
    $("#Label18").val(localStorage.getItem("C02_START_V2_WIRE_Length"));

    let C02_START_V2_WIRE_Length = localStorage.getItem("C02_START_V2_WIRE_Length");
    C02_START_V2_WIRE_Length = C02_START_V2_WIRE_Length.replace(",", "");
    let WIRE_LENCK = Number(C02_START_V2_WIRE_Length);

    if ((WIRE_LENCK >= 0) && (WIRE_LENCK <= 500)) {
        $("#STL10").val(WIRE_LENCK);
        $("#STM10").val(WIRE_LENCK + 5);
    }

    if ((WIRE_LENCK >= 501) && (WIRE_LENCK <= 2000)) {
        $("#STL10").val(WIRE_LENCK);
        $("#STM10").val(WIRE_LENCK + 10);
    }

    if ((WIRE_LENCK >= 2001) && (WIRE_LENCK <= 90000)) {
        $("#STL10").val(WIRE_LENCK);
        $("#STM10").val(WIRE_LENCK + 15);
    }

    let C02_START_V2_ST_DWIRE1 = localStorage.getItem("C02_START_V2_ST_DWIRE1");
    C02_START_V2_WIRE_Length = C02_START_V2_WIRE_Length.replace(",", "");

    let BBBB = [];
    let CCCC = [];
    let LLLL = [];
    let MMMM = [];

    BBBB.push($("#ST11").val());
    BBBB.push($("#ST12").val());
    BBBB.push($("#ST21").val());
    BBBB.push($("#ST22").val());

    let OO = 0;
    for (let i = 0; i < BBBB.length; i++) {
        let AA = BBBB[i];
        try {
            let BB = AA.substring(0, AA.indexOf("/") - 1);
            let CC = BB.substring(0, BB.indexOf("±") - 1);
            let CCC = "";
            let DD = BB.substring(0, BB.indexOf("±") + 1);
            try {
                CCC = CC.substring(0, CC.indexOf("(") + 1);
            }
            catch (e) {
                CCC = CC;
            }
            OO = OO + 1;
            CCCC.push(CCC);
            let CCCDD = Number(CCC) - Number(DD);
            LLLL.push(CCCDD);
            CCCDD = Number(CCC) + Number(DD);
            MMMM.push(CCCDD);
            let BB1 = AA.substring(0, AA.indexOf("/") + 1).trim();
            let CC1 = BB1.substring(0, BB1.indexOf("±") - 1);
            let DD1 = BB1.substring(0, BB1.indexOf("±") + 1);
            let CCC1 = "";
            try {
                CCC1 = CC1.substring(0, CC1.indexOf("(") - 1);
            }
            catch (e) {
                CCC1 = CC1
            }
            OO = OO + 1
            CCCC.push(CCC1);
            let CCC1DD1 = Number(CCC1) - Number(DD1);
            LLLL.push(CCC1DD1);
            CCC1DD1 = Number(CCC1) + Number(DD1);
            MMMM.push(CCC1DD1);
        }
        catch (e) {
            OO = OO + 1;
            CCCC.push("");
            LLLL.push("");
            MMMM.push("");

            OO = OO + 1;
            CCCC.push("");
            LLLL.push("");
            MMMM.push("");
        }
        $("#STS1").val(CCCC[0]);
        $("#STS2").val(CCCC[1]);
        $("#STS3").val(CCCC[2]);
        $("#STS4").val(CCCC[3]);
        $("#STS5").val(CCCC[4]);
        $("#STS6").val(CCCC[5]);
        $("#STS7").val(CCCC[6]);
        $("#STS8").val(CCCC[7]);

        $("#STL1").val(LLLL[0]);
        $("#STL2").val(LLLL[1]);
        $("#STL3").val(LLLL[2]);
        $("#STL4").val(LLLL[3]);
        $("#STL5").val(LLLL[4]);
        $("#STL6").val(LLLL[5]);
        $("#STL7").val(LLLL[6]);
        $("#STL8").val(LLLL[7]);

        $("#STM1").val(MMMM[0]);
        $("#STM2").val(MMMM[1]);
        $("#STM3").val(MMMM[2]);
        $("#STM4").val(MMMM[3]);
        $("#STM5").val(MMMM[4]);
        $("#STM6").val(MMMM[5]);
        $("#STM7").val(MMMM[6]);
        $("#STM8").val(MMMM[7]);

        if ($("#STS1").val() == "") {
            document.getElementById("TextBox1").disabled = true;
        }
        if ($("#STS2").val() == "") {
            document.getElementById("TextBox2").disabled = true;
        }
        if ($("#STS3").val() == "") {
            document.getElementById("TextBox3").disabled = true;
        }
        if ($("#STS4").val() == "") {
            document.getElementById("TextBox4").disabled = true;
        }
        if ($("#STS5").val() == "") {
            document.getElementById("TextBox5").disabled = true;
        }
        if ($("#STS6").val() == "") {
            document.getElementById("TextBox6").disabled = true;
        }
        if ($("#STS7").val() == "") {
            document.getElementById("TextBox7").disabled = true;
        }
        if ($("#STS8").val() == "") {
            document.getElementById("TextBox8").disabled = true;
        }

        $("#Label4").val(localStorage.getItem("C02_START_V2_VLT1"));
        $("#Label5").val(localStorage.getItem("C02_START_V2_VLT1"));
        $("#Label6").val(localStorage.getItem("C02_START_V2_VLT2"));
        $("#Label7").val(localStorage.getItem("C02_START_V2_VLT2"));

        let Label4 = $("#Label4").val();
        let Label6 = $("#Label6").val();

        let BBB = Label4.includes("(");
        let DDD = Label6.includes("(");

        if (BBB == true) {
            document.getElementById("TextBox1").disabled = false;
            document.getElementById("TextBox2").disabled = false;
            document.getElementById("TextBox3").disabled = false;
            document.getElementById("TextBox4").disabled = false;
        }

        if (DDD == true) {
            document.getElementById("TextBox5").disabled = false;
            document.getElementById("TextBox6").disabled = false;
            document.getElementById("TextBox7").disabled = false;
            document.getElementById("TextBox8").disabled = false;
        }

        $("#TextBox1").val("");
        TextBox1_TextChanged();
        $("#TextBox2").val("");
        TextBox2_TextChanged();
        $("#TextBox3").val("");
        TextBox3_TextChanged();
        $("#TextBox4").val("");
        TextBox4_TextChanged();
        $("#TextBox5").val("");
        TextBox5_TextChanged();
        $("#TextBox6").val("");
        TextBox6_TextChanged();
        $("#TextBox7").val("");
        TextBox7_TextChanged();
        $("#TextBox8").val("");
        TextBox8_TextChanged();
        $("#TextBox9").val("");
        TextBox9_TextChanged();
        $("#TextBox10").val("");
        TextBox10_TextChanged();

        $("#LR1").val("");
        $("#LR2").val("");
        $("#LR3").val("");
        $("#LR4").val("");
        $("#LR5").val("");
        $("#LR6").val("");
        $("#LR7").val("");
        $("#LR8").val("");
        $("#LR9").val("");
        $("#LR10").val("");

        document.getElementById("MRUS").style.backgroundColor = "white";

        document.getElementById("LR1").style.backgroundColor = "white";
        document.getElementById("LR2").style.backgroundColor = "white";
        document.getElementById("LR3").style.backgroundColor = "whitesmoke";
        document.getElementById("LR4").style.backgroundColor = "whitesmoke";
        document.getElementById("LR5").style.backgroundColor = "white";
        document.getElementById("LR6").style.backgroundColor = "white";
        document.getElementById("LR7").style.backgroundColor = "whitesmoke";
        document.getElementById("LR8").style.backgroundColor = "whitesmoke";
        document.getElementById("LR9").style.backgroundColor = "white";
        document.getElementById("LR10").style.backgroundColor = "white";

        ORDER_CHG();
        C02_SPC_Load();

        if ((BBB == false) && (DDD == false)) {
            Buttonclose_Click();
        }

        if ($("#STL1").val() == "") {
            document.getElementById("TextBox1").disabled = true;
        }
        if ($("#STL2").val() == "") {
            document.getElementById("TextBox2").disabled = true;
        }
        if ($("#STL3").val() == "") {
            document.getElementById("TextBox3").disabled = true;
        }
        if ($("#STL4").val() == "") {
            document.getElementById("TextBox4").disabled = true;
        }
        if ($("#STL5").val() == "") {
            document.getElementById("TextBox5").disabled = true;
        }
        if ($("#STL6").val() == "") {
            document.getElementById("TextBox6").disabled = true;
        }
        if ($("#STL7").val() == "") {
            document.getElementById("TextBox7").disabled = true;
        }
        if ($("#STL8").val() == "") {
            document.getElementById("TextBox8").disabled = true;
        }
        if (document.getElementById("TextBox1").disabled == false) {
            $("#TextBox1").focus();
        }
        else {
            $("#TextBox5").focus();
        }
    }

});
function C02_SPC_Load() {
    let F_CO = $("#Label19").val();
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        SearchString: F_CO,
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C02_SPC/C02_SPC_Load";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            let BaseResultSub = data;
            BaseResult.Search = BaseResultSub.Search;
            if (BaseResult.Search.length == 0) {
                $("#Label17").val(0);
            }
            $("#Label17").val(BaseResult.Search[0].STRENGTH);
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
function ORDER_CHG() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        SearchString: OR_IDX,
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C02_SPC/ORDER_CHG";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
function Form_RUS() {
    let CHK = [];
    let VAL = [];
    CHK.push(document.getElementById("TextBox1").disabled);
    CHK.push(document.getElementById("TextBox2").disabled);
    CHK.push(document.getElementById("TextBox3").disabled);
    CHK.push(document.getElementById("TextBox4").disabled);
    CHK.push(document.getElementById("TextBox5").disabled);
    CHK.push(document.getElementById("TextBox6").disabled);
    CHK.push(document.getElementById("TextBox7").disabled);
    CHK.push(document.getElementById("TextBox8").disabled);
    CHK.push(document.getElementById("TextBox9").disabled);
    CHK.push(document.getElementById("TextBox10").disabled);

    let AA = [];
    AA.push($("#LR1").val());
    AA.push($("#LR2").val());
    AA.push($("#LR3").val());
    AA.push($("#LR4").val());
    AA.push($("#LR5").val());
    AA.push($("#LR6").val());
    AA.push($("#LR7").val());
    AA.push($("#LR8").val());
    AA.push($("#LR9").val());
    AA.push($("#LR10").val());

    for (let i = 0; i < CHK.length; i++) {
        if (CHK[i] == false) {
            if (AA[i] == "OK") {
                VAL.push(true);
            }
        }
        else {
            VAL.push(false);
        }
    }
    if ((VAL[0] == true) && (VAL[1] == true) && (VAL[2] == true) && (VAL[3] == true) && (VAL[4] == true) && (VAL[5] == true) && (VAL[6] == true) && (VAL[7] == true) && (VAL[8] == true) && (VAL[9] == true) && (VAL[10] == true)) {
        $("#MRUS").val("OK");
        document.getElementById("MRUS").style.backgroundColor = "lime";
    }
    else {
        $("#MRUS").val("NG");
        document.getElementById("MRUS").style.backgroundColor = "red";
    }
}
$("#Button1").click(function (e) {
    Button1_Click();
});
function Button1_Click() {
    let IsCheck = true;
    let TextBox1 = $("#TextBox1").val();
    let TextBox2 = $("#TextBox2").val();
    let TextBox3 = $("#TextBox3").val();
    let TextBox4 = $("#TextBox4").val();
    let TextBox5 = $("#TextBox5").val();
    let TextBox6 = $("#TextBox6").val();
    let TextBox7 = $("#TextBox7").val();
    let TextBox8 = $("#TextBox8").val();
    let TextBox9 = $("#TextBox9").val();
    let TextBox10 = $("#TextBox10").val();

    let Label8 = localStorage.getItem("C02_START_V2_Label8");
    let Label10 = $("#Label10").val();
    let Label11 = $("#Label11").val();

    let MRUS = $("#MRUS").val();

    if (MRUS == "NG") {
        IsCheck = false;
        alert("The test result is an error.");
    }
    if (IsCheck == true) {
        if (MRUS == "OK") {
            $("#BackGround").css("display", "block");
            let BaseParameter = new Object();
            BaseParameter = {
                ListSearchString: [],
            }
            BaseParameter.USER_ID = GetCookieValue("UserID");
            BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
            BaseParameter.ListSearchString.push(TextBox1);
            BaseParameter.ListSearchString.push(TextBox2);
            BaseParameter.ListSearchString.push(TextBox3);
            BaseParameter.ListSearchString.push(TextBox4);
            BaseParameter.ListSearchString.push(TextBox5);
            BaseParameter.ListSearchString.push(TextBox6);
            BaseParameter.ListSearchString.push(TextBox7);
            BaseParameter.ListSearchString.push(TextBox8);
            BaseParameter.ListSearchString.push(TextBox10);
            BaseParameter.ListSearchString.push(Label8);
            BaseParameter.ListSearchString.push(Label10);
            BaseParameter.ListSearchString.push(Label11);
            BaseParameter.ListSearchString.push(MRUS);
            BaseParameter.ListSearchString.push(TextBox9);
            let formUpload = new FormData();
            formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
            let url = "/C02_SPC/Button1_Click";

            fetch(url, {
                method: "POST",
                body: formUpload,
                headers: {
                }
            }).then((response) => {
                response.json().then((data) => {
                    $("#BackGround").css("display", "none");
                    localStorage.setItem("C02_START_V2_SPC_EXIT", true);
                    Buttonclose_Click();
                }).catch((err) => {
                    $("#BackGround").css("display", "none");
                })
            });
        }
    }
}
$("#TextBox1").keydown(function (e) {
    if (e.keyCode == 13) {
        TextBox1_KeyDown();
    }
});
function TextBox1_KeyDown() {
    $("#TextBox2").focus();
}
$("#TextBox2").keydown(function (e) {
    if (e.keyCode == 13) {
        TextBox2_KeyDown();
    }
});
function TextBox2_KeyDown() {
    $("#TextBox3").focus();
}
$("#TextBox3").keydown(function (e) {
    if (e.keyCode == 13) {
        TextBox3_KeyDown();
    }
});
function TextBox3_KeyDown() {
    $("#TextBox4").focus();
}
$("#TextBox4").keydown(function (e) {
    if (e.keyCode == 13) {
        TextBox4_KeyDown();
    }
});
function TextBox4_KeyDown() {
    if (document.getElementById("TextBox5").disabled == false) {
        $("#TextBox5").focus();
    }
    else {
        $("#TextBox9").focus();
    }
}
$("#TextBox5").keydown(function (e) {
    if (e.keyCode == 13) {
        TextBox5_KeyDown();
    }
});
function TextBox5_KeyDown() {
    $("#TextBox6").focus();
}
$("#TextBox6").keydown(function (e) {
    if (e.keyCode == 13) {
        TextBox6_KeyDown();
    }
});
function TextBox6_KeyDown() {
    $("#TextBox7").focus();
}
$("#TextBox7").keydown(function (e) {
    if (e.keyCode == 13) {
        TextBox7_KeyDown();
    }
});
function TextBox7_KeyDown() {
    $("#TextBox8").focus();
}
$("#TextBox8").keydown(function (e) {
    if (e.keyCode == 13) {
        TextBox8_KeyDown();
    }
});
function TextBox8_KeyDown() {
    $("#TextBox8").focus();
}
$("#TextBox9").keydown(function (e) {
    if (e.keyCode == 13) {
        TextBox10_KeyDown();
    }
});
function TextBox10_KeyDown() {
    $("#TextBox10").focus();
}
$("#TextBox10").keydown(function (e) {
    if (e.keyCode == 13) {
        TextBox10_KeyDown_1();
    }
});
function TextBox10_KeyDown_1() {
    $("#TextBox10").focus();
}
$("#TextBox1").change(function (e) {
    TextBox1_TextChanged()
});
function TextBox1_TextChanged() {
    try {
        let TextBox1 = $("#TextBox1").val();
        let STL1 = $("#STL1").val();
        let STM1 = $("#STM1").val();
        if (SPCValuesCheckValue(TextBox1, STL1, STM1)) {
            $("#LR1").val("OK");
            document.getElementById("LR1").style.backgroundColor = "lime";
        }
        else {
            $("#LR1").val("NG");
            document.getElementById("LR1").style.backgroundColor = "red";
        }
    }
    catch (e) {
        alert(e);
    }
    Form_RUS();
}
$("#TextBox2").change(function (e) {
    TextBox2_TextChanged()
});
function TextBox2_TextChanged() {
    try {
        let TextBox2 = $("#TextBox2").val();
        let STL2 = $("#STL2").val();
        let STM2 = $("#STM2").val();
        if (SPCValuesCheckValue(TextBox2, STL2, STM2)) {
            $("#LR2").val("OK");
            document.getElementById("LR2").style.backgroundColor = "lime";
        }
        else {
            $("#LR2").val("NG");
            document.getElementById("LR2").style.backgroundColor = "red";
        }
    }
    catch (e) {
        alert(e);
    }
    Form_RUS();
}
$("#TextBox3").change(function (e) {
    TextBox3_TextChanged()
});
function TextBox3_TextChanged() {
    try {
        let TextBox3 = $("#TextBox3").val();
        let STL3 = $("#STL3").val();
        let STM3 = $("#STM3").val();
        if (SPCValuesCheckValue(TextBox3, STL3, STM3)) {
            $("#LR3").val("OK");
            document.getElementById("LR3").style.backgroundColor = "lime";
        }
        else {
            $("#LR3").val("NG");
            document.getElementById("LR3").style.backgroundColor = "red";
        }
    }
    catch (e) {
        alert(e);
    }
    Form_RUS();
}
$("#TextBox4").change(function (e) {
    TextBox4_TextChanged()
});
function TextBox4_TextChanged() {
    try {
        let TextBox4 = $("#TextBox4").val();
        let STL4 = $("#STL4").val();
        let STM4 = $("#STM4").val();
        if (SPCValuesCheckValue(TextBox4, STL4, STM4)) {
            $("#LR4").val("OK");
            document.getElementById("LR4").style.backgroundColor = "lime";
        }
        else {
            $("#LR4").val("NG");
            document.getElementById("LR4").style.backgroundColor = "red";
        }
    }
    catch (e) {
        alert(e);
    }
    Form_RUS();
}
$("#TextBox5").change(function (e) {
    TextBox5_TextChanged()
});
function TextBox5_TextChanged() {
    try {
        let TextBox5 = $("#TextBox5").val();
        let STL5 = $("#STL5").val();
        let STM5 = $("#STM5").val();
        if (SPCValuesCheckValue(TextBox5, STL5, STM5)) {
            $("#LR5").val("OK");
            document.getElementById("LR5").style.backgroundColor = "lime";
        }
        else {
            $("#LR5").val("NG");
            document.getElementById("LR5").style.backgroundColor = "red";
        }
    }
    catch (e) {
        alert(e);
    }
    Form_RUS();
}
$("#TextBox6").change(function (e) {
    TextBox6_TextChanged()
});
function TextBox6_TextChanged() {
    try {
        let TextBox6 = $("#TextBox6").val();
        let STL6 = $("#STL6").val();
        let STM6 = $("#STM6").val();
        if (SPCValuesCheckValue(TextBox6, STL6, STM6)) {
            $("#LR6").val("OK");
            document.getElementById("LR6").style.backgroundColor = "lime";
        }
        else {
            $("#LR6").val("NG");
            document.getElementById("LR6").style.backgroundColor = "red";
        }
    }
    catch (e) {
        alert(e);
    }
    Form_RUS();
}
$("#TextBox7").change(function (e) {
    TextBox7_TextChanged()
});
function TextBox7_TextChanged() {
    try {
        let TextBox7 = $("#TextBox7").val();
        let STL7 = $("#STL7").val();
        let STM7 = $("#STM7").val();
        if (SPCValuesCheckValue(TextBox7, STL7, STM7)) {
            $("#LR7").val("OK");
            document.getElementById("LR7").style.backgroundColor = "lime";
        }
        else {
            $("#LR7").val("NG");
            document.getElementById("LR7").style.backgroundColor = "red";
        }
    }
    catch (e) {
        alert(e);
    }
    Form_RUS();
}
$("#TextBox8").change(function (e) {
    TextBox8_TextChanged()
});
function TextBox8_TextChanged() {
    try {
        let TextBox8 = $("#TextBox8").val();
        let STL8 = $("#STL8").val();
        let STM8 = $("#STM8").val();
        if (SPCValuesCheckValue(TextBox8, STL8, STM8)) {
            $("#LR8").val("OK");
            document.getElementById("LR8").style.backgroundColor = "lime";
        }
        else {
            $("#LR8").val("NG");
            document.getElementById("LR8").style.backgroundColor = "red";
        }
    }
    catch (e) {
        alert(e);
    }
    Form_RUS();
}
$("#TextBox9").change(function (e) {
    TextBox9_TextChanged()
});
function TextBox9_TextChanged() {
    try {
        let AA = Number($("#TextBox9").val());
        let CC = Number($("#Label17").val());

        if (CC <= AA) {
            $("#LR9").val("OK");
            document.getElementById("LR9").style.backgroundColor = "lime";
        }
        else {
            $("#LR9").val("NG");
            document.getElementById("LR9").style.backgroundColor = "red";
        }
    }
    catch (e) {
        alert(e);
    }
    Form_RUS();
}
$("#TextBox10").change(function (e) {
    TextBox10_TextChanged()
});
function TextBox10_TextChanged() {
    try {
        let TextBox10 = $("#TextBox10").val();
        let STL10 = $("#STL10").val();
        let STM10 = $("#STM10").val();
        if (SPCValuesCheckValue(TextBox10, STL10, STM10)) {
            $("#LR10").val("OK");
            document.getElementById("LR10").style.backgroundColor = "lime";
        }
        else {
            $("#LR10").val("NG");
            document.getElementById("LR10").style.backgroundColor = "red";
        }
    }
    catch (e) {
        alert(e);
    }
    Form_RUS();
}
function Buttonclose_Click() {
    localStorage.setItem("C02_SPC_Close", 1);
    window.close();
}
