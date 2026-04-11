using Microsoft.AspNetCore.Components.Forms;

namespace BlazorDemo.OfficeFileApi.Pages {
    public abstract class DemoPageComponentWithValidation<TModel> : DemoPageComponentBase where TModel : class, new() {
        protected TModel model = new TModel();
        protected EditContext editFormContext;
        protected override void OnInitialized() {
            this.editFormContext = new EditContext(model);
            base.OnInitialized();
        }
        protected override Task ProcessDownloadWithLoading(Func<Task> operation) {
            if(!editFormContext.Validate())
                return Task.CompletedTask;
            return base.ProcessDownloadWithLoading(operation);
        }
    }
}
