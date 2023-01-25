import React, { useState, useEffect } from "react";
import styles from '../appointment/OverviewAppointments.css';
import AppointmentList from "./AppointmentList";
import AppointmentService from "../Server/AppointmentServer";
import authService from "../Server/authServer"
import UserService from "../Server/UserServer";
function OverAppointment() {

    const [appointCards, setAppointmentCards] = useState([]);
    const [filter, setFilter] = useState([]);
    const [searchSubject, setsearchSubject] = useState("");

    const currentuserRole = authService.getCurrentUserRole;


    const handleChangeSearch= e =>{
        e.preventDefault();
        setsearchSubject(e.target.value);
    }

   
    const findBySubject = async () => {
        
        if(searchSubject === ""){
            FilterAscending();
        }
        const objectappointCardsSubject = await AppointmentService.getAppointmentsBySubjectName(searchSubject);
        setAppointmentCards(objectappointCardsSubject.data.appointments);
        console.log(appointCards);  
        
    }

    const retrieveAppointmentCards = async () => {
        
        const objectappointCards = await AppointmentService.getAllAppointments()
        if(currentuserRole === 'Admin'){
            setAppointmentCards(objectappointCards.data.appointments)
        }
        else{
            
        }
        setAppointmentCards(objectappointCards.data.appointments)
        console.log(appointCards)
    }

    const FilterAscending = async () => {
        const objectappointCardsFilterAscending = await AppointmentService.getAppointmentsFilterAscending()
        setAppointmentCards(objectappointCardsFilterAscending.data.appointments);
        console.log(objectappointCardsFilterAscending);  

    }

    const FilterDecending = async () => {
        const objectappointCardsFilterDecending = await AppointmentService.getAppointmentsFilterDecending()
        setAppointmentCards(objectappointCardsFilterDecending.data.appointments)
    }

    const handleChange = (e) => {
        setFilter(e.target.value);
    }


    useEffect(() => {
        switch (filter) {
            case "asc":
                FilterAscending();
                break;
            case "desc":
                FilterDecending();
                break;
            case "def":
                retrieveAppointmentCards();
                break;
            case "subject":
                findBySubject();
            default:
                retrieveAppointmentCards();
        }

    }, [filter])

    return (
        <section className={styles.OverviewAppointment}>
            <div className="flexboxAppointmentFunctions">
                <div className="flexboxAppointmentFunctionsContent">
                    <div className="flexboxFilterby">
                        <p>sort by:</p>
                        <select onChange={handleChange} value={filter} id="selectsort">
                            <optgroup label="Select"  >
                                <option value="def">...</option>
                                <option value="asc">Date Ascending</option>
                                <option value="desc">Date Decending</option>
                            </optgroup>
                        </select>
                    </div>

                    <div className="flexboxSearchBy">   
                        <div className="search">
                            <input type="text" className="searchtext" placeholder="Search by recruiter name..."  onChange={e => handleChangeSearch(e)} name="search" />
                            <button className="btnsearch" type="submit" onClick={findBySubject}>Search</button>
                        </div>


                    </div>
                </div>
            </div>
            <AppointmentList appointCards={appointCards} />
        </section>




    )
}

export default OverAppointment;