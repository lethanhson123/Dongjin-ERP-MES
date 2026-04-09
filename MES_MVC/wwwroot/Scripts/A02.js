let BaseResult;

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
}
let formUpload = new FormData();
formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
let url = "/A02/Buttonfind_Click";

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
function Buttonadd_Click() {
$("#BackGround").css("display", "block");
let BaseParameter = new Object();
BaseParameter = {
}
let formUpload = new FormData();
formUpload.append('BaseParameter', JSON.stringify(BaseParameter));
let url = "/A02/Buttonadd_Click";

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
let url = "/A02/Buttonsave_Click";

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
let url = "/A02/Buttondelete_Click";

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
let url = "/A02/Buttoncancel_Click";

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
let url = "/A02/Buttoninport_Click";

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
let url = "/A02/Buttonexport_Click";

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
let url = "/A02/Buttonprint_Click";

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


