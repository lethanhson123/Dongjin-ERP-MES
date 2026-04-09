let IsTableSort = false;
let BaseResult = new Object();
let TagIndex = 1;
let Now;
let DataGridView1RowIndex = 0;
let DataGridView2RowIndex = 0;
let PART_LOC = "0";

$(document).ready(function () {
    var now = new Date();
    var day = ("0" + now.getDate()).slice(-2);
    var month = ("0" + (now.getMonth() + 1)).slice(-2);
    var today = now.getFullYear() + "-" + (month) + "-" + (day);
    Now = today;
});

$("#suchTB1").keydown(function (e) {
    if (e.keyCode == 13) {
        SuchTB1_KeyDown();
    }
});
function SuchTB1_KeyDown() {
    Buttonfind_Click();
}
$("#TextBox3").focusout(function (e) {
    TextBox3_Leave();
});
function TextBox3_Leave() {
    if (BaseResult) {
        if (BaseResult.DataGridView2) {
            if (BaseResult.DataGridView2.length > 0) {
                let AA = Number(BaseResult.DataGridView2[0].Incoming_QTY);
                let BB = Number(BaseResult.DataGridView2[0].OUT_QTY);
                let CC = Number($("#TextBox3").val());
                let DDD = CC - (AA - BB);
                $("#TextBox1").val(DDD);
            }
        }
    }
}
$("#TextBox3").keypress(function (e) {
    TextBox3_KeyPress();
});
function TextBox3_KeyPress() {

}
$("#Label1").click(function (e) {
    Label1_DoubleClick();
});
function Label1_DoubleClick() {
    try {
        let TextBox3 = $("#TextBox3").val();
        if (TextBox3.length > 0) {
            let ST_QTY_CH = Number($("#TextBox3").val());
            let BE_QTY_CH = Number(BaseResult.DataGridView2[0].Change_QTY);
            let QTY_CH = Number(BaseResult.DataGridView2[0].Difference_QTY);
            let QTY_MES = Number(BaseResult.DataGridView2[0].MES_STOCK);
            let TextBox1 = BE_QTY_CH - QTY_CH + (ST_QTY_CH - QTY_MES);
            $("#TextBox1").val(TextBox1);
        }
    }
    catch (err) {
        console.log(err);
    }
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
    let IsFind = true;
    let suchTB1 = $("#suchTB1").val();
    if (suchTB1 == "") {
        IsFind = false;
    }
    if (IsFind == true) {
        $("#BackGround").css("display", "block");
        let BaseParameter = new Object();
        BaseParameter = {
            SearchString: suchTB1,
        }
        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        let url = "/G02/Buttonfind_Click";

        fetch(url, {
            method: "POST",
            body: formUpload,
            headers: {
            }
        }).then((response) => {
            response.json().then((data) => {
                BaseResult = data;
                if (BaseResult) {
                    if (BaseResult.DGV_G02_01) {
                        if (BaseResult.DGV_G02_01.length > 0) {
                            PART_LOC = BaseResult.DGV_G02_01[0].PART_SCN;
                            $("#TextBox1").val("");
                            $("#TextBox3").val("");
                            $("#TextBox4").val("");
                            STOCK_LOAD();
                            tiivaj_HISTORY();
                        }
                        else {
                            alert(localStorage.getItem("ERRORAnErrorOccurred"));
                        }
                    }
                }
                //$("#BackGround").css("display", "none");
            }).catch((err) => {
                $("#BackGround").css("display", "none");
            })
        });
    }
    else {
        alert(localStorage.getItem("ERRORAnErrorOccurred"));
    }
}
function Buttonadd_Click() {

}
function Buttonsave_Click() {
    let IsSave = true;
    let LBPIDX = $("#LBPIDX").val();
    if (LBPIDX == "-") {
        IsSave = false;
        alert(localStorage.getItem("Notification_G02_001"));
    }
    let TextBox4 = $("#TextBox4").val();
    if (TextBox4 == "") {
        IsSave = false;
        alert(localStorage.getItem("Notification_G02_002"));
    }
    let TextBox1 = $("#TextBox1").val();
    if (TextBox1 == "-") {
        IsSave = false;
        alert(localStorage.getItem("Notification_G02_003"));
    }
    let TextBox3 = $("#TextBox3").val();
    if (TextBox3 == "-") {
        IsSave = false;
        alert(localStorage.getItem("Notification_G02_004"));
    }
    if (IsSave == true) {
        $("#BackGround").css("display", "block");
        let BaseParameter = new Object();
        BaseParameter = {
            ListSearchString: [],
        }
        BaseParameter.USER_ID = GetCookieValue("UserID");
        BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
        BaseParameter.ListSearchString.push($("#LBPIDX").val());
        BaseParameter.ListSearchString.push($("#LBQTY").val());
        BaseParameter.ListSearchString.push($("#ComboBox1").val());
        BaseParameter.ListSearchString.push($("#TextBox1").val());
        BaseParameter.ListSearchString.push($("#TextBox3").val());
        BaseParameter.ListSearchString.push($("#TextBox4").val());

        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        let url = "/G02/Buttonsave_Click";

        fetch(url, {
            method: "POST",
            body: formUpload,
            headers: {
            }
        }).then((response) => {
            response.json().then((data) => {
                Buttonfind_Click();

                alert(localStorage.getItem("SaveComplete"));

                $("#TextBox1").val("");
                $("#TextBox3").val("");
                $("#TextBox4").val("");

                //$("#BackGround").css("display", "none");
            }).catch((err) => {
                alert(localStorage.getItem("ERRORPleaseCheckAgain"));
                $("#TextBox1").val("");
                $("#TextBox3").val("");
                $("#TextBox4").val("");
                $("#BackGround").css("display", "none");
            })
        });
    }
}
function Buttondelete_Click() {

}
function Buttoncancel_Click() {

}
function Buttoninport_Click() {

}
function Buttonexport_Click() {

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
                    HTML = HTML + "<tr onclick='DataGridView1_SelectionChanged(" + i + ")'>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].ADJ_DTM + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].ADJ_QTY + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].ADJ_BF_QTY + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].ADJ_AF_QTY + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].ADJ_RSON + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].CREATE_DTM + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].CREATE_USER + "</td>";
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
function DataGridView1_SelectionChanged(i) {
    DataGridView1RowIndex = i;
}
function DataGridView1CHKChanged(i) {
    let id = "DataGridView1CHK" + i;
    DataGridView1RowIndex = i;
    BaseResult.DataGridView1[DataGridView1RowIndex].CHK = $("#" + id).is(":checked");
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
                        HTML = HTML + "<td><label><input id='DataGridView1CHK" + i + "' class='form-check-input' type='checkbox' checked onclick='DataGridView1CHKChanged(" + i + ")'><span></span></label></td>";
                    }
                    else {
                        HTML = HTML + "<td><label><input id='DataGridView1CHK" + i + "' class='form-check-input' type='checkbox' onclick='DataGridView1CHKChanged(" + i + ")'><span></span></label></td>";
                    }
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].PART_IDX + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].PART_NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].PART_NM + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].Change_QTY + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].Incoming_QTY + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].OUT_QTY + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].Difference_QTY + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].MES_STOCK + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].EXCEL + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].Verification + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].Location + "</td>";
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
function DataGridView2_SelectionChanged(i) {
    DataGridView2RowIndex = i;
}
function STOCK_LOAD() {
    let WHR_CD = "";
    if (PART_LOC == "5") {
        WHR_CD = " WHERE `CD_IDX` = '1'  ";
    }
    if (PART_LOC == "6") {
        WHR_CD = " WHERE `CD_IDX` = '2'  ";
    }
    if (WHR_CD == "") {
        $("#LBNO").val("-");
        $("#LBNM").val("-");
        $("#LBCAR").val("-");
        $("#LBFML").val("-");
        $("#LBQTY").val("-");
    }
    if (BaseResult.DGV_G02_01) {
        if (BaseResult.DGV_G02_01.length > 0) {
            $("#LBPIDX").val(BaseResult.DGV_G02_01[0].PART_IDX);
            $("#LBNO").val(BaseResult.DGV_G02_01[0].PART_NO);
            $("#LBNM").val(BaseResult.DGV_G02_01[0].PART_NM);
            $("#LBCAR").val(BaseResult.DGV_G02_01[0].PART_CAR);
            $("#LBFML").val(BaseResult.DGV_G02_01[0].PART_FML);
            $("#LBQTY").val(BaseResult.DGV_G02_01[0].QTY);
        }
    }
    let LBNO = $("#LBNO").val();
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        ListSearchString: [],
    }
    BaseParameter.ListSearchString.push(PART_LOC);
    BaseParameter.ListSearchString.push(LBNO);
    BaseParameter.DGV_G02_01 = BaseResult.DGV_G02_01;
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/G02/STOCK_LOAD";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            let BaseResultSTOCK_LOAD = data;
            BaseResult.DGV_G02_CB = BaseResultSTOCK_LOAD.DGV_G02_CB;
            BaseResult.DataGridView2 = BaseResultSTOCK_LOAD.DataGridView2;            
            if (BaseResult) {
                if (BaseResult.DGV_G02_CB) {
                    if (BaseResult.DGV_G02_CB.length > 0) {
                        $("#ComboBox1").empty();
                        for (let i = 0; i < BaseResult.DGV_G02_CB.length; i++) {
                            var optionComboBox1 = document.createElement("option");
                            optionComboBox1.text = BaseResult.DGV_G02_CB[i].CD_SYS_NOTE;
                            optionComboBox1.value = BaseResult.DGV_G02_CB[i].CD_SYS_NOTE;

                            var ComboBox1 = document.getElementById("ComboBox1");
                            ComboBox1.add(optionComboBox1);
                        }
                    }
                }
                if (BaseResult.DataGridView2) {
                    if (BaseResult.DataGridView2.length > 0) {                        
                        DataGridView2Render();
                    }
                }
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });

}
function tiivaj_HISTORY() {
    let LBNO = $("#LBNO").val();
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        SearchString: LBNO,
    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/G02/tiivaj_HISTORY";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            let BaseResulttiivaj_HISTORY = data;            
            BaseResult.DataGridView1 = BaseResulttiivaj_HISTORY.DataGridView1;
            if (BaseResult) {
                if (BaseResult.DataGridView1) {
                    if (BaseResult.DataGridView1.length > 0) {                        
                        DataGridView1Render();
                    }
                }
            }
            //$("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}