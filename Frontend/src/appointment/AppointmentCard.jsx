import React, {useEffect} from "react";
import { useNavigate } from "react-router-dom";
import moment from "moment";
import { useState } from "react";
import UserServer from "../Server/UserServer"
function AppointmentCard(props){

    const[candidate, setCandidate] = useState();
    const[recruiter, setRecruiter] = useState();

    const Navigate = useNavigate()

    const getCandidate = () => {
        UserServer.getCandidateByAppointmentId(props.appointCards.applicationId)
            .then(response => {
                setCandidate(response.data);
                console.log(response.data);
            })
            .catch(e => {
                console.log(e);
            });
    };

    const getRecruiter = () => {
        UserServer.getRecruiterByAppointmentId(props.appointCards.applicationId)
            .then(response => {
                setRecruiter(response.data);
                console.log(response.data);
            })
            .catch(e => {
                console.log(e);
            });
    };

    const startDateString = moment(new Date(props.appointCards.startDate + "Z")).format("MMMM Do YYYY, h:mm a"); 
    const endDateString =moment(new Date(props.appointCards.endDate + "Z")).format("h:mm a")
    function moreInfo(event, appointment){
        event.preventDefault();
        console.log("appointment ID: " + appointment)
        Navigate("/singleappointment/" + appointment)
    }


    useEffect(() => {
        getRecruiter();
        getCandidate();
    }, []);

    if(candidate != undefined && recruiter != undefined){
    return(
        <li>

        <div className="appointment-card">
       
        <div className="flexboxBodyAppointment">
            <div className="flexboxBodyAppointmentContent">

                <div className="flextboxStandardInfo">
                    <div className="flexboxStandardInfoContent">
                        <div className="">Starts on: <div className="strong">{startDateString}</div></div>
                        <div className="">Ends at: < div className="strong">{endDateString}</div></div>

                    </div>

                    <div className="flexboxDetailedInfoContent">
                        <div className="">Recruiter name: < div className="strong">{recruiter.firstName} {recruiter.lastName}</div></div>
                        <div className="">Candidate name: < div className="strong">{candidate.firstName} {candidate.lastName}</div></div>

                    </div>
                </div>
            </div>
          
        </div>
        <div className="flexboxUrl">
                    <div className="flexboxUrlContent">
                    Location: {props.appointCards.location}

                    </div>
                </div>

        <div className="appointment-card__cta">
          <span className="btn btn-color-01 appointment-button-goto" onClick={ e=> moreInfo(e, props.appointCards.id)} >View</span>
        </div>
      </div>
        </li>
        
    )
    }
    else{
        return<></>
    }
}
export default AppointmentCard;
