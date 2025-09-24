'use client'

import { useState } from 'react'

import { HeartIcon } from 'lucide-react'

import { Button } from '@/components/ui/button'
import { Card, CardHeader, CardTitle, CardFooter, CardContent } from '@/components/ui/card'

import { cn } from '@/lib/utils'

import { ArrowRight } from 'lucide-react'


interface ProductCardProps {
  imageUrl: string;
  title: string; 
  description: string;
  price: string | number;
  onDetailsClick?: () => void; 
}

const ProductCard: React.FC<ProductCardProps> = ({
  imageUrl,
  title,
  description,
  price,
  onDetailsClick,
}) => {
  const [liked, setLiked] = useState<boolean>(false);

  return (
    <div className="relative max-w-md rounded-xl bg-gray-300 pt-0 shadow-lg">
      <div className="flex h-60 items-center justify-center">
        <img
          src={imageUrl}
          alt={title}
          className="w-75"
          onError={(e) => {
            (e.target as HTMLImageElement).src = "https://dummyimage.com/500x500/ffffff/000000&text=Image+not+found";
          }}
        />
      </div>
      <Button
        size="icon"
        onClick={() => setLiked(!liked)}
        className="bg-primary/10 hover:bg-primary/20 absolute top-4 right-4 rounded-full"
      >
        <HeartIcon
          className={cn(
            "size-4",
            liked ? "fill-destructive stroke-destructive" : "stroke-white"
          )}
        />
        <span className="sr-only">Like</span>
      </Button>
      <Card className="border-none">
        <CardHeader>
          <CardTitle>{title}</CardTitle>
        </CardHeader>
        <CardContent>
          <p>{description}</p>
        </CardContent>
        <CardFooter className="justify-between gap-3 max-sm:flex-col max-sm:items-stretch">
          <div className="flex flex-col">
            <span className="text-sm font-medium uppercase">Price</span>
            <span className="text-xl font-semibold">
              {typeof price === "number" ? `$${price.toFixed(2)}` : price}
            </span>
          </div>
          <Button size="lg" onClick={onDetailsClick}>
            Details <ArrowRight />
          </Button>
        </CardFooter>
      </Card>
    </div>
  );
};

export default ProductCard;
