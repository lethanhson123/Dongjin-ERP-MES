let SETUP_FORMTimer;

$(document).ready(function () {

    // Init modal
    $('.modal').modal();

    let BC_MAST = "DJG00$$YUNZFHN11ZMZNWNHYZDLMWE4M2Y4MGZHYMRKM2Q0YTU2OTK112";
    SettingsMASTER_BC = BC_MAST;
    localStorage.setItem("SettingsMASTER_BC", SettingsMASTER_BC);

    let SettingsMC_NM = localStorage.getItem("SettingsMC_NM");
    $("#MC_Label").val(SettingsMC_NM);
    $("#MC_Label_Form").val(SettingsMC_NM);
});

// CANCEL
$("#Cancel").click(function () {
    $("#UserID").val("");
    $("#Password").val("");
});

// MC LIST
$("#Button2").click(function () {
    Button2_Click();
});

function Button2_Click() {
    localStorage.setItem("SETUP_FORM_Close", 0);
    let url = "/SETUP_FORM";
    OpenWindowByURL(url, 800, 460);
    SETUP_FORMTimerStartInterval();
}

function SETUP_FORMTimerStartInterval() {
    SETUP_FORMTimer = setInterval(function () {
        let SETUP_FORM_Close = localStorage.getItem("SETUP_FORM_Close");

        if (SETUP_FORM_Close == "1") {
            clearInterval(SETUP_FORMTimer);

            localStorage.setItem("SETUP_FORM_Close", 0);

            let SettingsMC_NM = localStorage.getItem("SettingsMC_NM");
            $("#MC_Label").val(SettingsMC_NM);
            $("#MC_Label_Form").val(SettingsMC_NM);
        }
    }, 100);
}

// VALIDATE PASSWORD
function validatePassword(password) {
    const regex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$/;
    return regex.test(password);
}

// CHANGE PASSWORD
function submitChangePassword() {

    let username = document.getElementById("username").value;
    let oldPassword = document.getElementById("oldPassword").value;
    let newPassword = document.getElementById("newPassword").value;
    let confirm = document.getElementById("confirmPassword").value;

    if (!oldPassword) {
        alert(MSG.OLD_REQUIRED);
        return;
    }

    if (!validatePassword(newPassword)) {
        alert(MSG.WEAK_PASSWORD);
        return;
    }

    if (newPassword !== confirm) {
        alert(MSG.NOT_MATCH);
        return;
    }

    fetch('/MAIN_Login/ChangePasswordFirstLogin', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
            username: username,
            oldPassword: oldPassword,
            newPassword: newPassword
        })
    })
        .then(res => res.json())
        .then(data => {
            if (data.success) {
                alert(MSG.SUCCESS);
                window.location.href = "/MAIN";
            } else {
                alert(data.message);
            }
        });
}