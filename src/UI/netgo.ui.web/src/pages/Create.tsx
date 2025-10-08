import { useEffect, useState } from "react";
import { createProduct } from "../services/productService";
import type { CreateProductDTO, ListCategoryDTO, ProductDetailDto } from "../types/dtos";
import { useNavigate } from "react-router-dom";
import { getUserById } from "../services/UserService";
import { getCategories } from "../services/categoryService"; 

export default function CreatePage() {
  const navigate = useNavigate();

  const [form, setForm] = useState<CreateProductDTO>({
    userId: "",
    categoryId: "",
    title: "",
    description: "",
    tradable: false,
    price: 0,
    details: [],
    images: [],
  });

  const [detailTitle, setDetailTitle] = useState("");
  const [detailValue, setDetailValue] = useState("");
  const [categories, setCategories] = useState<ListCategoryDTO[]>([]);

  function handleChange(e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement | HTMLSelectElement>) {
    const target = e.target;

    if (target instanceof HTMLInputElement) {
      const { name, value, type, checked } = target;
      setForm((prev) => ({
        ...prev,
        [name]: type === "checkbox" ? checked : value,
      }));
    } else {
      const { name, value } = target;
      setForm((prev) => ({
        ...prev,
        [name]: value,
      }));
    }
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
      images: Array.from(files),
    }));
  }

  async function handleSubmit(e: React.FormEvent) {
    e.preventDefault();
    try {
      await createProduct(form);
      navigate("/"); 
    } catch {
        //navigate("/login");
    }
  }

  useEffect(() => {
    (async () => {
      const uid = localStorage.getItem("uid");
      if (!uid) {
        navigate("/login");
        return;
      }

      try {
        const user = await getUserById(uid);
        if (!user) {
          navigate("/login");
          return;
        }

        setForm((prev) => ({ ...prev, userId: uid }));

        const categoryList = await getCategories();
        setCategories(categoryList);
      } catch (err) {
        console.error("Veri yüklenemedi:", err);
      }
    })();
  }, [navigate]);

  return (
    <form onSubmit={handleSubmit}>
      <div>
        <label>Kategori:</label>
        <select
          name="categoryId"
          value={form.categoryId}
          onChange={handleChange}
          required
        >
          <option value="">Kategori seç</option>
          {categories.map((c) => (
            <option key={c.id} value={c.id}>
              {c.name}
            </option>
          ))}
        </select>
      </div>

      <div>
        <label>Başlık:</label>
        <input
          name="title"
          value={form.title}
          onChange={handleChange}
          required
        />
      </div>

      <div>
        <label>Açıklama:</label>
        <textarea
          name="description"
          value={form.description}
          onChange={handleChange}
          required
        />
      </div>

      <div>
        <label>Fiyat:</label>
        <input
          type="number"
          name="price"
          value={form.price}
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
          <button type="button" onClick={handleAddDetail}>
            Ekle
          </button>
        </div>
        <ul>
          {form.details.map((d, i) => (
            <li key={i}>
              {d.title}: {d.value}{" "}
              <button type="button" onClick={() => handleRemoveDetail(i)}>
                Sil
              </button>
            </li>
          ))}
        </ul>
      </div>

      <div>
        <label>Resimler:</label>
        <input type="file" multiple onChange={handleFiles} />
        <ul>
          {form.images.map((file, i) => (
            <li key={i}>{file.name}</li>
          ))}
        </ul>
      </div>

      <button type="submit">Ürünü Oluştur</button>
    </form>
  );
}
