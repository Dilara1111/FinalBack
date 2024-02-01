const productImage = document.getElementById('productImage');
const addToCartText = document.getElementById('addToCart');


productImage.addEventListener('mouseout', function() {
    addToCartText.innerText = 'Add to Cart';
});