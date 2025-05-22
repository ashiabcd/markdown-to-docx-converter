# Markdown to DOCX Converter

This is a full-stack web application that allows users to input Markdown text and convert it into a downloadable and previewable `.docx` Word document.

It uses a **React-based frontend** and a **.NET Core Web API backend** that leverages the **OpenXML SDK** to generate the Word document programmatically. The app is designed for simplicity and functionality — supporting essential formatting elements such as headings, paragraphs, bold and italic text, bullet lists, and hyperlinks.

---

## Architecture Overview

The application follows a classic client-server architecture:

```plaintext
markdown-to-docx-converter/
├── markdown-to-docx-frontend/   → React app (user interface, preview, download)
└── MarkdownToDocxApi/           → .NET Core Web API (handles conversion logic)
```

### Flow

1. User types Markdown in the frontend.
2. Frontend sends a POST request to the backend API.
3. Backend converts **Markdown → HTML → DOCX**.
4. Backend returns the `.docx` file.
5. Frontend renders the preview and enables file download.

---

## Tools & Libraries Used

| Layer      | Technologies                                                |
|------------|-------------------------------------------------------------|
| Frontend   | React, Vite, Axios, [`docx-preview`](https://www.npmjs.com/package/docx-preview) |
| Backend    | .NET 8 Web API, [OpenXML SDK](https://github.com/OfficeDev/Open-XML-SDK), [Markdig](https://github.com/lunet-io/markdig), [HtmlAgilityPack](https://html-agility-pack.net/) |
| Interop    | REST API (`POST /api/markdown/convert`)                    |

---

## Setup Instructions

### 1. Clone the Repository

```bash
git clone https://github.com/ashiabcd/markdown-to-docx-converter.git
cd markdown-to-docx-converter
```

### 2. Start the Backend

```bash
cd MarkdownToDocxApi
dotnet restore
dotnet run
```

- Backend runs at: `https://localhost:5243/`  
- API endpoint: `POST /api/markdown/convert`

### 3. Start the Frontend

```bash
cd ../markdown-to-docx-frontend
npm install
npm run dev
```

- Frontend runs at: `http://localhost:5173/`  
- Sends POST requests to the backend to convert Markdown to DOCX.

---


