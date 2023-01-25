import "../styles/CreateAppointment.css";

import Input from "../components/fields/Input";
import Select from "../components/fields/Select";
import React, { useState, useEffect } from "react"
import { useParams } from "react-router-dom";
import AppointmentServer from "../Server/AppointmentServer.js";
import DatePicker from "react-datepicker";
import "react-datepicker/dist/react-datepicker.css";
import { useNavigate } from "react-router-dom";

function CreateAppointment() {

    const [appointment, setAppointment] = useState({
        date: '',
        time: '',
        location: 'placeholder location',
        duration: 30,
        recruiterEmail: 'placeholder email',
        recruiterName: 'placeholder name',
    });

    const [availableTimes, setAvailableTimes] = useState([]);

    const handleChange = (e) => {
        setAppointment({
            ...appointment,
            [e.target.name]: e.target.value
        });
        console.log(appointment);
    }

    // usernameHashed = email hashed and identifierHashed is vacancy id hashed
    const { usernameHashed, identifierHashed } = useParams();
    const [verfiyLinkStatus, setVerfiyLinkStatus] = useState()

    const [appointmentRequest, setAppointmentRequest] = useState()
    const Navigate = useNavigate()

    const verifyAccess = async () => {
        try {
            const verifyAccessResponse = await AppointmentServer.verifyLinkAppointmentLink({ usernameHashed, identifierHashed })
            setVerfiyLinkStatus(verifyAccessResponse.data.linkStatus)
        }
        catch (err) {
            console.error(err)
            //ToDo:Navigate to error page
        }
    }

    const getAppointmentCreationInfo = () => {
        AppointmentServer.getAppointmentCreationInfo({ usernameHashed, identifierHashed })
            .then((response) => {
                setAppointment({
                    ...appointment,
                    recruiterEmail: response.data.recruiterEmail,
                    recruiterName: response.data.recruiterName,
                    location: response.data.location,
                })
            })
            .catch((err) => {
                console.error(err)
            })
    }

    useEffect(() => {
        verifyAccess()
    }, []);

    useEffect(() => {
        if (verfiyLinkStatus === "Created") {
            getAppointmentCreationInfo()
        }
    }, [verfiyLinkStatus]);

    const currentUrl = window.location.href;

    const [errorMessages, setErrorMessages] = useState('')

    const handleSubmit = async (e) => {
        e.preventDefault();
        if (appointment.date === "" || appointment.date === undefined) {
            setErrorMessages("Please select a date.")
            return;
        }
        if (appointment.time === "No times available" || appointment.time === undefined) {
            setErrorMessages("Please choose a different date as there are no times available.")
            return;
        }
        let startDate = new Date(appointment.date);
        startDate.setHours(appointment.time.split(':')[0]);
        startDate.setMinutes(appointment.time.split(':')[1]);
        startDate.setSeconds(appointment.time.split(':')[2]);
        console.log(startDate);
        let endDate = new Date(startDate.getTime() + appointment.duration * 60000);
        console.log(endDate);
        setAppointmentRequest({
            "CandidateEmailHashed": usernameHashed,
            "VacancyIdHashed": identifierHashed,
            "RecruiterEmail": appointment.recruiterEmail,
            "StartDate": startDate,
            "EndDate": endDate,
            "Location": appointment.location
        });
        //try {
        //    const verifyAccessResponse = await AppointmentServer.createAppointment(appointmentRequest)
        //    console.log(verifyAccessResponse);
        //}
        //catch (err) {
        //    //ToDo:Navigate to error page
        //}
    }

    useEffect(() => {
        AppointmentServer.getAvailableTimes({"Date": appointment.date, "RecruiterEmail": appointment.recruiterEmail})
            .then((response) => {
                setAvailableTimes(response.data)
            })
            .catch((err) => {
                console.error(err)
                setAvailableTimes([{"label": "No times available", "value": "No times available"}])
            })
    }, [appointment.date]);

    useEffect(() => {
        //console.log(appointmentRequest);
        AppointmentServer.createAppointment(appointmentRequest)
            .then((response) => {
                console.log(response);
                Navigate("/login")
            })
            .catch((error) => {
                console.error(error);
            })
    }, [appointmentRequest]);
    //#region viewTypes
    const creatView =
        <div id="block-drigro-content" className="block block-main page content block-system block-system-main-block">
            <div className="block-inner">
                <div role="article" typeof="schema:WebPage" className="paragraph-large">
                    <div>
                        <div className="mt-0 mb-6 paragraph paragraph--type--block paragraph--1021">
                            <div className="container">
                                <div className="row">
                                    <div className="col col-lg-8 body">
                                        <div className="block block-mijn driessen registratie block-custom-vacancy block-custom-vacancy-mijn-driessen-registration">
                                            <div className="block-inner">
                                                <div className="mijn-driessen-iframeregistration">
                                                    <form className="form-register form-register-page-1 form-view-embed">
                                                        <h2>Appointment Info</h2>
                                                        <fieldset>
                                                        {
                                                            errorMessages !== '' ? <div className="alert alert-danger" role="alert">{errorMessages}</div> : null
                                                        }
                                                            <div className="row">
                                                                <div className="col-sm-8">
                                                                    <div className="required form-group">
                                                                        <label>Date</label>
                                                                        <DatePicker
                                                                            className="form-control text-box single-line"
                                                                            closeOnScroll={(e) => e.target === document}
                                                                            selected={appointment.date}
                                                                            onChange={date => setAppointment({ ...appointment, date: date })}
                                                                            minDate={new Date()}
                                                                            placeholderText="Select a date"
                                                                            todayButton="Today"
                                                                            dateFormat="dd/MM/yyyy"
                                                                            /*showTimeSelect
                                                                            timeFormat="HH:mm"
                                                                            timeIntervals={30}
                                                                            timeCaption="time"
                                                                            dateFormat="MMMM d, yyyy h:mm aa"*/
                                                                        />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div className="row">
                                                                <div className="col-sm-6">
                                                                    <Select label="Start Time" name="time" value={appointment.time} onChange={handleChange} options={availableTimes} />
                                                                </div>
                                                                <div className="col-sm-6">
                                                                    <Input name="duration" label="Duration" value={appointment.duration + " minutes"} type="text" readonly={true} />
                                                                </div>
                                                            </div>
                                                            <div className="row">
                                                                <div className="col-sm-6">
                                                                    <Input label="Recruiter name" value={appointment.recruiterName} type="text" readonly={true} />
                                                                </div>
                                                                <div className="col-sm-6">
                                                                    <Input label="Recruiter email" value={appointment.recruiterEmail} type="text" readonly={true} />
                                                                </div>
                                                            </div>
                                                        </fieldset>
                                                        <h2>Location</h2>
                                                        <fieldset>
                                                            <div className="row">
                                                                <Input label="Location" name="location" value={appointment.location} type="text" readonly={true} />
                                                            </div>
                                                        </fieldset>
                                                        <div className="form-actions form-group mt-5">
                                                            <button style={{backgroundColor: "#28a745"}} type="submit" onClick={handleSubmit} className="btn btn-success">Submit</button>
                                                        </div>
                                                    </form>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

    const loadingView = <div class="d-flex align-items-center center">
    <div class="spinner-border ml-auto" role="status" aria-hidden="true">Loading...</div>
  </div>
    //#endregion

    switch (verfiyLinkStatus) {
        case "Doesn't exist":
            //ToDo: Add some style
            return (<div>This link doesn't exist</div>)
        case "Created":
            return (creatView)
        case "Used":
            //ToDo: Add some style
            return (<div>This link got used</div>)
        default:
            return (loadingView)
    }
}
export default CreateAppointment;