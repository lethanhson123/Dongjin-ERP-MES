let IsTableSort = false;
let BaseResult = new Object();
let ORDER_IDX;
$(window).focus(function () {
}).blur(function () {
    Buttonclose_Click();
});

$(document).ready(function () {
    localStorage.setItem("C11_APPLICATION_Close", 0);

    ORDER_IDX = localStorage.getItem("C11_APPLICATION_ORDER_IDX");
    $("#Label3").val(localStorage.getItem("C11_APPLICATION_Label3"));
    $("#Label1").val(localStorage.getItem("C11_APPLICATION_Label1"));
    $("#Label2").val(localStorage.getItem("C11_APPLICATION_Label2"));

    $("#Label5").val("---");
    $("#Label4").val("---");

    $("#TextBox1").val("");
    $("#TextBox1").focus();
});
$("#TextBox1").keydown(function (e) {
    if (e.keyCode == 13) {
        TextBox1_KeyDown();
    }
});
function TextBox1_KeyDown() {
    Button1_Click();
}
$("#Button1").click(function (e) {
    Button1_Click();
});
function Button1_Click() {
    let IsCheck = true;
    let APPT = $("#TextBox1").val();
    APPT = APPT.toUpperCase();
    let CO = APPT.length;
    let AAA = APPT.substr(0, CO - 1);    
    let BBB = APPT.substr(CO - 1, 1);   
    if ($("#Label3").val() == "APPLICATION #J") {
        let APP_12 = false;
        if (AAA == localStorage.getItem("C11_3_TL_NM")) {
            $("#Label1").val(localStorage.getItem("C11_3_TL_NM"));
        }
    }
    if ($("#Label3").val() == "APPLICATION #J2") {
        let APP_12 = false;
        if (AAA == localStorage.getItem("C11_4_TR_NM")) {
            $("#Label1").val(localStorage.getItem("C11_4_TR_NM"));
        }
    }
    let Label1 = $("#Label1").val();
    Label1 = Label1.toUpperCase();
    if (AAA == Label1) {
        $("#BackGround").css("display", "block");
        let BaseParameter = new Object();
        BaseParameter = {
            ListSearchString: [],
        }
        BaseParameter.USER_ID = GetCookieValue("UserID");
        BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
        BaseParameter.ListSearchString.push(AAA);
        BaseParameter.ListSearchString.push(BBB);
        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        let url = "/C11_APPLICATION/Button1_Click";

        fetch(url, {
            method: "POST",
            body: formUpload,
            headers: {
            }
        }).then((response) => {
            response.json().then((data) => {
                BaseResult.Search = data.Search;                
                if (BaseResult.Search.length <= 0) {
                    IsCheck = false
                    alert("No APPLICATOR Data in MES. Không có dữ liệu ỨNG DỤNG trong MES.");
                }
                if (IsCheck == true) {
                    $("#Label5").val(BaseResult.Search[0].APP_NAME);
                    $("#Label4").val(BaseResult.Search[0].SEQ);
                    $("#Label9").val(BaseResult.Search[0].WK_CNT);
                    $("#Label10").val(BaseResult.Search[0].TOOLMASTER_IDX);
                }
                $("#BackGround").css("display", "none");
            }).catch((err) => {
                $("#BackGround").css("display", "none");
            })
        });
    }
    else {
        IsCheck = false
        alert("APPLICATOR을 잘못 등록했습니다. Đăng ký sai của ỨNG DỤNG.");
    }
}
$("#Button2").click(function (e) {
    Button2_Click();
});
function Button2_Click() {
    let IsCheck = true;
    let Label5 = $("#Label5").val();
    if (Label5 == "---") {
        IsCheck = false;
    }
    let Label4 = $("#Label4").val();
    if (Label4 == "---") {
        IsCheck = false;
    }
    let Label3 = $("#Label3").val();
    let Label10 = $("#Label10").val();

    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        ListSearchString: [],
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    BaseParameter.ListSearchString.push(Label3);
    BaseParameter.ListSearchString.push(Label10);    
    BaseParameter.ListSearchString.push(ORDER_IDX);
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C11_APPLICATION/Button2_Click";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            $("#BackGround").css("display", "none");
            Buttonclose_Click();
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
$("#Buttonclose").click(function (e) {
    Buttonclose_Click();
});
function Buttonclose_Click() {
    localStorage.setItem("C11_APPLICATION_Close", 1);
    window.close();
}
