let IsTableSort = false;
let BaseResult;
let RowIndex;

$(window).focus(function () {
}).blur(function () {
    localStorage.setItem("D04_LOC_YN_Close", 1);
    window.close();
});
$(document).ready(function () {
    localStorage.setItem("D04_LOC_YN_Close", 0);
    localStorage.setItem("D04_PO_LOC_CHK", false);
    $("#Label3").val(localStorage.getItem("D04_LOC_YN_Label3"));
});
$("#Button1").click(function () {
    Button1_Click();
});
function Button1_Click() {
    localStorage.setItem("D04_PO_LOC_CHK", true);
    localStorage.setItem("D04_LOC_YN_Close", 1);
    window.close();
}
$("#Button2").click(function () {
    Button2_Click();
});
function Button2_Click() {
    localStorage.setItem("D04_PO_LOC_CHK", false);
    localStorage.setItem("D04_LOC_YN_Close", 1);
    window.close();
}

