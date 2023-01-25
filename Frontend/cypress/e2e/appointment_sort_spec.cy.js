describe('template spec', () => {
  it('passes', () => {
    cy.visit('http://localhost:3000/')
    cy.get(".layout__navigation").contains("Appointments").click().wait(1000)
    cy.get('#selectsort').select('asc',{force: true})
    cy.get('#selectsort').should('have.value','asc',{force: true})

  })
})