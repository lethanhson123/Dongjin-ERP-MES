let IsTableSort = false;
let BaseResult = new Object();
let Timer1;
let C_EXIT;
let DGV_LHRowIndex = 0;
let TI_CONT;
let APP_MAX;
let SPC_EXIT;
let C11_SPC_L_Flag = 0;

$(window).focus(function () {
}).blur(function () {
    //Buttonclose_Click();
});

$(document).ready(function () {
    $(".modal").modal({
        dismissible: false
    });
    localStorage.setItem("C11_1_Close", 0);

    BaseResult.R_DGV = JSON.parse(localStorage.getItem("C11_R_DGV"));
    BaseResult.DGV_LH = JSON.parse(localStorage.getItem("C11_1_DGV_LH"));
    DGV_LHRender();

    C_EXIT = localStorage.getItem("C11_1_C_EXIT");
    $("#C11_D04").val(localStorage.getItem("C11_1_C11_D04"));
    $("#ST_DWIRE1").val(localStorage.getItem("C11_1_ST_DWIRE1"));
    $("#WIRE_Length").val(localStorage.getItem("C11_1_WIRE_Length"));
    $("#ST_DSTR1").val(localStorage.getItem("C11_1_ST_DSTR1"));
    $("#ST_DCC1").val(localStorage.getItem("C11_1_ST_DCC1"));
    $("#ST_DIC1").val(localStorage.getItem("C11_1_ST_DIC1"));
    $("#C11_COUNT1").val(localStorage.getItem("C11_1_C11_COUNT1"));
    $("#C11_COUNT3").val(localStorage.getItem("C11_1_C11_COUNT3"));
    $("#C11_D01").val(localStorage.getItem("C11_1_C11_D01"));
    $("#ORDER").val(localStorage.getItem("C11_1_ORDER"));


    SettingsMC_NM = localStorage.getItem("SettingsMC_NM");
    $("#C11_D02").val(SettingsMC_NM);

    ORDER_LOAD(0);

});

