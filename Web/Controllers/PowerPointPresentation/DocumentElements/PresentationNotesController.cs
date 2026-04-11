using BlazorDemo.OfficeFileApi.Models.PowerPointPresentation.DocumentElements;
#region Code
using DevExpress.Docs.Presentation;
#endregion Code
using Microsoft.AspNetCore.Mvc;

namespace BlazorDemo.OfficeFileApi.Controllers.PowerPointPresentation.DocumentElements {
    [ApiController]
    [Route("api/presentation/notes")]
    public class PresentationNotesController : PresentationControllerBase {

        [HttpPost("process")]
        public async Task<IActionResult> ProcessNotes(IFormFile file, [FromForm] PresentationNotesModel model) {
            using var input = new MemoryStream();
            await file.CopyToAsync(input);
            input.Position = 0;
            var format = PresentationFormatHelper.GetDocumentFormat(model.OutputFormat);
            var outputStream = ApplyNotes(input, model.ActionType, model.NoteText, model.SlideIndex - 1, format);
            return CreateOutputFile(outputStream, model.OutputFormat, model.OutputName);
        }
        #region Code
        Stream ApplyNotes(Stream input, NotesActionType action, string noteText, int slideIndex, DocumentFormat format) {
            using var presentation = new Presentation(input);

            switch(action) {
                case NotesActionType.ClearAllNotes:
                    ClearAllNotes(presentation);
                    break;
                case NotesActionType.AddNoteToSlide:
                    AddUpdateNote(presentation, slideIndex, noteText);
                    break;
            }
            var outputStream = new MemoryStream();

            presentation.SaveDocument(outputStream, format);

            outputStream.Position = 0;
            return outputStream;
        }

        void AddUpdateNote(Presentation presentation, int slideIndex, string noteText) {
            if(slideIndex >= 0 && slideIndex < presentation.Slides.Count) {
                var slide = presentation.Slides[slideIndex];
                if(presentation.NotesMaster == null)
                    presentation.NotesMaster = new NotesMaster(name: "notesMaster");
                NotesSlide notes;
                if(slide.Notes == null) {
                    notes = new NotesSlide();
                    slide.Notes = notes;
                } else
                    notes = slide.Notes;
                notes.TextArea.Text = noteText;
            }
        }

        void ClearAllNotes(Presentation presentation) {
            foreach(var slide in presentation.Slides)
                slide.Notes = null;
        }
        #endregion Code
    }
}
