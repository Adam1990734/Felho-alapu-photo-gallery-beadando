import { useEffect, useState } from "react";
import { createImage, getImages } from "./api";
import type { ImageItem } from "./types";

function App() {

  const [images, setImages] = useState<ImageItem[]>([]);

  const [title, setTitle] = useState("");
  const [description, setDescription] = useState("");
  const [fileName, setFileName] = useState("");

  const loadImages = async () => {
    const data = await getImages();
    setImages(data);
  };

  useEffect(() => {
    loadImages();
  }, []);

  const handleSubmit = async () => {

    await createImage(
      title,
      description,
      fileName
    );

    setTitle("");
    setDescription("");
    setFileName("");

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
        placeholder="File Name"
        value={fileName}
        onChange={(e) => setFileName(e.target.value)}
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

          <small>{image.fileName}</small>

          <hr />
        </div>
      ))}
    </>
  );
}

export default App
