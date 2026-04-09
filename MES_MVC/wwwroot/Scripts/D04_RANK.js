let IsTableSort = false;
let BaseResult;
let RowIndex;
let CHK_SV = 9;
$(window).focus(function () {
}).blur(function () {    
    localStorage.setItem("D04_RANK_Close", 1);
    window.close();
});
$(document).ready(function () {    
    localStorage.setItem("D04_RANK_Close", 0);
    $("#Label1").val(localStorage.getItem("D04_RANK_Label1"));
    $("#Label2").val(localStorage.getItem("D04_RANK_Label2"));
    $("#Label3").val(localStorage.getItem("D04_RANK_Label3"));
    $("#Label4").val(localStorage.getItem("D04_RANK_Label4"));
    $("#Label5").val(localStorage.getItem("D04_RANK_Label5"));
});
$("#Button1").click(function () {
    Button1_Click();
});
function Button1_Click() {    
    CHK_SV = 1;
    SELECT_SAVE();   
}
$("#Button3").click(function () {
    Button3_Click();
});
function Button3_Click() {
    CHK_SV = 2;
    SELECT_SAVE();    
}
$("#Button4").click(function () {
    Button4_Click();
});
function Button4_Click() {
    CHK_SV = 9;
    SELECT_SAVE();  
}
function SELECT_SAVE() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        SearchString: $("#Label5").val(),
        CHK_SV: CHK_SV,
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");  
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/D04_RANK/SELECT_SAVE";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {        
            localStorage.setItem("D04_RANK_Close", 1);
            window.close();
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}



