let BaseResult;
$(document).ready(function () {
    var now = new Date();
    var day = ("0" + now.getDate()).slice(-2);
    var month = ("0" + (now.getMonth() + 1)).slice(-2);
    var today = now.getFullYear() + "-" + (month) + "-" + (day);
    $("#DateTimePicker1").val(today);
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

$("#TextBox1").keydown(function (e) {
    if (e.keyCode == 13) {
        TextBox1_KeyDown();
    }
});
function TextBox1_KeyDown() {
    Buttonfind_Click();
}
function Buttonfind_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
        ListSearchString: [],
    }
    let A01 = $('#TextBox1').val();
    let A02 = $('#TextBox2').val();
    let A03 = $('#TextBox3').val();
    let A04 = $('#DateTimePicker1').val();
    let A05 = $('#TextBox5').val();
    let A06 = $('#TextBox6').val();
    let A07 = $('#ComboBox1').val();
    let A08 = $('#ComboBox2').val();
    let A09 = $('#TextBox9').val();
    let A10 = $('#TextBox10').val();
    let A11 = $('#TextBox4').val();
    BaseParameter.ListSearchString.push(A01);
    BaseParameter.ListSearchString.push(A02);
    BaseParameter.ListSearchString.push(A03);
    BaseParameter.ListSearchString.push(A04);
    BaseParameter.ListSearchString.push(A05);
    BaseParameter.ListSearchString.push(A06);
    BaseParameter.ListSearchString.push(A07);
    BaseParameter.ListSearchString.push(A08);
    BaseParameter.ListSearchString.push(A09);
    BaseParameter.ListSearchString.push(A10);
    BaseParameter.ListSearchString.push(A11);
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/B11/Buttonfind_Click";

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
function Buttonadd_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/B11/Buttonadd_Click";

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
function Buttonsave_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/B11/Buttonsave_Click";

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
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/B11/Buttondelete_Click";

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
function Buttoncancel_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/B11/Buttoncancel_Click";

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
function Buttoninport_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/B11/Buttoninport_Click";

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
function Buttonexport_Click() {
    //$("#BackGround").css("display", "block");
    //let BaseParameter = new Object();
    //BaseParameter = {
    //    ListSearchString: [],
    //}
    //let A01 = $('#TextBox1').val();
    //let A02 = $('#TextBox2').val();
    //let A03 = $('#TextBox3').val();
    //let A04 = $('#DateTimePicker1').val();
    //let A05 = $('#TextBox5').val();
    //let A06 = $('#TextBox6').val();
    //let A07 = $('#ComboBox1').val();
    //let A08 = $('#ComboBox2').val();
    //let A09 = $('#TextBox9').val();
    //let A10 = $('#TextBox10').val();
    //let A11 = $('#TextBox4').val();
    //BaseParameter.ListSearchString.push(A01);
    //BaseParameter.ListSearchString.push(A02);
    //BaseParameter.ListSearchString.push(A03);
    //BaseParameter.ListSearchString.push(A04);
    //BaseParameter.ListSearchString.push(A05);
    //BaseParameter.ListSearchString.push(A06);
    //BaseParameter.ListSearchString.push(A07);
    //BaseParameter.ListSearchString.push(A08);
    //BaseParameter.ListSearchString.push(A09);
    //BaseParameter.ListSearchString.push(A10);
    //BaseParameter.ListSearchString.push(A11);
    //let formUpload = new FormData();
    //formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    //let url = "/B11/Buttonexport_Click";

    //fetch(url, {
    //    method: "POST",
    //    body: formUpload,
    //    headers: {
    //    }
    //}).then((response) => {
    //    response.json().then((data) => {
    //        let BaseResultButtonexport = data;
    //        if (BaseResultButtonexport) {
    //            if (BaseResultButtonexport.Code) {
    //                let url = BaseResultButtonexport.Code;
    //                window.open(url, "_blank");
    //            }
    //        }
    //        $("#BackGround").css("display", "none");
    //    }).catch((err) => {
    //        $("#BackGround").css("display", "none");
    //    })
    //});

    TableHTMLToExcel("DataGridView1Table", "B11", "B11");
}
function Buttonprint_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/B11/Buttonprint_Click";

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

function DataGridView1Render() {
    let HTML = "";    
    if (BaseResult) {
        if (BaseResult.DataGridView1) {
            if (BaseResult.DataGridView1.length > 0) {                
                HTML = HTML + "<tbody>";
                for (let i = 0; i < BaseResult.DataGridView1.length; i++) {                    
                    HTML = HTML + "<tr>";                   
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].PART_NO + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].DESC + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].SNP + "</td>";
                    HTML = HTML + "<td>" + new Date(BaseResult.DataGridView1[i].MTIN_DTM.replace(/(\d{2})-(\d{2})-(\d{4})/, "$2/$1/$3")).toISOString().slice(0, 10) + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].PKG_GRP_IDX + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].BARCD_ID + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].PKG_GRP + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].DSCN_YN + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].PKG_QTY + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].PART_LOC + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].PKG_OUTQTY + "</td>";
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].QTY + "</td>";                  
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].PLET_NO + "</td>";                  
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].SHPD_NO + "</td>";                  
                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].STOCK + "</td>";                  
                    HTML = HTML + "</tr>";
                }
                HTML = HTML + "</tbody>";
            }
        }
    }
    document.getElementById("DataGridView1").innerHTML = HTML;
}
function DataGridView1Sort() {
  
}
let IsTableSort = false;
let DataGridView1Table = document.getElementById("DataGridView1Table");
DataGridView1Table.addEventListener('click', function (event) {
    if (event.target.tagName === 'TH') {
        let text = event.target.innerText;
        let key = "PART_NO";
        IsTableSort = !IsTableSort;
        if (text == "TOP BARCODE") {
            if (IsTableSort == true) {
                BaseResult.DataGridView1.sort((a, b) => (a.PKG_GRP_IDX > b.PKG_GRP_IDX ? 1 : -1));
            }
            else {
                BaseResult.DataGridView1.sort((a, b) => (a.PKG_GRP_IDX < b.PKG_GRP_IDX ? 1 : -1));
            }
        }
        else {
            if (text == "PACKING QTY") {
                if (IsTableSort == true) {
                    BaseResult.DataGridView1.sort((a, b) => (a.PKG_QTY > b.PKG_QTY ? 1 : -1));
                }
                else {
                    BaseResult.DataGridView1.sort((a, b) => (a.PKG_QTY < b.PKG_QTY ? 1 : -1));
                }
            }
            else {
                if (text == "OUT_QTY") {
                    if (IsTableSort == true) {
                        BaseResult.DataGridView1.sort((a, b) => (a.PKG_OUTQTY > b.PKG_OUTQTY ? 1 : -1));
                    }
                    else {
                        BaseResult.DataGridView1.sort((a, b) => (a.PKG_OUTQTY < b.PKG_OUTQTY ? 1 : -1));
                    }
                }
                else {
                    if (text == "SHIPPING NO") {
                        if (IsTableSort == true) {
                            BaseResult.DataGridView1.sort((a, b) => (a.SHPD_NO > b.SHPD_NO ? 1 : -1));
                        }
                        else {
                            BaseResult.DataGridView1.sort((a, b) => (a.SHPD_NO < b.SHPD_NO ? 1 : -1));
                        }
                    }
                    else {
                        if (text == "SNP") {
                            if (IsTableSort == true) {
                                BaseResult.DataGridView1.sort((a, b) => (a.SNP > b.SNP ? 1 : -1));
                            }
                            else {
                                BaseResult.DataGridView1.sort((a, b) => (a.SNP < b.SNP ? 1 : -1));
                            }
                        }
                        else {
                            ListSort(BaseResult.DataGridView1, key, text, IsTableSort);
                        }
                    }
                }
            }
        }
        DataGridView1Render();
    }
});


