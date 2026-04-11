using System.ComponentModel.DataAnnotations;

namespace BlazorDemo.OfficeFileApi.Models.PowerPointPresentation.DocumentElements {
    public class PresentationNotesModel : PresentationModelBase {
        [Required(ErrorMessage = "Please enter note text")]
        public string NoteText { get; set; } = "Sample note text";

        public int SlideIndex { get; set; } = 1;

        // Operations
        public NotesActionType ActionType { get; set; } = NotesActionType.AddNoteToSlide;

        public override string OutputName { get; set; } = "Presentation_Notes";
        internal readonly List<object> ActionTypes = new() {
            new { Text = "Add/Update Slide Note", Value = NotesActionType.AddNoteToSlide },
            new { Text = "Clear All Notes", Value = NotesActionType.ClearAllNotes }
        };
        internal readonly List<FormatInfo> OutputFormats = new() {
            new FormatInfo { Text = "PowerPoint Presentation (.pptx)", Value = "pptx" },
            new FormatInfo { Text = "PowerPoint Macro-Enabled Presentation (.pptm)", Value = "pptm" },
            new FormatInfo { Text = "PowerPoint Template (.potx)", Value = "potx" },
            new FormatInfo { Text = "PowerPoint Macro-Enabled Template (.potm)", Value = "potm" }
        };
    }
    public enum NotesActionType {
        AddNoteToSlide,
        ClearAllNotes
    }
}
