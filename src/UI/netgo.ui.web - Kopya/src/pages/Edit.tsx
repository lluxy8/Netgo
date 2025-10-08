import { useEffect, useState } from "react";
import { getProductById, updateProduct } from "../services/productService";
import type { UpdateProductDTO, ProductDetailDto } from "../types/dtos";
import { useNavigate, useParams } from "react-router-dom";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '@/components/ui/card'
import { Button } from '@/components/ui/button'
import { Input } from '@/components/ui/input'
import { Label } from '@/components/ui/label'
import { Checkbox } from '@/components/ui/checkbox'
import { Alert, AlertDescription } from '@/components/ui/alert'
import { Plus, X, Upload, Package, ArrowLeft } from 'lucide-react'
import { Link } from 'react-router-dom'
import { minioBaseUrl } from "@/services/Minio";
import { Textarea } from "@/components/ui/textarea";
import { AxiosError } from "axios";
import ErrorMessages from "@/components/ErrorMessages";

export default function EditPage() {
  const [form, setForm] = useState<UpdateProductDTO | null>(null);
  const [detailTitle, setDetailTitle] = useState("");
  const [detailValue, setDetailValue] = useState("");
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const { id } = useParams();
  const navigate = useNavigate();

  function handleChange(
    e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>
  ) {
    const target = e.target;
    const { name, type } = target;

    const value = type === "checkbox" && 'checked' in target ? target.checked : target.value;

    if (form) {
      setForm((prev) => ({
        ...prev!,
        [name]: value,
      }));
    }
  }


  function handleAddDetail() {
    if (!detailTitle || !detailValue || !form) return;
    const newDetail: ProductDetailDto = { title: detailTitle, value: detailValue };
    setForm((prev) => ({
      ...prev!,
      details: [...prev!.details, newDetail],
    }));
    setDetailTitle("");
    setDetailValue("");
  }

  function handleRemoveDetail(index: number) {
    if (!form) return;
    setForm((prev) => ({
      ...prev!,
      details: prev!.details.filter((_, i) => i !== index),
    }));
  }

  function handleFiles(e: React.ChangeEvent<HTMLInputElement>) {
    const files = e.target.files;
    if (!files || !form) return;
    setForm((prev) => ({
      ...prev!,
      newImages: Array.from(files),
    }));
  }

  async function handleSubmit(e: React.FormEvent) {
    e.preventDefault();
    if (!form) return;

    setLoading(true);
    setError(null);
    
    try {
      await updateProduct(form);
      navigate(`/details/${id}`);
    } catch (err) {
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
  }

  useEffect(() => {
    const fetchProduct = async () => {
      if (!id) {
        navigate("/notfound");
        return;
      }

      try {
        setLoading(true);
        const response = await getProductById(id);
        setForm(response);
      } catch (err) {
        console.error("Ürün yüklenemedi:", err);
        setError("Ürün yüklenirken bir hata oluştu.");
      } finally {
        setLoading(false);
      }
    };

    fetchProduct();
  }, [id, navigate]);

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

  if (!form) {
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
    <div className="max-w-2xl mx-auto space-y-6">
      {/* Header */}
      <div className="flex items-center gap-4">
        <Button variant="outline" asChild>
          <Link to={`/details/${id}`} className="flex items-center gap-2">
            <ArrowLeft className="h-4 w-4" />
            Geri Dön
          </Link>
        </Button>
        <div>
          <h1 className="text-3xl font-bold">Ürünü Düzenle</h1>
          <p className="text-muted-foreground">
            Ürün bilgilerinizi güncelleyin
          </p>
        </div>
      </div>

      {/* Error Alert */}
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

      <form onSubmit={handleSubmit} className="space-y-6">
        {/* Basic Information */}
        <Card>
          <CardHeader>
            <CardTitle>Temel Bilgiler</CardTitle>
            <CardDescription>
              Ürününüzün temel bilgilerini güncelleyin
            </CardDescription>
          </CardHeader>
          <CardContent className="space-y-4">
            <div className="space-y-2">
              <Label htmlFor="categoryId">Başlık</Label>
              <Input
                id="title"
                name="title"
                value={form.title}
                onChange={handleChange}
                required
              />
            </div>

            <div className="space-y-2">
              <Label htmlFor="description">Açıklama</Label>
              <Textarea
                id="description"
                name="description"
                value={form.description}
                onChange={handleChange}
                placeholder="Ürününüz hakkında detaylı açıklama yazın"
                rows={4}
                required
              />
            </div>

            <div className="space-y-2">
              <Label htmlFor="categoryId">Kategori ID</Label>
              <Input
                id="categoryId"
                name="categoryId"
                value={form.categoryId}
                onChange={handleChange}
                required
              />
            </div>

            <div className="flex items-center space-x-2">
              <Checkbox
                id="tradable"
                name="tradable"
                checked={form.tradable}
                onCheckedChange={(checked) => 
                  setForm(prev => ({ ...prev!, tradable: checked as boolean }))
                }
              />
              <Label htmlFor="tradable">Takaslanabilir</Label>
            </div>
          </CardContent>
        </Card>

        {/* Product Details */}
        <Card>
          <CardHeader>
            <CardTitle>Ürün Detayları</CardTitle>
            <CardDescription>
              Ürününüz hakkında ek bilgiler ekleyin veya düzenleyin
            </CardDescription>
          </CardHeader>
          <CardContent className="space-y-4">
            <div className="grid grid-cols-1 md:grid-cols-3 gap-2">
              <Input
                placeholder="Detay başlığı (örn: Marka)"
                value={detailTitle}
                onChange={(e) => setDetailTitle(e.target.value)}
              />
              <Input
                placeholder="Detay değeri (örn: Apple)"
                value={detailValue}
                onChange={(e) => setDetailValue(e.target.value)}
              />
              <Button
                type="button"
                onClick={handleAddDetail}
                disabled={!detailTitle || !detailValue}
                className="flex items-center gap-2"
              >
                <Plus className="h-4 w-4" />
                Ekle
              </Button>
            </div>

            {form.details.length > 0 && (
              <div className="space-y-2">
                <Label>Mevcut Detaylar</Label>
                <div className="space-y-2">
                  {form.details.map((d, i) => (
                    <div key={i} className="flex items-center justify-between p-2 bg-muted rounded-md">
                      <span className="text-sm">
                        <strong>{d.title}:</strong> {d.value}
                      </span>
                      <Button
                        type="button"
                        variant="ghost"
                        size="sm"
                        onClick={() => handleRemoveDetail(i)}
                        className="h-8 w-8 p-0"
                      >
                        <X className="h-4 w-4" />
                      </Button>
                    </div>
                  ))}
                </div>
              </div>
            )}
          </CardContent>
        </Card>

        {/* Current Images */}
        <Card>
          <CardHeader>
            <CardTitle>Mevcut Resimler</CardTitle>
            <CardDescription>
              Ürününüzün mevcut resimleri
            </CardDescription>
          </CardHeader>
          <CardContent>
            {form.images.length > 0 ? (
              <div className="grid grid-cols-2 md:grid-cols-3 gap-2">
                {form.images.map((img, i) => (
                  <div key={i} className="aspect-square bg-muted rounded-md overflow-hidden">
                    <img 
                      src={minioBaseUrl() + img} 
                      alt={`Mevcut resim ${i + 1}`}
                      className="w-full h-full object-cover"
                    />
                  </div>
                ))}
              </div>
            ) : (
              <p className="text-muted-foreground text-sm">Mevcut resim bulunmuyor</p>
            )}
          </CardContent>
        </Card>

        {/* New Images */}
        <Card>
          <CardHeader>
            <CardTitle>Yeni Resimler Ekle</CardTitle>
            <CardDescription>
              Ürününüze yeni resimler ekleyin
            </CardDescription>
          </CardHeader>
          <CardContent className="space-y-4">
            <div className="border-2 border-dashed border-muted-foreground/25 rounded-lg p-6 text-center">
              <Upload className="h-8 w-8 mx-auto mb-2 text-muted-foreground" />
              <Label htmlFor="newImages" className="cursor-pointer">
                <span className="text-sm font-medium">Yeni resimler seçin</span>
                <Input
                  id="newImages"
                  type="file"
                  multiple
                  onChange={handleFiles}
                  accept="image/*"
                  className="hidden"
                />
              </Label>
              <p className="text-xs text-muted-foreground mt-1">
                PNG, JPG, GIF formatları desteklenir
              </p>
            </div>

            {form.newImages && form.newImages.length > 0 && (
              <div className="space-y-2">
                <Label>Seçilen Yeni Dosyalar</Label>
                <div className="grid grid-cols-2 md:grid-cols-3 gap-2">
                  {form.newImages.map((file, i) => (
                    <div key={i} className="flex items-center justify-between p-2 bg-muted rounded-md">
                      <span className="text-sm truncate">{file.name}</span>
                      <Button
                        type="button"
                        variant="ghost"
                        size="sm"
                        onClick={() => {
                          const newImages = form.newImages!.filter((_, index) => index !== i);
                          setForm(prev => ({ ...prev!, newImages }));
                        }}
                        className="h-6 w-6 p-0"
                      >
                        <X className="h-3 w-3" />
                      </Button>
                    </div>
                  ))}
                </div>
              </div>
            )}
          </CardContent>
        </Card>

        {/* Submit Button */}
        <div className="flex justify-end gap-4">
          <Button
            type="button"
            variant="outline"
            onClick={() => navigate(`/details/${id}`)}
          >
            İptal
          </Button>
          <Button
            type="submit"
            disabled={loading}
            className="flex items-center gap-2"
          >
            {loading ? (
              <>
                <div className="animate-spin rounded-full h-4 w-4 border-b-2 border-white"></div>
                Güncelleniyor...
              </>
            ) : (
              <>
                <Package className="h-4 w-4" />
                Güncelle
              </>
            )}
          </Button>
        </div>
      </form>
    </div>
  );
}
