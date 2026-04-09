let IsTableSort = false;
let BaseResult = new Object();
let TagIndex = 1;
let FAC_ST = "";
let T3_Timer1;
let T1_Timer1;
let T1_Timer2;
let T1_COUNT = 0;
let Now;

let DataGridView2RowIndex = 0;
let DataGridView3RowIndex = 0;
let DataGridView4RowIndex = 0;
let T2_DGV1RowIndex = 0;

$(document).ready(function () {
    var now = new Date();
    var day = ("0" + now.getDate()).slice(-2);
    var month = ("0" + (now.getMonth() + 1)).slice(-2);
    var today = now.getFullYear() + "-" + (month) + "-" + (day);
    Now = today;
    $("#DateTimePicker2").val(today);
    $("#T3_S2").val(today);
    $("#T2_S2").val(today);

    $("#LB_DT").val("-");
    $("#LB_STG").val("-");
    //$("#LB_STG").val(9.656.toFixed(2));
    CB_FCTRY_LINE();

    clearInterval(T3_Timer1);

    COMLIST_LINE_1();
    COMLIST_LINE_2();
    COMLIST_LINE_3();

});
function T3_Timer1Start() {
    clearInterval(T3_Timer1);
    T3_Timer1 = setInterval(function () {
        T3_Timer1_Tick();
    }, 30000);
}
function T1_Timer1Start() {
    clearInterval(T1_Timer1);
    T1_Timer1 = setInterval(function () {
        T1_Timer1_Tick();
    }, 10000);
}
function T1_Timer2Start() {
    clearInterval(T1_Timer2);
    T1_Timer2 = setInterval(function () {
        T1_Timer2_Tick();
    }, 1000);
}
function T3_Timer1_Tick() {
    if (TagIndex == 1) {
        Buttonfind_Click();
    }
}
function T1_Timer1_Tick() {
    if (BaseResult.DataGridView2.length > 0) {
        var audio = new Audio("/Media/Dinn_ding.wav");
        audio.play();
    }
}
function T1_Timer2_Tick() {
    if (TagIndex == 1) {
        if (T1_COUNT > 14) {
            T1_COUNT = 0;
            $("#Label21").val("TIME : 0");
            Buttonfind_Click();
        }
        else {
            T1_COUNT = T1_COUNT + 1;
            $("#Label21").val("TIME : " + T1_COUNT);
        }
    }
}

