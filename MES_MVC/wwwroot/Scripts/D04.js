let IsTableSort = false;
let IsTableClick = false;
let BaseResult = new Object();
let TagIndex = 1;
let DateNow;
let DataGridView1RowIndex = 0;
let DataGridView2RowIndex = 0;
let DataGridView3RowIndex = 0;
let DataGridView4RowIndex = 0;
let DataGridView5RowIndex = 0;
let DataGridView6RowIndex = 0;
let DataGridView7RowIndex = 0;
let DataGridView8RowIndex = 0;
let DataGridView9RowIndex = 0;
let DataGridView10RowIndex = 0;
let DataGridView11RowIndex = 0;
let DGV_OUTRowIndex = 0;
let BOX_CONT = 0;
let PO_QTY_CHK = false;
let T1_CHK = false;
let BC_DSCN_YN = "Y";
let PO_LOC_CHK = false;
let D04_RANKTimer;
let D04_PNO_CHKTimer;
let D04_LOC_YNTimer;
let D04_QTY_YNTimer;
let D04_POADDTimer;
let D04_POLISTTimer;
let D04_PLT_PRNTTimer;
let PO_CD_CHK = "";

$(document).ready(function () {
    var now = new Date();
    DateNow = DateToString(now);

    $("#DateTimePicker1").val(DateNow);
    $("#DateTimePicker2").val(DateNow);
    $("#DTPsuck1").val(DateNow);
    now.setDate(now.getDate() + 1);
    $("#DTPsuck2").val(DateToString(now));

    document.getElementById("RBY").checked = true;
    document.getElementById("RadioButton5").checked = true;

    //document.getElementById("Label11").disabled = true;
    document.getElementById("TextBox4").disabled = true;
    //document.getElementById("Label12").disabled = true;
    document.getElementById("TextBox5").disabled = true;

    $("#Label22").val("N/A");
    BOX_CONT = 0

    document.getElementById("RadioButton9").checked = true;
    document.getElementById("CheckBox1").checked = false;

    $("#MaskedTextBox6").val("DJG- -");
    $("#MaskedTextBox1").focus();

    //$("#MaskedTextBox1").val("INC-P250606-01");
    //$("#MaskedTextBox6").val("DJG-25060601-300");
    //$("#MaskedTextBox5").val("DJGPALL147557");


    BaseResult.DataGridView1 = new Object();
    BaseResult.DataGridView1 = [];
    BaseResult.DataGridView2 = new Object();
    BaseResult.DataGridView2 = [];
    BaseResult.DataGridView3 = new Object();
    BaseResult.DataGridView3 = [];
    BaseResult.DataGridView6 = new Object();
    BaseResult.DataGridView6 = [];
    BaseResult.DataGridView7 = new Object();
    BaseResult.DataGridView7 = [];
    BaseResult.DataGridView8 = new Object();
    BaseResult.DataGridView8 = [];
    BaseResult.DataGridView9 = new Object();
    BaseResult.DataGridView9 = [];
    BaseResult.DataGridView10 = new Object();
    BaseResult.DataGridView10 = [];
    BaseResult.DataGridView11 = new Object();
    BaseResult.DataGridView11 = [];
    BaseResult.DGV_OUT = new Object();
    BaseResult.DGV_OUT = [];

});

$("#ATag001").click(function (e) {
    TagIndex = 1;
    $("#MaskedTextBox1").focus();
});
$("#ATag007").click(function (e) {
    TagIndex = 7;
    document.getElementById("LOC_TextBox1").readOnly = false;
    $("#LOC_TextBox1").focus();

    $("#LOC_Label11").val("-");
    $("#LOC_Label12").val("-");
    $("#LOC_Label13").val("-");
    $("#LOC_Label14").val("-");
    $("#LOC_Label15").val("-");

    $("#LOC_TextBox1").val("");
    $("#LOC_TextBox2").val("");

    document.getElementById("LOC_TextBox1").readOnly = false;
    $("#LOC_TextBox1").focus();
});
$("#ATag005").click(function (e) {
    TagIndex = 5;
    PO_LIST_CB();
});
$("#ATag006").click(function (e) {
    TagIndex = 6;
    PO_LIST_CB();
});
$("#ATag002").click(function (e) {
    TagIndex = 2;
});
$("#ATag004").click(function (e) {
    TagIndex = 4;
});
$("#ATag008").click(function (e) {
    TagIndex = 8;
    document.getElementById("RadioButton12").checked = true;
});
$("#ATag009").click(function (e) {
    TagIndex = 9;
    PO_LIST_CB();
});
$("#ComboBox2").change(function (e) {
    ComboBox2_SelectedIndexChanged();
});
function ComboBox2_SelectedIndexChanged() {
    $("#ComboBox20").val($("#ComboBox2").val());
}
$("#TextBox2").change(function (e) {
    TextBox2_SelectedIndexChanged();
});
function TextBox2_SelectedIndexChanged() {
    $("#TextBox211").val($("#TextBox2").val());
}
function PO_LIST_CB() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    BaseParameter.PageSize = 20;
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/D04/PO_LIST_CB";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            let BaseResultPO_LIST_CB = data;
            BaseResult.ComboBox2 = BaseResultPO_LIST_CB.ComboBox2;
            $("#TextBox9").empty();
            $("#TextBox2").empty();
            $("#ComboBox2").empty();
            for (let i = 0; i < BaseResult.ComboBox2.length; i++) {

                var option = document.createElement("option");
                option.text = BaseResult.ComboBox2[i].PO_CODE;
                option.value = BaseResult.ComboBox2[i].PO_CODE;

                var TextBox9 = document.getElementById("TextBox9");
                TextBox9.add(option);
            }

            for (let i = 0; i < BaseResult.ComboBox2.length; i++) {

                var option = document.createElement("option");
                option.text = BaseResult.ComboBox2[i].PO_CODE;
                option.value = BaseResult.ComboBox2[i].PO_CODE;

                var TextBox2 = document.getElementById("TextBox2");
                TextBox2.add(option);
            }
            TextBox2_SelectedIndexChanged();

            for (let i = 0; i < BaseResult.ComboBox2.length; i++) {

                var option = document.createElement("option");
                option.text = BaseResult.ComboBox2[i].PO_CODE;
                option.value = BaseResult.ComboBox2[i].PO_CODE;

                var ComboBox2 = document.getElementById("ComboBox2");
                ComboBox2.add(option);
            }
            ComboBox2_SelectedIndexChanged();
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
$("#Label15").click(function () {
    Label15_DoubleClick();
});
function Label15_DoubleClick() {
    PO_CD_CHK = "SUCH";
    localStorage.setItem("D04_PO_CD_CHK", PO_CD_CHK);
    let url = "/D04_POLIST";
    OpenWindowByURL(url, 800, 460);
    D04_POLISTTimerStartInterval();
}
$("#Label24").click(function () {
    Label24_DoubleClick();
});
function Label24_DoubleClick() {
    PO_CD_CHK = "ADD";
    localStorage.setItem("D04_PO_CD_CHK", PO_CD_CHK);
    let url = "/D04_POLIST";
    OpenWindowByURL(url, 800, 460);
    D04_POLISTTimerStartInterval();
}
function D04_POLISTTimerStartInterval() {
    D04_POLISTTimer = setInterval(function () {
        let D04_POLIST_Close = localStorage.getItem("D04_POLIST_Close");
        if (D04_POLIST_Close == "1") {
            clearInterval(D04_POLISTTimer);
            D04_POLIST_Close = 0;
            localStorage.setItem("D04_POLIST_Close", D04_POLIST_Close);
            Label15_DoubleClickSub();
            $("#MaskedTextBox6").val("");
            $("#MaskedTextBox6").focus();
        }
    }, 100);
}
function Label15_DoubleClickSub() {
    let D04_POLIST_PO_CD = localStorage.getItem("D04_POLIST_PO_CD");

    switch (PO_CD_CHK) {
        case "ADD":
            $("#TextBoxA1").val(D04_POLIST_PO_CD);
            break;
        case "SUCH":
            $("#MaskedTextBox1").val(D04_POLIST_PO_CD);
            break;
        case "PLT_LIST":
            $("#TextBox2").val(D04_POLIST_PO_CD);
            break;
        case "SHIP_LIST":
            $("#TextBox9").val(D04_POLIST_PO_CD);
            break;
    }
}
$("#Button4").click(function () {
    Button4_Click();
});
function Button4_Click() {
    let CheckBox2 = document.getElementById("RadioButton12").checked;
    if (CheckBox2 == false) {
        $("#TextBoxA1").val("");
    }
    $("#TextBoxA2").val("");
    $("#TextBox6").val("");
    $("#TextBoxA0").val("NEW");
    document.getElementById("TextBoxA1").readOnly = false;
    document.getElementById("TextBoxA2").readOnly = false;
}
$("#Button3").click(function () {
    Button3_Click();
});
function Button3_Click() {
    try {
        let IsCheck = true;
        let TextBoxA0 = $("#TextBoxA0").val();
        let TextBox6 = $("#TextBox6").val();
        let TextBoxA3 = $("#TextBoxA3").val();
        let TextBoxA2 = $("#TextBoxA2").val();
        let TextBoxA1 = $("#TextBoxA1").val();
        let TextBox22 = $("#TextBox22").val();
        let chk_text = TextBoxA0;
        let Dgv_count = Number(TextBox6);
        let TextBoxA3Number = Number(TextBoxA3);
        if (Dgv_count > TextBoxA3Number) {
            IsCheck = false;
            alert("Ues Pallet NO(PACK_QTY) & . Please Check Again.");
        }

        if (IsCheck == true) {
            $("#BackGround").css("display", "block");
            let BaseParameter = new Object();
            BaseParameter = {
                Action: TagIndex,
                ListSearchString: [],
            }
            BaseParameter.USER_ID = GetCookieValue("UserID");
            BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
            BaseParameter.ListSearchString.push(TextBoxA0);
            BaseParameter.ListSearchString.push(TextBox6);
            BaseParameter.ListSearchString.push(TextBoxA3);
            BaseParameter.ListSearchString.push(TextBoxA2);
            BaseParameter.ListSearchString.push(TextBoxA1);
            BaseParameter.ListSearchString.push(TextBox22);

            let formUpload = new FormData();
            formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
            let url = "/D04/Button3_Click";

            fetch(url, {
                method: "POST",
                body: formUpload,
                headers: {
                }
            }).then((response) => {
                response.json().then((data) => {

                    if (chk_text == "NEW") {
                        alert(localStorage.getItem("SaveSuccess"));
                        Buttoncancel_Click();
                    }
                    else {
                        alert("Edit Completed");
                        Buttoncancel_Click();
                    }
                    $("#BackGround").css("display", "none");
                }).catch((err) => {
                    alert(localStorage.getItem("ERROR"));
                    $("#BackGround").css("display", "none");
                })
            });
        }
    }
    catch (e) {
        alert("Please Check Again. " + e);
    }
}
$("#Label5").click(function () {
    Label5_DoubleClick();
});
function Label5_DoubleClick() {
    let url = "/D04_PNO_CHK";
    OpenWindowByURL(url, 800, 460);
}
$("#MaskedTextBox1").keydown(function (e) {
    if (e.keyCode == 13) {
        MaskedTextBox1_KeyDown();
    }
});
function MaskedTextBox1_KeyDown() {
    Buttonfind_Click();
    $("#MaskedTextBox6").val("");
    $("#MaskedTextBox6").focus();
}
$("#MaskedTextBox1").blur(function (e) {
    MaskedTextBox1_Leave();
});
function MaskedTextBox1_Leave() {
    let MaskedTextBox1 = $("#MaskedTextBox1").val();
    MaskedTextBox1 = MaskedTextBox1.toUpperCase();
    $("#MaskedTextBox1").val(MaskedTextBox1);
}
$("#MaskedTextBox6").keydown(function (e) {
    if (e.keyCode == 13) {
        MaskedTextBox6_KeyDown();
    }
});
function MaskedTextBox6_KeyDown() {
    $("#MaskedTextBox5").val("");
    $("#MaskedTextBox5").focus();
}
$("#TextBox10").keydown(function (e) {
    if (e.keyCode == 13) {
        TextBox10_KeyDown();
    }
});
function TextBox10_KeyDown() {
    Buttonfind_Click();
    $("#MaskedTextBox6").focus();
}
$("#MaskedTextBox5").keydown(function (e) {
    if (e.keyCode == 13) {
        MaskedTextBox5_KeyDown();
    }
});
function MaskedTextBox5_KeyDown() {
    $("#ITEMBCID").val("");
    $("#ITEMBCID").focus();
}
$("#MaskedTextBox5").blur(function (e) {
    MaskedTextBox5_Leave();
});
function MaskedTextBox5_Leave() {
    let MaskedTextBox5 = $("#MaskedTextBox5").val();
    MaskedTextBox5 = MaskedTextBox5.toUpperCase();
    $("#MaskedTextBox5").val(MaskedTextBox5);
    let AAA = MaskedTextBox5.length;
    let SearchWithinThis = MaskedTextBox5;
    let SearchForThis = "DJG";
    let FirstCharacter = SearchWithinThis.indexOf(SearchForThis);
    if (FirstCharacter < 0) {
        if (AAA == 0) {
            if (confirm("Confirm Delete?")) {
            }
        }
        else {
            if (8 >= AAA && AAA <= 10) {
                alert("DATA LOAD 오류가 발생 하였습니다. DATA LOAD Một lỗi đã xảy ra.");
                $("#MaskedTextBox5").val("");
            }
        }
    }
}
$("#CheckBox1").click(function () {
    CheckBox1_CheckedChanged();
});
function CheckBox1_CheckedChanged() {
    let CheckBox1 = document.getElementById("CheckBox1").checked;
    if (CheckBox1 == true) {
        document.getElementById("Label4").innerHTML = "Product BARCODE ID : ";
    }
    if (CheckBox1 == false) {
        document.getElementById("Label4").innerHTML = "Serial BARCODE ID : ";
    }
}
$("#ITEMBCID").keydown(function (e) {
    if (e.keyCode == 13) {
        ITEMBCID_KeyDown();
    }
});
function ITEMBCID_KeyDown() {
    Button2_Click();
}
$("#Button2").click(function () {
    Button2_Click();
});
let Button2IsCheck = true;
function Button2_Click() {
    document.getElementById("ITEMBCID").disabled = true;
    Button2IsCheck = true;

    let AAA = $("#ITEMBCID").val().trim();
    if (AAA == "") {
        Button2IsCheck = false;
        var audio = new Audio("/Media/Sash_brk.wav");
        audio.play();
        alert("erial BARCODE ID Vui lòng kiểm tra lại. Serial BARCODE ID 없음. Serial BARCODE ID Please Check Again.");
        $("#LBRESULT").val("NOK");
        document.getElementById("LBRESULT").style.color = "red";
        $("#ITEMBCID").val("");
        $("#ITEMBCID").focus("");
    }
    if (BaseResult.DGV_OUT.length <= 0) {
        Button2IsCheck = false;
        var audio = new Audio("/Media/Sash_brk.wav");
        audio.play();
        alert("Chọn MÃ PO. Vui lòng kiểm tra lại. Select PO CODE. Please Check Again.");
        $("#LBRESULT").val("NOK");
        document.getElementById("LBRESULT").style.color = "red";
        $("#ITEMBCID").val("");
        $("#ITEMBCID").focus("");
    }
    let MaskedTextBox6 = $("#MaskedTextBox6").val();
    if (MaskedTextBox6.length <= 13) {
        Button2IsCheck = false;
        var audio = new Audio("/Media/Sash_brk.wav");
        audio.play();
        alert("Chọn Pallet NO. Vui lòng kiểm tra lại. Select Pallet NO. Please Check Again.");
        $("#LBRESULT").val("NOK");
        document.getElementById("LBRESULT").style.color = "red";
        $("#ITEMBCID").val("");
        $("#ITEMBCID").focus("");
    }
    let listchk = false;

    for (let n = 0; n < BaseResult.DataGridView3.length; n++) {
        let ITEMBCID = $("#ITEMBCID").val();
        let Serial_Barcode = BaseResult.DataGridView3[n].Serial_Barcode;
        if (Serial_Barcode == ITEMBCID) {
            Button2IsCheck = false;
            var audio = new Audio("/Media/Sash_brk.wav");
            audio.play();
            alert("đăng ký trùng lặp 이미 등록된 바코드 The barcode is already registered. Please Check Again.");
            $("#LBRESULT").val("NOK");
            document.getElementById("LBRESULT").style.color = "red";
            $("#ITEMBCID").val("");
            $("#ITEMBCID").focus("");
        }
    }
    document.getElementById("MaskedTextBox1").readOnly = false;
    document.getElementById("TextBox10").readOnly = false

    if (Button2IsCheck == true) {
        let MaskedTextBox1 = $("#MaskedTextBox1").val();
        $("#BackGround").css("display", "block");
        let BaseParameter = new Object();
        BaseParameter = {
            Action: TagIndex,
            ListSearchString: [],
        }
        BaseParameter.USER_ID = GetCookieValue("UserID");
        BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
        BaseParameter.CheckBox1 = document.getElementById("CheckBox1").checked;
        BaseParameter.ListSearchString.push(AAA);
        BaseParameter.ListSearchString.push(MaskedTextBox1);

        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        let url = "/D04/Button2_Click";

        fetch(url, {
            method: "POST",
            body: formUpload,
            headers: {
            }
        }).then((response) => {
            response.json().then((data) => {
                let BaseResultSub01 = data;
                BaseResult.COUNT_DGV = BaseResultSub01.COUNT_DGV;
                BaseResult.SP_CHK = BaseResultSub01.SP_CHK;
                let PART_IDX_LOC = 0;
                try {
                    if (BaseResult.COUNT_DGV[0].VLID_PART_IDX == 0) {
                        BaseResult.COUNT_DGV_CHK = BaseResultSub01.COUNT_DGV_CHK;
                        if (BaseResult.COUNT_DGV_CHK[0].VLID_PART_IDX > 0) {
                            Button2IsCheck = false;
                            var audio = new Audio("/Media/ERROR_BIBI.wav");
                            audio.play();
                            alert("Mã vạch đã được sử dụng. 이미 사용된 바코드. Barcodes already used.");
                            $("#LBRESULT").val("NOK");
                            document.getElementById("LBRESULT").style.color = "red";
                        }
                        else {
                            Button2IsCheck = false;
                            var audio = new Audio("/Media/ERROR_BIBI.wav");
                            audio.play();
                            alert("mã vạch chưa đăng ký. 등록되지 않은 바코드. unregistered barcode.");
                            $("#LBRESULT").val("NOK");
                            document.getElementById("LBRESULT").style.color = "red";
                        }
                    }
                    PART_IDX_LOC = BaseResult.COUNT_DGV[0].VLID_PART_IDX;
                }
                catch (err) {
                    Button2IsCheck = false;
                    var audio = new Audio("/Media/Sash_brk.wav");
                    audio.play();
                    alert("erial BARCODE ID Vui lòng kiểm tra lại. Serial BARCODE ID 없음. Serial BARCODE ID Please Check Again.");
                    $("#LBRESULT").val("NOK");
                    document.getElementById("LBRESULT").style.color = "red";
                }
                if (Button2IsCheck == true) {
                    try {
                        BaseResult.DGV_D04_ALOC = BaseResultSub01.DGV_D04_ALOC;
                        let ALOC = "";
                        if (BaseResult.DGV_D04_ALOC.length <= 0) {
                            ALOC = "ALL";
                        }
                        else {
                            ALOC = BaseResult.DGV_D04_ALOC[0].D_ARRVL.substring(0, 2);
                        }
                        let PO_LOC = MaskedTextBox1.substring(0, 2);
                        let LOC_CHK = true;
                        switch (PO_LOC) {
                            case "BU":
                                localStorage.setItem("D04_LOC_YN_Label3", "BUSAN");
                                if (ALOC == PO_LOC) {
                                    LOC_CHK = true;
                                }
                                else {
                                    LOC_CHK = false;
                                }
                                break;
                            case "IN":
                                localStorage.setItem("D04_LOC_YN_Label3", "INCHEON");
                                if (ALOC == PO_LOC) {
                                    LOC_CHK = true;
                                }
                                else {
                                    LOC_CHK = false;
                                }
                                break;
                        }
                        if (LOC_CHK == false) {
                            //var audio = new Audio("/Media/Sash_brk.wav");
                            //audio.play();
                            let D04_LOC_YN_Label3 = "PO : " + localStorage.getItem("D04_LOC_YN_Label3") + "  /  " + "SCAN PART NO : " + BaseResult.DGV_D04_ALOC[0].D_ARRVL;
                            localStorage.setItem("D04_LOC_YN_Label3", D04_LOC_YN_Label3);
                            PO_LOC_CHK = false;
                            localStorage.setItem("D04_LOC_YN_Close", 0);
                            localStorage.setItem("D04_PO_LOC_CHK", false);
                            let url = "/D04_LOC_YN";
                            OpenWindowByURL(url, 800, 460);
                            D04_LOC_YNTimerStartInterval();
                        }
                        else {
                            localStorage.setItem("D04_LOC_YN_Close", 1);
                            localStorage.setItem("D04_PO_LOC_CHK", true);
                            D04_LOC_YNTimerStartInterval();
                        }
                    }
                    catch (err) {
                        localStorage.setItem("D04_LOC_YN_Close", 1);
                        localStorage.setItem("D04_PO_LOC_CHK", true);
                        D04_LOC_YNTimerStartInterval();
                    }
                }
                document.getElementById("ITEMBCID").disabled = false;
                $("#BackGround").css("display", "none");
            }).catch((err) => {
                alert(localStorage.getItem("ERROR"));
                $("#BackGround").css("display", "none");
            })
        });
    }
    else {
        document.getElementById("ITEMBCID").disabled = false;
    }
}
function D04_LOC_YNTimerStartInterval() {
    D04_LOC_YNTimer = setInterval(function () {
        let D04_LOC_YN_Close = localStorage.getItem("D04_LOC_YN_Close");
        if (D04_LOC_YN_Close == "1") {
            clearInterval(D04_LOC_YNTimer);
            D04_LOC_YN_Close = 0;
            localStorage.setItem("D04_LOC_YN_Close", D04_LOC_YN_Close);
            PO_QTY_CHK = Boolean(localStorage.getItem("D04_PO_LOC_CHK"));
            Button2_ClickSub();
        }
    }, 100);
}
let ck_PNNO;
let Part_no;
let ckCODE;
let ckPART;
let ckLINE;
let ckAA;
let qtyAA;
let qtyBB;
let ckBB;
let QTY;
let QTY_ch;
let TOT_QTY;
let DJG_PLT;
let CUT_PLT;

