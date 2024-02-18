$(document).ready(function () {
    $(".btn-message").click(function (e) {
        e.preventDefault();

        let name = $("#name").val();
        let email = $("#email").val();

        let message = $("#message").val();

        if (!name || !email || !message) {
            alert("Please fill in all required fields.");
            return;
        }

        $.ajax({
            url: 'Contact/Add',
            type: 'POST',
            data: {
                name: name,
                email: email,
                messageInfo: message
            },
            success: function (response) {

                location.reload();
                $("#name, #emails,  #message").val('');
            },
            error: function (error) {
                console.error(error);
            }
        });
    })
});

