let BaseResult;
let controller = "/A11/";
let Table1;
let Table2;

$(document).ready(function () {
    Table1 = $("#Table1").DataTable({
        scrollX: true,
        scrollY: "25vh",
        scrollCollapse: true,
        paging: true,
        pageLength: 1000,
        lengthChange: false,
        autoWidth: false,
        responsive: false,
        ordering: true,
        searching: false,
        fixedHeader: false,
        columnDefs: [
            { targets: "_all", orderable: true, className: "dt-center" },
            { targets: "_all", className: "dt-nowrap" }
        ],
        createdRow: function (row, data) {
            //if (data[14] === "D") {
            //    $(row).addClass("red-text");
            //}
        }
    });

    Table2 = $("#Table2").DataTable({
        scrollX: true,
        scrollY: "45vh",
        scrollCollapse: true,
        paging: true,
        pageLength: 1000,
        lengthChange: false,
        autoWidth: false,
        responsive: false,
        ordering: true,
        searching: false,
        fixedHeader: false,
        columnDefs: [
            { targets: "_all", orderable: true, className: "dt-center" },
            { targets: "_all", className: "dt-nowrap" }
        ],
        createdRow: function (row, data) {
            //if (data[14] === "D") {
            //    $(row).addClass("red-text");
            //}
        }
    });

    // Thêm min-height sau khi bảng đã khởi tạo, tùy vào kích thước mà điều chỉnh cho phù hợp màn hình
    $(".dataTables_scrollBody").css("min-height", "28vh");
    
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
   
}
function Buttonadd_Click() {
    showEditModal(null, function (result) {
        if (result) {
            AddNewMenu();
        } else {
            $("#BackGround").css("display", "none");
        }
    });
}
function Buttonsave_Click() {

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
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter = {
    }
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = controller + "Buttonexport_Click";

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