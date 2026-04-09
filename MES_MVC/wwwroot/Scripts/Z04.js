let IsTableSort = false;
let BaseResult = new Object();
let TagIndex = 1;
let Now;
let Z04_1IntervalTimer;
let Z04_ADMINIntervalTimer;
let DataGridView1RowIndex = 0;
let DataGridView2RowIndex = 0;
let DataGridView3RowIndex = 0;
let DataGridView4RowIndex = 0;
let DataGridView5RowIndex = 0;
let DataGridView6RowIndex = 0;
function getCurrentUser() {
    return GetCookieValue('UserID') || 'SYSTEM';
}

$(document).ready(function () {
    var now = new Date();
    var day = ("0" + now.getDate()).slice(-2);
    var month = ("0" + (now.getMonth() + 1)).slice(-2);
    var today = now.getFullYear() + "-" + (month) + "-" + (day);
    Now = today;
    $("#T1_CB01").val("");
    $("#T1_CB01").focus();
    document.getElementById("RadioButton1").checked = true;
    $("#Label24").val("-");
    $("#Label27").val("-");
    CB_DATASET();
    BaseResult.DataGridView1 = new Object();
    BaseResult.DataGridView1 = [];
});

$("#ATag001").click(function (e) {
    TagIndex = 1;
});
$("#ATag002").click(function (e) {
    TagIndex = 2;
});
$("#ATag003").click(function (e) {
    TagIndex = 3;
    document.getElementById("RadioButton3").checked = true;
    $("#TextBox15").focus();
});
$("#ATag004").click(function (e) {
    TagIndex = 4;
    document.getElementById("CheckBox1").checked = true;
    $("#TextBox19").focus();
});
$("#ATag005").click(function (e) {
    TagIndex = 5;
    document.getElementById("ComboBox1").selectedIndex = "0";
    $("#T5_CB01").focus();
});

$("#TextBox8").keydown(function (e) {
    if (e.keyCode == 13) {
        TextBox8_KeyDown();
    }
});
function TextBox8_KeyDown() {
    let RadioButton1 = document.getElementById("RadioButton1").checked;
    let RadioButton2 = document.getElementById("RadioButton2").checked;

    if (RadioButton1 == true || RadioButton2 == true) {
        PART_LIST(function () {
            if (RadioButton1 == true) {
                if (BaseResult.DataGridView2 && BaseResult.DataGridView2.length >= 1) {
                    // Focus vào ô QTY đầu tiên của DataGridView2 — giống VB focus vào Cells(2)
                    let firstQTY = document.getElementById("DataGridView2QTY0");
                    if (firstQTY) firstQTY.focus();
                }
            }
            if (RadioButton2 == true) {
                if (!BaseResult.DataGridView2 || BaseResult.DataGridView2.length <= 0) {
                    alert("PART NO 없음..... PART NO Một lỗi đã xảy ra.");
                    return;
                }
                // Tìm LAST_NO từ DataGridView1 — giống VB: Rows(Rows.Count-1).Cells("DGV1_03").Value + 1
                try {
                    let LAST_NO = BaseResult.DataGridView1[BaseResult.DataGridView1.length - 1].TSYEAR_INV_SERIALNO;
                    LAST_NO = Number(LAST_NO) + 1;
                    $("#TextBox16").val(LAST_NO);
                } catch (ex) {
                    $("#TextBox16").val(Number($("#TextBox16").val()) + 1);
                }
                $("#TextBox16").focus();
            }
        });
    }
}

function PART_LIST(callback) {
    let AAA = $("#TextBox8").val();
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        Action: TagIndex,
        SearchString: AAA,
    };
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/Z04/PART_LIST";
    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {}
    }).then((response) => {
        response.json().then((data) => {
            let BaseResultPART_LIST = data;
            BaseResult.DataGridView2 = BaseResultPART_LIST.DataGridView2;
            DataGridView2Render();
            $("#BackGround").css("display", "none");
            if (typeof callback === "function") callback();
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        });
    });
}

// FIX: TextBox16_KeyDown — thêm $ bị thiếu, logic đúng theo VB
$("#TextBox16").keydown(function (e) {
    if (e.keyCode == 13) {
        TextBox16_KeyDown();
    }
});
function TextBox16_KeyDown() {
    let RadioButton2 = document.getElementById("RadioButton2").checked;
    if (RadioButton2 == true) {
        if ($("#TextBox16").val() == "") {
            alert("Serial NO 오류가 발생 하였습니다. Serial NO Một lỗi đã xảy ra.");
            return;
        }

        // FIX: thêm $ bị thiếu
        let SE_NO1 = Number($("#TextBox5").val());
        let SE_NO2 = Number($("#TextBox6").val());
        let SE_NO = Number($("#TextBox16").val());

        if (SE_NO1 <= SE_NO && SE_NO <= SE_NO2) {
            // Focus vào ô QTY đầu tiên của DataGridView2 — giống VB: Cells(2)
            if (BaseResult.DataGridView2 && BaseResult.DataGridView2.length >= 1) {
                let firstQTY = document.getElementById("DataGridView2QTY0");
                if (firstQTY) firstQTY.focus();
            }
        } else {
            alert("Serial NO 오류가 발생 하였습니다. Serial NO Một lỗi đã xảy ra.");
        }
    }
}

// Chỉ cho nhập số vào TextBox16 — giống VB TextBox16_KeyPress
$("#TextBox16").keypress(function (e) {
    if (!((e.keyCode >= 48 && e.keyCode <= 57) || e.keyCode == 8 || e.keyCode == 46)) {
        e.preventDefault();
    }
});

