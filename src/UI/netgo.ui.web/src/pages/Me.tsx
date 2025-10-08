import { useEffect, useState } from "react"
import type { User } from "../types/dtos";
import { useNavigate } from "react-router-dom";
import { getUserById } from "../services/UserService";

export default function MePage() {
    const navigate = useNavigate();
    const uid = localStorage.getItem("uid")
    const [user, setUser] = useState<User>();
    
    useEffect(() => {
        if (!uid) {
            navigate("/login");
            return;
        }

        const fetchUser = async () => {
            const response = await getUserById(uid);
            if (!response) {
                navigate("/login");
                return;
            }
            setUser(response);
        };

        fetchUser();
    }, [uid, navigate]); 

    return (
     <>
        {user?.email} <br />
        {user?.firstName} <br />
        {user?.lastName} <br />
     </> 
    )
}