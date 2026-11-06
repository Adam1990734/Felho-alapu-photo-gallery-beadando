import type { ImageItem } from "./types";

export const API_PORT = 7214;

const API_URL = `https://localhost:${API_PORT}/api/images`;

export async function getImages(): Promise<ImageItem[]> {
    const response = await fetch(API_URL);

    if (!response.ok) {
        throw new Error("Failed to load images");
    }

    return response.json();
}

export async function createImage(
    title: string,
    description: string,
    fileName: string
) {
    const response = await fetch(API_URL, {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify({
            title,
            description,
            fileName
        })
    });

    if (!response.ok) {
        throw new Error("Failed to create image");
    }

    return response.json();
}

export async function uploadImage(
  title: string,
  description: string,
  file: File
) {
  const formData = new FormData();

  formData.append("title", title);
  formData.append("description", description);
  formData.append("file", file);

  const response = await fetch(
    `https://localhost:${API_PORT}/api/images/upload`,
    {
      method: "POST",
      body: formData
    });

  if (!response.ok) {
    throw new Error("Upload failed");
  }

  return response.json();
}