$("#RadioButton1").click(function () {
    RadioButton1_CheckedChanged();
});
$("#RadioButton2").click(function () {
    RadioButton1_CheckedChanged();
});
function RadioButton1_CheckedChanged() {
    let RadioButton1 = document.getElementById("RadioButton1").checked;
    if (RadioButton1 == true) {
        document.getElementById("TextBox16").readOnly = true;
        $("#TextBox16").val("");
    }
    let RadioButton2 = document.getElementById("RadioButton2").checked;
    if (RadioButton2 == true) {
        document.getElementById("TextBox16").readOnly = false;
        $("#TextBox16").val("");
    }
}

$("#Button2").click(function () {
    Button2_Click();
});
function Button2_Click() {
    let url = "/Z04_ADMIN";
    window.location.href = url;
}

$("#T1_CB01").change(function () {
    T1_CB01_SelectedIndexChanged();
});
function T1_CB01_SelectedIndexChanged() {
    Buttonfind_Click();
}
$("#T2_CB01").change(function () {
    T2_CB01_SelectedIndexChanged();
});
function T2_CB01_SelectedIndexChanged() {
    Buttonfind_Click();
}

$("#Button1").click(function () {
    Button1_Click();
});
function Button1_Click() {
    let url = "/Z04_1";
    OpenWindowByURL(url, 200, 200);
    localStorage.setItem("Z04_1_Close", 0);
    Z04_1StartInterval();
}

$("#RadioButton3").click(function () {
    RadioButton3_CheckedChanged();
});
$("#RadioButton4").click(function () {
    RadioButton3_CheckedChanged();
});
function RadioButton3_CheckedChanged() {
    let RadioButton3 = document.getElementById("RadioButton3").checked;
    if (RadioButton3 == true) {
        $("#Label18").val("MES NO : ");
        document.getElementById("TextBox14").readOnly = true;
        document.getElementById("TextBox13").readOnly = true;
        $("#TextBox15").focus();
    }
    let RadioButton4 = document.getElementById("RadioButton4").checked;
    if (RadioButton4 == true) {
        $("#Label18").val("Department : ");
        document.getElementById("TextBox14").readOnly = false;
        document.getElementById("TextBox13").readOnly = false;
        $("#TextBox15").focus();
    }
}

$("#TextBox15").keydown(function (e) {
    if (e.keyCode == 13) {
        TextBox15_KeyDown();
    }
});
function TextBox15_KeyDown() {
    Buttonfind_Click();
}

$("#Buttonfind").click(function () { Buttonfind_Click(); });
$("#Buttonadd").click(function () { Buttonadd_Click(); });
$("#Buttonsave").click(function () { Buttonsave_Click(); });
$("#Buttondelete").click(function () { Buttondelete_Click(); });
$("#Buttoncancel").click(function () { Buttoncancel_Click(); });
$("#Buttoninport").click(function () { Buttoninport_Click(); });
$("#Buttonexport").click(function () { Buttonexport_Click(); });
$("#Buttonprint").click(function () { Buttonprint_Click(); });
$("#Buttonhelp").click(function () { Buttonhelp_Click(); });
$("#Buttonclose").click(function () { Buttonclose_Click(); });

