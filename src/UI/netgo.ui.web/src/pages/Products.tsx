import { useEffect, useState } from "react";
import { getProducts } from "../services/productService";
import type { GetProductsDTO, ListCategoryDTO, ListProductDTO, PagedResult } from "../types/dtos";
import { AxiosError } from "axios";
import { getCategories } from "../services/categoryService";

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
  const [categoryId, setCategoryId] = useState<string | undefined>();
  const [filter, setFilter] = useState<GetProductsDTO>(defaultFilter);
  const [error, setError] = useState<string>();

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
    <>    
      <span>{error}</span>

      <h1>Filter</h1>
      <form
        onSubmit={(e) => {
          e.preventDefault();
          fetchProducts();
        }}
      >
        <label>
          Title:
          <input
            onChange={handleChange}
            type="text"
            name="title"
            value={filter.title ?? ""}
          />
        </label>

        <br />

        <label>
            Category:
            <select onChange={(e) => {setCategoryId(e.target.value)}} name="categoryId">
                <option selected>Select a category</option>
                {categories?.map((category) => (
                    <option value={category.id}>{category.name}</option>
                ))}
            </select>
        </label>
        <br />

        <label>
          PriceMin:
          <input
            onChange={handleChange}
            type="number"
            name="priceMin"
            step="0.01"
            value={filter.priceMin ?? ""}
          />
        </label>
        <br />

        <label>
          PriceMax:
          <input
            onChange={handleChange}
            type="number"
            name="priceMax"
            step="0.01"
            value={filter.priceMax ?? ""}
          />
        </label>
        <br />

        <label>           
          PriceFixed:
          <input
            onChange={handleChange}
            type="number"
            name="priceFixed"
            value={filter.priceFixed ?? 0}
          />
        </label>
        <br />

        <label>
          <input
            onChange={handleChange}
            type="checkbox"
            name="tradable"
            checked={filter.tradable ?? false}
          />
          Tradable
        </label>
        <br />

        <label>
          <input
            onChange={handleChange}
            type="checkbox"
            name="sold"
            checked={filter.sold ?? false}
          />
          Sold
        </label>
        <br />

        <button type="submit">Search</button>
        <button
          type="button"
          onClick={() => {
            setFilter(defaultFilter);
          }}
        >
          Reset
        </button>
        <br />
        <button disabled={products && products.remainingCount <= 0} onClick={() => {changePage(true)}}>Next page</button>
        <button disabled={filter.page <= 1} onClick={() => {changePage(false)}}>Previous page</button>
      </form>

      <h1>Products</h1>
      {products && products.items.length > 0 ? (
        products.items.map((product) => (
          <div key={product.id}>
            <span>{product.id} ~ {product.title}</span>
            <br />
          </div>
        ))
      ) : (
        <span>No product found</span>
      )}
    </>
  );
}
