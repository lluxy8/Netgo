import { getMyProducts } from "@/services/productService";
import { useEffect } from "react";

export default function MyProductsPage()
{
    useEffect( () => {
        const fetchProducts = async () => {
            await getMyProducts();
        }

        fetchProducts();
    }, [])
    return(
        <>

        </>
    );
}