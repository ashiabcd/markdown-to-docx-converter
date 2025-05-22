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




## Demo Test Cases

| # | Test Case             | What to Paste into App | What to Verify |
|---|-----------------------|------------------------|----------------|
| 1 | Simple Heading        | `# Welcome to My Doc` | Heading 1 renders large and bold; DOCX uses Heading1 style. |
| 2 | Bold & Italic         | `This is **bold** and *italic* text.` | Bold and italic formatting appears correctly. |
| 3 | Bullet List           | `- First item\n- Second item\n- Third item` | Each item is a bullet (`•`) in preview and DOCX. |
| 4 | Hyperlink             | `Go to [Google](https://www.google.com)` | Text is underlined/blue and clickable in DOCX. |
| 5 | Mixed Formatting      | `## Features\n- **Fast** conversion\n- *Easy* to use\n- [Docs](https://example.com)` | Heading 2, bold, italic, and hyperlink all render as expected. |
| 6 | Empty Input           | *(leave blank)* | App shows error: “Markdown content is empty.” |
| 7 | Comprehensive Example | See below ↓ | All key features tested in one block. |

<details>
<summary><strong>Comprehensive Example (Test 7 Input)</strong></summary>

```markdown
# Project Title

Welcome to **Markdown to DOCX Converter**. It is *fast* and easy.

## Key Features

- Live **conversion** of Markdown to DOCX  
- In‑browser *preview*  
- One‑click download

Learn more on [GitHub](https://github.com/ashiabcd/markdown-to-docx-converter).

