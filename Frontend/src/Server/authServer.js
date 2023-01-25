const getCurrentUserRole = () => {
    const user = getCurrentUser();
    if (user) {
        return decodeToken(user).role;
    }
    return null;
}

const getCurrentUserId = () => {
    const user = getCurrentUser();
    if(user){
        return decodeToken(user).id;
    }
    return null;
}
const getCurrentUsername = () => {
    const user = getCurrentUser();
    if(user){
        return decodeToken(user).email;
    }
    return null;
}

const decodeToken = (token) => {
    const base64Url = token.split('.')[1];
    const base64 = base64Url.replace('-', '+').replace('_', '/');
    const decode = JSON.parse(window.atob(base64));
    return decode;
}

const getCurrentUser = () => {
    return JSON.parse(localStorage.getItem('tokens'));
}

const authLogin = {
    getCurrentUserRole,
    decodeToken,
    getCurrentUser,
    getCurrentUserId,
    getCurrentUsername
};

export default authLogin;