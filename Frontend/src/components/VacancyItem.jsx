import React from 'react';
import {
  BrowserRouter as Router,
  Route,
  Routes,
  Link,
} from "react-router-dom";
import './VacancyItem.css'

function VacancyItem(props) {

  return (

    <Link to={`../candidates/${props.vacancyItem.id}`} className="vacancy-card">
        <div className="vacancy-card__jobtitle">{props.vacancyItem.title}</div>
        <div className="vacancy-card__labels">
          <ul className="vacancy-labels">
            <li className="vacancy__label vacancy__label--city">{props.vacancyItem.location}</li>
            <li className="vacancy__label vacancy__label--length">{props.vacancyItem.meetingLocation}</li>
          </ul>
        </div>
        <div className="vacancy-card__cta">
          <span id='btnshowcandidates' className="btn btn-color-01 vacancy-button-goto">Show candidates</span>
        </div>
    </Link>
  )
}

export default VacancyItem;