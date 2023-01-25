import http from "./ServerBase";

const getRecruiterByAppointmentId = id =>{
    return http.get(`/recruiter/${id}`)
}

const getCandidateByAppointmentId = id =>{
    return http.get(`/candidate/${id}`)
}

const UserService ={
    getRecruiterByAppointmentId,
    getCandidateByAppointmentId,
    
};

export default UserService;