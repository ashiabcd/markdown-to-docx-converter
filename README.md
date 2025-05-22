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
| Frontend   | React, Vite, Axios, docx-preview |
| Backend    | .NET 8 Web API, OpenXML SDK, Markdig, HtmlAgilityPack |
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

## Assumptions & Scope

- Supports only basic Markdown syntax:
  - Headings (`#`, `##`)
  - Bold (`**text**`)
  - Italic (`*text*`)
  - Unordered lists (`- item`)
  - Links (`[text](url)`)
- Does **not** support:
  - Tables
  - Images
  - Code blocks
  - Raw HTML
- Designed for local development with **CORS enabled** for convenience
- Assumes **valid, well‑formed Markdown** input




##  Demo Test Cases

| # | Test Case             | Markdown Input                                                                                                                                                                                                                                                   | What to Verify                                                                                                   |
|---|-----------------------|-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|------------------------------------------------------------------------------------------------------------------|
| 1 | Simple Heading        | `# Welcome to My Doc`                                                                                                                                                                                                                                             | Preview shows a large bold Heading 1; downloaded DOCX has Heading 1 style.                                       |
| 2 | Bold & Italic         | `This is **bold** text and this is *italic* text.`                                                                                                                                                                                                                 | “bold” is bold and “italic” is italic in both the preview and downloaded DOCX.                                   |
| 3 | Bullet List           | `- First item<br>- Second item<br>- Third item`                                                                                                                                                                                                                   | Preview shows `• First item`, `• Second item`, `• Third item`; downloaded DOCX uses Word’s bullet style.       |
| 4 | Hyperlink             | `Go to [Google](https://www.google.com) for more info.`                                                                                                                                                                                                           | Link text is blue and underlined in the preview; clicking in the downloaded DOCX opens Google.                  |
| 5 | Mixed Formatting      | `## Features<br>- **Fast** conversion<br>- *Easy* to use<br>- [Docs](https://example.com)`                                                                                                                                                                         | Heading 2 renders correctly; first list item is bold; second is italic; third is a clickable hyperlink.         |
| 6 | Empty Input           | *(leave textarea blank)*                                                                                                                                                                                                                                           | App shows an error “Markdown content is empty.” and does not attempt conversion.                                |
| 7 | Comprehensive Example | `# Project Title<br><br>Welcome to **Markdown to DOCX Converter**. It is *fast* and easy.<br><br>## Key Features<br>- Live **conversion** of Markdown to DOCX  <br>- In‑browser *preview*  <br>- One‑click download<br><br>Learn more on [GitHub](https://github.com/ashiabcd/markdown-to-docx-converter).` | - Heading 1 renders as large bold title<br>- Bold and italic formatting applied correctly<br>- Bullet list shows “•”<br>- Hyperlink is underlined blue and clickable. |

---

**How to run these tests:**

1. Start your backend (`dotnet run`) and frontend (`npm run dev`).  
2. Open `http://localhost:5173/` in your browser.  
3. For each test case above:  
   - **Copy** the **Markdown Input** (without backticks or fences) into the textarea.  
   - Click **Convert to DOCX**.  
   - Verify the live preview and download/open the DOCX file match the **What to Verify** column.



