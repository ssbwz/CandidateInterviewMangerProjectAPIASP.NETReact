import http from "./ServerBase";

const create = (data) => {
    return http.post("/login", data);
};

const LoginServer ={
    create,
};

export default LoginServer;