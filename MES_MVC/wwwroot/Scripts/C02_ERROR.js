let IsTableSort = false;
let BaseResult = new Object();
let EW_TIME_Timer1;
let StartTime;
let LAST_ID;

$(window).focus(function () {
}).blur(function () {
    Buttonclose_Click();
});

$(document).ready(function () {
    localStorage.setItem("C02_ERROR_Close", 0);

    WORING_IDX = localStorage.getItem("WORING_IDX");
    if (WORING_IDX == null) {
        WORING_IDX = 0;
        localStorage.setItem("WORING_IDX", WORING_IDX);
    }

    $("#Label1").val(localStorage.getItem("C02_ERROR_Label1"));

    StartTime = new Date();

    $("#TextBox1").val("");
    $("#LBT1R").val("");
    $("#LBT2R").val("");
    $("#LBA1R").val("");
    $("#LBA2R").val("");
    $("#LBS1R").val("");
    $("#LBS2R").val("");
    $("#LBWR").val("");
    $("#LBT1V").val("");
    $("#LBT2V").val("");
    $("#LBA1V").val("");
    $("#LBA2V").val("");
    $("#LBS1V").val("");
    $("#LBS2V").val("");
    $("#LBWV").val("");
    $("#LBA1S").val("");
    $("#LBA2S").val("");
    $("#LBA1SEQ").val("");
    $("#LBA2SEQ").val("");
    $("#LBA1IDX").val("");
    $("#LBA2IDX").val("");
    $("#LBR1").val("");
    $("#LBR2").val("");
    $("#LBR3").val("");
    $("#LBR4").val("");
    $("#LBR5").val("");
    $("#LBR11").val("");
    $("#LBR22").val("");

    document.getElementById("LBR1").style.backgroundColor = "white";
    document.getElementById("LBR2").style.backgroundColor = "white";
    document.getElementById("LBR3").style.backgroundColor = "white";
    document.getElementById("LBR4").style.backgroundColor = "white";
    document.getElementById("LBR5").style.backgroundColor = "white";
    document.getElementById("LBR11").style.backgroundColor = "white";
    document.getElementById("LBR22").style.backgroundColor = "white";

    document.getElementById("CHBT1").checked = false;
    document.getElementById("CHBA1").checked = false;
    document.getElementById("CHBT2").checked = false;
    document.getElementById("CHBA2").checked = false;
    document.getElementById("CHBS1").checked = false;
    document.getElementById("CHBS2").checked = false;
    document.getElementById("CHBW").checked = false;

    C02_ERROR_Load();
});

