let IsTableSort = false;
let BaseResult;
let RowIndex;
let Action = 1;
let B03AddTextBox1;
let DateNow;
$(document).ready(function () {
    var now = new Date();
    DateNow = DateToString(now);
    $("#Date1").val(DateNow);
    $("#rbchk1").prop("checked", true);

    //RowIndex = Number(localStorage.getItem("B03RowIndex"));
    //let Date1 = localStorage.getItem("B03Date1");
    //$("#TBsuch1").val(localStorage.getItem("B03TBsuch1"));
    //$("#TBsuch2").val(localStorage.getItem("B03TBsuch2"));
    //$("#TBsuch3").val(localStorage.getItem("B03TBsuch3"));
    //$("#TBsuch4").val(localStorage.getItem("B03TBsuch4"));
    //if (Date1) {    
    //    $("#Date1").val(Date1);
    //    Buttonfind_Click();
    //    DataGridView_SelectionChanged(RowIndex);
    //}
});
function StartInterval() {
    setInterval(function () {
        Buttonadd_ClickSub();
    }, 100);
}
function Buttonadd_ClickSub() {
    let B03AddTextBox1 = localStorage.getItem("B03AddTextBox1");
    localStorage.setItem("B03AddTextBox1", "");
    if (B03AddTextBox1) {
        if (B03AddTextBox1.length > 0) {
            let BaseParameter = new Object();
            BaseParameter = {
                SearchString: B03AddTextBox1,
            }
            if (BaseParameter.SearchString) {
                if (BaseParameter.SearchString.length > 0) {
                    $("#BackGround").css("display", "block");
                    Action = 0;
                    $("#Buttonsave").text("Save");
                    $("#TBsuch1").val("");
                    let formUpload = new FormData();
                    formUpload.append("BaseParameter", JSON.stringify(BaseParameter));
                    let url = "/B03/Buttonadd_Click";

                    fetch(url, {
                        method: "POST",
                        body: formUpload,
                        headers: {
                        }
                    }).then((response) => {
                        response.json().then((data) => {
                            let BaseResultButtonadd = data;
                            if (BaseResultButtonadd) {
                                if (BaseResultButtonadd.DGV_B03_01) {
                                    if (BaseResultButtonadd.DGV_B03_01.length > 0) {
                                        let suchidx = BaseResultButtonadd.DGV_B03_01[0].PART_IDX;
                                        let suchpartnm = BaseResultButtonadd.DGV_B03_01[0].PART_NM;
                                        let suchspn = BaseResultButtonadd.DGV_B03_01[0].PART_SNP;
                                        let idxchk = true;
                                        if (idxchk == true) {
                                            $("#TBPARTNO").val(BaseParameter.SearchString);
                                            $("#TBPARTIDX").val(suchidx);
                                            $("#TBPARTNM").val(suchpartnm);
                                            $("#TBPARTSNP").val(suchspn);
                                            $("#TB_01").val("");
                                            $("#TB_02").val("");
                                            $("#TB_03").val("");
                                            $("#TB_04").val("");
                                            $("#TB_05").val("");
                                            $("#TB_06").val("");
                                            $("#TB_03").focus();
                                        }
                                    }
                                    else {
                                        alert("Part number Not found. Please Check Again.");
                                    }
                                }
                            }
                            $("#BackGround").css("display", "none");
                        }).catch((err) => {
                            alert("Part number Not found. Please Check Again.");
                            $("#BackGround").css("display", "none");
                        })
                    });
                }
                else {
                    alert("Part number Not found. Please Check Again.");
                }
            }
            else {
                alert("Part number Not found. Please Check Again.");
            }
        }
    }
}
$("#TBsuch1").keydown(function (e) {
    if (e.keyCode == 13) {
        Buttonfind_Click();
    }
});
$("#TBsuch2").keydown(function (e) {
    if (e.keyCode == 13) {
        Buttonfind_Click();
    }
});
$("#TBsuch3").keydown(function (e) {
    if (e.keyCode == 13) {
        Buttonfind_Click();
    }
});
$("#TBsuch4").keydown(function (e) {
    if (e.keyCode == 13) {
        Buttonfind_Click();
    }
});
$("TB_08").focusout(function () {
    BaseResult.DataGridView[RowIndex].PART_LOC = $("#TB_08").val();
});
$("#rbchk1").click(function () {
    for (let i = 0; i < BaseResult.DataGridView.length; i++) {
        BaseResult.DataGridView[i].CHK = true;
    }
    DataGridViewRender();
});
$("#rbchk2").click(function () {
    for (let i = 0; i < BaseResult.DataGridView.length; i++) {
        BaseResult.DataGridView[i].CHK = !BaseResult.DataGridView[i].CHK;
    }
    DataGridViewRender();
});
$("#Button2").click(function () {
    let url = "/B03_1";
    OpenWindowByURL(url, 300, 200);
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
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        ListSearchString: [],
    }
    BaseParameter.ListSearchString.push($("#Date1").val());
    BaseParameter.ListSearchString.push($("#TBsuch1").val());
    BaseParameter.ListSearchString.push($("#TBsuch2").val());
    BaseParameter.ListSearchString.push($("#TBsuch3").val());
    BaseParameter.ListSearchString.push($("#TBsuch4").val());

    let formUpload = new FormData();
    formUpload.append("BaseParameter", JSON.stringify(BaseParameter));
    let url = "/B03/Buttonfind_Click";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            BaseResult = data;
            DataGridViewRender();
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
function Buttonadd_Click() {
    let url = "/B03Add";
    OpenWindowByURL(url, 800, 800);
    StartInterval();
}
function Buttonsave_Click() {
    let IsSave = true;
    let TB_03 = $("#TB_03").val();
    if (TB_03 == "") {
        alert(localStorage.getItem("ERRORPleaseCheckAgain"));
        IsSave = false;
        $("#TB_03").focus();
    }
    else {
        if (TB_03.includes("e")) {
            alert(localStorage.getItem("ERRORPleaseCheckAgain"));
            IsSave = false;
            $("#TB_03").focus();
        }
        let TB_03Number = Number(TB_03);
        if (TB_03Number < 0) {
            alert(localStorage.getItem("ERRORPleaseCheckAgain"));
            IsSave = false;
            $("#TB_03").focus();
        }
        else {
            if (TB_03Number >= 10000000000) {
                alert(localStorage.getItem("ERRORPleaseCheckAgain"));
                IsSave = false;
                $("#TB_03").focus();
            }
        }
    }
    //let TB_04 = $("#TB_04").val();
    //if (TB_04 == "") {
    //    alert(localStorage.getItem("ERRORPleaseCheckAgain"));
    //    IsSave = false;
    //    $("#TB_04").focus();
    //}
    //else {
    //    if (TB_04.includes("e")) {
    //        alert(localStorage.getItem("ERRORPleaseCheckAgain"));
    //        IsSave = false;
    //        $("#TB_04").focus();
    //    }
    //    let TB_04Number = Number(TB_04);
    //    if (TB_04Number < 0) {
    //        alert(localStorage.getItem("ERRORPleaseCheckAgain"));
    //        IsSave = false;
    //        $("#TB_04").focus();
    //    }
    //}
    //let TB_05 = $("#TB_05").val();
    //if (TB_05 == "") {
    //    alert(localStorage.getItem("ERRORPleaseCheckAgain"));
    //    IsSave = false;
    //    $("#TB_05").focus();
    //}
    //else {
    //    if (TB_05.includes("e")) {
    //        alert(localStorage.getItem("ERRORPleaseCheckAgain"));
    //        IsSave = false;
    //        $("#TB_05").focus();
    //    }
    //    let TB_05Number = Number(TB_05);
    //    if (TB_05Number < 0) {
    //        alert(localStorage.getItem("ERRORPleaseCheckAgain"));
    //        IsSave = false;
    //        $("#TB_05").focus();
    //    }
    //}
    let TBPARTSNP = $("#TBPARTSNP").val();
    if (TBPARTSNP.length > 9) {
        alert(localStorage.getItem("ERRORPleaseCheckAgain"));
        IsSave = false;
        $("#TBPARTSNP").focus();
    }
    let TB_01 = $("#TB_01").val();
    if (TB_01.length > 5) {
        alert(localStorage.getItem("ERRORPleaseCheckAgain"));
        IsSave = false;
        $("#TB_01").focus();
    }
    if (IsSave == true) {
        $("#BackGround").css("display", "block");
        let BaseParameter = new Object();
        BaseParameter = {
            SearchString: $("#Buttonsave").text(),
            Action: Action,
            ListSearchString: [],
        }

        BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
        BaseParameter.ListSearchString.push($('#TBPARTIDX').val());
        BaseParameter.ListSearchString.push($('#TBPARTNM').val());
        BaseParameter.ListSearchString.push($('#TB_01').val());
        BaseParameter.ListSearchString.push($('#TB_03').val());
        BaseParameter.ListSearchString.push($('#TB_04').val());
        BaseParameter.ListSearchString.push($('#TB_05').val());
        BaseParameter.ListSearchString.push($('#TB_06').val());
        BaseParameter.ListSearchString.push($('#TB_02').val());
        BaseParameter.ListSearchString.push($('#TB_07').val());
        BaseParameter.ListSearchString.push($('#TBPARTSNP').val());
        BaseParameter.ListSearchString.push($('#Date1').val());
        BaseParameter.ListSearchString.push($('#TBMTIN_IDX').val());
        let formUpload = new FormData();
        formUpload.append("BaseParameter", JSON.stringify(BaseParameter));
        let url = "/B03/Buttonsave_Click";

        fetch(url, {
            method: "POST",
            body: formUpload,
            headers: {
            }
        }).then((response) => {
            response.json().then((data) => {
                if (BaseParameter.Action == 0) {
                    alert("Save is complete.");
                }
                if (BaseParameter.Action == 1) {
                    alert("Edit is complete.");
                }
                Buttonfind_Click();
                $("#BackGround").css("display", "none");
            }).catch((err) => {
                alert("Try again from the beginning. Please Check Again.");
                $("#BackGround").css("display", "none");
            })
        });
        $("#Buttonsave").text("Edit");
        Action = 1;
    }
    else {
    }
}
function Buttondelete_Click() {
    let i = 0;
    while (i < BaseResult.DataGridView.length) {
        if (BaseResult.DataGridView[i].CHK == true) {
            BaseResult.DataGridView.splice(i, 1);
        }
        else {
            i++;
        }
    }
    DataGridViewRender();
}
function Buttoncancel_Click() {
    Action = 1;
    $("#Buttonsave").text("Edit");
    $("#TBPARTNO").val("");
    $("#TBPARTIDX").val("");
    $("#TBPARTNM").val("");
    $("#TBPARTSNP").val("");
    $("#TB_01").val("");
    $("#TB_02").val("");
    $("#TB_03").val("");
    $("#TB_04").val("");
    $("#TB_05").val("");
    $("#TB_06").val("");
}
function Buttoninport_Click() {
    //$("#BackGround").css("display", "block");
    //let BaseParameter = new Object();
    //BaseParameter = {
    //}
    //let formUpload = new FormData();
    //formUpload.append("BaseParameter", JSON.stringify(BaseParameter));
    //let url = "/B03/Buttoninport_Click";

    //fetch(url, {
    //    method: "POST",
    //    body: formUpload,
    //    headers: {
    //    }
    //}).then((response) => {
    //    response.json().then((data) => {
    //        $("#BackGround").css("display", "none");
    //    }).catch((err) => {
    //        $("#BackGround").css("display", "none");
    //    })
    //});
}
function Buttonexport_Click() {
    //$("#BackGround").css("display", "block");
    //let BaseParameter = new Object();
    //BaseParameter = {
    //}
    //let formUpload = new FormData();
    //formUpload.append("BaseParameter", JSON.stringify(BaseParameter));
    //let url = "/B03/Buttonexport_Click";

    //fetch(url, {
    //    method: "POST",
    //    body: formUpload,
    //    headers: {
    //    }
    //}).then((response) => {
    //    response.json().then((data) => {
    //        $("#BackGround").css("display", "none");
    //    }).catch((err) => {
    //        $("#BackGround").css("display", "none");
    //    })
    //});
}
function Buttonprint_Click() {
    let j = BaseResult.DataGridView.length - 1;
    if (j + 1 <= 0) {
        alert("Please select the MT Barcode(DATABASE).");
    }
    else {
        $("#BackGround").css("display", "block");
        localStorage.setItem("B03RowIndex", RowIndex);
        localStorage.setItem("B03Date1", $('#Date1').val());
        localStorage.setItem("B03TBsuch1", $('#TBsuch1').val());
        localStorage.setItem("B03TBsuch2", $('#TBsuch2').val());
        localStorage.setItem("B03TBsuch3", $('#TBsuch3').val());
        localStorage.setItem("B03TBsuch4", $('#TBsuch4').val());

        let BaseParameter = new Object();
        BaseParameter = {
        }
        BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
        BaseParameter.DataGridView = BaseResult.DataGridView;
        let formUpload = new FormData();
        formUpload.append("BaseParameter", JSON.stringify(BaseParameter));
        let url = "/B03/Buttonprint_Click";

        fetch(url, {
            method: "POST",
            body: formUpload,
            headers: {
            }
        }).then((response) => {
            response.json().then((data) => {
                let BaseResultButtonprint = data;
                if (BaseResultButtonprint) {
                    if (BaseResultButtonprint.Code) {                        
                        let url = BaseResultButtonprint.Code;
                        OpenWindowByURL(url, 400, 400);                           
                    }
                }
                $("#BackGround").css("display", "none");
            }).catch((err) => {
                $("#BackGround").css("display", "none");
            })
        });
        Buttondelete_Click();
    }
}
function Buttonhelp_Click() {
    let url = "/WMP_PLAY";
    OpenWindowByURL(url, 800, 460);
}
function Buttonclose_Click() {
    history.back();
}
function DataGridViewRender() {
    let HTML = "";
    if (BaseResult) {
        if (BaseResult.DataGridView) {
            if (BaseResult.DataGridView.length > 0) {
                DataGridView_SelectionChanged(0);
                for (let i = 0; i < BaseResult.DataGridView.length; i++) {
                    HTML = HTML + "<tr onclick='DataGridView_SelectionChanged(" + i + ")'>";
                    if (BaseResult.DataGridView[i].CHK) {
                        HTML = HTML + "<td onclick='DataGridViewCHKChanged(" + i + ")'><input id='DataGridViewCHK" + i + "' class='form-check-input' type='checkbox' checked><span></span></td>";
                    }
                    else {
                        HTML = HTML + "<td onclick='DataGridViewCHKChanged(" + i + ")'><input id='DataGridViewCHK" + i + "'' class='form-check-input' type='checkbox'><span></span></td>";
                    }
                    //HTML = HTML + "<td><input id='DataGridViewCHK" + i + "' class='form-check-input' type='checkbox'><span></span></td>";
                    HTML = HTML + "<td>" + new Date(BaseResult.DataGridView[i].MTIN_DTM.replace(/(\d{2})-(\d{2})-(\d{4})/, "$2/$1/$3")).toISOString().slice(0, 10) + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView[i].PART_NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView[i].PART_NM + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView[i].UTM + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView[i].SNP_QTY + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView[i].QTY + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView[i].NET_WT + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView[i].GRS_WT + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView[i].PLET_NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView[i].SHPD_NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView[i].PART_LOC + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView[i].MTIN_RMK + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView[i].CREATE_DTM + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView[i].CREATE_USER + "</td>";
                    HTML = HTML + "</tr>";
                }
            }
        }
    }
    document.getElementById("DataGridView").innerHTML = HTML;
}
function DataGridView1Sort() {
    //IsTableSort = !IsTableSort;
    //DataGridViewSort(BaseResult.DataGridView, IsTableSort);
    //DataGridViewRender();
}
let DataGridViewTable = document.getElementById("DataGridViewTable");
DataGridViewTable.addEventListener('click', function (event) {
    if (event.target.tagName === 'TH') {
        let text = event.target.innerText;
        let key = "PART_NO";
        IsTableSort = !IsTableSort;
        switch (text) {
            case "PALLET NO":
                if (IsTableSort == true) {
                    BaseResult.DataGridView.sort((a, b) => (a.PLET_NO > b.PLET_NO ? 1 : -1));
                }
                else {
                    BaseResult.DataGridView.sort((a, b) => (a.PLET_NO < b.PLET_NO ? 1 : -1));
                }
                break;            
            default:
                ListSort(BaseResult.T_DGV_02, key, text, IsTableSort);
                break;
        }
        DataGridViewRender();
    }
});
function DataGridView_SelectionChanged(index) {
    $("#Buttonsave").text("Edit");
    RowIndex = index;
    if (BaseResult) {
        if (BaseResult.DataGridView) {
            if (BaseResult.DataGridView.length > 0) {
                if (RowIndex >= 0) {
                    $("#TBPARTNO").val(BaseResult.DataGridView[RowIndex].PART_NO);
                    $("#TBPARTNM").val(BaseResult.DataGridView[RowIndex].PART_NM);
                    $("#TB_01").val(BaseResult.DataGridView[RowIndex].UTM);
                    $("#TB_02").val(BaseResult.DataGridView[RowIndex].SHPD_NO);
                    $("#TBPARTSNP").val(BaseResult.DataGridView[RowIndex].SNP_QTY);
                    $("#TB_03").val(BaseResult.DataGridView[RowIndex].QTY);
                    $("#TB_04").val(BaseResult.DataGridView[RowIndex].NET_WT);
                    $("#TB_05").val(BaseResult.DataGridView[RowIndex].GRS_WT);
                    $("#TB_06").val(BaseResult.DataGridView[RowIndex].PLET_NO);
                    $("#TB_07").val(BaseResult.DataGridView[RowIndex].MTIN_RMK);
                    $("#TBPARTIDX").val(BaseResult.DataGridView[RowIndex].PART_IDX);
                    $("#TBMTIN_IDX").val(BaseResult.DataGridView[RowIndex].MTIN_IDX);
                    $("#TB_08").val(BaseResult.DataGridView[RowIndex].PART_LOC);
                }
            }
        }
    }
}
function DataGridViewCHKChanged(index) {
    RowIndex = index;
    let id = "DataGridViewCHK" + RowIndex;
    for (let i = 0; i < BaseResult.DataGridView.length; i++) {
        if (i == RowIndex) {
            BaseResult.DataGridView[i].CHK = !BaseResult.DataGridView[i].CHK;
        }
    }
    DataGridViewRender();
}