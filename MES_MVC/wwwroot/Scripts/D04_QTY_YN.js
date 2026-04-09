let IsTableSort = false;
let BaseResult;
let RowIndex;

$(window).focus(function () {
}).blur(function () {
    Button2_Click();
});
$(document).ready(function () {    
    localStorage.setItem("D04_QTY_YN_Close", 0);    
    localStorage.setItem("D04_PO_QTY_CHK", false);

    $("#Button1").focus();
});
$("#Button1").click(function () {
    Button1_Click();
});
function Button1_Click() {    
    localStorage.setItem("D04_PO_QTY_CHK", true);
    localStorage.setItem("D04_QTY_YN_Close", 1);
    window.close();
}
$("#Button2").click(function () {
    Button2_Click();
});
function Button2_Click() {    
    localStorage.setItem("D04_PO_QTY_CHK", false);
    localStorage.setItem("D04_QTY_YN_Close", 1);
    window.close();
}

