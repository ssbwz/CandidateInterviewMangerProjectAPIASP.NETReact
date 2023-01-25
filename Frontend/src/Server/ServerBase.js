import axios from "axios";
const header = () => {
  if (JSON.parse(localStorage.getItem("tokens")) !== null) {
      return {
          headers: {
            'Accept': 'application/json',
              "Content-Type": "application/json",
              "Authorization": "Bearer " + JSON.parse(localStorage.getItem("tokens"))
          }
      }
  }
  else
      return {
        headers: {
            "Content-Type": "application/json"
        }
      }
}

export default axios.create({
  baseURL: "https://localhost:7111/",
  ...header()
}
);