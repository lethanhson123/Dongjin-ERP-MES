let IsTableSort = false;
let BaseResult = new Object();
let TagIndex = 1;
let FNM;
let DataGridView1RowIndex = 0;
let DataGridView2RowIndex = 0;
let T2_DGV1RowIndex = 0;
let CompanyID = 16;
$(document).ready(function () {
    var IPAddress = location.host;
    if (IPAddress.includes("192.168.1.240")) {
        CompanyID = 17;
    }
    var now = new Date();
    var day = ("0" + now.getDate()).slice(-2);
    var month = ("0" + (now.getMonth() + 1)).slice(-2);
    var today = now.getFullYear() + "-" + (month) + "-" + (day);
    document.getElementById("DateTimePicker1").readOnly = true;
    $("#DateTimePicker1").val(today);
    $("#T2_S2").val(today);
    $("#Tag003Date").val(today);
    FNM = localStorage.getItem("FNM_B09");
    if (FNM == null) {
        FNM = 0;
    }
    if (FNM == 0) {
        document.getElementById("RadioButton1").checked = true;
    }
    else {
        document.getElementById("RadioButton2").checked = true;
    }
    localStorage.setItem("FNM_B09", FNM);
    //COMLIST_LINE();    

    BaseResult.ListWarehouseRequestDetail = [];
    BaseResult.ListWarehouseRequestDetail2 = [];
    BaseResult.ListWarehouseOutput = [];
    BaseResult.ListWarehouseOutputDetail = [];
    BaseResult.ListWarehouseRequest = [];
    BaseResult.ListWarehouseRequestDetail1 = [];
    GetListCategoryDepartment();

    var optionEmpty = document.createElement("option");
    optionEmpty.text = "";
    optionEmpty.value = "";
    var cbbDisplay = document.getElementById("cbbDisplay");
    cbbDisplay.add(optionEmpty);

    if (CompanyID == 17) {
        var option = document.createElement("option");
        option.text = "ZDA";
        option.value = "ZDA";

        var cbbDisplay = document.getElementById("cbbDisplay");
        cbbDisplay.add(option);

        var option01 = document.createElement("option");
        option01.text = "MV/ME";
        option01.value = "MV/ME";

        var cbbDisplay = document.getElementById("cbbDisplay");
        cbbDisplay.add(option01);
    }

    if (CompanyID == 16) {

        var option02 = document.createElement("option");
        option02.text = "PKI";
        option02.value = "PKI";

        var cbbDisplay = document.getElementById("cbbDisplay");
        cbbDisplay.add(option02);

        var option = document.createElement("option");
        option.text = "DH";
        option.value = "DH";

        var cbbDisplay = document.getElementById("cbbDisplay");
        cbbDisplay.add(option);

        var option01 = document.createElement("option");
        option01.text = "U100";
        option01.value = "U100";

        var option02 = document.createElement("option");
        option01.text = "U100";
        option01.value = "U100";

        var cbbDisplay = document.getElementById("cbbDisplay");
        cbbDisplay.add(option01);
    }

    BaseResult.DataGridView2 = new Object();
});
$("#ATag001").click(function (e) {
    TagIndex = 1;
});
$("#ATag002").click(function (e) {
    TagIndex = 2;
    Buttonfind_Click();
});
$("#ATag003").click(function (e) {
    TagIndex = 3;
    Buttonfind_Click();
});
$("#ATag004").click(function (e) {
    TagIndex = 4;
    $('txtBarcodeOutput').focus();
});
$('#txtBarcodeOutput').on('keydown', function (event) {
    if (event.which === 13) {
        let Barcode = $("#txtBarcodeOutput").val();
        $("#txtBarcodeOutput").val("");
        $("#txtBarcodeOutput").focus();
        let CategoryDepartmentID = 204;
        let IDERP = 217;
        if (CompanyID == 17) {
            CategoryDepartmentID = 208;
            IDERP = 224
        }
        $("#BackGround").css("display", "block");
        let MembershipID = GetCookieValue("USER_IDX");
        let BaseParameter = new Object();
        BaseParameter = {
            Action: TagIndex,
            CompanyID: CompanyID,
            CategoryDepartmentID: CategoryDepartmentID,
            IDERP: IDERP,
            MembershipID: MembershipID,
            SearchString: Barcode,
        }
        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        let url = "/B09/SaveWarehouseOutputDetailBarcode";

        fetch(url, {
            method: "POST",
            body: formUpload,
            headers: {
            }
        }).then((response) => {
            response.json().then((data) => {
                BaseResult.ListWarehouseOutputDetailBarcode = data.ListWarehouseOutputDetailBarcode;
                DataGridViewBarcodeOutputRender();
                $("#BackGround").css("display", "none");
            }).catch((err) => {
                $("#BackGround").css("display", "none");
            })
        });
    }
});
$('#txtBarcodeInput').on('keydown', function (event) {
    if (event.which === 13) {
    }
});
function DataGridViewBarcodeOutputRender() {
    let HTML = "";
    if (BaseResult) {
        if (BaseResult.ListWarehouseOutputDetailBarcode) {
            if (BaseResult.ListWarehouseOutputDetailBarcode.length > 0) {
                for (let i = 0; i < BaseResult.ListWarehouseOutputDetailBarcode.length; i++) {
                    HTML = HTML + "<tr>";
                    HTML = HTML + "<td>" + BaseResult.ListWarehouseOutputDetailBarcode[i].ID + "</td>";
                    HTML = HTML + "<td>" + BaseResult.ListWarehouseOutputDetailBarcode[i].Barcode + "</td>";
                    HTML = HTML + "<td style='text-align: right;'>" + BaseResult.ListWarehouseOutputDetailBarcode[i].Quantity + "</td>";
                    HTML = HTML + "</tr>";
                }
            }
        }
    }
    document.getElementById("DataGridViewBarcodeOutput").innerHTML = HTML;
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
        //let IsFind = true;
        //let cb_Stage1 = $("#cb_Stage1").val();
        //if (cb_Stage1 == "Select Stage...") {
        //    IsFind = false;
        //}
        //if (IsFind == true) {
        //    $("#BackGround").css("display", "block");
        //    let BaseParameter = new Object();
        //    BaseParameter = {
        //        Action: TagIndex,
        //        ListSearchString: [],
        //    }
        //    BaseParameter.CheckBox1 = document.getElementById("CheckBox1").checked;
        //    BaseParameter.ListSearchString.push($("#TextBox1").val());
        //    BaseParameter.ListSearchString.push($("#TextBox2").val());
        //    let formUpload = new FormData();
        //    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        //    let url = "/B09/Buttonfind_Click";

        //    fetch(url, {
        //        method: "POST",
        //        body: formUpload,
        //        headers: {
        //        }
        //    }).then((response) => {
        //        response.json().then((data) => {
        //            let BaseResultButtonfind = data;
        //            BaseResult.DataGridView1 = BaseResultButtonfind.DataGridView1;
        //            DataGridView1Render();
        //            $("#BackGround").css("display", "none");
        //        }).catch((err) => {
        //            $("#BackGround").css("display", "none");
        //        })
        //    });
        //}

        $("#BackGround").css("display", "block");
        let BaseParameter = new Object();
        BaseParameter = {
            Action: TagIndex,
            CompanyID: CompanyID,
            SearchString: $("#TextBox1").val(),
        }
        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        let url = "/B09/GetListMaterial";

        fetch(url, {
            method: "POST",
            body: formUpload,
            headers: {
            }
        }).then((response) => {
            response.json().then((data) => {
                let BaseResultSub = data;
                BaseResult.ListMaterial = BaseResultSub.ListMaterial;
                DataGridView1Render();
                $("#BackGround").css("display", "none");
            }).catch((err) => {
                $("#BackGround").css("display", "none");
            })
        });

    }
    if (TagIndex == 2) {
        //$("#BackGround").css("display", "block");
        //let BaseParameter = new Object();
        //BaseParameter = {
        //    Action: TagIndex,
        //    ListSearchString: [],
        //}
        //BaseParameter.ListSearchString.push($("#ComboBox2").val());
        //BaseParameter.ListSearchString.push($("#ComboBox3").val());
        //BaseParameter.ListSearchString.push($("#T2_S1").val());
        //BaseParameter.ListSearchString.push($("#T2_S2").val());
        //BaseParameter.ListSearchString.push($("#T2_S3").val());
        //BaseParameter.ListSearchString.push($("#T2_S4").val());
        //let formUpload = new FormData();
        //formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        //let url = "/B09/Buttonfind_Click";

        //fetch(url, {
        //    method: "POST",
        //    body: formUpload,
        //    headers: {
        //    }
        //}).then((response) => {
        //    response.json().then((data) => {
        //        let BaseResultButtonfind = data;
        //        BaseResult.T2_DGV1 = BaseResultButtonfind.T2_DGV1;
        //        T2_DGV1Render();
        //        $("#BackGround").css("display", "none");
        //    }).catch((err) => {
        //        $("#BackGround").css("display", "none");
        //    })
        //});

        //$("#BackGround").css("display", "block");
        //let BaseParameter = new Object();
        //BaseParameter = {
        //    Action: TagIndex,
        //    CategoryDepartmentID: Number($("#T2_S1").val()),
        //    Date: $("#T2_S2").val(),
        //    SearchString: $("#T2_S3").val(),
        //    Code: $("#ComboBox2").val(),
        //}
        //let formUpload = new FormData();
        //formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        //let url = "/B09/GetListWarehouseRequestDetail";

        //fetch(url, {
        //    method: "POST",
        //    body: formUpload,
        //    headers: {
        //    }
        //}).then((response) => {
        //    response.json().then((data) => {
        //        let BaseResultSub = data;
        //        BaseResult.ListWarehouseRequestDetail2 = BaseResultSub.ListWarehouseRequestDetail;
        //        T2_DGV1Render();
        //        $("#BackGround").css("display", "none");
        //    }).catch((err) => {
        //        $("#BackGround").css("display", "none");
        //    })
        //});

        $("#BackGround").css("display", "block");
        let MembershipID = GetCookieValue("USER_IDX");
        let BaseParameter = new Object();
        BaseParameter = {
            Action: TagIndex,
            CompanyID: CompanyID,
            CategoryDepartmentID: Number($("#T2_S1").val()),
            Date: $("#T2_S2").val(),
            MembershipID: MembershipID,
            SearchString: $("#T2_S3").val(),
        }
        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        let url = "/B09/GetWarehouseOutputByCategoryDepartmentIDAndDateToList";

        fetch(url, {
            method: "POST",
            body: formUpload,
            headers: {
            }
        }).then((response) => {
            response.json().then((data) => {
                let BaseResultSub = data;
                BaseResult.ListWarehouseOutput = BaseResultSub.ListWarehouseOutput;
                WarehouseOutputRender();
                $("#BackGround").css("display", "none");
            }).catch((err) => {
                $("#BackGround").css("display", "none");
            })
        });
        GetWarehouseOutputDetailByParentIDToList();
    }
    if (TagIndex == 3) {
        $("#BackGround").css("display", "block");
        let MembershipID = GetCookieValue("USER_IDX");
        let BaseParameter = new Object();
        BaseParameter = {
            Action: TagIndex,
            CompanyID: CompanyID,
            CategoryDepartmentID: Number($("#Tag003CategoryDepartment").val()),
            Date: $("#Tag003Date").val(),
            MembershipID: MembershipID,
            SearchString: $("#Tag003SearchString").val(),
        }
        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        let url = "/B09/GetWarehouseRequestByCategoryDepartmentIDAndDateToList";

        fetch(url, {
            method: "POST",
            body: formUpload,
            headers: {
            }
        }).then((response) => {
            response.json().then((data) => {
                let BaseResultSub = data;
                BaseResult.ListWarehouseRequest = BaseResultSub.ListWarehouseRequest;
                WarehouseRequestRender();
                $("#BackGround").css("display", "none");
            }).catch((err) => {
                $("#BackGround").css("display", "none");
            })
        });
        GetWarehouseRequestDetailByParentIDToList();
    }
}
function Buttonadd_Click() {

}
function Buttonsave_Click() {
    if (TagIndex == 1) {
        //let IsSave = false;
        //if (BaseResult) {
        //    if (BaseResult.DataGridView2) {
        //        if (BaseResult.DataGridView2.length > 0) {
        //            IsSave = true;
        //        }
        //    }
        //}
        //if (IsSave == true) {
        //    $("#BackGround").css("display", "block");
        //    let BaseParameter = new Object();
        //    BaseParameter = {
        //        Action: TagIndex,
        //    }
        //    BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
        //    BaseParameter.DataGridView2 = BaseResult.DataGridView2;
        //    let formUpload = new FormData();
        //    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        //    let url = "/B09/Buttonsave_Click";

        //    fetch(url, {
        //        method: "POST",
        //        body: formUpload,
        //        headers: {
        //        }
        //    }).then((response) => {
        //        response.json().then((data) => {
        //            alert(localStorage.getItem("SaveSuccess"));
        //            BaseResult.DataGridView2 = [];
        //            DataGridView2Render();
        //            $("#BackGround").css("display", "none");
        //        }).catch((err) => {
        //            $("#BackGround").css("display", "none");
        //        })
        //    });
        //}

        let IsSave = false;
        if (BaseResult) {
            if (BaseResult.ListWarehouseRequestDetail) {
                if (BaseResult.ListWarehouseRequestDetail.length > 0) {
                    IsSave = true;
                }
            }
        }
        if (IsSave == true) {
            $("#BackGround").css("display", "block");
            let BaseParameter = new Object();
            BaseParameter = {
                Action: TagIndex,
            }
            BaseParameter.MembershipID = GetCookieValue("USER_IDX");
            BaseParameter.CategoryDepartmentID = Number($("#cb_Stage1").val());
            BaseParameter.CompanyID = CompanyID;
            BaseParameter.GroupCode = $("#cbbDisplay").val();
            BaseParameter.USER_IDX = GetCookieValue("USER_IDX");
            BaseParameter.ListWarehouseRequestDetail = BaseResult.ListWarehouseRequestDetail;
            let formUpload = new FormData();
            formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
            let url = "/B09/SaveListWarehouseRequestDetail";

            fetch(url, {
                method: "POST",
                body: formUpload,
                headers: {
                }
            }).then((response) => {
                response.json().then((data) => {
                    alert(localStorage.getItem("SaveSuccess"));
                    BaseResult.ListWarehouseRequestDetail = [];
                    DataGridView2Render();
                    $("#BackGround").css("display", "none");
                }).catch((err) => {
                    $("#BackGround").css("display", "none");
                })
            });
        }
    }
}
function Buttondelete_Click() {
    if (TagIndex == 1) {
        //if (confirm(localStorage.getItem("DeleteConfirm"))) {
        //    BaseResult.DataGridView2.splice(DataGridView2RowIndex, 1);
        //    DataGridView2Render();
        //}
        if (confirm(localStorage.getItem("DeleteConfirm"))) {
            console.log(BaseResult.ListWarehouseRequestDetail);
            console.log(DataGridView2RowIndex);
            BaseResult.ListWarehouseRequestDetail.splice(DataGridView2RowIndex, 1);
            DataGridView2Render();
            console.log(BaseResult.ListWarehouseRequestDetail);
        }
    }
    if (TagIndex == 2) {
        //let IsDelete = true;
        //Buttonfind_Click();
        //let D3_D01 = BaseResult.T2_DGV1[T2_DGV1RowIndex].CODE;
        //let D3_D02 = BaseResult.T2_DGV1[T2_DGV1RowIndex].TMMTIN_CNF_YN;
        //if (D3_D02 == "Y") {
        //    alert(localStorage.getItem("Notification_B09_001"));
        //    IsDelete = false;
        //}
        //if (IsDelete == true) {
        //    if (confirm(localStorage.getItem("DeleteConfirm"))) {
        //        $("#BackGround").css("display", "block");
        //        let BaseParameter = new Object();
        //        BaseParameter = {
        //            Action: TagIndex,
        //            SearchString: D3_D01,
        //        }
        //        let formUpload = new FormData();
        //        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
        //        let url = "/B09/Buttondelete_Click";

        //        fetch(url, {
        //            method: "POST",
        //            body: formUpload,
        //            headers: {
        //            }
        //        }).then((response) => {
        //            response.json().then((data) => {
        //                Buttonfind_Click();
        //                alert(localStorage.getItem("SaveSuccess"));
        //                $("#BackGround").css("display", "none");
        //            }).catch((err) => {
        //                $("#BackGround").css("display", "none");
        //            })
        //        });
        //    }
        //}

        if (BaseResult.ListWarehouseRequestDetail2 && BaseResult.ListWarehouseRequestDetail2.length > 0) {
            let IDERP = BaseResult.ListWarehouseRequestDetail2[T2_DGV1RowIndex].ID;
            let IsDelete = !BaseResult.ListWarehouseRequestDetail2[T2_DGV1RowIndex].Active;
            if (IsDelete == true) {
                if (confirm(localStorage.getItem("DeleteConfirm"))) {
                    $("#BackGround").css("display", "block");
                    let BaseParameter = new Object();
                    BaseParameter = {
                        Action: TagIndex,
                        IDERP: IDERP,
                    }
                    let formUpload = new FormData();
                    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
                    let url = "/B09/RemoveWarehouseRequestDetailByID";

                    fetch(url, {
                        method: "POST",
                        body: formUpload,
                        headers: {
                        }
                    }).then((response) => {
                        response.json().then((data) => {
                            alert(localStorage.getItem("DeleteSuccess"));
                            BaseResult.ListWarehouseRequestDetail2.splice(T2_DGV1RowIndex, 1);
                            T2_DGV1Render();
                            $("#BackGround").css("display", "none");
                        }).catch((err) => {
                            $("#BackGround").css("display", "none");
                        })
                    });
                }
            }
            else {
                alert(localStorage.getItem("DeleteNotSuccess"));
            }
        }
    }
}
function Buttoncancel_Click() {
    if (TagIndex == 1) {
        BaseResult.DataGridView2 = [];
    }
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
$("#RadioButton1").click(function () {
    RadioButton1_Click();
});
$("#RadioButton2").click(function () {
    RadioButton1_Click();
});
function RadioButton1_Click() {
    let RadioButton1 = document.getElementById("RadioButton1").checked;
    let RadioButton2 = document.getElementById("RadioButton2").checked;
    if (RadioButton1 == true) {
        COMLIST_LINE();
        FNM = 0;
    }
    else {
        COMLIST_LINE();
        FNM = 1;
    }
    localStorage.setItem("FNM_B09", FNM);
}
$("#cb_Stage1").change(function () {
    ComboBox1_SelectedIndexChanged();
});
function ComboBox1_SelectedIndexChanged() {
    //BaseResult.DataGridView2 = [];
    BaseResult.ListWarehouseRequestDetail = [];
}

//function DataGridView1Render() {
//    let HTML = "";
//    if (BaseResult) {
//        if (BaseResult.DataGridView1) {
//            if (BaseResult.DataGridView1.length > 0) {
//                for (let i = 0; i < BaseResult.DataGridView1.length; i++) {
//                    HTML = HTML + "<tr>";
//                    HTML = HTML + "<td onclick='DataGridView1_CellClick(" + i + ")'><button class='btn waves-effect waves-light grey darken-1'>" + BaseResult.DataGridView1[i].ORDER + "</button></td>";
//                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].PART_IDX + "</td>";
//                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].TMMTIN_CODE + "</td>";
//                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].PART_NO + "</td>";
//                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].PART_NM + "</td>";
//                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].PART_FML + "</td>";
//                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].PART_SNP + "</td>";
//                    HTML = HTML + "<td><input id='DataGridView1QTY" + i + "' type='number' class='form-control' value='" + BaseResult.DataGridView1[i].QTY + "'></td>";
//                    HTML = HTML + "<td>" + BaseResult.DataGridView1[i].STOCK + "</td>";
//                    HTML = HTML + "</tr>";
//                }
//            }
//        }
//    }
//    document.getElementById("DataGridView1").innerHTML = HTML;
//}
//function DataGridView1Sort() {
//    IsTableSort = !IsTableSort;
//    DataGridViewSort(BaseResult.DataGridView1, IsTableSort);
//    DataGridView1Render();
//}
//function DataGridView1_CellClick(i) {
//    let IsCellClick = true;
//    DataGridView1RowIndex = i;
//    BaseResult.DataGridView1[DataGridView1RowIndex].QTY = $("#DataGridView1QTY" + DataGridView1RowIndex).val();
//    let cb_Stage1 = $("#cb_Stage1").val();
//    if (cb_Stage1 == "Select Stage...") {
//        IsCellClick = false;
//    }

//    if (IsCellClick == true) {
//        let AA = cb_Stage1;
//        let DT = $("#DateTimePicker1").val();
//        let BB = BaseResult.DataGridView1[DataGridView1RowIndex].PART_IDX;
//        let CC = BaseResult.DataGridView1[DataGridView1RowIndex].PART_NO;
//        let DD = BaseResult.DataGridView1[DataGridView1RowIndex].PART_NM;
//        let EE = BaseResult.DataGridView1[DataGridView1RowIndex].PART_FML;
//        let FF = BaseResult.DataGridView1[DataGridView1RowIndex].PART_SNP;
//        let GG = BaseResult.DataGridView1[DataGridView1RowIndex].QTY;
//        let HH = BaseResult.DataGridView1[DataGridView1RowIndex].TMMTIN_CODE;
//        if (GG == 0) {
//            if (FF == 0) {
//                IsCellClick = false;
//            }
//            else {
//                GG = FF;
//            }
//        }
//        if (IsCellClick == true) {
//            let PART_CHK = false;
//            if (BaseResult.DataGridView2.length > 0) {
//                for (let i = 0; i < BaseResult.DataGridView2.length; i++) {
//                    if (CC == BaseResult.DataGridView2[i].PART_NO) {
//                        BaseResult.DataGridView2[i].QTY = BaseResult.DataGridView2[i].QTY + GG;
//                        PART_CHK = true;
//                        i = BaseResult.DataGridView2.length;
//                    }
//                }
//            }
//            if (PART_CHK == false) {
//                let DataGridView2Item = new Object();
//                DataGridView2Item = {
//                    STAGE: AA,
//                    DATE: DT,
//                    DJG_CODE: BB,
//                    TYPE: HH,
//                    PART_NO: CC,
//                    PART_NAME: DD,
//                    FAMILY: EE,
//                    PART_SNP: FF,
//                    QTY: GG,
//                }
//                BaseResult.DataGridView2.push(DataGridView2Item);
//            }
//            DataGridView2Render();
//        }
//    }
//}

function WarehouseRequestRender() {
    let HTML = "";
    if (BaseResult) {
        if (BaseResult.ListWarehouseRequest) {
            if (BaseResult.ListWarehouseRequest.length > 0) {
                WarehouseRequestClick(BaseResult.ListWarehouseRequest[0].ID);
                for (let i = 0; i < BaseResult.ListWarehouseRequest.length; i++) {
                    HTML = HTML + "<tr onclick='WarehouseRequestClick(" + BaseResult.ListWarehouseRequest[i].ID + ")'>";
                    let No = i + 1;
                    HTML = HTML + "<td>" + No + "</td>";
                    HTML = HTML + "<td>" + BaseResult.ListWarehouseRequest[i].ID + "</td>";
                    HTML = HTML + "<td style='font-size: 12px;'>" + BaseResult.ListWarehouseRequest[i].Code + "</td>";
                    HTML = HTML + "<td style='font-size: 12px;'>" + BaseResult.ListWarehouseRequest[i].Date + "</td>";
                    HTML = HTML + "</tr>";
                }
            }
        }
    }
    document.getElementById("WarehouseRequest").innerHTML = HTML;
}
function WarehouseRequestSort() {
    IsTableSort = !IsTableSort;
    DataGridViewSort(BaseResult.ListWarehouseRequest, IsTableSort);
    WarehouseRequestRender();
}
let WarehouseRequestID = 0;
function WarehouseRequestClick(ID) {
    WarehouseRequestID = ID;
    GetWarehouseRequestDetailByParentIDToList();
}
function GetWarehouseRequestDetailByParentIDToList() {
    $("#BackGround").css("display", "block");
    let MembershipID = GetCookieValue("USER_IDX");
    let BaseParameter = new Object();
    BaseParameter = {
        Action: TagIndex,
        CompanyID: CompanyID,
        CategoryDepartmentID: Number($("#Tag003CategoryDepartment").val()),
        Date: $("#Tag003Date").val(),
        MembershipID: MembershipID,
        SearchString: $("#Tag003PartNo").val(),
        ID: WarehouseRequestID,
    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/B09/GetWarehouseRequestDetailByParentIDToList";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            let BaseResultSub = data;
            BaseResult.ListWarehouseRequestDetail1 = BaseResultSub.ListWarehouseRequestDetail1;
            WarehouseRequestDetailRender();
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });

}
function WarehouseRequestDetailRender() {
    let HTML = "";
    console.log(BaseResult.ListWarehouseRequestDetail1);
    if (BaseResult) {
        if (BaseResult.ListWarehouseRequestDetail1) {
            if (BaseResult.ListWarehouseRequestDetail1.length > 0) {
                for (let i = 0; i < BaseResult.ListWarehouseRequestDetail1.length; i++) {
                    console.log(BaseResult.ListWarehouseRequestDetail1[i]);
                    let No = i + 1;
                    HTML = HTML + "<tr>";
                    HTML = HTML + "<td>" + No + "</td>";
                    HTML = HTML + "<td>" + BaseResult.ListWarehouseRequestDetail1[i].Active + "</td>";
                    HTML = HTML + "<td style='font-size: 12px;'>" + BaseResult.ListWarehouseRequestDetail1[i].Code + "</td>";
                    HTML = HTML + "<td>" + BaseResult.ListWarehouseRequestDetail1[i].MaterialName + "</td>";
                    HTML = HTML + "<td style='color: #000000; text-align: right;'>" + BaseResult.ListWarehouseRequestDetail1[i].Quantity + "</td>";
                    HTML = HTML + "<td style='font-size: 12px;'>" + BaseResult.ListWarehouseRequestDetail1[i].UpdateDate + "</td>";
                    HTML = HTML + "<td>" + BaseResult.ListWarehouseRequestDetail1[i].ID + "</td>";
                    HTML = HTML + "</tr>";
                }
            }
        }
    }
    document.getElementById("WarehouseRequestDetail").innerHTML = HTML;
}
function WarehouseRequestDetailSort() {
    IsTableSort = !IsTableSort;
    DataGridViewSort(BaseResult.ListWarehouseRequestDetail1, IsTableSort);
    WarehouseRequestDetailRender();
}

