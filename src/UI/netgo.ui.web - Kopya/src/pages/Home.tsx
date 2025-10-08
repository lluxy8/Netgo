import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '@/components/ui/card'
import { Button } from '@/components/ui/button'
import { Package, Users, TrendingUp, Shield } from 'lucide-react'
import { Link } from 'react-router-dom'

export default function HomePage() {
    const isLoggedIn = localStorage.getItem('uid')

    return (
        <div className="space-y-12">
            {/* Hero Section */}
            <section className="text-center space-y-6">
                <h1 className="text-4xl font-bold tracking-tight sm:text-6xl">
                    NetGo ile{' '}
                    <span className="text-primary">takas</span> yapın
                </h1>
                <p className="text-xl text-muted-foreground max-w-2xl mx-auto">
                    Kullanmadığınız eşyalarınızı başkalarıyla takas edin. 
                    Hem çevre dostu hem de ekonomik bir çözüm.
                </p>
                <div className="flex flex-col sm:flex-row gap-4 justify-center">
                    {isLoggedIn ? (
                        <>
                            <Button asChild size="lg">
                                <Link to="/products">Ürünleri Keşfet</Link>
                            </Button>
                            <Button asChild variant="outline" size="lg">
                                <Link to="/create">Ürün Ekle</Link>
                            </Button>
                        </>
                    ) : (
                        <>
                            <Button asChild size="lg">
                                <Link to="/register">Hemen Başla</Link>
                            </Button>
                            <Button asChild variant="outline" size="lg">
                                <Link to="/products">Ürünleri Gör</Link>
                            </Button>
                        </>
                    )}
                </div>
            </section>

            {/* Features Section */}
            <section className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
                <Card>
                    <CardHeader className="text-center">
                        <Package className="h-12 w-12 text-primary mx-auto mb-4" />
                        <CardTitle>Kolay Takas</CardTitle>
                        <CardDescription>
                            Ürünlerinizi kolayca listeyin ve takas yapın
                        </CardDescription>
                    </CardHeader>
                </Card>

                <Card>
                    <CardHeader className="text-center">
                        <Users className="h-12 w-12 text-primary mx-auto mb-4" />
                        <CardTitle>Güvenli Topluluk</CardTitle>
                        <CardDescription>
                            Doğrulanmış kullanıcılarla güvenli takas
                        </CardDescription>
                    </CardHeader>
                </Card>

                <Card>
                    <CardHeader className="text-center">
                        <TrendingUp className="h-12 w-12 text-primary mx-auto mb-4" />
                        <CardTitle>Ekonomik</CardTitle>
                        <CardDescription>
                            Para harcamadan ihtiyacınız olan eşyaları alın
                        </CardDescription>
                    </CardHeader>
                </Card>

                <Card>
                    <CardHeader className="text-center">
                        <Shield className="h-12 w-12 text-primary mx-auto mb-4" />
                        <CardTitle>Çevre Dostu</CardTitle>
                        <CardDescription>
                            Sürdürülebilir yaşam için eşya döngüsü
                        </CardDescription>
                    </CardHeader>
                </Card>
            </section>

            {/* How it Works Section */}
            <section className="text-center space-y-8">
                <h2 className="text-3xl font-bold">Nasıl Çalışır?</h2>
                <div className="grid grid-cols-1 md:grid-cols-3 gap-8">
                    <div className="space-y-4">
                        <div className="w-16 h-16 bg-primary text-primary-foreground rounded-full flex items-center justify-center text-2xl font-bold mx-auto">
                            1
                        </div>
                        <h3 className="text-xl font-semibold">Ürün Ekle</h3>
                        <p className="text-muted-foreground">
                            Kullanmadığınız eşyalarınızı fotoğraflarıyla birlikte ekleyin
                        </p>
                    </div>
                    <div className="space-y-4">
                        <div className="w-16 h-16 bg-primary text-primary-foreground rounded-full flex items-center justify-center text-2xl font-bold mx-auto">
                            2
                        </div>
                        <h3 className="text-xl font-semibold">Keşfet</h3>
                        <p className="text-muted-foreground">
                            İhtiyacınız olan eşyaları arayın ve filtreleyin
                        </p>
                    </div>
                    <div className="space-y-4">
                        <div className="w-16 h-16 bg-primary text-primary-foreground rounded-full flex items-center justify-center text-2xl font-bold mx-auto">
                            3
                        </div>
                        <h3 className="text-xl font-semibold">Takas Et</h3>
                        <p className="text-muted-foreground">
                            Diğer kullanıcılarla iletişime geçin ve takas yapın
                        </p>
                    </div>
                </div>
            </section>
        </div>
    )
}