function Button2_ClickSub() {
    try {
        if (PO_QTY_CHK == true) {
            ck_PNNO = BaseResult.COUNT_DGV[0].VLID_PART_IDX;
            Part_no = false;
            let IsCheck = true;
            for (let ii = 0; ii < BaseResult.DGV_OUT.length; ii++) {
                DGV_OUTRowIndex = ii;
                ckCODE = BaseResult.DGV_OUT[ii].PDOTPL_IDX;
                ckPART = BaseResult.DGV_OUT[ii].PART_NO;
                ckLINE = BaseResult.DGV_OUT[ii].PART_GRP;
                ckAA = Number(BaseResult.DGV_OUT[ii].PART_IDX);
                qtyAA = Number(BaseResult.DGV_OUT[ii].PO_QTY) - Number(BaseResult.DGV_OUT[ii].QTY);
                qtyBB = Number(BaseResult.DGV_OUT[ii].QTY);
                ckBB = Number(BaseResult.COUNT_DGV[0].VLID_PART_IDX);
                QTY = Number(BaseResult.COUNT_DGV[0].CONT);
                QTY_ch = Number(BaseResult.DGV_OUT[ii].QTY1);
                TOT_QTY = QTY_ch + QTY;
                DJG_PLT = $("#MaskedTextBox6").val();
                CUT_PLT = $("#MaskedTextBox5").val();
                if (ckAA == ckBB) {
                    IsCheck = false;
                    PO_QTY_CHK = false;
                    if (TOT_QTY <= qtyAA) {
                        PO_QTY_CHK = true;
                        localStorage.setItem("D04_QTY_YN_Close", 1);
                        localStorage.setItem("D04_PO_LOC_CHK", PO_QTY_CHK);
                        D04_QTY_YNTimerStartInterval();
                        ii = BaseResult.DGV_OUT.length;
                    }
                    else {
                        PO_QTY_CHK = false;
                        var audio = new Audio("/Media/Sash_brk.wav");
                        audio.play();
                        localStorage.setItem("D04_QTY_YN_Close", 0);
                        localStorage.setItem("D04_PO_LOC_CHK", PO_QTY_CHK);
                        let url = "/D04_QTY_YN";
                        OpenWindowByURL(url, 800, 460);
                        D04_QTY_YNTimerStartInterval();
                        ii = BaseResult.DGV_OUT.length;
                    }
                }
            }
            if (IsCheck == true) {
                if (Button2IsCheck == true) {
                    if (Part_no == false) {
                        $("#BackGround").css("display", "block");
                        let BaseParameter = new Object();
                        BaseParameter = {
                            ListSearchString: [],
                        }
                        BaseParameter.USER_ID = GetCookieValue("UserID");
                        BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
                        BaseParameter.ListSearchString.push($("#MaskedTextBox1").val());
                        BaseParameter.ListSearchString.push(ck_PNNO);
                        let formUpload = new FormData();
                        formUpload.append("BaseParameter", JSON.stringify(BaseParameter));
                        let url = "/D04/Button2_Click01";

                        fetch(url, {
                            method: "POST",
                            body: formUpload,
                            headers: {
                            }
                        }).then((response) => {
                            response.json().then((data) => {
                                BaseResult.QTY_CHK = data.QTY_CHK;
                                if (BaseResult.QTY_CHK == true) {
                                    var audio = new Audio("/Media/ERROR_BIBI.wav");
                                    audio.play();
                                    alert("Đóng gói. 포장 완료.Packed.");
                                    $("#LBRESULT").val("NOK");
                                    document.getElementById("LBRESULT").style.color = "red";
                                }
                                else {
                                    var audio = new Audio("/Media/Sash_brk.wav");
                                    audio.play();
                                    alert("The Part number does Not match.  Please Check Again.");
                                    $("#LBRESULT").val("NOK");
                                    document.getElementById("LBRESULT").style.color = "red";
                                }
                                $("#BackGround").css("display", "none");
                            }).catch((err) => {
                                alert(localStorage.getItem("ERROR"));
                                $("#BackGround").css("display", "none");
                            })
                        });
                    }
                }
                $("#ITEMBCID").val("");
                $("#ITEMBCID").focus();
                document.getElementById("MaskedTextBox6").disabled = false;
                document.getElementById("MaskedTextBox5").disabled = false;
            }
            //if (PO_QTY_CHK == true) {
            //    if (Part_no == false) {
            //        let MaskedTextBox1 = $("#MaskedTextBox1").val();
            //        $("#BackGround").css("display", "block");
            //        let BaseParameter = new Object();
            //        BaseParameter = {
            //            ListSearchString: [],
            //        }
            //        BaseParameter.USER_ID = GetCookieValue("UserID");
            //        BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
            //        BaseParameter.ListSearchString.push(MaskedTextBox1);
            //        BaseParameter.ListSearchString.push(ck_PNNO);
            //        let formUpload = new FormData();
            //        formUpload.append("BaseParameter", JSON.stringify(BaseParameter));
            //        let url = "/D04/Button2_Click01";

            //        fetch(url, {
            //            method: "POST",
            //            body: formUpload,
            //            headers: {
            //            }
            //        }).then((response) => {
            //            response.json().then((data) => {
            //                BaseResult.QTY_CHK = data.QTY_CHK;
            //                if (BaseResult.QTY_CHK == true) {
            //                    var audio = new Audio("/Media/ERROR_BIBI.wav");
            //                    audio.play();
            //                    alert("Đóng gói. 포장 완료.Packed.");
            //                    $("#LBRESULT").val("NOK");
            //                    document.getElementById("LBRESULT").style.color = "red";
            //                }
            //                else {
            //                    var audio = new Audio("/Media/Sash_brk.wav");
            //                    audio.play();
            //                    alert("The Part number does Not match.  Please Check Again.");
            //                    $("#LBRESULT").val("NOK");
            //                    document.getElementById("LBRESULT").style.color = "red";
            //                }
            //                $("#BackGround").css("display", "none");
            //            }).catch((err) => {
            //                alert(localStorage.getItem("ERROR"));
            //                $("#BackGround").css("display", "none");
            //            })
            //        });
            //    }
            //}
            //$("#ITEMBCID").val("");
            //$("#ITEMBCID").focus();
            //document.getElementById("MaskedTextBox6").disabled = false;
            //document.getElementById("MaskedTextBox5").disabled = false;
        }
    }
    catch (err) {
        var audio = new Audio("/Media/Sash_brk.wav");
        audio.play();
        alert("Serial BARCODE ID & PARTNO & PALLET NO Please Check Again." + err);
        $("#LBRESULT").val("NOK");
        document.getElementById("LBRESULT").style.color = "red";
    }
}
function D04_LOC_YNTimerStartInterval01() {
    D04_LOC_YNTimer = setInterval(function () {
        let D04_LOC_YN_Close = localStorage.getItem("D04_LOC_YN_Close");
        if (D04_LOC_YN_Close == "1") {
            clearInterval(D04_LOC_YNTimer);
            D04_LOC_YN_Close = 0;
            localStorage.setItem("D04_LOC_YN_Close", D04_LOC_YN_Close);
            PO_QTY_CHK = Boolean(localStorage.getItem("D04_PO_LOC_CHK"));
            Button2_ClickSub01();
        }
    }, 100);
}
function Button2_ClickSub01() {
    if (PO_QTY_CHK == true) {
        BaseResult.DGV_OUT[DGV_OUTRowIndex].QTY1 = TOT_QTY;
        DGV_OUTRender();
        let DataGridView3Item = {
            CHK: true,
            Pallet_NO: DJG_PLT,
            Customer_Pallet: CUT_PLT,
            Serial_Barcode: $("#ITEMBCID").val(),
            QTY: QTY,
            PART_NO: ckPART,
            PO_CODE: $("#MaskedTextBox1").val(),
            PO_IDX: ckCODE,
            PART_CODE: ckAA,
            Split: BaseResult.SP_CHK,
        }
        if (BaseResult.DataGridView3.length == 0) {
            BaseResult.DataGridView3.push(DataGridView3Item);
        }
        else {
            let IsCheckPush = true;
            for (let i = 0; i < BaseResult.DataGridView3.length; i++) {
                if (BaseResult.DataGridView3[i].Serial_Barcode == $("#ITEMBCID").val()) {
                    IsCheckPush = false;
                    i = BaseResult.DataGridView3.length;
                }
            }
            if (IsCheckPush == true) {
                BaseResult.DataGridView3.push(DataGridView3Item);
            }
        }
        DataGridView3Render();
        let LBQTY = Number($("#LBQTY").val()) + QTY;
        $("#LBQTY").val(LBQTY);
        LBQTY_TextChanged();
        BOX_CONT = BOX_CONT + 1;
        $("#Label36").val(BOX_CONT);
        let S_CHKPART = false;
        let S_QTY = 1;
        for (let S_II = 0; S_II < BaseResult.DataGridView1.length; S_II++) {
            let S_AA = BaseResult.DataGridView1[S_II].QTY;
            let S_BB = BaseResult.DataGridView1[S_II].BOX_COUNT;
            if (ckPART == BaseResult.DataGridView1[S_II].PART_NO) {
                S_CHKPART = true;
                BaseResult.DataGridView1[S_II].QTY = S_AA + QTY;
                BaseResult.DataGridView1[S_II].BOX_COUNT = S_BB + 1;
                S_II = BaseResult.DataGridView1.length;
            }
        }
        if (S_CHKPART == false) {
            let DataGridView1Item = {
                PART_NO: ckPART,
                PART_LINE: ckLINE,
                QTY: QTY,
                BOX_COUNT: S_QTY,
            }
            BaseResult.DataGridView1.push(DataGridView1Item);
        }
        DataGridView1Render();
        Part_no = true;
        var audio = new Audio("/Media/HOOK_S.wav");
        audio.play();
    }
    else {
        Button2IsCheck = false;
        var audio = new Audio("/Media/Sash_brk.wav");
        audio.play();
        alert("Số lượng đặt hàng đã vượt quá. PO 수량이 초과되었습니다. More quantity than PO_QTY. Please Check Again.");
        $("#LBRESULT").val("NOK");
        document.getElementById("LBRESULT").style.color = "red";
        $("#ITEMBCID").val("");
        $("#ITEMBCID").focus();
    }
    if (Button2IsCheck == true) {
        if (Part_no == false) {
            $("#BackGround").css("display", "block");
            let BaseParameter = new Object();
            BaseParameter = {
                ListSearchString: [],
            }
            BaseParameter.USER_ID = GetCookieValue("UserID");
            BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
            BaseParameter.ListSearchString.push($("#MaskedTextBox1").val());
            BaseParameter.ListSearchString.push(ck_PNNO);
            let formUpload = new FormData();
            formUpload.append("BaseParameter", JSON.stringify(BaseParameter));
            let url = "/D04/Button2_Click01";

            fetch(url, {
                method: "POST",
                body: formUpload,
                headers: {
                }
            }).then((response) => {
                response.json().then((data) => {
                    BaseResult.QTY_CHK = data.QTY_CHK;
                    if (BaseResult.QTY_CHK == true) {
                        var audio = new Audio("/Media/ERROR_BIBI.wav");
                        audio.play();
                        alert("Đóng gói. 포장 완료.Packed.");
                        $("#LBRESULT").val("NOK");
                        document.getElementById("LBRESULT").style.color = "red";
                    }
                    else {
                        var audio = new Audio("/Media/Sash_brk.wav");
                        audio.play();
                        alert("The Part number does Not match.  Please Check Again.");
                        $("#LBRESULT").val("NOK");
                        document.getElementById("LBRESULT").style.color = "red";
                    }
                    $("#BackGround").css("display", "none");
                }).catch((err) => {
                    alert(localStorage.getItem("ERROR"));
                    $("#BackGround").css("display", "none");
                })
            });
        }
    }
    $("#ITEMBCID").val("");
    $("#ITEMBCID").focus();
    document.getElementById("MaskedTextBox6").disabled = false;
    document.getElementById("MaskedTextBox5").disabled = false;
}
function D04_QTY_YNTimerStartInterval() {
    D04_QTY_YNTimer = setInterval(function () {
        let D04_QTY_YN_Close = localStorage.getItem("D04_QTY_YN_Close");
        if (D04_QTY_YN_Close == "1") {
            clearInterval(D04_QTY_YNTimer);
            D04_QTY_YN_Close = 0;
            localStorage.setItem("D04_QTY_YN_Close", D04_QTY_YN_Close);
            PO_QTY_CHK = Boolean(localStorage.getItem("D04_PO_LOC_CHK"));
            Button2_ClickSub01();
        }
    }, 100);
}
function Button2_ClickSubD04_QTY_YN() {
    if (PO_QTY_CHK == true) {
        let DataGridView3Item = {
            CHK: true,
            Pallet_NO: DJG_PLT,
            Customer_Pallet: CUT_PLT,
            Serial_Barcode: $("#ITEMBCID").val(),
            QTY: QTY,
            PART_NO: ckPART,
            PO_CODE: $("#MaskedTextBox1").val(),
            PO_IDX: ckCODE,
            PART_CODE: ckAA,
            Split: BaseResultButton2.SP_CHK,
        };
        if (BaseResult.DataGridView3.length == 0) {
            BaseResult.DataGridView3.push(DataGridView3Item);
        }
        else {
            let IsCheckPush = true;
            for (let i = 0; i < BaseResult.DataGridView3.length; i++) {
                if (BaseResult.DataGridView3[i].Serial_Barcode == $("#ITEMBCID").val()) {
                    IsCheckPush = false;
                    i = BaseResult.DataGridView3.length;
                }
            }
            if (IsCheckPush == true) {
                BaseResult.DataGridView3.push(DataGridView3Item);
            }
        }

        let LBQTY = Number($("#LBQTY").val());
        LBQTY = LBQTY + QTY;
        $("#LBQTY").val(LBQTY);
        LBQTY_TextChanged();
        BOX_CONT = BOX_CONT + 1;
        $("#Label36").val(BOX_CONT);
        var audio = new Audio("/Media/HOOK_S.wav");
        audio.play();
        //ii = BaseResultButton2.DGV_OUT.length;
    }
    else {
        Button2IsCheck = false;
        ii = BaseResultButton2.DGV_OUT.length;
        var audio = new Audio("/Media/Sash_brk.wav");
        audio.play();
        alert("Số lượng đặt hàng đã vượt quá. PO 수량이 초과되었습니다. More quantity than PO_QTY. Please Check Again.");
        $("#LBRESULT").val("NOK");
        document.getElementById("LBRESULT").style.color = "red";
        $("#ITEMBCID").val("");
        $("#ITEMBCID").focus();
    }

}
$("#LBQTY").change(function () {
    LBQTY_TextChanged();
});
function LBQTY_TextChanged() {
    let IsCheck = true;
    let Label29 = Number($("#Label29").val());
    if (Label29 == 0) {
        IsCheck = false;
    }
    if (IsCheck == true) {
        let LBQTY = Number($("#LBQTY").val());
        if (Label29 == LBQTY) {
            $("#LBRESULT").val("Complete");
            document.getElementById("LBRESULT").style.color = "yellow";
        }
        else {
            $("#LBRESULT").val("Shipping");
            document.getElementById("LBRESULT").style.color = "green";
        }
    }
}
$("#Button5").click(function () {
    Button5_Click();
});
function Button5_Click() {
    let IsCheck = true;
    let MaskedTextBox1 = Number($("#MaskedTextBox1").val());
    if (MaskedTextBox1 <= 0) {
        IsCheck = false;
    }
    if (IsCheck == true) {
        localStorage.setItem("D04_POADD_TextBoxA1", $("#MaskedTextBox1").val());
        localStorage.setItem("D04_POADD_TextBoxA2", "");
        localStorage.setItem("D04_POADD_TextBoxA3", 0);
        localStorage.setItem("D04_POADD_TextBox6", 0);
        localStorage.setItem("D04_POADD_Label1", "NG");

        localStorage.setItem("D04_POADD_Close", 0);
        let url = "/D04_POADD";
        OpenWindowByURL(url, 800, 460);
        D04_POADDTimerStartInterval();
    }
}
function D04_POADDTimerStartInterval() {
    D04_POADDTimer = setInterval(function () {
        let D04_POADD_Close = localStorage.getItem("D04_POADD_Close");
        if (D04_POADD_Close == "1") {
            clearInterval(D04_POADDTimer);
            D04_POADD_Close = 0;
            localStorage.setItem("D04_POADD_Close", D04_POADD_Close);
            DGV_OUT_LOAD(0);
            $("#MaskedTextBox6").focus();
        }
    }, 100);
}
function DGV_OUT_LOAD(Flag) {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        Action: TagIndex,
        ListSearchString: [],
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    BaseParameter.CheckBox3 = document.getElementById("CheckBox3").checked;
    BaseParameter.ListSearchString.push($("#MaskedTextBox1").val());
    BaseParameter.ListSearchString.push($("#TextBox10").val());
    let formUpload = new FormData();
    formUpload.append("BaseParameter", JSON.stringify(BaseParameter));
    let url = "/D04/DGV_OUT_LOAD";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            let BaseResultDGV_OUT_LOAD = data;
            BaseResult.DGV_OUT = BaseResultDGV_OUT_LOAD.DGV_OUT;
            DGV_OUTRender();
            if (Flag == 1) {
                let IsCheck = true;
                if (BaseResult.DGV_OUT.length == 0) {
                    IsCheck = false;
                    var audio = new Audio("/Media/Sash_brk.wav");
                    audio.play();
                    alert("Wrong PO CODE. Please Check Again.");
                }
                if (IsCheck == true) {
                    let Dgv_outcount = 0;
                    for (let i = 0; i < BaseResult.DGV_OUT.length; i++) {
                        Dgv_outcount = Dgv_outcount + BaseResult.DGV_OUT[i].Not_yet_packing;
                    }
                    $("#Label29").val(Dgv_outcount);
                    $("#LBQTY").val(0);
                    LBQTY_TextChanged();
                    $("#LBRESULT").val("--");
                    document.getElementById("LBRESULT").style.color = "black";
                    document.getElementById("MaskedTextBox6").readOnly = false;
                    document.getElementById("MaskedTextBox5").readOnly = false;
                    $("#MaskedTextBox6").focus();
                }
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            alert(localStorage.getItem("ERROR"));
            $("#BackGround").css("display", "none");
        })
    });
}
$("#LOC_TextBox1").keydown(function (e) {
    if (e.keyCode == 13) {
        LOC_TextBox1_KeyDown();
    }
});
function LOC_TextBox1_KeyDown() {
    Buttonfind_Click();
}
$("#LOC_TextBox2").keydown(function (e) {
    if (e.keyCode == 13) {
        LOC_TextBox2_KeyDown();
    }
});
function LOC_TextBox2_KeyDown() {
    Buttonsave_Click();
    $("#LOC_TextBox1").focus();
}
$("#TextBox14").change(function () {
    ComboBox1_SelectedIndexChanged();
});
function ComboBox1_SelectedIndexChanged() {
    DGV7_LOAD(0);
}
function DGV7_LOAD(Flag) {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        Action: TagIndex,
        ListSearchString: [],
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    BaseParameter.ListSearchString.push($("#TextBox14").val());
    BaseParameter.ListSearchString.push($("#TextBox211").val());
    BaseParameter.ListSearchString.push($("#TextBox3").val());
    let formUpload = new FormData();
    formUpload.append("BaseParameter", JSON.stringify(BaseParameter));
    let url = "/D04/DGV7_LOAD";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            let BaseResultDGV_OUT_LOAD = data;
            BaseResult.DataGridView7 = BaseResultDGV_OUT_LOAD.DataGridView7;
            DataGridView7Render();
            $("#Label22").val("N/A");

            if (Flag == 0) {
                let BOX_SQ = 0;
                $("#Label54").val(BOX_SQ);
                for (let i = 0; i < BaseResult.DataGridView7.length; i++) {
                    BOX_SQ = BOX_SQ + BaseResult.DataGridView7[i].BOX_QTY;
                }
                $("#Label54").val(BOX_SQ);
                $("#BackGround").css("display", "none");
            }
            if (Flag == 1) {
                Buttonprint_ClickSub();
            }
        }).catch((err) => {
            alert(localStorage.getItem("ERROR"));
            $("#BackGround").css("display", "none");
        })
    });
}
$("#RadioButton5").click(function () {
    RadioButton5_Click();
});
$("#RadioButton6").click(function () {
    RadioButton5_Click();
});
function RadioButton5_Click() {
    let RadioButton5 = document.getElementById("RadioButton5").checked;
    let RadioButton6 = document.getElementById("RadioButton6").checked;
    if (RadioButton5 == true) {
        document.getElementById("TextBox4").readOnly = true;
        document.getElementById("TextBox5").readOnly = true;
    }
    if (RadioButton6 == true) {
        document.getElementById("TextBox4").readOnly = false;
        document.getElementById("TextBox5").readOnly = false;
    }
}
$("#TextBox7").keydown(function (e) {
    if (e.keyCode == 13) {
        TextBox7_KeyDown();
    }
});
function TextBox7_KeyDown() {
    Buttonfind_Click();
}
$("#TextBox8").keydown(function (e) {
    if (e.keyCode == 13) {
        TextBox8_KeyDown();
    }
});
function TextBox8_KeyDown() {
    Buttonfind_Click();
}
$("#RadioButton7").click(function () {
    RadioButton8_Click();
});
$("#RadioButton8").click(function () {
    RadioButton8_Click();
});
function RadioButton8_Click() {
    RBchk_list6();
}
function RBchk_list6() {
    let RadioButton7 = document.getElementById("RadioButton7").checked;
    let RadioButton8 = document.getElementById("RadioButton8").checked;
    if (RadioButton8 == true) {
        for (let i = 0; i < BaseResult.DataGridView8.length; i++) {
            BaseResult.DataGridView8[i].CHK = true;
        }
        DataGridView8Render();
    }
    if (RadioButton7 == true) {
        for (let i = 0; i < BaseResult.DataGridView8.length; i++) {
            BaseResult.DataGridView8[i].CHK = !BaseResult.DataGridView8[i].CHK;
        }
        DataGridView8Render();
    }
}
$("#DTPsuck1").change(function (e) {
    DTPsuck1_ValueChanged();
});
function DTPsuck1_ValueChanged() {
    let DTPsuck1 = $("#DTPsuck1").val();
    let DTPsuck1Date = new Date();
    DTPsuck1Date.setDate(DTPsuck1Date.getDate() + 1);
    $("#DTPsuck2").val(DateToString(DTPsuck1Date));
}
$("#TB_LIST_PARTNO").keydown(function (e) {
    if (e.keyCode == 13) {
        TB_LIST_PARTNO_KeyDown();
    }
});
function TB_LIST_PARTNO_KeyDown() {
    Buttonfind_Click();
}
$("#RBN").click(function () {
    RBN_Click();
});
function RBN_Click() {
    CHK_SQL();
}
$("#RBY").click(function () {
    RBY_Click();
});
function RBY_CheckedChanged() {
    CHK_SQL();
}
function RBY_Click() {
    CHK_SQL();
}
function CHK_SQL() {
    let RBY = document.getElementById("RBY").checked;
    let RBN = document.getElementById("RBN").checked;
    if (RBY == true) {
        BC_DSCN_YN = "Y";
        $("#Label6").val("SHIPPING DATE :  ");
        document.getElementById("TextBox11").readOnly = false;
        document.getElementById("TextBox12").readOnly = false;
    }
    if (RBN == true) {
        BC_DSCN_YN = "N";
        $("#Label6").val("INPUT DATE : ");
        document.getElementById("TextBox11").readOnly = true;
        document.getElementById("TextBox12").readOnly = true;
    }
}
$("#RadioButton3").click(function () {
    RadioButton4_Click();
});
$("#RadioButton4").click(function () {
    RadioButton4_Click();
});
function RadioButton4_Click() {
    let RadioButton3 = document.getElementById("RadioButton3").checked;
    let RadioButton4 = document.getElementById("RadioButton4").checked;
    if (RadioButton4 == true) {
        for (let i = 0; i < BaseResult.DataGridView2.length; i++) {
            BaseResult.DataGridView2[i].CHK = true;
        }
        DataGridView2Render();
    }
    if (RadioButton3 == true) {
        for (let i = 0; i < BaseResult.DataGridView2.length; i++) {
            BaseResult.DataGridView2[i].CHK = !BaseResult.DataGridView2[i].CHK;
        }
        DataGridView2Render();
    }
}
$("#DateTimePicker1").change(function (e) {
    DateTimePicker1_ValueChanged();
});
function DateTimePicker1_ValueChanged() {

}
$("#DateTimePicker2").change(function (e) {
    DateTimePicker2_ValueChanged();
});
function DateTimePicker2_ValueChanged() {

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
        BOX_CONT = 0;
        $("#Label36").val(BOX_CONT);
        document.getElementById("MaskedTextBox1").readOnly = false;
        document.getElementById("TextBox10").readOnly = false;
        let MaskedTextBox1 = $("#MaskedTextBox1").val();
        let lencount = MaskedTextBox1.length;
        if (lencount < 5) {
            IsCheck = false;
            alert("Not PO CODE. Please Check Again.");
            var audio = new Audio("/Media/Sash_brk.wav");
            audio.play();
        }
        if (IsCheck == true) {
            BaseResult.DataGridView1 = [];
            DataGridView1Render();
            BaseResult.DataGridView3 = [];
            DataGridView3Render();
            DGV_OUT_LOAD(1);
        }
    }
    if (TagIndex == 2) {
        let ADATE = $("#DTPsuck1").val();
        let BDATE = $("#DTPsuck2").val();
        let LPARKNO = $("#TB_LIST_PARTNO").val();
        let SERIERID = $("#TextBox1").val();
        let LPOCODE = $("#TextBox11").val();
        let LPNTNO = $("#TextBox12").val();
        let PART_NM = $("#TextBox15").val();
        let RO_BC = $("#TextBox21").val();
        BaseResult.DataGridView2 = [];
        DataGridView2Render();
        $("#BackGround").css("display", "block");
        let BaseParameter = new Object();
        BaseParameter = {
            Action: TagIndex,
            ListSearchString: [],
        }
        BaseParameter.USER_ID = GetCookieValue("UserID");
        BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
        BaseParameter.ListSearchString.push(ADATE);
        BaseParameter.ListSearchString.push(BDATE);
        BaseParameter.ListSearchString.push(LPARKNO);
        BaseParameter.ListSearchString.push(SERIERID);
        BaseParameter.ListSearchString.push(LPOCODE);
        BaseParameter.ListSearchString.push(LPNTNO);
        BaseParameter.ListSearchString.push(PART_NM);
        BaseParameter.ListSearchString.push(RO_BC);
        BaseParameter.ListSearchString.push(BC_DSCN_YN);
        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        let url = "/D04/Buttonfind_Click";

        fetch(url, {
            method: "POST",
            body: formUpload,
            headers: {
            }
        }).then((response) => {
            response.json().then((data) => {
                let BaseResultButtonfind = data;
                BaseResult.DataGridView2 = BaseResultButtonfind.DataGridView2;
                BaseResult.ErrorNumber = BaseResultButtonfind.ErrorNumber;
                DataGridView2Render();
                $("#Label67").val(BaseResult.ErrorNumber);
                $("#BackGround").css("display", "none");
            }).catch((err) => {
                $("#BackGround").css("display", "none");
            })
        });
    }
    if (TagIndex == 5) {
        $("#BackGround").css("display", "block");
        let BaseParameter = new Object();
        BaseParameter = {
            Action: TagIndex,
            SearchString: $("#TextBox211").val(),
        }
        BaseParameter.USER_ID = GetCookieValue("UserID");
        BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        let url = "/D04/Buttonfind_Click";

        fetch(url, {
            method: "POST",
            body: formUpload,
            headers: {
            }
        }).then((response) => {
            response.json().then((data) => {
                let BaseResultButtonfind = data;
                BaseResult.DGV_D04_CB1 = BaseResultButtonfind.DGV_D04_CB1;
                $("#TextBox14").empty();
                for (let i = 0; i < BaseResult.DGV_D04_CB1.length; i++) {

                    var option = document.createElement("option");
                    option.text = BaseResult.DGV_D04_CB1[i].PLET_NO;
                    option.value = BaseResult.DGV_D04_CB1[i].PLET_NO;

                    var TextBox14 = document.getElementById("TextBox14");
                    TextBox14.add(option);
                }
                ComboBox1_SelectedIndexChanged();

                BaseResult.T_DGV_02 = BaseResultButtonfind.DGV_D04_CB1;
                PalletNODataGridViewRender(0);
                //$("#BackGround").css("display", "none");
            }).catch((err) => {
                $("#BackGround").css("display", "none");
            })
        });
    }
    if (TagIndex == 6) {
        let AAA = $("#TextBox9").val();
        let BBB = $("#TextBox8").val();
        let CCC = $("#TextBox7").val();
        let DDD = $("#TextBox13").val();

        $("#BackGround").css("display", "block");
        let BaseParameter = new Object();
        BaseParameter = {
            Action: TagIndex,
            ListSearchString: [],
        }
        BaseParameter.USER_ID = GetCookieValue("UserID");
        BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
        BaseParameter.ListSearchString.push(AAA);
        BaseParameter.ListSearchString.push(BBB);
        BaseParameter.ListSearchString.push(CCC);
        BaseParameter.ListSearchString.push(DDD);
        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        let url = "/D04/Buttonfind_Click";

        fetch(url, {
            method: "POST",
            body: formUpload,
            headers: {
            }
        }).then((response) => {
            response.json().then((data) => {
                let BaseResultButtonfind = data;
                BaseResult.DataGridView8 = BaseResultButtonfind.DataGridView8;
                DataGridView8Render();
                BaseResult.DataGridView10 = BaseResultButtonfind.DataGridView10;
                DataGridView10Render();
                BaseResult.Search = BaseResultButtonfind.Search;
                $("#Label57").val(BaseResult.Search[0].PLT);
                $("#Label55").val(BaseResult.Search[0].BOX);
                $("#Label61").val(BaseResult.Search[0].QTY);
                $("#BackGround").css("display", "none");
            }).catch((err) => {
                $("#BackGround").css("display", "none");
            })
        });
    }
    if (TagIndex == 4) {
        let DATE_A = $("#DateTimePicker2").val();
        let DATE_B = $("#DateTimePicker1").val();
        let AAA = $("#TextBox20").val();
        let BBB = $("#TextBox16").val();
        let CCC = $("#TextBox19").val();
        let DDD = $("#TextBox18").val();

        $("#BackGround").css("display", "block");
        let BaseParameter = new Object();
        BaseParameter = {
            Action: TagIndex,
            ListSearchString: [],
        }
        BaseParameter.USER_ID = GetCookieValue("UserID");
        BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
        BaseParameter.ListSearchString.push(DATE_A);
        BaseParameter.ListSearchString.push(DATE_B);
        BaseParameter.ListSearchString.push(AAA);
        BaseParameter.ListSearchString.push(BBB);
        BaseParameter.ListSearchString.push(CCC);
        BaseParameter.ListSearchString.push(DDD);
        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        let url = "/D04/Buttonfind_Click";

        fetch(url, {
            method: "POST",
            body: formUpload,
            headers: {
            }
        }).then((response) => {
            response.json().then((data) => {
                let BaseResultButtonfind = data;
                BaseResult.DataGridView6 = BaseResultButtonfind.DataGridView6;
                DataGridView6Render();
                $("#BackGround").css("display", "none");
            }).catch((err) => {
                $("#BackGround").css("display", "none");
            })
        });
    }
    if (TagIndex == 7) {
        let IsCheck = true;
        $("#LOC_Label11").val("-");
        $("#LOC_Label12").val("-");
        $("#LOC_Label13").val("-");
        $("#LOC_Label14").val("-");
        $("#LOC_Label15").val("-");
        let LOC_TextBox1 = $("#LOC_TextBox1").val();
        if (LOC_TextBox1 == "") {
            IsCheck = false;
        }
        if (IsCheck == true) {
            T1_CHK = false;

            let BC_TEXT = $("#LOC_TextBox1").val();

            $("#BackGround").css("display", "block");
            let BaseParameter = new Object();
            BaseParameter = {
                Action: TagIndex,
                SearchString: BC_TEXT,
            }
            BaseParameter.USER_ID = GetCookieValue("UserID");
            BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
            let formUpload = new FormData();
            formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
            let url = "/D04/Buttonfind_Click";

            fetch(url, {
                method: "POST",
                body: formUpload,
                headers: {
                }
            }).then((response) => {
                response.json().then((data) => {
                    let BaseResultButtonfind = data;
                    BaseResult.DGV_D04_07 = BaseResultButtonfind.DGV_D04_07;
                    if (BaseResult.DGV_D04_07.length > 0) {
                        $("#LOC_Label11").val(BaseResult.DGV_D04_07[0].PART_NO);
                        $("#LOC_Label12").val(BaseResult.DGV_D04_07[0].Name);
                        $("#LOC_Label13").val(BaseResult.DGV_D04_07[0].QTY);
                        $("#LOC_Label14").val(BaseResult.DGV_D04_07[0].VLID_GRP);
                        $("#LOC_Label15").val(BaseResult.DGV_D04_07[0].LOC);
                        T1_CHK = true;
                    }
                    else {
                        var audio = new Audio("/Media/Sash_brk.wav");
                        audio.play();
                        M.toast({ html: "바코드 정보 없음. No barcode information. Một lỗi đã xảy ra.", classes: 'red', displayLength: 6000 });
                        //alert("바코드 정보 없음. No barcode information. Một lỗi đã xảy ra.");
                        $("#LOC_TextBox1").val("");
                    }
                    if (T1_CHK == true) {
                        document.getElementById("LOC_TextBox1").readOnly = false;
                        $("#LOC_TextBox2").focus();
                    }
                    else {
                        $("#LOC_TextBox1").val("");
                        $("#LOC_TextBox1").focus();
                    }
                    $("#BackGround").css("display", "none");
                }).catch((err) => {
                    $("#BackGround").css("display", "none");
                })
            });
        }

    }
    if (TagIndex == 8) {
        let AAA = $("#TextBox17").val();

        $("#BackGround").css("display", "block");
        let BaseParameter = new Object();
        BaseParameter = {
            Action: TagIndex,
            SearchString: AAA,
        }
        BaseParameter.USER_ID = GetCookieValue("UserID");
        BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
        BaseParameter.RadioButton11 = document.getElementById("RadioButton11").checked;
        BaseParameter.RadioButton12 = document.getElementById("RadioButton12").checked;
        BaseParameter.RadioButton13 = document.getElementById("RadioButton13").checked;
        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        let url = "/D04/Buttonfind_Click";

        fetch(url, {
            method: "POST",
            body: formUpload,
            headers: {
            }
        }).then((response) => {
            response.json().then((data) => {
                let BaseResultButtonfind = data;
                BaseResult.DataGridView9 = BaseResultButtonfind.DataGridView9;
                DataGridView9Render();
                $("#BackGround").css("display", "none");
            }).catch((err) => {
                $("#BackGround").css("display", "none");
            })
        });
    }
    if (TagIndex == 9) {
        let AAA = $("#ComboBox2").val();
        let AAA1 = $("#ComboBox20").val();

        $("#BackGround").css("display", "block");
        let BaseParameter = new Object();
        BaseParameter = {
            Action: TagIndex,
            ListSearchString: [],
        }
        BaseParameter.USER_ID = GetCookieValue("UserID");
        BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
        BaseParameter.ListSearchString.push(AAA);
        BaseParameter.ListSearchString.push(AAA1);

        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        let url = "/D04/Buttonfind_Click";

        fetch(url, {
            method: "POST",
            body: formUpload,
            headers: {
            }
        }).then((response) => {
            response.json().then((data) => {
                let BaseResultButtonfind = data;
                BaseResult.DataGridView11 = BaseResultButtonfind.DataGridView11;
                DataGridView11Render();
                $("#BackGround").css("display", "none");
            }).catch((err) => {
                $("#BackGround").css("display", "none");
            })
        });
    }

}



