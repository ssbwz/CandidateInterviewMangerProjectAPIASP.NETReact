describe('Show appointment', () => {
  it('passes', () => {
cy.visit('http://localhost:3000/')
cy.get(".layout__navigation").contains("Appointments").click().wait(1000)
cy.get(".appointment-button-goto").contains("View").click({force: true})

  })
})