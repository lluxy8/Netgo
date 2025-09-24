import { cn } from "@/lib/utils";
import { minioBaseUrl } from "@/Services/minioClient";
import imgnotfoundimg from '../assets/imgnotfound.jpg'
import blackimg from '../assets/black.png'


interface ImageProps{
    src: string
    alt: string
    placeholderImage?: boolean
    className: string
}

export const Image: React.FC<ImageProps> = ({
    src,
    alt,
    placeholderImage = false,
    className
}) => {
    return <img
          src={placeholderImage ? blackimg : minioBaseUrl + src}
          alt={alt}
          className={cn(className)}
          onError={(e) => {
            (e.target as HTMLImageElement).src = imgnotfoundimg;
          }}></img>
}