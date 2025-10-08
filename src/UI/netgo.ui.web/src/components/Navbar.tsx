import { useNavigate } from "react-router-dom";

export default function Navbar(){
    const uid = localStorage.getItem("uid");
    const navigate = useNavigate();
    return (
        <>
            {!uid 
                ? <button onClick={() => {navigate("/login")}}>Login</button> 
                : ""}         
        </>
    )
}