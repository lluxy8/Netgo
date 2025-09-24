import { ProductCarousel } from "@/components/ProductCarousel";
import type { ProductWithOwnerDTO } from "@/types/dtos";
import { useLoaderData  } from "react-router-dom"
import { Image } from "@/components/Image";
import { Button } from "@/components/ui/button";
import { Table } from "@/components/Table";

function Detail() {
    const response = useLoaderData() as ProductWithOwnerDTO;
    
    return (
     <>
        <div className="grid grid-cols-1 md:grid-cols-3 gap-10 flex-none items-center">
            <div className="rounded-lg h-90 md:col-span-2 w-full bg-gray-100 flex justify-center">
                <ProductCarousel images={response.productImages} />
            </div>
            <div className="rounded-lg w-full bg-gray-100 p-5 max-h-50 flex justify-center items-center">
                <div className="grid grid-cols-2">
                    <div>
                        <Image 
                            placeholderImage 
                            className="w-20 h-20 rounded-full" 
                            src={response.ownerAvatar} alt="asd" 
                        />
                        <h1 className="font-semibold mt-2">{response.ownerName}</h1>
                        <span className="text-sm text-gray-600">{response.ownerLocation}</span>
                    </div>
                    <div className="my-2 px-3 flex justify-end">
                        <Button variant={"destructive"}>Contact with seller</Button>
                    </div>
                </div>
            </div>
            <div className="bg-gray-100 w-full col-span-2 rounded-lg p-4">
                <h1 className="text-xl font-semibold">{response.productTitle}</h1>
                <p className="text-gray-600">{response.productDescription}</p>

                {response.productDetails.length <= 0 
                ? <p className="text-sm text-gray-600 mt-8">Product has no details</p>
                : <Table keyValues={response.productDetails} />}
            </div>
        </div>
     </>
    )
}

export default Detail