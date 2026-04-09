let IsTableSort = false;
let BaseResult = new Object();
let TagIndex = 1;
let DateNow;
let DataGridView1RowIndex = 0;
let DataGridView2RowIndex = 0;
let DataGridView3RowIndex = 0;

$(document).ready(function () {
    var now = new Date();
    var day = ("0" + now.getDate()).slice(-2);
    var month = ("0" + (now.getMonth() + 1)).slice(-2);
    var today = now.getFullYear() + "-" + (month) + "-" + (day);
    DateNow = today;
    Lan_Change();
});

function Lan_Change() {
    Form_Label2();
}
function Form_Label2() {
    FIND_NO(0);
    document.getElementById("Button1").disabled = true;
}
function FIND_NO(Flag) {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/D11/FIND_NO";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            BaseResult = data;
            if (BaseResult) {
                if (BaseResult.DGV_D11_NO) {
                    if (BaseResult.DGV_D11_NO.length > 0) {
                        $("#LB01").val(BaseResult.DGV_D11_NO[0].LABEL_TXT);
                        $("#LB02").val("");
                        $("#LB03").val("");
                        $("#LBN01").val(BaseResult.DGV_D11_NO[0].LABEL_NO);
                        $("#LBN02").val("");
                        $("#LBN03").val("");
                    }
                }
            }            
            if (Flag == 1) {                    
                let LBN01 = Number($("#LBN01").val());                
                let NumericUpDown1 = Number($("#NumericUpDown1").val());
                LBN01 = LBN01 + 1;                
                let LB02 = "PALL" + LBN01.toString().padStart(8, '0');                
                $("#LB02").val(LB02);
                let LB03 = "PALL" + (LBN01 + NumericUpDown1).toString().padStart(8, '0');
                $("#LB03").val(LB03);
                let LBN02 = LBN01;
                $("#LBN02").val(LBN02);
                let LBN03 = LBN01 + NumericUpDown1;
                $("#LBN03").val(LBN03);
                document.getElementById("NumericUpDown1").disabled = true;
                document.getElementById("Button1").disabled = false;
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            //alert(localStorage.getItem("ERROR"));
            $("#BackGround").css("display", "none");
        })
    });
}
$("#NumericUpDown1").keydown(function (e) {
    if (e.keyCode == 13) {
        NumericUpDown1_KeyDown();
    }
});
function NumericUpDown1_KeyDown() {
    Buttonfind_Click();
}
$("#Button1").click(function () {
    Button1_Click();
});
function Button1_Click() {    
    let IsCheck = true;
    let ADD_NO = Number($("#NumericUpDown1").val());
    if (ADD_NO <= 0) {
        IsCheck = false;        
    }
    if (IsCheck == true) {
        let LBN03 = $("#LBN03").val();
        let LBN02 = $("#LBN02").val();
        $("#BackGround").css("display", "block");
        let BaseParameter = new Object();
        BaseParameter = {
            ListSearchString: [],
        }
        BaseParameter.USER_ID = GetCookieValue("UserID");
        BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
        BaseParameter.ListSearchString.push(ADD_NO);
        BaseParameter.ListSearchString.push(LBN03);
        BaseParameter.ListSearchString.push(LBN02);
        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        let url = "/D11/Button1_Click";

        fetch(url, {
            method: "POST",
            body: formUpload,
            headers: {
            }
        }).then((response) => {
            response.json().then((data) => {                
                let BaseResultButton1 = data;
                if (BaseResultButton1) {
                    if (BaseResultButton1.Code) {                        
                        let url = BaseResultButton1.Code;
                        OpenWindowByURL(url, 200, 200);
                    }
                }
                $("#BackGround").css("display", "none");
            }).catch((err) => {                
                $("#BackGround").css("display", "none");
            })
        });
    }
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
        let IsCheck = true;
        let ADD_NO = Number($("#NumericUpDown1").val());
        if (ADD_NO <= 0) {
            IsCheck = false;
        }
        if (IsCheck == true) {
            FIND_NO(1);
        }
    }
}
function Buttonadd_Click() {

}
function Buttonsave_Click() {

}
function Buttondelete_Click() {

}
function Buttoncancel_Click() {
    if (TagIndex == 1) {
        document.getElementById("NumericUpDown1").disabled = false;
        document.getElementById("Button1").disabled = true;
        $("#LB02").val("");
        $("#LB03").val("");
        $("#LBN02").val("");
        $("#LBN03").val("");
        FIND_NO(0);
    }
}
function Buttoninport_Click() {

}
function Buttonexport_Click() {

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


