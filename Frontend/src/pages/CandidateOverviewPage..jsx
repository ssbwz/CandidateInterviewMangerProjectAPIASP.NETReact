import React, { useState, useEffect } from "react";
import CandidateList from "../components/CandidateList";
import CandidateServer from "../Server/CandidateServer";
import { useParams } from 'react-router-dom'
import vacancyServer from "../Server/vacancyServer";

function CandidateOverviewPage() {

    const { vacancyId } = useParams();

    const [applicationItems, setApplicationItems] = useState([]);

    const getAllApplications = async () => {
        const objectCandidateItems = await CandidateServer.getAllApplications(vacancyId)
        setApplicationItems(objectCandidateItems.data)
    }

    const [vacancy, setVacancy] = useState();

    const getVacancyById = async () =>{
        vacancyServer.getVacancyById(vacancyId).then(response => {
            setVacancy(response.data)
        })
    }

    useEffect(() => {
        getAllApplications()
        getVacancyById()
    }, []);


    if(vacancy == undefined){
        return<>    </>
    }
    if(applicationItems == null){
        return(
            <div className="content">
                <h1>No assigned candidates!</h1>
            </div>
        )
    }
    return (
        <div className="content">
            <a className="backbtn" href="/">Back</a>
            <h1>{vacancy.title}</h1>
            <CandidateList applicationItems={applicationItems} />
        </div>
    )
}

export default CandidateOverviewPage;