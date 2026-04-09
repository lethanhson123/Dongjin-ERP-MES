let IsTableSort = false;
let BaseResult = new Object();
let TagIndex = 1;
let DataGridView1RowIndex = 0;
let T_DGV_02RowIndex = 0;
let IntervalTimer;
$(document).ready(function () {
    var now = new Date();
    var day = ("0" + now.getDate()).slice(-2);
    var month = ("0" + (now.getMonth() + 1)).slice(-2);
    var today = now.getFullYear() + "-" + (month) + "-" + (day);

    $("#DateTimePicker3").val(today);
    $("#DateTimePicker1").val(today);
    $("#TBTEXT03").val(today);
    $("#IN_LISTDATE1").val(today);
    $("#IN_LISTDATE2").val(today);
    $("#OUT_LISTDATE1").val(today);
    $("#OUT_LISTDATE2").val(today);

    $("#TextBox4").val("");
    $("#TextBox5").val("");
    MC_LOAD();
});
function MC_LOAD() {

}
$("#ATag001").click(function (e) {
    TagIndex = 1;
});
$("#ATag002").click(function (e) {
    TagIndex = 2;
});
$("#ATag003").click(function (e) {
    TagIndex = 3;
});
$("#ATag004").click(function (e) {
    TagIndex = 4;
});
$("#ATag005").click(function (e) {
    TagIndex = 5;
});
$("#ATag006").click(function (e) {
    TagIndex = 6;
});
$("#DateTimePicker3").change(function (e) {
    DateTimePicker3_ValueChanged();
});
$("#DateTimePicker1").change(function (e) {
    DateTimePicker1_ValueChanged_1();
});
function DateTimePicker3_ValueChanged() {
    let DateTimePicker3 = $("#DateTimePicker3").val();
    let DateTimePicker1 = $("#DateTimePicker1").val();
    var DateTimePicker3Date = new Date(DateTimePicker3.replace(/(\d{2})-(\d{2})-(\d{4})/, "$2/$1/$3"));
    var DateTimePicker1Date = new Date(DateTimePicker1.replace(/(\d{2})-(\d{2})-(\d{4})/, "$2/$1/$3"));
    if (DateTimePicker1Date < DateTimePicker3Date) {
        $("#DateTimePicker1").val(DateTimePicker3);
    }
}
function DateTimePicker1_ValueChanged_1() {
    DateTimePicker3_ValueChanged();
}
$("#ComboBox1").change(function (e) {
    ComboBox1_SelectedIndexChanged();
});
function ComboBox1_SelectedIndexChanged() {
    Buttonfind_Click();
}
$("#TextBox1").keydown(function (e) {
    if (e.keyCode == 13) {
        TextBox1_KeyDown();
    }
});
function TextBox1_KeyDown() {
    Buttonfind_Click();
}
$("#rbchk1").click(function () {
    rbchk1_Click();
});
$("#rbchk2").click(function () {
    rbchk1_Click();
});
function rbchk1_Click() {
    CHK_ALL();
}
function CHK_ALL() {
    if (BaseResult) {
        if (BaseResult.DataGridView1) {
            if (BaseResult.DataGridView1.length > 0) {
                let rbchk1 = document.getElementById("rbchk1").checked;
                let rbchk2 = document.getElementById("rbchk2").checked;

                if (rbchk1 == true) {
                    for (let i = 0; i < BaseResult.DataGridView1.length; i++) {
                        BaseResult.DataGridView1[i].CHK = true;
                    }
                }
                if (rbchk2 == true) {
                    for (let i = 0; i < BaseResult.DataGridView1.length; i++) {
                        BaseResult.DataGridView1[i].CHK = !BaseResult.DataGridView1[i].CHK;
                    }
                }
                DataGridView1Render();
            }
        }
    }
}
$("#TextBox2").keydown(function (e) {
    if (e.keyCode == 13) {
        TextBox2_KeyDown();
    }
});
function TextBox2_KeyDown() {
    Buttonadd_Click();
    //Buttonfind_Click();
}
$("#TextBox4").keydown(function (e) {
    if (e.keyCode == 13) {
        TextBox4_KeyDown();
    }
});
function TextBox4_KeyDown() {
    $("#TextBox3").focus();
}
$("#TextBox3").keydown(function (e) {
    if (e.keyCode == 13) {
        TextBox3_KeyDown();
    }
});
function TextBox3_KeyDown() {
    Buttonadd_Click();
    Buttonfind_Click();
}
$("#TextBox5").keydown(function (e) {
    if (e.keyCode == 13) {
        TextBox5_KeyDown();
    }
});
function TextBox5_KeyDown() {
    Buttonadd_Click();
    Buttonfind_Click();
    $("#TextBox3").val("");
    $("#TextBox3").focus();
}
$("#IN_LISTDATE1").change(function (e) {
    DateTimePicker1_ValueChanged();
});
$("#IN_LISTDATE2").change(function (e) {
    DateTimePicker2_ValueChanged();
});
function DateTimePicker1_ValueChanged() {
    let IN_LISTDATE1 = $("#IN_LISTDATE1").val();
    let IN_LISTDATE2 = $("#IN_LISTDATE2").val();
    var IN_LISTDATE1Date = new Date(IN_LISTDATE1.replace(/(\d{2})-(\d{2})-(\d{4})/, "$2/$1/$3"));
    var IN_LISTDATE2Date = new Date(IN_LISTDATE2.replace(/(\d{2})-(\d{2})-(\d{4})/, "$2/$1/$3"));
    if (IN_LISTDATE2Date < IN_LISTDATE1Date) {
        $("#IN_LISTDATE2").val(IN_LISTDATE1);
    }
}
function DateTimePicker2_ValueChanged() {
    DateTimePicker1_ValueChanged();
}
$("#TextBox7").keypress(function (e) {
    TextBox7_KeyPress();
});
function TextBox7_KeyPress() {

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
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        Action: TagIndex,
        ListSearchString: [],
    }
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    switch (TagIndex) {
        case 1:
            BaseParameter.ListSearchString.push($("#DateTimePicker1").val());
            BaseParameter.ListSearchString.push($("#DateTimePicker3").val());
            BaseParameter.ListSearchString.push($("#ComboBox2").val());
            BaseParameter.ListSearchString.push($("#ComboBox1").val());
            BaseParameter.ListSearchString.push($("#TextBox1").val());
            break;
        case 2:
            break;
        case 3:
            break;
        case 4:
            BaseParameter.ListSearchString.push($("#TBTEXT01").val());
            BaseParameter.ListSearchString.push($("#TBTEXT03").val());
            break;
        case 5:
            BaseParameter.ListSearchString.push($("#IN_LISTDATE1").val());
            BaseParameter.ListSearchString.push($("#IN_LISTDATE2").val());
            BaseParameter.ListSearchString.push($("#TextBox6").val());
            $("#Label11").val("-");
            $("#Label12").val("-");
            break;
        case 6:
            BaseParameter.ListSearchString.push($("#OUT_LISTDATE1").val());
            BaseParameter.ListSearchString.push($("#OUT_LISTDATE2").val());
            break;
    }

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/B08/Buttonfind_Click";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            BaseResult = data;
            switch (TagIndex) {
                case 1:
                    DataGridView1Render();
                    break;
                case 2:
                    DataGridView2Render();
                    break;
                case 3:
                    DataGridView3Render();
                    break;
                case 4:
                    T_DGV_01Render();
                    T_DGV_02Render();
                    break;
                case 5:
                    IN_LIST_DGVRender();
                    let W_TIME = 0;
                    let TextBox7 = $("#TextBox7").val();
                    if (TextBox7 == "") {
                        TextBox7 = "450";
                        $("#TextBox7").val(TextBox7);
                    }
                    W_TIME = Number(TextBox7);
                    $("#Label11").val("-");
                    $("#Label12").val("-");
                    if (BaseResult) {
                        if (BaseResult.DGV_B08_09_1) {
                            if (BaseResult.DGV_B08_09_1.length > 0) {
                                let Label11 = BaseResult.DGV_B08_09_1[0].MT;
                                let Label12 = BaseResult.DGV_B08_09_1[0].MIN / W_TIME * 100;
                                $("#Label11").val("" + Label11.toFixed(2));
                                $("#Label12").val("" + Label12.toFixed(2));
                            }
                        }
                    }
                    break;
                case 6:
                    OUT_LIST_DGVRender();
                    break;
            }

            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
