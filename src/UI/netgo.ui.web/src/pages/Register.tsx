import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import type { RegistrationRequest } from "../types/dtos";
import { register } from "../services/authService";
import { AxiosError } from "axios";
import iller from "../assets/turkiye.json";
import ErrorMessages from "../components/ErrorMessages";

export default function RegisterPage() {
    const navigate = useNavigate();
    const [form, setForm] = useState<RegistrationRequest>({
        email: "",
        password: "",
        contactInfo: "",
        location: "",
        firstName: "",
        lastName: "",
        profilePicture: undefined
    });

    const [loading, setLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);
    const [cities, setCities] = useState<{ [key: string]: string }>({});
    const [selected, setSelected] = useState("");

    useEffect(() => {
        setCities(iller);
    }, []);

    const handleChange = (
        e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement | HTMLSelectElement>
        ) => {
        const target = e.target;

        if (target.name === "location") {
            setSelected(target.value);
        }

        if (target instanceof HTMLInputElement && target.type === "file") {
            const file = target.files?.[0];
            if (file) {
                setForm({ ...form, profilePicture: file });
            }
        } else {
            setForm({ ...form, [target.name]: target.value });
        }
    };

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setLoading(true);
        setError(null);
        try {
            await register(form);
            navigate("/login"); 
        } catch (err: unknown) {
            if (err instanceof AxiosError) {
                const errors = err.response?.data?.errors;
                setError(errors != null 
                    ? <ErrorMessages errors={errors} />
                    : err.response?.data?.message || "Unexpected error");
            } else {
                setError("Unexpected error");
            }
        } finally {
            setLoading(false);
        }
    };
        
    return (
        <>
        <form
            onSubmit={handleSubmit}>
                <h1>Register</h1>

                {error && (
                <div>{error}</div>
                )}

                <div>
                    <label>First Name</label>
                    <input
                        type="text"
                        name="firstName"
                        value={form.firstName}
                        onChange={handleChange}
                        required
                    />
                </div>

                <div>
                    <label>Last Name</label>
                    <input
                        type="text"
                        name="lastName"
                        value={form.lastName}
                        onChange={handleChange}
                        required
                    />
                </div>


                <div>
                    <label>Email</label>
                    <input
                        type="email"
                        name="email"
                        value={form.email}
                        onChange={handleChange}
                        required
                    />
                </div>
                
                <div>
                    <label>Password</label>
                    <input
                        type="password"
                        name="password"
                        value={form.password}
                        onChange={handleChange}
                        required
                    />
                </div>

                <div>
                    <label>Contact Info</label>
                    <textarea
                        name="contactInfo"
                        value={form.contactInfo}
                        onChange={handleChange}
                        required
                    />
                </div>

                <div>
                    <label>Select City</label>
                    <select
                        value={selected}
                        name="location"
                        onChange={handleChange}
                        required
                    >
                        <option value="">Şehir seçin</option>
                        {Object.entries(cities).map(([key, name]) => (
                            <option key={key} value={name}>
                                {name}
                            </option>
                        ))}
                    </select>
                </div>
                
                <div>
                    <label>Profile Picture</label>
                    <input
                        type="file"
                        name="profilePicture"
                        onChange={handleChange}
                        required
                    />
                </div>

                <button
                type="submit"
                disabled={loading}
                >
                {loading ? "..." : "Login"}
                </button>
            </form>
        </>
    )
}