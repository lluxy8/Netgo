import { useEffect, useState } from "react";
import { createProduct } from "../services/productService";
import type { CreateProductDTO, ListCategoryDTO, ProductDetailDto } from "../types/dtos";
import { useNavigate } from "react-router-dom";
import { getUserById } from "../services/UserService";
import { getCategories } from "../services/categoryService";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '@/components/ui/card'
import { Button } from '@/components/ui/button'
import { Input } from '@/components/ui/input'
import { Label } from '@/components/ui/label'
import { Textarea } from '@/components/ui/textarea'
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from '@/components/ui/select'
import { Checkbox } from '@/components/ui/checkbox'
import { Alert, AlertDescription } from '@/components/ui/alert'
import { Plus, X, Upload, Package } from 'lucide-react'

export default function CreatePage() {
  const navigate = useNavigate();

  const [form, setForm] = useState<CreateProductDTO>({
    userId: "",
    categoryId: "",
    title: "",
    description: "",
    tradable: false,
    price: 0,
    details: [],
    images: [],
  });

  const [detailTitle, setDetailTitle] = useState("");
  const [detailValue, setDetailValue] = useState("");
  const [categories, setCategories] = useState<ListCategoryDTO[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  function handleChange(e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) {
    const target = e.target;
    const { name, type } = target;

    const value = type === "checkbox" && 'checked' in target ? target.checked : target.value;

    setForm((prev) => ({
      ...prev,
      [name]: value,
    }));
  }


  function handleAddDetail() {
    if (!detailTitle || !detailValue) return;
    const newDetail: ProductDetailDto = { title: detailTitle, value: detailValue };
    setForm((prev) => ({
      ...prev,
      details: [...prev.details, newDetail],
    }));
    setDetailTitle("");
    setDetailValue("");
  }

  function handleRemoveDetail(index: number) {
    setForm((prev) => ({
      ...prev,
      details: prev.details.filter((_, i) => i !== index),
    }));
  }

  function handleFiles(e: React.ChangeEvent<HTMLInputElement>) {
    const files = e.target.files;
    if (!files) return;
    setForm((prev) => ({
      ...prev,
      images: Array.from(files),
    }));
  }

  async function handleSubmit(e: React.FormEvent) {
    e.preventDefault();
    setLoading(true);
    setError(null);
    
    try {
      await createProduct(form);
      navigate("/products"); 
    } catch (err) {
      setError("Ürün oluşturulurken bir hata oluştu. Lütfen tekrar deneyin.");
      console.error("Ürün oluşturma hatası:", err);
    } finally {
      setLoading(false);
    }
  }

  useEffect(() => {
    (async () => {
      const uid = localStorage.getItem("uid");
      if (!uid) {
        navigate("/login");
        return;
      }

      try {
        const user = await getUserById(uid);
        if (!user) {
          navigate("/login");
          return;
        }

        setForm((prev) => ({ ...prev, userId: uid }));

        const categoryList = await getCategories();
        setCategories(categoryList);
      } catch (err) {
        console.error("Veri yüklenemedi:", err);
        setError("Veriler yüklenirken bir hata oluştu.");
      }
    })();
  }, [navigate]);

  return (
    <div className="max-w-2xl mx-auto space-y-6">
      {/* Header */}
      <div className="text-center">
        <h1 className="text-3xl font-bold">Yeni Ürün Ekle</h1>
        <p className="text-muted-foreground">
          Ürününüzü detaylı bir şekilde tanımlayın ve takas için hazırlayın
        </p>
      </div>

      {/* Error Alert */}
      {error && (
        <Alert variant="destructive">
          <AlertDescription>{error}</AlertDescription>
        </Alert>
      )}

      <form onSubmit={handleSubmit} className="space-y-6">
        {/* Basic Information */}
        <Card>
          <CardHeader>
            <CardTitle>Temel Bilgiler</CardTitle>
            <CardDescription>
              Ürününüz hakkında temel bilgileri girin
            </CardDescription>
          </CardHeader>
          <CardContent className="space-y-4">
            <div className="space-y-2">
              <Label htmlFor="title">Ürün Başlığı</Label>
              <Input
                id="title"
                name="title"
                value={form.title}
                onChange={handleChange}
                placeholder="Ürününüzün başlığını girin"
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

            <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
              <div className="space-y-2">
                <Label htmlFor="categoryId">Kategori</Label>
                <Select value={form.categoryId} onValueChange={(value) => 
                  setForm(prev => ({ ...prev, categoryId: value }))
                }>
                  <SelectTrigger>
                    <SelectValue placeholder="Kategori seçin" />
                  </SelectTrigger>
                  <SelectContent>
                    {categories.map((c) => (
                      <SelectItem key={c.id} value={c.id}>
                        {c.name}
                      </SelectItem>
                    ))}
                  </SelectContent>
                </Select>
              </div>

              <div className="space-y-2">
                <Label htmlFor="price">Fiyat (₺)</Label>
                <Input
                  id="price"
                  type="number"
                  name="price"
                  value={form.price}
                  onChange={handleChange}
                  placeholder="0.00"
                  step="0.01"
                  min="0"
                  required
                />
              </div>
            </div>

            <div className="flex items-center space-x-2">
              <Checkbox
                id="tradable"
                name="tradable"
                checked={form.tradable}
                onCheckedChange={(checked) => 
                  setForm(prev => ({ ...prev, tradable: checked as boolean }))
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
              Ürününüz hakkında ek bilgiler ekleyin (opsiyonel)
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
                <Label>Eklenen Detaylar</Label>
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

        {/* Images */}
        <Card>
          <CardHeader>
            <CardTitle>Ürün Fotoğrafları</CardTitle>
            <CardDescription>
              Ürününüzün fotoğraflarını yükleyin (birden fazla seçebilirsiniz)
            </CardDescription>
          </CardHeader>
          <CardContent className="space-y-4">
            <div className="border-2 border-dashed border-muted-foreground/25 rounded-lg p-6 text-center">
              <Upload className="h-8 w-8 mx-auto mb-2 text-muted-foreground" />
              <Label htmlFor="images" className="cursor-pointer">
                <span className="text-sm font-medium">Fotoğraf seçin</span>
                <Input
                  id="images"
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

            {form.images.length > 0 && (
              <div className="space-y-2">
                <Label>Seçilen Dosyalar</Label>
                <div className="grid grid-cols-2 md:grid-cols-3 gap-2">
                  {form.images.map((file, i) => (
                    <div key={i} className="flex items-center justify-between p-2 bg-muted rounded-md">
                      <span className="text-sm truncate">{file.name}</span>
                      <Button
                        type="button"
                        variant="ghost"
                        size="sm"
                        onClick={() => {
                          const newImages = form.images.filter((_, index) => index !== i);
                          setForm(prev => ({ ...prev, images: newImages }));
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
            onClick={() => navigate("/products")}
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
                Oluşturuluyor...
              </>
            ) : (
              <>
                <Package className="h-4 w-4" />
                Ürünü Oluştur
              </>
            )}
          </Button>
        </div>
      </form>
    </div>
  );
}
