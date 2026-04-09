let IsTableSort = false;
let BaseResult;

$(window).focus(function () {
}).blur(function () {
    Buttonclose_Click();
});

$(document).ready(function () {
    $("#Label1").val(localStorage.getItem("C04_1_Label1"));
    $("#Label2").val(localStorage.getItem("C04_1_Label2"));
    $("#Label5").val(localStorage.getItem("C04_1_Label5"));
    $("#Label6").val(localStorage.getItem("C04_1_Label6"));
    $("#Label7").val(localStorage.getItem("C04_1_Label7"));
    $("#Label8").val(localStorage.getItem("C04_1_Label8"));
});
$("#Buttonsave").click(function () {
    Buttonsave_Click();
});
function Buttonsave_Click() {
    let Label5 = $("#Label5").val();
    let Label6 = $("#Label6").val();
    let Label7 = $("#Label7").val();
    let Label8 = $("#Label8").val();
    let Label1 = $("#Label1").val();


    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {        
        ListSearchString: [],
    }
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.ListSearchString.push(Label5);
    BaseParameter.ListSearchString.push(Label6);
    BaseParameter.ListSearchString.push(Label7);
    BaseParameter.ListSearchString.push(Label8);
    BaseParameter.ListSearchString.push(Label1);
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C04_1/Buttonsave_Click";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            alert("정상처리 되었습니다. Đã được lưu.");
            Buttonclose_Click();
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            alert("오류가 발생 하였습니다. Một lỗi đã xảy ra." + err);
            $("#BackGround").css("display", "none");
        })
    });
}
$("#Button1").click(function () {
    Button1_Click();
});
function Button1_Click() {
    let IsCheck = true;
    let Label5 = $("#Label5").val();
    if (Label5 == "(899997)") {
        IsCheck = false;
    }
    if (Label5 == "(899998)") {
        IsCheck = false;
    }
    if (Label5 == "(899999)") {
        IsCheck = false;
    }
    if (IsCheck == true) {
        if (Label5.includes("(")) {
            Label5 = Label5.replace("(", "");
            Label5 = Label5.replace(")", ""); 
            $("#Label5").val(Label5);
            let Label6 = $("#Label6").val();
            if (Label6.length > 3) {
                Label6 = Label6.replace("(", "");
                Label6 = Label6.replace(")", ""); 
                $("#Label6").val(Label6);
            }
        }
        else {
            let Label5Sub = "(" + Label5 + ")";
            $("#Label5").val(Label5Sub);
            let Label6 = $("#Label6").val();
            if (Label6.length > 3) {
                let Label6Sub = "(" + Label6 + ")";
                $("#Label6").val(Label6Sub);
            }
        }
    }
}
$("#Button2").click(function () {
    Button2_Click();
});
function Button2_Click() {
    let IsCheck = true;
    let Label7 = $("#Label7").val();
    if (Label7 == "(899997)") {
        IsCheck = false;
    }
    if (Label7 == "(899998)") {
        IsCheck = false;
    }
    if (Label7 == "(899999)") {
        IsCheck = false;
    }
    if (IsCheck == true) {
        if (Label7.includes("(")) {            
            Label7 = Label7.replace("(", ""); 
            Label7 = Label7.replace(")", ""); 
            $("#Label7").val(Label7);
            let Label8 = $("#Label8").val();
            if (Label8.length > 3) {
                Label8 = Label8.replace("(", "");
                Label8 = Label8.replace(")", ""); 
                $("#Label8").val(Label8);
            }
        }
        else {
            Label7 = "(" + Label7 + ")";
            $("#Label7").val(Label7);
            let Label8 = $("#Label8").val();
            if (Label8.length > 3) {
                Label8 = "(" + Label8 + ")";
                $("#Label8").val(Label8);
            }
        }
    }
}
$("#Buttonclose").click(function () {
    Buttonclose_Click();
});
function Buttonclose_Click() {
    localStorage.setItem("C04_1_Close", 1);
    window.close();
}
