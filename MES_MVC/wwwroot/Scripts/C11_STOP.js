let IsTableSort = false;
let BaseResult;
let RowIndex;
let Timer1;
let EW_TIME_Timer1;
let Begin
let STOPW_ORING_IDX = 0;
$(window).focus(function () {
}).blur(function () {   
    C11_STOP_FormClosed();
});
$(document).ready(function () {

    localStorage.setItem("C11_STOP_Close", 0);

    $("#Label5").val(localStorage.getItem("C11_STOP_Label5"));
    $("#STOP_MC").val(localStorage.getItem("C11_STOP_STOP_MC"));
    $("#Label2").val(localStorage.getItem("C11_STOP_Label2"));

    var now = new Date();
    var day = ("0" + now.getDate()).slice(-2);
    var month = ("0" + (now.getMonth() + 1)).slice(-2);
    var today = now.getFullYear() + "-" + (month) + "-" + (day);
    Begin = now;

    $("#Label4").val("00:00:00");

    Timer1_Tick();
    PageLoad();
});
function C11_STOP_FormClosed() {
    let Label6 = $("#Label6").val();
    let STOP_MC = $("#STOP_MC").val();
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        ListSearchString: [],
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    BaseParameter.ListSearchString.push(Begin);
    BaseParameter.ListSearchString.push(Label6);
    BaseParameter.ListSearchString.push(STOP_MC);
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C11_STOP/C11_STOP_FormClosed";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            $("#BackGround").css("display", "none");
            SettingsNON_OPER_CHK = false;
            localStorage.setItem("SettingsNON_OPER_CHK", SettingsNON_OPER_CHK);
            clearInterval(Timer1);
            OPER_TIME();
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
function Timer1_Tick() {
    Timer1 = setInterval(function () {
        let End = new Date();
        $("#Label4").val(CounterByBegin_EndToString(Begin, End));
    }, 1000);
}
function PageLoad() {
    let STOP_MC = $("#STOP_MC").val();
    let Label5 = $("#Label5").val();
    let Label2 = $("#Label2").val();
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        ListSearchString: [],
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    BaseParameter.ListSearchString.push(STOP_MC);
    BaseParameter.ListSearchString.push(Label5);
    BaseParameter.ListSearchString.push(Label2);
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C11_STOP/PageLoad";

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
                    if (BaseResult.DataGridView1.length > 0) {
                        $("#Label6").val(BaseResult.DataGridView1[0].TSNON_OPER_IDX);
                        SettingsNON_OPER_CHK = true;
                        SettingsNON_OPER_IDX = BaseResult.DataGridView1[0].TSNON_OPER_IDX;
                        localStorage.setItem("SettingsNON_OPER_CHK", SettingsNON_OPER_CHK);
                        localStorage.setItem("SettingsNON_OPER_IDX", SettingsNON_OPER_IDX);
                    }
                }
            }
            SW_TIME();
            EW_TIME_Timer1StartInterval();
            document.getElementById("Button2").disabled = true;
            if ($("#Label5").val() == "M") {
                document.getElementById("Button2").disabled = false;
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });

}
function EW_TIME_Timer1StartInterval() {
    EW_TIME_Timer1 = setInterval(function () {
        EW_TIME_Timer1_Tick();
    }, 60000);
}
function EW_TIME_Timer1_Tick() {
    EW_TIME(0);
}
function EW_TIME(Flag) {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        SearchString: STOPW_ORING_IDX,
    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C11_STOP/EW_TIME";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            $("#BackGround").css("display", "none");
            if (Flag == 1) {
                localStorage.setItem("C11_STOP_Close", 1);
                window.close();
            }
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
function SW_TIME() {
    let USER_MC = localStorage.getItem("C02_MCbox");
    let USER_ORIDX = localStorage.getItem("C02_START_V2_Label8");
    let Label5 = $("#Label5").val();;
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        ListSearchString: [],
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    BaseParameter.ListSearchString.push(USER_MC);
    BaseParameter.ListSearchString.push(USER_ORIDX);
    BaseParameter.ListSearchString.push(Label5);
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C11_STOP/SW_TIME";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            BaseResult.DGV_WT = data.DGV_WT;
            try {
                STOPW_ORING_IDX = data.DGV_WT[0].TOWT_INDX;
            }
            catch (e) {
                STOPW_ORING_IDX = 0;
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}

function OPER_TIME() {
    let STOP_MC = $("#STOP_MC").val();
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        SearchString: STOP_MC,
    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C11_STOP/OPER_TIME";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            $("#BackGround").css("display", "none");
            try {
                BaseResult.Search = data.Search;
                let TOT_SUM = BaseResult.Search[0].SUM_TIME;

                let H_TIME = Math.floor(TOT_SUM / 60 / 60);
                TOT_SUM = TOT_SUM - (H_TIME * 60 * 60);
                let M_TIME = Math.floor(TOT_SUM / 60);
                let S_TIME = TOT_SUM - (M_TIME * 60);
                let C11_BI_STOPTIMESub = String(H_TIME).padStart(2, '0') + String(M_TIME).padStart(2, '0') + String(S_TIME).padStart(2, '0');
                let H_TIME1 = C11_BI_STOPTIMESub.substr(0, 2);
                let M_TIME1 = C11_BI_STOPTIMESub.substr(2, 2);
                let S_TIME1 = C11_BI_STOPTIMESub.substr(4, 2);               
                let C11_BI_STOPTIME = H_TIME1 + ":" + M_TIME1 + ":" + S_TIME1;
                localStorage.setItem("C11_BI_STOPTIME", C11_BI_STOPTIME);
            }
            catch (e) {
            }
            EW_TIME(1);
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
$("#Button2").click(function () {
    Button2_Click();
});
function Button2_Click() {
    $("#STOP_MC").val("점검 중 / Đang kiểm tra");
    let STOP_MC = $("#STOP_MC").val();
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        SearchString: STOP_MC,
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C11_STOP/Button2_Click";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
$("#Button1").click(function () {
    Button1_Click();
});
function Button1_Click() {
    let Label5 = $("#Label5").val();
    if (Label5 == "M") {
        let STOP_MC = $("#STOP_MC").val();
        $("#BackGround").css("display", "block");
        let BaseParameter = new Object();
        BaseParameter = {
            SearchString: STOP_MC,
        }
        BaseParameter.USER_ID = GetCookieValue("UserID");
        BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        let url = "/C11_STOP/Button1_Click";

        fetch(url, {
            method: "POST",
            body: formUpload,
            headers: {
            }
        }).then((response) => {
            response.json().then((data) => {
                $("#BackGround").css("display", "none");
                C11_STOP_FormClosed();
            }).catch((err) => {
                $("#BackGround").css("display", "none");
            })
        });
    }
    else {
        C11_STOP_FormClosed();
    }
}