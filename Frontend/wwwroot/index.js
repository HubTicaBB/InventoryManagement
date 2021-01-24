const endpoint = 'http://localhost:6101/api/inventory';

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

        let priceCell = row.insertCell(2);
        priceCell.innerHTML = item.unitPrice.toLocaleString('se', { minimumFractionDigits: 2 }) + '&nbsp;&nbsp;  SEK';
        priceCell.className = 'text-right';

        let quantityCelll = row.insertCell(3);
        quantityCelll.innerHTML = item.quantityOnStock;
        quantityCelll.className = 'text-right';

        let totalPriceCell = row.insertCell(4);
        totalPriceCell.innerHTML = calculateTotal(item).toLocaleString('se', { minimumFractionDigits: 2 }) + '&nbsp;&nbsp;  SEK';
        totalPriceCell.className = 'text-right';

        let reorderCell = row.insertCell(5);
        reorderCell.innerHTML = getReorderButton(item);
        reorderCell.className = 'text-center';
    });
}

const calculateTotal = (item) => item.unitPrice * item.quantityOnStock;

const getReorderButton = (item) => `
    <button onclick="setReorderModal(${item.id}, '${item.name}')" class="btn btn-success">
        <i class="fas fa-dolly"></i>&nbsp; Manual Reorder
    </button>
`;

const setReorderModal = (id, itemName) => {
    setModalDisplayTo('block', 'reorder-modal', id);

    var modalTitle = document.getElementById('reorder-modal-title');
    modalTitle.innerHTML = `Reordering ${itemName} (Product ID: ${id})`;
    modalTitle.className = 'text-primary';

    document.getElementById('reorder-modal-body').innerHTML = getManualReorderForm(id);
}

const setBulkOrderModal = () => {
    setModalDisplayTo('block', 'bulk-order-modal');
}

const setModalDisplayTo = (displayValue, id) => {
    document.getElementById(id).style.display = displayValue;
}

const getManualReorderForm = (id) => `
    <form onsubmit="reorder(${id})" >
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

const reorder = async (id) => {
    const body = {
        id: id,
        reorderQuantity: +document.getElementById('quantity').value
    };

    await fetch(endpoint, {
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

const bulkReorder = async () => {
    await fetch(`${endpoint}/bulk`, {
        method: 'PUT'
    })
        .then(response = response.json)
        .then(data => console.log(data))
        .catch(error = console.error(error));

    setModalDisplayTo('none', 'bulk-order-modal');
    getIngredients();
}

getIngredients();