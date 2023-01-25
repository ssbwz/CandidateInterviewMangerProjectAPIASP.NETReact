import { useLocation, Outlet, Navigate } from "react-router-dom"
import authService from "./Server/authServer";
const PrivateRoute = ({ allowedRoles }) => {
    const location = useLocation();
    const loggedInUser = authService.getCurrentUserRole();
    if(!loggedInUser){
        return <Navigate to="/login" state={{ from: location }} replace />

    }

    if(allowedRoles && !allowedRoles.find(role => role.includes(loggedInUser[0]))){
        return <Navigate to="/unauthorized" state={{ from: location }} replace />
    }
    
    return (
        <Outlet />
    )
}
export default PrivateRoute;