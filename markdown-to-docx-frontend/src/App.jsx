import React, { useState, useEffect, useRef } from 'react'; 
import axios from 'axios';
import { renderAsync } from 'docx-preview';

/**
 * App.jsx — Markdown to DOCX Converter Frontend
 *
 * This React component provides a complete frontend UI for converting
 * Markdown input into a downloadable and previewable DOCX file.
 *
 * Features:
 * - Accepts Markdown input via a textarea.
 * - Sends it to a .NET Core backend to generate a DOCX.
 * - Shows loading state and error handling.
 * - Renders live preview using docx-preview.
 * - Offers a toggle for animated background.
 */

export default function App() {
  // State to hold the Markdown input from the user
  const [markdown, setMarkdown] = useState('');

  // Holds the returned .docx file buffer
  const [docBuffer, setDocBuffer] = useState(null);

  // Shows loading state while converting
  const [loading, setLoading] = useState(false);

  // Shows preview if conversion was successful
  const [showPreview, setShowPreview] = useState(false);

  // Error message if backend fails
  const [error, setError] = useState(null);

  // Toggles background animation
  const [animate, setAnimate] = useState(true);

  // Ref for the preview rendering container
  const previewRef = useRef();

  /**
   * Trigger Markdown → DOCX conversion
   * Sends the markdown input to the backend and sets the returned buffer
   */
  const handleConvert = async () => {
    setLoading(true);
    setShowPreview(false);
    setError(null);
    setDocBuffer(null);

    const start = Date.now();
    try {
      const { data } = await axios.post(
        'http://localhost:5243/api/markdown/convert',
        { markdown },
        { responseType: 'arraybuffer' }
      );
      setDocBuffer(data);
    } catch {
      setError('Could not generate document. Please try again.');
    } finally {
      const delay = Math.max(0, 800 - (Date.now() - start));
      setTimeout(() => {
        setLoading(false);
        setShowPreview(true);
      }, delay);
    }
  };

  /**
   * When the DOCX buffer updates and preview is toggled on,
   * render the DOCX into the preview container using docx-preview
   */
  useEffect(() => {
    if (showPreview && docBuffer && previewRef.current) {
      previewRef.current.innerHTML = '';
      renderAsync(docBuffer, previewRef.current).catch(console.error);
    }
  }, [showPreview, docBuffer]);

  return (
    <>
      {/* 
        Full-page animated or static background 
        - controlled by `animate` checkbox 
      */}
      <div
        style={{
          position: 'fixed',
          inset: 0,
          zIndex: -1,
          background: animate
            ? 'linear-gradient(-45deg, #ff6ec4, #7873f5, #42e695, #f9f871)'
            : '#f3f4f6',
          backgroundSize: '400% 400%',
          animation: animate ? 'gradientShift 15s ease infinite' : 'none',
        }}
      />

      {/* 
        Grid layout to center the content card in the viewport 
        Responsive, clean layout
      */}
      <div
        style={{
          width: '100vw',
          height: '100vh',
          display: 'grid',
          placeItems: 'center',
          padding: '24px',
          boxSizing: 'border-box'
        }}
      >
        {/* 
          Inner white card (main content area) 
        */}
        <div
          style={{
            width: '100%',
            maxWidth: '640px',
            background: 'white',
            borderRadius: '12px',
            boxShadow: '0 6px 18px rgba(0,0,0,0.1)',
            padding: '24px',
            display: 'flex',
            flexDirection: 'column',
            gap: '16px',
          }}
        >
          {/* App Title */}
          <h1 style={{ fontSize: '2rem', fontWeight: 700, textAlign: 'center' }}>
            Markdown → DOCX Converter
          </h1>

          {/* Textarea for user Markdown input */}
          <textarea
            style={{
              width: '100%',
              height: '180px',
              resize: 'none',
              padding: '12px',
              fontSize: '1rem',
              borderRadius: '6px',
              border: '1px solid #d1d5db',
            }}
            placeholder="Enter Markdown here…"
            value={markdown}
            onChange={(e) => setMarkdown(e.target.value)}
          />

          {/* Convert Button (disabled while loading) */}
          <button
            onClick={handleConvert}
            disabled={loading}
            style={{
              padding: '10px 20px',
              fontSize: '1rem',
              background: loading ? '#9ca3af' : '#2563eb',
              color: 'white',
              border: 'none',
              borderRadius: '6px',
              cursor: loading ? 'not-allowed' : 'pointer',
            }}
          >
            {loading ? 'Generating…' : 'Convert to DOCX'}
          </button>

          {/* Loading status message */}
          {loading && (
            <p
              style={{
                textAlign: 'center',
                color: '#2563eb',
                fontWeight: 500,
                animation: 'pulse 1s infinite',
              }}
            >
              ⏳ Generating the document…
            </p>
          )}

          {/* Display error message if conversion fails */}
          {error && (
            <p style={{ textAlign: 'center', color: '#dc2626', fontWeight: 500 }}>
              {error}
            </p>
          )}

          {/* DOCX preview and download link */}
          {showPreview && docBuffer && !loading && (
            <>
              <h2 style={{ textAlign: 'center', fontSize: '1.25rem' }}>Preview</h2>
              <div
                ref={previewRef}
                style={{
                  width: '100%',
                  height: '220px',
                  overflow: 'auto',
                  border: '1px solid #d1d5db',
                  borderRadius: '6px',
                  padding: '8px',
                  background: 'white',
                }}
              />
              {/* Download link for the generated DOCX file */}
              <a
                href={URL.createObjectURL(
                  new Blob([docBuffer], {
                    type: 'application/vnd.openxmlformats-officedocument.wordprocessingml.document',
                  })
                )}
                download="output.docx"
                style={{
                  textAlign: 'center',
                  display: 'block',
                  marginTop: '8px',
                  color: '#2563eb',
                  textDecoration: 'underline',
                }}
              >
                Download DOCX
              </a>
            </>
          )}

          {/* Checkbox to toggle background animation */}
          <div style={{ textAlign: 'center', marginTop: '12px' }}>
            <label>
              <input
                type="checkbox"
                checked={animate}
                onChange={() => setAnimate(!animate)}
                style={{ marginRight: '8px' }}
              />
              Animate Background
            </label>
          </div>
        </div>
      </div>

      {/* Keyframe animations for background gradient and loading pulse */}
      <style>
        {`
          @keyframes gradientShift {
            0% { background-position: 0% 50%; }
            50% { background-position: 100% 50%; }
            100% { background-position: 0% 50%; }
          }
          @keyframes pulse {
            0% { opacity: 1; }
            50% { opacity: 0.4; }
            100% { opacity: 1; }
          }
        `}
      </style>
    </>
  );
}
