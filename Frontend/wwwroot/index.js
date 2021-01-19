function getIngredients() {
    fetch('http://localhost:6101/api/ingredients)
        .then(response => response.json())
        .then(data => console.log(data))
}

getIngredients();