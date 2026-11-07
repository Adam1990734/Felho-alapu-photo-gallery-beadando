import { useEffect, useRef, useState } from "react";
import { uploadImage, getImages, deleteImage } from "./api";
import type { ImageItem } from "./types";

function App() {

  const [images, setImages] = useState<ImageItem[]>([]);

  const [title, setTitle] = useState("");
  const [description, setDescription] = useState("");
  const fileInputRef = useRef<HTMLInputElement>(null);
  const [file, setFile] = useState<File | null>(null);

  const loadImages = async () => {
    const data = await getImages();
    setImages(data);
  };

  const handleDelete = async (
    id: string
  ) => {

    if (
      !window.confirm(
        "Biztosan törölni szeretnéd?"
      )
    ) {
      return;
    }

    await deleteImage(id);

    await loadImages();
  };

  useEffect(() => {
    loadImages();
  }, []);

  const handleSubmit = async () => {
    if (!file)
    {
        alert("Select a file");
        return;
    }

    await uploadImage(
        title,
        description,
        file
    );

    setTitle("");
    setDescription("");
    setFile(null);
    if (fileInputRef.current)
      fileInputRef.current.value = "";

    await loadImages();
  };

  return (
    <div className="container">
      <h1>Photo Gallery</h1>

      <div className="upload-card">

        <h2>New Image</h2>

        <input
          placeholder="Title"
          value={title}
          onChange={(e) => setTitle(e.target.value)}
        />

        <input
          placeholder="Description"
          value={description}
          onChange={(e) => setDescription(e.target.value)}
        />

        <input
          ref={fileInputRef}
          type="file"
          onChange={(e) =>
            setFile(
              e.target.files?.[0] ?? null
            )
          }
        />

        <button
          onClick={handleSubmit}
          disabled={!file}
        >
          Upload
        </button>

      </div>

      <h2 className="gallery-title">
        Images
      </h2>

      <div className="gallery">

        {images.map(image => (
          <div
            className="image-card"
            key={image.id}
          >
            <img
              src={image.imageUrl}
              alt={image.title}
            />

            <div className="card-content">

              <h3>{image.title}</h3>

              <p>{image.description}</p>

              <button
                className="delete-btn"
                onClick={() =>
                  handleDelete(image.id)
                }
              >
                Törlés
              </button>
            </div>
          </div>
        ))}

      </div>
    </div>
  );
}

export default App
