/**
 * MarkdownToDocxService.cs
 *
 * This service class is responsible for converting Markdown input into a formatted DOCX file.
 * It is used in a .NET Core Web API to support frontend requests for document generation.
 *
 * Key functionality:
 * - Converts Markdown to HTML using Markdig
 * - Parses the HTML using HtmlAgilityPack
 * - Translates HTML elements (headings, paragraphs, lists, links, formatting) into WordprocessingML
 * - Adds default styles for Heading1 and Heading2
 * - Generates and returns a DOCX file as a byte array (in memory, no file saved to disk)
 *
 * External Libraries Used:
 * - Markdig: for Markdown to HTML conversion
 * - HtmlAgilityPack: for DOM-like parsing of HTML
 * - OpenXML SDK: to programmatically build DOCX documents
 */

using System.IO;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Markdig;
using HtmlAgilityPack;

namespace MarkdownToDocxApi.Services
{
    public class MarkdownToDocxService
    {
        // Main function to convert input Markdown string to a DOCX file
        public byte[] ConvertMarkdownToDocx(string markdown)
        {
            // Create an in-memory stream to store the generated .docx file
            using var ms = new MemoryStream();

            // Create the Wordprocessing document (in-memory)
            using (var doc = WordprocessingDocument.Create(ms, WordprocessingDocumentType.Document, true))
            {
                // Add the main document part (where content lives)
                var mainPart = doc.AddMainDocumentPart();

                // Add heading styles (Heading1, Heading2)
                AddDefaultStyles(mainPart);

                // Initialize the document structure with a <body> tag
                mainPart.Document = new Document(new Body());
                var body = mainPart.Document.Body;

                // Convert Markdown to HTML using Markdig
                var html = Markdown.ToHtml(markdown);

                // Parse HTML using HtmlAgilityPack
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(html);

                // Traverse all top-level HTML elements (e.g., <h1>, <p>, <ul>)
                foreach (var node in htmlDoc.DocumentNode.ChildNodes)
                {
                    // Handle unordered lists (<ul>)
                    if (node.Name == "ul")
                    {
                        foreach (var li in node.SelectNodes("li"))
                        {
                            // Add bullet point ("• ") and convert to paragraph
                            var bulletPara = new Paragraph(
                                new Run(new Text("• " + li.InnerText)
                                {
                                    Space = SpaceProcessingModeValues.Preserve
                                }));
                            body.Append(bulletPara);
                        }
                    }
                    else
                    {
                        // Convert other HTML elements (like <p>, <h1>, <a>) to Word paragraphs
                        var paragraph = ConvertHtmlNodeToParagraph(node, mainPart);
                        if (paragraph != null)
                        {
                            body.Append(paragraph);
                        }
                    }
                }

                // Finalize and save the document
                mainPart.Document.Save();
            }

            // Return the generated document as a byte array
            return ms.ToArray();
        }

        // Converts individual HTML nodes (<h1>, <p>, <strong>, <a>, etc.) to Word paragraph blocks
        private Paragraph ConvertHtmlNodeToParagraph(HtmlNode node, MainDocumentPart mainPart)
        {
            var paragraph = new Paragraph();

            // Handle <h1> as Heading1 style
            if (node.Name == "h1")
            {
                paragraph.AppendChild(new ParagraphProperties(new ParagraphStyleId { Val = "Heading1" }));
                paragraph.Append(new Run(new Text(node.InnerText)
                {
                    Space = SpaceProcessingModeValues.Preserve
                }));
                return paragraph;
            }
            // Handle <h2> as Heading2 style
            else if (node.Name == "h2")
            {
                paragraph.AppendChild(new ParagraphProperties(new ParagraphStyleId { Val = "Heading2" }));
                paragraph.Append(new Run(new Text(node.InnerText)
                {
                    Space = SpaceProcessingModeValues.Preserve
                }));
                return paragraph;
            }
            // Handle <p> (paragraphs) and inline formatting
            else if (node.Name == "p")
            {
                bool first = true;

                // Traverse children of the <p> tag (e.g. plain text, <strong>, <em>, <a>)
                foreach (var child in node.ChildNodes)
                {
                    // Add a space between inline elements
                    if (!first)
                        paragraph.Append(new Run(new Text(" ") { Space = SpaceProcessingModeValues.Preserve }));
                    first = false;

                    if (child.Name == "#text")
                    {
                        // Handle plain text
                        paragraph.Append(new Run(new Text(child.InnerText)
                        {
                            Space = SpaceProcessingModeValues.Preserve
                        }));
                    }
                    else if (child.Name == "strong")
                    {
                        // Handle <strong> → bold text
                        paragraph.Append(new Run(
                            new RunProperties(new Bold()),
                            new Text(child.InnerText)
                            {
                                Space = SpaceProcessingModeValues.Preserve
                            }));
                    }
                    else if (child.Name == "em")
                    {
                        // Handle <em> → italic text
                        paragraph.Append(new Run(
                            new RunProperties(new Italic()),
                            new Text(child.InnerText)
                            {
                                Space = SpaceProcessingModeValues.Preserve
                            }));
                    }
                    else if (child.Name == "a")
                    {
                        // Handle <a> → hyperlinks
                        string url = child.GetAttributeValue("href", "#");

                        // Create a hyperlink relationship in the DOCX file
                        var hyperlinkRel = mainPart.AddHyperlinkRelationship(new Uri(url), true);

                        // Create the actual hyperlink element with blue underlined styling
                        var hyperlink = new Hyperlink(
                            new Run(
                                new RunProperties(
                                    new RunStyle { Val = "Hyperlink" },
                                    new Color { Val = "0000FF" }, // Blue color
                                    new Underline { Val = UnderlineValues.Single } // Underlined
                                ),
                                new Text(child.InnerText)
                                {
                                    Space = SpaceProcessingModeValues.Preserve
                                }
                            ))
                        {
                            Id = hyperlinkRel.Id
                        };

                        paragraph.Append(hyperlink);
                    }
                }

                return paragraph;
            }

            // Unsupported or unhandled element
            return null;
        }

        // Adds default Word styles (Heading1 and Heading2) to support styled headings
        private void AddDefaultStyles(MainDocumentPart mainPart)
        {
            var stylesPart = mainPart.AddNewPart<StyleDefinitionsPart>();

            stylesPart.Styles = new Styles(
                // Define Heading 1 style
                new Style(
                    new StyleName { Val = "Heading1" },
                    new BasedOn { Val = "Normal" },
                    new UIPriority { Val = 9 },
                    new PrimaryStyle(),
                    new StyleParagraphProperties(
                        new KeepNext(),
                        new KeepLines(),
                        new SpacingBetweenLines { Before = "480", After = "240" },
                        new OutlineLevel { Val = 0 }),
                    new StyleRunProperties(
                        new Bold(),
                        new FontSize { Val = "32" } // 16pt
                    )
                )
                {
                    Type = StyleValues.Paragraph,
                    StyleId = "Heading1"
                },

                // Define Heading 2 style
                new Style(
                    new StyleName { Val = "Heading2" },
                    new BasedOn { Val = "Normal" },
                    new UIPriority { Val = 9 },
                    new PrimaryStyle(),
                    new StyleParagraphProperties(
                        new KeepNext(),
                        new KeepLines(),
                        new SpacingBetweenLines { Before = "400", After = "200" },
                        new OutlineLevel { Val = 1 }),
                    new StyleRunProperties(
                        new Bold(),
                        new FontSize { Val = "28" } // 14pt
                    )
                )
                {
                    Type = StyleValues.Paragraph,
                    StyleId = "Heading2"
                }
            );
        }
    }
}
