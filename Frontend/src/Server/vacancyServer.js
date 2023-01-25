import http from "./ServerBase";


const getAllVacancies = () => {
    return http.get("/vacancies");
};

const getVacancyById = (vacancyId) => {
    return http.get(`/vacancy?vacancyId=${vacancyId}`);
};

const vacancyServer = {
    getAllVacancies,
    getVacancyById
};

export default vacancyServer;