import type { ImageItem } from "./types";

export const API_PORT = 8080;

export const API_URL = "";

export async function getImages(): Promise<ImageItem[]> {
    const response = await fetch(`${API_URL}/api/images`);

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
    const response = await fetch(`${API_URL}/api/images`, {
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
    `${API_URL}/api/images/upload`,
    {
      method: "POST",
      body: formData
    });

  if (!response.ok) {
    throw new Error("Upload failed");
  }

  return response.json();
}

export async function deleteImage(
  id: string
) {
  const response = await fetch(
    `${API_URL}/api/images/${id}`,
    {
      method: "DELETE"
    });

  if (!response.ok) {
    throw new Error("Failed to delete image");
  }
}