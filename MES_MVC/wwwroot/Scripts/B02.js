let IsTableSort = false;
let BaseResult = new Object();
let TagIndex = 1;
let T1_CHK = true;
let BarcodeDGVRowIndex = 0;
let DataGridView1RowIndex = 0;
let TMMTINDGVRowIndex = 0;
let DateNow;
$(document).ready(function () {
    var now = new Date();
    DateNow = DateToString(now);
    $("#DTPsuck").val(DateNow);
    document.getElementById("Buttoncancel").disabled = true;
    document.getElementById("Buttoninport").disabled = true;
    document.getElementById("Buttonexport").disabled = true;
    document.getElementById("Buttonprint").disabled = true;
    BaseResult.BarcodeDGV = new Object();
    BaseResult.BarcodeDGV = [];

});
$("#TextBox1").keydown(function (e) {
    if (e.keyCode == 13) {
        Buttonfind_Click();
        if (T1_CHK == true) {
            document.getElementById("LOC_TextBox1").readOnly = true;
            $("#LOC_TextBox1").focus();
        }
        else {
            $("#LOC_TextBox1").val("");
            $("#LOC_TextBox1").focus();
        }
    }
});
$("#TextBox2").keydown(function (e) {
    if (e.keyCode == 13) {
        Buttonsave_Click();
        $("#LOC_TextBox1").focus();
    }
});
$("#TBBarcode").keydown(function (e) {
    if (e.keyCode == 13) {        
        TBBarcode_KeyDown();
    }
});
function TBBarcode_KeyDown() {    
    Buttonadd_Click();
}
$("#LOC_TextBox1").keydown(function (e) {
    if (e.keyCode == 13) {
        TextBox1_KeyDown();
    }
});
function TextBox1_KeyDown() {    
    Buttonfind_Click();   
}
$("#LOC_TextBox2").keydown(function (e) {
    if (e.keyCode == 13) {
        TextBox1_KeyDown();
    }
});
function TextBox2_KeyDown() {
    Buttonsave_Click();
    $("#LOC_TextBox1").focus();
}
$("#ATag001").click(function (e) {
    TagIndex = 1;
});
$("#ATag002").click(function (e) {
    TagIndex = 2;
    TabControl1_SelectedIndexChanged();
});
$("#ATag003").click(function (e) {
    TagIndex = 3;
    TabControl1_SelectedIndexChanged();
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
function TabControl1_SelectedIndexChanged() {
    if (TagIndex == 2) {
        document.getElementById("LOC_TextBox1").readOnly = false;
        $("#LOC_TextBox1").focus();
        $("#LOC_Label11").val("-");
        $("#LOC_Label12").val("-");
        $("#LOC_Label13").val("-");
        $("#LOC_Label14").val("-");
        $("#LOC_Label15").val("-");
        $("#LOC_TextBox1").val("");
        $("#LOC_TextBox2").val("");
        $("#LOC_TextBox1").focus();
    }
    if (TagIndex == 3) {
        document.getElementById("RadioButton1").checked = true;
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

    let DTPsuck = $("#DTPsuck").val();
    let LOC_TextBox1 = $("#LOC_TextBox1").val();
    let TextBox3 = $("#TextBox3").val();
    let RadioButton1 = $("#RadioButton1").val();
    let RadioButton2 = $("#RadioButton2").val();

    let BaseParameter = new Object();
    BaseParameter = {
        Action: TagIndex,
        ListSearchString: [],
    }
    BaseParameter.ListSearchString.push(DTPsuck);
    BaseParameter.ListSearchString.push(LOC_TextBox1);
    BaseParameter.ListSearchString.push(TextBox3);
    BaseParameter.ListSearchString.push(RadioButton1);
    BaseParameter.ListSearchString.push(RadioButton2);

    if (TagIndex == 1) {        
        $("#BackGround").css("display", "block");
        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        let url = "/B02/Buttonfind_Click";

        fetch(url, {
            method: "POST",
            body: formUpload,
            headers: {
            }
        }).then((response) => {
            response.json().then((data) => {
                BaseResult = data;
                TMMTINDGVRender();
                $("#BackGround").css("display", "none");
            }).catch((err) => {
                $("#BackGround").css("display", "none");
            })
        });
        BaseResult.BarcodeDGV = [];
        BarcodeDGVRender();
    }
    if (TagIndex == 2) {
        $("#LOC_Label11").val("-");
        $("#LOC_Label12").val("-");
        $("#LOC_Label13").val("-");
        $("#LOC_Label14").val("-");
        $("#LOC_Label15").val("-");


        if (LOC_TextBox1) {
            if (LOC_TextBox1.length > 0) {
                T1_CHK = false;
                $("#BackGround").css("display", "block");
                let formUpload = new FormData();
                formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
                let url = "/B02/Buttonfind_Click";

                fetch(url, {
                    method: "POST",
                    body: formUpload,
                    headers: {
                    }
                }).then((response) => {
                    response.json().then((data) => {
                        BaseResult = data;
                        if (BaseResult) {
                            if (BaseResult.ListtmbrcdTranfer01) {
                                if (BaseResult.ListtmbrcdTranfer01.length > 0) {
                                    $("#LOC_Label11").val(BaseResult.ListtmbrcdTranfer01[0].PART_NO);
                                    $("#LOC_Label12").val(BaseResult.ListtmbrcdTranfer01[0].NAME);
                                    $("#LOC_Label13").val(BaseResult.ListtmbrcdTranfer01[0].QTY);
                                    $("#LOC_Label14").val(BaseResult.ListtmbrcdTranfer01[0].BARCD_ID);
                                    $("#LOC_Label15").val(BaseResult.ListtmbrcdTranfer01[0].LOC);
                                    T1_CHK = true;
                                    if (T1_CHK == true) {
                                        document.getElementById("LOC_TextBox1").readOnly = true;
                                        $("#LOC_TextBox2").focus();
                                    }
                                    else {
                                        $("#LOC_TextBox1").val("");
                                        $("#LOC_TextBox1").focus();
                                    }                                   
                                }
                                else {
                                    var audio = new Audio("/Media/Sash_brk.wav");
                                    audio.play();
                                    $("#LOC_TextBox1").val("");
                                    $("#LOC_TextBox1").focus();
                                    alert("바코드 정보 없음. No barcode information. Một lỗi đã xảy ra.");
                                }
                            }
                        }
                        $("#BackGround").css("display", "none");
                    }).catch((err) => {
                        var audio = new Audio("/Media/Sash_brk.wav");
                        audio.play();
                        alert("DATA LOAD 오류가 발생 하였습니다. DATA LOAD Một lỗi đã xảy ra.");
                        $("#BackGround").css("display", "none");
                    })
                });
            }
        }
    }
    if (TagIndex == 3) {
        $("#BackGround").css("display", "block");

        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        let url = "/B02/Buttonfind_Click";

        fetch(url, {
            method: "POST",
            body: formUpload,
            headers: {
            }
        }).then((response) => {
            response.json().then((data) => {
                BaseResult = data;
                DataGridView1Render();
                $("#BackGround").css("display", "none");
            }).catch((err) => {
                $("#BackGround").css("display", "none");
            })
        });
    }
}
function Buttonadd_Click() {
    if (TagIndex == 1) {        
        let DTPsuck = $("#DTPsuck").val();
        let TBBarcode = $("#TBBarcode").val();

        let BaseParameter = new Object();
        BaseParameter = {
            Action: TagIndex,
            ListSearchString: [],
        }
        BaseParameter.ListSearchString.push(DTPsuck);
        BaseParameter.ListSearchString.push(TBBarcode);
        $("#BackGround").css("display", "block");
        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        let url = "/B02/Buttonadd_Click";

        fetch(url, {
            method: "POST",
            body: formUpload,
            headers: {
            }
        }).then((response) => {
            response.json().then((data) => {
                let BaseResultButtonadd = data;
                if (BaseResultButtonadd) {
                    if (BaseResultButtonadd.BarcodeDGV) {
                        if (BaseResultButtonadd.BarcodeDGV.length > 0) {                           
                            if (BaseResult.BarcodeDGV == null) {
                                BaseResult.BarcodeDGV = BaseResultButtonadd.BarcodeDGV;
                            }
                            else {
                                BaseResult.BarcodeDGV.push(BaseResultButtonadd.BarcodeDGV[0]);
                                PlaySuccessSound();
                            }
                            if (BaseResultButtonadd.BarcodeDGV[0].Result == "Small Barcode") {
                                PlayErrorSound();
                                alert("Large Barcode Reading. Please Check Again.");
                            }
                        }
                    }
                    BaseResult.ListtmbrcdTranfer = BaseResultButtonadd.ListtmbrcdTranfer;
                    if (BaseResult.ListtmbrcdTranfer) {
                        let i = 0;
                        let j = BaseResult.ListtmbrcdTranfer.length;
                        let g = 0
                        let k = BaseResult.BarcodeDGV.length;
                        if (j > 0) {
                            for (i = 0; i < j; i++) {
                                let barcode_id = BaseResult.ListtmbrcdTranfer[i].BARCD_ID;
                                for (g = 0; g < k; g++) {
                                    let Barcode = BaseResult.BarcodeDGV[g].Barcode;
                                    if (barcode_id == Barcode) {
                                        BaseResult.BarcodeDGV[g].Result = "Complete";
                                        BaseResult.ListtmbrcdTranfer[i].Result = "Complete";
                                        BaseResult.ListtmbrcdTranfer[i].CHK = true;
                                        g = k;
                                        PlaySuccessSound();
                                    }
                                }
                            }
                        }                      
                    }
                    BarcodeDGVRender();
                    TMMTINDGVRender();
                }
                $("#BackGround").css("display", "none");
            }).catch((err) => {
                $("#BackGround").css("display", "none");
            })
        });
        $("#TBBarcode").val("");
        $("#TBBarcode").focus();
    }
}
function Buttonsave_Click() { 

    if (TagIndex == 1) {
        if (BaseResult) {
            if (BaseResult.ListtmbrcdTranfer) {
                let j = BaseResult.ListtmbrcdTranfer.length;
                if (j < 0) {
                    alert("Please Check Again.");
                }
                else {                    
                    $("#BackGround").css("display", "block");
                    let BaseParameter = new Object();
                    BaseParameter = {
                        Action: TagIndex,                    
                    }
                    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
                    BaseParameter.USER_ID = GetCookieValue("UserID");               
                    BaseParameter.ListtmbrcdTranfer = BaseResult.ListtmbrcdTranfer;                    
                    let formUpload = new FormData();
                    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
                    let url = "/B02/Buttonsave_Click";

                    fetch(url, {
                        method: "POST",
                        body: formUpload,
                        headers: {
                        }
                    }).then((response) => {
                        response.json().then((data) => {                                 
                            BaseResult.BarcodeDGV = [];
                            BarcodeDGVRender();
                            Buttonfind_Click();    
                            PlaySuccessSound();
                            alert(localStorage.getItem("SaveSuccess"));
                            $("#BackGround").css("display", "none");
                        }).catch((err) => {
                            PlayErrorSound();
                            alert(localStorage.getItem("ERRORPleaseCheckAgain"));
                            $("#BackGround").css("display", "none");
                        })
                    });
                }
            }
        }
    }
    if (TagIndex == 2) {
        let IsSave = true;
        let LOC_TextBox1 = $("#LOC_TextBox1").val();       
        if (LOC_TextBox1 == "") {
            IsSave = false;
        }
        let LOC_TextBox2 = $("#LOC_TextBox2").val();
        if (LOC_TextBox2 == "") {
            IsSave = false;
        }
        if (IsSave == true) {
            $("#BackGround").css("display", "block");
            let AAA = $("#LOC_Label14").val();
            let BBB = $("#LOC_TextBox2").val();
            let BaseParameter = new Object();
            BaseParameter = {
                Action: TagIndex,
                ListSearchString: [],
            }    
            BaseParameter.ListSearchString.push(AAA);
            BaseParameter.ListSearchString.push(BBB);
            let formUpload = new FormData();
            formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
            let url = "/B02/Buttonsave_Click";

            fetch(url, {
                method: "POST",
                body: formUpload,
                headers: {
                }
            }).then((response) => {
                response.json().then((data) => {
                    $("#LOC_Label17").val($("#LOC_TextBox2").val());

                    var audio = new Audio("/Media/HOOK_S.wav");
                    audio.play();

                    alert(localStorage.getItem("SaveSuccess"));

                    $("#LOC_TextBox1").val("");
                    $("#LOC_TextBox2").val("");
                    document.getElementById("LOC_TextBox1").readOnly = false;
                    $("#LOC_TextBox1").focus();
                    
                    $("#BackGround").css("display", "none");
                }).catch((err) => {
                    var audio = new Audio("/Media/Sash_brk.wav");
                    audio.play();
                    alert(localStorage.getItem("ERRORPleaseCheckAgain"));
                    $("#BackGround").css("display", "none");
                })
            });
        }
    }

}
function Buttondelete_Click() {
    if (TagIndex == 1) {
        if (BaseResult) {
            if (BaseResult.ListtmbrcdTranfer) {
                if (BaseResult.BarcodeDGV) {
                    let Result = BaseResult.BarcodeDGV[BarcodeDGVRowIndex].Result;
                    let Barcode = BaseResult.BarcodeDGV[BarcodeDGVRowIndex].Result;
                    if (Result == "Complete") {
                        for (let j = 0; j < BaseResult.ListtmbrcdTranfer.length; j++) {
                            let BARCD_ID = BaseResult.ListtmbrcdTranfer[j].BARCD_ID;
                            if (BARCD_ID == Barcode) {
                                BaseResult.ListtmbrcdTranfer[j].CHK = false;
                                BaseResult.ListtmbrcdTranfer[j].Result = "";
                            }
                        }
                        TMMTINDGVRender();
                    }
                    BaseResult.BarcodeDGV.splice(BarcodeDGVRowIndex, 1);
                    BarcodeDGVRender();
                }
            }
        }
    }
}
function Buttoncancel_Click() {
    if (TagIndex == 2) {
        $("#LOC_Label11").val("-");
        $("#LOC_Label12").val("-");
        $("#LOC_Label13").val("-");
        $("#LOC_Label14").val("-");
        $("#LOC_Label15").val("-");
        $("#LOC_TextBox1").val("");
        $("#LOC_TextBox2").val("");
        $("#LOC_TextBox1").focus();
    }
}
function Buttoninport_Click() {
    //$("#BackGround").css("display", "block");
    //let BaseParameter = new Object();
    //BaseParameter = {
    //}
    //let formUpload = new FormData();
    //formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    //let url = "/B02/Buttoninport_Click";

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
    //formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    //let url = "/B02/Buttonexport_Click";

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
    //$("#BackGround").css("display", "block");
    //let BaseParameter = new Object();
    //BaseParameter = {
    //}
    //let formUpload = new FormData();
    //formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    //let url = "/B02/Buttonprint_Click";

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
function Buttonhelp_Click() {
    let url = "/WMP_PLAY";
    OpenWindowByURL(url, 800, 460);
}
function Buttonclose_Click() {
    history.back();
}

function TMMTINDGVRender() {
    let HTML = "";
    if (BaseResult) {
        if (BaseResult.ListtmbrcdTranfer) {
            if (BaseResult.ListtmbrcdTranfer.length > 0) {
                TMMTINDGV_SelectionChanged(0);
                for (let i = 0; i < BaseResult.ListtmbrcdTranfer.length; i++) {
                    if (BaseResult.ListtmbrcdTranfer[i].CHK == true) {
                        HTML = HTML + "<tr style='background-color: lightgreen;'>";
                    }
                    else {
                        HTML = HTML + "<tr>";
                    }
                    if (BaseResult.ListtmbrcdTranfer[i].CHK) {
                        HTML = HTML + "<td><label><input id='TMMTINDGVCHK" + i + "' class='form-check-input' type='checkbox' checked disabled><span></span></label></td>";
                    }
                    else {
                        HTML = HTML + "<td><label><input id='TMMTINDGVCHK" + i + "' class='form-check-input' type='checkbox' disabled><span></span></label></td>";
                    }
                    HTML = HTML + "<td>" + BaseResult.ListtmbrcdTranfer[i].Result + "</td>";
                    HTML = HTML + "<td>" + BaseResult.ListtmbrcdTranfer[i].PART_NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.ListtmbrcdTranfer[i].PART_NM + "</td>";
                    HTML = HTML + "<td>" + BaseResult.ListtmbrcdTranfer[i].PART_SNP + "</td>";
                    HTML = HTML + "<td>" + BaseResult.ListtmbrcdTranfer[i].UTM + "</td>";
                    HTML = HTML + "<td>" + BaseResult.ListtmbrcdTranfer[i].STOCK + "</td>";
                    HTML = HTML + "<td>" + BaseResult.ListtmbrcdTranfer[i].QTY + "</td>";
                    HTML = HTML + "<td>" + BaseResult.ListtmbrcdTranfer[i].PKG_QTY + "</td>";
                    HTML = HTML + "<td>" + BaseResult.ListtmbrcdTranfer[i].NET_WT + "</td>";
                    HTML = HTML + "<td>" + BaseResult.ListtmbrcdTranfer[i].GRS_WT + "</td>";
                    HTML = HTML + "<td>" + BaseResult.ListtmbrcdTranfer[i].PLET_NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.ListtmbrcdTranfer[i].SHPD_NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.ListtmbrcdTranfer[i].MTIN_DTM + "</td>";
                    HTML = HTML + "<td>" + BaseResult.ListtmbrcdTranfer[i].DSCN_YN + "</td>";
                    HTML = HTML + "<td>" + BaseResult.ListtmbrcdTranfer[i].MTIN_RMK + "</td>";
                    HTML = HTML + "<td>" + BaseResult.ListtmbrcdTranfer[i].CREATE_DTM + "</td>";
                    HTML = HTML + "<td>" + BaseResult.ListtmbrcdTranfer[i].CREATE_USER + "</td>";
                    HTML = HTML + "<td>" + BaseResult.ListtmbrcdTranfer[i].UPDATE_DTM + "</td>";
                    HTML = HTML + "<td>" + BaseResult.ListtmbrcdTranfer[i].UPDATE_USER + "</td>";
                    HTML = HTML + "<td>" + BaseResult.ListtmbrcdTranfer[i].BARCD_ID + "</td>";
                    //HTML = HTML + "<td>" + BaseResult.ListtmbrcdTranfer[i].PART_IDX + "</td>";
                    //HTML = HTML + "<td>" + BaseResult.ListtmbrcdTranfer[i].BRCD_PRNT + "</td>";
                    //HTML = HTML + "<td>" + BaseResult.ListtmbrcdTranfer[i].MTIN_IDX + "</td>";
                    //HTML = HTML + "<td>" + BaseResult.ListtmbrcdTranfer[i].BARCD_IDX + "</td>";
                    HTML = HTML + "</tr>";
                }
            }
        }
    }
    document.getElementById("TMMTINDGV").innerHTML = HTML;
}
function TMMTINDGVSort() {
    IsTableSort = !IsTableSort;
    DataGridViewSort(BaseResult.ListtmbrcdTranfer, IsTableSort);
    TMMTINDGVRender();
}
function TMMTINDGV_SelectionChanged(i) {
    TMMTINDGVRowIndex = i;
}
function DataGridView1Render() {
    let HTML = "";
    if (BaseResult) {
        if (BaseResult.ListtmbrcdTranfer03) {
            if (BaseResult.ListtmbrcdTranfer03.length > 0) {
                DataGridView1_SelectionChanged(0);
                for (let i = 0; i < BaseResult.ListtmbrcdTranfer03.length; i++) {
                    HTML = HTML + "<tr>";
                    HTML = HTML + "<td>" + BaseResult.ListtmbrcdTranfer03[i].BARCD_ID + "</td>";
                    HTML = HTML + "<td>" + BaseResult.ListtmbrcdTranfer03[i].DATE + "</td>";
                    HTML = HTML + "<td>" + BaseResult.ListtmbrcdTranfer03[i].PART_NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.ListtmbrcdTranfer03[i].NAME + "</td>";
                    HTML = HTML + "<td>" + BaseResult.ListtmbrcdTranfer03[i].LOC + "</td>";
                    HTML = HTML + "<td>" + BaseResult.ListtmbrcdTranfer03[i].QTY + "</td>";
                    HTML = HTML + "</tr>";
                }
            }
        }
    }
    document.getElementById("DataGridView1").innerHTML = HTML;
}
function DataGridView1Sort() {
    IsTableSort = !IsTableSort;
    DataGridViewSort(BaseResult.ListtmbrcdTranfer03, IsTableSort);
    DataGridView1Render();
}
function DataGridView1_SelectionChanged(i) {
    DataGridView1RowIndex = i;
}
function BarcodeDGVRender() {
    let HTML = "";
    if (BaseResult) {
        if (BaseResult.BarcodeDGV) {
            if (BaseResult.BarcodeDGV.length > 0) {
                BarcodeDGV_SelectionChanged(0);
                for (let i = 0; i < BaseResult.BarcodeDGV.length; i++) {
                    HTML = HTML + "<tr onclick='BarcodeDGV_SelectionChanged(" + i + ")'>";
                    HTML = HTML + "<td>" + BaseResult.BarcodeDGV[i].Barcode + "</td>";
                    HTML = HTML + "<td>" + BaseResult.BarcodeDGV[i].Result + "</td>";
                    HTML = HTML + "</tr>";
                }
            }
        }
    }
    document.getElementById("BarcodeDGV").innerHTML = HTML;
}
function BarcodeDGVSort() {
    IsTableSort = !IsTableSort;
    DataGridViewSort(BaseResult.BarcodeDGV, IsTableSort);
    BarcodeDGVRender();
}
function BarcodeDGV_SelectionChanged(i) {
    BarcodeDGVRowIndex = i;
}

function SUCHK_Change() { }