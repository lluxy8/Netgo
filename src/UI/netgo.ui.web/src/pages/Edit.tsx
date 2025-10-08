import { useEffect, useState } from "react";
import { getProductById, updateProduct } from "../services/productService";
import type { UpdateProductDTO, ProductDetailDto } from "../types/dtos";
import { useNavigate, useParams } from "react-router-dom";

export default function EditPage() {
  const [form, setForm] = useState<UpdateProductDTO>;
  const [detailTitle, setDetailTitle] = useState("");
  const [detailValue, setDetailValue] = useState("");
  const { id } = useParams();
  const navigate = useNavigate();

  function handleChange(e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) {
    const target = e.target as HTMLInputElement;
    const { name, value, type, checked } = target;

    setForm((prev) => ({
      ...prev,
      [name]: type === "checkbox" ? checked : value,
    }));
  }

  function handleAddDetail() {
    if (!detailTitle || !detailValue) return;
    const newDetail: ProductDetailDto = { title: detailTitle, value: detailValue };
    setForm((prev) => ({
      ...prev,
      details: [...prev.details, newDetail],
    }));
    setDetailTitle("");
    setDetailValue("");
  }

  function handleRemoveDetail(index: number) {
    setForm((prev) => ({
      ...prev,
      details: prev.details.filter((_, i) => i !== index),
    }));
  }

  function handleFiles(e: React.ChangeEvent<HTMLInputElement>) {
    const files = e.target.files;
    if (!files) return;
    setForm((prev) => ({
      ...prev,
      newImages: Array.from(files),
    }));
  }

  async function handleSubmit(e: React.FormEvent) {
    e.preventDefault();
    try {
      const result = await updateProduct(form);
      console.log("Ürün güncellendi:", result);
      alert("Ürün başarıyla güncellendi!");
    } catch (err) {
      console.error("Hata:", err);
      alert("Ürün güncellenemedi!");
    }
  }

  useEffect(() => {
    const fethProduct = async () => {
        if(!id)
            navigate("/ntofound");
        
        const response = await getProductById(id!);
        setForm(response);
    }

    fethProduct();
  });

  return (
    <form onSubmit={handleSubmit}>
      <div>
        <label>Kategori:</label>
        <input
          name="categoryId"
          value={form.categoryId}
          onChange={handleChange}
          required
        />
      </div>

      <div>
        <label>Takaslanabilir mi?</label>
        <input
          type="checkbox"
          name="tradable"
          checked={form.tradable}
          onChange={handleChange}
        />
      </div>

      <div>
        <label>Ürün Detayları:</label>
        <div>
          <input
            placeholder="Detay başlığı"
            value={detailTitle}
            onChange={(e) => setDetailTitle(e.target.value)}
          />
          <input
            placeholder="Detay değeri"
            value={detailValue}
            onChange={(e) => setDetailValue(e.target.value)}
          />
          <button type="button" onClick={handleAddDetail}>Ekle</button>
        </div>
        <ul>
          {form.details.map((d, i) => (
            <li key={i}>
              {d.title}: {d.value}{" "}
              <button type="button" onClick={() => handleRemoveDetail(i)}>Sil</button>
            </li>
          ))}
        </ul>
      </div>

      <div>
        <label>Mevcut Resimler:</label>
        <ul>
          {form.images.map((img, i) => (
            <li key={i}>{img}</li>
          ))}
        </ul>
      </div>

      <div>
        <label>Yeni Resimler:</label>
        <input type="file" multiple onChange={handleFiles} />
        <ul>
          {form.newImages.map((file, i) => (
            <li key={i}>{file.name}</li>
          ))}
        </ul>
      </div>

      <button type="submit">Güncelle</button>
    </form>
  );
}
