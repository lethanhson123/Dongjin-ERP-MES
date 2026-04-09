let IsTableSort = false;
let BaseResult;
let RowIndex;

$(window).focus(function () {    
}).blur(function () {
    localStorage.setItem("D04_POADD_Close", 1);
    window.close();
});

$(document).ready(function () {
    localStorage.setItem("D04_POADD_Close", 0);    
    
    $("#TextBoxA1").val(localStorage.getItem("D04_POADD_TextBoxA1"));    
    $("#TextBoxA2").val(localStorage.getItem("D04_POADD_TextBoxA2"));
    $("#TextBoxA3").val(localStorage.getItem("D04_POADD_TextBoxA3"));
    $("#TextBox6").val(localStorage.getItem("D04_POADD_TextBox6"));
    $("#Label1").val(localStorage.getItem("D04_POADD_Label1"));    

    document.getElementById("CheckBox1").checked = true;   
    $("#TextBoxA2").focus();    
});
$("#Button3").click(function () {
    Button3_Click();
});
function Button3_Click() {
    let Label1 = $("#Label1").val();
    if (Label1 == "OK") {
        let PO_CODE = $("#TextBoxA1").val();
        let PO_QTY = $("#TextBoxA3").val();
        let PART_IDXK = $("#Label2").val();
        let PART_SNP = $("#Label3").val();
        let TextBoxA0 = $("#TextBoxA0").val();
        $("#BackGround").css("display", "block");
        let BaseParameter = new Object();
        BaseParameter = {
            ListSearchString: [],
        }
        BaseParameter.USER_ID = GetCookieValue("UserID");
        BaseParameter.USER_IDX = GetCookieValue("USER_IDX");        
        BaseParameter.ListSearchString.push(PO_CODE);
        BaseParameter.ListSearchString.push(PO_QTY);
        BaseParameter.ListSearchString.push(PART_IDXK);
        BaseParameter.ListSearchString.push(PART_SNP);
        BaseParameter.ListSearchString.push(TextBoxA0);
        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        let url = "/D04_POADD/Button3_Click";

        fetch(url, {
            method: "POST",
            body: formUpload,
            headers: {
            }
        }).then((response) => {
            response.json().then((data) => {
                alert(localStorage.getItem("SaveSuccess"));
                localStorage.setItem("D04_POADD_Close", 1);
                window.close();
                $("#BackGround").css("display", "none");
            }).catch((err) => {                
                $("#BackGround").css("display", "none");
            })
        });
    }
}
$("#TextBoxA2").keydown(function (e) {
    if (e.keyCode == 13) {
        TextBoxA2_KeyDown();
    }
});
function TextBoxA2_KeyDown() {    
    let PART_IDX = $("#TextBoxA2").val();
    let PO_CODE = $("#TextBoxA1").val();
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        ListSearchString: [],
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    BaseParameter.CheckBox1 = document.getElementById("CheckBox1").checked;
    BaseParameter.ListSearchString.push(PART_IDX);
    BaseParameter.ListSearchString.push(PO_CODE);
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/D04_POADD/TextBoxA2_KeyDown";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            let BaseResultTextBoxA2_KeyDown = data;
            let PART_IDXK = BaseResultTextBoxA2_KeyDown.Search[0].PART_IDX;
            let PART_SNP = BaseResultTextBoxA2_KeyDown.Search[0].PART_SNP;
            $("#Label2").val(PART_IDXK);
            $("#Label3").val(PART_SNP);
            if (BaseResultTextBoxA2_KeyDown.DGV_D04_PO01.length > 0) {
                $("#TextBoxA0").val(BaseResultTextBoxA2_KeyDown.DGV_D04_PO01[0].PDOTPL_IDX);
                $("#TextBoxA3").val(BaseResultTextBoxA2_KeyDown.DGV_D04_PO01[0].PO_QTY);
                $("#TextBox6").val(BaseResultTextBoxA2_KeyDown.DGV_D04_PO01[0].PACK_QTY);
            }
            else {
                $("#TextBoxA0").val("NEW");
                $("#TextBoxA3").val("0");
                $("#TextBox6").val("0");
            }
            $("#Label1").val("OK");
            document.getElementById("Label1").style.color = "green";
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });

}