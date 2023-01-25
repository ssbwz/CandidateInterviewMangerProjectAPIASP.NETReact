import React, { useState } from "react";
import VacancyItem from "./VacancyItem";
import Pagination from "./Pagination"
import "./VacancyList.css";

function VacancyList(props) {

    // User is currently on this page
    const [currentPage, setCurrentPage] = useState(1);
    // No of Records to be displayed on each page   
    const [recordsPerPage] = useState(10);

    const indexOfLastRecord = currentPage * recordsPerPage;
    const indexOfFirstRecord = indexOfLastRecord - recordsPerPage;

    // Records to be displayed on the current page
    const currentRecords = props.vacancyItems.slice(indexOfFirstRecord,
        indexOfLastRecord);

    const nPages = Math.ceil(props.vacancyItems.length / recordsPerPage)

  return (
    <div className="vacancies_list">
      {currentRecords.map(vacancyItem => (
        <VacancyItem key={vacancyItem.id} vacancyItem={vacancyItem} />
      ))}
      <Pagination
                nPages={nPages}
                currentPage={currentPage}
                setCurrentPage={setCurrentPage}
            />
    </div>
  )
}

export default VacancyList;