import React, { useState } from "react";
import "../styles/Login.css";
import { useAuth } from "../auth/Auth";
import LoginService from "../Server/LoginServer"
const LoginPage = () => {

    const [errorMessage, showErrorMessage] = useState("hide")
    const error = () => {
        showErrorMessage("error-popup")
    }


    const [isLoggedIn, setLoggedIn] = useState(false);
    const { setAuthTokens } = useAuth();


    const initialLoginState = {
        email: "",
        password: ""
    };
    const [login, setLogin] = useState(initialLoginState);

    const handleInputChange = event => {
        const { name, value } = event.target;
        setLogin({ ...login, [name]: value });
    };

    const saveLogin = () => {
        var data = {
            email: login.email,
            password: login.password,
        };

        LoginService.create(data)
            .then(response => {
                if (response.status === 200) {

                    setAuthTokens(response.data);
                    setLoggedIn(true);
                    console.log(response.data);
                }
                else {
                }
            })
            .catch(e => {
                console.log(e);
            });
    };

    if (isLoggedIn) {
        return window.location.href = '/';
    }

    return (
        <div className="loginPage">
            <div className="login-container">
                <div className="cover">
                    <div className={errorMessage}>
                        <h3>! Invalid credentials !</h3>
                    </div>
                    <h1>Log in</h1>
                    <input type="text" id="username" value={login.email} onChange={handleInputChange} name="email" placeholder="Email" required />
                    <input type="password" placeholder="Password" id="password" value={login.password} onChange={handleInputChange} name="password" required />
                    <div className="login-btn" type="submit" onClick={() => { saveLogin(); }}>Login</div>
                </div>
            </div>
        </div>
    )
}

export default LoginPage