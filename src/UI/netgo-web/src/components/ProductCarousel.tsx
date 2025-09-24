import { Card, CardContent } from "@/components/ui/card"
import {
  Carousel,
  CarouselContent,
  CarouselItem,
  CarouselNext,
  CarouselPrevious,
} from "@/components/ui/carousel"
import { minioBaseUrl } from "@/Services/minioClient"

interface CarouselProps{
    images: string[]
}

export const ProductCarousel: React.FC<CarouselProps> = ({
    images
}) =>  {
  return (
    <Carousel className="w-full h-full">
      <CarouselContent>
        {images.map((image, index) => (
          <CarouselItem key={index}>
            <div className="p-1">
              <Card className="p-0 h-90">
                <CardContent className="flex justify-center align-center h-full p-0">
                  <img src={minioBaseUrl + image} />
                </CardContent>
              </Card>
            </div>
          </CarouselItem>
        ))}
      </CarouselContent>
      <CarouselPrevious />
      <CarouselNext />
    </Carousel>
  )
}
