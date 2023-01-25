import React from 'react';
import './CandidateItem.css'

function CandidateItem(props) {
  console.log(props.applicationItem)

  return (
    <div className="candidate-card">
      <div className="candidate-card__jobtitle">{props.applicationItem.candidate.firstName} {props.applicationItem.candidate.lastName}</div>
      <div className="candidate-card__labels">
        <ul className="candidate-labels">
          <li className="candidate__label candidate__label--length">{props.applicationItem.candidate.email}</li>
        </ul>
      </div>
    </div>
  )
}

export default CandidateItem;