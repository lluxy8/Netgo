"use client";

import { useEffect, useState } from "react"
import ProductCard from "../components/ProductCard"
import { getProducts } from "../Services/productService";
import type { GetProductsDTO, ListProductDTO } from "../types/dtos";
import { minioBaseUrl } from "../Services/minioClient";
import { SkeletonCard } from "../components/SkeletonCard";
import { useNavigate } from "react-router-dom";

function Home() {
  const [products, setProducts] = useState<Array<ListProductDTO>>([]);
  const [loading, setLoading] = useState(true);

  const filter: GetProductsDTO = {page: 1, pageSize: 8};
  useEffect(() => {
    setLoading(true);
    setTimeout(() => {
      getProducts(filter)
        .then(setProducts)
        .finally(() => setLoading(false));
    }, 1);
  }, []);

  const navigate = useNavigate();

  const goToDetail = (id:string) => {
    navigate("/details/" + id);
  };

  return (
    <>
      <div className="grid grid-cols-2 md:grid-cols-4 gap-4">
        {loading
          ? Array.from({ length: filter.pageSize }).map((_, i) => (
              <SkeletonCard key={i} />
            ))
          : products.map((c: ListProductDTO) => (
              <ProductCard
                key={c.id} 
                imageUrl={minioBaseUrl + c.image}
                title={c.title}
                description={c.description}
                price={c.price}
                onDetailsClick={() => {goToDetail(c.id)}}
              />
            ))}
      </div>
    </>
  )
}

export default Home
