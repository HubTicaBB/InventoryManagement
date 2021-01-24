/// <reference types="cypress" />

describe('Order pizza using Pizzeria API', () => {

    let orderId = 0;
    const pizza = 'Margherita';
    const extras = ['Shrimps', 'Mussels', 'Coriander'];

    let cheeseInitialQuantity = 0;
    let shrimpsInitialQuantity = 0;
    let musselsInitialQuantity = 0;
    let corianderInitialQuantity = 0;

    let cheeseFinalQuantity = 0;
    let shrimpsFinalQuantity = 0;
    let musselsFinalQuantity = 0;
    let corianderFinalQuantity = 0;

    it('Open the application', () => {
        cy.visit('http://localhost:49178');
    })

    it('Get initial stock quantities', () => {
        cy.get('#Cheese-quantity').invoke('val').then(cheeseQty => {
            cheeseInitialQuantity = cheeseQty;            
          });
        cy.get('#Shrimps-quantity').invoke('val').then(shrimpsQty => {
            shrimpsInitialQuantity = shrimpsQty;                
        });
        cy.get('#Mussels-quantity').invoke('val').then(musselsQty => {
            musselsInitialQuantity = musselsQty;
        });
        cy.get('#Coriander-quantity').invoke('val').then(corianderQty => {
            corianderInitialQuantity = corianderQty;                
        });
    })
    
    it('Put a pizza on your order', () => {
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

    it('Put extras on your order', () => {
        let index = 0;
        extras.forEach(extra => {
            cy.expect(extra).to.be.eq(extra);
            cy.request('PUT', `http://localhost:6102/api/orders/${orderId}?action=add&productName=${extra}`)
                .then(response => {
                    expect(response).property('status').to.equal(200);
                    expect(response).property('body').to.have.property('id');
                    expect(response.body.id).to.be.eq(orderId);
                    expect(response).property('body').to.have.property('ingredients');
                    expect(response.body.ingredients[index].name).to.equal(extra);
                    index++;
                });
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
        cy.request({
            method: 'PUT', 
            url: `http://localhost:6102/api/orders/${orderId}/status=delivered`,
            failOnStatusCode: false
        })
            .then(response => {
                if (cheeseInitialQuantity >= 1 && shrimpsInitialQuantity >= 1 && musselsInitialQuantity >= 1 && corianderInitialQuantity >= 1) {
                    expect(response).property('status').to.equal(200);
                    expect(response).property('body').to.have.property('id');
                    expect(response.body.id).to.be.eq(orderId);
                    expect(response).property('body').to.have.property('status');
                    expect(response.body.status).to.be.eq(2);
                }
                else {
                    expect(response).property('status').to.equal(400);
                }
            })
    });

    it('Get final stock quantities', () => {
        cy.get('#refresh-btn').click();
        cy.get('#Cheese-quantity').invoke('val').then(cheeseQty => {
            cheeseFinalQuantity = cheeseQty;            
          });
        cy.get('#Shrimps-quantity').invoke('val').then(shrimpsQty => {
            shrimpsFinalQuantity = shrimpsQty;                
        });
        cy.get('#Mussels-quantity').invoke('val').then(musselsQty => {
            musselsFinalQuantity = musselsQty;
        });
        cy.get('#Coriander-quantity').invoke('val').then(corianderQty => {
            corianderFinalQuantity = corianderQty;                
        });
    })

    it('Check if ingredients quantity decreased', () => {        
        if (cheeseInitialQuantity >= 1 && shrimpsInitialQuantity >= 1 && musselsInitialQuantity >= 1 && corianderInitialQuantity >= 1) {
            cy.expect(cheeseFinalQuantity).to.be.lessThan(cheeseInitialQuantity);
            cy.expect(shrimpsFinalQuantity).to.be.lessThan(shrimpsInitialQuantity);
            cy.expect(musselsFinalQuantity).to.be.lessThan(musselsInitialQuantity);
            cy.expect(corianderFinalQuantity).to.be.lessThan(corianderInitialQuantity);
        }
        else {
            cy.expect(cheeseFinalQuantity).to.eq(cheeseInitialQuantity);
            cy.expect(shrimpsFinalQuantity).to.eq(shrimpsInitialQuantity);
            cy.expect(musselsFinalQuantity).to.eq(musselsInitialQuantity);
            cy.expect(corianderFinalQuantity).to.eq(corianderInitialQuantity);
        }       
    });

    it('Restore database', () => {
        if (cheeseInitialQuantity !== cheeseFinalQuantity) {
            cy.get('#Cheese-reorder-btn').click();
            cy.get('#quantity').type(1);
            cy.get('#submit-manual-order-btn').click();
        };
        if (shrimpsFinalQuantity !== shrimpsInitialQuantity) {
            cy.get('#Shrimps-reorder-btn').click();
            cy.get('#quantity').type(1);
            cy.get('#submit-manual-order-btn').click();
        };
        if (musselsFinalQuantity !== musselsInitialQuantity) {
            cy.get('#Mussels-reorder-btn').click();
            cy.get('#quantity').type(1);
            cy.get('#submit-manual-order-btn').click();
        };
        if (corianderFinalQuantity !== corianderInitialQuantity) {
            cy.get('#Coriander-reorder-btn').click();
            cy.get('#quantity').type(1);
            cy.get('#submit-manual-order-btn').click();
        }
    })
})