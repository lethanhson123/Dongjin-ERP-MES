let IsTableSort = false;
let BaseResult = new Object();
let TagIndex = 6;
let DateNow;
let DataGridView9RowIndex = 0;
let DataGridView10RowIndex = 0;
let DataGridView11RowIndex = 0;
let DataGridView12RowIndex = 0;
let DataGridView1RowIndex = 0;
let DataGridView2RowIndex = 0;
let DataGridView5RowIndex = 0;
let DataGridView6RowIndex = 0;
let DataGridView7RowIndex = 0;
let DataGridView8RowIndex = 0;
let DataGridView3RowIndex = 0;
let DataGridView4RowIndex = 0;
let C04_1Timer;

$(document).ready(function () {
    var now = new Date();
    DateNow = DateToString(now);

    $("#DateTimePicker3").val(DateNow);
    $("#DateTimePicker4").val(DateNow);
    $("#DateTimePicker6").val(DateNow);
    $("#DateTimePicker5").val(DateNow);
    $("#DateTimePicker8").val(DateNow);
    $("#DateTimePicker7").val(DateNow);
    $("#DateTimePicker10").val(DateNow);
    $("#dateTextBox1").val(DateNow);
    $("#DateTimePicker1").val(DateNow);
    $("#DateTimePicker2").val(DateNow);

    document.getElementById("RadioButton1").checked = true;

    $("#Label45").val(0);
    $("#Label43").val(0);
    $("#Label44").val(0);
    $("#Label42").val(0);

    $("#Label37").val(0);
    $("#Label36").val(0);
    $("#Label35").val(0);
    $("#Label34").val(0);

    $("#Label30").val(0);
    $("#Label31").val(0);
    $("#Label32").val(0);
    $("#Label33").val(0);

    BaseResult.DataGridView1 = new Object();
    BaseResult.DataGridView1 = [];
    BaseResult.DataGridView2 = new Object();
    BaseResult.DataGridView2 = [];
    BaseResult.DataGridView3 = new Object();
    BaseResult.DataGridView3 = [];
    BaseResult.DataGridView4 = new Object();
    BaseResult.DataGridView4 = [];
    BaseResult.DataGridView5 = new Object();
    BaseResult.DataGridView5 = [];
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
    BaseResult.DataGridView12 = new Object();
    BaseResult.DataGridView12 = [];

});

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
$("#ATag007").click(function (e) {
    TagIndex = 7;
});
$("#ATag008").click(function (e) {
    TagIndex = 8;
});
$("#ATag009").click(function (e) {
    TagIndex = 9;
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
    if (TagIndex == 1) {
        let AAA = $("#TextBox1").val();
        let BBB = $("#TextBox2").val();
        let CCC = $("#TextBox3").val();
        let DDD = $("#dateTextBox1").val();
        let EEE = $("#ComboBox1").val();

        $("#BackGround").css("display", "block");
        let BaseParameter = new Object();
        BaseParameter = {
            Action: TagIndex,
            ListSearchString: [],
        }
        BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
        BaseParameter.USER_ID = GetCookieValue("UserID");
        BaseParameter.ListSearchString.push(AAA);
        BaseParameter.ListSearchString.push(BBB);
        BaseParameter.ListSearchString.push(CCC);
        BaseParameter.ListSearchString.push(DDD);
        BaseParameter.ListSearchString.push(EEE);
        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        let url = "/C04/Buttonfind_Click";

        fetch(url, {
            method: "POST",
            body: formUpload,
            headers: {
            }
        }).then((response) => {
            response.json().then((data) => {
                let BaseResultSub = data;
                BaseResult.DataGridView1 = BaseResultSub.DataGridView1;
                DataGridView1Render();
                $("#BackGround").css("display", "none");
            }).catch((err) => {
                $("#BackGround").css("display", "none");
            })
        });
    }
    if (TagIndex == 2) {
        BaseResult.DataGridView3 = [];
        DataGridView3Render();
        let TextBox5 = $("#TextBox5").val();
        $("#BackGround").css("display", "block");
        let BaseParameter = new Object();
        BaseParameter = {
            Action: TagIndex,
            ListSearchString: [],
        }
        BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
        BaseParameter.USER_ID = GetCookieValue("UserID");
        BaseParameter.ListSearchString.push(TextBox5);
        BaseParameter.RadioButton1 = document.getElementById("RadioButton1").checked;
        BaseParameter.RadioButton2 = document.getElementById("RadioButton2").checked;
        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        let url = "/C04/Buttonfind_Click";

        fetch(url, {
            method: "POST",
            body: formUpload,
            headers: {
            }
        }).then((response) => {
            response.json().then((data) => {
                let BaseResultSub = data;
                BaseResult.DataGridView3 = BaseResultSub.DataGridView3;
                DataGridView3Render();
                BaseResult.DataGridView4 = BaseResultSub.DataGridView4;
                DataGridView4Render();
                $("#BackGround").css("display", "none");
            }).catch((err) => {
                $("#BackGround").css("display", "none");
            })
        });
    }
    if (TagIndex == 3) {
        let AAA = $("#TextBox7").val();
        let BBB = $("#TextBox6").val();
        let CCC = $("#TextBox4").val();
        let DDD = $("#DateTimePicker1").val();
        let EEE = $("#ComboBox2").val();

        $("#BackGround").css("display", "block");
        let BaseParameter = new Object();
        BaseParameter = {
            Action: TagIndex,
            ListSearchString: [],
        }
        BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
        BaseParameter.USER_ID = GetCookieValue("UserID");
        BaseParameter.ListSearchString.push(AAA);
        BaseParameter.ListSearchString.push(BBB);
        BaseParameter.ListSearchString.push(CCC);
        BaseParameter.ListSearchString.push(DDD);
        BaseParameter.ListSearchString.push(EEE);
        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        let url = "/C04/Buttonfind_Click";

        fetch(url, {
            method: "POST",
            body: formUpload,
            headers: {
            }
        }).then((response) => {
            response.json().then((data) => {
                let BaseResultSub = data;
                BaseResult.DataGridView5 = BaseResultSub.DataGridView5;
                DataGridView5Render();
                $("#BackGround").css("display", "none");
            }).catch((err) => {
                $("#BackGround").css("display", "none");
            })
        });
    }
    if (TagIndex == 4) {
        let AAA = $("#TextBox10").val();
        let BBB = $("#TextBox9").val();
        let DDD = $("#DateTimePicker2").val();
        let EEE = $("#ComboBox3").val();

        $("#BackGround").css("display", "block");
        let BaseParameter = new Object();
        BaseParameter = {
            Action: TagIndex,
            ListSearchString: [],
        }
        BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
        BaseParameter.USER_ID = GetCookieValue("UserID");
        BaseParameter.ListSearchString.push(AAA);
        BaseParameter.ListSearchString.push(BBB);
        BaseParameter.ListSearchString.push(DDD);
        BaseParameter.ListSearchString.push(EEE);
        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        let url = "/C04/Buttonfind_Click";

        fetch(url, {
            method: "POST",
            body: formUpload,
            headers: {
            }
        }).then((response) => {
            response.json().then((data) => {
                let BaseResultSub = data;
                BaseResult.DataGridView7 = BaseResultSub.DataGridView7;
                DataGridView7Render();
                $("#BackGround").css("display", "none");
            }).catch((err) => {
                $("#BackGround").css("display", "none");
            })
        });
    }
    if (TagIndex == 6) {
        let DT1 = $("#DateTimePicker3").val();
        let DT2 = $("#DateTimePicker4").val();
        let AAAA = $("#TextBox8").val();
        let BBBB = $("#TextBox11").val();
        let CCCC = $("#TextBox16").val();

        $("#BackGround").css("display", "block");
        let BaseParameter = new Object();
        BaseParameter = {
            Action: TagIndex,
            ListSearchString: [],
        }
        BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
        BaseParameter.USER_ID = GetCookieValue("UserID");
        BaseParameter.ListSearchString.push(DT1);
        BaseParameter.ListSearchString.push(DT2);
        BaseParameter.ListSearchString.push(AAAA);
        BaseParameter.ListSearchString.push(BBBB);
        BaseParameter.ListSearchString.push(CCCC);
        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        let url = "/C04/Buttonfind_Click";

        fetch(url, {
            method: "POST",
            body: formUpload,
            headers: {
            }
        }).then((response) => {
            response.json().then((data) => {
                let BaseResultSub = data;
                BaseResult.Search = BaseResultSub.Search;
                BaseResult.DataGridView9 = BaseResultSub.DataGridView9;
                DataGridView9Render();
                try {
                    //$("#Label45").val(Number(BaseResult.Search[0].COL1).toLocaleString());
                    //$("#Label43").val(Number(BaseResult.Search[0].COL2).toLocaleString());
                    //$("#Label44").val(Number(BaseResult.Search[0].COL3).toLocaleString());
                    //$("#Label42").val(Number(BaseResult.Search[0].COL4).toLocaleString());    

                    $("#Label45").val(BaseResult.Search[0].COL1);
                    $("#Label43").val(BaseResult.Search[0].COL2);
                    $("#Label44").val(BaseResult.Search[0].COL3);
                    $("#Label42").val(BaseResult.Search[0].COL4);
                }
                catch (err) {
                    $("#Label45").val("ERROR");
                    $("#Label43").val("ERROR");
                    $("#Label44").val("ERROR");
                    $("#Label42").val("ERROR");
                }
                $("#BackGround").css("display", "none");
            }).catch((err) => {
                $("#BackGround").css("display", "none");
            })
        });
    }
    if (TagIndex == 7) {
        let DT1 = $("#DateTimePicker6").val();
        let DT2 = $("#DateTimePicker5").val();
        let AAAA = $("#TextBox13").val();
        let BBBB = $("#TextBox12").val();

        $("#BackGround").css("display", "block");
        let BaseParameter = new Object();
        BaseParameter = {
            Action: TagIndex,
            ListSearchString: [],
        }
        BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
        BaseParameter.USER_ID = GetCookieValue("UserID");
        BaseParameter.ListSearchString.push(DT1);
        BaseParameter.ListSearchString.push(DT2);
        BaseParameter.ListSearchString.push(AAAA);
        BaseParameter.ListSearchString.push(BBBB);
        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        let url = "/C04/Buttonfind_Click";

        fetch(url, {
            method: "POST",
            body: formUpload,
            headers: {
            }
        }).then((response) => {
            response.json().then((data) => {
                let BaseResultSub = data;
                BaseResult.Search = BaseResultSub.Search;
                BaseResult.DataGridView10 = BaseResultSub.DataGridView10;
                DataGridView10Render();
                try {
                    $("#Label37").val(BaseResult.Search[0].COL1);
                    $("#Label36").val(BaseResult.Search[0].COL2);
                    $("#Label35").val(BaseResult.Search[0].COL3);
                    $("#Label34").val(BaseResult.Search[0].COL4);
                }
                catch (err) {
                    $("#Label37").val("ERROR");
                    $("#Label36").val("ERROR");
                    $("#Label35").val("ERROR");
                    $("#Label34").val("ERROR");
                }
                $("#BackGround").css("display", "none");
            }).catch((err) => {
                $("#BackGround").css("display", "none");
            })
        });
    }
    if (TagIndex == 8) {
        let DT1 = $("#DateTimePicker8").val();
        let DT2 = $("#DateTimePicker7").val();
        let BBBB = $("#TextBox18").val();
        let CCCC = $("#TextBox15").val();

        $("#BackGround").css("display", "block");
        let BaseParameter = new Object();
        BaseParameter = {
            Action: TagIndex,
            ListSearchString: [],
        }
        BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
        BaseParameter.USER_ID = GetCookieValue("UserID");
        BaseParameter.ListSearchString.push(DT1);
        BaseParameter.ListSearchString.push(DT2);
        BaseParameter.ListSearchString.push(BBBB);
        BaseParameter.ListSearchString.push(CCCC);
        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        let url = "/C04/Buttonfind_Click";

        fetch(url, {
            method: "POST",
            body: formUpload,
            headers: {
            }
        }).then((response) => {
            response.json().then((data) => {
                let BaseResultSub = data;
                BaseResult.Search = BaseResultSub.Search;
                BaseResult.DataGridView11 = BaseResultSub.DataGridView11;
                DataGridView11Render();
                try {
                    $("#Label30").val(BaseResult.Search[0].COL1);
                    $("#Label31").val(BaseResult.Search[0].COL2);
                    $("#Label32").val(BaseResult.Search[0].COL3);
                    $("#Label33").val(BaseResult.Search[0].COL4);
                }
                catch (err) {
                    $("#Label30").val("ERROR");
                    $("#Label31").val("ERROR");
                    $("#Label32").val("ERROR");
                    $("#Label33").val("ERROR");
                }
                $("#BackGround").css("display", "none");
            }).catch((err) => {
                $("#BackGround").css("display", "none");
            })
        });
    }
    if (TagIndex == 9) {
        let DT1 = $("#DateTimePicker10").val();
        let DT2 = $("#DateTimePicker10").val();
        let AAA = $("#TextBox19").val();
        let BBB = $("#TextBox17").val();
        let CCC = $("#TextBox14").val();

        $("#BackGround").css("display", "block");
        let BaseParameter = new Object();
        BaseParameter = {
            Action: TagIndex,
            ListSearchString: [],
        }
        BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
        BaseParameter.USER_ID = GetCookieValue("UserID");
        BaseParameter.ListSearchString.push(DT1);
        BaseParameter.ListSearchString.push(DT2);
        BaseParameter.ListSearchString.push(AAA);
        BaseParameter.ListSearchString.push(BBB);
        BaseParameter.ListSearchString.push(CCC);
        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        let url = "/C04/Buttonfind_Click";

        fetch(url, {
            method: "POST",
            body: formUpload,
            headers: {
            }
        }).then((response) => {
            response.json().then((data) => {
                let BaseResultSub = data;
                BaseResult.DataGridView12 = BaseResultSub.DataGridView12;
                DataGridView12Render();
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

}
function Buttondelete_Click() {

}
function Buttoncancel_Click() {

}
function Buttoninport_Click() {

}
function Buttonexport_Click() {
    if (TagIndex == 1) {
        let LabelNAME = $("#LabelNAME").val();
        let fileName = LabelNAME + DateNow + "KOMAX_ORDER_LIST";
        TableHTMLToExcel("DataGridView1Table", fileName, fileName);
    }
    if (TagIndex == 3) {
        let LabelNAME = $("#LabelNAME").val();
        let fileName = LabelNAME + DateNow + "LP_ORDER_LIST";
        TableHTMLToExcel("DataGridView5Table", fileName, fileName);
    }
    if (TagIndex == 4) {
        let LabelNAME = $("#LabelNAME").val();
        let fileName = LabelNAME + DateNow + "SPST_ORDER_LIST";
        TableHTMLToExcel("DataGridView7Table", fileName, fileName);
    }
    if (TagIndex == 6) {
        //let LabelNAME = $("#LabelNAME").val();
        //let fileName = LabelNAME + DateNow + "LEAD_PRDC_QTY";
        //TableHTMLToExcel("DataGridView9Table", fileName, fileName);

        let DT1 = $("#DateTimePicker3").val();
        let DT2 = $("#DateTimePicker4").val();
        let AAAA = $("#TextBox8").val();
        let BBBB = $("#TextBox11").val();
        let CCCC = $("#TextBox16").val();

        $("#BackGround").css("display", "block");
        let BaseParameter = new Object();
        BaseParameter = {
            Action: TagIndex,
            ListSearchString: [],
        }
        BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
        BaseParameter.USER_ID = GetCookieValue("UserID");
        BaseParameter.ListSearchString.push(DT1);
        BaseParameter.ListSearchString.push(DT2);
        BaseParameter.ListSearchString.push(AAAA);
        BaseParameter.ListSearchString.push(BBBB);
        BaseParameter.ListSearchString.push(CCCC);
        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        let url = "/C04/Buttonexport_ClickSub";

        fetch(url, {
            method: "POST",
            body: formUpload,
            headers: {
            }
        }).then((response) => {
            response.json().then((data) => {
                if (data) {
                    if (data.Code) {
                        BaseResult.Code = data.Code;
                        console.log(BaseResult.Code);
                        Buttonexport_ClickInterval();
                        Buttonexport_ClickSub();
                    }
                }
                //$("#BackGround").css("display", "none");
            }).catch((err) => {
                //$("#BackGround").css("display", "none");
            })
        });
    }
    if (TagIndex == 7) {
        let LabelNAME = $("#LabelNAME").val();
        let fileName = LabelNAME + DateNow + "MC_PRDC_QTY";
        TableHTMLToExcel("DataGridView10Table", fileName, fileName);
    }
    if (TagIndex == 8) {
        let LabelNAME = $("#LabelNAME").val();
        let fileName = LabelNAME + DateNow + "WORKER_PRDC_QTY";
        TableHTMLToExcel("DataGridView11Table", fileName, fileName);
    }
    if (TagIndex == 9) {
        let LabelNAME = $("#LabelNAME").val();
        let fileName = LabelNAME + DateNow + "WORKER_LEAD_DAY";
        TableHTMLToExcel("DataGridView12Table", fileName, fileName);
    }
}
function Buttonexport_ClickInterval() {
    let Buttonexport_ClickTimer = setInterval(function () {
        $("#BackGround").css("display", "block");        
        var xhr = new XMLHttpRequest();
        xhr.open('HEAD', BaseResult.Code, false);
        xhr.send();
        if (xhr.status === 200) {
            window.location.href = BaseResult.Code;            
            $("#BackGround").css("display", "none");
            clearInterval(Buttonexport_ClickTimer);
            return true;
        } else if (xhr.status === 404) {            
            return false;
        } else {            
            return false;
        }
    }, 1000);
}
function Buttonexport_ClickSub() {   
    if (TagIndex == 6) {
        let DT1 = $("#DateTimePicker3").val();
        let DT2 = $("#DateTimePicker4").val();
        let AAAA = $("#TextBox8").val();
        let BBBB = $("#TextBox11").val();
        let CCCC = $("#TextBox16").val();

        //$("#BackGround").css("display", "block");
        let BaseParameter = new Object();
        BaseParameter = {
            Action: TagIndex,
            ListSearchString: [],
        }
        BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
        BaseParameter.USER_ID = GetCookieValue("UserID");
        BaseParameter.ListSearchString.push(DT1);
        BaseParameter.ListSearchString.push(DT2);
        BaseParameter.ListSearchString.push(AAAA);
        BaseParameter.ListSearchString.push(BBBB);
        BaseParameter.ListSearchString.push(CCCC);
        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        let url = "/C04/Buttonexport_Click";

        fetch(url, {
            method: "POST",
            body: formUpload,
            headers: {
            }
        }).then((response) => {
            response.json().then((data) => {               
                //$("#BackGround").css("display", "none");
            }).catch((err) => {
                //$("#BackGround").css("display", "none");
            })
        });
    }
    if (TagIndex == 7) {
        let LabelNAME = $("#LabelNAME").val();
        let fileName = LabelNAME + DateNow + "MC_PRDC_QTY";
        TableHTMLToExcel("DataGridView10Table", fileName, fileName);
    }
    if (TagIndex == 8) {
        let LabelNAME = $("#LabelNAME").val();
        let fileName = LabelNAME + DateNow + "WORKER_PRDC_QTY";
        TableHTMLToExcel("DataGridView11Table", fileName, fileName);
    }
    if (TagIndex == 9) {
        let LabelNAME = $("#LabelNAME").val();
        let fileName = LabelNAME + DateNow + "WORKER_LEAD_DAY";
        TableHTMLToExcel("DataGridView12Table", fileName, fileName);
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
    if (BaseResult) {
        if (BaseResult.DataGridView1) {
            if (BaseResult.DataGridView1.length > 0) {
                DataGridView1_SelectionChanged(0);
                for (let i = 0; i < BaseResult.DataGridView1.length; i++) {
                    if (BaseResult.DataGridView1[i].STATUS == "NOT") {
                        BaseResult.DataGridView1[i].Description = "B2.png";
                    }
                    else {
                        BaseResult.DataGridView1[i].Description = "R1.png";
                    }
                    HTML = HTML + "<tr onclick='DataGridView1_SelectionChanged(" + i + ")'>";
                    HTML = HTML + "<td onclick='DataGridView1_CellClick(" + i + ")'><img src='/Media/" + BaseResult.DataGridView1[i].Description + "' width='28' height='28' /></td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].STATUS + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].CODE + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].ORDER_NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].WORK_WEEK + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].CONDITION + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].LEAD_NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].PROJECT + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].PO_QTY + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].WORK_QTY + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].PO_DATE + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].MC + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].BUNDLE_SIZE + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].HOOK_RACK + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].WIRE + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].T1_DIR + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].TERM1 + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].STRIP1 + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].SEAL1 + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].CCH_W1 + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].ICH_W1 + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].T2_DIR + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].TERM2 + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].STRIP2 + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].SEAL2 + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].CCH_W2 + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].ICH_W2 + "</td>";
                    HTML = HTML + "<td>" + NameShortByName(BaseResult.DataGridView1[i].SP_ST) + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].REP + "</td>";
                    HTML = HTML + "</tr>";
                }
            }
        }
    }
    document.getElementById("DataGridView1").innerHTML = HTML;
}

function DataGridView1_SelectionChanged(i) {
    DataGridView1RowIndex = i;
    let AAA = BaseResult.DataGridView1[DataGridView1RowIndex].CODE;
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        SearchString: AAA,
    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C04/DataGridView1_SelectionChanged";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            let BaseResultSub = data;
            BaseResult.DataGridView2 = BaseResultSub.DataGridView2;
            DataGridView2Render();
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
function DataGridView1CHKChanged(i) {
    DataGridView1RowIndex = i;
}
function DataGridView1_CellClick(i) {
    DataGridView1RowIndex = i;
    if (BaseResult.DataGridView1[DataGridView1RowIndex].CONDITION == "Stay") {
        if (BaseResult.DataGridView1[DataGridView1RowIndex].STATUS == "NOT") {
            localStorage.setItem("C04_1_Label1", BaseResult.DataGridView1[DataGridView1RowIndex].CODE);
            localStorage.setItem("C04_1_Label2", BaseResult.DataGridView1[DataGridView1RowIndex].LEAD_NO);
            localStorage.setItem("C04_1_Label5", BaseResult.DataGridView1[DataGridView1RowIndex].TERM1);
            localStorage.setItem("C04_1_Label6", BaseResult.DataGridView1[DataGridView1RowIndex].SEAL1);
            localStorage.setItem("C04_1_Label7", BaseResult.DataGridView1[DataGridView1RowIndex].TERM2);
            localStorage.setItem("C04_1_Label8", BaseResult.DataGridView1[DataGridView1RowIndex].SEAL2);

            localStorage.setItem("C04_1_Close", 0);
            let url = "/C04_1";
            OpenWindowByURL(url, 800, 460);
            C04_1TimerStartInterval();
        }
    }
}
function C04_1TimerStartInterval() {
    C04_1Timer = setInterval(function () {
        let C04_1_Close = localStorage.getItem("C04_1_Close");
        if (C04_1_Close == "1") {
            clearInterval(C04_1Timer);
            C04_1Timer_Close = 0;
            localStorage.setItem("C04_1_Close", C04_1_Close);
            Buttonfind_Click();
        }
    }, 100);
}
let DataGridView1Table = document.getElementById("DataGridView1Table");
DataGridView1Table.addEventListener('click', function (event) {
    if (event.target.tagName === 'TH') {
        let text = event.target.innerText;
        let key = "CHK";
        IsTableSort = !IsTableSort;
        ListSort(BaseResult.DataGridView1, key, text, IsTableSort);
        DataGridView1Render();
    }
});
function DataGridView2Render() {
    let HTML = "";
    if (BaseResult) {
        if (BaseResult.DataGridView2) {
            if (BaseResult.DataGridView2.length > 0) {
                DataGridView2_SelectionChanged(0);
                for (let i = 0; i < BaseResult.DataGridView2.length; i++) {
                    HTML = HTML + "<tr onclick='DataGridView2_SelectionChanged(" + i + ")'>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].CODE + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].BARCODE_NAME + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].Barcode_SEQ + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].TrolleyCode + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].WORK_END + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].UPDATE_USER + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].USER_NAME + "</td>";
                    HTML = HTML + "</tr>";
                }
            }
        }
    }
    document.getElementById("DataGridView2").innerHTML = HTML;
}

function DataGridView2_SelectionChanged(i) {
    DataGridView2RowIndex = i;
}
let DataGridView2Table = document.getElementById("DataGridView2Table");
DataGridView2Table.addEventListener('click', function (event) {
    if (event.target.tagName === 'TH') {
        let text = event.target.innerText;
        let key = "CODE";
        IsTableSort = !IsTableSort;
        ListSort(BaseResult.DataGridView2, key, text, IsTableSort);
        DataGridView2Render();
    }
});
function DataGridView3Render() {
    let HTML = "";
    if (BaseResult) {
        if (BaseResult.DataGridView3) {
            if (BaseResult.DataGridView3.length > 0) {
                DataGridView3_SelectionChanged(0);
                for (let i = 0; i < BaseResult.DataGridView3.length; i++) {
                    if (BaseResult.DataGridView3[i].Stock_status == "Bad") {
                        HTML = HTML + "<tr style='background-color: red;' onclick='DataGridView3_SelectionChanged(" + i + ")'>";
                    }
                    else {
                        HTML = HTML + "<tr onclick='DataGridView3_SelectionChanged(" + i + ")'>";
                    }
                    HTML = HTML + "<td>" + BaseResult.DataGridView3[i].HOOK_RACK + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView3[i].LEAD_NM + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView3[i].STOCK_QTY + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView3[i].IN_QTY + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView3[i].OUT_QTY + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView3[i].Stock_status + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView3[i].CHK_QTY + "</td>";
                    HTML = HTML + "</tr>";
                }
            }
        }
    }
    document.getElementById("DataGridView3").innerHTML = HTML;
}

function DataGridView3_SelectionChanged(i) {
    DataGridView3RowIndex = i;
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
function DataGridView4Render() {
    let HTML = "";
    if (BaseResult) {
        if (BaseResult.DataGridView4) {
            if (BaseResult.DataGridView4.length > 0) {
                DataGridView4_SelectionChanged(0);
                for (let i = 0; i < BaseResult.DataGridView4.length; i++) {
                    HTML = HTML + "<tr onclick='DataGridView4_SelectionChanged(" + i + ")'>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView4[i].HOOK_RACK + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView4[i].LEAD_NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView4[i].BARCODE_NAME + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView4[i].SEQ + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView4[i].QTY + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView4[i].STATUS + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView4[i].IN_DATE + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView4[i].OUT_DATE + "</td>";
                    HTML = HTML + "</tr>";
                }
            }
        }
    }
    document.getElementById("DataGridView4").innerHTML = HTML;
}

function DataGridView4_SelectionChanged(i) {
    DataGridView4RowIndex = i;
}
let DataGridView4Table = document.getElementById("DataGridView4Table");
DataGridView4Table.addEventListener('click', function (event) {
    if (event.target.tagName === 'TH') {
        let text = event.target.innerText;
        let key = "CHK";
        IsTableSort = !IsTableSort;
        ListSort(BaseResult.DataGridView4, key, text, IsTableSort);
        DataGridView4Render();
    }
});
function DataGridView5Render() {
    let HTML = "";
    if (BaseResult) {
        if (BaseResult.DataGridView5) {
            if (BaseResult.DataGridView5.length > 0) {
                DataGridView5_SelectionChanged(0);
                for (let i = 0; i < BaseResult.DataGridView5.length; i++) {
                    HTML = HTML + "<tr onclick='DataGridView5_SelectionChanged(" + i + ")'>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView5[i].CODE + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView5[i].ORDER_NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView5[i].WORK_WEEK + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView5[i].CONDITION + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView5[i].LEAD_NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView5[i].PROJECT + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView5[i].PO_QTY + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView5[i].ADJ_AF_QTY + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView5[i].PO_DATE + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView5[i].MC + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView5[i].TERM1 + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView5[i].T1_W_QTY + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView5[i].TERM2 + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView5[i].T2_W_QTY + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView5[i].BUNDLE_SIZE + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView5[i].HOOK_RACK + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView5[i].WIRE + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView5[i].T1_DIR + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView5[i].TERM1 + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView5[i].TERM1 + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView5[i].STRIP1 + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView5[i].SEAL1 + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView5[i].CCH_W1 + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView5[i].ICH_W1 + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView5[i].TERM2 + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView5[i].T2_DIR + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView5[i].TERM2 + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView5[i].STRIP2 + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView5[i].SEAL2 + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView5[i].CCH_W2 + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView5[i].ICH_W2 + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView5[i].SP_ST + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView5[i].REP + "</td>";
                    HTML = HTML + "</tr>";
                }
            }
        }
    }
    document.getElementById("DataGridView5").innerHTML = HTML;
}

function DataGridView5_SelectionChanged(i) {
    DataGridView5RowIndex = i;
    let AAA = BaseResult.DataGridView5[DataGridView5RowIndex].CODE;
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        SearchString: AAA,
    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C04/DataGridView5_SelectionChanged";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            let BaseResultSub = data;
            BaseResult.DataGridView6 = BaseResultSub.DataGridView6;
            DataGridView6Render();
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
let DataGridView5Table = document.getElementById("DataGridView5Table");
DataGridView5Table.addEventListener('click', function (event) {
    if (event.target.tagName === 'TH') {
        let text = event.target.innerText;
        let key = "CODE";
        IsTableSort = !IsTableSort;
        ListSort(BaseResult.DataGridView5, key, text, IsTableSort);
        DataGridView5Render();
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
                    HTML = HTML + "<td>" + BaseResult.DataGridView6[i].CODE + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView6[i].BARCODE_NAME + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView6[i].Barcode_SEQ + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView6[i].WORK_END + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView6[i].UPDATE_USER + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView6[i].USER_NAME + "</td>";
                    HTML = HTML + "</tr>";
                }
            }
        }
    }
    document.getElementById("DataGridView6").innerHTML = HTML;
}

function DataGridView6_SelectionChanged(i) {
    DataGridView6RowIndex = i;
}
let DataGridView6Table = document.getElementById("DataGridView6Table");
DataGridView6Table.addEventListener('click', function (event) {
    if (event.target.tagName === 'TH') {
        let text = event.target.innerText;
        let key = "CODE";
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
                    HTML = HTML + "<td>" + BaseResult.DataGridView7[i].OR_NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView7[i].WORK_WEEK + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView7[i].CONDITION + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView7[i].LEAD_NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView7[i].PO_QTY + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView7[i].SAFTY_QTY + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView7[i].PERFORMN + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView7[i].PO_DT + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView7[i].MC + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView7[i].LEAD_COUNT + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView7[i].BUNDLE_SIZE + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView7[i].HOOK_RACK + "</td>";
                    HTML = HTML + "</tr>";
                }
            }
        }
    }
    document.getElementById("DataGridView7").innerHTML = HTML;
}

function DataGridView7_SelectionChanged(i) {
    DataGridView7RowIndex = i;
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
                    HTML = HTML + "<td>" + BaseResult.DataGridView8[i].CODE + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView8[i].BARCODE_NAME + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView8[i].Barcode_SEQ + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView8[i].WORK_END + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView8[i].UPDATE_USER + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView8[i].USER_NAME + "</td>";
                    HTML = HTML + "</tr>";
                }
            }
        }
    }
    document.getElementById("DataGridView8").innerHTML = HTML;
}

function DataGridView8_SelectionChanged(i) {
    DataGridView8RowIndex = i;
    let AAA = BaseResult.DataGridView7[DataGridView7RowIndex].CODE;
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        SearchString: AAA,
    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/C04/DataGridView8_SelectionChanged";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            let BaseResultSub = data;
            BaseResult.DataGridView8 = BaseResultSub.DataGridView8;
            DataGridView8Render();
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
let DataGridView8Table = document.getElementById("DataGridView8Table");
DataGridView8Table.addEventListener('click', function (event) {
    if (event.target.tagName === 'TH') {
        let text = event.target.innerText;
        let key = "CODE";
        IsTableSort = !IsTableSort;
        ListSort(BaseResult.DataGridView8, key, text, IsTableSort);
        DataGridView8Render();
    }
});
function DataGridView9Render() {
    let HTML = "";
    if (BaseResult) {
        if (BaseResult.DataGridView9) {
            if (BaseResult.DataGridView9.length > 0) {
                DataGridView9_SelectionChanged(0);
                let Count = BaseResult.DataGridView9.length;
                //if (Count > 50) {
                //    Count = 50;
                //}
                for (let i = 0; i < Count; i++) {
                    HTML = HTML + "<tr onclick='DataGridView9_SelectionChanged(" + i + ")'>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView9[i].STAGE + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView9[i].TORDER_IDX + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView9[i].DATE + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView9[i].LEAD_NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView9[i].MC_NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView9[i].TERM + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView9[i].WK_QTY + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView9[i].FIRST_TIME + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView9[i].END_TIME + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView9[i].CREATE_USER + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView9[i].Name + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView9[i].CREATE_DTM + "</td>";
                    HTML = HTML + "</tr>";
                }
            }
        }
    }
    document.getElementById("DataGridView9").innerHTML = HTML;
}

function DataGridView9_SelectionChanged(i) {
    DataGridView9RowIndex = i;
}
let DataGridView9Table = document.getElementById("DataGridView9Table");
DataGridView9Table.addEventListener('click', function (event) {
    if (event.target.tagName === 'TH') {
        let text = event.target.innerText;
        let key = "STAGE";
        IsTableSort = !IsTableSort;
        ListSort(BaseResult.DataGridView9, key, text, IsTableSort);
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
                    HTML = HTML + "<td>" + BaseResult.DataGridView10[i].STAGE + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView10[i].MC_NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView10[i].DATE + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView10[i].LEAD_COUNT + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView10[i].WK_QTY + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView10[i].FIRST_TIME + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView10[i].END_TIME + "</td>";
                    HTML = HTML + "</tr>";
                }
            }
        }
    }
    document.getElementById("DataGridView10").innerHTML = HTML;
}

