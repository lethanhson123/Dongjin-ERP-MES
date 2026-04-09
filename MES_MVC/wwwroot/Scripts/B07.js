let IsTableSort = false;
let BaseResult = new Object();
let TagIndex = 1;
let DataGridView3RowIndex = 0;
let DataGridView4RowIndex = 0;
let DataGridView5RowIndex = 0;
let FROM_NM;
$(document).ready(function () {
    var now = new Date();
    var day = ("0" + now.getDate()).slice(-2);
    var month = ("0" + (now.getMonth() + 1)).slice(-2);
    var today = now.getFullYear() + "-" + (month) + "-" + (day);
    $("#DateTimePicker1").val(today);
    localStorage.setItem("B07_Label4", "");
    localStorage.setItem("B07_TextBox1", "");
    localStorage.setItem("B07_TextBox5", "");
    BaseResult.DataGridView2 = new Object();
    BaseResult.DataGridView2 = [];
});
function StartInterval() {
    setInterval(function () {
        //let Label4 = localStorage.getItem("B07_Label4");
        //if (Label4.length > 0) {
        //    $("#Label4").val(Label4);
        //    localStorage.setItem("B07_Label4", "");
        //}
        let TextBox1 = localStorage.getItem("B07_TextBox1");
        if (TextBox1.length > 0) {
            $("#TextBox1").val(TextBox1);
            localStorage.setItem("B07_TextBox1", "");
        }
        let TextBox5 = localStorage.getItem("B07_TextBox5");
        if (TextBox5.length > 0) {
            $("#TextBox5").val(TextBox5);
            localStorage.setItem("B07_TextBox5", "");
        }
    }, 100);
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
    if (TagIndex == 2) {
        $("#BackGround").css("display", "block");
        let BaseParameter = new Object();
        BaseParameter = {
            Action: TagIndex,
            ListSearchString: [],
        }
        BaseParameter.SearchString = $("#TextBox10").val();
        BaseParameter.ListSearchString.push($("#TextBox11").val());
        BaseParameter.ListSearchString.push($("#SuchBB").val());
        BaseParameter.ListSearchString.push($("#SuchCC").val());
        BaseParameter.ListSearchString.push($("#SuchDD").val());
        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        let url = "/B07/Buttonfind_Click";

        fetch(url, {
            method: "POST",
            body: formUpload,
            headers: {
            }
        }).then((response) => {
            response.json().then((data) => {
                BaseResult = data;
                if (TagIndex == 2) {
                    DataGridView3Render();
                }
                if (TagIndex == 3) {
                    DataGridView5Render();
                }
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
        BaseParameter.SearchString = $("#TextBox10").val();
        BaseParameter.ListSearchString.push($("#TextBox11").val());
        BaseParameter.ListSearchString.push($("#SuchBB").val());
        BaseParameter.ListSearchString.push($("#SuchCC").val());
        BaseParameter.ListSearchString.push($("#SuchDD").val());
        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        let url = "/B07/Buttonfind_Click";

        fetch(url, {
            method: "POST",
            body: formUpload,
            headers: {
            }
        }).then((response) => {
            response.json().then((data) => {
                BaseResult = data;
                if (TagIndex == 2) {
                    DataGridView3Render();
                }
                if (TagIndex == 3) {
                    DataGridView5Render();
                }
                $("#BackGround").css("display", "none");
            }).catch((err) => {
                $("#BackGround").css("display", "none");
            })
        });
    }
}
function Buttonadd_Click() {
    if (TagIndex == 1) {
        let TextBox1 = $("#TextBox1").val();
        if (TextBox1.length > 0) {
            $("#BackGround").css("display", "block");
            let BaseParameter = new Object();
            BaseParameter = {
                Action: TagIndex,
                SearchString: TextBox1,
            }
            let formUpload = new FormData();
            formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
            let url = "/B07/Buttonadd_Click";

            fetch(url, {
                method: "POST",
                body: formUpload,
                headers: {
                }
            }).then((response) => {
                response.json().then((data) => {
                    let BaseResultButtonadd = data;
                    if (BaseResultButtonadd) {
                        if (BaseResultButtonadd.DataGridView2) {
                            if (BaseResultButtonadd.DataGridView2.length > 0) {
                                console.log(BaseResultButtonadd.DataGridView2[0].TTC_PART_IDX);
                                let DataGridView2Item = {
                                    CHK: true,
                                    ORDER_DATE: $("#DateTimePicker1").val(),
                                    CUT_ORDER: $("#TextBox2").val(),
                                    TTC_PART_IDX: BaseResultButtonadd.DataGridView2[0].TTC_PART_IDX,
                                    TC_PART_NM: BaseResultButtonadd.DataGridView2[0].TC_PART_NM,
                                    TC_DESC: BaseResultButtonadd.DataGridView2[0].TC_DESC,
                                    RAW_PART_NO: BaseResultButtonadd.DataGridView2[0].RAW_PART_NO,
                                    TC_SIZE: BaseResultButtonadd.DataGridView2[0].TC_SIZE,
                                    TC_MC: BaseResultButtonadd.DataGridView2[0].TC_MC,
                                    TC_PACKUNIT: BaseResultButtonadd.DataGridView2[0].TC_PACKUNIT,
                                    TC_LOC: BaseResultButtonadd.DataGridView2[0].TC_LOC,
                                }
                                BaseResult.DataGridView2.push(DataGridView2Item);
                            }
                        }
                    }
                    DataGridView2Render();
                    $("#BackGround").css("display", "none");
                }).catch((err) => {
                    $("#BackGround").css("display", "none");
                })
            });
        }
    }
    if (TagIndex == 2) {
        $("#TextBox3").val("");
        document.getElementById("TextBox3").readOnly = false;
        $("#TextBox4").val("");
        $("#TextBox5").val("");
        $("#TextBox6").val("");
        $("#TextBox7").val("");
        $("#TextBox8").val("");
        $("#TextBox9").val("");
        $("#TextBox12").val("1");
        $("#TextBox16").val("10");
        $("#Label4").val("New");
        localStorage.setItem("B07_Label4", "New");
    }
}
function Buttonsave_Click() {
    if (TagIndex == 1) {
        if (BaseResult) {
            if (BaseResult.DataGridView2) {
                if (BaseResult.DataGridView2.length > 0) {
                    $("#BackGround").css("display", "block");
                    let BaseParameter = new Object();
                    BaseParameter = {
                        Action: TagIndex,
                    }
                    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
                    BaseParameter.DataGridView2 = BaseResult.DataGridView2;
                    let formUpload = new FormData();
                    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
                    let url = "/B07/Buttonsave_Click";

                    fetch(url, {
                        method: "POST",
                        body: formUpload,
                        headers: {
                        }
                    }).then((response) => {
                        response.json().then((data) => {
                            let BaseResultButtonsave = data
                            BaseResult.DataGridView2 = BaseResultButtonsave.DataGridView2;
                            DataGridView2Render();
                            $("#TextBox1").val("");
                            $("#TextBox2").val("");
                            alert(localStorage.getItem("SaveSuccess"));
                            $("#BackGround").css("display", "none");
                        }).catch((err) => {
                            alert(localStorage.getItem("SaveNotSuccess"));
                            $("#BackGround").css("display", "none");
                        })
                    });
                }
            }
        }
    }
    if (TagIndex == 2) {
        let IsSave = true;
        let Label4 = $("#Label4").val();
        if (Label4 == "") {
            IsSave = false;
        }
        if (Label4 != "New") {
            IsSave = false;
            alert("다시 확인 바랍니다. Vui lòng kiểm tra lại.");
        }
        if (IsSave == true) {
            let AAA = $("#TextBox3").val();
            let BBB = $("#TextBox4").val();
            let CCC = $("#TextBox5").val();
            let DDD = $("#TextBox6").val();
            let EEE = $("#TextBox7").val();
            let FFF = $("#TextBox8").val();
            let GGG = $("#TextBox9").val();
            let HHH = Label4;
            let KKK = $("#TextBox12").val();
            let NNN = $("#TextBox16").val();
            if (Label4 == "New") {
                let TextBox5 = $("#TextBox5").val();
                if (TextBox5 == "") {
                    alert(localStorage.getItem("ERRORCheck"));
                    IsSave = false;
                }
            }
            if (IsSave == true) {
                $("#BackGround").css("display", "block");
                let BaseParameter = new Object();
                BaseParameter = {
                    Action: TagIndex,
                    SearchString: Label4,
                    ListSearchString: [],
                }
                BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
                BaseParameter.ListSearchString.push(AAA);
                BaseParameter.ListSearchString.push(BBB);
                BaseParameter.ListSearchString.push(CCC);
                BaseParameter.ListSearchString.push(DDD);
                BaseParameter.ListSearchString.push(EEE);
                BaseParameter.ListSearchString.push(FFF);
                BaseParameter.ListSearchString.push(GGG);
                BaseParameter.ListSearchString.push(HHH);
                BaseParameter.ListSearchString.push(KKK);
                BaseParameter.ListSearchString.push(NNN);
                let formUpload = new FormData();
                formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
                let url = "/B07/Buttonsave_Click";

                fetch(url, {
                    method: "POST",
                    body: formUpload,
                    headers: {
                    }
                }).then((response) => {
                    response.json().then((data) => {
                        let BaseResultButtonsave = data
                        alert(localStorage.getItem("SaveSuccess"));
                        $("#Label4").val("");
                        Button1_Click();
                        $("#BackGround").css("display", "none");
                    }).catch((err) => {
                        alert(localStorage.getItem("SaveNotSuccess"));
                        $("#BackGround").css("display", "none");
                    })
                });
            }
        }
    }
    if (TagIndex == 3) {
        let BOM_IDX = BaseResult.DataGridView4[DataGridView4RowIndex].BOM_IDX;
        let Text2 = $("#Text2").val();
        $("#BackGround").css("display", "block");
        let BaseParameter = new Object();
        BaseParameter = {
            Action: TagIndex,
            ListSearchString: [],
        }
        BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
        BaseParameter.ListSearchString.push(BOM_IDX);
        BaseParameter.ListSearchString.push(Text2);
        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        let url = "/B07/Buttonsave_Click";

        fetch(url, {
            method: "POST",
            body: formUpload,
            headers: {
            }
        }).then((response) => {
            response.json().then((data) => {
                let BaseResultButtonsave = data
                alert(localStorage.getItem("SaveSuccess"));
                DGV_BOM_LD();
                $("#BackGround").css("display", "none");
            }).catch((err) => {
                alert(localStorage.getItem("SaveNotSuccess"));
                $("#BackGround").css("display", "none");
            })
        });
    }
}
function Buttondelete_Click() {
    if (TagIndex == 3) {
        if (BaseResult) {
            if (BaseResult.DataGridView4) {
                if (BaseResult.DataGridView4.length > 0) {
                    let BOM_IDX = BaseResult.DataGridView4[DataGridView4RowIndex].BOM_IDX;
                    let DeleteConfirm = localStorage.getItem("DeleteConfirm");
                    if (confirm(DeleteConfirm)) {
                        $("#BackGround").css("display", "block");
                        let BaseParameter = new Object();
                        BaseParameter = {
                            Action: TagIndex,
                            SearchString: BOM_IDX,
                        }
                        BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
                        let formUpload = new FormData();
                        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
                        let url = "/B07/Buttondelete_Click";
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
                        alert("정상처리 되었습니다. Đã được lưu.");
                        DGV_BOM_LD();
                    }
                }
            }
        }
    }
}
function Buttoncancel_Click() {
    if (TagIndex == 1) {
        if (BaseResult) {
            if (BaseResult.DataGridView1) {
                BaseResult.DataGridView1 = [];
                DataGridView1Render();
            }
        }
        document.getElementById("Button1").disabled = true;
        $("#TextBox1").val("");
        $("#TextBox2").val("");
    }
    if (TagIndex == 2) {
        $("#TextBox3").val("");
        $("#TextBox4").val("");
        $("#TextBox5").val("");
        $("#TextBox6").val("");
        $("#TextBox7").val("");
        $("#TextBox8").val("");
        $("#TextBox9").val("");
        $("#Label4").val("");
        localStorage.setItem("B07_Label4", "");
    }
}
function Buttoninport_Click() {
    if (TagIndex == 1) {
        $("#FileToUpload").click();
    }
    if (TagIndex == 2) {
        OpenB07_3();
    }
}
$("#FileToUpload").change(function () {
    Buttoninport_ClickSub();
});
function Buttoninport_ClickSub() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
    }
    var FileToUpload = $('#FileToUpload').prop('files');
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    if (FileToUpload) {
        if (FileToUpload.length > 0) {
            for (var i = 0; i < FileToUpload.length; i++) {
                formUpload.append('file[]', FileToUpload[i]);
            }
        }
    }
    let url = "/B07/Buttoninport_Click";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            BaseResult = data;
            DataGridView1Render();
            alert(localStorage.getItem("ExcelImportSuccess"));
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            alert(localStorage.getItem("SaveNotSuccess"));
            $("#BackGround").css("display", "none");
        })
    });
}
function Buttonexport_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/B07/Buttonexport_Click";

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
function Buttonprint_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/B07/Buttonprint_Click";

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
function Buttonhelp_Click() {
    let url = "/WMP_PLAY";
    OpenWindowByURL(url, 800, 460);
}
function Buttonclose_Click() {
    history.back();
}
$("#RadioButton1").click(function () {
    RadioButton2_Click();
});
$("#RadioButton2").click(function () {
    RadioButton2_Click();
});
function RadioButton2_Click() {
    let RadioButton2 = $("#RadioButton2").val();
    if (RadioButton2 == true) {
        if (BaseResult) {
            if (BaseResult.DataGridView1) {
                if (BaseResult.DataGridView1.length > 0) {
                    for (let i = 0; i < BaseResult.DataGridView1.length; i++) {
                        BaseResult.DataGridView1[i].CHK = RadioButton2;
                    }
                    DataGridView1Render();
                }
            }
        }
    }
    let RadioButton1 = $("#RadioButton1").val();
    if (RadioButton1 == true) {
        if (BaseResult) {
            if (BaseResult.DataGridView1) {
                if (BaseResult.DataGridView1.length > 0) {
                    for (let i = 0; i < BaseResult.DataGridView1.length; i++) {
                        BaseResult.DataGridView1[i].CHK = !BaseResult.DataGridView1[i].CHK;
                    }
                    DataGridView1Render();
                }
            }
        }
    }
}
$("#rbchk2").click(function () {
    rbchk1_Click();
});
$("#rbchk1").click(function () {
    rbchk1_Click();
});
function rbchk1_Click() {
    let rbchk1 = $("#rbchk1").val();
    if (rbchk1 == true) {
        if (BaseResult) {
            if (BaseResult.DataGridView2) {
                if (BaseResult.DataGridView2.length > 0) {
                    for (let i = 0; i < BaseResult.DataGridView2.length; i++) {
                        BaseResult.DataGridView2[i].CHK = rbchk1;
                    }
                    DataGridView2Render();
                }
            }
        }
    }
    let rbchk2 = $("#rbchk2").val();
    if (rbchk2 == true) {
        if (BaseResult) {
            if (BaseResult.DataGridView2) {
                if (BaseResult.DataGridView2.length > 0) {
                    for (let i = 0; i < BaseResult.DataGridView2.length; i++) {
                        BaseResult.DataGridView2[i].CHK = !BaseResult.DataGridView2[i].CHK;
                    }
                    DataGridView2Render();
                }
            }
        }
    }
}
$("#TextBox1").click(function () {
    TextBox1_Click();
});
function TextBox1_Click() {
    FROM_NM = "TAB_1";
    OpenB07_1();
}
$("#Button1").click(function () {
    Button1_Click();
});
function Button1_Click() {
    if (BaseResult) {
        if (BaseResult.DataGridView1) {
            if (BaseResult.DataGridView1.length > 0) {
                BaseResult.DataGridView2 = [];
                DataGridView2Render();
                $("#BackGround").css("display", "block");
                let BaseParameter = new Object();
                BaseParameter = {
                }
                BaseParameter.DataGridView1 = BaseResult.DataGridView1;
                let formUpload = new FormData();
                formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
                let url = "/B07/Button1_Click";

                fetch(url, {
                    method: "POST",
                    body: formUpload,
                    headers: {
                    }
                }).then((response) => {
                    response.json().then((data) => {
                        let BaseResultButton1 = data;
                        BaseResult.DataGridView2 = BaseResultButton1.DataGridView2;
                        DataGridView2Render();
                        document.getElementById("Button1").disabled = false;
                        $("#BackGround").css("display", "none");
                    }).catch((err) => {
                        $("#BackGround").css("display", "none");
                    })
                });
            }
        }
    }
}
$("#TextBox5").click(function () {
    TextBox5_DoubleClick();
});
function TextBox5_DoubleClick() {
    OpenB07_2();
}
$("#Button2").click(function () {
    Button2_Click();
});
function Button2_Click() {
    OpenB07_2();
}
$("#Text2").click(function () {
    Text2_KeyPress();
});
function Text2_KeyPress() {

}
$("#Button4").click(function () {
    Button4_Click();
});
function Button4_Click() {
    FROM_NM = "TAB_3";
    OpenB07_1();
}
function OpenB07_2() {
    let url = "/B07_2";
    OpenWindowByURL(url, 800, 400);
    StartInterval();
}
function OpenB07_3() {
    let url = "/B07_3";
    OpenWindowByURL(url, 800, 400);
}
function OpenB07_1() {
    localStorage.setItem("B07_1_FROM_NM", FROM_NM);
    let url = "/B07_1";
    OpenWindowByURL(url, 800, 400);
    StartInterval();
}
function DataGridView1Render() {
    let HTML = "";
    if (BaseResult) {
        if (BaseResult.DataGridView1) {
            if (BaseResult.DataGridView1.length > 0) {
                for (let i = 0; i < BaseResult.DataGridView1.length; i++) {
                    HTML = HTML + "<tr>";
                    if (BaseResult.DataGridView1[i].CHK) {
                        HTML = HTML + "<td><label><input id='DataGridView1CHK" + i + "' class='form-check-input' type='checkbox' checked><span></span></label></td>";
                    }
                    else {
                        HTML = HTML + "<td><label><input id='DataGridView1CHK" + i + "' class='form-check-input' type='checkbox'><span></span></label></td>";
                    }
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].DATEString + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].Tube_Cutting_Part_No + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].CUT_ORDER + "</td>";
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
                    HTML = HTML + "<tr>";
                    if (BaseResult.DataGridView2[i].CHK) {
                        HTML = HTML + "<td><input id='DataGridView2CHK" + i + "' class='form-check-input' type='checkbox' checked><span></span></td>";
                    }
                    else {
                        HTML = HTML + "<td><input id='DataGridView2CHK" + i + "' class='form-check-input' type='checkbox'><span></span></td>";
                    }
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].ORDER_DATE + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].TC_PART_NM + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].CUT_ORDER + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].TC_DESC + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].RAW_PART_NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].TC_SIZE + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].TC_MC + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].TC_PACKUNIT + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].TC_LOC + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].TTC_PART_IDX + "</td>";                    
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
                DataGridView3_SelectionChanged(0);
                for (let i = 0; i < BaseResult.DataGridView3.length; i++) {
                    HTML = HTML + "<tr onclick='DataGridView3_SelectionChanged(" + i + ")'>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView3[i].TC_PART_NM + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView3[i].TC_DESC + "</td>";
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
    DataGridView3Render();
}
function DataGridView3_SelectionChanged(i) {
    DataGridView3RowIndex = i;
    if (BaseResult) {
        if (BaseResult.DataGridView3) {
            $("#TextBox3").val(BaseResult.DataGridView3[DataGridView3RowIndex].TC_PART_NM);
            $("#TextBox4").val(BaseResult.DataGridView3[DataGridView3RowIndex].TC_DESC);
            $("#TextBox5").val(BaseResult.DataGridView3[DataGridView3RowIndex].RAW_PART_NO);
            $("#TextBox6").val(BaseResult.DataGridView3[DataGridView3RowIndex].TC_SIZE);
            $("#TextBox7").val(BaseResult.DataGridView3[DataGridView3RowIndex].TC_MC);
            $("#TextBox8").val(BaseResult.DataGridView3[DataGridView3RowIndex].TC_PACKUNIT);
            $("#TextBox9").val(BaseResult.DataGridView3[DataGridView3RowIndex].TC_LOC);
            $("#TextBox12").val(BaseResult.DataGridView3[DataGridView3RowIndex].TC_W_S);
            $("#TextBox16").val(BaseResult.DataGridView3[DataGridView3RowIndex].TC_W_MS);
            $("#Label4").val(BaseResult.DataGridView3[DataGridView3RowIndex].TTC_PART_IDX);
            localStorage.setItem("B07_Label4", BaseResult.DataGridView3[DataGridView3RowIndex].TTC_PART_IDX);
        }
    }
}
function DataGridView4Render() {
    let HTML = "";
    if (BaseResult) {
        if (BaseResult.DataGridView4) {
            if (BaseResult.DataGridView4.length > 0) {
                for (let i = 0; i < BaseResult.DataGridView4.length; i++) {
                    HTML = HTML + "<tr onclick='DataGridView4_SelectionChanged(" + i + ")'>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView4[i].FG_PART_NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView4[i].FG_PART_NAME + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView4[i].FG_PART_MODEL + "</td>";
                    HTML = HTML + "</tr>";
                }
            }
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
    if (BaseResult) {
        if (BaseResult.DataGridView5) {
            if (BaseResult.DataGridView5.length > 0) {
                DataGridView5_SelectionChanged(0);
                for (let i = 0; i < BaseResult.DataGridView5.length; i++) {
                    HTML = HTML + "<tr onclick='DataGridView5_SelectionChanged(" + i + ")'>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView5[i].FG_PART_NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView5[i].FG_PART_NAME + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView5[i].FG_PART_MODEL + "</td>";
                    HTML = HTML + "</tr>";
                }
            }
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
    DGV_BOM_LD();
}
function DGV_BOM_LD() {
    $("#Text1").val("");
    $("#Text2").val("");
    $("#Text4").val("");
    if (DataGridView5RowIndex > -1) {
        if (BaseResult) {
            if (BaseResult.DataGridView5) {
                let P_IDX = BaseResult.DataGridView5[DataGridView5RowIndex].PART_IDX;
                $("#BackGround").css("display", "block");
                let BaseParameter = new Object();
                BaseParameter = {
                    SearchString: P_IDX,
                }
                let formUpload = new FormData();
                formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
                let url = "/B07/DGV_BOM_LD";

                fetch(url, {
                    method: "POST",
                    body: formUpload,
                    headers: {
                    }
                }).then((response) => {
                    response.json().then((data) => {
                        let BaseResultDGV_BOM_LD = data;
                        BaseResult.DataGridView4 = BaseResultDGV_BOM_LD.DataGridView4;
                        DataGridView4Render();
                        $("#TextBox15").val(BaseResult.DataGridView5[DataGridView5RowIndex].FG_PART_NO);
                        $("#TextBox14").val(BaseResult.DataGridView5[DataGridView5RowIndex].FG_PART_NAME);
                        $("#TextBox13").val(BaseResult.DataGridView5[DataGridView5RowIndex].FG_PART_MODEL);
                        $("#TextPP").val(BaseResult.DataGridView5[DataGridView5RowIndex].PART_IDX);
                        $("#Text1").val($("#TextBox15").val());
                        $("#BackGround").css("display", "none");
                    }).catch((err) => {
                        BaseResult.DataGridView4 = [];
                        DataGridView4Render();
                        $("#BackGround").css("display", "none");
                    })
                });
            }
        }
    }
}