function Buttonadd_Click() {

}
function Buttonsave_Click() {
    if (TagIndex == 1) {
        let IsSave = true;
        if (BaseResult.DGV_OUT.length <= -1) {
            IsSave = false;
        }
        if (BaseResult.DataGridView3.length <= -1) {
            IsSave = false;
        }
        let MaskedTextBox5 = $("#MaskedTextBox5").val();
        let MaskedTextBox6 = $("#MaskedTextBox6").val();
        //if (MaskedTextBox6 == "") {
        //    IsSave = false;
        //    alert("Please input Pallet NO. Vui lòng nhập Pallet NO.");
        //}
        if (IsSave == true) {
            $("#BackGround").css("display", "block");
            let BaseParameter = new Object();
            BaseParameter = {
                Action: TagIndex,
                ListSearchString: [],
            }
            BaseParameter.USER_ID = GetCookieValue("UserID");
            BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
            BaseParameter.DataGridView3 = BaseResult.DataGridView3;
            BaseParameter.DGV_OUT = BaseResult.DGV_OUT;
            BaseParameter.ListSearchString.push(MaskedTextBox5);
            BaseParameter.ListSearchString.push(MaskedTextBox6);
            let formUpload = new FormData();
            formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
            let url = "/D04/Buttonsave_Click";

            fetch(url, {
                method: "POST",
                body: formUpload,
                headers: {
                }
            }).then((response) => {
                response.json().then((data) => {
                    Buttonfind_Click();
                    M.toast({ html: "Save Completed.", classes: 'green', displayLength: 6000 });
                    BOX_CONT = 0;
                    $("#Label36").val(BOX_CONT);
                    $("#ITEMBCID").val("");
                    $("#TextBox10").val("");
                    $("#MaskedTextBox6").val("");
                    $("#MaskedTextBox5").val("");
                    $("#MaskedTextBox6").focus();
                    $("#BackGround").css("display", "none");
                }).catch((err) => {
                    $("#BackGround").css("display", "none");
                })
            });
        }
    }
    if (TagIndex == 5) {
        let IsSave = true;
        if (BaseResult.DataGridView7.length <= 0) {
            IsSave = false;
            alert("Please select data. Please Check Again.");
        }
        let Label22 = $("#Label22").val();
        if (Label22 == "N/A") {
            IsSave = false;
            alert("Please select data. Please Check Again.");
        }
        let MaskedTextBox4 = $("#MaskedTextBox4").val();
        if (Label22.length != 14) {
            IsSave = false;
            alert("파렛트 번호가 잘못되었습니다. Vui lòng nhập số pallet");
        }
        if (IsSave == true) {
            let AAA = $("#MaskedTextBox4").val();
            let BBB = $("#TextBox4").val();
            let CCC = $("#Label22").val();
            let PO_CODE = $("#Label34").val();
            let MaskedTextBox4 = $("#MaskedTextBox4").val();
            $("#BackGround").css("display", "block");
            let BaseParameter = new Object();
            BaseParameter = {
                Action: TagIndex,
                ListSearchString: [],
            }
            BaseParameter.USER_ID = GetCookieValue("UserID");
            BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
            BaseParameter.RadioButton5 = document.getElementById("RadioButton5").checked;
            BaseParameter.RadioButton6 = document.getElementById("RadioButton6").checked;
            BaseParameter.ListSearchString.push(AAA);
            BaseParameter.ListSearchString.push(BBB);
            BaseParameter.ListSearchString.push(CCC);
            BaseParameter.ListSearchString.push(PO_CODE);
            BaseParameter.ListSearchString.push(MaskedTextBox4);
            let formUpload = new FormData();
            formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
            let url = "/D04/Buttonsave_Click";

            fetch(url, {
                method: "POST",
                body: formUpload,
                headers: {
                }
            }).then((response) => {
                response.json().then((data) => {
                    $("#LOC_Label17").val($("#LOC_TextBox2").val());
                    var audio = new Audio("/Media/HOOK_S.wav");
                    audio.play();
                    M.toast({ html: "정상처리 되었습니다. Đã được lưu.", classes: 'green', displayLength: 6000 });
                    $("#LOC_TextBox1").val("");
                    $("#LOC_TextBox2").val("");
                    document.getElementById("LOC_TextBox1").readOnly = false;
                    $("#LOC_TextBox1").focus();
                    $("#BackGround").css("display", "none");
                }).catch((err) => {
                    $("#BackGround").css("display", "none");
                })
            });
        }
    }
    if (TagIndex == 7) {
        let IsSave = true;
        let LOC_TextBox1 = $("#LOC_TextBox1").val();
        let LOC_TextBox2 = $("#LOC_TextBox2").val();
        if (LOC_TextBox1 == "") {
            IsSave = false;
        }
        if (LOC_TextBox2 == "") {
            IsSave = false;
        }
        if (IsSave == true) {
            let AAA = $("#LOC_Label14").val();
            let BBB = $("#LOC_TextBox2").val();
            $("#BackGround").css("display", "block");
            let BaseParameter = new Object();
            BaseParameter = {
                Action: TagIndex,
                ListSearchString: [],
            }
            BaseParameter.USER_ID = GetCookieValue("UserID");
            BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
            BaseParameter.ListSearchString.push(AAA);
            BaseParameter.ListSearchString.push(BBB);
            let formUpload = new FormData();
            formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
            let url = "/D04/Buttonsave_Click";

            fetch(url, {
                method: "POST",
                body: formUpload,
                headers: {
                }
            }).then((response) => {
                response.json().then((data) => {
                    $("#LOC_Label17").val($("#LOC_TextBox2").val());
                    var audio = new Audio("/Media/HOOK_S.wav");
                    audio.play();
                    M.toast({ html: "정상처리 되었습니다. Đã được lưu.", classes: 'green', displayLength: 6000 });
                    $("#LOC_TextBox1").val("");
                    $("#LOC_TextBox2").val("");
                    document.getElementById("LOC_TextBox1").readOnly = false;
                    $("#LOC_TextBox1").focus();
                    $("#BackGround").css("display", "none");
                }).catch((err) => {
                    $("#BackGround").css("display", "none");
                })
            });
        }
    }

}
function Buttondelete_Click() {
    if (TagIndex == 1) {
        let IsDelete = true;
        let DelAA = BaseResult.DataGridView3[DataGridView3RowIndex].QTY;
        let DelBB = BaseResult.DataGridView3[DataGridView3RowIndex].PART_CODE;
        for (let i = 0; i < BaseResult.DGV_OUT.length; i++) {
            let GGV_OUT12 = BaseResult.DGV_OUT[i].PART_IDX;
            if (GGV_OUT12 == DelBB) {
                BaseResult.DGV_OUT[i].QTY1 = BaseResult.DGV_OUT[i].QTY1 - DelAA;
                let LBQTY = Number($("#LBQTY").val());
                LBQTY = LBQTY - DelAA;
                $("#LBQTY").val(LBQTY);
                LBQTY_TextChanged();
                BOX_CONT = BOX_CONT - 1;
                $("#Label36").val(BOX_CONT);
                i = BaseResult.DGV_OUT.length;
            }
        }
        DGV_OUTRender();
        let ckPART = BaseResult.DataGridView3[DataGridView3RowIndex].PART_NO;
        let QTY = DelAA;
        let S_CHKPART = false;
        let S_QTY = 1;
        for (let i = 0; i < BaseResult.DataGridView1.length; i++) {
            let S_AA = BaseResult.DataGridView1[i].QTY;
            let S_BB = BaseResult.DataGridView1[i].BOX_COUNT;
            let DGV1_PARTNO = BaseResult.DataGridView1[i].PART_NO;
            if (ckPART == DGV1_PARTNO) {
                S_CHKPART = true;
                BaseResult.DataGridView1[i].QTY = S_AA - QTY;
                BaseResult.DataGridView1[i].BOX_COUNT = S_BB - 1;
                if (BaseResult.DataGridView1[i].QTY == 0) {
                    BaseResult.DataGridView1.splice(i, 1);
                }
                i = BaseResult.DataGridView1.length;
            }
        }
        DataGridView1Render();
        BaseResult.DataGridView3.splice(DataGridView3RowIndex, 1);
        DataGridView3Render();
        if (BaseResult.DataGridView3.length == 0) {
            document.getElementById("MaskedTextBox6").readOnly = false;
            document.getElementById("MaskedTextBox5").readOnly = false;

            $("#ITEMBCID").val("");
            $("#TextBox10").val("");
            $("#MaskedTextBox6").val("");
            $("#MaskedTextBox5").val("");
            $("#MaskedTextBox6").focus();
        }
        else {
            $("#ITEMBCID").val("");
            $("#ITEMBCID").focus();
        }
    }
    if (TagIndex == 2) {
        let IsDelete = true;
        if (confirm("Delete the selected Notice data?.")) {
            if (BaseResult.DataGridView2.length <= 0) {
                IsDelete = false;
            }
            if (IsDelete == true) {
                for (let i = 0; i < BaseResult.DataGridView2.length; i++) {
                    let BBB = BaseResult.DataGridView2[i].VLID_DSCN_YN;
                    if (BBB == "N") {
                        IsDelete = false;
                    }
                }
                if (IsDelete == true) {
                    $("#BackGround").css("display", "block");
                    let BaseParameter = new Object();
                    BaseParameter = {
                        Action: TagIndex,
                    }
                    BaseParameter.USER_ID = GetCookieValue("UserID");
                    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
                    BaseParameter.DataGridView2 = BaseResult.DataGridView2;
                    let formUpload = new FormData();
                    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
                    let url = "/D04/Buttondelete_Click";

                    fetch(url, {
                        method: "POST",
                        body: formUpload,
                        headers: {
                        }
                    }).then((response) => {
                        response.json().then((data) => {
                            Buttonfind_Click();
                            alert("Delete date Completed.");
                            $("#BackGround").css("display", "none");
                        }).catch((err) => {
                            $("#BackGround").css("display", "none");
                        })
                    });
                }
            }
        }
    }
    if (TagIndex == 6) {
        let IsDelete = true;
        if (confirm("Delete the selected Notice data?.")) {
            if (BaseResult.DataGridView8.length <= 0) {
                IsDelete = false;
                alert("Select Pallet NO. Please Check Again.");
            }
            if (IsDelete == true) {
                $("#BackGround").css("display", "block");
                let BaseParameter = new Object();
                BaseParameter = {
                    Action: TagIndex,
                }
                BaseParameter.USER_ID = GetCookieValue("UserID");
                BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
                BaseParameter.DataGridView8 = BaseResult.DataGridView8;
                let formUpload = new FormData();
                formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
                let url = "/D04/Buttondelete_Click";

                fetch(url, {
                    method: "POST",
                    body: formUpload,
                    headers: {
                    }
                }).then((response) => {
                    response.json().then((data) => {
                        Buttonfind_Click();
                        alert("Delete date Completed.");
                        //$("#BackGround").css("display", "none");
                    }).catch((err) => {
                        $("#BackGround").css("display", "none");
                    })
                });
            }
        }
    }

}
function Buttoncancel_Click() {
    if (TagIndex == 1) {
        $("#DateTimePicker3").val(DateNow);
    }
}
function Buttoninport_Click() {

}
function Buttonexport_Click() {
    if (TagIndex == 1) {
        let fileName = "D04" + DateNow + "_TP1";
        TableHTMLToExcel("DGV_OUTTable", fileName, fileName);
    }
    if (TagIndex == 2) {
        let fileName = "D04" + DateNow + "_TP2";
        DataGridView2RenderSub();
        TableHTMLToExcel("DataGridView2Table", fileName, fileName);
        DataGridView2Render();
        //let DataGridView2 = BaseResult.DataGridView2;
        //let ADATE = $("#DTPsuck1").val();
        //let BDATE = $("#DTPsuck2").val();
        //let LPARKNO = $("#TB_LIST_PARTNO").val();
        //let SERIERID = $("#TextBox1").val();
        //let LPOCODE = $("#TextBox11").val();
        //let LPNTNO = $("#TextBox12").val();
        //let PART_NM = $("#TextBox15").val();
        //let RO_BC = $("#TextBox21").val();
        //$("#BackGround").css("display", "block");
        //let BaseParameter = new Object();
        //BaseParameter = {
        //    Action: TagIndex,
        //    ListSearchString: [],
        //}
        //BaseParameter.USER_ID = GetCookieValue("UserID");
        //BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
        //BaseParameter.ListSearchString.push(ADATE);
        //BaseParameter.ListSearchString.push(BDATE);
        //BaseParameter.ListSearchString.push(LPARKNO);
        //BaseParameter.ListSearchString.push(SERIERID);
        //BaseParameter.ListSearchString.push(LPOCODE);
        //BaseParameter.ListSearchString.push(LPNTNO);
        //BaseParameter.ListSearchString.push(PART_NM);
        //BaseParameter.ListSearchString.push(RO_BC);
        //BaseParameter.ListSearchString.push(BC_DSCN_YN);
        //let formUpload = new FormData();
        //formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        //let url = "/D04/Buttonexport_Click";

        //fetch(url, {
        //    method: "POST",
        //    body: formUpload,
        //    headers: {
        //    }
        //}).then((response) => {
        //    response.json().then((data) => {
        //        let BaseResultButtonfind = data;
        //        BaseResult.DataGridView2 = BaseResultButtonfind.DataGridView2;
        //        BaseResult.ErrorNumber = BaseResultButtonfind.ErrorNumber;
        //        DataGridView2Render();
        //        $("#Label67").val(BaseResult.ErrorNumber);                

        //        let fileName = "D04" + DateNow + "_TP2";
        //        TableHTMLToExcel("DataGridView2Table", fileName, fileName);

        //        $("#BackGround").css("display", "none");
        //    }).catch((err) => {
        //        BaseResult.DataGridView2 = DataGridView2;
        //        DataGridView2Render();
        //        let fileName = "D04" + DateNow + "_TP2";
        //        TableHTMLToExcel("DataGridView2Table", fileName, fileName);
        //        $("#BackGround").css("display", "none");
        //    })
        //});


    }
    if (TagIndex == 5) {
        let fileName = "D04" + DateNow + "_TP5";
        TableHTMLToExcel("DataGridView7Table", fileName, fileName);
    }
    if (TagIndex == 6) {
        let fileName = "D04" + DateNow + "_TP8";
        TableHTMLToExcel("DataGridView8Table", fileName, fileName);
    }
}
function Buttonprint_Click() {
    if (TagIndex == 1) {
        localStorage.setItem("D04_PLT_PRNT_MaskedTextBox1", $("#MaskedTextBox1").val());
        let url = "/D04_PLT_PRNT";
        OpenWindowByURL(url, 800, 460);
    }
    if (TagIndex == 5) {
        //DGV7_LOAD(1);
        Buttonprint_ClickSub();
    }
}
function Buttonprint_ClickSub() {
    //let IsCheck = true;
    //let TextBox14 = $("#TextBox14").val();
    //if (TextBox14 <= 13) {
    //    IsCheck = false;
    //    alert("팔렛트 번호 없음. NOT Pallet NO. Một lỗi đã xảy ra.");
    //}
    //if (BaseResult.DataGridView7.length <= 0) {
    //    IsCheck = false;
    //    alert("데이터 없음. DATA NULL. Một lỗi đã xảy ra.");
    //}
    //if (IsCheck == true) {
    //    let SU01 = $("#TextBox14").val();
    //    let SU02 = $("#TextBox2").val();
    //    let SU03 = $("#TextBox3").val();
    //    $("#BackGround").css("display", "block");
    //    let BaseParameter = new Object();
    //    BaseParameter = {
    //        Action: TagIndex,
    //        ListSearchString: [],
    //    }
    //    BaseParameter.USER_ID = GetCookieValue("UserID");
    //    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    //    BaseParameter.RadioButton9 = document.getElementById("RadioButton9").checked;
    //    BaseParameter.RadioButton10 = document.getElementById("RadioButton10").checked;
    //    BaseParameter.RadioButton14 = document.getElementById("RadioButton14").checked;
    //    BaseParameter.ListSearchString.push(SU01);
    //    BaseParameter.ListSearchString.push(SU02);
    //    BaseParameter.ListSearchString.push(SU03);
    //    let formUpload = new FormData();
    //    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    //    let url = "/D04/Buttonprint_Click";

    //    fetch(url, {
    //        method: "POST",
    //        body: formUpload,
    //        headers: {
    //        }
    //    }).then((response) => {
    //        response.json().then((data) => {
    //            let BaseResultPrint = data;
    //            if (BaseResultPrint) {
    //                if (BaseResultPrint.Code) {
    //                    let url = BaseResultPrint.Code;
    //                    OpenWindowByURL(url, 200, 200);
    //                    $("#BackGround").css("display", "none");
    //                }
    //            }
    //        }).catch((err) => {
    //            $("#BackGround").css("display", "none");
    //        })
    //    });
    //}


    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        Action: TagIndex,
        ListSearchString: [],
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    BaseParameter.RadioButton9 = document.getElementById("RadioButton9").checked;
    BaseParameter.RadioButton10 = document.getElementById("RadioButton10").checked;
    BaseParameter.RadioButton14 = document.getElementById("RadioButton14").checked;
    BaseParameter.ListSearchString.push($("#TextBox211").val());
    BaseParameter.ListSearchString.push($("#TextBox3").val());
    BaseParameter.T_DGV_02 = BaseResult.T_DGV_02;
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/D04/Buttonprint_Click";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            //BaseResult.DGV_01 = data.DGV_01;
            //if (BaseResult.DGV_01 != null && BaseResult.DGV_01.length > 0) {
            //    for (let i = 0; i < BaseResult.DGV_01.length; i++) {
            //        let url = BaseResult.DGV_01[i].CONTENT;
            //        console.log(url);
            //        window.open(url, '_blank').focus();
            //    }
            //}
            let BaseResultPrint = data;
            if (BaseResultPrint) {
                if (BaseResultPrint.Code) {
                    let url = BaseResultPrint.Code;
                    window.open(url, '_blank').focus();
                }
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
function Buttonhelp_Click() {
    let url = "/WMP_PLAY";
    OpenWindowByURL(url, 800, 460);
}
function Buttonclose_Click() {
    history.back();
}

let PalletNODataGridViewRowIndex = 0;
function PalletNODataGridViewRender(Action) {
    let HTML = "";
    if (BaseResult) {
        if (BaseResult.T_DGV_02) {
            if (BaseResult.T_DGV_02.length > 0) {
                if (Action == 0) {
                    BaseResult.T_DGV_02[0].CHK = true;
                }
                for (let i = 0; i < BaseResult.T_DGV_02.length; i++) {
                    HTML = HTML + "<tr>";
                    if (BaseResult.T_DGV_02[i].CHK) {
                        HTML = HTML + "<td><label><input id='PalletNODataGridViewCHK" + i + "' class='form-check-input' type='checkbox' checked onclick='PalletNODataGridViewCHKChanged(" + i + ")'><span></span></label></td>";
                    }
                    else {
                        HTML = HTML + "<td><label><input id='PalletNODataGridViewCHK" + i + "' class='form-check-input' type='checkbox' onclick='PalletNODataGridViewCHKChanged(" + i + ")'><span></span></label></td>";
                    }
                    HTML = HTML + "<td>" + BaseResult.T_DGV_02[i].PLET_NO + "</td>";
                    HTML = HTML + "</tr>";
                }
            }
        }
    }
    document.getElementById("PalletNODataGridView").innerHTML = HTML;
}
function PalletNODataGridViewSort() {
    IsTableSort = !IsTableSort;
    DataGridViewSort(BaseResult.T_DGV_02, IsTableSort);
    PalletNODataGridViewRender(1);
}
function PalletNODataGridViewCHKChanged(i) {
    let id = "PalletNODataGridViewCHK" + i;
    PalletNODataGridViewRowIndex = i;
    BaseResult.T_DGV_02[PalletNODataGridViewRowIndex].CHK = $("#" + id).is(":checked");
}
function PalletNODataGridViewCHKChangedAll() {
    for (let i = 0; i < BaseResult.T_DGV_02.length; i++) {
        BaseResult.T_DGV_02[i].CHK = $("#PalletNOCheckBox").is(":checked");
    }
    PalletNODataGridViewRender(1);
}
function DataGridView1Render() {
    let HTML = "";
    if (BaseResult) {
        if (BaseResult.DataGridView1) {
            if (BaseResult.DataGridView1.length > 0) {
                DataGridView1_SelectionChanged(0);
                for (let i = 0; i < BaseResult.DataGridView1.length; i++) {
                    HTML = HTML + "<tr onclick='DataGridView1_SelectionChanged(" + i + ")'>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].PART_NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].PART_LINE + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].QTY + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].BOX_COUNT + "</td>";
                    HTML = HTML + "</tr>";
                }
            }
        }
    }
    document.getElementById("DataGridView1").innerHTML = HTML;
}
function DataGridView1Sort() {
    //IsTableSort = !IsTableSort;
    //DataGridViewSort(BaseResult.DataGridView1, IsTableSort);
    //DataGridView1Render();
}
function DataGridView1_SelectionChanged(i) {
    DataGridView1RowIndex = i;
}
let DataGridView1Table = document.getElementById("DataGridView1Table");
DataGridView1Table.addEventListener('click', function (event) {
    if (event.target.tagName === 'TH') {
        let text = event.target.innerText;
        let key = "PART_NO";
        IsTableSort = !IsTableSort;
        ListSort(BaseResult.DataGridView1, key, text, IsTableSort);
        DataGridView1Render();
    }
});
function DataGridView2RenderSub() {
    let HTML = "";
    if (BaseResult) {
        if (BaseResult.DataGridView2) {
            if (BaseResult.DataGridView2.length > 0) {
                DataGridView2_SelectionChanged(0);
                for (let i = 0; i < BaseResult.DataGridView2.length; i++) {
                    HTML = HTML + "<tr onclick='DataGridView2_SelectionChanged(" + i + ")'>";
                    HTML = HTML + "<td></td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].PO_CODE + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].PART_NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].PART_NAME + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].PART_FML + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].PART_STAGE + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].PART_SNP + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].PO_QTY + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].Pallet_NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].PALLET_CODE + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].SERIAL_ID + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].INPUT_DATE + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].Good_BRCODE + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].VLID_REMARK + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].SHIPPING_DATE + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].VLID_DSCN_YN + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].STOCK + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].PDMTIN_IDX + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].DONE_YN + "</td>";
                    HTML = HTML + "</tr>";
                }
            }
        }
    }
    document.getElementById("DataGridView2").innerHTML = HTML;
}
function DataGridView2Render() {
    let HTML = "";
    if (BaseResult) {
        if (BaseResult.DataGridView2) {
            if (BaseResult.DataGridView2.length > 0) {
                DataGridView2_SelectionChanged(0);
                for (let i = 0; i < BaseResult.DataGridView2.length; i++) {
                    HTML = HTML + "<tr onclick='DataGridView2_SelectionChanged(" + i + ")'>";
                    if (BaseResult.DataGridView2[i].CHK) {
                        HTML = HTML + "<td onclick='DataGridView2CHKChanged(" + i + ")'><input class='form-check-input' type='checkbox' checked ><span></span></label></td>";
                    }
                    else {
                        HTML = HTML + "<td onclick='DataGridView2CHKChanged(" + i + ")'><input class='form-check-input' type='checkbox'><span></span></label></td>";
                    }
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].PO_CODE + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].PART_NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].PART_NAME + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].PART_FML + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].PART_STAGE + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].PART_SNP + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].PO_QTY + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].Pallet_NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].PALLET_CODE + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].SERIAL_ID + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].INPUT_DATE + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].Good_BRCODE + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].VLID_REMARK + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].SHIPPING_DATE + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].VLID_DSCN_YN + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].STOCK + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].PDMTIN_IDX + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].DONE_YN + "</td>";
                    HTML = HTML + "</tr>";
                }
            }
        }
    }
    document.getElementById("DataGridView2").innerHTML = HTML;
}
function DataGridView2Sort() {
    //IsTableSort = !IsTableSort;
    //DataGridViewSort(BaseResult.DataGridView2, IsTableSort);
    //DataGridView2Render();
}
function DataGridView2_SelectionChanged(i) {
    DataGridView2RowIndex = i;
}
function DataGridView2CHKChanged(i) {
    DataGridView2RowIndex = i;
    BaseResult.DataGridView2[DataGridView2RowIndex].CHK = !BaseResult.DataGridView2[DataGridView2RowIndex].CHK;
    DataGridView2Render();
}
let DataGridView2Table = document.getElementById("DataGridView2Table");
DataGridView2Table.addEventListener('click', function (event) {
    if (event.target.tagName === 'TH') {
        let text = event.target.innerText;
        let key = "CHK";
        IsTableSort = !IsTableSort;
        switch (text) {
            case "GOOOD_BRCODE":
                if (IsTableSort == true) {
                    BaseResult.DataGridView2.sort((a, b) => (a.Good_BRCODE > b.Good_BRCODE ? 1 : -1));
                }
                else {
                    BaseResult.DataGridView2.sort((a, b) => (a.Good_BRCODE < b.Good_BRCODE ? 1 : -1));
                }
                break;
            case "PALLET_NO":
                if (IsTableSort == true) {
                    BaseResult.DataGridView8.sort((a, b) => (a.Pallet_NO > b.Pallet_NO ? 1 : -1));
                }
                else {
                    BaseResult.DataGridView8.sort((a, b) => (a.Pallet_NO < b.Pallet_NO ? 1 : -1));
                }
                break;
            default:
                ListSort(BaseResult.DataGridView2, key, text, IsTableSort);
                break;
        }
        DataGridView2Render();
    }
});
function DataGridView2_DataError() {
    alert("Too much data. Please Check Again.");
}
function DataGridView3Render() {
    let HTML = "";
    if (BaseResult) {
        if (BaseResult.DataGridView3) {
            if (BaseResult.DataGridView3.length > 0) {
                DataGridView3_SelectionChanged(0);
                for (let i = 0; i < BaseResult.DataGridView3.length; i++) {
                    HTML = HTML + "<tr onclick='DataGridView3_SelectionChanged(" + i + ")'>";
                    if (BaseResult.DataGridView3[i].CHK) {
                        HTML = HTML + "<td onclick='DataGridView3CHKChanged(" + i + ")'><input class='form-check-input' type='checkbox' checked><span></span></label></td>";
                    }
                    else {
                        HTML = HTML + "<td onclick='DataGridView3CHKChanged(" + i + ")'><input class='form-check-input' type='checkbox'><span></span></label></td>";
                    }
                    HTML = HTML + "<td>" + BaseResult.DataGridView3[i].Pallet_NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView3[i].Customer_Pallet + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView3[i].Serial_Barcode + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView3[i].QTY + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView3[i].PART_NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView3[i].PO_CODE + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView3[i].Split + "</td>";
                    HTML = HTML + "</tr>";
                }
            }
        }
    }
    document.getElementById("DataGridView3").innerHTML = HTML;
}
function DataGridView3Sort() {
    //IsTableSort = !IsTableSort;
    //DataGridViewSort(BaseResult.DataGridView3, IsTableSort);
    //DataGridView3Render();
}
function DataGridView3_SelectionChanged(i) {
    DataGridView3RowIndex = i;
}
function DataGridView3CHKChanged(i) {
    DataGridView3RowIndex = i;
    BaseResult.DataGridView3[DataGridView3RowIndex].CHK = !BaseResult.DataGridView3[DataGridView3RowIndex].CHK;
    DataGridView3Render();
}
let DataGridView3Table = document.getElementById("DataGridView3Table");
DataGridView3Table.addEventListener('click', function (event) {
    if (event.target.tagName === 'TH') {
        let text = event.target.innerText;
        let key = "CHK";
        IsTableSort = !IsTableSort;
        ListSort(BaseResult.DataGridView3, key, text, IsTableSort);
        DataGridView3Render();
    }
});
function DataGridView6Render() {
    let HTML = "";
    if (BaseResult) {
        if (BaseResult.DataGridView6) {
            if (BaseResult.DataGridView6.length > 0) {
                DataGridView6_SelectionChanged(0);
                for (let i = 0; i < BaseResult.DataGridView6.length; i++) {
                    HTML = HTML + "<tr onclick='DataGridView6_SelectionChanged(" + i + ")'>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView6[i].PART_NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView6[i].PART_NAME + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView6[i].PART_FML + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView6[i].PART_STAGE + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView6[i].SHIPPING_DATE + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView6[i].QTY + "</td>";
                    HTML = HTML + "</tr>";
                }
            }
        }
    }
    document.getElementById("DataGridView6").innerHTML = HTML;
}
function DataGridView6Sort() {
    //IsTableSort = !IsTableSort;
    //DataGridViewSort(BaseResult.DataGridView6, IsTableSort);
    //DataGridView6Render();
}
function DataGridView6_SelectionChanged(i) {
    DataGridView6RowIndex = i;
}
let DataGridView6Table = document.getElementById("DataGridView6Table");
DataGridView6Table.addEventListener('click', function (event) {
    if (event.target.tagName === 'TH') {
        let text = event.target.innerText;
        let key = "PART_NO";
        IsTableSort = !IsTableSort;
        ListSort(BaseResult.DataGridView6, key, text, IsTableSort);
        DataGridView6Render();
    }
});
function DataGridView7Render() {
    let HTML = "";
    if (BaseResult) {
        if (BaseResult.DataGridView7) {
            if (BaseResult.DataGridView7.length > 0) {
                DataGridView7_SelectionChanged(0);
                for (let i = 0; i < BaseResult.DataGridView7.length; i++) {
                    HTML = HTML + "<tr onclick='DataGridView7_SelectionChanged(" + i + ")'>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView7[i].CODE + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView7[i].PO_CODE + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView7[i].PLET_NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView7[i].SERIAL_ID + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView7[i].PART_NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView7[i].PART_GRP + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView7[i].PART_NAME + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView7[i].PART_SNP + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView7[i].PO_QTY + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView7[i].QTY + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView7[i].BOX_QTY + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView7[i].Not_yet_packing + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView7[i].Inventory + "</td>";
                    //HTML = HTML + "<td>" + BaseResult.DataGridView7[i].TDPDOTPLMU_IDX + "</td>";
                    //HTML = HTML + "<td>" + BaseResult.DataGridView7[i].PDOTPL_IDX + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView7[i].Name + "</td>";
                    HTML = HTML + "</tr>";
                }
            }
        }
    }
    document.getElementById("DataGridView7").innerHTML = HTML;
}
function DataGridView7Sort() {
    //IsTableSort = !IsTableSort;
    //DataGridViewSort(BaseResult.DataGridView7, IsTableSort);
    //DataGridView7Render();
}
function DataGridView7_SelectionChanged(i) {
    DataGridView7RowIndex = i;
    $("#MaskedTextBox2").val(BaseResult.DataGridView7[DataGridView7RowIndex].PLET_NO);
    $("#TextBox4").val(BaseResult.DataGridView7[DataGridView7RowIndex].SERIAL_ID);
    $("#TextBox5").val(BaseResult.DataGridView7[DataGridView7RowIndex].PART_NO);
    $("#Label22").val(BaseResult.DataGridView7[DataGridView7RowIndex].TDPDOTPLMU_IDX);
    $("#Label34").val(BaseResult.DataGridView7[DataGridView7RowIndex].PDOTPL_IDX);
}
let DataGridView7Table = document.getElementById("DataGridView7Table");
DataGridView7Table.addEventListener('click', function (event) {
    if (event.target.tagName === 'TH') {
        let text = event.target.innerText;
        let key = "CODE";
        IsTableSort = !IsTableSort;
        ListSort(BaseResult.DataGridView7, key, text, IsTableSort);
        DataGridView7Render();
    }
});
function DataGridView8Render() {
    let HTML = "";
    if (BaseResult) {
        if (BaseResult.DataGridView8) {
            if (BaseResult.DataGridView8.length > 0) {
                DataGridView8_SelectionChanged(0);
                for (let i = 0; i < BaseResult.DataGridView8.length; i++) {
                    HTML = HTML + "<tr onclick='DataGridView8_SelectionChanged(" + i + ")'>";
                    if (BaseResult.DataGridView8[i].CHK) {
                        HTML = HTML + "<td onclick='DataGridView8CHKChanged(" + i + ")'><input class='form-check-input' type='checkbox' checked><span></span></td>";
                    }
                    else {
                        HTML = HTML + "<td onclick='DataGridView8CHKChanged(" + i + ")'><input class='form-check-input' type='checkbox'><span></span></td>";
                    }
                    HTML = HTML + "<td>" + BaseResult.DataGridView8[i].CODE + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView8[i].PO_CODE + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView8[i].PLET_NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView8[i].CM_PALLET_NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView8[i].PART_NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView8[i].PART_GRP + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView8[i].PART_NM + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView8[i].PART_SNP + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView8[i].VLID_GRP + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView8[i].CREATE_DTM + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView8[i].PO_QTY + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView8[i].QTY + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView8[i].BOX_QTY + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView8[i].Not_yet_packing + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView8[i].Inventory + "</td>";
                    if (BaseResult.DataGridView8[i].Ship_Status == "Waiting") {
                        HTML = HTML + "<td style='background-color: yellow;'>" + BaseResult.DataGridView8[i].Ship_Status + "</td>";
                    }
                    else {
                        HTML = HTML + "<td style='background-color: lime;'>" + BaseResult.DataGridView8[i].Ship_Status + "</td>";
                    }
                    HTML = HTML + "<td>" + BaseResult.DataGridView8[i].DONE_YN + "</td>";
                    HTML = HTML + "</tr>";
                }
            }
        }
    }
    document.getElementById("DataGridView8").innerHTML = HTML;
}
function DataGridView8Sort() {
    //IsTableSort = !IsTableSort;
    //DataGridViewSort(BaseResult.DataGridView8, IsTableSort);
    //DataGridView8Render();
}
function DataGridView8_SelectionChanged(i) {
    DataGridView8RowIndex = i;
}
function DataGridView8CHKChanged(i) {
    DataGridView8RowIndex = i;
    BaseResult.DataGridView8[DataGridView8RowIndex].CHK = !BaseResult.DataGridView8[DataGridView8RowIndex].CHK;
    DataGridView8Render();
}
let DataGridView8Table = document.getElementById("DataGridView8Table");
DataGridView8Table.addEventListener('click', function (event) {
    if (event.target.tagName === 'TH') {
        let text = event.target.innerText;
        let key = "CHK";
        IsTableSort = !IsTableSort;
        switch (text) {
            case "SERIAL ID":
                if (IsTableSort == true) {
                    BaseResult.DataGridView8.sort((a, b) => (a.VLID_GRP > b.VLID_GRP ? 1 : -1));
                }
                else {
                    BaseResult.DataGridView8.sort((a, b) => (a.VLID_GRP < b.VLID_GRP ? 1 : -1));
                }
                break;
            case "SCAN_DATE":
                if (IsTableSort == true) {
                    BaseResult.DataGridView8.sort((a, b) => (a.CREATE_DTM > b.CREATE_DTM ? 1 : -1));
                }
                else {
                    BaseResult.DataGridView8.sort((a, b) => (a.CREATE_DTM < b.CREATE_DTM ? 1 : -1));
                }
                break;
            default:
                ListSort(BaseResult.DataGridView8, key, text, IsTableSort);
                break;
        }
        DataGridView8Render();
    }
});
function DataGridView8_Paint() {
    DGV8_SORT();
}
function DGV8_SORT() {

}
function DataGridView9Render() {
    let HTML = "";
    if (BaseResult) {
        if (BaseResult.DataGridView9) {
            if (BaseResult.DataGridView9.length > 0) {
                DataGridView9_SelectionChanged(0);
                for (let i = 0; i < BaseResult.DataGridView9.length; i++) {
                    HTML = HTML + "<tr onclick='DataGridView9_SelectionChanged(" + i + ")'>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView9[i].PART_NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView9[i].Name + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView9[i].VLID_DTM + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView9[i].LOC + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView9[i].QTY + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView9[i].VLID_GRP + "</td>";
                    HTML = HTML + "</tr>";
                }
            }
        }
    }
    document.getElementById("DataGridView9").innerHTML = HTML;
}
function DataGridView9Sort() {
    //IsTableSort = !IsTableSort;
    //DataGridViewSort(BaseResult.DataGridView9, IsTableSort);
    //DataGridView9Render();
}
function DataGridView9_SelectionChanged(i) {
    DataGridView9RowIndex = i;
}
let DataGridView9Table = document.getElementById("DataGridView9Table");
DataGridView9Table.addEventListener('click', function (event) {
    if (event.target.tagName === 'TH') {
        let text = event.target.innerText;
        let key = "PART_NO";
        IsTableSort = !IsTableSort;
        if (text == "LOCATION") {
            if (IsTableSort == true) {
                BaseResult.DataGridView9.sort((a, b) => (a.LOC > b.LOC ? 1 : -1));
            }
            else {
                BaseResult.DataGridView9.sort((a, b) => (a.LOC < b.LOC ? 1 : -1));
            }
        }
        else {
            if (text == "SERIAL_NO") {
                if (IsTableSort == true) {
                    BaseResult.DataGridView9.sort((a, b) => (a.VLID_GRP > b.VLID_GRP ? 1 : -1));
                }
                else {
                    BaseResult.DataGridView9.sort((a, b) => (a.VLID_GRP < b.VLID_GRP ? 1 : -1));
                }
            }
            else {
                ListSort(BaseResult.DataGridView9, key, text, IsTableSort);
            }
        }

        DataGridView9Render();
    }
});
function DataGridView10Render() {
    let HTML = "";
    if (BaseResult) {
        if (BaseResult.DataGridView10) {
            if (BaseResult.DataGridView10.length > 0) {
                DataGridView10_SelectionChanged(0);
                for (let i = 0; i < BaseResult.DataGridView10.length; i++) {
                    HTML = HTML + "<tr onclick='DataGridView10_SelectionChanged(" + i + ")'>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView10[i].PO_CODE + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView10[i].PART_NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView10[i].PART_GRP + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView10[i].PART_NM + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView10[i].PART_SNP + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView10[i].PO_QTY + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView10[i].QTY + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView10[i].BOX_QTY + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView10[i].Not_yet_packing + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView10[i].Inventory + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView10[i].Ship_Status + "</td>";
                    HTML = HTML + "</tr>";
                }
            }
        }
    }
    document.getElementById("DataGridView10").innerHTML = HTML;
}
function DataGridView10Sort() {
    IsTableSort = !IsTableSort;
    DataGridViewSort(BaseResult.DataGridView10, IsTableSort);
    DataGridView10Render();
}
function DataGridView10_SelectionChanged(i) {
    DataGridView10RowIndex = i;
}
let DataGridView10Table = document.getElementById("DataGridView10Table");
DataGridView10Table.addEventListener('click', function (event) {
    if (event.target.tagName === 'TH') {
        let text = event.target.innerText;
        let key = "PO_CODE";
        IsTableSort = !IsTableSort;
        ListSort(BaseResult.DataGridView10, key, text, IsTableSort);
        DataGridView10Render();
    }
});
function DataGridView11Render() {
    let HTML = "";
    if (BaseResult) {
        if (BaseResult.DataGridView11) {
            if (BaseResult.DataGridView11.length > 0) {
                DataGridView11_SelectionChanged(0);
                for (let i = 0; i < BaseResult.DataGridView11.length; i++) {
                    if (BaseResult.DataGridView11[i].PLET_NO1) {
                        BaseResult.DataGridView11[i].PLET_NO1 = BaseResult.DataGridView11[i].PLET_NO1.trim();
                    }
                    HTML = HTML + "<tr onclick='DataGridView11_SelectionChanged(" + i + ")'>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView11[i].PLET_NO1 + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView11[i].PART_NM + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView11[i].PART_NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView11[i].PART_SNP + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView11[i].BOX_QTY + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView11[i].PALLET_SERIAL_NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView11[i].REMARK + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView11[i].QTY + "</td>";
                    HTML = HTML + "</tr>";
                }
            }
        }
    }
    document.getElementById("DataGridView11").innerHTML = HTML;
}
function DataGridView11Sort() {
    //IsTableSort = !IsTableSort;
    //DataGridViewSort(BaseResult.DataGridView11, IsTableSort);
    //DataGridView11Render();
}
function DataGridView11_SelectionChanged(i) {
    DataGridView11RowIndex = i;
}
let DataGridView11Table = document.getElementById("DataGridView11Table");
DataGridView11Table.addEventListener('click', function (event) {
    if (event.target.tagName === 'TH') {
        let text = event.target.innerText;
        if (text == "PALLET_NO") {
            text = "PLET_NO1";
        }
        let key = "PLET_NO1";
        IsTableSort = !IsTableSort;
        ListSort(BaseResult.DataGridView11, key, text, IsTableSort);
        DataGridView11Render();
    }
});
function DGV_OUTRender() {
    let HTML = "";
    if (BaseResult) {
        if (BaseResult.DGV_OUT) {
            if (BaseResult.DGV_OUT.length > 0) {
                DGV_OUT_SelectionChanged(0);
                for (let i = 0; i < BaseResult.DGV_OUT.length; i++) {
                    let SORT_NO = BaseResult.DGV_OUT[i].SORTNO;
                    HTML = HTML + "<tr id='DGV_OUTRow" + i + "' ondblclick='DGV_OUT_DoubleClick(" + i + ")' onclick='DGV_OUT_SelectionChanged(" + i + ")' style='font-size: 14px;'>";
                    if (SORT_NO == 1) {
                        HTML = HTML + "<td style='background-color: orange; padding: 2px;'>" + BaseResult.DGV_OUT[i].PO_CODE + "</td>";
                        HTML = HTML + "<td style='background-color: orange; padding: 2px;'>" + BaseResult.DGV_OUT[i].PART_NO + "</td>";
                        HTML = HTML + "<td style='background-color: orange; padding: 2px;'>" + BaseResult.DGV_OUT[i].PART_GRP + "</td>";
                        HTML = HTML + "<td style='background-color: orange; padding: 2px;'>" + BaseResult.DGV_OUT[i].PART_NM + "</td>";
                        HTML = HTML + "<td style='background-color: orange; padding: 2px;'>" + BaseResult.DGV_OUT[i].PART_SNP + "</td>";
                        HTML = HTML + "<td style='background-color: orange; padding: 2px;'>" + BaseResult.DGV_OUT[i].PO_QTY + "</td>";
                    }
                    else {
                        if (SORT_NO == 2) {
                            HTML = HTML + "<td style='background-color: yellow; padding: 2px;'>" + BaseResult.DGV_OUT[i].PO_CODE + "</td>";
                            HTML = HTML + "<td style='background-color: yellow; padding: 2px;'>" + BaseResult.DGV_OUT[i].PART_NO + "</td>";
                            HTML = HTML + "<td style='background-color: yellow; padding: 2px;'>" + BaseResult.DGV_OUT[i].PART_GRP + "</td>";
                            HTML = HTML + "<td style='background-color: yellow; padding: 2px;'>" + BaseResult.DGV_OUT[i].PART_NM + "</td>";
                            HTML = HTML + "<td style='background-color: yellow; padding: 2px;'>" + BaseResult.DGV_OUT[i].PART_SNP + "</td>";
                            HTML = HTML + "<td style='background-color: yellow; padding: 2px;'>" + BaseResult.DGV_OUT[i].PO_QTY + "</td>";
                        }
                        else {
                            HTML = HTML + "<td style='padding: 2px;'>" + BaseResult.DGV_OUT[i].PO_CODE + "</td>";
                            HTML = HTML + "<td style='padding: 2px;'>" + BaseResult.DGV_OUT[i].PART_NO + "</td>";
                            HTML = HTML + "<td style='padding: 2px;'>" + BaseResult.DGV_OUT[i].PART_GRP + "</td>";
                            HTML = HTML + "<td style='padding: 2px;'>" + BaseResult.DGV_OUT[i].PART_NM + "</td>";
                            HTML = HTML + "<td style='padding: 2px;'>" + BaseResult.DGV_OUT[i].PART_SNP + "</td>";
                            HTML = HTML + "<td style='padding: 2px;'>" + BaseResult.DGV_OUT[i].PO_QTY + "</td>";
                        }
                    }
                    HTML = HTML + "<td style='padding: 2px;'>" + BaseResult.DGV_OUT[i].NT_QTY + "</td>";
                    HTML = HTML + "<td style='padding: 2px;'>" + BaseResult.DGV_OUT[i].STOCK + "</td>";
                    HTML = HTML + "<td style='padding: 2px;'>" + BaseResult.DGV_OUT[i].QTY + "</td>";
                    HTML = HTML + "<td style='padding: 2px;'>" + BaseResult.DGV_OUT[i].BOX_QTY + "</td>";
                    HTML = HTML + "<td style='padding: 2px;'>" + BaseResult.DGV_OUT[i].PACK_QTY + "</td>";
                    HTML = HTML + "<td style='padding: 2px;'>" + BaseResult.DGV_OUT[i].Not_yet_packing + "</td>";
                    if (BaseResult.DGV_OUT[i].STATUS <= 0) {
                        HTML = HTML + "<td style='background-color: red; padding: 2px;'>" + BaseResult.DGV_OUT[i].STATUS + "</td>";
                    }
                    else {
                        HTML = HTML + "<td style='padding: 2px;'>" + BaseResult.DGV_OUT[i].STATUS + "</td>";
                    }
                    HTML = HTML + "<td style='background-color: yellow; padding: 2px;'>" + BaseResult.DGV_OUT[i].QTY1 + "</td>";
                    HTML = HTML + "</tr>";
                }
            }
        }
    }
    document.getElementById("DGV_OUT").innerHTML = HTML;
}
let DGV_OUTTable = document.getElementById("DGV_OUTTable");
DGV_OUTTable.addEventListener('click', function (event) {
    if (event.target.tagName === 'TD') {
        IsTableClick = true;
    }
    if (event.target.tagName === 'TH') {
        let text = event.target.innerText;
        let key = "PO_CODE";
        IsTableSort = !IsTableSort;
        switch (text) {
            case "PO_CODE":
                if (IsTableSort == true) {
                    BaseResult.DGV_OUT.sort((a, b) => (a.PO_CODE > b.PO_CODE ? 1 : -1));
                }
                else {
                    BaseResult.DGV_OUT.sort((a, b) => (a.PO_CODE < b.PO_CODE ? 1 : -1));
                }
                break;
            case "PART_NO":
                if (IsTableSort == true) {
                    BaseResult.DGV_OUT.sort((a, b) => (a.PART_NO > b.PART_NO ? 1 : -1));
                }
                else {
                    BaseResult.DGV_OUT.sort((a, b) => (a.PART_NO < b.PART_NO ? 1 : -1));
                }
                break;
            case "PART_LINE":
                if (IsTableSort == true) {
                    BaseResult.DGV_OUT.sort((a, b) => (a.PART_GRP > b.PART_GRP ? 1 : -1));
                }
                else {
                    BaseResult.DGV_OUT.sort((a, b) => (a.PART_GRP < b.PART_GRP ? 1 : -1));
                }
                break;
            case "PART_NM":
                if (IsTableSort == true) {
                    BaseResult.DGV_OUT.sort((a, b) => (a.PART_NM > b.PART_NM ? 1 : -1));
                }
                else {
                    BaseResult.DGV_OUT.sort((a, b) => (a.PART_NM < b.PART_NM ? 1 : -1));
                }
                break;
            case "SNP":
                if (IsTableSort == true) {
                    BaseResult.DGV_OUT.sort((a, b) => (a.PART_SNP > b.PART_SNP ? 1 : -1));
                }
                else {
                    BaseResult.DGV_OUT.sort((a, b) => (a.PART_SNP < b.PART_SNP ? 1 : -1));
                }
                break;
            case "SNP":
                if (IsTableSort == true) {
                    BaseResult.DGV_OUT.sort((a, b) => (a.PART_SNP > b.PART_SNP ? 1 : -1));
                }
                else {
                    BaseResult.DGV_OUT.sort((a, b) => (a.PART_SNP < b.PART_SNP ? 1 : -1));
                }
                break;
            case "PO_QTY":
                if (IsTableSort == true) {
                    BaseResult.DGV_OUT.sort((a, b) => (a.PO_QTY > b.PO_QTY ? 1 : -1));
                }
                else {
                    BaseResult.DGV_OUT.sort((a, b) => (a.PO_QTY < b.PO_QTY ? 1 : -1));
                }
                break;
            case "NEXT_PO":
                if (IsTableSort == true) {
                    BaseResult.DGV_OUT.sort((a, b) => (a.NT_QTY > b.NT_QTY ? 1 : -1));
                }
                else {
                    BaseResult.DGV_OUT.sort((a, b) => (a.NT_QTY < b.NT_QTY ? 1 : -1));
                }
                break;
            case "STOCK":
                if (IsTableSort == true) {
                    BaseResult.DGV_OUT.sort((a, b) => (a.STOCK > b.STOCK ? 1 : -1));
                }
                else {
                    BaseResult.DGV_OUT.sort((a, b) => (a.STOCK < b.STOCK ? 1 : -1));
                }
                break;
            case "QTY":
                if (IsTableSort == true) {
                    BaseResult.DGV_OUT.sort((a, b) => (a.QTY > b.QTY ? 1 : -1));
                }
                else {
                    BaseResult.DGV_OUT.sort((a, b) => (a.QTY < b.QTY ? 1 : -1));
                }
                break;
            case "BOX_QTY":
                if (IsTableSort == true) {
                    BaseResult.DGV_OUT.sort((a, b) => (a.BOX_QTY > b.BOX_QTY ? 1 : -1));
                }
                else {
                    BaseResult.DGV_OUT.sort((a, b) => (a.BOX_QTY < b.BOX_QTY ? 1 : -1));
                }
                break;
            case "PACK_QTY":
                if (IsTableSort == true) {
                    BaseResult.DGV_OUT.sort((a, b) => (a.PACK_QTY > b.PACK_QTY ? 1 : -1));
                }
                else {
                    BaseResult.DGV_OUT.sort((a, b) => (a.PACK_QTY < b.PACK_QTY ? 1 : -1));
                }
                break;
            case "NOT_YET_PACKING":
                if (IsTableSort == true) {
                    BaseResult.DGV_OUT.sort((a, b) => (a.Not_yet_packing > b.Not_yet_packing ? 1 : -1));
                }
                else {
                    BaseResult.DGV_OUT.sort((a, b) => (a.Not_yet_packing < b.Not_yet_packing ? 1 : -1));
                }
                break;
            case "STATUS":
                if (IsTableSort == true) {
                    BaseResult.DGV_OUT.sort((a, b) => (a.STATUS > b.STATUS ? 1 : -1));
                }
                else {
                    BaseResult.DGV_OUT.sort((a, b) => (a.STATUS < b.STATUS ? 1 : -1));
                }
                break;
            case "QTY1":
                if (IsTableSort == true) {
                    BaseResult.DGV_OUT.sort((a, b) => (a.QTY1 > b.QTY1 ? 1 : -1));
                }
                else {
                    BaseResult.DGV_OUT.sort((a, b) => (a.QTY1 < b.QTY1 ? 1 : -1));
                }
                break;
            default:
                ListSort(BaseResult.DGV_OUT, key, text, IsTableSort);
                break;
        }
        DGV_OUTRender();
    }
});
function DGV_OUTSort() {
    //IsTableSort = !IsTableSort;
    //DataGridViewSort(BaseResult.DGV_OUT, IsTableSort);
    //DGV_OUTRender();
}
function DGV_OUT_SelectionChanged(i) {
    DGV_OUTRowIndex = i;
    let A0 = BaseResult.DGV_OUT[DGV_OUTRowIndex].PDOTPL_IDX;
    let A1 = BaseResult.DGV_OUT[DGV_OUTRowIndex].PO_CODE;
    let A2 = BaseResult.DGV_OUT[DGV_OUTRowIndex].PART_NO;
    let A3 = BaseResult.DGV_OUT[DGV_OUTRowIndex].PO_QTY;
    let A4 = BaseResult.DGV_OUT[DGV_OUTRowIndex].PACK_QTY;

    $("#TextBoxA0").val(A0);
    $("#TextBoxA1").val(A1);
    $("#TextBoxA2").val(A2);
    $("#TextBoxA3").val(A3);
    $("#TextBox6").val(A4);
    document.getElementById("TextBoxA1").readOnly = true;
    document.getElementById("TextBoxA2").readOnly = true;
}
document.addEventListener('keydown', function (event) {
    if (IsTableClick == true) {
        if (event.key == "ArrowDown") {
            DGV_OUTRowIndex = DGV_OUTRowIndex + 1;
            if (DGV_OUTRowIndex > BaseResult.DGV_OUT.length - 1) {
                DGV_OUTRowIndex = BaseResult.DGV_OUT.length - 1;
            }
        }
        if (event.key == "ArrowUp") {
            DGV_OUTRowIndex = DGV_OUTRowIndex - 1;
            if (DGV_OUTRowIndex < 0) {
                DGV_OUTRowIndex = 0;
            }
        }

        for (let i = 0; i < BaseResult.DGV_OUT.length; i++) {
            let ID = "DGV_OUTRow" + i;
            document.getElementById(ID).classList.remove('selected');
        }
        let IDCurent = "DGV_OUTRow" + DGV_OUTRowIndex;
        document.getElementById(IDCurent).classList.toggle('selected');
        DGV_OUT_SelectionChanged(DGV_OUTRowIndex);
    }
});