function DataGridView10_SelectionChanged(i) {
    DataGridView10RowIndex = i;
}
let DataGridView10Table = document.getElementById("DataGridView10Table");
DataGridView10Table.addEventListener('click', function (event) {
    if (event.target.tagName === 'TH') {
        let text = event.target.innerText;
        let key = "STAGE";
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
                    HTML = HTML + "<tr onclick='DataGridView11_SelectionChanged(" + i + ")'>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView11[i].STAGE + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView11[i].CREATE_USER + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView11[i].Name + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView11[i].DATE + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView11[i].LEAD_COUNT + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView11[i].WK_QTY + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView11[i].FIRST_TIME + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView11[i].END_TIME + "</td>";
                    HTML = HTML + "</tr>";
                }
            }
        }
    }
    document.getElementById("DataGridView11").innerHTML = HTML;
}

function DataGridView11_SelectionChanged(i) {
    DataGridView11RowIndex = i;
}
let DataGridView11Table = document.getElementById("DataGridView11Table");
DataGridView11Table.addEventListener('click', function (event) {
    if (event.target.tagName === 'TH') {
        let text = event.target.innerText;
        let key = "STAGE";
        IsTableSort = !IsTableSort;
        ListSort(BaseResult.DataGridView11, key, text, IsTableSort);
        DataGridView11Render();
    }
});
function DataGridView12Render() {
    let HTML = "";
    if (BaseResult) {
        if (BaseResult.DataGridView12) {
            if (BaseResult.DataGridView12.length > 0) {
                DataGridView12_SelectionChanged(0);
                for (let i = 0; i < BaseResult.DataGridView12.length; i++) {
                    HTML = HTML + "<tr onclick='DataGridView12_SelectionChanged(" + i + ")'>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView12[i].STAGE + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView12[i].DATE + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView12[i].LEAD_NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView12[i].MC_NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView12[i].TIME + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView12[i].WK_QTY + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView12[i].FIRST_TIME + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView12[i].END_TIME + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView12[i].CREATE_USER + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView12[i].Name + "</td>";
                    HTML = HTML + "</tr>";
                }
            }
        }
    }
    document.getElementById("DataGridView12").innerHTML = HTML;
}

function DataGridView12_SelectionChanged(i) {
    DataGridView12RowIndex = i;
}
let DataGridView12Table = document.getElementById("DataGridView12Table");
DataGridView12Table.addEventListener('click', function (event) {
    if (event.target.tagName === 'TH') {
        let text = event.target.innerText;
        let key = "STAGE";
        IsTableSort = !IsTableSort;
        ListSort(BaseResult.DataGridView12, key, text, IsTableSort);
        DataGridView12Render();
    }
});