// load thông tin form lỗi liên quan
function C02_ERROR_Load() {
    let IsCheck = true;
    let Label1 = $("#Label1").val();
    let MCbox = localStorage.getItem("C02_MCbox");
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        ListSearchString: [],
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    BaseParameter.ListSearchString.push(Label1);
    BaseParameter.ListSearchString.push(MCbox);
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C02_ERROR/C02_ERROR_Load";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            let BaseResultSub = data;
            BaseResult.DataGridView1 = BaseResult.DataGridView1;
            BaseResult.DataGridView2 = BaseResult.DataGridView2;
            BaseResult.DataGridView3 = BaseResult.DataGridView3;

            if (BaseResult.DataGridView1.length > 0) {
                $("#LBT1S").val(BaseResult.DataGridView1[0].TERM1);
                $("#LBA1S").val(BaseResult.DataGridView1[0].TERM1);
                $("#LBS1S").val(BaseResult.DataGridView1[0].SEAL1);
                $("#LBT2S").val(BaseResult.DataGridView1[0].TERM2);
                $("#LBA2S").val(BaseResult.DataGridView1[0].TERM2);
                $("#LBS2S").val(BaseResult.DataGridView1[0].SEAL2);
                $("#Label2").val(BaseResult.DataGridView1[0].WIRE);
            }

            $("#ComboBox1").empty();

            let PP = BaseResult.DataGridView1.length;
            if (PP != 0) {
                IsCheck = false;
                localStorage.setItem("C02_START_V2_C_EXIT", true);
                Buttonclose_Click();
            }
            if (IsCheck == true) {
                if (BaseResult.DataGridView2.length > 0) {
                    if (BaseResult.DataGridView2[0].WIRE_IDX == 0) {
                        document.getElementById("CHBW").checked = false;
                    }
                    else {
                        document.getElementById("CHBW").checked = true;
                        var ComboBox1 = document.getElementById("ComboBox1");

                        var option = document.createElement("option");
                        option.text = "WIRE";
                        option.value = "WIRE";
                        ComboBox1.add(option);
                    }
                    if (BaseResult.DataGridView2[0].T1_IDX == 0) {
                        document.getElementById("CHBT1").checked = false;
                        document.getElementById("CHBA1").checked = false;
                    }
                    else {
                        document.getElementById("CHBT1").checked = true;
                        document.getElementById("CHBA1").checked = true;

                        var ComboBox1 = document.getElementById("ComboBox1");

                        var option = document.createElement("option");
                        option.text = "TERM1";
                        option.value = "TERM1";
                        ComboBox1.add(option);

                        option = document.createElement("option");
                        option.text = "APPLICATOR1";
                        option.value = "APPLICATOR1";
                        ComboBox1.add(option);
                    }
                    if (BaseResult.DataGridView2[0].S1_IDX == 0) {
                        document.getElementById("CHBS1").checked = false;
                    }
                    else {
                        document.getElementById("CHBS1").checked = true;
                        var ComboBox1 = document.getElementById("ComboBox1");

                        var option = document.createElement("option");
                        option.text = "SEAL1";
                        option.value = "SEAL1";
                        ComboBox1.add(option);
                    }
                    if (BaseResult.DataGridView2[0].T2_IDX == 0) {
                        document.getElementById("CHBT2").checked = false;
                        document.getElementById("CHBA2").checked = false;
                    }
                    else {
                        document.getElementById("CHBT2").checked = true;
                        document.getElementById("CHBA2").checked = true;
                        var ComboBox1 = document.getElementById("ComboBox1");

                        var option = document.createElement("option");
                        option.text = "TERM2";
                        option.value = "TERM2";
                        ComboBox1.add(option);

                        option = document.createElement("option");
                        option.text = "APPLICATOR2";
                        option.value = "APPLICATOR2";
                        ComboBox1.add(option);
                    }
                    if (BaseResult.DataGridView2[0].S2_IDX == 0) {
                        document.getElementById("CHBS2").checked = false;
                    }
                    else {
                        document.getElementById("CHBS2").checked = true;
                        var ComboBox1 = document.getElementById("ComboBox1");

                        var option = document.createElement("option");
                        option.text = "SEAL2";
                        option.value = "SEAL2";
                        ComboBox1.add(option);
                    }
                    $("#LBWS").val(BaseResult.DataGridView1[0].WIRE_NM);
                    let C02_SHIELDWIRE_CHK = Boolean(localStorage.getItem("C02_SHIELDWIRE_CHK"));
                    if (C02_SHIELDWIRE_CHK == true) {
                        document.getElementById("CHBT1").checked = false;
                        document.getElementById("CHBA1").checked = false;
                        document.getElementById("CHBT2").checked = false;
                        document.getElementById("CHBA2").checked = false;
                        document.getElementById("CHBS1").checked = false;
                        document.getElementById("CHBS2").checked = false;
                    }

                }
                if (BaseResult.DataGridView2.length > 0) {
                    LAST_ID = BaseResult.DataGridView3[0].TSNON_OPER_IDX;
                }
                SW_TIME();
                EW_TIME_Timer1StartInterval();
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}

// lòa thông tin thời gian dừng làm việc
function SW_TIME() {
    let Text = "KOMAX_ERROR";
    let MCbox = localStorage.getItem("C02_MCbox");
    let Label1 = $("#Label1").val();
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        ListSearchString: [],
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    BaseParameter.ListSearchString.push(MCbox);
    BaseParameter.ListSearchString.push(Label1);
    BaseParameter.ListSearchString.push(Text);
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C02_ERROR/SW_TIME";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            let BaseResultSub = data;
            BaseResult.Search = BaseResult.Search;
            if (BaseResult.Search.length > 0) {
                WORING_IDX = BaseResult.Search[0].TOWT_INDX;
                localStorage.setItem("WORING_IDX", WORING_IDX);
            }

            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}

function EW_TIME_Timer1StartInterval() {
    EW_TIME_Timer1 = setInterval(function () {
        Timer1_Tick();
    }, 60000);
}
function Timer1_Tick() {
    EW_TIME(0);
}
function EW_TIME(Flag) {
    WORING_IDX = localStorage.getItem("WORING_IDX");
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        ListSearchString: [],
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    BaseParameter.ListSearchString.push(WORING_IDX);
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C02_ERROR/EW_TIME";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            if (Flag == 1) {
                localStorage.setItem("C02_ERROR_Close", 1);
                window.close();
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
$("#Button1").click(function (e) {
    Button1_Click();
});
function Button1_Click() {
    let IsCheck = true;
    let chuoiNhap = "";
    let ComboBox1 = $("#ComboBox1").val();
    let TextBox1 = $("#TextBox1").val();
    if (ComboBox1 == "WIRE") {
        chuoiNhap = TextBox1;
    }
    else {
        chuoiNhap = TextBox1.toUpperCase();
    }

    SettingsMASTER_BC = localStorage.getItem("SettingsMASTER_BC");
    let BC_MAST = SettingsMASTER_BC;

    let BAR_TEXT = false;
    if (ComboBox1 == "APPLICATOR1") {
        BAR_TEXT = true;
    }
    if (ComboBox1 == "APPLICATOR2") {
        BAR_TEXT = true;
    }
    if (chuoiNhap == BC_MAST) {
        BAR_TEXT = true;
    }

    if (BAR_TEXT == false) {
        $("#BackGround").css("display", "block");
        let BaseParameter = new Object();
        BaseParameter = {
            ListSearchString: [],
        }
        BaseParameter.USER_ID = GetCookieValue("UserID");
        BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
        BaseParameter.ListSearchString.push(chuoiNhap);

        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        let url = "/C02_ERROR/Button1_Click";

        fetch(url, {
            method: "POST",
            body: formUpload,
            headers: {
            }
        }).then((response) => {
            response.json().then((data) => {
                let BaseResultSub = data;
                BaseResult.DataGridView7 = BaseResultSub.DataGridView7;
                if (BaseResult.DataGridView7.length <= 0) {
                    IsCheck == false;
                    alert("오류가 발생 하였습니다.(바코드 이력 없음). BARCODE Một lỗi đã xảy ra.");
                }
                if (IsCheck == true) {
                    Button1_ClickSub001();
                }
                $("#BackGround").css("display", "none");
            }).catch((err) => {
                $("#BackGround").css("display", "none");
            })
        });
    }
    else {
        Button1_ClickSub001();
    }
}
function Button1_ClickSub001() {
    let IsCheck = true;
    let chuoiNhap = "";
    let ComboBox1 = $("#ComboBox1").val();
    let TextBox1 = $("#TextBox1").val();
    if (ComboBox1 == "WIRE") {
        chuoiNhap = TextBox1;
    }
    else {
        chuoiNhap = TextBox1.toUpperCase();
    }
    SettingsMASTER_BC = localStorage.getItem("SettingsMASTER_BC");
    let BC_MAST = SettingsMASTER_BC;
    switch (ComboBox1) {
        case "TERM1":
            try {
                let T1 = "";
                if (chuoiNhap == BC_MAST) {
                    T1 = $("#LBT1S").val();
                }
                else {
                    T1 = chuoiNhap.substring(0, chuoiNhap.indexOf("$$") - 1);
                }
                if ($("#LBT1S").val() == T1) {
                    $("#LBT1R").val("OK");
                    $("#LBR1").val("OK");
                    document.getElementById("LBR1").style.backgroundColor = "lime";
                    try {
                        let selectedIndex = document.getElementById("ComboBox1").selectedIndex;
                        selectedIndex = selectedIndex + 1;
                        document.getElementById("ComboBox1").selectedIndex = selectedIndex;
                    }
                    catch (e) {
                    }
                }
                else {
                    $("#LBT1R").val("NG");
                    $("#LBR1").val("NG");
                    document.getElementById("LBR1").style.backgroundColor = "red";
                }
                $("#LBT1V").val(T1);
                $("#TextBox1").val("");
            }
            catch (e) {
                alert("오류가 발생 하였습니다. Một lỗi đã xảy ra." + e);
            }
            break;
        case "TERM2":
            try {
                let T2 = "";
                if (chuoiNhap == BC_MAST) {
                    T2 = $("#LBT2S").val();
                }
                else {
                    T2 = chuoiNhap.substring(0, chuoiNhap.indexOf("$$") - 1);
                }
                if ($("#LBT2S").val() == T2) {
                    $("#LBT2R").val("OK");
                    $("#LBR2").val("OK");
                    document.getElementById("LBR2").style.backgroundColor = "lime";
                    try {
                        let selectedIndex = document.getElementById("ComboBox1").selectedIndex;
                        selectedIndex = selectedIndex + 1;
                        document.getElementById("ComboBox1").selectedIndex = selectedIndex;
                    }
                    catch (e) {
                    }
                }
                else {
                    $("#LBT2R").val("NG");
                    $("#LBR2").val("NG");
                    document.getElementById("LBR2").style.backgroundColor = "red";
                }
                $("#LBT2V").val(T2);
                $("#TextBox1").val("");
            }
            catch (e) {
                alert("오류가 발생 하였습니다. Một lỗi đã xảy ra." + e);
            }
            break;
        case "APPLICATOR1":
            let A1 = chuoiNhap.substring(0, chuoiNhap.length - 1);
            let LBA1S = $("#LBA1S").val();
            if (LBA1S == A1) {
                $("#LBA1R").val("OK");
                let LBA1SEQ = chuoiNhap.substring(chuoiNhap.length - 1, 1);
                $("#LBA1SEQ").val(LBA1SEQ);
                let ASC_SEQ = LBA1SEQ.charCodeAt(0);
                if (ASC_SEQ <= 64) {
                    IsCheck == false;
                    alert("No APPLICATOR Data in MES. Không có dữ liệu ỨNG DỤNG trong MES.");
                }
                if (ASC_SEQ >= 90) {
                    IsCheck == false;
                    alert("No APPLICATOR Data in MES. Không có dữ liệu ỨNG DỤNG trong MES.");
                }
                if (IsCheck == true) {
                    LBA1SEQ = $("#LBA1SEQ").val();
                    let Label1 = $("#Label1").val();
                    $("#BackGround").css("display", "block");
                    let BaseParameter = new Object();
                    BaseParameter = {
                        ListSearchString: [],
                    }
                    BaseParameter.USER_ID = GetCookieValue("UserID");
                    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
                    BaseParameter.ListSearchString.push(A1);
                    BaseParameter.ListSearchString.push(LBA1SEQ);
                    BaseParameter.ListSearchString.push(Label1);
                    let formUpload = new FormData();
                    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
                    let url = "/C02_ERROR/Button1_ClickSub001";

                    fetch(url, {
                        method: "POST",
                        body: formUpload,
                        headers: {
                        }
                    }).then((response) => {
                        response.json().then((data) => {
                            let BaseResultSub = data;
                            BaseResult.DataGridView4 = BaseResultSub.DataGridView4;
                            BaseResult.DataGridView5 = BaseResultSub.DataGridView5;
                            BaseResult.DataGridView6 = BaseResultSub.DataGridView6;
                            if (BaseResult.DataGridView4.length <= 0) {
                                if (BaseResult.DataGridView5.length <= 0) {
                                    IsCheck == false;
                                    alert("Barcode Please Check Again. & NOT add APPLICATOR data");
                                    $("#LBA1R").val("Not data");
                                    $("#LBA1V").val(A1);
                                    $("#TextBox1").val("");
                                }
                                else {
                                    if (BaseResult.DataGridView6.length > 0) {
                                        $("#LBR11").val(BaseResult.DataGridView6[0].WK_CNT);
                                        document.getElementById("LBR11").style.backgroundColor = "lime";
                                    }
                                }
                            }
                            else {
                                $("#LBR11").val(BaseResult.DataGridView4[0].WK_CNT);
                                document.getElementById("LBR11").style.backgroundColor = "lime";
                                $("#LBA1IDX").val(BaseResult.DataGridView4[0].TOOL_IDX);
                            }
                            if (IsCheck == true) {
                                document.getElementById("ComboBox1").selectedIndex = document.getElementById("ComboBox1").selectedIndex + 1;
                            }
                            $("#BackGround").css("display", "none");
                        }).catch((err) => {
                            $("#BackGround").css("display", "none");
                        })
                    });
                }
            }
            else {
                $("#LBA1R").val("NG");
                document.getElementById("LBR11").style.backgroundColor = "red";
            }
            $("#LBA1V").val(A1);
            $("#TextBox1").val("");
            break;
        case "APPLICATOR2":
            let A2 = chuoiNhap.substring(0, chuoiNhap.length - 1);
            let LBA2S = $("#LBA2S").val();
            if (LBA2S == A1) {
                $("#LBA2R").val("OK");
                let LBA2SEQ = chuoiNhap.substring(chuoiNhap.length - 1, 1);
                $("#LBA2SEQ").val(LBA2SEQ);
                let ASC_SEQ = LBA2SEQ.charCodeAt(0);
                if (ASC_SEQ <= 64) {
                    IsCheck == false;
                    alert("No APPLICATOR Data in MES. Không có dữ liệu ỨNG DỤNG trong MES.");
                }
                if (ASC_SEQ >= 90) {
                    IsCheck == false;
                    alert("No APPLICATOR Data in MES. Không có dữ liệu ỨNG DỤNG trong MES.");
                }
                if (IsCheck == true) {
                    LBA2SEQ = $("#LBA2SEQ").val();
                    let Label1 = $("#Label1").val();
                    $("#BackGround").css("display", "block");
                    let BaseParameter = new Object();
                    BaseParameter = {
                        ListSearchString: [],
                    }
                    BaseParameter.USER_ID = GetCookieValue("UserID");
                    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
                    BaseParameter.ListSearchString.push(A2);
                    BaseParameter.ListSearchString.push(LBA2SEQ);
                    BaseParameter.ListSearchString.push(Label1);
                    let formUpload = new FormData();
                    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
                    let url = "/C02_ERROR/Button1_ClickSub002";

                    fetch(url, {
                        method: "POST",
                        body: formUpload,
                        headers: {
                        }
                    }).then((response) => {
                        response.json().then((data) => {
                            let BaseResultSub = data;
                            BaseResult.DataGridView7 = BaseResultSub.DataGridView7;
                            BaseResult.DataGridView8 = BaseResultSub.DataGridView8;
                            BaseResult.DataGridView9 = BaseResultSub.DataGridView9;
                            if (BaseResult.DataGridView7.length <= 0) {
                                if (BaseResult.DataGridView8.length <= 0) {
                                    IsCheck == false;
                                    alert("Barcode Please Check Again. & NOT add APPLICATOR data");
                                    $("#LBA2R").val("Not data");
                                    $("#LBA2V").val(A2);
                                    $("#TextBox1").val("");
                                }
                                else {
                                    if (BaseResult.DataGridView9.length > 0) {
                                        $("#LBR22").val(BaseResult.DataGridView9[0].WK_CNT);
                                        document.getElementById("LBR22").style.backgroundColor = "lime";
                                    }
                                }
                            }
                            else {
                                $("#LBR22").val(BaseResult.DataGridView7[0].WK_CNT);
                                document.getElementById("LBR22").style.backgroundColor = "lime";
                                $("#LBA2IDX").val(BaseResult.DataGridView7[0].TOOL_IDX);
                            }
                            if (IsCheck == true) {
                                document.getElementById("ComboBox1").selectedIndex = document.getElementById("ComboBox1").selectedIndex + 1;
                            }
                            $("#BackGround").css("display", "none");
                        }).catch((err) => {
                            $("#BackGround").css("display", "none");
                        })
                    });
                }
            }
            else {
                $("#LBA2R").val("NG");
                document.getElementById("LBR22").style.backgroundColor = "red";
            }
            $("#LBA2V").val(A2);
            $("#TextBox1").val("");
            break;
        case "SEAL1":
            try {
                let S1 = "";
                if (chuoiNhap == BC_MAST) {
                    S1 = $("#LBS1S").val();
                }
                else {
                    S1 = chuoiNhap.substring(0, chuoiNhap.indexOf("$$") - 1);
                }
                if ($("#LBS1S").val() == S1) {
                    $("#LBS1R").val("OK");
                    $("#LBR3").val("OK");
                    document.getElementById("LBR3").style.backgroundColor = "lime";
                    try {
                        let selectedIndex = document.getElementById("ComboBox1").selectedIndex;
                        selectedIndex = selectedIndex + 1;
                        document.getElementById("ComboBox1").selectedIndex = selectedIndex;
                    }
                    catch (e) {
                    }
                }
                else {
                    $("#LBS1R").val("NG");
                    $("#LBR3").val("NG");
                    document.getElementById("LBR3").style.backgroundColor = "red";
                }
                $("#LBS1V").val(S1);
                $("#TextBox1").val("");
            }
            catch (e) {
                alert("Barcode Please Check Again." + e);
            }
            break;
        case "SEAL2":
            try {
                let S2 = "";
                if (chuoiNhap == BC_MAST) {
                    S2 = $("#LBS2S").val();
                }
                else {
                    S2 = chuoiNhap.substring(0, chuoiNhap.indexOf("$$") - 1);
                }
                if ($("#LBS2S").val() == S2) {
                    $("#LBS2R").val("OK");
                    $("#LBR4").val("OK");
                    document.getElementById("LBR4").style.backgroundColor = "lime";
                    try {
                        let selectedIndex = document.getElementById("ComboBox1").selectedIndex;
                        selectedIndex = selectedIndex + 1;
                        document.getElementById("ComboBox1").selectedIndex = selectedIndex;
                    }
                    catch (e) {
                    }
                }
                else {
                    $("#LBS2R").val("NG");
                    $("#LBR4").val("NG");
                    document.getElementById("LBR4").style.backgroundColor = "red";
                }
                $("#LBS2V").val(S2);
                $("#TextBox1").val("");
            }
            catch (e) {
                alert("오류가 발생 하였습니다. Một lỗi đã xảy ra." + e);
            }
            break;
        case "WIRE":
            try {
                let WW = "";
                if (chuoiNhap == BC_MAST) {
                    WW = $("#LBWS").val();
                }
                else {
                    WW = chuoiNhap.substring(0, chuoiNhap.indexOf("$$") - 1);
                }
                if ($("#LBWS").val() == WW) {
                    $("#LBWR").val("OK");
                    $("#LBR5").val("OK");
                    document.getElementById("LBR5").style.backgroundColor = "lime";
                    try {
                        let selectedIndex = document.getElementById("ComboBox1").selectedIndex;
                        selectedIndex = selectedIndex + 1;
                        document.getElementById("ComboBox1").selectedIndex = selectedIndex;
                    }
                    catch (e) {
                    }
                }
                else {
                    $("#LBWR").val("NG");
                    $("#LBR5").val("NG");
                    document.getElementById("LBR5").style.backgroundColor = "red";
                }
                $("#LBWV").val(WW);
                $("#TextBox1").val("");
            }
            catch (e) {
                alert("오류가 발생 하였습니다. Một lỗi đã xảy ra." + e);
            }
            break;
    }
    $("#TextBox1").focus();
}
$("#Button2").click(function (e) {
    Button2_Click();
});
function Button2_Click() {
    let AA1 = document.getElementById("CHBT1").checked;
    let AA2 = document.getElementById("CHBT2").checked;
    let AA3 = document.getElementById("CHBA1").checked;
    let AA4 = document.getElementById("CHBA2").checked;
    let AA5 = document.getElementById("CHBS1").checked;
    let AA6 = document.getElementById("CHBS2").checked;
    let AA7 = document.getElementById("CHBW").checked;

    let BB1 = false;
    let BB2 = false;
    let BB3 = false;
    let BB4 = false;
    let BB5 = false;
    let BB6 = false;
    let BB7 = false;

    if (AA1 == true) {
        if ($("#LBT1R").val() == "OK") {
            BB1 = true;
        }
    }
    else {
        AA1 = true;
        BB1 = true;
    }
    if (AA2 == true) {
        if ($("#LBT2R").val() == "OK") {
            BB2 = true;
        }
    }
    else {
        AA2 = true;
        BB2 = true;
    }
    if (AA3 == true) {
        if ($("#LBA1R").val() == "OK") {
            BB3 = true;
        }
    }
    else {
        AA3 = true;
        BB3 = true;
    }
    if (AA4 == true) {
        if ($("#LBA2R").val() == "OK") {
            BB4 = true;
        }
    }
    else {
        AA4 = true;
        BB4 = true;
    }
    if (AA5 == true) {
        if ($("#LBS1R").val() == "OK") {
            BB5 = true;
        }
    }
    else {
        AA5 = true;
        BB5 = true;
    }
    if (AA6 == true) {
        if ($("#LBS2R").val() == "OK") {
            BB6 = true;
        }
    }
    else {
        AA6 = true;
        BB6 = true;
    }
    if (AA7 == true) {
        if ($("#LBWR").val() == "OK") {
            BB7 = true;
        }
    }
    else {
        AA7 = true;
        BB7 = true;
    }
    if ((AA1 == true) && (BB1 == true) && (AA2 == true) && (BB2 == true) && (AA3 == true) && (BB3 == true) && (AA4 == true) && (BB4 == true) && (AA5 == true) && (BB5 == true) && (AA6 == true) && (BB6 == true) && (AA7 == true) && (BB7 == true)) {
        let Label1 = $("#Label1").val();
        $("#BackGround").css("display", "block");
        let BaseParameter = new Object();
        BaseParameter = {
            ListSearchString: [],
        }
        BaseParameter.USER_ID = GetCookieValue("UserID");
        BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
        BaseParameter.ListSearchString.push(Label1);
        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        let url = "/C02_ERROR/Button2_Click";

        fetch(url, {
            method: "POST",
            body: formUpload,
            headers: {
            }
        }).then((response) => {
            response.json().then((data) => {
                localStorage.setItem("C02_START_V2_C_EXIT", true);
                C02_ERROR_FormClosed();
                $("#BackGround").css("display", "none");
            }).catch((err) => {
                $("#BackGround").css("display", "none");
            })
        });
    }

}
$("#TextBox1").keydown(function (e) {
    if (e.keyCode == 13) {
        TextBox1_KeyDown();
    }
});
function TextBox1_KeyDown() {
    Button1_Click();
}
$("#Buttonclose").click(function (e) {
    Buttonclose_Click();
});
function Buttonclose_Click() {
    localStorage.setItem("C02_START_V2_C_EXIT", true);
    C02_ERROR_FormClosed();
}
function C02_ERROR_FormClosed() {
    let MCbox = localStorage.getItem("C02_MCbox");
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        ListSearchString: [],
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    BaseParameter.ListSearchString.push(LAST_ID);
    BaseParameter.ListSearchString.push(MCbox);
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C02_ERROR/C02_ERROR_FormClosed";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            localStorage.setItem("SettingsNON_OPER_CHK", false);
            EW_TIME(1);
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
