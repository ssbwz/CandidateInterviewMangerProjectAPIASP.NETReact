import React, { useState, useEffect } from "react";
import VacancyList from "../components/VacancyList";
import vacancyServer from "../Server/vacancyServer";
// import jessieStyle from "../styles/style.css"
// import appStyle from "../App.css"

function VacancyOverviewPage() {

    const [vacancyItems, setVacancyItems] = useState([]);

    const getAllVacancies = async () => {
        const objectVacancyItems = await vacancyServer.getAllVacancies()
        setVacancyItems(objectVacancyItems.data)
    }

    useEffect(() => {
        getAllVacancies()
    }, []);

    const vacancyItemsData = [
        {
            id: 1,
            title: "HR Teamcoach",
            description: "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer id condimentum ipsum, at lacinia arcu. Praesent egestas neque laoreet eros volutpat imperdiet. Nunc cursus ligula condimentum, vulputate tellus sit amet, tincidunt tortor. Ut erat urna, congue vel rutrum tincidunt, dapibus at enim. Suspendisse imperdiet nulla vitae mi pellentesque, vitae sodales nibh blandit. Pellentesque et orci placerat, finibus eros sed, blandit sem. Donec turpis est, viverra ut viverra vitae, vehicula a orci. Sed et sagittis libero. Praesent tempor elit nec felis consequat, eu iaculis justo bibendum. Duis malesuada nulla a diam fringilla, nec vulputate diam malesuada. Sed efficitur sagittis turpis, vitae bibendum nunc imperdiet id. Pellentesque fermentum ullamcorper purus, vel consectetur dui scelerisque eget.",
            city: "Eindhoven",
            length: "Permanent",
            hours: "20 hours",
            grade: "HBO"
        },
        {
            id: 2,
            title: "Teacher",
            description: "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer id condimentum ipsum, at lacinia arcu. Praesent egestas neque laoreet eros volutpat imperdiet. Nunc cursus ligula condimentum, vulputate tellus sit amet, tincidunt tortor. Ut erat urna, congue vel rutrum tincidunt, dapibus at enim. Suspendisse imperdiet nulla vitae mi pellentesque, vitae sodales nibh blandit. Pellentesque et orci placerat, finibus eros sed, blandit sem. Donec turpis est, viverra ut viverra vitae, vehicula a orci. Sed et sagittis libero. Praesent tempor elit nec felis consequat, eu iaculis justo bibendum. Duis malesuada nulla a diam fringilla, nec vulputate diam malesuada. Sed efficitur sagittis turpis, vitae bibendum nunc imperdiet id. Pellentesque fermentum ullamcorper purus, vel consectetur dui scelerisque eget.",
            city: "Amsterdam",
            length: "Permanent",
            hours: "30 hours",
            grade: "HBO"
        },
        {
            id: 3,
            title: "Recruiter",
            description: "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer id condimentum ipsum, at lacinia arcu. Praesent egestas neque laoreet eros volutpat imperdiet. Nunc cursus ligula condimentum, vulputate tellus sit amet, tincidunt tortor. Ut erat urna, congue vel rutrum tincidunt, dapibus at enim. Suspendisse imperdiet nulla vitae mi pellentesque, vitae sodales nibh blandit. Pellentesque et orci placerat, finibus eros sed, blandit sem. Donec turpis est, viverra ut viverra vitae, vehicula a orci. Sed et sagittis libero. Praesent tempor elit nec felis consequat, eu iaculis justo bibendum. Duis malesuada nulla a diam fringilla, nec vulputate diam malesuada. Sed efficitur sagittis turpis, vitae bibendum nunc imperdiet id. Pellentesque fermentum ullamcorper purus, vel consectetur dui scelerisque eget.",
            city: "Eindhoven",
            length: "Permanent",
            hours: "15 hours",
            grade: "HBO"
        }, {
            id: 4,
            title: "Janitor",
            description: "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer id condimentum ipsum, at lacinia arcu. Praesent egestas neque laoreet eros volutpat imperdiet. Nunc cursus ligula condimentum, vulputate tellus sit amet, tincidunt tortor. Ut erat urna, congue vel rutrum tincidunt, dapibus at enim. Suspendisse imperdiet nulla vitae mi pellentesque, vitae sodales nibh blandit. Pellentesque et orci placerat, finibus eros sed, blandit sem. Donec turpis est, viverra ut viverra vitae, vehicula a orci. Sed et sagittis libero. Praesent tempor elit nec felis consequat, eu iaculis justo bibendum. Duis malesuada nulla a diam fringilla, nec vulputate diam malesuada. Sed efficitur sagittis turpis, vitae bibendum nunc imperdiet id. Pellentesque fermentum ullamcorper purus, vel consectetur dui scelerisque eget.",
            city: "Breda",
            length: "Permanent",
            hours: "40 hours",
            grade: "MBO"
        },
    ]



    return (
                <div className="content">
                    <VacancyList vacancyItems={vacancyItems} />
                </div>
    )
}

export default VacancyOverviewPage;