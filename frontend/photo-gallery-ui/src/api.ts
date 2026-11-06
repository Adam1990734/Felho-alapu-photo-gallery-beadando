import type { ImageItem } from "./types";

const API_URL = "https://localhost:7214/api/images";

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