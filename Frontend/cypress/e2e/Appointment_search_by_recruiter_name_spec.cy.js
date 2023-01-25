describe('Search appointment by recruiter name', () => {
  it('passes', () => {
    cy.visit('http://localhost:3000/')
    cy.get(".layout__navigation").contains("Appointments").click().wait(1000)
    cy.get(".searchtext").type("Niels Van",{force: true})
    cy.get(".btnsearch").click({force:true})

  })
})