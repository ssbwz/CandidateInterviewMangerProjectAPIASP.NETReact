describe('show candidate', () => {
  it('passes', () => {
    cy.visit('http://localhost:3000/')    
    cy.get('#btnshowcandidates').should('have.class', 'btn btn-color-01 vacancy-button-goto')
    cy.get('#btnshowcandidates').should('be.visible').click({force: true})  
  })
})