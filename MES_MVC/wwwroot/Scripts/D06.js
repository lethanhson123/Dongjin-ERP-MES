let BaseResult;
let table2;
let controller = "/D06/"

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
    table2.ajax.reload(); // Chỉ cần gọi lại ajax
}
function Buttonadd_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = controller + "Buttonadd_Click";

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
    let url = controller + "Buttonsave_Click";

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
    let url = controller + "Buttondelete_Click";

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
    let url = controller + "Buttoncancel_Click";

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
    let url = controller + "Buttoninport_Click";

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
    ExportToExcelFull();
}
function Buttonprint_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = controller + "Buttonprint_Click";

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

let currentStart = 0; // Biến toàn cục để lưu start

$(document).ready(function () {
    // Set default date
    const today = new Date();
    const yyyy = today.getFullYear();
    const mm = String(today.getMonth() + 1).padStart(2, '0');
    const dd = String(today.getDate()).padStart(2, '0');
    const currentDate = `${yyyy}-${mm}-${dd}`;
    $('#txt_fromDate').val(currentDate);
    $('#txt_toDate').val(currentDate);

    table2 = $("#DataTable").DataTable({
        serverSide: true,
        processing: true,
        scrollX: true,
        scrollY: "62vh",
        scrollCollapse: true,
        paging: true,
        pageLength: 5000,
        lengthChange: false,
        ordering: false,
        searching: false,
        ajax: {
            url: controller + "Buttonfind_Click",
            type: "POST",
            data: function (d) {
                currentStart = d.start;
                d.BaseParameter = JSON.stringify({
                    FromDate: $("#txt_fromDate").val(),
                    ToDate: $("#txt_toDate").val(),
                    PartNo: $("#txt_PartNo").val(),
                    PartName: $("#txt_PartName").val(),
                    ShippNo: $("#txt_POShippCode").val(),
                    PalletNo: $("#txt_PalletNo").val(),
                    PackingLotCode: $("#txt_PackingLot").val(),
                    StartNumber: d.start,
                    Length: d.length
                });
                // Hiện tiến trình khi bắt đầu gửi dữ liệu
                $("#BackGround").css("display", "block");
            },
            dataSrc: function (json) {
                // Ẩn tiến trình khi dữ liệu đã nhận
                $("#BackGround").css("display", "none");

                if (!json || !json.data) {
                    console.error("Không có dữ liệu trả về hoặc lỗi backend", json);
                    return [];
                }
                return json.data;
            },
            error: function (xhr, error, thrown) {
                // Ẩn tiến trình nếu xảy ra lỗi
                $("#BackGround").css("display", "none");            
            }
        },
        columns: [
            {
                data: null,
                render: function (data, type, row, meta) {
                    return currentStart + meta.row + 1;
                }
            },
            { data: "CODE" },
            { data: "PO_CODE" },
            { data: "PLET_NO" },
            { data: "CM_PALLET_NO" },
            { data: "PART_NO" },
            { data: "PART_GRP" },
            { data: "PART_NM" },
            { data: "PART_SNP" },
            { data: "VLID_GRP" },          
            { data: "PO_QTY" },
            { data: "QTY" },
            { data: "BOX_QTY" },
            { data: "Not_yet_packing" },
            { data: "Inventory" },
            { data: "CREATE_DTM" },
            { data: "CREATE_USER" },
            { data: "UPDATE_DTM" },
            { data: "UPDATE_USER" },
        ]
    });

    // Thêm dòng này để đảm bảo luôn hiện thanh cuộn
    $(".dataTables_scrollBody").css({
        "min-height": "62vh",
        "overflow-y": "scroll"
    });
});


//async function ExportToExcelFull() { 

//    //truyền các biến trước khi gửi lên server
//    const BaseParameter = {
//        FromDate: $("#txt_fromDate").val(),
//        ToDate: $("#txt_toDate").val(),
//        PartNo: $("#txt_PartNo").val(),
//        PartName: $("#txt_PartName").val(),
//        ShippNo: $("#txt_POShippCode").val(),
//        PalletNo: $("#txt_PalletNo").val(),
//        PackingLotCode: $("#txt_PackingLot").val(),
//    };

//    const formUpload = new FormData();
//    formUpload.append("BaseParameter", JSON.stringify(BaseParameter));

//    //thục hiện hiên thị process loadding...
//    $("#BackGround").fadeIn();

//    try {
//        //truyền và chờ server trả kết quả
//        const response = await fetch(controller + "Buttonexport_Click", {
//            method: "POST",
//            body: formUpload
//        });
//        //chuyển đổi dữ liệu nhận được
//        const data = await response.json();

//        if (data.DataGridView8 && data.DataGridView8.length > 0) {
//            //lọc các trường cần xuất excel
//            const exportData = data.DataGridView8.map(x => ({
//                "Code": x.CODE,
//                "Shipping_CODE": x.PO_CODE,
//                "Pallet_NO": x.PLET_NO,
//                "CM_PALLET_NO": x.CM_PALLET_NO,
//                "PART_NO": x.PART_NO,
//                "Group": x.PART_GRP,
//                "PART_Name": x.PART_NM,
//                "SNP": x.PART_SNP,
//                "Packing_Lot": x.VLID_GRP,
//                "PO_QTY": x.PO_QTY,
//                "QTY": x.QTY,
//                "BOX_QTY": x.BOX_QTY,
//                "Not_yet_packing": x.Not_yet_packing,
//                "Inventory": x.Inventory,
//                "Packing_Time": FormatDateTime(x.CREATE_DTM),
//                "Packing_By": x.CREATE_USER,
//                "Shipped_Time": FormatDateTime(x.UPDATE_DTM),
//                "Shipped_By": x.UPDATE_USER,
//            }));
//            ExportJSONToExcel(exportData, "Shipping history", "D06_ShippingHistory.xlsx");

//            ShowMessage(localizedText.Success, "ok");
//        } else {
//            ShowMessage(localizedText.Empty, "error");
//        }

//    } catch (err) {
//        ShowMessage(err.message || err, "error");
//    } finally {
//        $("#BackGround").fadeOut();
//    }
//}

async function ExportToExcelFull() {
    const BaseParameter = {
        FromDate: $("#txt_fromDate").val(),
        ToDate: $("#txt_toDate").val(),
        PartNo: $("#txt_PartNo").val(),
        PartName: $("#txt_PartName").val(),
        ShippNo: $("#txt_POShippCode").val(),
        PalletNo: $("#txt_PalletNo").val(),
        PackingLotCode: $("#txt_PackingLot").val(),
    };

    const formUpload = new FormData();
    formUpload.append("BaseParameter", JSON.stringify(BaseParameter));

    $("#BackGround").fadeIn();

    try {
        const response = await fetch(controller + "Buttonexport_Click", {
            method: "POST",
            body: formUpload
        });

        if (!response.ok) throw new Error("Tải file thất bại!");

        const blob = await response.blob();
        const url = window.URL.createObjectURL(blob);
        const link = document.createElement("a");
        link.href = url;
        link.download = "D06_ShippingHistory.xlsx";
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
        ShowMessage("Tải file thành công", "ok");
    } catch (err) {
        ShowMessage(err.message || err, "error");
    } finally {
        $("#BackGround").fadeOut();
    }
}


