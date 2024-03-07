$(document).ready(function () {
    $('.add-to-cart').click(function () {
    let id = $(this).data("id");
    console.log(id)
});

    $('.add-to-cart').click(function () {
        let id = $(this).data("id");
        let a = $("#item-count");
        console.log(a)
        let quantity = $(this).data('quantity');
        let price = $(this).data('price');
        console.log(`id:${id}` + `quantity:${quantity}` + `price:${price}`)
        $.ajax({
            method: "POST",
            url: "basket/add",
            data: {
                id: id,
                quantity: quantity
            },
            success: () => {
                let count = a.text();
                count++;
                a.text("");
                a.text(`${count}`)
                console.log("OK")
            }
        })
    })

    $(".removebtn").click(function () {
    let id = $(this).data("id");
    $.ajax({
        method: "GET",
        url: "/basket/delete",
        data: {
            id: id,
        },
        success: () => {
            $(`.basketProducts[id=${id}]`).remove();
        }
    })
}
)
});
