/// <reference types="cypress" />

describe('Order pizza using Pizzeria API', () => {

    let orderId = 0;
    const pizza = 'Margherita';
    let extras = ['Shrimps', 'Shrimps', 'Mussels', 'Coriander'];

    it('Open the application', () => {
        cy.visit('http://localhost:49178');
    })
    
    it('Put a pizza to your order', () => {
        cy.request('POST', `http://localhost:6102/api/orders?productName=${pizza}`)
            .then(response => {
                expect(response).property('status').to.equal(200);
                expect(response).property('body').to.have.property('id');
                orderId = response.body.id;
                expect(response).property('body').to.have.property('pizzas');
                expect(response.body.pizzas[0]).to.have.property('name');
                expect(response.body.pizzas[0].name).to.be.eq(pizza);
            })
    });

    it('Put extras to your order', () => {
        let index = 0;
        extras.forEach(extra => {
            cy.request('PUT', `http://localhost:6102/api/orders/${orderId}?action=add&productName=${extra}`)
                .then(response => {
                    expect(response).property('status').to.equal(200);
                    expect(response).property('body').to.have.property('id');
                    expect(response.body.id).to.be.eq(orderId);
                    expect(response).property('body').to.have.property('ingredients');
                    expect(response.body.ingredients[index].name).to.equal(extra);
                    index++;
                })
        })
    });

    it('Submit order', () => {
        cy.request('PUT', `http://localhost:6102/api/orders/${orderId}/submit`)
        .then(response => {
            expect(response).property('status').to.equal(200);
            expect(response).property('body').to.have.property('id');
            expect(response.body.id).to.be.eq(orderId);
            expect(response).property('body').to.have.property('status');
            expect(response.body.status).to.be.eq(1);
        })
    });

    it('Use ingredients and mark as delivered', () => {
        cy.request('PUT', `http://localhost:6102/api/orders/${orderId}/status=delivered`)
        .then(response => {
            expect(response).property('status').to.equal(200);
            expect(response).property('body').to.have.property('id');
            expect(response.body.id).to.be.eq(orderId);
            expect(response).property('body').to.have.property('status');
            expect(response.body.status).to.be.eq(2);
        })
    });
})