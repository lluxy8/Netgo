import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import type { ProductDTO } from "../types/dtos";
import { getProductById } from "../services/productService";

export function DetailsPage() {
    const { id } = useParams()
    const [product, setProduct] = useState<ProductDTO>();
    const navigate = useNavigate();
    useEffect(() => {
        const fetchProduct = async () => {
            try{
                if(!id)
                    navigate("/notfound")

                const response = await getProductById(id!);
                setProduct(response);
            }
            catch{
                //navigate("/notfound")
            }
        }

        fetchProduct();
    }, [navigate, id])

    return(
        <>
            {product?.id} ~ {product?.title}
        </>
    )
}