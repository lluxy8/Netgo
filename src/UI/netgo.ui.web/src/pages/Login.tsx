"use client";

import { useState } from "react"
import type { AuthRequest } from "../types/dtos"
import { login } from "../services/authService"
import { useNavigate } from "react-router-dom";
import { AxiosError } from "axios";

export default function LoginPage() {

    const navigate = useNavigate();
    const [form, setForm] = useState<AuthRequest>({
        email: "",
        password: "",
    });

    const [loading, setLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setForm({ ...form, [e.target.name]: e.target.value });
    };

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setLoading(true);
        setError(null);
        try {
            const response = await login(form);
            localStorage.setItem("uid", response.id)
            navigate("/"); 
        } catch (err: unknown) {
            if (err instanceof AxiosError) {
                setError(err.response?.data?.errors != null 
                    ? err.response?.data?.errors
                    : err.response?.data?.message || "Unexpected error");
            } else {
                setError("Unexpected errors");
            }
        } finally {
            setLoading(false);
        }
    };
        
    return (
        <>
        <form
            onSubmit={handleSubmit}>
                <h1>Login</h1>

                {error && (
                <div>{error}</div>
                )}

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