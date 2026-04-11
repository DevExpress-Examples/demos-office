using DevExpress.XtraRichEdit.API.Native;

namespace BlazorDemo.OfficeFileApi
{
    public static class SubDocumentHelper
    {
        public delegate void SubDocumentDelegate(SubDocument subDocument);
        public static void ForEachSubDocument(this Document document, SubDocumentDelegate subDocumentProcessor)
        {
            subDocumentProcessor(document);
            ProcessFootnotes(document.Footnotes, subDocumentProcessor);
            ProcessEndnotes(document.Endnotes, subDocumentProcessor);
            ProcessShapes(document.Shapes, subDocumentProcessor);
            ProcessComments(document.Comments, subDocumentProcessor);
            foreach (Section section in document.Sections)
            {
                ProcessSection(section, HeaderFooterType.First, subDocumentProcessor);
                ProcessSection(section, HeaderFooterType.Odd, subDocumentProcessor);
                ProcessSection(section, HeaderFooterType.Even, subDocumentProcessor);
            }
        }
        static void ProcessSection(Section section, HeaderFooterType headerFooterType, SubDocumentDelegate subDocumentProcessor)
        {
            if (section.HasHeader(headerFooterType))
            {
                SubDocument header = section.BeginUpdateHeader(headerFooterType);
                subDocumentProcessor(header);
                ProcessShapes(header.Shapes, subDocumentProcessor);
                section.EndUpdateHeader(header);
            }
            if (section.HasFooter(headerFooterType))
            {
                SubDocument footer = section.BeginUpdateFooter(headerFooterType);
                subDocumentProcessor(footer);
                ProcessShapes(footer.Shapes, subDocumentProcessor);
                section.EndUpdateFooter(footer);
            }
        }
        static void ProcessShapes(ShapeCollection shapes, SubDocumentDelegate subDocumentProcessor)
        {
            foreach (Shape shape in shapes)
                if (shape.ShapeFormat != null)
                    subDocumentProcessor(shape.ShapeFormat.TextBox.Document);
        }
        static void ProcessComments(CommentCollection comments, SubDocumentDelegate subDocumentProcessor)
        {
            foreach (Comment comment in comments)
            {
                SubDocument commentSubDocument = comment.BeginUpdate();
                subDocumentProcessor(commentSubDocument);
                comment.EndUpdate(commentSubDocument);
            }
        }
        static void ProcessFootnotes(NoteCollection footnotes, SubDocumentDelegate subDocumentProcessor)
        {
            foreach (Note note in footnotes)
            {
                SubDocument noteSubDocument = note.BeginUpdate();
                subDocumentProcessor(noteSubDocument);
                note.EndUpdate(noteSubDocument);
            }
        }
        static void ProcessEndnotes(NoteCollection endnotes, SubDocumentDelegate subDocumentProcessor)
        {
            foreach (Note note in endnotes)
            {
                SubDocument noteSubDocument = note.BeginUpdate();
                subDocumentProcessor(noteSubDocument);
                note.EndUpdate(noteSubDocument);
            }
        }
    }
}
