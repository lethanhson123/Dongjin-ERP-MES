

let IsTableSort = false;
let BaseResult = new Object();
let TagIndex = 1;
let BC_DSCN_YN = "Y";
let DataGridView1RowIndex = 0;
let DataGridView2RowIndex = 0;
let DataGridView4RowIndex = 0;
let T_DGV_01RowIndex = 0;
let T_DGV_02RowIndex = 0;
let DGV_LISTRowIndex = 0;
let DGV_CLS = false;
let IsTBBC_SUCKKeyDown = false;
let B04_ERRORTimer;
let B04_ERRORMAS_CHK = "";

let R1;
let R2;
let R3;
let R4;
let R5;
let R6;
let R7;
let R8;
let R9;
let R10;
let R11;
let R12;
let R13;
let R15;
let R16;

let R14 = 0;
$(document).ready(function () {
    BaseResult.T_DGV_02Sub = new Object();
    $(".modal").modal({
        dismissible: false
    });

    document.getElementById("Reason01").checked = false;
    document.getElementById("Reason02").checked = false;
    document.getElementById("Reason03").checked = false;
    document.getElementById("Reason04").checked = false;
    document.getElementById("B04_ERROR_Button1").disabled = true;

    var now = new Date();
    var day = ("0" + now.getDate()).slice(-2);
    var month = ("0" + (now.getMonth() + 1)).slice(-2);
    var today = now.getFullYear() + "-" + (month) + "-" + (day);
    $("#DTPsuck1").val(today);
    $("#DTPsuck2").val(today);
    $("#TBTEXT03").val(today);
    $("#DateTimePicker1").val(today);
    $("#SearchDate").val(today);

    $("#RBY").prop("checked", true);
    $("#BOX_QTY").val("0");

    let AAA = now.getFullYear();
    let BBB = month;
    $("#Label9").val(AAA);
    $("#Label10").val(BBB);
    $("#TBBarcode").focus();
    Load();
    localStorage.setItem("B04_ERRORMAS_CHK", "");
});
function PlaySuccessSound() {
    try {
        let audio = new Audio('/audio/HOOK_S.mp3');
        audio.play().catch(e => console.log('Cannot play success sound:', e));
    } catch (e) {
        console.log('Success sound not available');
    }
}

