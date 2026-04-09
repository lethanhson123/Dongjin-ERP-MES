$(document).ready(function () {
    // Initialize Materialize components
    M.FormSelect.init(document.querySelectorAll('select'));
    M.updateTextFields();

    // Form submit handler
    $("#SelfRegistrationForm").on("submit", function (e) {
        e.preventDefault();

        // Validate form
        if (!validateForm()) {
            return;
        }

        // Show loader
        $("#BackGround").css("display", "block");

        // Prepare data
        let BaseParameter = {
            SearchString: $("#RegistrationToken").val(), // Token
            TextBox1: $("#TextBox_Name").val(),
            ComboBox1: $("#ComboBox_Gender").val(),
            DateTimePicker1: $("#DatePicker_DOB").val(),
            ComboBox2: $("#ComboBox_MaritalStatus").val(),
            TextBox3: $("#TextBox_Dependents").val(),
            TextBox2: $("#TextBox_CitizenID").val(),
            DateTimePicker2: $("#DatePicker_IDIssueDate").val(),
            TextBox4: $("#TextBox_IDIssuePlace").val(),
            TextBox5: $("#TextBox_PermAddress").val(),
            TextBox6: $("#TextBox_CurrAddress").val(),
            TextBox7: $("#TextBox_Phone").val(),
            TextBox8: $("#TextBox_Email").val()
        };

        let formUpload = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(BaseParameter));

        // Submit to server
        fetch("/HR2/SubmitSelfRegistration", {
            method: "POST",
            body: formUpload
        })
            .then(response => response.json())
            .then(data => {
                $("#BackGround").css("display", "none");

                if (data.Success) {
                    // Success message
                    M.toast({
                        html: '<i class="material-icons">check_circle</i> ' + data.Message,
                        classes: 'green',
                        displayLength: 5000
                    });

                    // Disable form
                    $("#SelfRegistrationForm :input").prop("disabled", true);

                    // Show completion message
                    setTimeout(() => {
                        $(".card-content").html(`
                        <div class="center">
                            <i class="material-icons green-text" style="font-size: 100px;">check_circle</i>
                            <h5>Đăng Ký Thành Công!</h5>
                            <p>Thông tin của bạn đã được lưu vào hệ thống.</p>
                            <p>Vui lòng liên hệ phòng Nhân sự để biết thêm chi tiết.</p>
                        </div>
                    `);
                    }, 1000);
                } else {
                    // Error message
                    M.toast({
                        html: '<i class="material-icons">error</i> ' + data.Error,
                        classes: 'red',
                        displayLength: 4000
                    });
                }
            })
            .catch(err => {
                $("#BackGround").css("display", "none");
                M.toast({
                    html: '<i class="material-icons">error</i> Đã xảy ra lỗi: ' + err,
                    classes: 'red'
                });
            });
    });
});

// Validate form function
function validateForm() {
    let isValid = true;

    // Required fields
    let requiredFields = [
        { field: "#TextBox_Name", message: "Vui lòng nhập họ tên" },
        { field: "#ComboBox_Gender", message: "Vui lòng chọn giới tính" },
        { field: "#DatePicker_DOB", message: "Vui lòng nhập ngày sinh" },
        { field: "#TextBox_CitizenID", message: "Vui lòng nhập số CCCD" },
        { field: "#DatePicker_IDIssueDate", message: "Vui lòng nhập ngày cấp CCCD" },
        { field: "#TextBox_IDIssuePlace", message: "Vui lòng nhập nơi cấp CCCD" },
        { field: "#TextBox_PermAddress", message: "Vui lòng nhập địa chỉ thường trú" },
        { field: "#TextBox_CurrAddress", message: "Vui lòng nhập địa chỉ hiện tại" },
        { field: "#TextBox_Phone", message: "Vui lòng nhập số điện thoại" }
    ];

    for (let item of requiredFields) {
        let value = $(item.field).val();
        if (!value || value.trim() === "") {
            M.toast({
                html: '<i class="material-icons">warning</i> ' + item.message,
                classes: 'orange'
            });
            $(item.field).focus();
            isValid = false;
            break;
        }
    }

    // Validate CCCD (12 digits)
    if (isValid) {
        let cccd = $("#TextBox_CitizenID").val();
        if (!/^\d{12}$/.test(cccd)) {
            M.toast({
                html: '<i class="material-icons">warning</i> Số CCCD phải có đúng 12 chữ số',
                classes: 'orange'
            });
            $("#TextBox_CitizenID").focus();
            isValid = false;
        }
    }

    // Validate phone (10-11 digits)
    if (isValid) {
        let phone = $("#TextBox_Phone").val();
        if (!/^\d{10,11}$/.test(phone)) {
            M.toast({
                html: '<i class="material-icons">warning</i> Số điện thoại phải có 10-11 chữ số',
                classes: 'orange'
            });
            $("#TextBox_Phone").focus();
            isValid = false;
        }
    }

    // Validate email if provided
    if (isValid) {
        let email = $("#TextBox_Email").val();
        if (email && !/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email)) {
            M.toast({
                html: '<i class="material-icons">warning</i> Email không đúng định dạng',
                classes: 'orange'
            });
            $("#TextBox_Email").focus();
            isValid = false;
        }
    }

    // Validate age (must be 18+)
    if (isValid) {
        let dob = new Date($("#DatePicker_DOB").val());
        let age = new Date().getFullYear() - dob.getFullYear();
        if (age < 18) {
            M.toast({
                html: '<i class="material-icons">warning</i> Bạn phải đủ 18 tuổi',
                classes: 'orange'
            });
            $("#DatePicker_DOB").focus();
            isValid = false;
        }
    }

    return isValid;
}