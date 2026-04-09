let IsTableSort = false;
let BaseResult = new Object();
let Timer1;
let Timer2;
let C_EXIT;
let DGV_RHRowIndex = 0;
let TI_CONT;
let APP_MAX;
let SPC_EXIT;
let TL_NM;
let MAX_COUNT;
$(window).focus(function () {
}).blur(function () {
    //Buttonclose_Click();
});

$(document).ready(function () {
    localStorage.setItem("C11_4_Close", 0);

    C_EXIT = localStorage.getItem("C11_4_C_EXIT");
    TL_NM = localStorage.getItem("C11_4_TL_NM");
    $("#C11_COUNT3").val(localStorage.getItem("C11_4_C11_COUNT3"));
    $("#C11_D01").val(localStorage.getItem("C11_4_C11_D01"));
    $("#C11_COUNT1").val(localStorage.getItem("C11_4_C11_COUNT1"));
    $("#ORDER").val(localStorage.getItem("C11_4_ORDER"));

    document.getElementById("Buttonprint").disabled = false;
    SettingsMC_NM = localStorage.getItem("SettingsMC_NM");
    $("#C11_D02").val(SettingsMC_NM);
    ORDER_COUNT();
    Timer2StartInterval();
    ORDER_LOAD(0);

});
function ORDER_COUNT() {

    let ORDER = $("#ORDER").val();
    //$("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        ListSearchString: [],
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    BaseParameter.ListSearchString.push(ORDER);
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C11_4/ORDER_COUNT";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            let BaseResultSub = data;
            BaseResult.Search = BaseResultSub.Search;
            if (BaseResult.Search.length > 0) {
                for (let i = 0; i < BaseResult.Search.length; i++) {
                    if (BaseResult.Search[i].LOC_LRJ == "L") {
                        $("#COUNT_L").val(BaseResult.Search[i].PERFORMN);
                    }
                    if (BaseResult.Search[i].LOC_LRJ == "R") {
                        $("#COUNT_R").val(BaseResult.Search[i].PERFORMN);
                    }
                }
            }
            //Timer2StartInterval();
            //$("#BackGround").css("display", "none");
        }).catch((err) => {
            //$("#BackGround").css("display", "none");
        })
    });
}
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
    let url = "/C11_4/ORDER_LOAD";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            let BaseResultSub = data;
            BaseResult.Search1 = BaseResultSub.Search1;
            if (BaseResult.Search1.length <= 0) {
                IsCheck = false;
                Buttonclose_Click();
            }
            else {
                try {                    
                    $("#VLA1").val(BaseResult.Search1[0].APP);
                    $("#VLA11").val(BaseResult.Search1[0].SEQ);
                    $("#TOOL_CONT").val(BaseResult.Search1[0].COUNT);
                    $("#C11_COUNT2").val(BaseResult.Search1[0].PERFORMN);
                    APP_MAX = BaseResult.Search1[0].MAX;

                    if ($("#TOOL_CONT").val() == "") {
                        $("#TOOL_CONT").val(0);
                    }
                    if ($("#C11_COUNT2").val() == "") {
                        $("#C11_COUNT2").val(0);
                    }
                    let COUNT_L = Number($("#COUNT_L").val());
                    let COUNT_R = Number($("#COUNT_R").val());
                    if (COUNT_L > COUNT_R) {
                        MAX_COUNT = COUNT_R - BaseResult.Search1[0].PERFORMN;
                    }
                    else {
                        MAX_COUNT = COUNT_L - BaseResult.Search1[0].PERFORMN;
                    }                 
                    if (BaseResult.Search1[0].PERFORMN == BaseResult.Search1[0].TOT_COUNT) {
                        IsCheck = false;
                        alert("작업을 종료 하였습니다. Đã dừng làm việc.");
                        Buttonclose_Click();
                    }

                }
                catch (e) {
                    $("#VLA1").val("");
                    $("#VLA11").val("");
                    $("#TOOL_CONT").val(0);
                    $("#C11_COUNT2").val(0);
                    IsCheck = false;
                    alert("Please Check Again." + e);
                    Buttonclose_Click();
                }
            }
            if (IsCheck == true) {
                if (Flag == 0) {
                    if ($("#VLA1").val() == "") {
                        BT_APP1_Click();
                    }
                }
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}

$("#Buttonprint").click(function (e) {
    Buttonprint_Click();
});
function Buttonprint_Click() {
    let IsCheck = true;
    document.getElementById("Buttonprint").disabled = true;
    if ($("#VLA1").val() == "") {
        IsCheck = false;
        alert("APPLICATOR data null. Please Check Again.");
        document.getElementById("Buttonprint").disabled = false;
    }
    if (IsCheck == true) {
        let C11_COUNT3 = Number($("#C11_COUNT3").val());
        if (C11_COUNT3 > MAX_COUNT) {
            IsCheck = false;
            alert("실적처리 오류(WORK_COUNT). Không thể kết nối MES(WORK_COUNT).");
            document.getElementById("Buttonprint").disabled = false;
        }
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
            let Label8 = $("#Label8").val();
            $("#BackGround").css("display", "block");
            let BaseParameter = new Object();
            BaseParameter = {
                ListSearchString: [],
            }
            BaseParameter.USER_ID = GetCookieValue("UserID");
            BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
            BaseParameter.ListSearchString.push(SettingsMC_NM);
            BaseParameter.ListSearchString.push(Label8);
            let formUpload = new FormData();
            formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
            let url = "/C11_4/Buttonprint_Click";

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
                        ORDER_LOAD(1);
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
}

$("#BT_APP1").click(function (e) {
    BT_APP1_Click();
});
function BT_APP1_Click() {

    localStorage.setItem("C11_APPLICATION_Close", 0);
    localStorage.setItem("C11_APPLICATION_ORDER_IDX", $("#ORDER").val());
    localStorage.setItem("C11_APPLICATION_Label1", $("#VLA1").val());
    localStorage.setItem("C11_APPLICATION_Label2", $("#VLA11").val());
    localStorage.setItem("C11_APPLICATION_Label3", "APPLICATION #J");
    let url = "/C11_APPLICATION";
    OpenWindowByURL(url, 800, 460);
    C11_APPLICATIONTimerStartInterval();
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
function Timer1StartInterval() {
    Timer1 = setInterval(function () {
        Timer1_Tick();
    }, 100);
}
function Timer1_Tick() {
    if (TI_CONT >= 30) {
        document.getElementById("Buttonprint").disabled = false;
        clearInterval(Timer1);
    }
    else {
        TI_CONT = TI_CONT + 1;
        document.getElementById("Buttonprint").innerHTML = "Complete" + "  (" + TI_CONT + ")";
    }
}
function Timer2StartInterval() {
    Timer2 = setInterval(function () {
        Timer2_Tick();
    }, 5000);
}
function Timer2_Tick() {
    ORDER_COUNT();
    //Timer2StartInterval();
}
$("#Buttonclose").click(function (e) {
    Buttonclose_Click();
});
function Buttonclose_Click() {
    C11_4_FormClosed();
}
function C11_4_FormClosed() {
    localStorage.setItem("C11_4_Close", 1);
    window.close();
}