$("#ATag001").click(function (e) {
    TagIndex = 1;
    TabControl1_SelectedIndexChanged();
});
$("#ATag002").click(function (e) {
    TagIndex = 2;
    TabControl1_SelectedIndexChanged();
});
$("#ATag003").click(function (e) {
    TagIndex = 3;
    TabControl1_SelectedIndexChanged();
});
function TabControl1_SelectedIndexChanged() {
    if (TagIndex == 2) {

    }
    else {
        clearInterval(T3_Timer1);
    }
    if (TagIndex == 1) {

    }
    else {
        clearInterval(T1_Timer1);
        clearInterval(T1_Timer2);
    }
}
$("#CB_FCTRY1").change(function (e) {
    CB_FCTRY1_SelectedIndexChanged();
});
function CB_FCTRY1_SelectedIndexChanged() {
    let CB_FCTRY1 = $("#CB_FCTRY1").val();
    if (CB_FCTRY1 == "Factory 1") {
        FAC_ST = " AND RIGHT(`CD_SYS_NOTE`, 2)  <= 60 ";
    }
    else {
        FAC_ST = " AND RIGHT(`CD_SYS_NOTE`, 2)   > 60 ";
    }
}
$("#ComboBox2").change(function (e) {
    ComboBox2_SelectedIndexChanged();
});
function ComboBox2_SelectedIndexChanged() {
    Buttonfind_Click();
}
$("#DateTimePicker2").change(function (e) {
    DateTimePicker2_ValueChanged();
});
function DateTimePicker2_ValueChanged() {
    Buttonfind_Click();
}
$("#CB_FCTRY3").change(function (e) {
    CB_FCTRY3_SelectedIndexChanged();
});
function CB_FCTRY1_SelectedIndexChanged() {
    let CB_FCTRY3 = $("#CB_FCTRY3").val();
    if (CB_FCTRY3 == "Factory 1") {
        FAC_ST = " AND RIGHT(`CD_SYS_NOTE`, 2)  <= 60 ";
    }
    else {
        FAC_ST = " AND RIGHT(`CD_SYS_NOTE`, 2)   > 60 ";
    }
}
$("#T3_S3").keydown(function (e) {
    if (e.keyCode == 13) {
        T3_S3_KeyDown();
    }
});
function T3_S3_KeyDown() {
    Buttonfind_Click();
}
$("#T3_S4").keydown(function (e) {
    if (e.keyCode == 13) {
        T3_S4_KeyDown();
    }
});
function T3_S4_KeyDown() {
    Buttonfind_Click();
}
$("#RadioButton2").click(function () {
    RBchk0();
});
$("#RadioButton1").click(function () {
    RBchk0();
});
function RBchk0() {
    let RadioButton2 = document.getElementById("RadioButton2").checked;
    let RadioButton1 = document.getElementById("RadioButton1").checked;
    if (RadioButton2 == true) {
        let j = BaseResult.DataGridView3.length;
        if (j >= 9) {
            j = 9;
        }
        for (let i = 0; i < j; i++) {
            BaseResult.DataGridView3[i].CHK = true;
        }
        DataGridView3Render();
    }
    if (RadioButton1 == true) {
        let j = BaseResult.DataGridView3.length;
        if (j >= 9) {
            j = 9;
        }
        for (let i = 0; i < j; i++) {
            BaseResult.DataGridView3[i].CHK = false;
        }
        DataGridView3Render();
    }
}
$("#rbchk2").click(function () {
    RBchk();
});
$("#rbchk1").click(function () {
    RBchk();
});
function RBchk() {
    let rbchk1 = document.getElementById("rbchk1").checked;
    let rbchk2 = document.getElementById("rbchk2").checked;
    if (rbchk1 == true) {
        for (let i = 0; i < BaseResult.DataGridView4.length; i++) {
            BaseResult.DataGridView4[i].CHK = true;
        }
        DataGridView4Render();
    }
    if (rbchk2 == true) {
        for (let i = 0; i < BaseResult.DataGridView4.length; i++) {
            BaseResult.DataGridView4[i].CHK = false;
        }
        DataGridView4Render();
    }
}
$("#CB_FCTRY2").change(function (e) {
    CB_FCTRY2_SelectedIndexChanged();
});
function CB_FCTRY2_SelectedIndexChanged() {
    let CB_FCTRY2 = $("#CB_FCTRY2").val();
    if (CB_FCTRY2 == "Factory 1") {
        FAC_ST = " AND RIGHT(`CD_SYS_NOTE`, 2)  <= 60 ";
    }
    else {
        FAC_ST = " AND RIGHT(`CD_SYS_NOTE`, 2)   > 60 ";
    }
}
$("#T2_S4").keydown(function (e) {
    if (e.keyCode == 13) {
        T2_S4_KeyDown();
    }
});
function T2_S4_KeyDown() {
    Buttonfind_Click();
}
$("#T2_S3").keydown(function (e) {
    if (e.keyCode == 13) {
        T2_S3_KeyDown();
    }
});
function T2_S3_KeyDown() {
    Buttonfind_Click();
}
$("#T2_S5").keydown(function (e) {
    if (e.keyCode == 13) {
        T2_S5_KeyDown();
    }
});
function T2_S5_KeyDown() {
    Buttonfind_Click();
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
        $("#BackGround").css("display", "block");
        let BaseParameter = new Object();
        BaseParameter = {
            Action: TagIndex,
            ListSearchString: [],
        }
        BaseParameter.ListSearchString.push($("#CB_FCTRY1").val());
        BaseParameter.ListSearchString.push($("#ComboBox1").val());
        BaseParameter.ListSearchString.push($("#ComboBox2").val());
        BaseParameter.ListSearchString.push($("#DateTimePicker2").val());
        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        let url = "/B10/Buttonfind_Click";

        fetch(url, {
            method: "POST",
            body: formUpload,
            headers: {
            }
        }).then((response) => {
            response.json().then((data) => {
                let BaseResultButtonfind = data;
                BaseResult.DataGridView2 = BaseResultButtonfind.DataGridView2;
                DataGridView2Render();
                T1_Timer1Start();
                T1_Timer2Start();
                $("#BackGround").css("display", "none");
            }).catch((err) => {
                $("#BackGround").css("display", "none");
            })
        });
    }
    if (TagIndex == 2) {
        $("#BackGround").css("display", "block");
        let BaseParameter = new Object();
        BaseParameter = {
            Action: TagIndex,
            ListSearchString: [],
        }
        BaseParameter.ListSearchString.push($("#CB_FCTRY3").val());
        BaseParameter.ListSearchString.push($("#T3_S1").val());
        BaseParameter.ListSearchString.push($("#T3_S2").val());
        BaseParameter.ListSearchString.push($("#T3_S3").val());
        BaseParameter.ListSearchString.push($("#T3_S4").val());
        BaseParameter.ListSearchString.push($("#ComboBox4").val());
        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        let url = "/B10/Buttonfind_Click";

        fetch(url, {
            method: "POST",
            body: formUpload,
            headers: {
            }
        }).then((response) => {
            response.json().then((data) => {
                let BaseResultButtonfind = data;
                BaseResult.DataGridView4 = BaseResultButtonfind.DataGridView4;
                DataGridView4Render();
                let CB3_TEXT = $("#T3_S1").val();
                let DGV_D1 = $("#T3_S2").val();
                let DGV_D2 = CB3_TEXT;
                if (CB3_TEXT == "ALL") {
                    CB3_TEXT = "%%";
                    DGV_D2 = "ALL";
                }
                $("#Label7").val(DGV_D1);
                $("#Label6").val(DGV_D2);
                $("#BackGround").css("display", "none");
            }).catch((err) => {
                $("#BackGround").css("display", "none");
            })
        });
    }
    if (TagIndex == 3) {
        $("#BackGround").css("display", "block");
        let BaseParameter = new Object();
        BaseParameter = {
            Action: TagIndex,
            ListSearchString: [],
        }
        BaseParameter.CheckBox1 = document.getElementById("CheckBox1").checked;
        BaseParameter.ListSearchString.push($("#CB_FCTRY2").val());
        BaseParameter.ListSearchString.push($("#T2_S1").val());
        BaseParameter.ListSearchString.push($("#T2_S2").val());
        BaseParameter.ListSearchString.push($("#T2_S3").val());
        BaseParameter.ListSearchString.push($("#T2_S4").val());
        BaseParameter.ListSearchString.push($("#T2_S5").val());
        BaseParameter.ListSearchString.push($("#T2_S6").val());
        BaseParameter.ListSearchString.push($("#ComboBox3").val());
        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        let url = "/B10/Buttonfind_Click";

        fetch(url, {
            method: "POST",
            body: formUpload,
            headers: {
            }
        }).then((response) => {
            response.json().then((data) => {
                let BaseResultButtonfind = data;
                BaseResult.T2_DGV1 = BaseResultButtonfind.T2_DGV1;
                T2_DGV1Render();
                T3_Timer1Start();
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
        let DGV2_D1 = $("#LB_DT").val();
        if (DGV2_D1 == "-") {
            IsSave = false;
        }
        let DGV2_D2 = $("#LB_STG").val();
        if (DGV2_D2 == "-") {
            IsDelete = false;
        }
        if (BaseResult.DataGridView3.length == 0) {
            IsSave = false;
        }
        if (IsSave == true) {
            $("#BackGround").css("display", "block");
            let BaseParameter = new Object();
            BaseParameter = {
                Action: TagIndex,
                ListSearchString: [],
            }
            BaseParameter.USER_ID = GetCookieValue("UserID");
            BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
            BaseParameter.ListSearchString.push(DGV2_D1);
            BaseParameter.ListSearchString.push(DGV2_D2);
            BaseParameter.ListSearchString.push($("#ComboBox1").val());
            BaseParameter.DataGridView3 = BaseResult.DataGridView3;
            let formUpload = new FormData();
            formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
            let url = "/B10/Buttonsave_Click";

            fetch(url, {
                method: "POST",
                body: formUpload,
                headers: {
                }
            }).then((response) => {
                response.json().then((data) => {
                    let BaseResultButtonsave = data;
                    if (BaseResultButtonsave) {
                        if (BaseResultButtonsave.Code) {
                            let url = BaseResultButtonsave.Code;
                            OpenWindowByURL(url, 200, 200);
                        }
                        else {
                            alert(localStorage.getItem("SaveSuccess"));
                        }
                    }
                    Buttonfind_Click();
                    $("#BackGround").css("display", "none");
                }).catch((err) => {
                    $("#BackGround").css("display", "none");
                })
            });
        }
    }
    if (TagIndex == 2) {
        let IsSave = true;
        let Label7 = $("#Label7").val();
        if (Label7 == "-") {
            IsSave = false;
        }
        let Label6 = $("#Label6").val();
        if (Label6 == "-") {
            IsSave = false;
        }
        if (BaseResult.DataGridView4.length == 0) {
            IsSave = false;
        }
        if (IsSave == true) {
            $("#BackGround").css("display", "block");
            let BaseParameter = new Object();
            BaseParameter = {
                Action: TagIndex,
                ListSearchString: [],
            }
            BaseParameter.DataGridView4 = BaseResult.DataGridView4;
            let formUpload = new FormData();
            formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
            let url = "/B10/Buttonsave_Click";

            fetch(url, {
                method: "POST",
                body: formUpload,
                headers: {
                }
            }).then((response) => {
                response.json().then((data) => {
                    alert(localStorage.getItem("SaveSuccess"));
                    Buttonfind_Click();
                    $("#BackGround").css("display", "none");
                }).catch((err) => {
                    $("#BackGround").css("display", "none");
                })
            });
        }
    }
}
function Buttondelete_Click() {
    if (TagIndex == 2) {
        let IsDelete = true;
        let Label7 = $("#Label7").val();
        if (Label7 == "-") {
            IsDelete = false;
        }
        let Label6 = $("#Label6").val();
        if (Label6 == "-") {
            IsDelete = false;
        }
        if (BaseResult.DataGridView4.length == 0) {
            IsDelete = false;
        }
        if (IsDelete == true) {
            $("#BackGround").css("display", "block");
            let BaseParameter = new Object();
            BaseParameter = {
            }
            BaseParameter.DataGridView4 = BaseResult.DataGridView4;
            let formUpload = new FormData();
            formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
            let url = "/B10/Buttondelete_Click";

            fetch(url, {
                method: "POST",
                body: formUpload,
                headers: {
                }
            }).then((response) => {
                response.json().then((data) => {
                    alert(localStorage.getItem("SaveSuccess"));
                    Buttonfind_Click();
                    $("#BackGround").css("display", "none");
                }).catch((err) => {
                    $("#BackGround").css("display", "none");
                })
            });
        }

    }
}
function Buttoncancel_Click() {
    if (TagIndex == 3) {
        document.getElementById("T2_S1").selectedIndex = "0";
        $("#T2_S2").val(Now);
        $("#T2_S3").val("");
        $("#T2_S4").val("");
        $("#T2_S5").val("");
        document.getElementById("T2_S6").selectedIndex = "0";
    }
    if (TagIndex == 2) {
        document.getElementById("T3_S1").selectedIndex = "0";
        $("#T3_S2").val(Now);
        $("#T3_S3").val("");
        $("#T3_S4").val("");
    }
}
function Buttoninport_Click() {

}
function Buttonexport_Click() {
    if (TagIndex == 2) {
        TableHTMLToExcel("DataGridView4Table", "B10_MaterialApprove", "B10_MaterialApprove")
    }
    if (TagIndex == 3) {
        TableHTMLToExcel("T2_DGV1Table", "B10_MaterialList", "B10_MaterialList")
    }
}
function Buttonprint_Click() {
    if (TagIndex == 2) {
        let IsPrint = true;
        let T3_S4 = $("#T3_S4").val();
        if (T3_S4 == "") {
            alert(localStorage.getItem("Notification_B10_001"));
            IsPrint = false;
        }
        if (BaseResult.DataGridView4.length <= 0) {
            alert(localStorage.getItem("Notification_B10_002"));
            IsPrint = false;
        }
        if (IsPrint == true) {
            $("#BackGround").css("display", "block");
            let BaseParameter = new Object();
            BaseParameter = {
                Action: TagIndex,
            }
            BaseParameter.DataGridView4 = BaseResult.DataGridView4;
            let formUpload = new FormData();
            formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
            let url = "/B10/Buttonprint_Click";

            fetch(url, {
                method: "POST",
                body: formUpload,
                headers: {
                }
            }).then((response) => {
                response.json().then((data) => {
                    let BaseResultButtonprint = data
                    if (BaseResultButtonprint) {
                        if (BaseResultButtonprint.Code) {
                            let url = BaseResultButtonprint.Code;
                            OpenWindowByURL(url, 200, 200);
                        }
                    }
                    alert(localStorage.getItem("SaveSuccess"));
                    $("#BackGround").css("display", "none");
                }).catch((err) => {
                    $("#BackGround").css("display", "none");
                })
            });
        }

    }
}
function Buttonhelp_Click() {
    let url = "/WMP_PLAY";
    OpenWindowByURL(url, 800, 460);
}
function Buttonclose_Click() {
    history.back();
}
function CB_FCTRY_LINE() {
    $("#CB_FCTRY1").empty();
    $("#CB_FCTRY3").empty();
    $("#CB_FCTRY2").empty();
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/B10/CB_FCTRY_LINE";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            let BaseResultCB_FCTRY_LINE = data;
            BaseResult.CB_FCTRY1 = BaseResultCB_FCTRY_LINE.CB_FCTRY1;
            for (let i = 0; i < BaseResult.CB_FCTRY1.length; i++) {
                var optionCB_FCTRY1 = document.createElement("option");
                optionCB_FCTRY1.text = BaseResult.CB_FCTRY1[i].CD_SYS_NOTE;
                optionCB_FCTRY1.value = BaseResult.CB_FCTRY1[i].CD_SYS_NOTE;

                var CB_FCTRY1 = document.getElementById("CB_FCTRY1");
                CB_FCTRY1.add(optionCB_FCTRY1);

                var optionCB_FCTRY3 = document.createElement("option");
                optionCB_FCTRY3.text = BaseResult.CB_FCTRY1[i].CD_SYS_NOTE;
                optionCB_FCTRY3.value = BaseResult.CB_FCTRY1[i].CD_SYS_NOTE;

                var CB_FCTRY3 = document.getElementById("CB_FCTRY3");
                CB_FCTRY3.add(optionCB_FCTRY3);

                var optionCB_FCTRY2 = document.createElement("option");
                optionCB_FCTRY2.text = BaseResult.CB_FCTRY1[i].CD_SYS_NOTE;
                optionCB_FCTRY2.value = BaseResult.CB_FCTRY1[i].CD_SYS_NOTE;

                var CB_FCTRY2 = document.getElementById("CB_FCTRY2");
                CB_FCTRY2.add(optionCB_FCTRY2);
            }
            let CB_FCTRY1Value = $("#CB_FCTRY1").val();
            if (CB_FCTRY1Value == "Factory 1") {
                FAC_ST = " AND  RIGHT(`CD_SYS_NOTE`, 2) > 0  AND RIGHT(`CD_SYS_NOTE`, 2)  <= 60 ";
            }
            else {
                FAC_ST = " AND RIGHT(`CD_SYS_NOTE`, 2)   > 60 "
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
function COMLIST_LINE_1() {
    $("#T2_S1").empty();

    var optionALL = document.createElement("option");
    optionALL.text = "ALL";
    optionALL.value = "ALL";

    var T2_S1ALL = document.getElementById("T2_S1");
    T2_S1ALL.add(optionALL);

    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        SearchString: FAC_ST,
    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/B10/COMLIST_LINE_1";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            let BaseResultCOMLIST_LINE_1 = data;
            BaseResult.T2_S1 = BaseResultCOMLIST_LINE_1.T2_S1;
            for (let i = 0; i < BaseResult.T2_S1.length; i++) {

                var option = document.createElement("option");
                option.text = BaseResult.T2_S1[i].CD_SYS_NOTE;
                option.value = BaseResult.T2_S1[i].CD_SYS_NOTE;

                var T2_S1 = document.getElementById("T2_S1");
                T2_S1.add(option);
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
function COMLIST_LINE_2() {
    $("#ComboBox2").empty();

    var optionALL = document.createElement("option");
    optionALL.text = "ALL";
    optionALL.value = "ALL";

    var ComboBox2ALL = document.getElementById("ComboBox2");
    ComboBox2ALL.add(optionALL);

    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        SearchString: FAC_ST,
    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/B10/COMLIST_LINE_2";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            let BaseResultCOMLIST_LINE_2 = data;
            BaseResult.ComboBox2 = BaseResultCOMLIST_LINE_2.ComboBox2;
            for (let i = 0; i < BaseResult.ComboBox2.length; i++) {

                var option = document.createElement("option");
                option.text = BaseResult.ComboBox2[i].CD_SYS_NOTE;
                option.value = BaseResult.ComboBox2[i].CD_SYS_NOTE;

                var ComboBox2 = document.getElementById("ComboBox2");
                ComboBox2.add(option);
            }
            Buttonfind_Click();
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
function COMLIST_LINE_3() {
    $("#T3_S1").empty();

    var optionALL = document.createElement("option");
    optionALL.text = "ALL";
    optionALL.value = "ALL";

    var CT3_S1ALL = document.getElementById("T3_S1");
    CT3_S1ALL.add(optionALL);

    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        SearchString: FAC_ST,
    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/B10/COMLIST_LINE_3";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            let BaseResultCOMLIST_LINE_3 = data;
            BaseResult.T3_S1 = BaseResultCOMLIST_LINE_3.T3_S1;
            for (let i = 0; i < BaseResult.T3_S1.length; i++) {

                var option = document.createElement("option");
                option.text = BaseResult.T3_S1[i].CD_SYS_NOTE;
                option.value = BaseResult.T3_S1[i].CD_SYS_NOTE;

                var T3_S1 = document.getElementById("T3_S1");
                T3_S1.add(option);
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
function DataGridView2Render() {
    let HTML = "";
    BaseResult.DataGridView3 = [];
    DataGridView3Render();
    if (BaseResult) {
        if (BaseResult.DataGridView2) {
            if (BaseResult.DataGridView2.length > 0) {
                DataGridView2_SelectionChanged(0);
                for (let i = 0; i < BaseResult.DataGridView2.length; i++) {
                    HTML = HTML + "<tr onclick='DataGridView2_SelectionChanged(" + i + ")'>";
                    /*HTML = HTML + "<td>" + new Date(BaseResult.DataGridView2[i].DATE.replace(/(\d{2})-(\d{2})-(\d{4})/, "$2/$1/$3")).toISOString().slice(0, 10) + "</td>";*/
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].Name + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].STAGE + "</td>";
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
let DataGridView2Table = document.getElementById("DataGridView2Table");
DataGridView2Table.addEventListener('click', function (event) {
    if (event.target.tagName === 'TH') {
        let text = event.target.innerText;
        let key = "DATE";
        IsTableSort = !IsTableSort;
        ListSort(BaseResult.DataGridView2, key, text, IsTableSort);
        DataGridView2Render();
    }
});
function DataGridView2_SelectionChanged(i) {
    DataGridView2RowIndex = i;
    DataT2DGV_LOAD();
}
function DataT2DGV_LOAD() {
    $("#LB_DT").val("-");
    $("#LB_STG").val("-");
    let DGV_D1 = BaseResult.DataGridView2[DataGridView2RowIndex].DATE;
    let DGV_D2 = BaseResult.DataGridView2[DataGridView2RowIndex].STAGE;
    $("#LB_DT").val(DGV_D1);
    $("#LB_STG").val(DGV_D2);


    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        ListSearchString: [],
    }
    BaseParameter.ListSearchString.push(DGV_D1);
    BaseParameter.ListSearchString.push(DGV_D2);
    BaseParameter.ListSearchString.push($("#ComboBox1").val());
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/B10/DataT2DGV_LOAD";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            let BaseResultDataT2DGV_LOAD = data;
            BaseResult.DataGridView3 = BaseResultDataT2DGV_LOAD.DataGridView3;
            DataGridView3Render();
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });

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
                        HTML = HTML + "<td><label><input id='DataGridView3CHK" + i + "' class='form-check-input' type='checkbox' checked onclick='DataGridView3CHKChanged(" + i + ")'><span></span></label></td>";
                    }
                    else {
                        HTML = HTML + "<td><label><input id='DataGridView3CHK" + i + "' class='form-check-input' type='checkbox' onclick='DataGridView3CHKChanged(" + i + ")'><span></span></label></td>";
                    }
                    HTML = HTML + "<td>" + BaseResult.DataGridView3[i].CODE + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView3[i].STAGE + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView3[i].LOC + "</td>";
                    /*HTML = HTML + "<td>" + new Date(BaseResult.DataGridView3[i].DATE.replace(/(\d{2})-(\d{2})-(\d{4})/, "$2/$1/$3")).toISOString().slice(0, 10) + "</td>";*/
                    HTML = HTML + "<td>" + BaseResult.DataGridView3[i].Name + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView3[i].DJG_CODE + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView3[i].PART_NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView3[i].PART_NAME + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView3[i].FAMILY + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView3[i].SNP + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView3[i].QTY + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView3[i].STOCK + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView3[i].BOX + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView3[i].TMMTIN_CODE + "</td>";
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
let DataGridView3Table = document.getElementById("DataGridView3Table");
DataGridView3Table.addEventListener('click', function (event) {
    if (event.target.tagName === 'TH') {
        let text = event.target.innerText;
        let key = "LOC";
        IsTableSort = !IsTableSort;
        if (text == "LOCATION") {
            if (IsTableSort == true) {
                BaseResult.DataGridView3.sort((a, b) => (a.LOC > b.LOC ? 1 : -1));
            }
            else {
                BaseResult.DataGridView3.sort((a, b) => (a.LOC < b.LOC ? 1 : -1));
            }
        }
        else {
            ListSort(BaseResult.DataGridView3, key, text, IsTableSort);
        }
        DataGridView3Render();
    }
});
function DataGridView3SortSub() {
    IsTableSort = !IsTableSort;
    if (BaseResult.DataGridView3) {
        if (IsTableSort == true) {
            switch (i) {
                case 9:
                    BaseResult.DataGridView3.sort((a, b) => (a.Receipt_Data > b.Receipt_Data ? 1 : -1));
                    break;
                case 11:
                    BaseResult.DataGridView3.sort((a, b) => (a.Release_Date > b.Release_Date ? 1 : -1));
                    break;
            }
        }
        else {
            switch (i) {
                case 9:
                    BaseResult.DataGridView3.sort((a, b) => (a.Receipt_Data < b.Receipt_Data ? 1 : -1));
                    break;
                case 11:
                    BaseResult.DataGridView3.sort((a, b) => (a.Release_Date < b.Release_Date ? 1 : -1));
                    break;
            }
        }
    }
    DataGridView3Render();
}
function DataGridView3_SelectionChanged(i) {
    DataGridView3RowIndex = i;
}
function DataGridView3_CellClick(i) {
    DataGridView3RowIndex = i;
    let IsCheck = true;
    if (BaseResult.DataGridView3[DataGridView3RowIndex].CHK == true) {
        BaseResult.DataGridView3[DataGridView3RowIndex].CHK = false;
        IsCheck = false;
    }
    if (IsCheck == true) {
        let CHK_COUNT = 0;
        for (let i = 0; i < BaseResult.DataGridView3.length; i++) {
            if (BaseResult.DataGridView3[i].CHK == true) {
                CHK_COUNT = CHK_COUNT + 1;
            }
        }
        if (CHK_COUNT <= 9) {
            BaseResult.DataGridView3[DataGridView3RowIndex].CHK = true;
        }
        else {
            BaseResult.DataGridView3[DataGridView3RowIndex].CHK = false;
        }
    }
    DataGridView3Render();
}
function DataGridView3CHKChanged(i) {
    DataGridView3RowIndex = i;
    DataGridView3_CellClick(i);
}
function DataGridView4Render() {
    let HTML = "";
    if (BaseResult) {
        if (BaseResult.DataGridView4) {
            if (BaseResult.DataGridView4.length > 0) {
                for (let i = 0; i < BaseResult.DataGridView4.length; i++) {
                    DataGridView4_SelectionChanged(0);
                    HTML = HTML + "<tr onclick='DataGridView4_SelectionChanged(" + i + ")'>";
                    if (BaseResult.DataGridView4[i].CHK) {
                        HTML = HTML + "<td><label><input id='DataGridView4CHK" + i + "' class='form-check-input' type='checkbox' checked onclick='DataGridView4CHKChanged(" + i + ")'><span></span></label></td>";
                    }
                    else {
                        HTML = HTML + "<td><label><input id='DataGridView4CHK" + i + "' class='form-check-input' type='checkbox' onclick='DataGridView4CHKChanged(" + i + ")'><span></span></label></td>";
                    }
                    HTML = HTML + "<td>" + BaseResult.DataGridView4[i].STAGE + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView4[i].DSCN + "</td>";
                    /*HTML = HTML + "<td>" + new Date(BaseResult.DataGridView4[i].DATE.replace(/(\d{2})-(\d{2})-(\d{4})/, "$2/$1/$3")).toISOString().slice(0, 10) + "</td>";*/
                    HTML = HTML + "<td>" + BaseResult.DataGridView4[i].Name + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView4[i].DJG_CODE + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView4[i].PART_NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView4[i].PART_NAME + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView4[i].FAMILY + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView4[i].SNP + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView4[i].QTY + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView4[i].LOC + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView4[i].BOX + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView4[i].TMMTIN_SHEETNO + "</td>";
                    HTML = HTML + "</tr>";
                }
            }
        }
    }
    document.getElementById("DataGridView4").innerHTML = HTML;
}
function DataGridView4Sort() {
    //IsTableSort = !IsTableSort;
    //DataGridViewSort(BaseResult.DataGridView4, IsTableSort);
    //DataGridView4Render();
}
let DataGridView4Table = document.getElementById("DataGridView4Table");
DataGridView4Table.addEventListener('click', function (event) {
    if (event.target.tagName === 'TH') {
        let text = event.target.innerText;
        let key = "STAGE";
        IsTableSort = !IsTableSort;
        ListSort(BaseResult.DataGridView4, key, text, IsTableSort);
        DataGridView4Render();
    }
});
function DataGridView4_SelectionChanged(i) {
    DataGridView4RowIndex = i;
}
function DataGridView4CHKChanged(i) {
    let id = "DataGridView4CHK" + i;
    DataGridView4RowIndex = i;
    BaseResult.DataGridView4[DataGridView4RowIndex].CHK = $("#" + id).is(":checked");
}
function T2_DGV1Render() {
    $("#Label16").val(0);
    let BOX_QTY = 0;
    let HTML = "";
    if (BaseResult) {
        if (BaseResult.T2_DGV1) {
            if (BaseResult.T2_DGV1.length > 0) {
                T2_DGV1_SelectionChanged(0);
                for (let i = 0; i < BaseResult.T2_DGV1.length; i++) {
                    if (BaseResult.T2_DGV1[i].TMMTIN_DSCN_YN == "C") {
                        HTML = HTML + "<tr style='background-color: yellow;' onclick='T2_DGV1_SelectionChanged(" + i + ")'>";
                    }
                    else {
                        HTML = HTML + "<tr onclick='T2_DGV1_SelectionChanged(" + i + ")'>";
                    }

                    if (BaseResult.T2_DGV1[i].TMMTIN_CNF_YN == "Y") {
                        HTML = HTML + "<td style='background-color: lightgreen;'>" + BaseResult.T2_DGV1[i].TMMTIN_CNF_YN + "</td>";
                    }
                    else {
                        HTML = HTML + "<td style='background-color: palevioletred;'>" + BaseResult.T2_DGV1[i].TMMTIN_CNF_YN + "</td>";
                    }
                    if (BaseResult.T2_DGV1[i].TMMTIN_DSCN_YN == "Y") {
                        HTML = HTML + "<td style='background-color: lightgreen;'>" + BaseResult.T2_DGV1[i].TMMTIN_DSCN_YN + "</td>";
                    }
                    else {
                        if (BaseResult.T2_DGV1[i].TMMTIN_DSCN_YN == "C") {
                            HTML = HTML + "<td style='background-color: yellow;'>" + BaseResult.T2_DGV1[i].TMMTIN_DSCN_YN + "</td>";
                        }
                        else {
                            HTML = HTML + "<td style='background-color: palevioletred;'>" + BaseResult.T2_DGV1[i].TMMTIN_DSCN_YN + "</td>";
                        }
                    }
                    HTML = HTML + "<td>" + BaseResult.T2_DGV1[i].STAGE + "</td>";
                    HTML = HTML + "<td>" + BaseResult.T2_DGV1[i].PART_NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.T2_DGV1[i].SNP + "</td>";
                    HTML = HTML + "<td>" + BaseResult.T2_DGV1[i].QTY + "</td>";
                    HTML = HTML + "<td><b>" + BaseResult.T2_DGV1[i].QuantityActual + "</b></td>";
                    if (BaseResult.T2_DGV1[i].QuantityGAP < 0) {
                        HTML = HTML + "<td style='color: red;'>" + BaseResult.T2_DGV1[i].QuantityGAP + "</td>";
                    }
                    else {
                        HTML = HTML + "<td style='color: green;'>" + BaseResult.T2_DGV1[i].QuantityGAP + "</td>";
                    }
                    HTML = HTML + "<td>" + BaseResult.T2_DGV1[i].PART_NAME + "</td>";
                    HTML = HTML + "<td>" + BaseResult.T2_DGV1[i].CREATE_DTM + "</td>";
                    HTML = HTML + "<td>" + BaseResult.T2_DGV1[i].ORDER + "</td>";
                    HTML = HTML + "<td>" + BaseResult.T2_DGV1[i].TMMTIN_SHEETNO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.T2_DGV1[i].BOX_COUNT + "</td>";
                    HTML = HTML + "</tr>";

                    BOX_QTY = BOX_QTY + BaseResult.T2_DGV1[i].BOX_COUNT;
                }
            }
        }
    }
    document.getElementById("T2_DGV1").innerHTML = HTML;
    $("#Label16").val(BOX_QTY);
    T2_DGV1_DataBindingComplete();
}
function T2_DGV1Sort() {
    //IsTableSort = !IsTableSort;
    //DataGridViewSort(BaseResult.T2_DGV1, IsTableSort);
    //T2_DGV1Render();
}
let T2_DGV1Table = document.getElementById("T2_DGV1Table");
T2_DGV1Table.addEventListener('click', function (event) {
    if (event.target.tagName === 'TH') {
        let text = event.target.innerText;
        let key = "STAGE";
        IsTableSort = !IsTableSort;
        if (text == "SHEET_NO") {
            if (IsTableSort == true) {
                BaseResult.T2_DGV1.sort((a, b) => (a.TMMTIN_SHEETNO > b.TMMTIN_SHEETNO ? 1 : -1));
            }
            else {
                BaseResult.T2_DGV1.sort((a, b) => (a.TMMTIN_SHEETNO < b.TMMTIN_SHEETNO ? 1 : -1));
            }
        }
        else {
            if (text == "DATE OF REQUEST") {
                if (IsTableSort == true) {
                    BaseResult.T2_DGV1.sort((a, b) => (a.CREATE_DTM > b.CREATE_DTM ? 1 : -1));
                }
                else {
                    BaseResult.T2_DGV1.sort((a, b) => (a.CREATE_DTM < b.CREATE_DTM ? 1 : -1));
                }
            }
            else {
                ListSort(BaseResult.T2_DGV1, key, text, IsTableSort);
            }
        }        
        T2_DGV1Render();
    }
});
function T2_DGV1_DataBindingComplete() {
    D2_SORT();
}
function T2_DGV1_SelectionChanged(i) {
    T2_DGV1RowIndex = i;
}
function D2_SORT() {

}