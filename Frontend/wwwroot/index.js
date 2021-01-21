﻿const endpoint = 'http://localhost:6101/api/inventory';

const getIngredients = async () => {
    await fetch(endpoint)
        .then(response => {
            if (response.ok) return response.json()
            else console.error(response.status, response.error)
        })
        .then(data => display(data))
        .catch(error => console.error('An error occurred while fetching ingredients', error));
}

const display = (data) => {
    const tbody = document.getElementById('inventory-tbody');

    data.forEach(item => {
        let row = tbody.insertRow();
        row.insertCell(0).innerHTML = item.id;
        row.insertCell(1).innerHTML = item.name;
        row.insertCell(2).innerHTML = item.unitPrice;
        row.insertCell(3).innerHTML = item.quantityOnStock.toFixed();
        row.insertCell(4).innerHTML = calculateTotal(item).toFixed();
        row.insertCell(5).innerHTML = getReorderButtons(item);
    });
}

const calculateTotal = (item) => item.unitPrice * item.quantityOnStock;

const getReorderButtons = (item) => `
    <div class="btn-group btn-group-toggle" data-toggle="buttons">
        <button onclick="setReorderModal(${item.id}, '${item.name}', 'Manual')" class="btn btn-outline-primary">
            <i class="fas fa-dolly"></i>&nbsp; Manual Entry
        </button>
        <button onclick="setReorderModal(${item.id}, '${item.name}', 'Bulk')" class="btn btn-outline-primary">
            <i class="fas fa-dolly-flatbed"></i>&nbsp; Bulk Delivery
        </button>
    </div>
`;

const setReorderModal = (id, itemName, orderType) => {
    setModalDisplayTo('block', 'reorder-modal', id);
    document.getElementById('modal-title').innerHTML = `${orderType} reorder: # ${id} - ${itemName}`;

    document.getElementById('modal-body').innerHTML = (orderType === 'Manual')
        ? getManualReorderForm(id)
        : getBulkReorderForm(id);
}

const setModalDisplayTo = (displayValue, id) => {
    document.getElementById(id).style.display = displayValue;
}

const getManualReorderForm = (id) => `
    <form method="PUT" onsubmit="reorder(${id})" >
        <div class="form-group">
            <div class="input-group mb-3">
                <div class="input-group-prepend">
                    <span class="input-group-text">Enter quantity:</span>
                </div>
                <input id="quantity" type="number" class="form-control" min="1">
                <div class="input-group-append">
                    <span class="input-group-text">units</span>
                </div>
            </div>
        </div>
        <div class="modal-footer">
            <input type="submit" class="btn btn-primary" value="Submit order"></input>
            <button class="btn btn-secondary" onclick="setModalDisplayTo('none', 'reorder-modal')">
                Cancel
            </button>
        </div>
    </form>
`;

const reorder = (id) => {
    const body = {
        id: id,
        reorderQuantity: +document.getElementById('quantity').value
    };

    fetch(endpoint, {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(body)
    })
        .then(response => response.json)
        .then(data => location.href = "/")
        .catch(error => console.error(error));

    setModalDisplayTo('none', 'reorder-modal');
    getIngredients();   
}

const getBulkReorderForm = (id) => `
    bulk form ${id}
`;

getIngredients();