function Buttonfind_Click() {
    if (TagIndex == 1) {
        let AA = $("#T1_CB01").val();
        $("#BackGround").css("display", "block");
        let BaseParameter = new Object();
        BaseParameter = { Action: TagIndex, SearchString: AA };
        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));

        fetch("/Z04/Buttonfind_Click", {
            method: "POST", body: formUpload, headers: {}
        }).then((response) => {
            response.json().then((data) => {
                let BaseResultButtonfind = data;
                BaseResult.DGV_Z04_02 = BaseResultButtonfind.DGV_Z04_02;
                BaseResult.DGV_Z04_03 = BaseResultButtonfind.DGV_Z04_03;
                if (BaseResult.DGV_Z04_02 && BaseResult.DGV_Z04_02.length > 0) {
                    $("#TextBox2").val(BaseResult.DGV_Z04_02[0].TSYEAR_DEPART);
                    $("#TextBox3").val(BaseResult.DGV_Z04_02[0].TSYEAR_PKILOC);
                    $("#TextBox4").val(BaseResult.DGV_Z04_02[0].TSYEAR_INPUTER);
                    $("#TextBox5").val(BaseResult.DGV_Z04_02[0].TSYEAR_SERIAL_NO1);
                    $("#TextBox6").val(BaseResult.DGV_Z04_02[0].TSYEAR_SERIAL_NO2);
                    $("#Label24").val(BaseResult.DGV_Z04_02[0].TSYEAR_YEAR);
                    $("#Label27").val(BaseResult.DGV_Z04_02[0].TSYEAR_MESNO);
                }
                BaseResult.DataGridView1 = BaseResult.DGV_Z04_03;
                DataGridView1Render();
                $("#BackGround").css("display", "none");
            }).catch((err) => {
                $("#BackGround").css("display", "none");
            });
        });
        $("#TextBox8").focus();
    }
    if (TagIndex == 2) {
        let AA = $("#T2_CB01").val();
        $("#BackGround").css("display", "block");
        let BaseParameter = new Object();
        BaseParameter = { Action: TagIndex, SearchString: AA };
        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));

        fetch("/Z04/Buttonfind_Click", {
            method: "POST", body: formUpload, headers: {}
        }).then((response) => {
            response.json().then((data) => {
                let BaseResultButtonfind = data;
                $("#TextBox12").val("");
                $("#TextBox10").val("");
                $("#TextBox11").val("");
                $("#TextBox7").val("");
                BaseResult.DataGridView3 = BaseResultButtonfind.DataGridView3;
                DataGridView3Render();
                $("#BackGround").css("display", "none");
            }).catch((err) => {
                $("#BackGround").css("display", "none");
            });
        });
        $("#TextBox8").focus();
    }
    if (TagIndex == 3) {
        let SS11 = $("#TextBox14").val();
        let SS22 = $("#TextBox13").val();
        $("#BackGround").css("display", "block");
        let BaseParameter = new Object();
        BaseParameter = { Action: TagIndex, ListSearchString: [] };
        BaseParameter.RadioButton3 = document.getElementById("RadioButton3").checked;
        BaseParameter.RadioButton4 = document.getElementById("RadioButton4").checked;
        BaseParameter.ListSearchString.push(SS11);
        BaseParameter.ListSearchString.push(SS22);
        BaseParameter.ListSearchString.push($("#TextBox15").val());
        BaseParameter.ListSearchString.push($("#T3_CB01").val());
        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));

        fetch("/Z04/Buttonfind_Click", {
            method: "POST", body: formUpload, headers: {}
        }).then((response) => {
            response.json().then((data) => {
                BaseResult.DataGridView4 = data.DataGridView4;
                DataGridView4Render();
                $("#BackGround").css("display", "none");
            }).catch((err) => {
                $("#BackGround").css("display", "none");
            });
        });
        $("#TextBox8").focus();
    }
    if (TagIndex == 4) {
        let T4_CB01 = $("#T4_CB01").val();
        let TextBox19 = $("#TextBox19").val();
        $("#BackGround").css("display", "block");
        let BaseParameter = new Object();
        BaseParameter = { Action: TagIndex, ListSearchString: [] };
        BaseParameter.CheckBox1 = document.getElementById("CheckBox1").checked;
        BaseParameter.ListSearchString.push(T4_CB01);
        BaseParameter.ListSearchString.push(TextBox19);
        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));

        fetch("/Z04/Buttonfind_Click", {
            method: "POST", body: formUpload, headers: {}
        }).then((response) => {
            response.json().then((data) => {
                BaseResult.DataGridView5 = data.DataGridView5;
                DataGridView5Render();
                $("#BackGround").css("display", "none");
            }).catch((err) => {
                $("#BackGround").css("display", "none");
            });
        });
        $("#TextBox8").focus();
    }
    if (TagIndex == 5) {
        let TextBox1 = $("#TextBox1").val();
        let ComboBox1 = $("#ComboBox1").val();
        let T5_CB01 = $("#T5_CB01").val();
        $("#BackGround").css("display", "block");
        let BaseParameter = new Object();
        BaseParameter = { Action: TagIndex, ListSearchString: [] };
        BaseParameter.ListSearchString.push(TextBox1);
        BaseParameter.ListSearchString.push(ComboBox1);
        BaseParameter.ListSearchString.push(T5_CB01);
        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));

        fetch("/Z04/Buttonfind_Click", {
            method: "POST", body: formUpload, headers: {}
        }).then((response) => {
            response.json().then((data) => {
                BaseResult.DataGridView6 = data.DataGridView6;
                DataGridView6Render();
                let SNO_CNT = 0;
                let INDT_CNT = 0;
                let CNT_RAT = 0;
                if (BaseResult.DataGridView6 && BaseResult.DataGridView6.length > 0) {
                    for (let i = 0; i < BaseResult.DataGridView6.length; i++) {
                        SNO_CNT = SNO_CNT + Number(BaseResult.DataGridView6[i].COUNT);
                        INDT_CNT = INDT_CNT + Number(BaseResult.DataGridView6[i].INPUT_DATA);
                    }
                }
                if (SNO_CNT != 0) {
                    CNT_RAT = (INDT_CNT / SNO_CNT) * 100;
                }
                let Label26 = "SERIAL NO Count ( " + SNO_CNT.toLocaleString() + " )  /  Input Data Count ( " + INDT_CNT.toLocaleString() + " )   " + CNT_RAT.toFixed(0) + " %";
                $("#Label26").val(Label26);
                $("#BackGround").css("display", "none");
            }).catch((err) => {
                $("#BackGround").css("display", "none");
            });
        });
        $("#TextBox8").focus();
    }
}

function Buttonadd_Click() {

}