function Buttonadd_Click() {
    if (TagIndex == 2) {
        let IsAdd = true;
        let TextBox2 = $("#TextBox2").val();
        if (TextBox2 == "") {
            IsAdd = false;
        }
        let TextBox4 = $("#TextBox4").val();
        if (TextBox4 == "") {
            IsAdd = false;
            alert(localStorage.getItem("ERROR_B08_Add_001"));
        }
        if (IsAdd == true) {
            $("#BackGround").css("display", "block");
            let BaseParameter = new Object();
            BaseParameter = {
                Action: TagIndex,
                ListSearchString: [],
            }
            BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
            BaseParameter.ListSearchString.push(TextBox2);
            BaseParameter.ListSearchString.push(TextBox4);
            let formUpload = new FormData();
            formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
            let url = "/B08/Buttonadd_Click";

            fetch(url, {
                method: "POST",
                body: formUpload,
                headers: {
                }
            }).then((response) => {
                response.json().then((data) => {
                    let BaseResultButtonadd = data;
                    if (BaseResultButtonadd) {
                        if (BaseResultButtonadd.DGV_B08_01) {
                            if (BaseResultButtonadd.DGV_B08_02) {
                                if (BaseResultButtonadd.DGV_B08_01.length <= 0) {
                                    if (BaseResultButtonadd.DGV_B08_02.length > 0) {
                                        Buttonfind_Click();
                                        $("#TextBox2").val("");
                                        $("#TextBox2").focus();
                                    }
                                    else {
                                        alert(localStorage.getItem("ERROR_B08_Add_002"));
                                        $("#TextBox2").val("");
                                        $("#TextBox2").focus();
                                    }
                                }
                                else {
                                    if (BaseResultButtonadd.DGV_B08_01.length >= 1) {
                                        var audio = new Audio("/Media/Sash_brk.wav");
                                        audio.play();
                                        alert(localStorage.getItem("ERROR_B08_Add_003"));
                                        $("#TextBox2").val("");
                                        $("#TextBox2").focus();
                                    }
                                    else {
                                        alert(localStorage.getItem("ERROR_B08_Add_002"));
                                        $("#TextBox2").val("");
                                        $("#TextBox2").focus();
                                    }
                                }
                            }
                        }
                    }
                    $("#BackGround").css("display", "none");
                }).catch((err) => {
                    alert(localStorage.getItem("ERRORAnErrorOccurred"));
                    $("#BackGround").css("display", "none");
                })
            });
        }
    }
    if (TagIndex == 3) {
        let IsAdd = true;
        let TextBox3 = $("#TextBox3").val();
        if (TextBox3 == "") {
            IsAdd = false;
        }
        let TextBox5 = $("#TextBox5").val();
        if (TextBox5 == "") {
            IsAdd = false;
            alert(localStorage.getItem("ERROR_B08_Add_004"));
        }
        if (IsAdd == true) {
            $("#BackGround").css("display", "block");
            let BaseParameter = new Object();
            BaseParameter = {
                Action: TagIndex,
                ListSearchString: [],
            }
            BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
            BaseParameter.ListSearchString.push(TextBox3);
            BaseParameter.ListSearchString.push(TextBox5);
            let formUpload = new FormData();
            formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
            let url = "/B08/Buttonadd_Click";

            fetch(url, {
                method: "POST",
                body: formUpload,
                headers: {
                }
            }).then((response) => {
                response.json().then((data) => {
                    let BaseResultButtonadd = data;
                    if (BaseResultButtonadd) {
                        if (BaseResultButtonadd.DGV_B08_01) {
                            if (BaseResultButtonadd.DGV_B08_02) {
                                if (BaseResultButtonadd.DGV_B08_01.length <= 0) {
                                    if (BaseResultButtonadd.DGV_B08_02.length > 0) {
                                        Buttonfind_Click();
                                        $("#TextBox3").val("");
                                        $("#TextBox3").focus();
                                    }
                                    else {
                                        alert(localStorage.getItem("ERROR_B08_Add_002"));
                                        $("#TextBox3").val("");
                                        $("#TextBox3").focus();
                                    }
                                }
                                else {
                                    if (BaseResultButtonadd.DGV_B08_02.length > 1) {
                                        var audio = new Audio("/Media/Sash_brk.wav");
                                        audio.play();
                                        alert(localStorage.getItem("ERROR_B08_Add_003"));
                                        $("#TextBox3").val("");
                                        $("#TextBox3").focus();
                                    }
                                    else {
                                        alert(localStorage.getItem("ERROR_B08_Add_002"));
                                        $("#TextBox3").val("");
                                        $("#TextBox3").focus();
                                    }
                                }
                            }
                        }
                    }
                    $("#BackGround").css("display", "none");
                }).catch((err) => {
                    alert(localStorage.getItem("ERRORAnErrorOccurred"));
                    $("#BackGround").css("display", "none");
                })
            });
        }
    }
}
function Buttonsave_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/B08/Buttonsave_Click";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
function Buttondelete_Click() {
    if (TagIndex == 4) {
        let IsDelete = false;
        if (BaseResult) {
            if (BaseResult.T_DGV_02) {
                if (BaseResult.T_DGV_02.length > 0) {
                    IsDelete = true;
                }
            }
        }
        if (IsDelete == true) {
            if (confirm(localStorage.getItem("Notification_B08_001"))) {
                $("#BackGround").css("display", "block");
                let BaseParameter = new Object();
                BaseParameter = {
                }
                BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
                BaseParameter.T_DGV_02 = BaseResult.T_DGV_02;
                let formUpload = new FormData();
                formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
                let url = "/B08/Buttondelete_Click";

                fetch(url, {
                    method: "POST",
                    body: formUpload,
                    headers: {
                    }
                }).then((response) => {
                    response.json().then((data) => {
                        Buttonfind_Click();
                        $("#BackGround").css("display", "none");
                    }).catch((err) => {
                        $("#BackGround").css("display", "none");
                    })
                });
            }
        }
    }

}
function Buttoncancel_Click() {

}
function Buttoninport_Click() {

}
function Buttonexport_Click() {
    if (TagIndex == 5) {
        TableHTMLToExcel("IN_LIST_DGVTable", "B08_IN_LIST", "B08_IN_LIST")
    }
}
function Buttonprint_Click() {
    localStorage.setItem("B08_REPRINT_Label4", BaseResult.DataGridView1[DataGridView1RowIndex].TTC_PO_INX);
    let url = "/B08_REPRINT";
    OpenWindowByURL(url, 800, 460);

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
    if (BaseResult) {
        if (BaseResult.DataGridView1) {
            if (BaseResult.DataGridView1.length > 0) {
                for (let i = 0; i < BaseResult.DataGridView1.length; i++) {
                    HTML = HTML + "<tr>";
                    if (BaseResult.DataGridView1[i].CHK) {
                        HTML = HTML + "<td><label><input id='DataGridView1CHK" + i + "' class='form-check-input' type='checkbox' checked onclick='DataGridView1CHKChanged(" + i + ")'><span></span></label></td>";
                    }
                    else {
                        HTML = HTML + "<td><label><input id='DataGridView1CHK" + i + "' class='form-check-input' type='checkbox' onclick='DataGridView1CHKChanged(" + i + ")'><span></span></label></td>";
                    }
                    HTML = HTML + "<td onclick='DataGridView1_CellClick(" + i + ")'><button class='btn waves-effect waves-light grey darken-1'>" + BaseResult.DataGridView1[i].CONDITION + "</button></td>";
                    switch (BaseResult.DataGridView1[i].TTC_ENG) {
                        case "N":
                            HTML = HTML + "<td class='green darken-4' ondblclick='DataGridView1_DoubleClick(" + i + ")' style='width: 50px; color: white;'>" + BaseResult.DataGridView1[i].TTC_ENG + "</td>";
                            break;
                        case "E":
                            HTML = HTML + "<td class='yellow darken-4' ondblclick='DataGridView1_DoubleClick(" + i + ")' style='width: 50px; color: white;'>" + BaseResult.DataGridView1[i].TTC_ENG + "</td>";
                            break;
                        case "S":
                            HTML = HTML + "<td class='red darken-4' ondblclick='DataGridView1_DoubleClick(" + i + ")' style='width: 50px; color: white;'>" + BaseResult.DataGridView1[i].TTC_ENG + "</td>";
                            break;
                    }
                    HTML = HTML + "<td>" + new Date(BaseResult.DataGridView1[i].TTC_PO_DT.replace(/(\d{2})-(\d{2})-(\d{4})/, "$2/$1/$3")).toISOString().slice(0, 10) + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].TC_PART_NM + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].TC_DESC + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].TTC_PO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].PERFORMN + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].RAW_PART_NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].TC_SIZE + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].TC_MC + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].TC_PACKUNIT + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].TC_LOC + "</td>";
                    HTML = HTML + "</tr>";
                }
            }
        }
    }
    document.getElementById("DataGridView1").innerHTML = HTML;
}
function DataGridView1Sort() {
    IsTableSort = !IsTableSort;
    DataGridViewSort(BaseResult.DataGridView1, IsTableSort);
    DataGridView1Render();
}
function DataGridView2Render() {
    let HTML = "";
    if (BaseResult) {
        if (BaseResult.DataGridView2) {
            if (BaseResult.DataGridView2.length > 0) {
                for (let i = 0; i < BaseResult.DataGridView2.length; i++) {
                    if (i == 0) {
                        HTML = HTML + "<tr style='color: green;'>";
                    }
                    else {
                        HTML = HTML + "<tr>";
                    }
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].RACKCODE + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].TCC_PARTNO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].BARCODE_NM + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].RACKDTM + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].IN_USER + "</td>";
                    HTML = HTML + "</tr>";
                }
            }
        }
    }
    document.getElementById("DataGridView2").innerHTML = HTML;
}
function DataGridView2Sort() {
    IsTableSort = !IsTableSort;
    DataGridViewSort(BaseResult.DataGridView2, IsTableSort);
    DataGridView2Render();
}
function DataGridView3Render() {
    let HTML = "";
    if (BaseResult) {
        if (BaseResult.DataGridView3) {
            if (BaseResult.DataGridView3.length > 0) {
                for (let i = 0; i < BaseResult.DataGridView3.length; i++) {
                    HTML = HTML + "<tr>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView3[i].RACKCODE + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView3[i].TCC_PARTNO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView3[i].BARCODE_NM + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView3[i].RACKDTM + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView3[i].IN_USER + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView3[i].RACKOUT_DTM + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView3[i].OUT_USER + "</td>";
                    HTML = HTML + "</tr>";
                }
            }
        }
    }
    document.getElementById("DataGridView3").innerHTML = HTML;
}
function DataGridView3Sort() {
    IsTableSort = !IsTableSort;
    DataGridViewSort(BaseResult.DataGridView3, IsTableSort);
    DataGridView3Sort();
}
function T_DGV_01Render() {
    let HTML = "";
    if (BaseResult) {
        if (BaseResult.T_DGV_01) {
            if (BaseResult.T_DGV_01.length > 0) {
                $("#TLB_05").val(BaseResult.T_DGV_01[0].PART_NO);
                $("#TLB_07").val(BaseResult.T_DGV_01[0].PART_NAME);
                $("#TLB_09").val(BaseResult.T_DGV_01[0].OUT_STOCK);
                for (let i = 0; i < BaseResult.T_DGV_01.length; i++) {
                    HTML = HTML + "<tr>";
                    HTML = HTML + "<td>" + BaseResult.T_DGV_01[i].NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.T_DGV_01[i].CREATE_DTM + "</td>";
                    HTML = HTML + "<td>" + BaseResult.T_DGV_01[i].PART_NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.T_DGV_01[i].PART_NAME + "</td>";
                    HTML = HTML + "<td>" + BaseResult.T_DGV_01[i].IN_STOCK + "</td>";
                    HTML = HTML + "<td>" + BaseResult.T_DGV_01[i].OUT_STOCK + "</td>";
                    HTML = HTML + "<td>" + BaseResult.T_DGV_01[i].STOCK + "</td>";
                    HTML = HTML + "</tr>";
                }
            }
        }
    }
    document.getElementById("T_DGV_01").innerHTML = HTML;
}
function T_DGV_01Sort() {
    IsTableSort = !IsTableSort;
    DataGridViewSort(BaseResult.T_DGV_01, IsTableSort);
    T_DGV_01Render();
}
function T_DGV_02Render() {
    let HTML = "";
    if (BaseResult) {
        if (BaseResult.T_DGV_02) {
            if (BaseResult.T_DGV_02.length > 0) {
                for (let i = 0; i < BaseResult.T_DGV_02.length; i++) {
                    HTML = HTML + "<tr>";
                    if (BaseResult.DataGridView1[i].CHK) {
                        HTML = HTML + "<td><label><input id='T_DGV_02CHK" + i + "' class='form-check-input' type='checkbox' checked onclick='T_DGV_02CHKChanged(" + i + ")'><span></span></label></td>";
                    }
                    else {
                        HTML = HTML + "<td><label><input id='T_DGV_02CHK" + i + "' class='form-check-input' type='checkbox' onclick='T_DGV_02CHKChanged(" + i + ")'><span></span></label></td>";
                    }
                    HTML = HTML + "<td>" + BaseResult.T_DGV_02[i].NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.T_DGV_02[i].RACKCODE + "</td>";
                    HTML = HTML + "<td>" + BaseResult.T_DGV_02[i].BARCODE_NM + "</td>";
                    HTML = HTML + "<td>" + BaseResult.T_DGV_02[i].QTY + "</td>";
                    HTML = HTML + "<td>" + BaseResult.T_DGV_02[i].RACKDTM + "</td>";
                    HTML = HTML + "<td>" + BaseResult.T_DGV_02[i].IN_USER + "</td>";
                    HTML = HTML + "<td>" + BaseResult.T_DGV_02[i].RACKOUT_DTM + "</td>";
                    HTML = HTML + "<td>" + BaseResult.T_DGV_02[i].OUT_USER + "</td>";
                    HTML = HTML + "</tr>";
                }
            }
        }
    }
    document.getElementById("T_DGV_02").innerHTML = HTML;
}
function T_DGV_02Sort() {
    IsTableSort = !IsTableSort;
    DataGridViewSort(BaseResult.T_DGV_02, IsTableSort);
    T_DGV_02Render();
}
function IN_LIST_DGVRender() {
    let HTML = "";
    if (BaseResult) {
        if (BaseResult.IN_LIST_DGV) {
            if (BaseResult.IN_LIST_DGV.length > 0) {
                for (let i = 0; i < BaseResult.IN_LIST_DGV.length; i++) {
                    HTML = HTML + "<tr>";
                    HTML = HTML + "<td>" + BaseResult.IN_LIST_DGV[i].IN_USERNAME + "</td>";
                    HTML = HTML + "<td>" + BaseResult.IN_LIST_DGV[i].DATE + "</td>";
                    HTML = HTML + "<td>" + BaseResult.IN_LIST_DGV[i].TIME + "</td>";
                    HTML = HTML + "<td>" + BaseResult.IN_LIST_DGV[i].TC_PART_NM + "</td>";
                    HTML = HTML + "<td>" + BaseResult.IN_LIST_DGV[i].TC_DESC + "</td>";
                    HTML = HTML + "<td>" + BaseResult.IN_LIST_DGV[i].QTY + "</td>";
                    HTML = HTML + "<td>" + BaseResult.IN_LIST_DGV[i].TC_SIZE + "</td>";
                    HTML = HTML + "<td>" + BaseResult.IN_LIST_DGV[i].Meter + "</td>";
                    HTML = HTML + "<td>" + BaseResult.IN_LIST_DGV[i].TC_W_S + "</td>";
                    HTML = HTML + "<td>" + BaseResult.IN_LIST_DGV[i].TC_W_MS + "</td>";
                    HTML = HTML + "<td>" + BaseResult.IN_LIST_DGV[i].MIN + "</td>";
                    HTML = HTML + "</tr>";
                }
            }
        }
    }
    document.getElementById("IN_LIST_DGV").innerHTML = HTML;
}
function IN_LIST_DGVSort() {
    IsTableSort = !IsTableSort;
    DataGridViewSort(BaseResult.IN_LIST_DGV, IsTableSort);
    IN_LIST_DGVRender();
}
function OUT_LIST_DGVRender() {
    let HTML = "";
    if (BaseResult) {
        if (BaseResult.OUT_LIST_DGV) {
            if (BaseResult.OUT_LIST_DGV.length > 0) {
                for (let i = 0; i < BaseResult.OUT_LIST_DGV.length; i++) {
                    HTML = HTML + "<tr>";
                    HTML = HTML + "<td>" + BaseResult.OUT_LIST_DGV[i].OUT_USERNAME + "</td>";
                    HTML = HTML + "<td>" + BaseResult.OUT_LIST_DGV[i].DATE + "</td>";
                    HTML = HTML + "<td>" + BaseResult.OUT_LIST_DGV[i].TIME + "</td>";
                    HTML = HTML + "<td>" + BaseResult.OUT_LIST_DGV[i].TC_PART_NM + "</td>";
                    HTML = HTML + "<td>" + BaseResult.OUT_LIST_DGV[i].TC_DESC + "</td>";
                    HTML = HTML + "<td>" + BaseResult.OUT_LIST_DGV[i].QTY + "</td>";
                    HTML = HTML + "</tr>";
                }
            }
        }
    }
    document.getElementById("OUT_LIST_DGV").innerHTML = HTML;
}
function OUT_LIST_DGVSort() {
    IsTableSort = !IsTableSort;
    DataGridViewSort(BaseResult.OUT_LIST_DGV, IsTableSort);
    OUT_LIST_DGVRender();
}
function DataGridView1CHKChanged(i) {
    let id = "DataGridView1CHK" + i;
    DataGridView1RowIndex = i;
    BaseResult.DataGridView1[DataGridView1RowIndex].CHK = $("#" + id).is(":checked");
}
function T_DGV_02CHKChanged(i) {
    let id = "T_DGV_02CHK" + i;
    T_DGV_02RowIndex = i;
    BaseResult.T_DGV_02[T_DGV_02RowIndex].CHK = $("#" + id).is(":checked");
}
function DataGridView1_CellClick(i) {
    let IsCheck = true;
    DataGridView1RowIndex = i;
    let CHKA = BaseResult.DataGridView1[DataGridView1RowIndex].CONDITION;
    let ENGT = BaseResult.DataGridView1[DataGridView1RowIndex].TTC_ENG;
    if (CHKA == "Working") {
        IsCheck = false;
    }
    if (IsCheck == true) {
        $("#BackGround").css("display", "block");
        let AAA = BaseResult.DataGridView1[DataGridView1RowIndex].TTC_PO_DT;
        let BBB = BaseResult.DataGridView1[DataGridView1RowIndex].TC_PART_NM;
        let CCC = BaseResult.DataGridView1[DataGridView1RowIndex].TC_DESC;
        let DDD = BaseResult.DataGridView1[DataGridView1RowIndex].TTC_PO;
        let FFF = BaseResult.DataGridView1[DataGridView1RowIndex].PERFORMN;
        let GGG = BaseResult.DataGridView1[DataGridView1RowIndex].RAW_PART_NO;
        let HHH = BaseResult.DataGridView1[DataGridView1RowIndex].TC_SIZE;
        let LLL = BaseResult.DataGridView1[DataGridView1RowIndex].TC_MC;
        let KKK = BaseResult.DataGridView1[DataGridView1RowIndex].TC_PACKUNIT;
        let MMM = BaseResult.DataGridView1[DataGridView1RowIndex].TC_LOC;
        let OOO = BaseResult.DataGridView1[DataGridView1RowIndex].TTC_PART_IDX;
        let PPP = BaseResult.DataGridView1[DataGridView1RowIndex].TTC_PO_INX;
        let BaseParameter = new Object();
        BaseParameter = {
            Action: TagIndex,
            ListSearchString: [],
        }
        BaseParameter.SuperResultTranfer = BaseResult.DataGridView1[DataGridView1RowIndex];
        BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
        BaseParameter.ListSearchString.push(AAA);
        BaseParameter.ListSearchString.push(BBB);
        BaseParameter.ListSearchString.push(CCC);
        BaseParameter.ListSearchString.push(DDD);
        BaseParameter.ListSearchString.push(FFF);
        BaseParameter.ListSearchString.push(GGG);
        BaseParameter.ListSearchString.push(HHH);
        BaseParameter.ListSearchString.push(LLL);
        BaseParameter.ListSearchString.push(KKK);
        BaseParameter.ListSearchString.push(MMM);
        BaseParameter.ListSearchString.push(OOO);
        BaseParameter.ListSearchString.push(PPP);
        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        let url = "/B08/DataGridView1_CellClick";

        fetch(url, {
            method: "POST",
            body: formUpload,
            headers: {
            }
        }).then((response) => {
            response.json().then((data) => {
                alert(localStorage.getItem("SaveSuccess"));
                let BaseResultDataGridView1_CellClick = data;                
                if (BaseResultDataGridView1_CellClick) {
                    if (BaseResultDataGridView1_CellClick.Code) {
                        let url = BaseResultDataGridView1_CellClick.Code;                        
                        OpenWindowByURL(url, 1000, 200);
                    }
                }
                Buttonfind_Click();
                $("#BackGround").css("display", "none");
            }).catch((err) => {
                alert(localStorage.getItem("SaveNotSuccess"));
                $("#BackGround").css("display", "none");
            })
        });
    }
}
function DataGridView1_DataBindingComplete() {

}
function DataGridView1_DoubleClick(i) {
    DataGridView1RowIndex = i;
    localStorage.setItem("B08_1_CHK_NO", BaseResult.DataGridView1[DataGridView1RowIndex].TTC_PO_INX);
    localStorage.setItem("B08_1_PART_NO_LB", BaseResult.DataGridView1[DataGridView1RowIndex].TC_PART_NM);
    localStorage.setItem("B08_1_STAY_LB", BaseResult.DataGridView1[DataGridView1RowIndex].TTC_ENG);
    let url = "/B08_1";
    OpenWindowByURL(url, 800, 600);
    localStorage.setItem("B08_1_Close", 0);
    StartInterval();
}
function StartInterval() {
    IntervalTimer = setInterval(function () {
        let IsB08_1Close = localStorage.getItem("B08_1_Close");
        if (IsB08_1Close == 1) {
            clearInterval(IntervalTimer);
            IsB08_1Close = 0;
            localStorage.setItem("B08_1_Close", IsB08_1Close);
            Buttonfind_Click();
        }
    }, 100);
}
