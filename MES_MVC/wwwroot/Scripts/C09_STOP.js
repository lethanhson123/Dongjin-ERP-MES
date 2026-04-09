let IsTableSort = false;
let BaseResult;
let RowIndex;
let Timer1;
let EW_TIME_Timer1;
let Begin
let STOPW_ORING_IDX = 0;
$(window).focus(function () {
}).blur(function () {
    C09_STOP_FormClosed();
});
$(document).ready(function () {

    localStorage.setItem("C09_STOP_Close", 0);

    var now = new Date();
    var day = ("0" + now.getDate()).slice(-2);
    var month = ("0" + (now.getMonth() + 1)).slice(-2);
    var today = now.getFullYear() + "-" + (month) + "-" + (day);
    Begin = now;

    Timer1_Tick();
    PageLoad();
    SW_TIME();
    EW_TIME_Timer1_Tick();
    document.getElementById("Button2").disabled = true;
    let Label5 = $("#Label5").val();
    if (Label5 == "M") {
        document.getElementById("Button2").disabled = false;
    }
});
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
    let url = "/C09_STOP/PageLoad";

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
                    }
                }
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });

}
function EW_TIME_Timer1_Tick() {
    EW_TIME_Timer1 = setInterval(function () {
        EW_TIME();
    }, 60000);
}
function EW_TIME() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        SearchString: STOPW_ORING_IDX,
    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C09_STOP/EW_TIME";

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
function SW_TIME() {
    let USER_MC = localStorage.getItem("C09_MCbox");
    let USER_ORIDX = localStorage.getItem("C09_START_V2_Label8");
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
    let url = "/C09_STOP/SW_TIME";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            let BaseResultSW_TIME = data;
            if (BaseResultSW_TIME) {
                if (BaseResultSW_TIME.DGV_WT) {
                    if (BaseResult.DGV_WT.length > 0) {
                        STOPW_ORING_IDX = BaseResult.DGV_WT[0].TOWT_INDX;
                    }
                }
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
function C09_STOP_FormClosed() {
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
    let url = "/C09_STOP/C09_STOP_FormClosed";

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
    SettingsNON_OPER_CHK = false;
    clearInterval(Timer1);
    OPER_TIME();
    EW_TIME();
    C09_STOP_FormClosed_Tick();
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
    let url = "/C09_STOP/OPER_TIME";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            let BaseResultOPER_TIME = data;
            localStorage.setItem("C09_START_V2_BI_STOPTIME", BaseResultOPER_TIME.Error);
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
function C09_STOP_FormClosed_Tick() {
    $("#BackGround").css("display", "block");
    setInterval(function () {
        localStorage.setItem("C09_STOP_Close", 1);
        $("#BackGround").css("display", "none");
        window.close();
    }, 5000);
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
    let url = "/C09_STOP/Button2_Click";

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
        let url = "/C09_STOP/Button1_Click";

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
}