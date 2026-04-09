let IsTableSort = false;
let BaseResult = new Object();
let OR_IDX
$(window).focus(function () {
}).blur(function () {
    Buttonclose_Click();
});

$(document).ready(function () {
    localStorage.setItem("C11_SPC_R_Close", 0);

    
    localStorage.setItem("C11_2_SPC_EXIT", false);

    document.getElementById("TextBox1").disabled = true;
    document.getElementById("TextBox2").disabled = true;
    document.getElementById("TextBox3").disabled = true;
    document.getElementById("TextBox4").disabled = true;
  

    $("#ST11").val(localStorage.getItem("C11_2_ST_DCC1"));
    $("#ST12").val(localStorage.getItem("C11_2_ST_DIC1"));
    $("#Label8").val(localStorage.getItem("C11_VLW"));
    $("#Label18").val(localStorage.getItem("C11_2_WIRE_Length"));
    $("#Label19").val(localStorage.getItem("C11_2_ST_DWIRE1"));

    let BBBB = [];
    let CCCC = [];
    let LLLL = [];
    let MMMM = [];

    BBBB.push($("#ST11").val());
    BBBB.push($("#ST12").val());  

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
     

        $("#STL1").val(LLLL[0]);
        $("#STL2").val(LLLL[1]);
        $("#STL3").val(LLLL[2]);
        $("#STL4").val(LLLL[3]);
     
        $("#STM1").val(MMMM[0]);
        $("#STM2").val(MMMM[1]);
        $("#STM3").val(MMMM[2]);
        $("#STM4").val(MMMM[3]);
        
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
       

        $("#Label4").val(localStorage.getItem("C11_2_VLT1"));
        $("#Label5").val(localStorage.getItem("C11_2_VLT1"));
      

        let Label4 = $("#Label4").val();
       

        let BBB = Label4.includes("(");
       
        if (BBB == true) {
            document.getElementById("TextBox1").disabled = false;
            document.getElementById("TextBox2").disabled = false;
            document.getElementById("TextBox3").disabled = false;
            document.getElementById("TextBox4").disabled = false;
        }

        

        $("#TextBox1").val("");
        $("#TextBox2").val("");
        $("#TextBox3").val("");
        $("#TextBox4").val("");       
        $("#TextBox10").val("");

        $("#LR1").val("");
        $("#LR2").val("");
        $("#LR3").val("");
        $("#LR4").val("");      
        $("#LR9").val("");

        document.getElementById("MRUS").style.backgroundColor = "white";

        document.getElementById("LR1").style.backgroundColor = "white";
        document.getElementById("LR2").style.backgroundColor = "white";
        document.getElementById("LR3").style.backgroundColor = "whitesmoke";
        document.getElementById("LR4").style.backgroundColor = "whitesmoke";     
        document.getElementById("LR9").style.backgroundColor = "white";

        
        C02_SPC_Load();

        if ((BBB == false)) {
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
        $("#TextBox1").focus();
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
    let url = "/C11_SPC_R/C02_SPC_Load";

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

function Form_RUS() {
    let CHK = [];
    let VAL = [];
    CHK.push(document.getElementById("TextBox1").disabled);
    CHK.push(document.getElementById("TextBox2").disabled);
    CHK.push(document.getElementById("TextBox3").disabled);
    CHK.push(document.getElementById("TextBox4").disabled);
    CHK.push(document.getElementById("TextBox10").disabled);

    let AA = [];
    AA.push($("#LR1").val());
    AA.push($("#LR2").val());
    AA.push($("#LR3").val());
    AA.push($("#LR4").val());
    AA.push($("#LR9").val());


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
    if ((VAL[0] == true) && (VAL[1] == true) && (VAL[2] == true) && (VAL[3] == true) && (VAL[4] == true)) {
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
    let TextBox10 = $("#TextBox10").val();

    let Label6 = $("#Label6").val();
    let Label10 = $("#Label10").val();  

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
            BaseParameter.ListSearchString.push(TextBox10);
            BaseParameter.ListSearchString.push(Label6);
            BaseParameter.ListSearchString.push(Label10);            
            BaseParameter.ListSearchString.push(MRUS);
            BaseParameter.ListSearchString.push(OR_IDX);
            let formUpload = new FormData();
            formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
            let url = "/C11_SPC_R/Button1_Click";

            fetch(url, {
                method: "POST",
                body: formUpload,
                headers: {
                }
            }).then((response) => {
                response.json().then((data) => {
                    $("#BackGround").css("display", "none");
                    localStorage.setItem("CC11_2_SPC_EXIT", true);
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
    $("#TextBox10").focus();
}

$("#TextBox10").keydown(function (e) {
    if (e.keyCode == 13) {
        TextBox10_KeyDown();
    }
});
function TextBox10_KeyDown() {
    $("#TextBox1").focus();
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

$("#TextBox10").change(function (e) {
    TextBox10_TextChanged()
});
function TextBox10_TextChanged() {
    try {
        let AA = Number($("#TextBox10").val());
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
function Buttonclose_Click() {
    localStorage.setItem("C11_SPC_R_Close", 1);
    window.close();
}
