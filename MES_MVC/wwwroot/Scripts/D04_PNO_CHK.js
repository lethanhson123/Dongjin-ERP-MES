let IsTableSort = false;
let BaseResult;
let RowIndex;

$(window).focus(function () {
}).blur(function () {
    localStorage.setItem("D04_PNO_CHK_Close", 1);
    window.close();
});
$(document).ready(function () {
    localStorage.setItem("D04_PNO_CHK_Close", 0);    
    document.getElementById("CheckBox1").checked = true;
    $("#Label1").val("0");
    $("#Label2").val("-");
    $("#Label3").val("-");
    $("#Label5").val("-");
    $("#TextBoxA2").val("-");
    $("#TextBoxA2").focus();
    PO_CODE();
});
function PO_CODE() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/D04_PNO_CHK/PO_CODE";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            let BaseResultPO_CODE = data;               
            $("#ComboBox1").empty();
            for (let i = 0; i < BaseResultPO_CODE.ComboBox1.length; i++) {

                var option = document.createElement("option");
                option.text = BaseResultPO_CODE.ComboBox1[i].PO_CODE;
                option.value = BaseResultPO_CODE.ComboBox1[i].PO_CODE;

                var ComboBox1 = document.getElementById("ComboBox1");
                ComboBox1.add(option);
            }         
            ComboBox1_SelectedValueChanged();
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
$("#Buttonclose").click(function () {
    Buttonclose_Click();
});
function Buttonclose_Click() {    
    localStorage.setItem("D04_PNO_CHK_Close", 1);
    window.close();
}
$("#Buttonfind").click(function () {
    Buttonfind_Click();
});
function Buttonfind_Click() {
    PO_CODE();
}
$("#ComboBox1").change(function () {
    ComboBox1_SelectedValueChanged();
});
function ComboBox1_SelectedValueChanged() {
    $("#Label1").val("0");
    $("#Label2").val("-");
    $("#Label3").val("-");
    $("#Label5").val("-");
    $("#TextBoxA2").val("-");
    $("#TextBoxA2").focus();
}
$("#TextBoxA2").keydown(function () {
    if (e.keyCode == 13) {
        TextBoxA2_KeyDown();
    }
});
function TextBoxA2_KeyDown() {
    $("#Label2").val("-");
    let PART_IDX = $("#TextBoxA2").val();
    let ComboBox1 = $("#ComboBox1").val();
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        ListSearchString: [],
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    BaseParameter.CheckBox1 = document.getElementById("CheckBox1").checked;
    BaseParameter.ListSearchString.push(PART_IDX);
    BaseParameter.ListSearchString.push(ComboBox1);
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/D04_PNO_CHK/TextBoxA2_KeyDown";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            let BaseResultTextBoxA2_KeyDown = data;
            $("#Label1").val(BaseResultTextBoxA2_KeyDown.DGV_PNOCHK_01[0].PO_QTY);
            $("#Label3").val(BaseResultTextBoxA2_KeyDown.PART_IDX);
            $("#Label5").val(BaseResultTextBoxA2_KeyDown.DGV_PNOCHK_01[0].PART_GRP);
            $("#Label6").val(BaseResultTextBoxA2_KeyDown.DGV_PNOCHK_01[0].NT_QTY);
            $("#Label8").val(BaseResultTextBoxA2_KeyDown.DGV_PNOCHK_01[0].PACK_QTY);
            $("#Label10").val(BaseResultTextBoxA2_KeyDown.DGV_PNOCHK_01[0].STOCK);
            $("#Label13").val(BaseResultTextBoxA2_KeyDown.DGV_PNOCHK_01[0].STATUS);
            $("#Label14").val(BaseResultTextBoxA2_KeyDown.DGV_PNOCHK_01[0].Not_yet_packing);

            let Not_yet_packing = Number(BaseResultTextBoxA2_KeyDown.DGV_PNOCHK_01[0].Not_yet_packing);
            if (Not_yet_packing > 0) {
                $("#Label2").val("OK : Shipment Possible");
                document.getElementById("Label2").style.color = "green";
            }
            else {
                $("#Label2").val("OK : Shipment Complete");
                document.getElementById("Label2").style.color = "green";
            }
            var audio = new Audio("/Media/SD_OK.wav");
            audio.play();
            $("#TextBoxA2").val("");
            $("#TextBoxA2").focus();
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });

}