function PlayErrorSound() {
    try {
        let audio = new Audio('/audio/Sash_brk.mp3');
        audio.play().catch(e => console.log('Cannot play error sound:', e));
    } catch (e) {
        console.log('Error sound not available');
    }
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
$("#TBBarcode").keydown(function (e) {
    if (e.keyCode == 13) {
        BTBCCHK_Click();
    }
});
$("#TBBarcode").click(function (e) {
    $("#TBpo_result").val("Stay");
});
$("#TBBarcode").change(function (e) {
    let TBBarcode = $("#TBBarcode").val();
    TBBarcode = TBBarcode.trim();
    $("#TBBarcode").val(TBBarcode);
});
$("#BTBCCHK").click(function (e) {
    BTBCCHK_Click();
});
$("#Button1").click(function (e) {
    let url = "/B04_1";
    OpenWindowByURL(url, 800, 500);
});
$("#DTPsuck1").change(function (e) {
    let DTPsuck1 = $("#DTPsuck1").val();
    let DTPsuck2 = $("#DTPsuck2").val();
    var DTPsuck1Date = new Date(DTPsuck1.replace(/(\d{2})-(\d{2})-(\d{4})/, "$2/$1/$3"));
    var DTPsuck2Date = new Date(DTPsuck2.replace(/(\d{2})-(\d{2})-(\d{4})/, "$2/$1/$3"));
    if (DTPsuck2Date < DTPsuck1Date) {
        $("#DTPsuck2").val(DTPsuck1);
    }

});
$("#DTPsuck2").change(function (e) {
    let DTPsuck1 = $("#DTPsuck1").val();
    let DTPsuck2 = $("#DTPsuck2").val();
    var DTPsuck1Date = new Date(DTPsuck1.replace(/(\d{2})-(\d{2})-(\d{4})/, "$2/$1/$3"));
    var DTPsuck2Date = new Date(DTPsuck2.replace(/(\d{2})-(\d{2})-(\d{4})/, "$2/$1/$3"));
    if (DTPsuck2Date < DTPsuck1Date) {
        $("#DTPsuck2").val(DTPsuck1);
    }
});
$("#RBY").click(function () {
    let RBY = $("#RBY").val();
    if (RBY == true) {
        BC_DSCN_YN = "Y";
    }
    let RBN = $("#RBN").val();
    if (RBN == true) {
        BC_DSCN_YN = "Y";
    }
});
$("#TBBC_SUCK").keydown(function (e) {
    if (e.keyCode == 13) {
        IsTBBC_SUCKKeyDown = true;
        Buttonfind_Click();
    }
});
$("#TBTEXT01").keydown(function (e) {
    if (e.keyCode == 13) {
        Buttonfind_Click();
    }
});
$("#TBTEXT03").change(function (e) {
    Buttonfind_Click();
});
$("#TLB_CHK1").click(function () {
    RBchk();
});
$("#TLB_CHK2").click(function () {
    RBchk();
});
$("#TextBox3").keydown(function (e) {
    if (e.keyCode == 13) {
        TextBox3_KeyDown();
    }
});
let TextBox3_KeyDownCount = 0;
function TextBox3_KeyDown() {
    let TextBox3 = $("#TextBox3").val();
    if (TextBox3_KeyDownCount == 0) {
        BaseResult.T_DGV_02Sub = BaseResult.T_DGV_02;
    }
    TextBox3_KeyDownCount = TextBox3_KeyDownCount + 1;
    if (TextBox3.length > 0) {
        BaseResult.T_DGV_02 = BaseResult.T_DGV_02.filter(item => item.BARCD_ID.includes(TextBox3));
    }
    else {
        BaseResult.T_DGV_02 = BaseResult.T_DGV_02Sub;
    }
    T_DGV_02Render();
}
$("#Search").keydown(function (e) {
    if (e.keyCode == 13) {
        Buttonfind_Click();
    }
});
$("#DateTimePicker1").change(function (e) {
    let DateTimePicker1 = $("#DateTimePicker1").val();
    var DateTimePicker1Date = new Date(DateTimePicker1.replace(/(\d{2})-(\d{2})-(\d{4})/, "$2/$1/$3"));
    var month = ("0" + (DateTimePicker1Date.getMonth() + 1)).slice(-2);
    $("#Label9").val(DateTimePicker1Date.getFullYear());
    $("#Label10").val(month);
});
$("#TLB_01").click(function (e) {
    let url = "/B04_2";
    OpenWindowByURL(url, 800, 400);
    TLB_01StartInterval();
});
$("#Reason01").click(function (e) {
    Reason01Click();
});
$("#Reason02").click(function (e) {
    Reason01Click();
});
$("#Reason03").click(function (e) {
    Reason01Click();
});
$("#Reason04").click(function (e) {
    Reason01Click();
});
function Reason01Click() {
    document.getElementById("B04_ERROR_Button1").disabled = false;
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
    if (TagIndex == 2) {
        $("#BackGround").css("display", "block");
        let ADATE = $("#DTPsuck2").val();
        let BDATE = $("#DTPsuck1").val();
        let BC_ID = $("#TBBC_SUCK").val();

        let BaseParameter = new Object();
        BaseParameter = {
            Action: TagIndex,
            ListSearchString: [],
        }
        BaseParameter.ListSearchString.push(ADATE);
        BaseParameter.ListSearchString.push(BDATE);
        BaseParameter.ListSearchString.push(BC_ID);
        BaseParameter.ListSearchString.push(BC_DSCN_YN);

        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        let url = "/B04/Buttonfind_Click";

        fetch(url, {
            method: "POST",
            body: formUpload,
            headers: {
            }
        }).then((response) => {
            response.json().then((data) => {
                let BaseResult2 = data;
                BaseResult.DGV_LIST = BaseResult2.DGV_LIST;
                if (IsTBBC_SUCKKeyDown == true) {
                    if (BaseResult) {
                        if (BaseResult.DGV_LIST) {
                            for (let i = 0; i < BaseResult.DGV_LIST.length; i++) {
                                if (BaseResult.DGV_LIST[i].BC_DSCN_CHK == true) {
                                    BaseResult.DGV_LIST[i].CHK = false;
                                }
                            }
                        }
                    }
                }
                DGV_LISTRender();
                $("#BackGround").css("display", "none");
            }).catch((err) => {
                $("#BackGround").css("display", "none");
            })
        });
    }
    if (TagIndex == 3) {
        $("#BackGround").css("display", "block");
        let AAA = $("#TBTEXT01").val();
        let CCC = $("#TBTEXT03").val();


        let BaseParameter = new Object();
        BaseParameter = {
            Action: TagIndex,
            ListSearchString: [],
        }
        BaseParameter.ListSearchString.push(AAA);
        BaseParameter.ListSearchString.push(CCC);

        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        let url = "/B04/Buttonfind_Click";

        fetch(url, {
            method: "POST",
            body: formUpload,
            headers: {
            }
        }).then((response) => {
            response.json().then((data) => {
                let BaseResult2 = data;
                BaseResult.T_DGV_01 = BaseResult2.T_DGV_01;
                BaseResult.T_DGV_02 = BaseResult2.T_DGV_02;
                BaseResult.CALL_STOCK = BaseResult2.CALL_STOCK;
                T_DGV_01Render();
                CALL_STOCK();
                T_DGV_02Render();
                $("#BackGround").css("display", "none");
            }).catch((err) => {
                $("#BackGround").css("display", "none");
            })
        });
    }
    if (TagIndex == 4) {
        $("#BackGround").css("display", "block");
        let D4_YEAR = $("#Label9").val();
        let D4_MONTH = $("#Label10").val();
        let AAA = $("#TextBox1").val();
        let BBB = $("#TextBox2").val();

        let BaseParameter = new Object();
        BaseParameter = {
            Action: TagIndex,
            ListSearchString: [],
        }
        BaseParameter.ListSearchString.push(D4_YEAR);
        BaseParameter.ListSearchString.push(D4_MONTH);
        BaseParameter.ListSearchString.push(AAA);
        BaseParameter.ListSearchString.push(BBB);

        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        let url = "/B04/Buttonfind_Click";

        fetch(url, {
            method: "POST",
            body: formUpload,
            headers: {
            }
        }).then((response) => {
            response.json().then((data) => {
                let BaseResult2 = data;
                BaseResult.DataGridView1 = BaseResult2.DataGridView1;
                BaseResult.DataGridView2 = BaseResult2.DataGridView2;
                DataGridView1Render();
                DataGridView2Render();
                $("#BackGround").css("display", "none");
            }).catch((err) => {
                $("#BackGround").css("display", "none");
            })
        });
    }
    if (TagIndex == 5) {
        $("#BackGround").css("display", "block");
        let SearchDate = $("#SearchDate").val();
        let Search = $("#Search").val();

        let BaseParameter = new Object();
        BaseParameter = {
            Action: TagIndex,
            ListSearchString: [],
        }
        BaseParameter.USER_ID = GetCookieValue("UserID");
        BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
        BaseParameter.ListSearchString.push(SearchDate);
        BaseParameter.ListSearchString.push(Search);

        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        let url = "/B04/Buttonfind_Click";

        fetch(url, {
            method: "POST",
            body: formUpload,
            headers: {
            }
        }).then((response) => {
            response.json().then((data) => {
                BaseResult.DataGridView7 = data.DataGridView7;
                BaseResult.DataGridView8 = data.DataGridView8;
                DataGridView7Render();
                DataGridView8Render();
                $("#BackGround").css("display", "none");
            }).catch((err) => {
                $("#BackGround").css("display", "none");
            })
        });
    }
    if (TagIndex == 6) {
        $("#BackGround").css("display", "block");
        let SearchString = $("#SearchString").val();
        let Year = $("#Year").val();
        let Month = $("#Month").val();
        

        let BaseParameter = new Object();
        BaseParameter = {
            Action: TagIndex,
            ListSearchString: [],
        }
        BaseParameter.USER_ID = GetCookieValue("UserID");
        BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
        BaseParameter.ListSearchString.push(SearchString);
        BaseParameter.ListSearchString.push(Year);
        BaseParameter.ListSearchString.push(Month);
        BaseParameter.CheckBox1 = document.getElementById("CheckBox1").checked;
        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        let url = "/B04/Buttonfind_Click";

        fetch(url, {
            method: "POST",
            body: formUpload,
            headers: {
            }
        }).then((response) => {
            response.json().then((data) => {
                BaseResult.DGV_B07_31 = data.DGV_B07_31;
                DGV_B07_31Render();
                $("#BackGround").css("display", "none");
            }).catch((err) => {
                $("#BackGround").css("display", "none");
            })
        });
    }
}
function Buttonadd_Click() {
    if (TagIndex == 1) {
        BTBCCHK_Click();
    }
}
function Buttonsave_Click() {
    let IsSave = true;
    if (TagIndex == 1) {
        if (DGV_CLS == true) {
            IsSave = false;
        }
        let po_result = "Ing......";
        let DGV_result1 = "ERROR : ";
        let DGV_result2 = "";

        if (BaseResult) {
            if (BaseResult.DataGridView4) {
                if (BaseResult.DataGridView4.length < 0) {
                    po_result = "No Data... Add Barcode...";
                    alert(po_result + "Please Check Again.");

                    IsSave = false;
                }
            }
        }
        if (IsSave == true) {
            $("#BackGround").css("display", "block");
            let BaseParameter = new Object();
            BaseParameter = {

            }
            BaseParameter.Action = TagIndex;
            BaseParameter.USER_ID = GetCookieValue("UserID");
            BaseParameter.DataGridView4 = BaseResult.DataGridView4;
            let formUpload = new FormData();
            formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
            let url = "/B04/Buttonsave_Click";

            fetch(url, {
                method: "POST",
                body: formUpload,
                headers: {
                }
            }).then((response) => {
                response.json().then((data) => {
                    let BaseResultSave = data;
                    BaseResult.DataGridView4 = BaseResultSave.DataGridView4;
                    BaseResult.DGV_result3 = BaseResultSave.DGV_result3;
                    DataGridView4Render();
                    PlaySuccessSound();
                    if (BaseResultSave) {
                        if (BaseResultSave.Code) {
                            let url = BaseResultSave.Code;
                            OpenWindowByURL(url, 200, 200);
                        }
                    }
                    $("#BackGround").css("display", "none");
                }).catch((err) => {
                    PlayErrorSound();
                    $("#BackGround").css("display", "none");
                })
            });
        }
        po_result = "Finish"
        $("#TBpo_result").val(po_result);
        DGV_CLS = true;
        $("#BOX_QTY").val("0");
        $("#TBBarcode").val("");
        $("#TBBarcode").focus();
    }
}
function Buttondelete_Click() {
    if (TagIndex == 1) {
        if (BaseResult) {
            if (BaseResult.DataGridView4) {
                if (BaseResult.DataGridView4.length > 0) {
                    BaseResult.DataGridView4.splice(DataGridView4RowIndex, 1);
                    DataGridView4Render();
                    $("#BOX_QTY").val(BaseResult.DataGridView4.length);
                }
            }
        }
    }
    if (TagIndex == 2) {
        if (BaseResult) {
            if (BaseResult.DGV_LIST) {
                if (BaseResult.DGV_LIST.length > 0) {
                    $("#BackGround").css("display", "block");
                    let BaseParameter = new Object();
                    BaseParameter = {

                    }
                    BaseParameter.Action = TagIndex;
                    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
                    BaseParameter.USER_ID = GetCookieValue("UserID");
                    BaseParameter.DGV_LIST = BaseResult.DGV_LIST;
                    let formUpload = new FormData();
                    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
                    let url = "/B04/Buttondelete_Click";

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
            }
        }
        $("#TBBC_SUCK").val("");
        Buttonfind_Click();
    }
}
function Buttoncancel_Click() {
    if (TagIndex == 1) {
        if (BaseResult) {
            if (BaseResult.DataGridView4) {
                if (BaseResult.DataGridView4.length > 0) {
                    BaseResult.DataGridView4.splice(DataGridView4, 1);
                    $("#BOX_QTY").val("0");
                }
            }
        }
    }
    if (TagIndex == 3) {
        if (BaseResult) {
            if (BaseResult.DataGridView4) {
                if (BaseResult.DataGridView4.length > 0) {
                    for (let i = 0; i < BaseResult.T_DGV_02.length; i++) {
                        if (BaseResult.T_DGV_02[i].CHK == true) {
                            BaseResult.T_DGV_02[i].CHK = false;
                        }
                    }
                }
            }
        }
    }
}
function Buttoninport_Click() {
    if (TagIndex == 1) {
        DGV_CLS = true;
        $("#BOX_QTY").val("0");
        $("#TBBarcode").val();
        $("#TBBarcode").focus();
    }
}
function Buttonexport_Click() {
    let tbodySelector, headerSelector, fileNamePrefix, sheetName;
    if (TagIndex == 1) {
        TableHTMLToExcel("DataGridView4Table", "B04Release", "B04Release");
    }
    if (TagIndex == 2) {
        TableHTMLToExcel("DGV_LISTTable", "B04ReleaseList", "B04ReleaseList");
    }
    if (TagIndex == 3) {        
        $("#BackGround").css("display", "block");        
        let CCC = $("#TBTEXT03").val();
        let BaseParameter = new Object();
        BaseParameter = {
            Action: TagIndex,            
        }
        BaseParameter.SearchString = CCC;       

        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        let url = "/B04/Buttonexport_Click";

        fetch(url, {
            method: "POST",
            body: formUpload,
            headers: {
            }
        }).then((response) => {
            response.json().then((data) => {                  
                window.open(data.Code, "_blank");
                $("#BackGround").css("display", "none");
            }).catch((err) => {
                $("#BackGround").css("display", "none");
            })
        });
    }
}
function Buttonprint_Click() {
    let IsSave = true;
    if (TagIndex == 1) {
        if (DGV_CLS == true) {
            IsSave = false;
        }
        let po_result = "Ing......";
        let DGV_result1 = "ERROR : ";
        let DGV_result2 = "";

        if (BaseResult) {
            if (BaseResult.DataGridView4) {
                if (BaseResult.DataGridView4.length < 0) {
                    po_result = "No Data... Add Barcode...";
                    alert(po_result + "Please Check Again.");
                    IsSave = false;
                }
            }
        }
        if (IsSave == true) {
            $("#BackGround").css("display", "block");
            let BaseParameter = new Object();
            BaseParameter = {
                Action: TagIndex,
            }
            BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
            BaseParameter.USER_ID = GetCookieValue("UserID");
            BaseParameter.DataGridView4 = BaseResult.DataGridView4;
            let formUpload = new FormData();
            formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
            let url = "/B04/Buttonprint_Click";

            fetch(url, {
                method: "POST",
                body: formUpload,
                headers: {
                }
            }).then((response) => {
                response.json().then((data) => {
                    let BaseResultPrint = data;
                    BaseResult.DataGridView4 = BaseResultPrint.DataGridView4;
                    BaseResult.DGV_result3 = BaseResultPrint.DGV_result3;
                    DataGridView4Render();
                    if (BaseResultPrint) {
                        if (BaseResultPrint.Code) {
                            let url = BaseResultPrint.Code;
                            OpenWindowByURL(url, 200, 200);
                        }
                    }
                    $("#BackGround").css("display", "none");
                }).catch((err) => {
                    $("#BackGround").css("display", "none");
                })
            });
        }

        po_result = "Finish"
        $("#TBpo_result").val(po_result);
        DGV_CLS = true;
        $("#BOX_QTY").val("0");
        $("#TBBarcode").val("");
        $("#TBBarcode").focus();
    }
}
function Buttonhelp_Click() {
    let url = "/WMP_PLAY";
    OpenWindowByURL(url, 200, 200);
}
function Buttonclose_Click() {
    history.back();
}
function RBchk() {
    let TLB_CHK1 = $("#TLB_CHK1").val();
    if (TLB_CHK1 == true) {
        if (BaseResult) {
            if (BaseResult.T_DGV_02) {
                if (BaseResult.T_DGV_02.length > 0) {
                    for (let i = 0; i < BaseResult.T_DGV_02.length; i++) {
                        BaseResult.T_DGV_02[i].CHK = TLB_CHK1;
                    }
                    T_DGV_02Render();
                }
            }
        }
    }
    let TLB_CHK2 = $("#TLB_CHK2").val();
    if (TLB_CHK2 == true) {
        if (BaseResult) {
            if (BaseResult.T_DGV_02) {
                if (BaseResult.T_DGV_02.length > 0) {
                    for (let i = 0; i < BaseResult.T_DGV_02.length; i++) {
                        BaseResult.T_DGV_02[i].CHK = !BaseResult.T_DGV_02[i].CHK;
                    }
                    T_DGV_02Render();
                }
            }
        }
    }
}
function DataGridView4_CellEndEdit() {
    if (BaseResult) {
        if (BaseResult.DataGridView4) {
            if (BaseResult.DataGridView4.length > 0) {
                let AAA = BaseResult.DataGridView4[DataGridView4RowIndex].PKG_QTY;
                let BBB = BaseResult.DataGridView4[DataGridView4RowIndex].MAX_PKG;
                if (AAA >= BBB) {
                    BaseResult.DataGridView4[DataGridView4RowIndex].PKG_QTY = BBB;
                    DataGridView4Render();
                }
            }
        }
    }
}
function DataGridView4_EditingControlShowing() {
    txtEdit_Keypress();
}
function txtEdit_Keypress() {

}
function T_DGV_01_Paint() {

}
function DataGridView4Render() {
    let HTML = "";
    if (BaseResult) {
        if (BaseResult.DGV_result3 == null) {
            BaseResult.DGV_result3 = true;
        }
        if (BaseResult.DataGridView4) {
            if (BaseResult.DataGridView4.length > 0) {
                DataGridView4_SelectionChanged(0);
                for (let i = 0; i < BaseResult.DataGridView4.length; i++) {
                    if (BaseResult.DataGridView4[i].RESULT == "Complete") {
                        HTML = HTML + "<tr onclick='DataGridView4_SelectionChanged(" + i + ")' style='background-color: lightgreen;'>";
                    }
                    else {
                        if (BaseResult.DGV_result3 == true) {
                            HTML = HTML + "<tr onclick='DataGridView4_SelectionChanged(" + i + ")'>";
                        }
                        else {
                            HTML = HTML + "<tr onclick='DataGridView4_SelectionChanged(" + i + ")' style='background-color: red;'>";
                        }
                    }
                    HTML = HTML + "<td>" + BaseResult.DataGridView4[i].RESULT + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView4[i].PART_IDX + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView4[i].PART_NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView4[i].PART_NM + "</td>";
                    HTML = HTML + "<td><input onblur='DataGridView4PKG_QTYChange(" + i + ", this)' id='DataGridView4PKG_QTY" + i + "' type='number' class='form-control' style='width: 100px;' value='" + BaseResult.DataGridView4[i].PKG_QTY + "'></td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView4[i].MTIN_DTM + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView4[i].QTY + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView4[i].PKG_GRP + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView4[i].PLET_NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView4[i].SHPD_NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView4[i].BARCD_ID + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView4[i].BC_DSCN_YN + "</td>";
                    //HTML = HTML + "<td>" + BaseResult.DataGridView4[i].PART_IDX + "</td>";
                    //HTML = HTML + "<td>" + BaseResult.DataGridView4[i].BARCD_IDX + "</td>";
                    //HTML = HTML + "<td>" + BaseResult.DataGridView4[i].PKG_GRP_IDX + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView4[i].R14 + "</td>";
                    HTML = HTML + "<td>";
                    HTML = HTML + "<select id='TMMTIN_SHEETNO" + i + "' class='browser-default form-select TMMTIN_SHEETNO'>";

                    if (BaseResult.DataGridView9.length > 0) {
                        BaseResult.DataGridView4[i].TMMTIN_DMM_IDX = BaseResult.DataGridView9[0].TMMTIN_SHEETNO;
                        BaseResult.DataGridView4[i].Name = BaseResult.DataGridView9[0].Name;
                        BaseResult.DataGridView4[i].TMMTIN_SHEETNO = BaseResult.DataGridView9[0].TMMTIN_SHEETNO;
                        for (let j = 0; j < BaseResult.DataGridView9.length; j++) {
                            HTML = HTML + "<option value='" + BaseResult.DataGridView9[j].TMMTIN_SHEETNO + "'>" + BaseResult.DataGridView9[j].Name + "</option>";
                        }
                    }
                    else {
                        BaseResult.DataGridView4[i].TMMTIN_DMM_IDX = 0;
                        BaseResult.DataGridView4[i].Name = "Không có Order";
                        //HTML = HTML + "<option value='" + BaseResult.DataGridView4[i].TMMTIN_DMM_IDX + "'>" + BaseResult.DataGridView4[i].Name + "</option>";
                    }
                    HTML = HTML + "<option value='0'>Không có Order</option>";
                    HTML = HTML + "</select>";
                    HTML = HTML + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView4[i].Reason + "</td>";
                    //HTML = HTML + "<td>" + BaseResult.DataGridView4[i].MAX_PKG + "</td>";
                    //HTML = HTML + "<td>" + BaseResult.DataGridView4[i].LOC + "</td>";
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
        let key = "PART_NO";
        IsTableSort = !IsTableSort;
        ListSort(BaseResult.DataGridView4, key, text, IsTableSort);
        DataGridView4Render();
    }
    let TMMTIN_SHEETNOID = "TMMTIN_SHEETNO" + DataGridView4RowIndex;
    BaseResult.DataGridView4[DataGridView4RowIndex].TMMTIN_DMM_IDX = $("#" + TMMTIN_SHEETNOID).val();
    let dropdown = document.getElementById(TMMTIN_SHEETNOID);
    const selectedText = dropdown.options[dropdown.selectedIndex].text;
    BaseResult.DataGridView4[DataGridView4RowIndex].Name = selectedText;
});



function DataGridView4_SelectionChanged(i) {
    DataGridView4RowIndex = i;
}
function DataGridView4PKG_QTYChange(i, input) {
    DataGridView4RowIndex = i;
    BaseResult.DataGridView4[DataGridView4RowIndex].PKG_QTY = input.value;
}
function DGV_LISTRender() {
    let HTML = "";
    if (BaseResult) {
        if (BaseResult.DGV_LIST) {
            if (BaseResult.DGV_LIST.length > 0) {
                DGV_LIST_SelectionChanged(0);
                for (let i = 0; i < BaseResult.DGV_LIST.length; i++) {
                    HTML = HTML + "<tr onclick='DGV_LIST_SelectionChanged(" + i + ")'>";
                    if (BaseResult.DGV_LIST[i].CHK) {
                        HTML = HTML + "<td><label><input id='DataGridView1CHK" + i + "' class='form-check-input' type='checkbox' checked><span></span></label></td>";
                    }
                    else {
                        HTML = HTML + "<td><label><input id='DataGridView1CHK" + i + "' class='form-check-input' type='checkbox'><span></span></label></td>";
                    }
                    HTML = HTML + "<td>" + BaseResult.DGV_LIST[i].BARCD_ID + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DGV_LIST[i].PART_NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DGV_LIST[i].PART_NM + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DGV_LIST[i].PART_CAR + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DGV_LIST[i].PART_FML + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DGV_LIST[i].PART_SCN + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DGV_LIST[i].Stock + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DGV_LIST[i].QTY + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DGV_LIST[i].PLET_NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DGV_LIST[i].SHPD_NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DGV_LIST[i].PKG_GRP + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DGV_LIST[i].PKG_QTY + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DGV_LIST[i].OUT_QTY + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DGV_LIST[i].MTIN_RMK + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DGV_LIST[i].MTIN_DTM + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DGV_LIST[i].BC_DSCN_YN + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DGV_LIST[i].UPDATE_DTM + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DGV_LIST[i].UPDATE_USER + "</td>";
                    //HTML = HTML + "<td>" + BaseResult.DGV_LIST[i].DSCN_YN + "</td>";
                    //HTML = HTML + "<td>" + BaseResult.DGV_LIST[i].IV_IDX + "</td>";
                    HTML = HTML + "</tr>";
                }
            }
        }
    }
    document.getElementById("DGV_LIST").innerHTML = HTML;
}

function DGV_LISTSort() {
    //IsTableSort = !IsTableSort;
    //DataGridViewSort(BaseResult.DGV_LIST, IsTableSort);
    //DGV_LISTRender();
}
let DGV_LISTTable = document.getElementById("DGV_LISTTable");
DGV_LISTTable.addEventListener('click', function (event) {
    if (event.target.tagName === 'TH') {
        let text = event.target.innerText;
        let key = "BARCD_ID";
        IsTableSort = !IsTableSort;
        ListSort(BaseResult.DGV_LIST, key, text, IsTableSort);
        DGV_LISTRender();
    }
});
function DGV_LIST_SelectionChanged(i) {
    DGV_LISTRowIndex = i;
}
function T_DGV_01Render() {
    let HTML = "";
    if (BaseResult) {
        if (BaseResult.T_DGV_01) {
            if (BaseResult.T_DGV_01.length > 0) {
                T_DGV_01_SelectionChanged(0);
                for (let i = 0; i < BaseResult.T_DGV_01.length; i++) {
                    if (BaseResult.T_DGV_01[i].TYPE == "IN") {
                        HTML = HTML + "<tr onclick='T_DGV_01_SelectionChanged(" + i + ")' style='background-color: lightgreen;'>";
                    }
                    else {
                        HTML = HTML + "<tr onclick='T_DGV_01_SelectionChanged(" + i + ")'>";
                    }
                    HTML = HTML + "<td>" + BaseResult.T_DGV_01[i].NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.T_DGV_01[i].TYPE + "</td>";
                    HTML = HTML + "<td>" + BaseResult.T_DGV_01[i].DateRDCE + "</td>";
                    HTML = HTML + "<td>" + BaseResult.T_DGV_01[i].SUM_QTY + "</td>";
                    HTML = HTML + "<td>" + BaseResult.T_DGV_01[i].STOCK_QTY + "</td>";
                    HTML = HTML + "<td>" + BaseResult.T_DGV_01[i].DATE + "</td>";
                    HTML = HTML + "<td>" + BaseResult.T_DGV_01[i].SHPD_NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.T_DGV_01[i].PLET_NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.T_DGV_01[i].USER + "</td>";
                    //HTML = HTML + "<td>" + BaseResult.T_DGV_01[i].Name + "</td>";
                    //HTML = HTML + "<td>" + BaseResult.T_DGV_01[i].Reason + "</td>";
                    HTML = HTML + "</tr>";
                }
            }
        }
    }
    document.getElementById("T_DGV_01").innerHTML = HTML;
}
function T_DGV_01Sort() {
    //IsTableSort = !IsTableSort;
    //DataGridViewSort(BaseResult.T_DGV_01, IsTableSort);    
    //T_DGV_01Render();
}
let T_DGV_01Table = document.getElementById("T_DGV_01Table");
T_DGV_01Table.addEventListener('click', function (event) {
    if (event.target.tagName === 'TH') {
        let text = event.target.innerText;
        let key = "TYPE";
        IsTableSort = !IsTableSort;
        ListSort(BaseResult.T_DGV_01, key, text, IsTableSort);
        T_DGV_01Render();
    }
});
function T_DGV_01SortSub(i) {
    IsTableSort = !IsTableSort;
    if (BaseResult.T_DGV_01) {
        if (IsTableSort == true) {
            switch (i) {
                case 1:
                    BaseResult.T_DGV_01.sort((a, b) => (a.NO > b.NO ? 1 : -1));
                    break;
                case 2:
                    BaseResult.T_DGV_01.sort((a, b) => (a.TYPE > b.TYPE ? 1 : -1));
                    break;
                case 3:
                    BaseResult.T_DGV_01.sort((a, b) => (a.DateRDCE > b.DateRDCE ? 1 : -1));
                    break;
                case 4:
                    BaseResult.T_DGV_01.sort((a, b) => (a.SUM_QTY > b.SUM_QTY ? 1 : -1));
                    break;
                case 5:
                    BaseResult.T_DGV_01.sort((a, b) => (a.STOCK_QTY > b.STOCK_QTY ? 1 : -1));
                    break;
                case 6:
                    BaseResult.T_DGV_01.sort((a, b) => (a.DATE > b.DATE ? 1 : -1));
                    break;
                case 7:
                    BaseResult.T_DGV_01.sort((a, b) => (a.SHPD_NO > b.SHPD_NO ? 1 : -1));
                    break;
                case 8:
                    BaseResult.T_DGV_01.sort((a, b) => (a.PLET_NO > b.PLET_NO ? 1 : -1));
                    break;
                case 9:
                    BaseResult.T_DGV_01.sort((a, b) => (a.USER > b.USER ? 1 : -1));
                    break;
            }
        }
        else {
            switch (i) {
                case 1:
                    BaseResult.T_DGV_01.sort((a, b) => (a.NO < b.NO ? 1 : -1));
                    break;
                case 2:
                    BaseResult.T_DGV_01.sort((a, b) => (a.TYPE < b.TYPE ? 1 : -1));
                    break;
                case 3:
                    BaseResult.T_DGV_01.sort((a, b) => (a.DateRDCE < b.DateRDCE ? 1 : -1));
                    break;
                case 4:
                    BaseResult.T_DGV_01.sort((a, b) => (a.SUM_QTY < b.SUM_QTY ? 1 : -1));
                    break;
                case 5:
                    BaseResult.T_DGV_01.sort((a, b) => (a.STOCK_QTY < b.STOCK_QTY ? 1 : -1));
                    break;
                case 6:
                    BaseResult.T_DGV_01.sort((a, b) => (a.DATE < b.DATE ? 1 : -1));
                    break;
                case 7:
                    BaseResult.T_DGV_01.sort((a, b) => (a.SHPD_NO < b.SHPD_NO ? 1 : -1));
                    break;
                case 8:
                    BaseResult.T_DGV_01.sort((a, b) => (a.PLET_NO < b.PLET_NO ? 1 : -1));
                    break;
                case 9:
                    BaseResult.T_DGV_01.sort((a, b) => (a.USER < b.USER ? 1 : -1));
                    break;
            }
        }
    }
    T_DGV_01Render();
}
function T_DGV_01_SelectionChanged(i) {
    T_DGV_01RowIndex = i;
}
function T_DGV_02Render() {
    let HTML = "";
    if (BaseResult) {
        if (BaseResult.T_DGV_02) {
            if (BaseResult.T_DGV_02.length > 0) {
                for (let i = 0; i < BaseResult.T_DGV_02.length; i++) {
                    if (BaseResult.T_DGV_02[i].CODE > 0) {
                        HTML = HTML + "<tr style='background-color: yellow; font-weight: bold;' onclick='T_DGV_02_SelectionChanged(" + i + ")'>";
                    }
                    else {
                        HTML = HTML + "<tr onclick='T_DGV_02_SelectionChanged(" + i + ")'>";
                    }
                    if (BaseResult.T_DGV_02[i].CHK) {
                        HTML = HTML + "<td><label><input id='DataGridView1CHK" + i + "' class='form-check-input' type='checkbox' checked><span></span></label></td>";
                    }
                    else {
                        HTML = HTML + "<td><label><input id='DataGridView1CHK" + i + "' class='form-check-input' type='checkbox'><span></span></label></td>";
                    }
                    HTML = HTML + "<td>" + BaseResult.T_DGV_02[i].PART_NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.T_DGV_02[i].PART_NAME + "</td>";
                    HTML = HTML + "<td>" + BaseResult.T_DGV_02[i].PART_SNP + "</td>";
                    HTML = HTML + "<td>" + BaseResult.T_DGV_02[i].PKG_QTY + "</td>";
                    HTML = HTML + "<td>" + BaseResult.T_DGV_02[i].PKG_OUTQTY + "</td>";
                    HTML = HTML + "<td>" + BaseResult.T_DGV_02[i].Pallet_NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.T_DGV_02[i].Shipping_NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.T_DGV_02[i].Receipt_Data + "</td>";
                    HTML = HTML + "<td>" + BaseResult.T_DGV_02[i].Receipt_USER + "</td>";
                    HTML = HTML + "<td>" + BaseResult.T_DGV_02[i].Release_Date + "</td>";
                    HTML = HTML + "<td>" + BaseResult.T_DGV_02[i].Release_USER + "</td>";
                    //HTML = HTML + "<td>" + BaseResult.T_DGV_02[i].BR_DSCN + "</td>";
                    //HTML = HTML + "<td>" + BaseResult.T_DGV_02[i].PKG_GRP + "</td>";
                    //HTML = HTML + "<td>" + BaseResult.T_DGV_02[i].MTIN_IDX + "</td>";
                    //HTML = HTML + "<td>" + BaseResult.T_DGV_02[i].PART_IDX + "</td>";
                    //HTML = HTML + "<td>" + BaseResult.T_DGV_02[i].BARCD_IDX + "</td>";
                    HTML = HTML + "<td>" + BaseResult.T_DGV_02[i].BARCD_ID + "</td>";
                    HTML = HTML + "<td>" + BaseResult.T_DGV_02[i].LOC + "</td>";
                    HTML = HTML + "<td>" + BaseResult.T_DGV_02[i].Name + "</td>";
                    HTML = HTML + "<td>" + BaseResult.T_DGV_02[i].Reason + "</td>";

                    /*HTML = HTML + "<td>" + BaseResult.T_DGV_02[i].OR_DT + "</td>";*/
                    HTML = HTML + "</tr>";
                }
            }
        }
    }
    document.getElementById("T_DGV_02").innerHTML = HTML;
}
function T_DGV_02Sort() {
    //IsTableSort = !IsTableSort;
    //if (BaseResult.T_DGV_02) {
    //    if (IsTableSort == true) {
    //        BaseResult.T_DGV_02.sort((a, b) => (a.Receipt_Data > b.Receipt_Data ? 1 : -1));
    //    }
    //    else {
    //        BaseResult.T_DGV_02.sort((a, b) => (a.Receipt_Data < b.Receipt_Data ? 1 : -1));
    //    }
    //}
    //DataGridViewSort(BaseResult.T_DGV_02, IsTableSort);
    //T_DGV_02Render();
}
let T_DGV_02Table = document.getElementById("T_DGV_02Table");
T_DGV_02Table.addEventListener('click', function (event) {
    if (event.target.tagName === 'TH') {
        let text = event.target.innerText;
        let key = "PART_NO";
        IsTableSort = !IsTableSort;
        switch (text) {
            case "IN DATE":
                if (IsTableSort == true) {
                    BaseResult.T_DGV_02.sort((a, b) => (a.Receipt_Data > b.Receipt_Data ? 1 : -1));
                }
                else {
                    BaseResult.T_DGV_02.sort((a, b) => (a.Receipt_Data < b.Receipt_Data ? 1 : -1));
                }
                break;
            case "OUT DATE":
                if (IsTableSort == true) {
                    BaseResult.T_DGV_02.sort((a, b) => (a.Release_Date > b.Release_Date ? 1 : -1));
                }
                else {
                    BaseResult.T_DGV_02.sort((a, b) => (a.Release_Date < b.Release_Date ? 1 : -1));
                }
                break;
            default:
                ListSort(BaseResult.T_DGV_02, key, text, IsTableSort);
                break;
        }
        T_DGV_02Render();
    }
});
function T_DGV_02SortReceipt_Data() {
    IsTableSort = !IsTableSort;
    if (BaseResult.T_DGV_02) {
        if (IsTableSort == true) {
            BaseResult.T_DGV_02.sort((a, b) => (a.Receipt_Data > b.Receipt_Data ? 1 : -1));
        }
        else {
            BaseResult.T_DGV_02.sort((a, b) => (a.Receipt_Data < b.Receipt_Data ? 1 : -1));
        }
    }
    T_DGV_02Render();
}
function T_DGV_02SortRelease_Date() {
    IsTableSort = !IsTableSort;
    if (BaseResult.T_DGV_02) {
        if (IsTableSort == true) {
            BaseResult.T_DGV_02.sort((a, b) => (a.Release_Date > b.Release_Date ? 1 : -1));
        }
        else {
            BaseResult.T_DGV_02.sort((a, b) => (a.Release_Date < b.Release_Date ? 1 : -1));
        }
    }
    T_DGV_02Render();
}
function T_DGV_02SortSub(i) {
    IsTableSort = !IsTableSort;
    if (BaseResult.T_DGV_02) {
        if (IsTableSort == true) {
            switch (i) {
                case 9:
                    BaseResult.T_DGV_02.sort((a, b) => (a.Receipt_Data > b.Receipt_Data ? 1 : -1));
                    break;
                case 11:
                    BaseResult.T_DGV_02.sort((a, b) => (a.Release_Date > b.Release_Date ? 1 : -1));
                    break;
            }
        }
        else {
            switch (i) {
                case 9:
                    BaseResult.T_DGV_02.sort((a, b) => (a.Receipt_Data < b.Receipt_Data ? 1 : -1));
                    break;
                case 11:
                    BaseResult.T_DGV_02.sort((a, b) => (a.Release_Date < b.Release_Date ? 1 : -1));
                    break;
            }
        }
    }
    T_DGV_02Render();
}
function T_DGV_02_SelectionChanged(i) {
    T_DGV_02RowIndex = i;

}
function DataGridView1Render() {
    let HTML = "";
    if (BaseResult) {
        if (BaseResult.DataGridView1) {
            if (BaseResult.DataGridView1.length > 0) {
                for (let i = 0; i < BaseResult.DataGridView1.length; i++) {
                    HTML = HTML + "<tr onclick='DataGridView1_SelectionChanged(" + i + ")'>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].YEAR + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].MONTH + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].WEEK + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].P_COUNT + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].BC_COUNT + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].QTY + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].IN_QTY + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].OUT_QTY + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].PO_PLAN + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].STAY_QTY + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].STAY_RATIO + "</td>";
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
function DataGridView2Render() {
    let HTML = "";
    if (BaseResult) {
        if (BaseResult.DataGridView2) {
            if (BaseResult.DataGridView2.length > 0) {
                for (let i = 0; i < BaseResult.DataGridView2.length; i++) {
                    HTML = HTML + "<tr onclick='DataGridView2_SelectionChanged(" + i + ")'>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].PART_NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].UTM + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].DESC + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].YEAR + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].MONTH + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].WEEK + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].BC_COUNT + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].QTY + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].IN_QTY + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].OUT_QTY + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].PO_PLAN + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].STAY_QTY + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].STAY_RATIO + "</td>";
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
function CALL_STOCK() {
    if (BaseResult) {
        if (BaseResult.CALL_STOCK) {
            if (BaseResult.CALL_STOCK.length > 0) {
                $("#TLB_05").val(BaseResult.CALL_STOCK[0].PART_NO);
                $("#TLB_07").val(BaseResult.CALL_STOCK[0].PART_NM);
                $("#TLB_09").val(BaseResult.CALL_STOCK[0].QTY);
                $("#TLB_10").val(BaseResult.CALL_STOCK[0].MT_EXCEL);
            }
        }
    }
}
function BTBCCHK_Click() {
    let pi_barcode = $("#TBBarcode").val();
    let po_result = "Ing......";
    if (pi_barcode == "") {
        alert("Check BarCode ID. Please Check Again.");
    }
    else {
        if (DGV_CLS == true) {
            let i = 0;
            BaseResult.DataGridView4 = [];
            DataGridView4Render();
            DGV_CLS = false;
        }

        $("#BackGround").css("display", "block");
        let BaseParameter = new Object();
        BaseParameter = {
            SearchString: pi_barcode,
        }
        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        let url = "/B04/BTBCCHK_Click";

        fetch(url, {
            method: "POST",
            body: formUpload,
            headers: {
            }
        }).then((response) => {
            response.json().then((data) => {
                let BaseResultBTBCCHK = data;
                BaseResult.DataGridView9 = BaseResultBTBCCHK.DataGridView9;
                let IsDGV_B04_03 = false;
                let IsNext = true;
                if (BaseResultBTBCCHK) {
                    if (BaseResultBTBCCHK.DGV_B04_03) {
                        if (BaseResultBTBCCHK.DGV_B04_03.length > 0) {
                            IsDGV_B04_03 = true;
                            R1 = BaseResultBTBCCHK.DGV_B04_03[0].PART_NO;
                            R2 = BaseResultBTBCCHK.DGV_B04_03[0].PART_NM;
                            R3 = BaseResultBTBCCHK.DGV_B04_03[0].PKG_QTY;
                            R4 = BaseResultBTBCCHK.DGV_B04_03[0].MTIN_DTM;
                            R5 = BaseResultBTBCCHK.DGV_B04_03[0].QTY;
                            R6 = BaseResultBTBCCHK.DGV_B04_03[0].PKG_GRP;
                            R7 = BaseResultBTBCCHK.DGV_B04_03[0].PLET_NO;
                            R8 = BaseResultBTBCCHK.DGV_B04_03[0].SHPD_NO;
                            R9 = BaseResultBTBCCHK.DGV_B04_03[0].BARCD_ID;
                            R10 = BaseResultBTBCCHK.DGV_B04_03[0].BC_DSCN_YN;
                            R11 = BaseResultBTBCCHK.DGV_B04_03[0].PART_IDX;
                            R12 = BaseResultBTBCCHK.DGV_B04_03[0].BARCD_IDX;
                            R13 = BaseResultBTBCCHK.DGV_B04_03[0].PKG_GRP_IDX;
                            R15 = BaseResultBTBCCHK.DGV_B04_03[0].MAX_PKG;
                            R16 = BaseResultBTBCCHK.DGV_B04_03[0].LOC;

                            R14 = 0;
                            let BC_II = 0;
                            let BC_JJ = 0;
                            if (BaseResult) {
                                if (BaseResult.DataGridView4) {
                                    R14 = BaseResult.DataGridView4.length + 1;
                                    BC_JJ = BaseResult.DataGridView4.length;
                                }
                            }
                            let BC_NM = pi_barcode.toUpperCase();
                            for (BC_II = 0; BC_II < BC_JJ; BC_II++) {
                                let BARCD_ID = BaseResult.DataGridView4[BC_II].BARCD_ID.toUpperCase();
                                if (BC_NM == BARCD_ID) {
                                    PlayErrorSound();
                                    alert("중복 바코드 발생 하였습니다. Cùng một mã vạch Một lỗi đã xảy ra.");
                                    $("#TBBarcode").val("");
                                    $("#TBBarcode").focus();
                                    IsNext = false;
                                }
                            }
                            if (IsNext == true) {
                                $("#BackGround").css("display", "block");
                                let BaseParameter = new Object();
                                BaseParameter = {
                                    ListSearchString: [],
                                }
                                BaseParameter.ListSearchString.push(R1);
                                BaseParameter.ListSearchString.push(R4);
                                let formUpload = new FormData();
                                formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
                                let url = "/B04/BTBCCHK_ClickSub";

                                fetch(url, {
                                    method: "POST",
                                    body: formUpload,
                                    headers: {
                                    }
                                }).then((response) => {
                                    response.json().then((data) => {
                                        let BaseResultBTBCCHK_ClickSub = data;
                                        if (BaseResultBTBCCHK_ClickSub.DGV_B04_02.length > 0) {
                                            //let url = "/B04_ERROR";
                                            //OpenWindowByURL(url, 400, 400);
                                            //B04_ERRORStartInterval();
                                            PlayErrorSound();
                                            $("#B04_ERROR").modal("open");

                                        }
                                        else {
                                            B04_ERRORMAS_CHK = "TRUE";
                                            localStorage.setItem("B04_ERRORMAS_CHK", "TRUE");
                                            BTBCCHK_ClickSub();
                                        }

                                        $("#BackGround").css("display", "none");
                                    }).catch((err) => {
                                        $("#BackGround").css("display", "none");
                                    })
                                });

                            }
                        }
                    }
                }

                if (IsDGV_B04_03 == false) {
                    PlayErrorSound();
                    po_result = "No goods receipt information(입고정보가 없습니다.)";
                    $("#TBpo_result").val(po_result);
                    $("#TBBarcode").val("");
                    $("#TBBarcode").focus();
                    $("#BackGround").css("display", "none");
                }
            }).catch((err) => {
                alert(localStorage.getItem("ERROR"));
                $("#BackGround").css("display", "none");
            })
        });
    }
}
function B04_ERRORStartInterval() {
    B04_ERRORTimer = setInterval(function () {
        let B04_ERROR_Close = localStorage.getItem("B04_ERROR_Close");
        if (B04_ERROR_Close == "1") {
            clearInterval(B04_ERRORTimer);
            B04_ERROR_Close = 0;
            localStorage.setItem("B04_ERROR_Close", B04_ERROR_Close);
            BTBCCHK_ClickSub();
        }
    }, 100);
}
function BTBCCHK_ClickSub() {
    //$("#BackGround").css("display", "none");   
    if (B04_ERRORMAS_CHK == "TRUE") {
        B04_ERRORMAS_CHK = "";
        if (BaseResult) {
            if (BaseResult.DataGridView4 == null) {
                BaseResult.DataGridView4 = new Object();
            }
            if (BaseResult.DataGridView4) {
                let DataGridView4Sub = new Object();
                DataGridView4Sub = {
                    RESULT: "Stay",
                    PART_NO: R1,
                    PART_NM: R2,
                    PKG_QTY: R3,
                    MTIN_DTM: R4,
                    QTY: R5,
                    PKG_GRP: R6,
                    PLET_NO: R7,
                    SHPD_NO: R8,
                    BARCD_ID: R9,
                    BC_DSCN_YN: R10,
                    PART_IDX: R11,
                    BARCD_IDX: R12,
                    PKG_GRP_IDX: R13,
                    R14: R14,
                    MAX_PKG: R15,
                    LOC: R16,
                    ReasonID: B04_ERROR_ReasonID,
                    Reason: B04_ERROR_Reason,
                }
                BaseResult.DataGridView4.push(DataGridView4Sub);
                DataGridView4Render();
                PlaySuccessSound();
                $("#BOX_QTY").val(BaseResult.DataGridView4.length);
            }
        }
        $("#TBBarcode").val("");
        $("#TBBarcode").focus();
    }
}
function TLB_01StartInterval() {
    setInterval(function () {
        $("#TBTEXT01").val(localStorage.getItem("B04_2BBB"));
    }, 100);
}
function Load() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {

    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/B04/Load";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            let BaseResultLoad = data;
            BaseResult.DataGridView4 = BaseResultLoad.DataGridView4;
            DataGridView4Render();
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}

//const table = document.getElementById("T_DGV_02Table");
//const ths = table.querySelectorAll('th');
//const thsArray = Array.from(ths);

//table.addEventListener('click', (event) => {
//    const columnIndex = thsArray.findIndex(row => row.contains(event.target));
//    const columnValues = [];
//    for (let i = 1; i < table.rows.length; i++) {
//        const row = table.rows[i];
//        if (row.cells.length > columnIndex) {
//            columnValues.push(row.cells[columnIndex].textContent);
//        }
//    }
//    if (columnValues.length > 0) {
//        const columnString = columnValues.join('\n');
//        if (navigator.clipboard) {
//            navigator.clipboard.writeText(columnString);
//        }
//        else {
//            alert(columnString);
//        }
//    }
//});

//const table = document.getElementById("T_DGV_01Table");
//const headers = table.querySelectorAll('th');

//headers.forEach((header, index) => {
//    header.addEventListener('click', () => {
//        const isAscending = header.classList.contains('asc');
//        sortTable(table, index, !isAscending);

//        headers.forEach(h => h.classList.remove('asc', 'desc'));
//        header.classList.add(isAscending ? 'desc' : 'asc');
//    });
//});

//function sortTable(table, columnIndex, ascending = true) {
//    const tbody = table.querySelector('tbody');
//    const rows = Array.from(tbody.querySelectorAll('tr'));

//    const collator = new Intl.Collator(undefined, { numeric: true, sensitivity: 'base' });

//    const sortedRows = rows.sort((rowA, rowB) => {
//        const cellA = rowA.querySelectorAll('td')[columnIndex].textContent.trim();
//        const cellB = rowB.querySelectorAll('td')[columnIndex].textContent.trim();

//        return ascending ? collator.compare(cellA, cellB) : collator.compare(cellB, cellA);
//    });

//    tbody.append(...sortedRows);
//}
let B04_ERROR_ReasonID = 0;
let B04_ERROR_Reason = "";
$("#B04_ERROR_Button1").click(function () {
    B04_ERROR_Button1_Click();
});
$("#B04_ERROR_Button2").click(function () {
    B04_ERROR_Button2_Click();
});
function B04_ERROR_Button1_Click() {
    B04_ERRORMAS_CHK = "TRUE";
    $("#B04_ERROR").modal("close");

    if (document.getElementById("Reason01").checked == true) {
        B04_ERROR_ReasonID = 1;
        B04_ERROR_Reason = document.getElementById("Reason01Text").innerHTML;
    }
    if (document.getElementById("Reason02").checked == true) {
        B04_ERROR_ReasonID = 2;
        B04_ERROR_Reason = document.getElementById("Reason02Text").innerHTML;
    }
    if (document.getElementById("Reason03").checked == true) {
        B04_ERROR_ReasonID = 3;
        B04_ERROR_Reason = document.getElementById("Reason03Text").innerHTML;
    }
    if (document.getElementById("Reason04").checked == true) {
        B04_ERROR_ReasonID = 4;
        B04_ERROR_Reason = document.getElementById("Reason04Text").innerHTML;
    }
    BTBCCHK_ClickSub();
}
function B04_ERROR_Button2_Click() {
    B04_ERRORMAS_CHK = "FALSE";
    $("#B04_ERROR").modal("close");
    BTBCCHK_ClickSub();
}
function DataGridView7Render() {
    let HTML = "";
    if (BaseResult) {
        if (BaseResult.DataGridView7) {
            if (BaseResult.DataGridView7.length > 0) {
                for (let i = 0; i < BaseResult.DataGridView7.length; i++) {
                    let No = i + 1;
                    HTML = HTML + "<tr onclick='DataGridView7_SelectionChanged(" + i + ")'>";
                    HTML = HTML + "<td>" + No + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView7[i].BARCD_ID + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView7[i].OUT_DTM + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView7[i].PKG_QTY + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView7[i].PKG_OUTQTY + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView7[i].Reason + "</td>";
                    HTML = HTML + "</tr>";
                }
            }
        }
    }
    document.getElementById("DataGridView7").innerHTML = HTML;
}
let DataGridView7RowIndex = 0;
function DataGridView7_SelectionChanged(i) {
    DataGridView7RowIndex = i;
}
function DataGridView8Render() {
    let HTML = "";
    if (BaseResult) {
        if (BaseResult.DataGridView8) {
            if (BaseResult.DataGridView8.length > 0) {
                for (let i = 0; i < BaseResult.DataGridView8.length; i++) {
                    let No = i + 1;
                    HTML = HTML + "<tr onclick='DataGridView8_SelectionChanged(" + i + ")'>";
                    HTML = HTML + "<td>" + No + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView8[i].BARCD_ID + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView8[i].OUT_DTM + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView8[i].PKG_QTY + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView8[i].PKG_OUTQTY + "</td>";
                    /*HTML = HTML + "<td>" + BaseResult.DataGridView8[i].Reason + "</td>";*/
                    HTML = HTML + "</tr>";
                }
            }
        }
    }
    document.getElementById("DataGridView8").innerHTML = HTML;
}
let DataGridView8RowIndex = 0;
function DataGridView8_SelectionChanged(i) {
    DataGridView8RowIndex = i;
}
function DGV_B07_31Render() {
    let HTML = "";
    if (BaseResult) {
        if (BaseResult.DGV_B07_31) {
            if (BaseResult.DGV_B07_31.length > 0) {
                for (let i = 0; i < BaseResult.DGV_B07_31.length; i++) {
                    let No = i + 1;
                    HTML = HTML + "<tr onclick='DGV_B07_31_SelectionChanged(" + i + ")'>";
                    HTML = HTML + "<td>" + No + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DGV_B07_31[i].PART_NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DGV_B07_31[i].BARCD_ID + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DGV_B07_31[i].PKG_QTY + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DGV_B07_31[i].PKG_OUTQTY + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DGV_B07_31[i].OUT_QTY + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DGV_B07_31[i].QuantityGAP + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DGV_B07_31[i].OUT_DTM + "</td>";                   
                    HTML = HTML + "</tr>";
                }
            }
        }
    }
    document.getElementById("DGV_B07_31").innerHTML = HTML;
}
let DGV_B07_31RowIndex = 0;
function DGV_B07_31_SelectionChanged(i) {
    DGV_B07_31RowIndex = i;
    Gettmbrcd_hisByBARCD_IDXToListAsync();
}


function Gettmbrcd_hisByBARCD_IDXToListAsync() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        Action: TagIndex,
        ListSearchString: [],
    }
    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
    BaseParameter.USER_ID = GetCookieValue("UserID");
    BaseParameter.ListSearchString.push(BaseResult.DGV_B07_31[DGV_B07_31RowIndex].BARCD_IDX);
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/B04/Gettmbrcd_hisByBARCD_IDX";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            BaseResult.DGV_B07_32 = data.DGV_B07_32;
            DGV_B07_32Render();
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}

function DGV_B07_32Render() {
    let HTML = "";
    if (BaseResult) {
        if (BaseResult.DGV_B07_32) {
            if (BaseResult.DGV_B07_32.length > 0) {
                for (let i = 0; i < BaseResult.DGV_B07_32.length; i++) {
                    let No = i + 1;
                    HTML = HTML + "<tr onclick='DGV_B07_32_SelectionChanged(" + i + ")'>";
                    HTML = HTML + "<td>" + No + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DGV_B07_32[i].BARCD_IDX + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DGV_B07_32[i].BARCD_ID + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DGV_B07_32[i].PKG_QTY + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DGV_B07_32[i].PKG_OUTQTY + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DGV_B07_32[i].OUT_DTM + "</td>";
                    //HTML = HTML + "<td>" + BaseResult.DGV_B07_32[i].UPDATE_DTM + "</td>";
                    //HTML = HTML + "<td>" + BaseResult.DGV_B07_32[i].UPDATE_USER + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DGV_B07_32[i].USER_NM + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DGV_B07_32[i].Name + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DGV_B07_32[i].Reason + "</td>";
                    HTML = HTML + "</tr>";
                }
            }
        }
    }
    document.getElementById("DGV_B07_32").innerHTML = HTML;
}
let DGV_B07_32RowIndex = 0;
function DGV_B07_32_SelectionChanged(i) {
    DGV_B07_32RowIndex = i;
}