function Buttonsave_Click() {
    if (TagIndex == 1) {
        if (BaseResult.DataGridView1) {
            let IsSave = true;
            if (BaseResult.DataGridView1.length <= 0) IsSave = false;
            let TextBox5 = $("#TextBox5").val();
            if (TextBox5 == "") IsSave = false;
            if (IsSave == true) {
                let AA = $("#Label24").val();
                let CC = $("#TextBox2").val();
                let DD = $("#TextBox3").val();
                let GG = $("#Label27").val();

                $("#BackGround").css("display", "block");
                let BaseParameter = new Object();
                BaseParameter = { Action: TagIndex, ListSearchString: [] };
                BaseParameter.USER_ID = getCurrentUser();
                BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
                BaseParameter.DataGridView1 = BaseResult.DataGridView1;
                BaseParameter.ListSearchString.push(TextBox5);
                BaseParameter.ListSearchString.push(AA);
                BaseParameter.ListSearchString.push(CC);
                BaseParameter.ListSearchString.push(DD);
                BaseParameter.ListSearchString.push(GG);
                let formUpload = new FormData();
                formUpload.append('BaseParameter', JSON.stringify(BaseParameter));

                fetch("/Z04/Buttonsave_Click", {
                    method: "POST", body: formUpload, headers: {}
                }).then((response) => {
                    response.json().then((data) => {
                        alert(localStorage.getItem("SaveSuccess"));
                        Buttonfind_Click();
                        $("#BackGround").css("display", "none");
                    }).catch((err) => {
                        Buttonfind_Click();
                        $("#BackGround").css("display", "none");
                    });
                });
            }
        }
    }
    if (TagIndex == 2) {
        if (BaseResult.DataGridView1) {
            let IsSave = true;
            let TextBox12 = $("#TextBox12").val();
            if (TextBox12 == "") IsSave = false;
            if (IsSave == true) {
                let AAA = $("#T2_CB01").val();
                let BBB = $("#TextBox12").val();
                let CCC = $("#TextBox18").val();
                let DDD = $("#TextBox11").val();
                let EEE = $("#TextBox17").val();
                let FFF = $("#TextBox7").val();

                $("#BackGround").css("display", "block");
                let BaseParameter = new Object();
                BaseParameter = { Action: TagIndex, ListSearchString: [] };
                BaseParameter.USER_ID = getCurrentUser();
                BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
                BaseParameter.ListSearchString.push(AAA);
                BaseParameter.ListSearchString.push(BBB);
                BaseParameter.ListSearchString.push(CCC);
                BaseParameter.ListSearchString.push(DDD);
                BaseParameter.ListSearchString.push(EEE);
                BaseParameter.ListSearchString.push(FFF);
                let formUpload = new FormData();
                formUpload.append('BaseParameter', JSON.stringify(BaseParameter));

                fetch("/Z04/Buttonsave_Click", {
                    method: "POST", body: formUpload, headers: {}
                }).then((response) => {
                    response.json().then((data) => {
                        alert(localStorage.getItem("SaveSuccess"));
                        Buttonfind_Click();
                        $("#BackGround").css("display", "none");
                    }).catch((err) => {
                        Buttonfind_Click();
                        $("#BackGround").css("display", "none");
                    });
                });
            }
        }
    }
}

function Buttondelete_Click() {
    if (TagIndex == 1) {
        let DGV_II = BaseResult.DataGridView1.length - 1;
        if (DGV_II > -1) {
            BaseResult.DataGridView1.splice(DGV_II, 1);
            DataGridView1Render();
        }
    }
    if (TagIndex == 2) {
        let IsDelete = true;
        let TextBox12 = $("#TextBox12").val();
        if (TextBox12 == "") IsDelete = false;
        if (IsDelete == true) {
            if (confirm(localStorage.getItem("DeleteConfirm"))) {
                let BBB = $("#TextBox12").val();
                let T2_CB01 = $("#T2_CB01").val();
                let Label29 = $("#Label29").val();
                $("#BackGround").css("display", "block");
                let BaseParameter = new Object();
                BaseParameter = { Action: TagIndex, ListSearchString: [] };
                BaseParameter.ListSearchString.push(BBB);
                BaseParameter.ListSearchString.push(T2_CB01);
                BaseParameter.ListSearchString.push(Label29);
                let formUpload = new FormData();
                formUpload.append('BaseParameter', JSON.stringify(BaseParameter));

                fetch("/Z04/Buttondelete_Click", {
                    method: "POST", body: formUpload, headers: {}
                }).then((response) => {
                    response.json().then((data) => {
                        alert(localStorage.getItem("SaveSuccess"));
                        Buttonfind_Click();
                        $("#BackGround").css("display", "none");
                    }).catch((err) => {
                        Buttonfind_Click();
                        $("#BackGround").css("display", "none");
                    });
                });
            }
        }
    }
}

