import { useEffect, useState } from "react"
import type { User } from "../types/dtos";
import { useNavigate } from "react-router-dom";
import { getMe } from "../services/UserService";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '@/components/ui/card'
import { Button } from '@/components/ui/button'
import { Badge } from '@/components/ui/badge'
import { Separator } from '@/components/ui/separator'
import { User as UserIcon, Mail, MapPin, Phone, Shield, Edit, Plus } from 'lucide-react'
import { Link } from 'react-router-dom'
import { minioBaseUrl } from "@/services/Minio";

export default function MePage() {
    const navigate = useNavigate();
    const uid = localStorage.getItem("uid")
    const [user, setUser] = useState<User>();
    const [loading, setLoading] = useState(true);
    
    useEffect(() => {

        const fetchUser = async () => {
            try {
                const response = await getMe();
                if (!response) {
                    navigate("/login");
                    return;
                }
                setUser(response);
            } catch (error) {
                console.error("Kullanıcı bilgileri yüklenemedi:", error);
                navigate("/login");
            } finally {
                setLoading(false);
            }
        };

        fetchUser();
    }, [uid, navigate]); 

    if (loading) {
        return (
            <div className="flex items-center justify-center py-12">
                <div className="text-center">
                    <div className="animate-spin rounded-full h-8 w-8 border-b-2 border-primary mx-auto mb-4"></div>
                    <p className="text-muted-foreground">Yükleniyor...</p>
                </div>
            </div>
        );
    }

    if (!user) {
        return null;
    }

    return (
        <div className="space-y-6">
            {/* Header */}
            <div className="flex flex-col sm:flex-row justify-between items-start sm:items-center gap-4">
                <div>
                    <h1 className="text-3xl font-bold">Profilim</h1>
                    <p className="text-muted-foreground">
                        Hesap bilgilerinizi görüntüleyin ve yönetin
                    </p>
                </div>
                <div className="flex gap-2">
                    <Button asChild variant="outline">
                        <Link to="/create">
                            <Plus className="h-4 w-4 mr-2" />
                            Ürün Ekle
                        </Link>
                    </Button>
                    <Button asChild>
                        <Link to="/edit">
                            <Edit className="h-4 w-4 mr-2" />
                            Düzenle
                        </Link>
                    </Button>
                </div>
            </div>

            <div className="grid grid-cols-1 lg:grid-cols-3 gap-6">
                {/* Profile Card */}
                <div className="lg:col-span-1">
                    <Card>
                        <CardHeader className="text-center">
                            <div className="mx-auto w-24 h-24 bg-primary/10 rounded-full flex items-center justify-center mb-4">
                                {user.profilePictureURL ? (
                                    <img 
                                        src={minioBaseUrl() + user.profilePictureURL} 
                                        alt="Profil"
                                        className="w-24 h-24 rounded-full object-cover"
                                    />
                                ) : (
                                    <UserIcon className="h-12 w-12 text-primary" />
                                )}
                            </div>
                            <CardTitle className="text-xl">
                                {user.firstName} {user.lastName}
                            </CardTitle>
                            <CardDescription>
                                {user.normalizedUserName}
                            </CardDescription>
                            {user.verifiedSeller && (
                                <Badge variant="default" className="mt-2">
                                    <Shield className="h-3 w-3 mr-1" />
                                    Doğrulanmış Satıcı
                                </Badge>
                            )}
                        </CardHeader>
                        <CardContent className="space-y-4">
                            <div className="flex items-center gap-3">
                                <Mail className="h-4 w-4 text-muted-foreground" />
                                <span className="text-sm">{user.email}</span>
                            </div>
                            <div className="flex items-center gap-3">
                                <MapPin className="h-4 w-4 text-muted-foreground" />
                                <span className="text-sm">{user.location}</span>
                            </div>
                            <div className="flex items-center gap-3">
                                <Phone className="h-4 w-4 text-muted-foreground" />
                                <span className="text-sm">{user.contactInfo}</span>
                            </div>
                        </CardContent>
                    </Card>
                </div>

                {/* Profile Details */}
                <div className="lg:col-span-2 space-y-6">
                    {/* Account Status */}
                    <Card>
                        <CardHeader>
                            <CardTitle>Hesap Durumu</CardTitle>
                        </CardHeader>
                        <CardContent className="space-y-4">
                            <div className="flex items-center justify-between">
                                <span className="text-sm font-medium">E-posta Doğrulama</span>
                                <Badge variant={user.emailConfirmed ? "default" : "destructive"}>
                                    {user.emailConfirmed ? "Doğrulanmış" : "Doğrulanmamış"}
                                </Badge>
                            </div>
                            <div className="flex items-center justify-between">
                                <span className="text-sm font-medium">Satıcı Durumu</span>
                                {user.verifiedSeller
                                ?
                                    <Badge className="text-green-500">
                                        Doğrulanmış Satıcı
                                    </Badge>
                                :   
                                    <Badge className="text-red-600">
                                        Doğrulanmamış Satıcı
                                    </Badge>}

                            </div>
                        </CardContent>
                    </Card>

                    {/* Contact Information */}
                    <Card>
                        <CardHeader>
                            <CardTitle>İletişim Bilgileri</CardTitle>
                            <CardDescription>
                                Diğer kullanıcıların sizinle iletişime geçebilmesi için bu bilgileri kullanır
                            </CardDescription>
                        </CardHeader>
                        <CardContent className="space-y-4">
                            <div>
                                <label className="text-sm font-medium text-muted-foreground">E-posta</label>
                                <p className="text-sm">{user.email}</p>
                            </div>
                            <Separator />
                            <div>
                                <label className="text-sm font-medium text-muted-foreground">Telefon/İletişim</label>
                                <p className="text-sm">{user.contactInfo}</p>
                            </div>
                            <Separator />
                            <div>
                                <label className="text-sm font-medium text-muted-foreground">Konum</label>
                                <p className="text-sm">{user.location}</p>
                            </div>
                        </CardContent>
                    </Card>

                    {/* Quick Actions */}
                    <Card>
                        <CardHeader>
                            <CardTitle>Hızlı İşlemler</CardTitle>
                        </CardHeader>
                        <CardContent>
                            <div className="grid grid-cols-1 sm:grid-cols-2 gap-4">
                                <Button asChild variant="outline" className="h-auto p-4">
                                    <Link to="/create" className="flex flex-col items-center gap-2">
                                        <Plus className="h-6 w-6" />
                                        <span>Yeni Ürün Ekle</span>
                                    </Link>
                                </Button>
                                <Button asChild variant="outline" className="h-auto p-4">
                                    <Link to="/products" className="flex flex-col items-center gap-2">
                                        <UserIcon className="h-6 w-6" />
                                        <span>Ürünlerimi Gör</span>
                                    </Link>
                                </Button>
                            </div>
                        </CardContent>
                    </Card>
                </div>
            </div>
        </div>
    )
}