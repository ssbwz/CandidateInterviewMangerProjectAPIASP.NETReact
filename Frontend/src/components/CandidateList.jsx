import React, { useState, useEffect } from "react";
import CandidateItem from "./CandidateItem";
import Pagination from "./Pagination"
import "./CandidateList.css";

function CandidateList(props) {

  // User is currently on this page
  const [currentPage, setCurrentPage] = useState(1);
  // No of Records to be displayed on each page   
  const [recordsPerPage] = useState(10);

  const indexOfLastRecord = currentPage * recordsPerPage;
  const indexOfFirstRecord = indexOfLastRecord - recordsPerPage;

  // Records to be displayed on the current page
  const currentRecords = props.applicationItems.slice(indexOfFirstRecord,
    indexOfLastRecord);

  const nPages = Math.ceil(props.applicationItems.length / recordsPerPage)

  return (
    <div className="candidates_list">
      <ul className="columns">
      {currentRecords.map(applicationItem => (
        <CandidateItem key={applicationItem.id} applicationItem={applicationItem} />
      ))}
      </ul>
      <br/>
      <Pagination
        nPages={nPages}
        currentPage={currentPage}
        setCurrentPage={setCurrentPage}
      />
    </div>
  )
}


export default CandidateList;