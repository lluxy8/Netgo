import { useState } from "react"
import type { AuthRequest } from "../types/dtos"
import { login } from "../services/authService"
import { useNavigate, Link } from "react-router-dom";
import { AxiosError } from "axios";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '@/components/ui/card'
import { Button } from '@/components/ui/button'
import { Input } from '@/components/ui/input'
import { Label } from '@/components/ui/label'
import { Alert, AlertDescription } from '@/components/ui/alert'
import { Package } from 'lucide-react'

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
                    : err.response?.data?.message || "Beklenmeyen bir hata oluştu");
            } else {
                setError("Beklenmeyen bir hata oluştu");
            }
        } finally {
            setLoading(false);
        }
    };
        
    return (
        <div className="min-h-screen flex items-center justify-center py-12 px-4 sm:px-6 lg:px-8">
            <div className="max-w-md w-full space-y-8">
                <div className="text-center">
                    <Link to="/" className="flex items-center justify-center space-x-2 mb-6">
                        <Package className="h-8 w-8 text-primary" />
                        <span className="text-2xl font-bold">NetGo</span>
                    </Link>
                    <h2 className="text-3xl font-bold">Hesabınıza giriş yapın</h2>
                    <p className="mt-2 text-muted-foreground">
                        Hesabınız yok mu?{' '}
                        <Link to="/register" className="text-primary hover:underline">
                            Kayıt olun
                        </Link>
                    </p>
                </div>

                <Card>
                    <CardHeader>
                        <CardTitle>Giriş Yap</CardTitle>
                        <CardDescription>
                            E-posta ve şifrenizi girerek hesabınıza giriş yapın
                        </CardDescription>
                    </CardHeader>
                    <CardContent>
                        <form onSubmit={handleSubmit} className="space-y-4">
                            {error && (
                                <Alert variant="destructive">
                                    <AlertDescription>{error}</AlertDescription>
                                </Alert>
                            )}

                            <div className="space-y-2">
                                <Label htmlFor="email">E-posta</Label>
                                <Input
                                    id="email"
                                    type="email"
                                    name="email"
                                    value={form.email}
                                    onChange={handleChange}
                                    placeholder="ornek@email.com"
                                    required
                                />
                            </div>

                            <div className="space-y-2">
                                <Label htmlFor="password">Şifre</Label>
                                <Input
                                    id="password"
                                    type="password"
                                    name="password"
                                    value={form.password}
                                    onChange={handleChange}
                                    placeholder="Şifrenizi girin"
                                    required
                                />
                            </div>

                            <Button
                                type="submit"
                                className="w-full"
                                disabled={loading}
                            >
                                {loading ? "Giriş yapılıyor..." : "Giriş Yap"}
                            </Button>
                        </form>
                    </CardContent>
                </Card>
            </div>
        </div>
    )
}