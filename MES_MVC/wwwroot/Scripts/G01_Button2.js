let BaseResult;
let RowIndex;

$(window).focus(function () {
}).blur(function () {
    localStorage.setItem("G01_Button2_Close", 1);
    window.close();
});
$(document).ready(function () {
    localStorage.setItem("G01_Button2_TextBox1", "0000");
    localStorage.setItem("G01_Button2_Close", 0);
});

$("#Button1").click(function () {
    Button1_Click();
});
$("#Button2").click(function () {
    Button2_Click();
});
function Button1_Click() {
    let TextBox1 = $("#TextBox1").val();    
    localStorage.setItem("G01_Button2_TextBox1", TextBox1);
    localStorage.setItem("G01_Button2_Close", 1);
    window.close();
}
function Button2_Click() {    
    localStorage.setItem("G01_Button2_TextBox1", "");
    localStorage.setItem("G01_Button2_Close", 1);
    window.close();
}
