let IsTableSort = false;
let BaseResult;
let RowIndex;

$(window).focus(function () {
}).blur(function () {
    let B03HTML = localStorage.getItem("B03HTML");
    if (B03HTML == 0) {
        window.close();
    }
});

$(document).ready(function () {
    //RowIndex = Number(localStorage.getItem("B03_1RowIndex"));
    //$("#TextBox1").val(localStorage.getItem("B03_1TextBox1"));
    //$("#TextBox2").val(localStorage.getItem("B03_1TextBox2"));
    //$("#TextBox3").val(localStorage.getItem("B03_1TextBox3"));
    //Button1_Click();
    //RETMBRCDDataGridView_SelectionChanged(RowIndex);

});
$("#TextBox1").keydown(function (e) {
    if (e.keyCode == 13) {
        Button1_Click();
    }
});
$("#TextBox2").keydown(function (e) {
    if (e.keyCode == 13) {
        Button1_Click();
    }
});
$("#TextBox3").keydown(function (e) {
    if (e.keyCode == 13) {
        Button1_Click();
    }
});
$("#Button1").click(function () {
    Button1_Click();
});
$("#Buttonprint").click(function () {
    Buttonprint_Click();
});
$("#Buttonclose").click(function () {
    Buttonclose_Click();
});
function Button1_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        ListSearchString: [],
    }
    BaseParameter.ListSearchString.push($('#TextBox1').val());
    BaseParameter.ListSearchString.push($('#TextBox2').val());
    BaseParameter.ListSearchString.push($('#TextBox3').val());
    let formUpload = new FormData();
    formUpload.append("BaseParameter", JSON.stringify(BaseParameter));
    let url = "/B03_1/Button1_Click";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            BaseResult = data;
            RenderDataGridView();
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
function Buttonprint_Click() {
    $("#BackGround").css("display", "block");

    localStorage.setItem("B03_1RowIndex", RowIndex);
    localStorage.setItem("B03_1TextBox1", $('#TextBox1').val());
    localStorage.setItem("B03_1TextBox2", $('#TextBox2').val());
    localStorage.setItem("B03_1TextBox3", $('#TextBox3').val());

    let BaseParameter = new Object();
    BaseParameter = {
        ListSearchString: [],
    }
    BaseParameter.ListSearchString.push($('#Label2').val());
    BaseParameter.ListSearchString.push($('#Label3').val());
    BaseParameter.ListSearchString.push($('#Label5').val());
    BaseParameter.ListSearchString.push($('#Label7').val());
    BaseParameter.ListSearchString.push($('#Label9').val());
    BaseParameter.ListSearchString.push($('#Label11').val());
    BaseParameter.ListSearchString.push($('#Label15').val());
    BaseParameter.ListSearchString.push($('#Label13').val());
    BaseParameter.ListSearchString.push($('#Label20').val());
    BaseParameter.ListSearchString.push($('#Label22').val());
    let formUpload = new FormData();
    formUpload.append("BaseParameter", JSON.stringify(BaseParameter));
    let url = "/B03_1/Buttonprint_Click";

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
                    localStorage.setItem("B03HTML", "1");
                    let url = BaseResultButtonprint.Code;                    
                    OpenWindowByURL(url, 200, 200);
                }
            }
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
function Buttonclose_Click() {
    window.close();
}
function RenderDataGridView() {
    let HTML = "";
    if (BaseResult) {
        if (BaseResult.ListtmbrcdTranfer) {
            if (BaseResult.ListtmbrcdTranfer.length > 0) {
                RETMBRCDDataGridView_SelectionChanged(0);
                for (let i = 0; i < BaseResult.ListtmbrcdTranfer.length; i++) {
                    HTML = HTML + "<tr onclick='RETMBRCDDataGridView_SelectionChanged(" + i + ")'>";
                    HTML = HTML + "<td>" + BaseResult.ListtmbrcdTranfer[i].BARCD_ID + "</td>";
                    HTML = HTML + "<td>" + new Date(BaseResult.ListtmbrcdTranfer[i].MTIN_DTM.replace(/(\d{2})-(\d{2})-(\d{4})/, "$2/$1/$3")).toISOString().slice(0, 10) + "</td>";
                    HTML = HTML + "<td>" + BaseResult.ListtmbrcdTranfer[i].PART_NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.ListtmbrcdTranfer[i].PART_NM + "</td>";
                    HTML = HTML + "<td>" + BaseResult.ListtmbrcdTranfer[i].PKG_GRP + "</td>";
                    HTML = HTML + "<td>" + BaseResult.ListtmbrcdTranfer[i].PKG_QTY + "</td>";
                    HTML = HTML + "<td>" + BaseResult.ListtmbrcdTranfer[i].UTM + "</td>";
                    HTML = HTML + "<td>" + BaseResult.ListtmbrcdTranfer[i].QTY + "</td>";
                    HTML = HTML + "<td>" + BaseResult.ListtmbrcdTranfer[i].NET_WT + "</td>";
                    HTML = HTML + "<td>" + BaseResult.ListtmbrcdTranfer[i].GRS_WT + "</td>";
                    HTML = HTML + "<td>" + BaseResult.ListtmbrcdTranfer[i].PLET_NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.ListtmbrcdTranfer[i].SHPD_NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.ListtmbrcdTranfer[i].DSCN_YN + "</td>";
                    HTML = HTML + "<td>" + BaseResult.ListtmbrcdTranfer[i].PART_LOC + "</td>";
                    HTML = HTML + "<td>" + BaseResult.ListtmbrcdTranfer[i].MTIN_RMK + "</td>";
                    HTML = HTML + "</tr>";
                }
            }
        }
    }
    document.getElementById("RETMBRCDDataGridView").innerHTML = HTML;
}
function DataGridViewSort() {
    IsTableSort = !IsTableSort;
    DataGridViewSort(BaseResult.ListtmbrcdTranfer, IsTableSort);
    RenderDataGridView();
}
function RETMBRCDDataGridView_SelectionChanged(index) {
    RowIndex = index;
    if (BaseResult) {
        if (BaseResult.ListtmbrcdTranfer) {
            if (BaseResult.ListtmbrcdTranfer.length > 0) {
                if (RowIndex >= 0) {
                    $("#Label2").val(BaseResult.ListtmbrcdTranfer[RowIndex].BARCD_ID);
                    $("#Label3").val(BaseResult.ListtmbrcdTranfer[RowIndex].PKG_GRP);
                    $("#Label5").val(BaseResult.ListtmbrcdTranfer[RowIndex].PKG_QTY);
                    $("#Label7").val(BaseResult.ListtmbrcdTranfer[RowIndex].QTY);
                    $("#Label9").val(BaseResult.ListtmbrcdTranfer[RowIndex].PART_NO);
                    $("#Label11").val(BaseResult.ListtmbrcdTranfer[RowIndex].PART_NM);
                    $("#Label15").val(BaseResult.ListtmbrcdTranfer[RowIndex].SHPD_NO);
                    $("#Label13").val(BaseResult.ListtmbrcdTranfer[RowIndex].UTM);
                    $("#Label20").val(BaseResult.ListtmbrcdTranfer[RowIndex].PART_LOC);
                    $("#Label22").val(BaseResult.ListtmbrcdTranfer[RowIndex].MTIN_DTM);
                }
            }
        }
    }
}