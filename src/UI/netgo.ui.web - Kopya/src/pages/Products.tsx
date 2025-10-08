import { useEffect, useState } from "react";
import { getProducts } from "../services/productService";
import type { GetProductsDTO, ListCategoryDTO, ListProductDTO, PagedResult } from "../types/dtos";
import { AxiosError } from "axios";
import { getCategories } from "../services/categoryService";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '@/components/ui/card'
import { Button } from '@/components/ui/button'
import { Input } from '@/components/ui/input'
import { Label } from '@/components/ui/label'
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from '@/components/ui/select'
import { Checkbox } from '@/components/ui/checkbox'
import { Badge } from '@/components/ui/badge'
import { Alert, AlertDescription } from '@/components/ui/alert'
import { Search, Filter, ChevronLeft, ChevronRight, Package } from 'lucide-react'
import { Link } from 'react-router-dom'
import { minioBaseUrl } from "@/services/Minio";

export default function ProductsPage() {
  const defaultFilter: GetProductsDTO = {
    title: null,
    categoryId: null,
    priceMin: null,
    priceMax: null,
    priceFixed: null,
    tradable: true,
    sold: false,
    page: 1,
    pageSize: 30,
  };

  const [products, setProducts] = useState<PagedResult<ListProductDTO>>();
  const [categories, setCategories] = useState<ListCategoryDTO[]>();
  const [categoryId, setCategoryId] = useState<string>("");
  const [filter, setFilter] = useState<GetProductsDTO>(defaultFilter);
  const [error, setError] = useState<string>();
  const [showFilters, setShowFilters] = useState(true);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, type, checked, value } = e.target;
    setFilter({
      ...filter,
      [name]: type === "checkbox" ? checked : value,
    });
  };

  const fetchProducts = async () => {
    try{
        if(categoryId)
            setFilter(prev => ({ ...prev, categoryId: categoryId}));
        const responseProducts = await getProducts(filter);
        setProducts(responseProducts);
    }
    catch(err: unknown){
        if(err instanceof AxiosError)
            setError(err.response?.data.message)
    }
  };

  const fetchCategories = async () => {
    const responseCategories = await getCategories();
    setCategories(responseCategories);
  }

  const changePage = (next:boolean) => {
    setFilter(prev => ({ ...prev, page: next ? prev.page + 1 : prev.page - 1 }));
  }

  useEffect(() => { 
    fetchCategories();
    fetchProducts();
  }, []);

  return (
<div className="space-y-6">
  {/* Header */}
  <div className="flex flex-col sm:flex-row justify-between items-start sm:items-center gap-4">
    <div>
      <h1 className="text-3xl font-bold">Ürünler</h1>
      <p className="text-muted-foreground">
        {products ? `${products.totalCount} ürün bulundu` : "Ürünler yükleniyor..."}
      </p>
    </div>

    <Button
      variant="outline"
      onClick={() => setShowFilters(!showFilters)}
      className="flex items-center gap-2"
    >
      <Filter className="h-4 w-4" />
      Filtreler
    </Button>
  </div>

  {/* Error */}
  {error && (
    <Alert variant="destructive">
      <AlertDescription>{error}</AlertDescription>
    </Alert>
  )}

  <div className="grid grid-cols-1 lg:grid-cols-5 gap-6">
    {/* Filters (sol panel) */}
    {showFilters && (
      <div className="lg:col-span-1">
        <Card className="sticky top-4">
          <CardHeader>
            <CardTitle className="flex items-center gap-2">
              <Filter className="h-5 w-5" />
              Filtreler
            </CardTitle>
          </CardHeader>
          <CardContent>
            <form
              onSubmit={(e) => {
                e.preventDefault();
                fetchProducts();
              }}
              className="space-y-4"
            >
              {/* Inputs */}
              <div className="space-y-4">
                <div className="space-y-2">
                  <Label htmlFor="title">Başlık</Label>
                  <Input
                    id="title"
                    onChange={handleChange}
                    type="text"
                    name="title"
                    value={filter.title ?? ""}
                    placeholder="Ürün adı ara..."
                  />
                </div>

                <div className="space-y-2">
                  <Label htmlFor="category">Kategori</Label>
                  <Select value={categoryId || ""} onValueChange={setCategoryId}>
                    <SelectTrigger>
                      <SelectValue placeholder="Kategori seçin" />
                    </SelectTrigger>
                    <SelectContent>
                      {categories?.map((category) => (
                        <SelectItem key={category.id} value={category.id}>
                          {category.name}
                        </SelectItem>
                      ))}
                    </SelectContent>
                  </Select>
                </div>

                <div className="grid grid-cols-2 gap-2">
                  <div className="space-y-2">
                    <Label htmlFor="priceMin">Min</Label>
                    <Input
                      id="priceMin"
                      onChange={handleChange}
                      type="number"
                      name="priceMin"
                      step="0.01"
                      value={filter.priceMin ?? ""}
                      placeholder="0.00"
                    />
                  </div>

                  <div className="space-y-2">
                    <Label htmlFor="priceMax">Max</Label>
                    <Input
                      id="priceMax"
                      onChange={handleChange}
                      type="number"
                      name="priceMax"
                      step="0.01"
                      value={filter.priceMax ?? ""}
                      placeholder="1000.00"
                    />
                  </div>
                </div>

                <div className="space-y-2">
                  <Label htmlFor="priceFixed">Sabit Fiyat</Label>
                  <Input
                    id="priceFixed"
                    onChange={handleChange}
                    type="number"
                    name="priceFixed"
                    value={filter.priceFixed ?? ""}
                    placeholder="0.00"
                  />
                </div>
              </div>

              {/* Checkboxes */}
              <div className="flex flex-col gap-3 pt-2">
                <div className="flex items-center space-x-2">
                  <Checkbox
                    id="tradable"
                    name="tradable"
                    checked={filter.tradable ?? false}
                    onCheckedChange={(checked) =>
                      setFilter((prev) => ({ ...prev, tradable: checked as boolean }))
                    }
                  />
                  <Label htmlFor="tradable">Takaslanabilir</Label>
                </div>

                <div className="flex items-center space-x-2">
                  <Checkbox
                    id="sold"
                    name="sold"
                    checked={filter.sold ?? false}
                    onCheckedChange={(checked) =>
                      setFilter((prev) => ({ ...prev, sold: checked as boolean }))
                    }
                  />
                  <Label htmlFor="sold">Satılmış</Label>
                </div>
              </div>

              {/* Buttons */}
              <div className="flex gap-2 pt-3">
                <Button type="submit" className="flex items-center gap-2 w-full">
                  <Search className="h-4 w-4" />
                  Ara
                </Button>
                <Button
                  type="button"
                  variant="outline"
                  onClick={() => {
                    setFilter(defaultFilter);
                    setCategoryId("");
                  }}
                  className="w-full"
                >
                  Sıfırla
                </Button>
              </div>
            </form>
          </CardContent>
        </Card>
      </div>
    )}

    {/* Products */}
    <div className={`${showFilters ? "lg:col-span-4" : "lg:col-span-5"}`}>
      <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 xl:grid-cols-4 gap-6">
        {products && products.items.length > 0 ? (
          products.items.map((product) => (
            <Card
              key={product.id}
              className="overflow-hidden hover:shadow-lg transition-shadow"
            >
              <div className="aspect-square bg-muted flex items-center justify-center">
                {product.image ? (
                  <img
                    src={minioBaseUrl() + product.image}
                    alt={product.title}
                    className="w-full h-full object-cover"
                  />
                ) : (
                  <Package className="h-12 w-12 text-muted-foreground" />
                )}
              </div>
              <CardHeader className="pb-2">
                <CardTitle className="text-lg line-clamp-2">{product.title}</CardTitle>
                <CardDescription className="line-clamp-2">
                  {product.description}
                </CardDescription>
              </CardHeader>
              <CardContent className="pt-0">
                <div className="flex items-center justify-between mb-4">
                  <span className="text-2xl font-bold text-primary">
                    ₺{product.price.toFixed(2)}
                  </span>
                  {product.tradable 
                    ?
                    <Badge className="text-green-500" variant="secondary">Takas</Badge>
                    :
                    <Badge className="text-red-600" variant="secondary">Takas</Badge>}
                </div>
                <Button variant="outline" asChild className="w-full">
                  <Link to={`/details/${product.id}`}>Detayları Gör</Link>
                </Button>
              </CardContent>
            </Card>
          ))
        ) : (
          <div className="col-span-full text-center py-12">
            <Package className="h-12 w-12 text-muted-foreground mx-auto mb-4" />
            <h3 className="text-lg font-semibold mb-2">Ürün bulunamadı</h3>
            <p className="text-muted-foreground">
              Arama kriterlerinizi değiştirerek tekrar deneyin.
            </p>
          </div>
        )}
      </div>

      {/* Pagination */}
      {products && products.items.length > 0 && (
        <div className="flex items-center justify-center gap-2 mt-8">
          <Button
            variant="outline"
            size="sm"
            disabled={filter.page <= 1}
            onClick={() => changePage(false)}
          >
            <ChevronLeft className="h-4 w-4" />
            Önceki
          </Button>
          <span className="text-sm text-muted-foreground">
            Sayfa {filter.page} / {Math.ceil(products.totalCount / filter.pageSize)}
          </span>
          <Button
            variant="outline"
            size="sm"
            disabled={products.remainingCount <= 0}
            onClick={() => changePage(true)}
          >
            Sonraki
            <ChevronRight className="h-4 w-4" />
          </Button>
        </div>
      )}
    </div>
  </div>
</div>

  );
}