function Buttoncancel_Click() {
    if (TagIndex == 1) {
        let IsCancel = true;
        let TextBox5Number = Number($("#TextBox5").val());
        if (TextBox5Number == 0) IsCancel = false;

        if (IsCancel == true) {
            if (BaseResult && BaseResult.DataGridView1) {
                let DGV1_CNT = BaseResult.DataGridView1.length - 1;
                let DGV1_SNO = 0;
                let SNO_MIN = 0;
                let SNO_MAX = 0;
                let RadioButton1 = document.getElementById("RadioButton1").checked;
                let RadioButton2 = document.getElementById("RadioButton2").checked;

                if (RadioButton1 == true) {
                    if (DGV1_CNT > -1) {
                        DGV1_SNO = Number(BaseResult.DataGridView1[DGV1_CNT].TSYEAR_INV_SERIALNO) + 1;
                        SNO_MIN = Number($("#TextBox5").val());
                        SNO_MAX = Number($("#TextBox6").val());
                    } else {
                        DGV1_SNO = Number($("#TextBox5").val());
                        SNO_MIN = Number($("#TextBox5").val());
                        SNO_MAX = Number($("#TextBox6").val());
                    }
                    if (DGV1_SNO > SNO_MAX) {
                        alert("시리얼 번호 오류가 발생 하였습니다. Serial number Một lỗi đã xảy ra.");
                        return;
                    }
                }

                if (RadioButton2 == true) {
                    DGV1_SNO = Number($("#TextBox16").val());
                    SNO_MIN = Number($("#TextBox5").val());
                    SNO_MAX = Number($("#TextBox6").val());
                    if (DGV1_SNO < SNO_MIN) {
                        alert("시리얼 번호 오류가 발생 하였습니다. Serial number Một lỗi đã xảy ra.");
                        return;
                    }
                    if (DGV1_SNO > SNO_MAX) {
                        alert("시리얼 번호 오류가 발생 하였습니다. Serial number Một lỗi đã xảy ra.");
                        return;
                    }
                    if (Number($("#TextBox16").val()) == 0) {
                        alert("시리얼 번호 오류가 발생 하였습니다. Serial number Một lỗi đã xảy ra.");
                        return;
                    }
                }

                // Kiểm tra trùng Serial NO
                for (let D1_II = 0; D1_II < BaseResult.DataGridView1.length; D1_II++) {
                    if (DGV1_SNO == BaseResult.DataGridView1[D1_II].TSYEAR_INV_SERIALNO) {
                        alert("시리얼 번호 오류가 발생 하였습니다. Serial number Một lỗi đã xảy ra.");
                        return;
                    }
                }

                let DGV_A10 = "";
                if (RadioButton1 == true) DGV_A10 = "A";
                if (RadioButton2 == true) DGV_A10 = "M";

                let DataGridView1Item = {
                    NO: "New",
                    TSYEAR_INV_PKILOC: $("#TextBox3").val(),
                    TSYEAR_INV_SERIALNO: DGV1_SNO,
                    PART_NO: "Cancel",
                    TSYEAR_INV_QTY: 0,
                    PART_NM: $("#TextBox2").val(),
                    TSYEAR_INV_PART_IDX: 0,
                    Name: GetCookieValue('UserID') || 'SYSTEM',  // Username
                    TSYEAR_INV_ANM: DGV_A10,                     // A/M
                    TSYEAR_INV_PART_TNM: "Cancel",
                    TSYEAR_INV_DJGLOC: "Cancel",
                };
                BaseResult.DataGridView1.push(DataGridView1Item);
                DataGridView1Render();
                DataGridView1RowIndex = BaseResult.DataGridView1.length - 1;
                $("#TextBox8").val("");
                $("#TextBox8").focus();
            }
        }
    }
}

function Buttoninport_Click() {

}