function DGV_OUT_DoubleClick(i) {
    DGV_OUTRowIndex = i;
    let A01 = BaseResult.DGV_OUT[DGV_OUTRowIndex].PART_NO;
    let A02 = BaseResult.DGV_OUT[DGV_OUTRowIndex].PART_GRP;
    let A03 = BaseResult.DGV_OUT[DGV_OUTRowIndex].PART_NM;
    let A04 = BaseResult.DGV_OUT[DGV_OUTRowIndex].PO_QTY;
    let A05 = BaseResult.DGV_OUT[DGV_OUTRowIndex].PDOTPL_IDX;

    localStorage.setItem("D04_RANK_Label1", "PART NO : " + A01);
    localStorage.setItem("D04_RANK_Label2", "PART NAME : " + A02);
    localStorage.setItem("D04_RANK_Label3", "LINE : " + A03);
    localStorage.setItem("D04_RANK_Label4", "QTY : " + A04);
    localStorage.setItem("D04_RANK_Label5", A05);

    let url = "/D04_RANK";
    OpenWindowByURL(url, 800, 460);
    D04_RANKTimerStartInterval();
}

function D04_RANKTimerStartInterval() {
    D04_RANKTimer = setInterval(function () {
        let D04_RANK_Close = localStorage.getItem("D04_RANK_Close");
        if (D04_RANK_Close == "1") {
            clearInterval(D04_RANKTimer);
            D04_QTY_YN_Close = 0;
            localStorage.setItem("D04_RANK_Close", D04_RANK_Close);
            Buttonfind_Click();
        }
    }, 100);
}
