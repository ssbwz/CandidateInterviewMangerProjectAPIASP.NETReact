import http from "./ServerBase";


http.create.baseURL += "/appointments"

const verifyLinkAppointmentLink = (linkValidation) => {
    return http.get(`/verifyLink?usernameHashed=${linkValidation.usernameHashed}&identifierHashed=${linkValidation.identifierHashed}`);
};
const createAppointment = (data) => {
    return http.post("/appointments", data);
};

const getAllAppointments = () =>{
    console.log(http.get("/appointments").request)
    return http.get("/appointments");
}

const getAppointmentsFilterAscending = () =>{
    return http.get("/FilterDateAscending")
}

const getAppointmentsFilterDecending = () =>{
    return http.get("/FilterDateDescending")
}

const getAppointmentsBySubjectName = (name) =>{
    return http.get(`/searchByRecruiterName/${name}`)
}

const getAppointmentCreationInfo = (linkValidation) => {
    return http.get(`/appointmentCreationInfo?usernameHashed=${linkValidation.usernameHashed}&identifierHashed=${linkValidation.identifierHashed}`);
}

const getAvailableTimes = (data) => {
    return http.post("/availableTimes", data);
}

const getAvailableRecruiters = (startDate) => {
    return http.get("/available/" + startDate)
}

const AppointmentService ={
    verifyLinkAppointmentLink,
    createAppointment,
    getAppointmentCreationInfo,
    getAvailableTimes,
    getAllAppointments,
    getAppointmentsFilterAscending,
    getAppointmentsFilterDecending,
    getAppointmentsBySubjectName,
    getAvailableRecruiters
};

export default AppointmentService;