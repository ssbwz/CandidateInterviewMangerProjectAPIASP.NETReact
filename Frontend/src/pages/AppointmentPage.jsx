import React, { useEffect, useState } from "react"
import { useParams } from "react-router-dom";
import { useNavigate } from "react-router-dom";
import style from "../components/Style1.css"
import http from "../Server/http-common"
import moment from "moment";
import AppointmentServer from "../Server/AppointmentServer";


function AppointmentPage() {

    const [appointment, setAppointment] = useState();
    const [currentAppointment, setCurrentAppointment] = useState();
    const { appointmentId } = useParams()
    const [reload, setReload] = useState();
    const [availableRecruiters, setAvailableRecruiters] = useState([]);
    const [currentUser, setCurrentUser] = useState();

    const Navigate = useNavigate()

    const getAppointmentById = async (id) => {
        await http.get(`appointments/${id}`)
            .then((response) => {
                //Changing the time local time
                response.data.startDate = new Date(response.data.startDate + "Z");
                response.data.endDate = new Date(response.data.endDate + "Z");
              setAppointment(response.data)
            })
            .catch((error) => {
                console.error(error);
            });
    };



    function removeAppointmentById(event) {
        event.preventDefault();
        console.log(appointmentId);
        http.delete(`appointments/${appointmentId}`).then(() => {
            Navigate("/appointments")
        });
    };

    function updateAppointment(newEmail, event) {
        event.preventDefault();
        let data = {
            "Id": appointment.id,
            "RecruiterEmail": newEmail.toString()
        }
        http.put("appointments", data).then((response) => {
            setReload(response);
            retrieveAvailableRecruiters()
        })
        .catch((error) => {
            console.error(error);
        });
    }

    useEffect(() => {
        getAppointmentById(appointmentId);
    }, []);

    useEffect(() => {
        getAppointmentById(appointmentId).then(() => {
            closeForm();

        });
    }, [reload]);

    useEffect(() => {
        retrieveAvailableRecruiters()
    }, [appointment])

    const retrieveAvailableRecruiters = async () => {
        const recruiters = await AppointmentServer.getAvailableRecruiters(new Date(appointment.startDate).toISOString())
        setAvailableRecruiters(recruiters.data)
    }

    function openForm() {
        document.getElementById("myForm").classList.add("form-popup-opened")
    }

    function closeForm() {
        document.getElementById("myForm").classList.remove("form-popup-opened")
    }

    if (appointment == undefined) {
        return (<p>loading...</p>)
    }
    else
        return (
            <section className={style.Style1}>
                <div className="containerTom">
                    <div className="containerTom">
                        <h1>Appointment information</h1>
                        <table className="tableTom">
                            <tbody>
                                <tr>
                                    <td className="data1">Recruiter's name</td>
                                    <td className="data2">{appointment.recruiterFirstName} {appointment.recruiterLastName}</td>
                                </tr>
                                <tr>
                                    <td className="data1">Candidate's name</td>
                                    <td className="data2">{appointment.candidateFirstName} {appointment.candidateLastName}</td>
                                </tr>
                                <tr>
                                    <td className="data1">Starts at</td>
                                    <td className="data2">{moment(appointment.startDate).format("MMMM Do YYYY, h:mm a")}</td>
                                </tr>
                                <tr>
                                    <td className="data1">Ends at</td>
                                    <td className="data2">{moment(appointment.endDate).format("h:mm a")}</td>
                                </tr>
                                <tr>
                                    <td className="data1">Meeting will be held at</td>
                                    <td className="data2">{appointment.location}</td>
                                </tr>
                            </tbody>
                        </table>


                        <div className="flex-containerTom">
                            <button className="btnTom" onClick={e => removeAppointmentById(e)}>Cancel the appointment</button>
                            <button className="btnTom" onClick={openForm}> Change the recruiter </button>
                        </div>
                    </div>
                    <div className="form-popup" id="myForm">
                        <form action="/action_page.php" className="form-container">
                            <select id="email" name="email">
                                <optgroup label="Select">
                                    {availableRecruiters.map(availableRecruiters => {
                                        return (
                                            <option value={availableRecruiters.email} id={availableRecruiters.id}>
                                                {availableRecruiters.email}
                                            </option>
                                        )
                                    })}
                                </optgroup>
                            </select>

                            <button type="submit" className="btn" onClick={e => updateAppointment(document.getElementById("email").value, e)}>Assign</button>
                            <button type="button" className="btn cancel" onClick={closeForm}>Close</button>
                        </form>
                    </div>

                </div>
            </section>
        );
}

export default AppointmentPage

