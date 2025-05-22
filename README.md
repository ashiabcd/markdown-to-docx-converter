#  Markdown to DOCX Converter

This is a full-stack web application that allows users to input Markdown text and convert it into a downloadable and previewable `.docx` Word document.

It uses a **React-based frontend** and a **.NET Core Web API backend** that leverages the **OpenXML SDK** to generate the Word document programmatically. The app is designed for simplicity and functionality — supporting essential formatting elements such as headings, paragraphs, bold and italic text, bullet lists, and hyperlinks.

---
##  Architecture Overview

The application follows a classic client-server architecture:
markdown-to-docx-converter/
├── markdown-to-docx-frontend/ → React app (user interface, preview, download)
└── MarkdownToDocxApi/ → .NET Core Web API (handles conversion logic)

###  Flow:

1. User types Markdown in the frontend.
2. Frontend sends a POST request to the backend API.
3. Backend converts Markdown → HTML → DOCX.
4. Backend returns the `.docx` file.
5. Frontend renders the preview and enables file download.

##  Tools & Libraries Used

| Layer      | Technologies                                                |
|------------|-------------------------------------------------------------|
| Frontend   | React, Vite, Axios, `docx-preview`                          |
| Backend    | .NET 8 Web API, OpenXML SDK, Markdig, HtmlAgilityPack       |
| Interop    | REST API (`POST /api/markdown/convert`)                    |

##  Setup Instructions

1. Clone the Repository

```bash
git clone https://github.com/ashiabcd/markdown-to-docx-converter.git
cd markdown-to-docx-converter

2. Start the backend

```bash
cd MarkdownToDocxApi
dotnet restore
dotnet run

- The backend runs on: https://localhost:5243/
- Exposes an endpoint: POST /api/markdown/convert

3. Start the frontend

```bash
cd markdown-to-docx-frontend
npm install
npm run dev

- The frontend runs on: http://localhost:5173/
- Sends POST requests to the backend endpoint to convert Markdow

   
