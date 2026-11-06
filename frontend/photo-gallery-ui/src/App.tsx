import { useEffect, useRef, useState } from "react";
import { uploadImage, getImages, API_PORT } from "./api";
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
    <>
      <h1>Photo Gallery</h1>

      <hr />

      <h2>New Image</h2>

      <input
        placeholder="Title"
        value={title}
        onChange={(e) => setTitle(e.target.value)}
      />

      <br />
      <br />

      <input
        placeholder="Description"
        value={description}
        onChange={(e) => setDescription(e.target.value)}
      />

      <br />
      <br />

      <input
        ref={fileInputRef}
        type="file"
        onChange={(e) =>
          setFile(
            e.target.files?.[0] ?? null
          )
        }
      />

      <br />
      <br />

      <button onClick={handleSubmit}>
        Save
      </button>

      <hr />

      <h2>Images</h2>

      {images.map(image => (
        <div key={image.id}>
          <h3>{image.title}</h3>

          <p>{image.description}</p>

          <img
            src={`https://localhost:${API_PORT}/uploads/${image.fileName}`}
            alt={image.title}
            width={300}
          />

          <hr />
        </div>
      ))}
    </>
  );
}

export default App
