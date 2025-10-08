import { Button } from '@/components/ui/button'
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '@/components/ui/card'
import { Home, Search, ArrowLeft } from 'lucide-react'
import { Link } from 'react-router-dom'

export default function NotFoundPage() {
    return (
        <div className="min-h-[60vh] flex items-center justify-center">
            <Card className="w-full max-w-md text-center">
                <CardHeader>
                    <div className="mx-auto w-24 h-24 bg-destructive/10 rounded-full flex items-center justify-center mb-4">
                        <Search className="h-12 w-12 text-destructive" />
                    </div>
                    <CardTitle className="text-2xl">Sayfa Bulunamadı</CardTitle>
                    <CardDescription>
                        Aradığınız sayfa mevcut değil veya taşınmış olabilir.
                    </CardDescription>
                </CardHeader>
                <CardContent className="space-y-4">
                    <p className="text-sm text-muted-foreground">
                        Lütfen URL'yi kontrol edin veya ana sayfaya dönün.
                    </p>
                    <div className="flex flex-col sm:flex-row gap-2">
                        <Button asChild className="flex-1">
                            <Link to="/" className="flex items-center gap-2">
                                <Home className="h-4 w-4" />
                                Ana Sayfa
                            </Link>
                        </Button>
                        <Button asChild variant="outline" className="flex-1">
                            <Link to="/products" className="flex items-center gap-2">
                                <Search className="h-4 w-4" />
                                Ürünleri Keşfet
                            </Link>
                        </Button>
                    </div>
                    <Button 
                        variant="ghost" 
                        onClick={() => window.history.back()}
                        className="flex items-center gap-2"
                    >
                        <ArrowLeft className="h-4 w-4" />
                        Geri Dön
                    </Button>
                </CardContent>
            </Card>
        </div>
    )
}