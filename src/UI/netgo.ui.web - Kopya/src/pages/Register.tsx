import { useEffect, useState } from "react";
import { useNavigate, Link } from "react-router-dom";
import type { RegistrationRequest } from "../types/dtos";
import { register } from "../services/authService";
import { AxiosError } from "axios";
import iller from "../assets/turkiye.json";
import ErrorMessages from "../components/ErrorMessages";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '@/components/ui/card'
import { Button } from '@/components/ui/button'
import { Input } from '@/components/ui/input'
import { Label } from '@/components/ui/label'
import { Textarea } from '@/components/ui/textarea'
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from '@/components/ui/select'
import { Alert, AlertDescription } from '@/components/ui/alert'
import { Package } from 'lucide-react'

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
        e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>
        ) => {
        const target = e.target;

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
                    <h2 className="text-3xl font-bold">Hesap oluşturun</h2>
                    <p className="mt-2 text-muted-foreground">
                        Zaten hesabınız var mı?{' '}
                        <Link to="/login" className="text-primary hover:underline">
                            Giriş yapın
                        </Link>
                    </p>
                </div>

                <Card>
                    <CardHeader>
                        <CardTitle>Kayıt Ol</CardTitle>
                        <CardDescription>
                            Bilgilerinizi girerek yeni bir hesap oluşturun
                        </CardDescription>
                    </CardHeader>
                    <CardContent>
                        <form onSubmit={handleSubmit} className="space-y-4">
                            {error && (
                                <Alert variant="destructive">
                                    <AlertDescription>
                                        <ul className="list-disc pl-5 space-y-1">
                                        {error
                                            .split('<br/>')
                                            .map(p => p.trim())
                                            .filter(Boolean) 
                                            .map((part, i) => (
                                            <li key={i}>{part}</li>
                                            ))}
                                        </ul>
                                    </AlertDescription>
                                </Alert>
                            )}

                            <div className="grid grid-cols-2 gap-4">
                                <div className="space-y-2">
                                    <Label htmlFor="firstName">Ad</Label>
                                    <Input
                                        id="firstName"
                                        type="text"
                                        name="firstName"
                                        value={form.firstName}
                                        onChange={handleChange}
                                        placeholder="Adınız"
                                        required
                                    />
                                </div>
                                <div className="space-y-2">
                                    <Label htmlFor="lastName">Soyad</Label>
                                    <Input
                                        id="lastName"
                                        type="text"
                                        name="lastName"
                                        value={form.lastName}
                                        onChange={handleChange}
                                        placeholder="Soyadınız"
                                        required
                                    />
                                </div>
                            </div>

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
                                    placeholder="Güçlü bir şifre girin"
                                    required
                                />
                            </div>

                            <div className="space-y-2">
                                <Label htmlFor="contactInfo">İletişim Bilgileri</Label>
                                <Textarea
                                    id="contactInfo"
                                    name="contactInfo"
                                    value={form.contactInfo}
                                    onChange={handleChange}
                                    placeholder="Telefon numaranız veya diğer iletişim bilgileriniz"
                                    required
                                />
                            </div>

                            <div className="space-y-2">
                                <Label htmlFor="location">Şehir</Label>
                                <Select value={selected} onValueChange={(value) => {
                                    setSelected(value);
                                    setForm({ ...form, location: value });
                                }}>
                                    <SelectTrigger>
                                        <SelectValue placeholder="Şehir seçin" />
                                    </SelectTrigger>
                                    <SelectContent>
                                        {Object.entries(cities).map(([key, name]) => (
                                            <SelectItem key={key} value={name}>
                                                {name}
                                            </SelectItem>
                                        ))}
                                    </SelectContent>
                                </Select>
                            </div>
                            
                            <div className="space-y-2">
                                <Label htmlFor="profilePicture">Profil Fotoğrafı</Label>
                                <Input
                                    id="profilePicture"
                                    type="file"
                                    name="profilePicture"
                                    onChange={handleChange}
                                    accept="image/*"
                                    required
                                />
                            </div>

                            <Button
                                type="submit"
                                className="w-full"
                                disabled={loading}
                            >
                                {loading ? "Hesap oluşturuluyor..." : "Hesap Oluştur"}
                            </Button>
                        </form>
                    </CardContent>
                </Card>
            </div>
        </div>
    )
}