let BaseResult;
let PHOTO_DSYV = false;
let SAVE_BUTCHK = false;

$(document).ready(function () {
    Load();
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


$("#Button1").click(function () {
    Button1_Click();
});
$("#Button2").click(function () {
    Button2_Click();
});
$("#Button3").click(function () {
    Button3_Click();
});
$("#Button4").click(function () {
    Button4_Click();
});
$("#Button5").click(function () {
    Button5_Click();
});

function Load() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/Z07_1/Load";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {}
    }).then((response) => {
        response.json().then((data) => {
            BaseResult = data;
            LoadComboBoxData();
            $("#DEP_INDEX").text("New");
            $("#Label17").text(formatDate(new Date()));
            ResetDT();
            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}

function LoadComboBoxData() {
    if (BaseResult && BaseResult.Success) {
      
        $("#ComboBox2").empty();
        if (BaseResult.ComboBox2) {
            for (let i = 0; i < BaseResult.ComboBox2.length; i++) {
                $("#ComboBox2").append('<option value="' + BaseResult.ComboBox2[i].SCRN_PATH + '">' + BaseResult.ComboBox2[i].NAME + '</option>');
            }
        }
    }
}

function ResetDT() {
  
    $("#RichTextBox1").val("");
    $("#Label13").text("-");
    $("#Label14").text("-");
    $("#Label7").text("-");
    $("#Label12").text("-");
    $("#Label18").text("-");
    $("#PictureBox1").attr("src", "").hide();
    $("#Label7").css("background-color", "#f5f5f5");
    $("#Label12").css("background-color", "#f5f5f5");
}

function Buttonfind_Click() {
    if ($("#DEP_INDEX").text() === "New" || $("#DEP_INDEX").text() === "") {
        alert("Please enter a valid ID.");
        return;
    }

    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter.ListSearchString = [];
    BaseParameter.ListSearchString.push($("#DEP_INDEX").text());

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/Z07_1/Buttonfind_Click";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {}
    }).then((response) => {
        response.json().then((data) => {
            BaseResult = data;
            if (data.DataGridView1 && data.DataGridView1.length > 0) {
                let item = data.DataGridView1[0];

               
                $("#ComboBox4").val(item.DELP_DEPT);
                $("#ComboBox2").val(getScrnPathFromMenu(item.MENU));
                $("#ComboBox1").val(item.DELP_NAME);
                $("#RichTextBox1").val(item.DELP_DETIL);
                $("#Label17").text(formatDate(item.DELP_DATE));
                $("#Label7").text(item.FILE_NM);
                $("#Label8").text(item.FILE_EX);
                $("#Label12").text(item.FILE_SIZE);
                $("#Label18").text(item.USER);
                $("#Label13").text(item.MES_VER || "-");
                $("#Label14").text(formatDate(item.UPDATE_DTM));

               
                if (item.FILE_DSYN === "Y") {
                    $("#Label7").css("background-color", "#ADFF2F");
                    $("#Label12").css("background-color", "#ADFF2F");
                    SAVE_BUTCHK = true;
                } else {
                    $("#Label7").css("background-color", "#f5f5f5");
                    $("#Label12").css("background-color", "#f5f5f5");
                    SAVE_BUTCHK = false;
                }

              
                if (item.DEP_PHOTO) {
                    loadPhoto(item.DEP_PHOTO);
                } else {
                    $("#PictureBox1").attr("src", "").hide();
                }

               
                $("#Buttonsave").prop("disabled", item.STATE !== "N");
            }

            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}

function Buttonadd_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/Z07_1/Buttonadd_Click";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {}
    }).then((response) => {
        response.json().then((data) => {
          
            $("#DEP_INDEX").text("New");
            $("#Label17").text(formatDate(new Date()));
            ResetDT();

       
            $("#Buttonsave").prop("disabled", false);

            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}

function Buttonsave_Click() {
  
    if (!$("#RichTextBox1").val()) {
        alert("Memo field is required.");
        return;
    }

    $("#BackGround").css("display", "block");


    let FILE_DSYN = "N";
    let FILE_NM = "";
    let FILE_EX = "";
    let FILE_SIZE = "0";
    let PHOTO_NM = "";

 
    if (SAVE_BUTCHK) {
        F_NM = $("#ComboBox2").val() + "_" + formatDateCode(new Date());
        FILE_DSYN = "Y";
        FILE_NM = F_NM + ".Z07";
        FILE_EX = $("#Label8").text();
        FILE_SIZE = $("#Label12").text();

      
    }

 
    if (PHOTO_DSYV) {
        F_NM = $("#ComboBox2").val() + "_" + formatDateCode(new Date());
        PHOTO_NM = F_NM + ".png";

     
    }

    
    let BaseParameter = new Object();
    BaseParameter.ListSearchString = [];
    BaseParameter.ListSearchString.push($("#DEP_INDEX").text());
    BaseParameter.ListSearchString.push($("#ComboBox4").val());

    let menuVal = $("#ComboBox2").val();
    if (menuVal === "000") menuVal = "ALL";
    if (menuVal === "001") menuVal = "New Menu";
    BaseParameter.ListSearchString.push(menuVal);

    BaseParameter.ListSearchString.push($("#ComboBox1").val());
    BaseParameter.ListSearchString.push($("#RichTextBox1").val());
    BaseParameter.ListSearchString.push(FILE_DSYN);
    BaseParameter.ListSearchString.push(FILE_NM);
    BaseParameter.ListSearchString.push(FILE_EX);
    BaseParameter.ListSearchString.push(FILE_SIZE);
    BaseParameter.ListSearchString.push(PHOTO_NM);

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/Z07_1/Buttonsave_Click";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {}
    }).then((response) => {
        response.json().then((data) => {
            if (data.Success) {
                $("#Label7").css("background-color", "#f5f5f5");
                $("#Label12").css("background-color", "#f5f5f5");

                alert("정상처리 되었습니다.\nĐã được lưu.");

               
                history.back();
            } else {
                alert(data.Error || "Save failed");
            }

            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}

function Buttondelete_Click() {
    if (!confirm("Are you sure you want to delete this record?")) {
        return;
    }

    if ($("#DEP_INDEX").text() === "New" || $("#DEP_INDEX").text() === "") {
        alert("Cannot delete a new record.");
        return;
    }

    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    BaseParameter.ListSearchString = [];
    BaseParameter.ListSearchString.push($("#DEP_INDEX").text());

    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/Z07_1/Buttondelete_Click";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {}
    }).then((response) => {
        response.json().then((data) => {
            if (data.Success) {
                alert("정상처리 되었습니다.\nĐã được lưu.");
                history.back();
            } else {
                alert(data.Error || "Delete failed");
            }

            $("#BackGround").css("display", "none");
        }).catch((err) => {
            $("#BackGround").css("display", "none");
        })
    });
}

function Buttoncancel_Click() {
    ResetDT();
}

function Buttoninport_Click() {
    $("#BackGround").css("display", "block");
    let BaseParameter = new Object();
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/Z07_1/Buttoninport_Click";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {}
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
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/Z07_1/Buttonexport_Click";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {}
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
    let formUpload = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
    let url = "/Z07_1/Buttonprint_Click";

    fetch(url, {
        method: "POST",
        body: formUpload,
        headers: {}
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


function Button1_Click() {

    try {
        navigator.clipboard.read().then(items => {
            for (let item of items) {
                if (item.types.includes('image/png') ||
                    item.types.includes('image/jpeg') ||
                    item.types.includes('image/gif')) {

                    item.getType(item.types.find(type => type.startsWith('image/'))).then(blob => {
                        const url = URL.createObjectURL(blob);
                        $("#PictureBox1").attr("src", url).show();
                        PHOTO_DSYV = true;
                    });
                    break;
                }
            }
        });
    } catch (err) {
        alert("Cannot access clipboard. Please use the file upload button instead.");
    }
}

function Button2_Click() {

    if ($("#Label7").text() === "-" || $("#Label7").text() === "") {
        return;
    }


    let fileName = $("#Label7").text();
    let ftpServer = getFtpServer();
    let downloadUrl = "/Z07_1/DownloadFile?fileName=" + encodeURIComponent(fileName) + "&ftpServer=" + encodeURIComponent(ftpServer);

   
    window.open(downloadUrl, '_blank');
}

function Button3_Click() {
   
    $("#OpenFileDialog1").click();
}


$("#OpenFileDialog1").change(function (e) {
    if (e.target.files.length === 0) return;

    let file = e.target.files[0];
    let fileName = file.name;
    let fileSize = file.size / 1000000; // Convert to MB
    let fileExt = "." + fileName.split('.').pop();

    $("#Label8").text(fileExt);
    $("#Label7").text(fileName);
    $("#Label12").text(fileSize.toFixed(2));

    if (fileSize >= 30) {
        $("#Label7").css("background-color", "pink");
        $("#Label12").css("background-color", "pink");
        SAVE_BUTCHK = false;
    } else {
        $("#Label7").css("background-color", "greenyellow");
        $("#Label12").css("background-color", "greenyellow");
        SAVE_BUTCHK = true;
    }

   
    if (fileSize > 1) {
        $("#ProgressBar1").show();
       
        setTimeout(() => {
            $("#ProgressBar1").hide();
        }, 2000);
    }
});

function Button4_Click() {
    // Delete File functionality
    $("#Label7").text("-");
    $("#Label8").text("");
    $("#Label12").text("-");
    $("#Label7").css("background-color", "#f5f5f5");
    $("#Label12").css("background-color", "#f5f5f5");
    SAVE_BUTCHK = false;
}

function Button5_Click() {
   
    let url = "/Z07_ADMIN";
    OpenWindowByURL(url, 800, 600);
}


function formatDate(dateString) {
    if (!dateString) return "-";

    let date;
    if (typeof dateString === 'string') {
        date = new Date(dateString);
    } else {
        date = dateString;
    }

    if (isNaN(date.getTime())) return "ERROR";

    return date.getFullYear() + "-" +
        String(date.getMonth() + 1).padStart(2, '0') + "-" +
        String(date.getDate()).padStart(2, '0');
}

function formatDateCode(date) {
    return date.getFullYear() +
        String(date.getMonth() + 1).padStart(2, '0') +
        String(date.getDate()).padStart(2, '0') +
        String(date.getSeconds()).padStart(2, '0');
}

function getScrnPathFromMenu(menuText) {
    if (!menuText) return "001";

   
    let parts = menuText.split('  ');
    if (parts.length > 0) {
        return parts[0];
    }
    return "001";
}

function loadPhoto(photoName) {
    if (!photoName) {
        $("#PictureBox1").attr("src", "").hide();
        return;
    }

    let ftpServer = getFtpServer();
    let photoUrl = "http://" + ftpServer + "/MES_DATE/MES_REQUEST/PHOTO/" + photoName;


    let img = new Image();
    img.onload = function () {
        $("#PictureBox1").attr("src", photoUrl).show();
    };
    img.onerror = function () {
       
        let defaultUrl = "http://" + ftpServer + "/MES_DATE/PHOTO/V00/DJG_IMG_NULL.png";
        $("#PictureBox1").attr("src", defaultUrl).show();
    };
    img.src = photoUrl;
}

function getFtpServer() {
 
    return "mes-ftp-server.domain.com";
}

