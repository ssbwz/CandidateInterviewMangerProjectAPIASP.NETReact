import http from "./ServerBase";


const getAllApplications = (vacancyId) => {
    return http.get(`/SingleVacancy?vacancyId=${vacancyId}`);
};

const CandidateServer ={
    getAllApplications,

};

export default CandidateServer;