function ORDER_LOAD(Flag) {
    let IsCheck = true;
    let ORDER = $("#ORDER").val();
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        ListSearchString: [],
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    BaseParameter.ListSearchString.push(ORDER);

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C11_1/ORDER_LOAD";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            let BaseResultSub = data;
            BaseResult.Search = BaseResultSub.Search;
            if (BaseResult.Search.length <= 0) {
                IsCheck = false;
                alert("오류가 발생 하였습니다. Một lỗi đã xảy ra.");
                Buttonclose_Click();
            }
            else {
                try {
                    $("#VLA1").val(BaseResult.Search[0].APP);
                    $("#VLA11").val(BaseResult.Search[0].SEQ);
                    $("#TOOL_CONT").val(BaseResult.Search[0].COUNT);
                    $("#C11_COUNT2").val(BaseResult.Search[0].PERFORMN);
                    $("#VLT1").val($("#C11_D04").val());

                    if ($("#TOOL_CONT").val() == "") {
                        $("#TOOL_CONT").val(0);
                    }
                    if ($("#C11_COUNT2").val() == "") {
                        $("#C11_COUNT2").val(0);
                    }
                    $("#Label1").val(Number($("#C11_COUNT2").val()) * BaseResult.DGV_LH.length);
                    if (BaseResult.Search[0].PERFORMN == BaseResult.Search[0].TOT_COUNT) {
                        IsCheck = false;
                        alert("작업을 종료 하였습니다. Đã dừng làm việc.");
                        Buttonclose_Click();
                    }
                }
                catch (e) {
                    IsCheck = false;
                    $("#VLA1").val("");
                    $("#VLA11").val("");
                    $("#TOOL_CONT").val("");
                    $("#C11_COUNT2").val("");
                    $("#VLT1").val("");
                    localStorage.setItem("C11_ER_L_LBT2S", BaseResult.R_DGV[0].TERM1);
                    localStorage.setItem("C11_ER_L_LBA2S", BaseResult.R_DGV[0].TERM1);
                    localStorage.setItem("C11_ER_L_Label1", localStorage.getItem("C11_ST_BCIDX"));

                    BaseResult.DGV_LH = [];
                    DGV_LHRender();

                    localStorage.setItem("C11_ER_L_Close", 0);
                    localStorage.setItem("C11_ER_L_LBT2S", BaseResult.R_DGV[0].TERM1);
                    localStorage.setItem("C11_ER_L_LBA2S", BaseResult.R_DGV[0].TERM1);
                    localStorage.setItem("C11_ER_L_Label1", localStorage.getItem("C11_ST_BCIDX"));

                    let url = "/C11_ER_L";
                    OpenWindowByURL(url, 800, 460);

                    Buttonclose_Click();
                }
            }

            if (IsCheck == true) {
                if (Flag == 0) {
                    if (C_EXIT == true) {
                        IsCheck = false;
                        Buttonclose_Click();
                    }
                    if (IsCheck == true) {
                        SPC_LOAD();
                    }
                }
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
function SPC_LOAD() {
    let ORDER = $("#ORDER").val();
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        ListSearchString: [],
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    BaseParameter.ListSearchString.push(ORDER);
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C11_1/SPC_LOAD";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            let BaseResultSub = data;
            BaseResult.Search1 = BaseResultSub.Search1;
            try {
                if (Number($("#C11_COUNT1").val()) < 11) {
                    document.getElementById("SPC1").innerHTML = "----";
                    document.getElementById("SPC1").disabled = true;
                }
                else {
                    document.getElementById("SPC1").innerHTML = "First";
                    document.getElementById("SPC1").disabled = false;
                }
                if (Number($("#C11_COUNT1").val()) < 501) {
                    document.getElementById("SPC2").innerHTML = "----";
                    document.getElementById("SPC2").disabled = true;
                }
                else {
                    document.getElementById("SPC2").innerHTML = "Middle";
                    document.getElementById("SPC2").disabled = false;
                }
            }
            catch (e) {

            }
            if (BaseResult.Search1.length > 0) {                
                for (let i = 0; i < BaseResult.Search1.length; i++) {                    
                    if (BaseResult.Search1[i].COLSIP == "First") {
                        document.getElementById("SPC1").innerHTML = "Complete";
                        document.getElementById("SPC1").disabled = true;
                    }
                    if (BaseResult.Search1[i].COLSIP == "Middle") {
                        document.getElementById("SPC2").innerHTML = "Complete";
                        document.getElementById("SPC2").disabled = true;
                    }
                    if (BaseResult.Search1[i].COLSIP == "End") {
                        document.getElementById("SPC3").innerHTML = "Complete";
                        document.getElementById("SPC3").disabled = true;
                    }
                }
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
function Timer1StartInterval() {
    Timer1 = setInterval(function () {
        Timer1_Tick();
    }, 200);
}
function Timer1_Tick() {
    if (TI_CONT >= 60) {
        document.getElementById("Buttonprint").disabled = false;        
        clearInterval(Timer1);
    }
    else {
        TI_CONT = TI_CONT + 1;        
        document.getElementById("Buttonprint").innerHTML = "Complete" + "  (" + TI_CONT + ")";        
    }
}
function C11_1_FormClosed() {
    localStorage.setItem("C11_1_Close", 1);
    window.close();
}

$("#SPC1").click(function (e) {
    SPC1_Click();
});
function SPC1_Click() {
    localStorage.setItem("C11_SPC_L_Close", 0);
    localStorage.setItem("C11_SPC_L_Label10", "First");
    localStorage.setItem("C11_SPC_L_Label6", $("#C11_D03").val());

    //let url = "/C11_SPC_L";
    //OpenWindowByURL(url, 800, 460);
    //C11_SPC_LTimerStartInterval(0);

    $("#C11_SPC_L_Label10").val("First");
    $("#C11_SPC_L_Label6").val($("#C11_D03").val());
    C11_SPC_L_Load();
    $("#C11_SPC_L").modal("open");
}
function C11_SPC_LTimerStartInterval(Flag) {
    let C11_SPC_LTimer = setInterval(function () {
        let C11_SPC_L_Close = localStorage.getItem("C11_SPC_L_Close");
        if (C11_SPC_L_Close == "1") {
            clearInterval(C11_SPC_LTimer);
            C11_SPC_L_Close = 0;
            localStorage.setItem("C11_SPC_L_Close", C11_SPC_L_Close);
            if (Flag == 0) {
                SPC_LOAD();
            }
            if (Flag == 1) {
                Buttonprint_ClickSub();
            }
        }
    }, 100);
}
function C11_SPC_L_Close() {
    if (C11_SPC_L_Flag == 0) {
        SPC_LOAD();
    }
    if (C11_SPC_L_Flag == 1) {
        Buttonprint_ClickSub();
    }
}
$("#SPC2").click(function (e) {
    SPC2_Click();
});
function SPC2_Click() {
    let C11_COUNT1 = Number($("#C11_COUNT1").val());
    let C11_COUNT2 = Number($("#C11_COUNT2").val());
    if (C11_COUNT1 > 501) {
        if (C11_COUNT1 / 2 <= C11_COUNT2) {
            localStorage.setItem("C11_SPC_L_Close", 0);
            localStorage.setItem("C11_SPC_L_Label10", "Middle");
            localStorage.setItem("C11_SPC_L_Label6", $("#C11_D03").val());
            //let url = "/C11_SPC_L";
            //OpenWindowByURL(url, 800, 460);
            //C11_SPC_LTimerStartInterval(0);

            $("#C11_SPC_L_Label10").val("Middle");
            $("#C11_SPC_L_Label6").val($("#C11_D03").val());
            C11_SPC_L_Load();
            $("#C11_SPC_L").modal("open");
        }
    }
}
$("#SPC3").click(function (e) {
    SPC3_Click();
});
function SPC3_Click() {
    let C11_COUNT1 = Number($("#C11_COUNT1").val());
    let C11_COUNT2 = Number($("#C11_COUNT2").val());
    let C11_COUNT3 = Number($("#C11_COUNT3").val());
    let AA = C11_COUNT1 - C11_COUNT2;
    let BB = AA - C11_COUNT3;
    if (BB <= 0) {
        localStorage.setItem("C11_SPC_L_Close", 0);
        localStorage.setItem("C11_SPC_L_Label10", "End");
        localStorage.setItem("C11_SPC_L_Label6", $("#C11_D03").val());
        //let url = "/C11_SPC_L";
        //OpenWindowByURL(url, 800, 460);
        //C11_SPC_LTimerStartInterval(0);

        $("#C11_SPC_L_Label10").val("End");
        $("#C11_SPC_L_Label6").val($("#C11_D03").val());
        C11_SPC_L_Load();
        $("#C11_SPC_L").modal("open");
    }
}
$("#Buttonprint").click(function (e) {
    Buttonprint_Click();
});
function Buttonprint_Click() {
    C11_SPC_L_Flag = 1;
    let IsCheck = true;
    document.getElementById("Buttonprint").disabled = true;
    if ($("#VLA1").val() == "") {
        IsCheck = false;
        alert("APPLICATOR data null. Please Check Again.");
        document.getElementById("Buttonprint").disabled = false;
    }
    if (IsCheck == true) {
        let TOOLAPP = 0;
        let TOOLAPP_CNT = Number($("#TOOL_CONT").val());
        try {
            TOOLAPP = APP_MAX;
        }
        catch (e) {
            TOOLAPP = 1000000
        }
        if (TOOLAPP <= TOOLAPP_CNT) {
            IsCheck = false;
            alert("Application check(Check counter). Please Check Again.");
            document.getElementById("Buttonprint").disabled = false;
        }
        if (IsCheck == true) {
            SPC_EXIT = true;
            if (document.getElementById("SPC1").innerHTML == "First") {
                localStorage.setItem("C11_SPC_L_Close", 0);
                localStorage.setItem("C11_SPC_L_Label10", "First");
                localStorage.setItem("C11_SPC_L_Label6", $("#C11_D03").val());
                //let url = "/C11_SPC_L";
                //OpenWindowByURL(url, 800, 460);
                //C11_SPC_LTimerStartInterval(1);

                $("#C11_SPC_L_Label10").val("First");
                $("#C11_SPC_L_Label6").val($("#C11_D03").val());
                C11_SPC_L_Load();
                $("#C11_SPC_L").modal("open");
                IsCheck = false;
            }
            let C11_COUNT1 = Number($("#C11_COUNT1").val());
            let C11_COUNT2 = Number($("#C11_COUNT2").val());

            if (C11_COUNT1 > 500) {            
                if (C11_COUNT1 / 2 <= C11_COUNT2) {
                    if (document.getElementById("SPC2").innerHTML == "Middle") {
                        localStorage.setItem("C11_SPC_L_Close", 0);
                        localStorage.setItem("C11_SPC_L_Label10", "Middle");
                        localStorage.setItem("C11_SPC_L_Label6", $("#C11_D03").val());
                        //let url = "/C11_SPC_L";
                        //OpenWindowByURL(url, 800, 460);
                        //C11_SPC_LTimerStartInterval(1);

                        $("#C11_SPC_L_Label10").val("Middle");
                        $("#C11_SPC_L_Label6").val($("#C11_D03").val());
                        C11_SPC_L_Load();
                        $("#C11_SPC_L").modal("open");
                        IsCheck = false;
                    }
                }
            }
            let A_CSPC = C11_COUNT1 - C11_COUNT2;
            let C11_COUNT3 = Number($("#C11_COUNT3").val());
            if (C11_COUNT3 * 2 >= A_CSPC) {
                if (document.getElementById("SPC3").innerHTML == "End") {
                    localStorage.setItem("C11_SPC_L_Close", 0);
                    localStorage.setItem("C11_SPC_L_Label10", "End");
                    localStorage.setItem("C11_SPC_L_Label6", $("#C11_D03").val());
                    //let url = "/C11_SPC_L";
                    //OpenWindowByURL(url, 800, 460);
                    //C11_SPC_LTimerStartInterval(1);

                    $("#C11_SPC_L_Label10").val("End");
                    $("#C11_SPC_L_Label6").val($("#C11_D03").val());
                    C11_SPC_L_Load();
                    $("#C11_SPC_L").modal("open");
                    IsCheck = false;
                }
            }
            if (IsCheck == true) {
                Buttonprint_ClickSub();
            }
        }
    }
}
function Buttonprint_ClickSub() {
    let IsCheck = true;
    if (SPC_EXIT == false) {
        IsCheck = false;
        alert("Application check(Check counter). Please Check Again.");
        document.getElementById("Buttonprint").disabled = false;
    }
    if (IsCheck == true) {
        let C11_COUNT3 = Number($("#C11_COUNT3").val());
        let TM_QTY = C11_COUNT3 - BaseResult.DGV_LH.length;
        $("#Label1").val(TM_QTY);

        let ORDER = $("#ORDER").val();
        let VLA1 = $("#VLA1").val();
        let VLA11 = $("#VLA11").val();
        let C11_D01 = $("#C11_D01").val();
        let TOOL_CONT = $("#TOOL_CONT").val();
        let C11_D02 = $("#C11_D02").val();
        let VLT1 = $("#VLT1").val();

        $("#BackGround").css("display", "block");
        let BaseParameter = new Object();
        BaseParameter = {
            ListSearchString: [],
        }
        BaseParameter.USER_ID = GetCookieValue("UserID");
        BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
        BaseParameter.ListSearchString.push(C11_COUNT3);
        BaseParameter.ListSearchString.push(ORDER);
        BaseParameter.ListSearchString.push(TM_QTY);
        BaseParameter.ListSearchString.push(VLA1);
        BaseParameter.ListSearchString.push(VLA11);
        BaseParameter.ListSearchString.push(C11_D01);
        BaseParameter.ListSearchString.push(TOOL_CONT);
        BaseParameter.ListSearchString.push(C11_D02);
        BaseParameter.ListSearchString.push(VLT1);
        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        let url = "/C11_1/Buttonprint_Click";

        fetch(url, {
            method: "POST",
            body: formUpload,
            headers: {
            }
        }).then((response) => {
            response.json().then((data) => {
                document.getElementById("Buttonprint").disabled = true;
                TI_CONT = 0;
                Timer1StartInterval();
                M.toast({ html: "정상처리 되었습니다. Đã được lưu.", classes: 'green', displayLength: 6000 });                
                try {
                    SPC_LOAD();
                }
                catch (e) {
                    alert("#1 Please Check Again. " + e);
                    document.getElementById("Buttonprint").disabled = false;
                }
                try {
                    ORDER_LOAD();
                }
                catch (e) {
                    alert("#2 Please Check Again. " + e);
                    document.getElementById("Buttonprint").disabled = false;
                }
                $("#BackGround").css("display", "none");
            }).catch((err) => {
                $("#BackGround").css("display", "none");
            })
        });
    }
}
function C11_APPLICATIONTimerStartInterval() {
    let C11_APPLICATIONTimer = setInterval(function () {
        let C11_APPLICATION_Close = localStorage.getItem("C11_APPLICATION_Close");
        if (C11_APPLICATION_Close == "1") {
            clearInterval(C11_APPLICATIONTimer);
            C11_APPLICATION_Close = 0;
            localStorage.setItem("C11_APPLICATION_Close", C11_APPLICATION_Close);
            ORDER_LOAD(1);
        }
    }, 100);
}

$("#BT_APP1").click(function (e) {
    BT_APP1_Click();
});
function BT_APP1_Click() {
    let IsCheck = true;
    if ($("#VLA1").val() == "") {
        IsCheck = false;
    }
    if (IsCheck == true) {
        localStorage.setItem("C11_APPLICATION_Close", 0);
        localStorage.setItem("C11_APPLICATION_ORDER_IDX", $("#ORDER").val());
        localStorage.setItem("C11_APPLICATION_Label1", $("#VLA1").val());
        localStorage.setItem("C11_APPLICATION_Label2", $("#VLA11").val());
        localStorage.setItem("C11_APPLICATION_Label3", "APPLICATION #L");
        let url = "/C11_APPLICATION";
        OpenWindowByURL(url, 800, 460);
        C11_APPLICATIONTimerStartInterval();
    }
}
function C11_APPLICATIONTimerStartInterval() {
    let C11_APPLICATIONTimer = setInterval(function () {
        let C11_APPLICATION_Close = localStorage.getItem("C11_APPLICATION_Close");
        if (C11_APPLICATION_Close == "1") {
            clearInterval(C11_APPLICATIONTimer);
            C11_APPLICATION_Close = 0;
            localStorage.setItem("C11_APPLICATION_Close", C11_APPLICATION_Close);
            ORDER_LOAD(1);
        }
    }, 100);
}
$("#Buttonclose").click(function (e) {
    Buttonclose_Click();
});
function Buttonclose_Click() {
    C11_1_FormClosed();
}

function DGV_LHRender() {
    let HTML = "";
    if (BaseResult) {
        if (BaseResult.DGV_LH) {
            if (BaseResult.DGV_LH.length > 0) {
                DGV_LH_SelectionChanged(0);
                for (let i = 0; i < BaseResult.DGV_LH.length; i++) {
                    HTML = HTML + "<tr onclick='DGV_LH_SelectionChanged(" + i + ")'>";
                    HTML = HTML + "<td>" + BaseResult.DGV_LH[i].NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DGV_LH[i].TERM1 + "</td>";
                    HTML = HTML + "</tr>";
                }
            }
        }
    }
    document.getElementById("DGV_LH").innerHTML = HTML;
}
function DGV_LH_SelectionChanged(i) {
    DGV_LHRowIndex = i;
}
let DGV_LHTable = document.getElementById("DGV_LHTable");
DGV_LHTable.addEventListener('click', function (event) {
    if (event.target.tagName === 'TH') {
        let text = event.target.innerText;
        let key = "NO";
        IsTableSort = !IsTableSort;
        ListSort(BaseResult.DGV_LH, key, text, IsTableSort);
        DGV_LHRender();
    }
});


function C11_SPC_L_Load() {
    localStorage.setItem("C11_SPC_L_Close", 0);


    localStorage.setItem("C11_1_SPC_EXIT", false);
    SPC_EXIT = false;

    document.getElementById("C11_SPC_L_TextBox1").disabled = false;
    document.getElementById("C11_SPC_L_TextBox2").disabled = false;
    document.getElementById("C11_SPC_L_TextBox3").disabled = false;
    document.getElementById("C11_SPC_L_TextBox4").disabled = false;


    $("#C11_SPC_L_ST11").val($("#ST_DCC1").val());
    $("#C11_SPC_L_ST12").val($("#ST_DIC1").val());
    $("#C11_SPC_L_Label8").val(localStorage.getItem("C11_VLW"));
    $("#C11_SPC_L_Label18").val($("#WIRE_Length").val());
    $("#C11_SPC_L_Label19").val($("#ST_DWIRE1").val());

    let BBBB = [];
    let CCCC = [];
    let LLLL = [];
    let MMMM = [];

    BBBB.push($("#C11_SPC_L_ST11").val());
    BBBB.push($("#C11_SPC_L_ST12").val());

    let OO = 0;
    for (let i = 0; i < 2; i++) {
        let AA = BBBB[i];
        try {
            let BB = AA.split("/")[0];
            let CC = BB.split("±")[0];
            let CCC = "";
            let DD = BB.split("±")[1];
            try {
                CCC = CC.split("(")[0];
            }
            catch (e) {
                CCC = CC;
            }
            OO = OO + 1;
            CCCC.push(CCC);
            let CCCDD = Number(CCC) - Number(DD);
            LLLL.push(CCCDD.toFixed(2));
            CCCDD = Number(CCC) + Number(DD);
            MMMM.push(CCCDD.toFixed(2));


            let BB1 = AA.split("/")[1];
            let CC1 = BB1.split("±")[0];
            let CCC1 = "";
            let DD1 = BB1.split("±")[1];
            try {
                CCC1 = CC1.split("(")[0];
            }
            catch (e) {
                CCC1 = CC1;
            }
            OO = OO + 1
            CCCC.push(CCC1);
            let CCC1DD1 = Number(CCC1) - Number(DD1);
            LLLL.push(CCC1DD1.toFixed(2));
            CCC1DD1 = Number(CCC1) + Number(DD1);
            MMMM.push(CCC1DD1.toFixed(2));
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
    }
    $("#C11_SPC_L_STS1").val(CCCC[0]);
    $("#C11_SPC_L_STS2").val(CCCC[1]);
    $("#C11_SPC_L_STS3").val(CCCC[2]);
    $("#C11_SPC_L_STS4").val(CCCC[3]);


    $("#C11_SPC_L_STL1").val(LLLL[0]);
    $("#C11_SPC_L_STL2").val(LLLL[1]);
    $("#C11_SPC_L_STL3").val(LLLL[2]);
    $("#C11_SPC_L_STL4").val(LLLL[3]);

    $("#C11_SPC_L_STM1").val(MMMM[0]);
    $("#C11_SPC_L_STM2").val(MMMM[1]);
    $("#C11_SPC_L_STM3").val(MMMM[2]);
    $("#C11_SPC_L_STM4").val(MMMM[3]);

    if ($("#C11_SPC_L_STS1").val() == "") {
        document.getElementById("C11_SPC_L_TextBox1").disabled = true;
    }
    if ($("#C11_SPC_L_STS2").val() == "") {
        document.getElementById("C11_SPC_L_TextBox2").disabled = true;
    }
    if ($("#C11_SPC_L_STS3").val() == "") {
        document.getElementById("C11_SPC_L_TextBox3").disabled = true;
    }
    if ($("#C11_SPC_L_STS4").val() == "") {
        document.getElementById("C11_SPC_L_TextBox4").disabled = true;
    }


    $("#C11_SPC_L_Label4").val($("#VLT1").val());
    $("#C11_SPC_L_Label5").val($("#VLT1").val());


    let Label4 = $("#C11_SPC_L_Label4").val();


    let BBB = Label4.includes("(");

    if (BBB == true) {
        document.getElementById("C11_SPC_L_TextBox1").disabled = true;
        document.getElementById("C11_SPC_L_TextBox2").disabled = true;
        document.getElementById("C11_SPC_L_TextBox3").disabled = true;
        document.getElementById("C11_SPC_L_TextBox4").disabled = true;
    }

    $("#C11_SPC_L_TextBox1").val("");
    $("#C11_SPC_L_TextBox2").val("");
    $("#C11_SPC_L_TextBox3").val("");
    $("#C11_SPC_L_TextBox4").val("");
    $("#C11_SPC_L_TextBox10").val("");

    $("#C11_SPC_L_LR1").val("");
    $("#C11_SPC_L_LR2").val("");
    $("#C11_SPC_L_LR3").val("");
    $("#C11_SPC_L_LR4").val("");
    $("#C11_SPC_L_LR9").val("");

    document.getElementById("C11_SPC_L_MRUS").style.backgroundColor = "white";

    document.getElementById("C11_SPC_L_LR1").style.backgroundColor = "white";
    document.getElementById("C11_SPC_L_LR2").style.backgroundColor = "white";
    document.getElementById("C11_SPC_L_LR3").style.backgroundColor = "whitesmoke";
    document.getElementById("C11_SPC_L_LR4").style.backgroundColor = "whitesmoke";
    document.getElementById("C11_SPC_L_LR9").style.backgroundColor = "white";


    C02_SPC_Load();

    if ((BBB == false)) {
        $("#C11_SPC_L").modal("close");
    }

    if ($("#C11_SPC_L_STL1").val() == "") {
        document.getElementById("C11_SPC_L_TextBox1").disabled = true;
    }
    if ($("#C11_SPC_L_STL2").val() == "") {
        document.getElementById("C11_SPC_L_TextBox2").disabled = true;
    }
    if ($("#C11_SPC_L_STL3").val() == "") {
        document.getElementById("C11_SPC_L_TextBox3").disabled = true;
    }
    if ($("#C11_SPC_L_STL4").val() == "") {
        document.getElementById("C11_SPC_L_TextBox4").disabled = true;
    }
    $("#C11_SPC_L_TextBox1").focus();
}
function C02_SPC_Load() {
    let F_CO = $("#C11_SPC_L_Label19").val();
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        SearchString: F_CO,
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C11_SPC_L/C02_SPC_Load";

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
                $("#C11_SPC_L_Label17").val(0);
            }
            $("#C11_SPC_L_Label17").val(BaseResult.Search[0].STRENGTH);
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
function C11_SPC_L_Form_RUS() {
    let CHK = [];
    let VAL = [];
    CHK.push(document.getElementById("C11_SPC_L_TextBox1").disabled);
    CHK.push(document.getElementById("C11_SPC_L_TextBox2").disabled);
    CHK.push(document.getElementById("C11_SPC_L_TextBox3").disabled);
    CHK.push(document.getElementById("C11_SPC_L_TextBox4").disabled);
    CHK.push(document.getElementById("C11_SPC_L_TextBox10").disabled);

    let AA = [];
    AA.push($("#C11_SPC_L_LR1").val());
    AA.push($("#C11_SPC_L_LR2").val());
    AA.push($("#C11_SPC_L_LR3").val());
    AA.push($("#C11_SPC_L_LR4").val());
    AA.push($("#C11_SPC_L_LR9").val());


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
        $("#C11_SPC_L_MRUS").val("OK");
        document.getElementById("C11_SPC_L_MRUS").style.color = "green";
    }
    else {
        $("#C11_SPC_L_MRUS").val("NG");
        document.getElementById("C11_SPC_L_MRUS").style.color = "red";
    }
}
$("#C11_SPC_L_TextBox1").keydown(function (e) {
    if (e.keyCode == 13) {
        C11_SPC_L_TextBox1_KeyDown();
    }
});
function C11_SPC_L_TextBox1_KeyDown() {
    $("#C11_SPC_L_TextBox2").focus();
}
$("#C11_SPC_L_TextBox2").keydown(function (e) {
    if (e.keyCode == 13) {
        C11_SPC_L_TextBox2_KeyDown();
    }
});
function C11_SPC_L_TextBox2_KeyDown() {
    $("#C11_SPC_L_TextBox3").focus();
}
$("#C11_SPC_L_TextBox3").keydown(function (e) {
    if (e.keyCode == 13) {
        C11_SPC_L_TextBox3_KeyDown();
    }
});
function C11_SPC_L_TextBox3_KeyDown() {
    $("#C11_SPC_L_TextBox4").focus();
}
$("#C11_SPC_L_TextBox4").keydown(function (e) {
    if (e.keyCode == 13) {
        C11_SPC_L_TextBox4_KeyDown();
    }
});
function C11_SPC_L_TextBox4_KeyDown() {
    $("#C11_SPC_L_TextBox10").focus();
}

$("#C11_SPC_L_TextBox10").keydown(function (e) {
    if (e.keyCode == 13) {
        C11_SPC_L_TextBox10_KeyDown();
    }
});
function C11_SPC_L_TextBox10_KeyDown() {
    $("#C11_SPC_L_TextBox1").focus();
}
$("#C11_SPC_L_TextBox1").change(function (e) {
    C11_SPC_L_TextBox1_TextChanged()
});
function C11_SPC_L_TextBox1_TextChanged() {
    try {
        let TextBox1 = $("#C11_SPC_L_TextBox1").val();
        let STL1 = $("#C11_SPC_L_STL1").val();
        let STM1 = $("#C11_SPC_L_STM1").val();
        if (SPCValuesCheckValue(TextBox1, STL1, STM1)) {
            $("#C11_SPC_L_LR1").val("OK");
            document.getElementById("C11_SPC_L_LR1").style.color = "green";
        }
        else {
            $("#C11_SPC_L_LR1").val("NG");
            document.getElementById("C11_SPC_L_LR1").style.color = "red";
        }
    }
    catch (e) {
        alert(e);
    }
    C11_SPC_L_Form_RUS();
}
$("#C11_SPC_L_TextBox2").change(function (e) {
    C11_SPC_L_TextBox2_TextChanged()
});
function C11_SPC_L_TextBox2_TextChanged() {
    try {
        let TextBox2 = $("#C11_SPC_L_TextBox2").val();
        let STL2 = $("#C11_SPC_L_STL2").val();
        let STM2 = $("#C11_SPC_L_STM2").val();
        if (SPCValuesCheckValue(TextBox2, STL2, STM2)) {
            $("#C11_SPC_L_LR2").val("OK");
            document.getElementById("C11_SPC_L_LR2").style.color = "green";
        }
        else {
            $("#C11_SPC_L_LR2").val("NG");
            document.getElementById("C11_SPC_L_LR2").style.color = "red";
        }
    }
    catch (e) {
        alert(e);
    }
    C11_SPC_L_Form_RUS();
}
$("#C11_SPC_L_TextBox3").change(function (e) {
    C11_SPC_L_TextBox3_TextChanged()
});
function C11_SPC_L_TextBox3_TextChanged() {
    try {
        let TextBox3 = $("#C11_SPC_L_TextBox3").val();
        let STL3 = $("#C11_SPC_L_STL3").val();
        let STM3 = $("#C11_SPC_L_STM3").val();
        if (SPCValuesCheckValue(TextBox3, STL3, STM3)) {
            $("#C11_SPC_L_LR3").val("OK");
            document.getElementById("C11_SPC_L_LR3").style.color = "green";
        }
        else {
            $("#C11_SPC_L_LR3").val("NG");
            document.getElementById("C11_SPC_L_LR3").style.color = "red";
        }
    }
    catch (e) {
        alert(e);
    }
    C11_SPC_L_Form_RUS();
}
$("#C11_SPC_L_TextBox4").change(function (e) {
    C11_SPC_L_TextBox4_TextChanged()
});
function C11_SPC_L_TextBox4_TextChanged() {
    try {
        let TextBox4 = $("#C11_SPC_L_TextBox4").val();
        let STL4 = $("#C11_SPC_L_STL4").val();
        let STM4 = $("#C11_SPC_L_STM4").val();
        if (SPCValuesCheckValue(TextBox4, STL4, STM4)) {
            $("#C11_SPC_L_LR4").val("OK");
            document.getElementById("C11_SPC_L_LR4").style.color = "green";
        }
        else {
            $("#C11_SPC_L_LR4").val("NG");
            document.getElementById("C11_SPC_L_LR4").style.color = "red";
        }
    }
    catch (e) {
        alert(e);
    }
    C11_SPC_L_Form_RUS();
}

$("#C11_SPC_L_TextBox10").change(function (e) {
    C11_SPC_L_TextBox10_TextChanged()
});
function C11_SPC_L_TextBox10_TextChanged() {
    try {
        let AA = Number($("#C11_SPC_L_TextBox10").val());
        let CC = Number($("#C11_SPC_L_Label17").val());

        if (CC <= AA) {
            $("#C11_SPC_L_LR9").val("OK");
            document.getElementById("C11_SPC_L_LR9").style.color = "green";
        }
        else {
            $("#C11_SPC_L_LR9").val("NG");
            document.getElementById("C11_SPC_L_LR9").style.color = "red";
        }
    }
    catch (e) {
        alert(e);
    }
    C11_SPC_L_Form_RUS();
}
$("#C11_SPC_L_Button1").click(function (e) {
    C11_SPC_L_Button1_Click();
});
function C11_SPC_L_Button1_Click() {
    let IsCheck = true;
    let OR_IDX = localStorage.getItem("C11_ST_BCIDX");
    let TextBox1 = $("#C11_SPC_L_TextBox1").val();
    let TextBox2 = $("#C11_SPC_L_TextBox2").val();
    let TextBox3 = $("#C11_SPC_L_TextBox3").val();
    let TextBox4 = $("#C11_SPC_L_TextBox4").val();
    let TextBox10 = $("#C11_SPC_L_TextBox10").val();

    let Label6 = $("#C11_SPC_L_Label6").val();
    let Label10 = $("#C11_SPC_L_Label10").val();

    let MRUS = $("#C11_SPC_L_MRUS").val();

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
            let url = "/C11_SPC_L/Button1_Click";

            fetch(url, {
                method: "POST",
                body: formUpload,
                headers: {
                }
            }).then((response) => {
                response.json().then((data) => {
                    $("#BackGround").css("display", "none");
                    localStorage.setItem("CC11_1_SPC_EXIT", true);
                    SPC_EXIT = true;
                    $("#C11_SPC_L").modal("close");
                    C11_SPC_L_Close();
                }).catch((err) => {
                    $("#BackGround").css("display", "none");
                })
            });
        }
    }
}
