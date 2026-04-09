let IsTableSort = false;
let BaseResult;
let StartTime;
let RunTime;
let LAST_ID;
$(window).focus(function () {
}).blur(function () {
    Buttonclose_Click();
});

$(document).ready(function () {
    localStorage.setItem("C05_ER_L_Close", 0);

    $("#LBT2S").val(localStorage.getItem("C05_ER_L_LBT2S"));
    $("#LBA2S").val(localStorage.getItem("C05_ER_L_LBA2S"));
    $("#Label1").val(localStorage.getItem("C05_ER_L_Label1"));

    StartTime = new Date();
    $("#TextBox1").val("");
    $("#LBT2R").val("");
    $("#LBA2R").val("");
    $("#LBT2V").val("");
    $("#LBA2V").val("");
    $("#LBA2SEQ").val("");
    $("#LBA2IDX").val("");
    $("#LBR2").val("");
    $("#LBR4").val("");
    $("#LBR22").val("");

    document.getElementById("CHBT2").checked = true;
    document.getElementById("CHBA2").checked = true;

    document.getElementById("LBR2").style.backgroundColor = "white";
    document.getElementById("LBR4").style.backgroundColor = "white";
    document.getElementById("LBR22").style.backgroundColor = "white";

    $("#ComboBox1").empty();

    if ($("#LBT2S").val() == "") {
        Buttonclose_Click();
    }

    var ComboBox1 = document.getElementById("ComboBox1");

    var option = document.createElement("option");
    option.text = "TERM";
    option.value = "TERM";
    ComboBox1.add(option);

    option = document.createElement("option");
    option.text = "APPLICATOR";
    option.value = "APPLICATOR";
    ComboBox1.add(option);
    C11_ERROR_Load();
});
function C11_ERROR_Load() {
    let MCbox = localStorage.getItem("C11_MCbox");
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        SearchString: MCbox,
    }
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C05_ER_L/C11_ERROR_Load";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            let BaseResultSub = data;
            BaseResult.Search = BaseResultSub.Search;
            LAST_ID = BaseResult.Search[0].TSNON_OPER_IDX;
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
    let AA = $("#TextBox1").val();
    AA = AA.toUpperCase();
    SettingsMASTER_BC = localStorage.getItem("SettingsMASTER_BC");
    let BC_MAST = SettingsMASTER_BC;
    let ComboBox1 = $("#ComboBox1").val();
    switch (ComboBox1) {
        case "TERM":
            try {
                let LBT2S = $("#LBT2S").val();
                let T2 = "";
                if (AA == BC_MAST) {
                    T2 = LBT2S;
                }
                else {
                    T2 = AA.substring(0, AA.indexOf("$$") - 1);
                }
                if (LBT2S == T2) {
                    $("#LBT2R").val("OK");
                    $("#LBR2").val("OK");
                    document.getElementById("LBR2").style.backgroundColor = "lime";
                    try {
                        let selectedIndex = Number(document.getElementById("ComboBox1").selectedIndex);
                        selectedIndex = selectedIndex + 1;
                        document.getElementById("ComboBox1").selectedIndex = selectedIndex.toString();
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
                alert("Barcode Please Check Again. " + e);
            }
            break;
        case "APPLICATOR":
            let A2 = AA.substring(0, AA.length - 1);
            let LBA2S = $("#LBA2S").val();
            if (LBA2S == A2) {
                $("#LBA2R").val("OK");
                let LBA2SEQ = AA.substring(AA.length - 1, 1);
                $("#LBA2SEQ").val(LBA2SEQ);
                let ASC_SEQ = LBA2SEQ.charCodeAt(0);
                if (ASC_SEQ <= 64) {
                    IsCheck == false;
                    alert("No APPLICATOR Data in MES. Không có dữ liệu ỨNG DỤNG trong MES.");
                }
                if (ASC_SEQ >= 72) {
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
                    let url = "/C05_ER_L/Button1_Click";

                    fetch(url, {
                        method: "POST",
                        body: formUpload,
                        headers: {
                        }
                    }).then((response) => {
                        response.json().then((data) => {
                            let BaseResultSub = data;
                            BaseResult.Search = BaseResultSub.Search;
                            BaseResult.Search1 = BaseResultSub.Search1;
                            BaseResult.DataGridView = BaseResultSub.DataGridView;
                            if (BaseResult.Search.length <= 0) {
                                if (BaseResult.Search1.length <= 0) {
                                    IsCheck == false;
                                    alert("Barcode Please Check Again. & NOT add APPLICATOR data");
                                    $("#LBA2R").val("Not data");
                                    $("#LBA2V").val(A2);
                                    $("#TextBox1").val("");
                                }
                                else {
                                    let WK_CNT = BaseResult.DataGridView[0].WK_CNT;
                                    $("#LBR22").val(WK_CNT);
                                    document.getElementById("LBR22").style.backgroundColor = "lime";
                                }
                            }
                            else {
                                let WK_CNT = BaseResult.Search[0].WK_CNT;
                                $("#LBR22").val(WK_CNT);
                                document.getElementById("LBR22").style.backgroundColor = "lime";
                                let TOOL_IDX = BaseResult.Search[0].TOOL_IDX;
                                $("#LBA2IDX").val(TOOL_IDX);
                            }
                            if (IsCheck == true) {

                            }
                            $("#BackGround").css("display", "none");
                        }).catch((err) => {
                            $("#BackGround").css("display", "none");
                        })
                    });
                }
                $("#LBA2V").val(A2);
                $("#TextBox1").val("");
            }
            break;
    }
    $("#TextBox1").focus();
}
$("#Button2").click(function (e) {
    Button2_Click();
});
function Button2_Click() {
    let AA2 = document.getElementById("CHBT2").checked;
    let AA4 = document.getElementById("CHBA2").checked;
    let BB2 = false;
    let BB4 = false;
    if (AA2 == true) {
        if ($("#LBT2R").val("OK")) {
            BB2 = true;
        }
    }
    else {
        AA2 = true;
        BB2 = true;
    }
    if (AA4 == true) {
        if ($("#LBA2R").val("OK")) {
            BB4 = true;
        }
    }
    else {
        AA4 = true;
        BB4 = true;
    }
    if ((AA2 == true) && (BB2 == true) && (AA4 == true) && (BB4 == true)) {
        let Label1 = $("#Label1").val();
        $("#BackGround").css("display", "block");
        let BaseParameter = new Object();
        BaseParameter = {
            SearchString: Label1,
        }
        BaseParameter.USER_ID = GetCookieValue("UserID");
        BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        let url = "/C05_ER_L/Button2_Click";

        fetch(url, {
            method: "POST",
            body: formUpload,
            headers: {
            }
        }).then((response) => {
            response.json().then((data) => {
                localStorage.setItem("C05_START_EXIT", false);             
                C11_ERROR_FormClosed();
                $("#BackGround").css("display", "none");
            }).catch((err) => {
                $("#BackGround").css("display", "none");
            })
        });
    }
    else {
        alert("Please Check Again.");
        localStorage.setItem("C05_START_EXIT", true)
    }
}
function C11_1TimerStartInterval() {
    let C11_1Timer = setInterval(function () {
        let C11_1_Close = localStorage.getItem("C11_1_Close");
        if (C11_1_Close == "1") {
            clearInterval(C11_1Timer);
            C11_1_Close = 0;
            localStorage.setItem("C11_1_Close", C11_1_Close);
        }
    }, 100);
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
    localStorage.setItem("C05_START_C_EXIT", true);
    C11_ERROR_FormClosed();
}
function C11_ERROR_FormClosed() {   
    let MCbox = localStorage.getItem("C11_MCbox");
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
    let url = "/C05_ER_L/C11_ERROR_FormClosed";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            $("#BackGround").css("display", "none");
            localStorage.setItem("SettingsNON_OPER_CHK", false);
            localStorage.setItem("C05_ER_L_Close", 1);
            window.close();
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });  
}
