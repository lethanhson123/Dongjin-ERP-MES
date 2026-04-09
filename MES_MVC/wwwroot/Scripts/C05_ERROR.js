let IsTableSort = false;
let BaseResult;

$(window).focus(function () {
}).blur(function () {
    Buttonclose_Click();
});

$(document).ready(function () {
    localStorage.setItem("C05_ERROR_Close", 0);


    
});
$("#Buttonclose").click(function (e) {
    Buttonclose_Click();
});
function Buttonclose_Click() {
    localStorage.setItem("C05_ERROR_Close", 1);
    window.close();
}
