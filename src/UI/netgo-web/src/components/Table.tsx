import type { ProductDetailDto } from "@/types/dtos"

interface TableProps{
    keyValues: ProductDetailDto[]
}

export const Table: React.FC<TableProps> = ({
    keyValues
}) => {
  return (
    <div className="my-6 w-full overflow-y-auto">
      <table className="w-full">
        <tbody>
          {keyValues.map((detail, index) => (
            <tr className="even:bg-muted m-0 border-t p-0" key={index}>
                <td className="border px-4 py-2 text-left [&[align=center]]:text-center [&[align=right]]:text-right">
                    {detail.title}
                </td>
                <td className="border px-4 py-2 text-left [&[align=center]]:text-center [&[align=right]]:text-right">
                    {detail.value}
                </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  )
}