function Buttonexport_Click() {
    if (TagIndex == 3) {
        TableHTMLToExcel("DataGridView4Table", "Z04_Review", "Z04_Review");
    }
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

function DataGridView1Render() {
    let HTML = "";
    if (BaseResult && BaseResult.DataGridView1 && BaseResult.DataGridView1.length > 0) {
        DataGridView1_SelectionChanged(0);
        for (let i = 0; i < BaseResult.DataGridView1.length; i++) {
            HTML += "<tr onclick='DataGridView1_SelectionChanged(" + i + ")'>";
            HTML += "<td>" + BaseResult.DataGridView1[i].NO + "</td>";
            HTML += "<td>" + BaseResult.DataGridView1[i].TSYEAR_INV_PKILOC + "</td>";
            HTML += "<td>" + BaseResult.DataGridView1[i].TSYEAR_INV_SERIALNO + "</td>";
            HTML += "<td>" + BaseResult.DataGridView1[i].PART_NO + "</td>";
            HTML += "<td>" + BaseResult.DataGridView1[i].TSYEAR_INV_QTY + "</td>";
            HTML += "<td>" + BaseResult.DataGridView1[i].TSYEAR_INV_DEPART + "</td>";
            HTML += "<td>" + BaseResult.DataGridView1[i].PART_NM + "</td>";
            HTML += "<td>" + BaseResult.DataGridView1[i].Name + "</td>";
            HTML += "<td>" + BaseResult.DataGridView1[i].TSYEAR_INV_ANM + "</td>";
            HTML += "<td>" + BaseResult.DataGridView1[i].TSYEAR_INV_DJGLOC + "</td>";
            HTML += "</tr>";
        }
    }
    document.getElementById("DataGridView1").innerHTML = HTML;
}

function DataGridView1Sort() {
    IsTableSort = !IsTableSort;
    DataGridViewSort(BaseResult.DataGridView1, IsTableSort);
    DataGridView1Render();
}

function DataGridView1_SelectionChanged(i) {
    DataGridView1RowIndex = i;
}

function DataGridView2Render() {
    let HTML = "";
    if (BaseResult && BaseResult.DataGridView2 && BaseResult.DataGridView2.length > 0) {
        DataGridView2_SelectionChanged(0);
        for (let i = 0; i < BaseResult.DataGridView2.length; i++) {
            HTML += "<tr onclick='DataGridView2_SelectionChanged(" + i + ")'>";
            HTML += "<td>" + BaseResult.DataGridView2[i].PART_NO + "</td>";
            HTML += "<td>" + BaseResult.DataGridView2[i].PART_NM + "</td>";
            HTML += "<td><input onblur='DataGridView2QTYChange(" + i + ", this)' id='DataGridView2QTY" + i + "' type='number' class='form-control' value='" + BaseResult.DataGridView2[i].QTY + "' style='width: 100px;'></td>";
            HTML += "<td><input onblur='DataGridView2DJG_LOCChange(" + i + ", this)' id='DataGridView2DJG_LOC" + i + "' type='text' class='form-control' value='" + BaseResult.DataGridView2[i].DJG_LOC + "' style='width: 100px;'></td>";
            HTML += "<td onclick='DataGridView2_CellClick(" + i + ")'><button id='DataGridView2CHK" + i + "' class='btn waves-effect waves-light grey darken-1'> >>> </button></td>";
            HTML += "</tr>";
        }
    }
    document.getElementById("DataGridView2").innerHTML = HTML;
}

function DataGridView2Sort() {
    IsTableSort = !IsTableSort;
    DataGridViewSort(BaseResult.DataGridView2, IsTableSort);
    DataGridView2Render();
}

function DataGridView2_SelectionChanged(i) {
    DataGridView2RowIndex = i;
}

function DataGridView2QTYChange(i, input) {
    DataGridView2RowIndex = i;
    BaseResult.DataGridView2[DataGridView2RowIndex].QTY = input.value;
    // Sau khi nhập QTY xong nhấn Enter/Tab — giống VB DataGridView2_CellEndEdit focus lên trên
    // rồi focus sang DJG_LOC
    let djgLoc = document.getElementById("DataGridView2DJG_LOC" + i);
    if (djgLoc) djgLoc.focus();
}

function DataGridView2DJG_LOCChange(i, input) {
    DataGridView2RowIndex = i;
    BaseResult.DataGridView2[DataGridView2RowIndex].DJG_LOC = input.value;
}
$("#DataGridView2").on("keydown", "input[id^='DataGridView2DJG_LOC']", function (e) {
    if (e.keyCode == 13) {
        let i = parseInt($(this).attr("id").replace("DataGridView2DJG_LOC", ""));
        DataGridView2RowIndex = i;
        ADD_PART();
    }
});
$("#DataGridView2").on("keydown", "input[id^='DataGridView2QTY']", function (e) {
    if (e.keyCode == 13) {
        e.preventDefault();
        let i = parseInt($(this).attr("id").replace("DataGridView2QTY", ""));
        let djgLoc = document.getElementById("DataGridView2DJG_LOC" + i);
        if (djgLoc) djgLoc.focus();
    }
});

function DataGridView2_CellClick(i) {
    DataGridView2RowIndex = i;
    let IsCheck = true;
    let TextBox5 = $("#TextBox5").val();
    if (TextBox5 == "") IsCheck = false;
    if (Number(TextBox5) == 0) IsCheck = false;
    if (IsCheck == true) {
        ADD_PART();
    }
}

// FIX: ADD_PART — sửa logic RadioButton1 thiếu code push item vào DataGridView1
function ADD_PART() {
    let IsCheck = true;
    let DGV1_CNT = BaseResult.DataGridView1.length - 1;
    let DGV1_SNO = 0;
    let SNO_MIN = 0;
    let SNO_MAX = 0;
    let RadioButton1 = document.getElementById("RadioButton1").checked;
    let RadioButton2 = document.getElementById("RadioButton2").checked;

    if (RadioButton1 == true) {
        if (DGV1_CNT > -1) {
            DGV1_SNO = Number(BaseResult.DataGridView1[DGV1_CNT].TSYEAR_INV_SERIALNO) + 1;
        } else {
            DGV1_SNO = Number($("#TextBox5").val());
        }
        SNO_MIN = Number($("#TextBox5").val());
        SNO_MAX = Number($("#TextBox6").val());

        if (Number($("#TextBox5").val()) == 0) {
            alert("MES NO 검색 오류 발생 하였습니다. MES NO & Serial number Một lỗi đã xảy ra.");
            IsCheck = false;
        }
        if (DGV1_SNO > SNO_MAX) {
            alert("시리얼 번호 오류가 발생 하였습니다. Serial number Một lỗi đã xảy ra.");
            IsCheck = false;
        }
    }

    if (RadioButton2 == true) {
        DGV1_SNO = Number($("#TextBox16").val());
        SNO_MIN = Number($("#TextBox5").val());
        SNO_MAX = Number($("#TextBox6").val());

        if (Number($("#TextBox5").val()) == 0) {
            alert("MES NO 검색 오류 발생 하였습니다. MES NO & Serial number Một lỗi đã xảy ra.");
            IsCheck = false;
        }
        if (DGV1_SNO > SNO_MAX) {
            alert("최대 시리얼 번호 오류가 발생 하였습니다. MAX Over Serial number Một lỗi đã xảy ra.");
            IsCheck = false;
        }
        if (DGV1_SNO < SNO_MIN) {
            alert("최소 시리얼 번호 오류가 발생 하였습니다. MIN Below Serial number Một lỗi đã xảy ra.");
            IsCheck = false;
        }
    }

    if (!IsCheck) return;

    let DGV_II = DataGridView2RowIndex;
    let ERR_TEXT = BaseResult.DataGridView2[DGV_II].QTY;
    if (ERR_TEXT == "" || ERR_TEXT == null) {
        alert("재고 수량 오류가 발생 하였습니다. Số lượng tồn. Một lỗi đã xảy ra.");
        return;
    }

    let DGV_A02 = $("#TextBox3").val();
    let DGV_A03 = 0;
    let DGV_A04 = BaseResult.DataGridView2[DGV_II].PART_NO;
    let DGV_A05 = BaseResult.DataGridView2[DGV_II].QTY;
    let DGV_A06 = $("#TextBox2").val();
    let DGV_A07 = BaseResult.DataGridView2[DGV_II].PART_NM;
    let DGV_A08 = BaseResult.DataGridView2[DGV_II].PART_IDX;
    let DGV_A09 = GetCookieValue("UserID");
    let DGV_A10 = "";
    let DGV_A11 = BaseResult.DataGridView2[DGV_II].TNM;
    let DGV_A12 = BaseResult.DataGridView2[DGV_II].DJG_LOC;

    if (RadioButton1 == true) {
        DGV_A03 = DGV1_SNO;
        DGV_A10 = "A";
    }
    if (RadioButton2 == true) {
        DGV_A03 = Number($("#TextBox16").val());
        DGV_A10 = "M";
    }

    // Kiểm tra trùng Serial NO
    for (let D1_II = 0; D1_II < BaseResult.DataGridView1.length; D1_II++) {
        if (DGV_A03 == BaseResult.DataGridView1[D1_II].TSYEAR_INV_SERIALNO) {
            alert("Serial NO 중복 오류가 발생 하였습니다. Serial NO Một lỗi đã xảy ra.");
            return;
        }
    }

    // FIX: RadioButton1 và RadioButton2 đều push item vào DataGridView1
    let DataGridView1Item = {
        NO: "New",
        TSYEAR_INV_PKILOC: DGV_A02,
        TSYEAR_INV_SERIALNO: DGV_A03,
        PART_NO: DGV_A04,
        TSYEAR_INV_QTY: DGV_A05,
        TSYEAR_INV_DEPART: DGV_A06,
        PART_NM: DGV_A07,
        TSYEAR_INV_PART_IDX: DGV_A08,
        Name: DGV_A09,                  // Username để hiển thị
        TSYEAR_INV_ANM: DGV_A10,        // A/M
        TSYEAR_INV_PART_TNM: DGV_A11,
        TSYEAR_INV_DJGLOC: DGV_A12,
    };
    BaseResult.DataGridView1.push(DataGridView1Item);
    DataGridView1Render();
    DataGridView1RowIndex = BaseResult.DataGridView1.length - 1;
    DataGridView1_SelectionChanged(DataGridView1RowIndex);

    $("#TextBox8").val("");
    $("#TextBox8").focus();
}

function DataGridView3Render() {
    let HTML = "";
    if (BaseResult && BaseResult.DataGridView3 && BaseResult.DataGridView3.length > 0) {
        DataGridView3_SelectionChanged(0);
        for (let i = 0; i < BaseResult.DataGridView3.length; i++) {
            HTML += "<tr onclick='DataGridView3_SelectionChanged(" + i + ")'>";
            HTML += "<td>" + BaseResult.DataGridView3[i].NO + "</td>";
            HTML += "<td>" + BaseResult.DataGridView3[i].TSYEAR_INV_PKILOC + "</td>";
            HTML += "<td>" + BaseResult.DataGridView3[i].TSYEAR_INV_SERIALNO + "</td>";
            HTML += "<td>" + BaseResult.DataGridView3[i].PART_NO + "</td>";
            HTML += "<td>" + BaseResult.DataGridView3[i].TSYEAR_INV_QTY + "</td>";
            HTML += "<td>" + BaseResult.DataGridView3[i].TSYEAR_INV_DEPART + "</td>";
            HTML += "<td>" + BaseResult.DataGridView3[i].PART_NM + "</td>";
            HTML += "<td>" + BaseResult.DataGridView3[i].TSYEAR_INV_ANM + "</td>";
            HTML += "<td>" + BaseResult.DataGridView3[i].TSYEAR_INV_DJGLOC + "</td>";
            HTML += "<td>" + BaseResult.DataGridView3[i].Name + "</td>";
            HTML += "<td>" + BaseResult.DataGridView3[i].TSYEAR_INV_YEAR + "</td>";
            HTML += "</tr>";
        }
    }
    document.getElementById("DataGridView3").innerHTML = HTML;
}

function DataGridView3Sort() {
    IsTableSort = !IsTableSort;
    DataGridViewSort(BaseResult.DataGridView3, IsTableSort);
    DataGridView3Render();
}

function DataGridView3_SelectionChanged(i) {
    DataGridView3RowIndex = i;
    $("#TextBox10").val(BaseResult.DataGridView3[i].PART_NO);
    $("#TextBox11").val(BaseResult.DataGridView3[i].TSYEAR_INV_QTY);
    $("#TextBox12").val(BaseResult.DataGridView3[i].TSYEAR_INV_SERIALNO);
    $("#TextBox18").val(BaseResult.DataGridView3[i].TSYEAR_INV_PART_IDX);
    $("#TextBox17").val(BaseResult.DataGridView3[i].TSYEAR_INV_PART_TNM);
    $("#TextBox7").val(BaseResult.DataGridView3[i].TSYEAR_INV_DJGLOC);
    $("#Label29").val(BaseResult.DataGridView3[i].TSYEAR_INV_YEAR);
}

function DataGridView4Render() {
    let HTML = "";
    if (BaseResult && BaseResult.DataGridView4 && BaseResult.DataGridView4.length > 0) {
        DataGridView4_SelectionChanged(0);
        for (let i = 0; i < BaseResult.DataGridView4.length; i++) {
            HTML += "<tr onclick='DataGridView4_SelectionChanged(" + i + ")'>";
            HTML += "<td>" + BaseResult.DataGridView4[i].NO + "</td>";
            HTML += "<td>" + BaseResult.DataGridView4[i].TSYEAR_INV_PKILOC + "</td>";
            HTML += "<td>" + BaseResult.DataGridView4[i].TSYEAR_INV_SERIALNO + "</td>";
            HTML += "<td>" + BaseResult.DataGridView4[i].PART_NO + "</td>";
            HTML += "<td>" + BaseResult.DataGridView4[i].TSYEAR_INV_QTY + "</td>";
            HTML += "<td>" + BaseResult.DataGridView4[i].TSYEAR_INV_DEPART + "</td>";
            HTML += "<td>" + BaseResult.DataGridView4[i].TSYEAR_INV_DJGLOC + "</td>";
            HTML += "<td>" + BaseResult.DataGridView4[i].PART_NM + "</td>";
            HTML += "<td>" + BaseResult.DataGridView4[i].CreateBy + "</td>";
            HTML += "<td>" + BaseResult.DataGridView4[i].CreateTime + "</td>";
            HTML += "<td>" + BaseResult.DataGridView4[i].UpdateBy + "</td>";
            HTML += "<td>" + BaseResult.DataGridView4[i].UpdateTime + "</td>";
            HTML += "</tr>";
        }
    }
    document.getElementById("DataGridView4").innerHTML = HTML;
}

function DataGridView4Sort() {
    IsTableSort = !IsTableSort;
    DataGridViewSort(BaseResult.DataGridView4, IsTableSort);
    DataGridView4Render();
}

function DataGridView4_SelectionChanged(i) {
    DataGridView4RowIndex = i;
}

function DataGridView5Render() {
    let HTML = "";
    if (BaseResult && BaseResult.DataGridView5 && BaseResult.DataGridView5.length > 0) {
        DataGridView5_SelectionChanged(0);
        for (let i = 0; i < BaseResult.DataGridView5.length; i++) {
            HTML += "<tr onclick='DataGridView5_SelectionChanged(" + i + ")'>";
            HTML += "<td>" + BaseResult.DataGridView5[i].NO + "</td>";
            HTML += "<td>" + BaseResult.DataGridView5[i].PART_NO + "</td>";
            HTML += "<td>" + BaseResult.DataGridView5[i].PART_NM + "</td>";
            HTML += "<td>" + BaseResult.DataGridView5[i].TSYEAR_INV_SERIALNO + "</td>";
            HTML += "<td>" + BaseResult.DataGridView5[i].TSYEAR_INV_QTY + "</td>";
            HTML += "</tr>";
        }
    }
    document.getElementById("DataGridView5").innerHTML = HTML;
}

function DataGridView5Sort() {
    IsTableSort = !IsTableSort;
    DataGridViewSort(BaseResult.DataGridView5, IsTableSort);
    DataGridView5Render();
}

function DataGridView5_SelectionChanged(i) {
    DataGridView5RowIndex = i;
}

function DataGridView6Render() {
    let HTML = "";
    if (BaseResult && BaseResult.DataGridView6 && BaseResult.DataGridView6.length > 0) {
        DataGridView6_SelectionChanged(0);
        for (let i = 0; i < BaseResult.DataGridView6.length; i++) {
            HTML += "<tr onclick='DataGridView6_SelectionChanged(" + i + ")'>";
            HTML += "<td>" + BaseResult.DataGridView6[i].TSYEAR_YEAR + "</td>";
            HTML += "<td>" + BaseResult.DataGridView6[i].TSYEAR_MESNO + "</td>";
            HTML += "<td>" + BaseResult.DataGridView6[i].TSYEAR_DEPART + "</td>";
            HTML += "<td>" + BaseResult.DataGridView6[i].TSYEAR_PKILOC + "</td>";
            HTML += "<td>" + BaseResult.DataGridView6[i].TSYEAR_INPUTER + "</td>";
            HTML += "<td>" + BaseResult.DataGridView6[i].TSYEAR_SERIAL_NO1 + "</td>";
            HTML += "<td>" + BaseResult.DataGridView6[i].TSYEAR_SERIAL_NO2 + "</td>";
            HTML += "<td>" + BaseResult.DataGridView6[i].COUNT + "</td>";
            HTML += "<td>" + BaseResult.DataGridView6[i].INPUT_DATA + "</td>";
            HTML += "<td>" + BaseResult.DataGridView6[i].STATE + "</td>";
            HTML += "</tr>";
        }
    }
    document.getElementById("DataGridView6").innerHTML = HTML;
}

function DataGridView6Sort() {
    IsTableSort = !IsTableSort;
    DataGridViewSort(BaseResult.DataGridView6, IsTableSort);
    DataGridView6Render();
}

function DataGridView6_SelectionChanged(i) {
    DataGridView6RowIndex = i;
}

function CB_DATASET() {

}

function Z04_1StartInterval() {
    Z04_1IntervalTimer = setInterval(function () {
        let Z04_1_Close = localStorage.getItem("Z04_1_Close");
        if (Z04_1_Close == 1) {
            clearInterval(Z04_1IntervalTimer);
            Z04_1_Close = 0;
            localStorage.setItem("Z04_1_Close", Z04_1_Close);
            let BBB = localStorage.getItem("Z04_TextBox10");
            let AAA = localStorage.getItem("Z04_TextBox18");
            let DDD = localStorage.getItem("Z04_TextBox17");
            $("#TextBox10").val(BBB);
            $("#TextBox18").val(AAA);
            $("#TextBox17").val(DDD);
        }
    }, 100);
}

function Z04_ADMINStartInterval() {
    Z04_ADMINIntervalTimer = setInterval(function () {
        let Z04_ADMIN_Close = localStorage.getItem("Z04_ADMIN_Close");
        if (Z04_ADMIN_Close == 1) {
            clearInterval(Z04_ADMINIntervalTimer);
            Z04_ADMIN_Close = 0;
            localStorage.setItem("Z04_ADMIN_Close", Z04_ADMIN_Close);
        }
    }, 100);
}