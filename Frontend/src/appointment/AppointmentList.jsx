import React from "react";
import AppointmentCard from "./AppointmentCard";

function AppointmentList(props){
   
    return(
        
        <ul className="lucasUL"> 
            {props.appointCards.map(appointCards =>(
                <AppointmentCard key={appointCards.id} appointCards={appointCards}/>
            ))}
        </ul>
    
    )
}
export default AppointmentList;