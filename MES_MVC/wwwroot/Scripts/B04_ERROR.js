let BaseResult;
let RowIndex;

$(window).focus(function () {
}).blur(function () {
    localStorage.setItem("B04_ERROR_Close", 1);
    window.close();
});
$(document).ready(function () {
    localStorage.setItem("B04_ERROR_Close", 0);
});

$("#Button1").click(function () {
    Button1_Click();
});
$("#Button2").click(function () {
    Button2_Click();
});
function Button1_Click() {
    localStorage.setItem("B04_ERRORMAS_CHK", "TRUE");
    localStorage.setItem("B04_ERROR_Close", 1);
    window.close();
}
function Button2_Click() {
    localStorage.setItem("B04_ERRORMAS_CHK", "FALSE");
    localStorage.setItem("B04_ERROR_Close", 1);
    window.close();
}
