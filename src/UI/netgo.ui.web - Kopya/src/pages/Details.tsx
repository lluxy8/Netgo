import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import type { ProductDTO } from "../types/dtos";
import { getProductById } from "../services/productService";
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card'
import { Button } from '@/components/ui/button'
import { Badge } from '@/components/ui/badge'
import { Separator } from '@/components/ui/separator'
import { Alert, AlertDescription } from '@/components/ui/alert'
import { ArrowLeft, Package, MessageCircle, Heart, Share2, Calendar } from 'lucide-react'
import { Link } from 'react-router-dom'
import { minioBaseUrl } from "@/services/Minio";

export function DetailsPage() {
    const { id } = useParams()
    const [product, setProduct] = useState<ProductDTO>();
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const navigate = useNavigate();

    useEffect(() => {
        const fetchProduct = async () => {
            try {
                if (!id) {
                    navigate("/notfound");
                    return;
                }

                setLoading(true);
                const response = await getProductById(id);
                setProduct(response);
            } catch (err) {
                console.error("Ürün yüklenemedi:", err);
                setError("Ürün yüklenirken bir hata oluştu.");
            } finally {
                setLoading(false);
            }
        }

        fetchProduct();
    }, [navigate, id])

    if (loading) {
        return (
            <div className="flex items-center justify-center py-12">
                <div className="text-center">
                    <div className="animate-spin rounded-full h-8 w-8 border-b-2 border-primary mx-auto mb-4"></div>
                    <p className="text-muted-foreground">Ürün yükleniyor...</p>
                </div>
            </div>
        );
    }

    if (error || !product) {
        return (
            <div className="text-center py-12">
                <Package className="h-12 w-12 text-muted-foreground mx-auto mb-4" />
                <h2 className="text-2xl font-bold mb-2">Ürün bulunamadı</h2>
                <p className="text-muted-foreground mb-4">
                    {error || "Aradığınız ürün mevcut değil."}
                </p>
                <Button asChild>
                    <Link to="/products">Ürünlere Dön</Link>
                </Button>
            </div>
        );
    }

    return (
        <div className="space-y-6">
            {/* Back Button */}
            <Button variant="outline" asChild>
                <Link to="/products" className="flex items-center gap-2">
                    <ArrowLeft className="h-4 w-4" />
                    Geri Dön
                </Link>
            </Button>

            <div className="grid grid-cols-1 lg:grid-cols-2 gap-8">
                {/* Product Images */}
                <div className="space-y-4">
                    <div className="aspect-square bg-muted rounded-lg overflow-hidden">
                        {product.images && product.images.length > 0 ? (
                            <img 
                                src={minioBaseUrl() + product.images[0]} 
                                alt={product.title}
                                className="w-full h-full object-cover"
                            />
                        ) : (
                            <div className="w-full h-full flex items-center justify-center">
                                <Package className="h-24 w-24 text-muted-foreground" />
                            </div>
                        )}
                    </div>
                    
                    {product.images && product.images.length > 1 && (
                        <div className="grid grid-cols-4 gap-2">
                            {product.images.slice(1, 5).map((image, index) => (
                                <div key={index} className="aspect-square bg-muted rounded-md overflow-hidden">
                                    <img 
                                        src={image} 
                                        alt={`${product.title} ${index + 2}`}
                                        className="w-full h-full object-cover"
                                    />
                                </div>
                            ))}
                        </div>
                    )}
                </div>

                {/* Product Details */}
                <div className="space-y-6">
                    <div>
                        <div className="flex items-start justify-between mb-2">
                            <h1 className="text-3xl font-bold">{product.title}</h1>
                            {product.tradable && (
                                <Badge variant="secondary" className="ml-2">
                                    Takas
                                </Badge>
                            )}
                        </div>
                        <p className="text-2xl font-bold text-primary mb-4">
                            ₺{product.price.toFixed(2)}
                        </p>
                        <p className="text-muted-foreground leading-relaxed">
                            {product.description}
                        </p>
                    </div>

                    <Separator />

                    {/* Product Details */}
                    {product.details && product.details.length > 0 && (
                        <div>
                            <h3 className="text-lg font-semibold mb-4">Ürün Detayları</h3>
                            <div className="space-y-3">
                                {product.details.map((detail, index) => (
                                    <div key={index} className="flex justify-between py-2 border-b border-muted">
                                        <span className="font-medium text-muted-foreground">
                                            {detail.title}:
                                        </span>
                                        <span>{detail.value}</span>
                                    </div>
                                ))}
                            </div>
                        </div>
                    )}

                    <Separator />

                    {/* Action Buttons */}
                    <div className="space-y-4">
                        <div className="flex gap-2">
                            <Button className="flex-1 flex items-center gap-2">
                                <MessageCircle className="h-4 w-4" />
                                İletişime Geç
                            </Button>
                            <Button variant="outline" size="icon">
                                <Heart className="h-4 w-4" />
                            </Button>
                            <Button variant="outline" size="icon">
                                <Share2 className="h-4 w-4" />
                            </Button>
                        </div>

                        {product.dateSold && (
                            <Alert>
                                <Calendar className="h-4 w-4" />
                                <AlertDescription>
                                    Bu ürün {new Date(product.dateSold).toLocaleDateString('tr-TR')} tarihinde satılmıştır.
                                </AlertDescription>
                            </Alert>
                        )}
                    </div>

                    {/* Additional Info */}
                    <Card>
                        <CardHeader>
                            <CardTitle className="text-lg">Ürün Bilgileri</CardTitle>
                        </CardHeader>
                        <CardContent className="space-y-2 text-sm text-muted-foreground">
                            <div className="flex justify-between">
                                <span>Ürün ID:</span>
                                <span className="font-mono">{product.id}</span>
                            </div>
                            <div className="flex justify-between">
                                <span>Kategori ID:</span>
                                <span className="font-mono">{product.categoryId}</span>
                            </div>
                            <div className="flex justify-between">
                                <span>Normalize Başlık:</span>
                                <span className="font-mono">{product.normalizedTitle}</span>
                            </div>
                        </CardContent>
                    </Card>
                </div>
            </div>
        </div>
    )
}