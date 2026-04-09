let BaseResult;
let RowIndex;

$(window).focus(function () {
}).blur(function () {
    window.close();
});
$(document).ready(function () {
});

$("#Button1").click(function () {
    Button1_Click();
});
$("#Button2").click(function () {
    Button2_Click();
});
function Button1_Click() {
    localStorage.setItem("B03AddTextBox1", $("#TextBox1").val());
    Button2_Click();
}
function Button2_Click() {
    localStorage.setItem("B03AddTextBox1", "");
    window.close();
}