function WarehouseOutputRender() {
    let HTML = "";
    let Now = new Date();
    let Year = Now.getFullYear();
    let Month = Now.getMonth() + 1;
    let Day = Now.getDate();
    let Tag003Confirm = 0;
    if (BaseResult) {
        if (BaseResult.ListWarehouseOutput) {
            if (BaseResult.ListWarehouseOutput.length > 0) {
                WarehouseOutputClick(BaseResult.ListWarehouseOutput[0].ID);

                for (let i = 0; i < BaseResult.ListWarehouseOutput.length; i++) {
                    if (BaseResult.ListWarehouseOutput[i].IsComplete) {
                        HTML = HTML + "<tr onclick='WarehouseOutputClick(" + BaseResult.ListWarehouseOutput[i].ID + ")' style='color: green;'>";
                    }
                    else {
                        HTML = HTML + "<tr onclick='WarehouseOutputClick(" + BaseResult.ListWarehouseOutput[i].ID + ")'>";
                    }
                    if (BaseResult.ListWarehouseOutput[i].IsComplete) {

                        let WarehouseOutputDate = new Date(BaseResult.ListWarehouseOutput[i].Date);
                        let WarehouseOutputDateYear = WarehouseOutputDate.getFullYear();
                        let WarehouseOutputDateMonth = WarehouseOutputDate.getMonth() + 1;
                        let WarehouseOutputDateDay = WarehouseOutputDate.getDate();
                        if (WarehouseOutputDateYear == Year && WarehouseOutputDateMonth == Month && WarehouseOutputDateDay == Day) {
                            HTML = HTML + "<td></td>";
                        }
                        else {
                            Tag003Confirm = Tag003Confirm + 1;
                            HTML = HTML + "<td onclick='WarehouseOutputCellClick(" + BaseResult.ListWarehouseOutput[i].ID + ")'><button class='btn waves-effect waves-light grey darken-1'>Confirm</button></td>";
                        }
                    }
                    else {
                        HTML = HTML + "<td></td>";
                    }
                    let No = i + 1;
                    HTML = HTML + "<td>" + No + "</td>";
                    if (BaseResult.ListWarehouseOutput[i].IsComplete) {
                        HTML = HTML + "<td><label><input id='ListWarehouseOutputIsComplete" + i + "' class='form-check-input' type='checkbox' checked><span></span></label></td>";
                    }
                    else {
                        HTML = HTML + "<td><label><input id='ListWarehouseOutputIsComplete" + i + "' class='form-check-input' type='checkbox'><span></span></label></td>";
                    }
                    HTML = HTML + "<td>" + BaseResult.ListWarehouseOutput[i].ID + "</td>";
                    HTML = HTML + "<td>" + BaseResult.ListWarehouseOutput[i].Code + "</td>";
                    HTML = HTML + "<td style='font-size: 12px;'>" + BaseResult.ListWarehouseOutput[i].Date + "</td>";
                    HTML = HTML + "</tr>";
                }
            }
        }
    }
    document.getElementById("WarehouseOutput").innerHTML = HTML;
    $("#Tag003Confirm").val(Tag003Confirm);
}
function WarehouseOutputSort() {
    IsTableSort = !IsTableSort;
    DataGridViewSort(BaseResult.ListWarehouseOutput, IsTableSort);
    WarehouseOutputRender();
}
let WarehouseOutputID = 0;
function WarehouseOutputClick(ID) {
    WarehouseOutputID = ID;
    GetWarehouseOutputDetailByParentIDToList();
}
function WarehouseOutputCellClick(ID) {
    //$("#BackGround").css("display", "block");
    let MembershipID = GetCookieValue("USER_IDX");
    let BaseParameter = new Object();
    BaseParameter = {
        Action: TagIndex,
        CompanyID: CompanyID,
        CategoryDepartmentID: Number($("#T2_S1").val()),
        MembershipID: MembershipID,
        ID: ID,
    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/B09/SaveWarehouseInputByWarehouseOutputID";

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
function GetWarehouseOutputDetailByParentIDToList() {
    $("#BackGround").css("display", "block");
    let MembershipID = GetCookieValue("USER_IDX");
    let BaseParameter = new Object();
    BaseParameter = {
        Action: TagIndex,
        CompanyID: CompanyID,
        CategoryDepartmentID: Number($("#T2_S1").val()),
        Date: $("#T2_S2").val(),
        MembershipID: MembershipID,
        SearchString: $("#T2_S4").val(),
        ID: WarehouseOutputID,
    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/B09/GetWarehouseOutputDetailByParentIDToList";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            let BaseResultSub = data;
            BaseResult.ListWarehouseOutputDetail = BaseResultSub.ListWarehouseOutputDetail;
            WarehouseOutputDetailRender();
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });

}
function WarehouseOutputDetailRender() {
    let HTML = "";
    if (BaseResult) {
        if (BaseResult.ListWarehouseOutputDetail) {
            if (BaseResult.ListWarehouseOutputDetail.length > 0) {
                for (let i = 0; i < BaseResult.ListWarehouseOutputDetail.length; i++) {
                    if (BaseResult.ListWarehouseOutputDetail[i].QuantityGAP > 0 || BaseResult.ListWarehouseOutputDetail[i].Active != true) {
                        HTML = HTML + "<tr style='color: red;'>";
                    }
                    else {
                        HTML = HTML + "<tr style='color: green;'>";
                    }
                    HTML = HTML + "<td onclick='WarehouseOutputDetailClick(" + i + ")'><button class='btn waves-effect waves-light cyan darken-1'>Save</button></td>";
                    let No = i + 1;
                    HTML = HTML + "<td>" + No + "</td>";
                    HTML = HTML + "<td>" + BaseResult.ListWarehouseOutputDetail[i].Active + "</td>";
                    HTML = HTML + "<td style='font-size: 12px;'>" + BaseResult.ListWarehouseOutputDetail[i].ParentName + "</td>";
                    HTML = HTML + "<td>" + BaseResult.ListWarehouseOutputDetail[i].MaterialName + "</td>";
                    HTML = HTML + "<td style='color: #000000; text-align: right;'>" + BaseResult.ListWarehouseOutputDetail[i].Quantity + "</td>";
                    /*HTML = HTML + "<td style='color: green; text-align: right;'><b>" + BaseResult.ListWarehouseOutputDetail[i].QuantityActual + "</b></td>";*/
                    HTML = HTML + "<td><input onblur='WarehouseOutputDetailCellClick(" + i + ", this)' type='number' class='form-control' value='" + BaseResult.ListWarehouseOutputDetail[i].QuantityActual + "' style='width: 100px !important;'></td>";
                    HTML = HTML + "<td style='color: red; text-align: right;'>" + BaseResult.ListWarehouseOutputDetail[i].QuantityGAP + "</td>";
                    HTML = HTML + "<td style='font-size: 12px;'>" + BaseResult.ListWarehouseOutputDetail[i].UpdateDate + "</td>";
                    HTML = HTML + "<td>" + BaseResult.ListWarehouseOutputDetail[i].ID + "</td>";
                    HTML = HTML + "</tr>";
                }
            }
        }
    }
    document.getElementById("WarehouseOutputDetail").innerHTML = HTML;
}
function WarehouseOutputDetailSort() {
    IsTableSort = !IsTableSort;
    DataGridViewSort(BaseResult.ListWarehouseOutputDetail, IsTableSort);
    WarehouseOutputDetailRender();
}
function WarehouseOutputDetailCellClick(i, input) {
    BaseResult.ListWarehouseOutputDetail[i].QuantityActual = input;
}
function WarehouseOutputDetailClick() {
    $("#BackGround").css("display", "block");
    let MembershipID = GetCookieValue("USER_IDX");
    let BaseParameter = new Object();
    BaseParameter = {
        ID: BaseResult.ListWarehouseOutputDetail[i].ID,
        Qty: BaseResult.ListWarehouseOutputDetail[i].QuantityActual,
    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/B09/SaveWarehouseOutputDetailByID";

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
function DataGridView1Render() {
    let HTML = "";
    if (BaseResult) {
        if (BaseResult.ListMaterial) {
            if (BaseResult.ListMaterial.length > 0) {
                for (let i = 0; i < BaseResult.ListMaterial.length; i++) {
                    HTML = HTML + "<tr>";
                    HTML = HTML + "<td onclick='DataGridView1_CellClick(" + i + ")'><button class='btn waves-effect waves-light grey darken-1'>ORDER</button></td>";
                    HTML = HTML + "<td>" + BaseResult.ListMaterial[i].ID + "</td>";
                    HTML = HTML + "<td>" + BaseResult.ListMaterial[i].CategoryMaterialName + "</td>";
                    HTML = HTML + "<td>" + BaseResult.ListMaterial[i].CategoryLineName + "</td>";
                    HTML = HTML + "<td><b>" + BaseResult.ListMaterial[i].Code + "</b></td>";
                    HTML = HTML + "<td>" + BaseResult.ListMaterial[i].Name + "</td>";
                    HTML = HTML + "<td>" + BaseResult.ListMaterial[i].CategoryFamilyName + "</td>";
                    HTML = HTML + "<td>" + BaseResult.ListMaterial[i].QuantitySNP + "</td>";
                    HTML = HTML + "<td><input id='DataGridView1QTY" + i + "' type='number' class='form-control' value='" + BaseResult.ListMaterial[i].Quantity + "' style='width: 200px;'></td>";
                    //if (BaseResult.ListMaterial[i].QuantitySNP > 0) {
                    //    BaseResult.ListMaterial[i].QuantityInput = BaseResult.ListMaterial[i].QuantitySNP * 10;
                    //}
                    //else {
                    //    BaseResult.ListMaterial[i].QuantityInput = 10;
                    //}
                    HTML = HTML + "<td><b>" + BaseResult.ListMaterial[i].QuantityInput + "</b></td>";
                    HTML = HTML + "</tr>";
                }
            }
        }
    }
    document.getElementById("DataGridView1").innerHTML = HTML;
}
function DataGridView1Sort() {
    IsTableSort = !IsTableSort;
    DataGridViewSort(BaseResult.ListMaterial, IsTableSort);
    DataGridView1Render();
}
function DataGridView1_CellClick(i) {
    DataGridView1RowIndex = i;
    BaseResult.ListMaterial[DataGridView1RowIndex].Quantity = Number($("#DataGridView1QTY" + DataGridView1RowIndex).val());
    if (BaseResult.ListMaterial[DataGridView1RowIndex].Quantity <= 0) {
        BaseResult.ListMaterial[DataGridView1RowIndex].Quantity = BaseResult.ListMaterial[i].QuantitySNP;
    }
    let WarehouseRequestDetail = new Object();
    WarehouseRequestDetail = {
        CompanyID: CompanyID,
        UpdateUserID: Number(GetCookieValue("USER_IDX")),
        ProductID: $("#cb_Stage1").val(),
        ProductName: $("#cb_Stage1 option:selected").text(),
        Date: $("#DateTimePicker1").val(),
        MaterialID: BaseResult.ListMaterial[DataGridView1RowIndex].ID,
        MaterialName: BaseResult.ListMaterial[DataGridView1RowIndex].Code,
        Display: BaseResult.ListMaterial[DataGridView1RowIndex].Name,
        Description: BaseResult.ListMaterial[DataGridView1RowIndex].CategoryMaterialName,
        CategoryFamilyName: BaseResult.ListMaterial[DataGridView1RowIndex].CategoryFamilyName,
        QuantitySNP: BaseResult.ListMaterial[DataGridView1RowIndex].QuantitySNP,
        Quantity: BaseResult.ListMaterial[DataGridView1RowIndex].Quantity,
    }
    let IsSave = true;
    for (let j = 0; j < BaseResult.ListWarehouseRequestDetail.length; j++) {
        if (BaseResult.ListWarehouseRequestDetail[j].MaterialID == WarehouseRequestDetail.MaterialID) {
            IsSave = false;
            BaseResult.ListWarehouseRequestDetail[j].Quantity = BaseResult.ListWarehouseRequestDetail[j].Quantity + WarehouseRequestDetail.Quantity;
        }
    }
    if (IsSave == true) {
        BaseResult.ListWarehouseRequestDetail.push(WarehouseRequestDetail);
    }

    DataGridView2Render();
}
//function DataGridView2Render() {
//    let HTML = "";
//    if (BaseResult) {
//        if (BaseResult.DataGridView2) {
//            if (BaseResult.DataGridView2.length > 0) {
//                for (let i = 0; i < BaseResult.DataGridView2.length; i++) {
//                    HTML = HTML + "<tr onclick='DataGridView2_SelectionChanged(" + i + ")'>";
//                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].STAGE + "</td>";
//                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].DATE + "</td>";
//                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].DJG_CODE + "</td>";
//                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].TYPE + "</td>";
//                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].PART_NO + "</td>";
//                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].PART_NAME + "</td>";
//                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].FAMILY + "</td>";
//                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].PART_SNP + "</td>";
//                    HTML = HTML + "<td>" + BaseResult.DataGridView2[i].QTY + "</td>";
//                    HTML = HTML + "</tr>";
//                }
//            }
//        }
//    }
//    document.getElementById("DataGridView2").innerHTML = HTML;
//}
//function DataGridView2Sort() {
//    IsTableSort = !IsTableSort;
//    DataGridViewSort(BaseResult.DataGridView2, IsTableSort);
//    DataGridView2Render();
//}
function DataGridView2_SelectionChanged(i) {
    DataGridView2RowIndex = i;
}

function DataGridView2Render() {
    let HTML = "";
    if (BaseResult) {
        if (BaseResult.ListWarehouseRequestDetail) {
            if (BaseResult.ListWarehouseRequestDetail.length > 0) {
                for (let i = 0; i < BaseResult.ListWarehouseRequestDetail.length; i++) {
                    HTML = HTML + "<tr onclick='DataGridView2_SelectionChanged(" + i + ")'>";
                    HTML = HTML + "<td>" + BaseResult.ListWarehouseRequestDetail[i].ProductName + "</td>";
                    HTML = HTML + "<td>" + BaseResult.ListWarehouseRequestDetail[i].Date + "</td>";
                    HTML = HTML + "<td>" + BaseResult.ListWarehouseRequestDetail[i].MaterialID + "</td>";
                    HTML = HTML + "<td>" + BaseResult.ListWarehouseRequestDetail[i].Description + "</td>";
                    HTML = HTML + "<td><b>" + BaseResult.ListWarehouseRequestDetail[i].MaterialName + "</b></td>";
                    HTML = HTML + "<td>" + BaseResult.ListWarehouseRequestDetail[i].Display + "</td>";
                    HTML = HTML + "<td>" + BaseResult.ListWarehouseRequestDetail[i].CategoryFamilyName + "</td>";
                    HTML = HTML + "<td>" + BaseResult.ListWarehouseRequestDetail[i].QuantitySNP + "</td>";
                    HTML = HTML + "<td><b>" + BaseResult.ListWarehouseRequestDetail[i].Quantity + "</b></td>";
                    HTML = HTML + "</tr>";
                }
            }
        }
    }
    document.getElementById("DataGridView2").innerHTML = HTML;
}
function DataGridView2Sort() {
    IsTableSort = !IsTableSort;
    DataGridViewSort(BaseResult.ListWarehouseRequestDetail, IsTableSort);
    DataGridView2Render();
}

//function T2_DGV1Render() {
//    let HTML = "";
//    if (BaseResult) {
//        if (BaseResult.T2_DGV1) {
//            if (BaseResult.T2_DGV1.length > 0) {
//                for (let i = 0; i < BaseResult.T2_DGV1.length; i++) {
//                    HTML = HTML + "<tr onclick='T2_DGV1_SelectionChanged(" + i + ")'>";
//                    if (BaseResult.T2_DGV1[i].TMMTIN_CNF_YN == "Y") {
//                        HTML = HTML + "<td style='background-color: lightgreen;'>" + BaseResult.T2_DGV1[i].TMMTIN_CNF_YN + "</td>";
//                    }
//                    else {
//                        HTML = HTML + "<td style='background-color: palevioletred;'>" + BaseResult.T2_DGV1[i].TMMTIN_CNF_YN + "</td>";
//                    }
//                    if (BaseResult.T2_DGV1[i].TMMTIN_DSCN_YN == "Y") {
//                        HTML = HTML + "<td style='background-color: lightgreen;'>" + BaseResult.T2_DGV1[i].TMMTIN_DSCN_YN + "</td>";
//                    }
//                    else {
//                        if (BaseResult.T2_DGV1[i].TMMTIN_DSCN_YN == "C") {
//                            HTML = HTML + "<td style='background-color: yellow;'>" + BaseResult.T2_DGV1[i].TMMTIN_DSCN_YN + "</td>";
//                        }
//                        else {
//                            HTML = HTML + "<td style='background-color: palevioletred;'>" + BaseResult.T2_DGV1[i].TMMTIN_DSCN_YN + "</td>";
//                        }
//                    }
//                    HTML = HTML + "<td>" + BaseResult.T2_DGV1[i].PART_NO + "</td>";
//                    HTML = HTML + "<td>" + BaseResult.T2_DGV1[i].TMMTIN_CODE + "</td>";
//                    HTML = HTML + "<td>" + BaseResult.T2_DGV1[i].SNP + "</td>";
//                    HTML = HTML + "<td>" + BaseResult.T2_DGV1[i].QTY + "</td>";
//                    HTML = HTML + "<td>" + BaseResult.T2_DGV1[i].PART_NAME + "</td>";
//                    HTML = HTML + "<td>" + BaseResult.T2_DGV1[i].CREATE_DTM + "</td>";
//                    HTML = HTML + "</tr>";
//                }
//            }
//        }
//    }
//    document.getElementById("T2_DGV1").innerHTML = HTML;
//}
//function T2_DGV1Sort() {
//    IsTableSort = !IsTableSort;
//    DataGridViewSort(BaseResult.T2_DGV1, IsTableSort);
//    T2_DGV1Render();
//}

function T2_DGV1Render() {
    let HTML = "";
    if (BaseResult) {
        if (BaseResult.ListWarehouseRequestDetail2) {
            if (BaseResult.ListWarehouseRequestDetail2.length > 0) {
                for (let i = 0; i < BaseResult.ListWarehouseRequestDetail2.length; i++) {
                    HTML = HTML + "<tr onclick='T2_DGV1_SelectionChanged(" + i + ")'>";
                    HTML = HTML + "<td style='background-color: lightgreen;'>Y</td>";
                    //if (BaseResult.ListWarehouseRequestDetail2[i].Active == true) {
                    //    HTML = HTML + "<td style='background-color: lightgreen;'>Y</td>";
                    //}
                    //else {
                    //    HTML = HTML + "<td style='background-color: palevioletred;'>N</td>";
                    //}
                    HTML = HTML + "<td style='background-color: palevioletred;'>N</td>";
                    HTML = HTML + "<td>" + BaseResult.ListWarehouseRequestDetail2[i].MaterialName + "</td>";
                    HTML = HTML + "<td>Material</td>";
                    HTML = HTML + "<td>" + BaseResult.ListWarehouseRequestDetail2[i].QuantitySNP + "</td>";
                    HTML = HTML + "<td>" + BaseResult.ListWarehouseRequestDetail2[i].Quantity + "</td>";
                    HTML = HTML + "<td>" + BaseResult.ListWarehouseRequestDetail2[i].Display + "</td>";
                    HTML = HTML + "<td>" + BaseResult.ListWarehouseRequestDetail2[i].Date + "</td>";
                    HTML = HTML + "</tr>";
                }
            }
        }
    }
    document.getElementById("T2_DGV1").innerHTML = HTML;
}
function T2_DGV1Sort() {
    IsTableSort = !IsTableSort;
    DataGridViewSort(BaseResult.ListWarehouseRequestDetail2, IsTableSort);
    T2_DGV1Render();
}
function T2_DGV1_SelectionChanged(i) {
    T2_DGV1RowIndex = i;
}
function T2_DGV1_DataBindingComplete() {
    D1_SORT();
}
function D1_SORT() {
    if (BaseResult) {
        if (BaseResult.T2_DGV1) {
            if (BaseResult.T2_DGV1.length > 0) {
                for (let i = 0; i < BaseResult.T2_DGV1.length; i++) {
                }
            }
        }
    }
}
function GetListCategoryDepartment() {
    $("#BackGround").css("display", "block");
    let MembershipID = GetCookieValue("USER_IDX");

    let BaseParameter = new Object();
    BaseParameter = {
        CompanyID: CompanyID,
        MembershipID: MembershipID
    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/B09/GetListCategoryDepartment";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            let BaseResultSub = data;
            BaseResult.ListCategoryDepartment = BaseResultSub.ListCategoryDepartment;
            $("#cb_Stage1").empty();
            $("#T2_S1").empty();
            $("#Tag003CategoryDepartment").empty();
            if (BaseResult) {
                if (BaseResult.ListCategoryDepartment) {
                    if (BaseResult.ListCategoryDepartment.length > 0) {
                        for (let i = 0; i < BaseResult.ListCategoryDepartment.length; i++) {
                            var option = document.createElement("option");
                            option.text = BaseResult.ListCategoryDepartment[i].Name;
                            option.value = BaseResult.ListCategoryDepartment[i].ID;

                            var cb_Stage1 = document.getElementById("cb_Stage1");
                            cb_Stage1.add(option);

                            var option2 = document.createElement("option");
                            option2.text = BaseResult.ListCategoryDepartment[i].Name;
                            option2.value = BaseResult.ListCategoryDepartment[i].ID;

                            var T2_S1 = document.getElementById("T2_S1");
                            T2_S1.add(option2);

                            var option3 = document.createElement("option");
                            option3.text = BaseResult.ListCategoryDepartment[i].Name;
                            option3.value = BaseResult.ListCategoryDepartment[i].ID;

                            var Tag003CategoryDepartment = document.getElementById("Tag003CategoryDepartment");
                            Tag003CategoryDepartment.add(option3);
                        }
                    }
                }
            }

            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}

function COMLIST_LINE() {
    $("#BackGround").css("display", "block");
    let RadioButton1 = document.getElementById("RadioButton1").checked;
    let BaseParameter = new Object();
    BaseParameter = {
        RadioButton1: RadioButton1
    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/B09/COMLIST_LINE";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {
        }
    }).then((response) => {
        response.json().then((data) => {
            let BaseResultCOMLIST_LINE = data;
            BaseResult.cb_Stage1 = BaseResultCOMLIST_LINE.cb_Stage1;
            BaseResult.T2_S1 = BaseResultCOMLIST_LINE.T2_S1;
            cb_Stage1Render();
            T2_S1Render();
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}
function cb_Stage1Render() {
    $("#cb_Stage1").empty();
    if (BaseResult) {
        if (BaseResult.cb_Stage1) {
            if (BaseResult.cb_Stage1.length > 0) {
                for (let i = 0; i < BaseResult.cb_Stage1.length; i++) {
                    var option = document.createElement("option");
                    option.text = BaseResult.cb_Stage1[i].CD_SYS_NOTE;
                    option.value = BaseResult.cb_Stage1[i].CD_SYS_NOTE;

                    var cb_Stage1 = document.getElementById("cb_Stage1");
                    cb_Stage1.add(option);
                }
            }
        }
    }
}
function T2_S1Render() {
    $("#T2_S1").empty();
    if (BaseResult) {
        if (BaseResult.T2_S1) {
            if (BaseResult.T2_S1.length > 0) {
                for (let i = 0; i < BaseResult.T2_S1.length; i++) {
                    var option = document.createElement("option");
                    option.text = BaseResult.T2_S1[i].CD_SYS_NOTE;
                    option.value = BaseResult.T2_S1[i].CD_SYS_NOTE;

                    var T2_S1 = document.getElementById("T2_S1");
                    T2_S1.add(option);
                }
            }
        }
    }
}