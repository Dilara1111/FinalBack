$(document).ready(function () {
    $('add-to-card').click(function () {
        let id = $(this).data("id");
        console.log()
    });
    $(".removeBtn").click(
        function () {
            let id = $(this.data("id"));
            $.ajax({
                method: "GET"
                url: "/basket/delete"
                data: {
                    id:id
                },
                success: () => {
                    $('.basketproduct[id=${id}]').remove();
                }
            })
        }
    )
});