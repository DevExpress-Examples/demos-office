namespace BlazorDemo.DemoData {
    public class DemoRootPage : DemoPageBase {
        public string AnalyticsId { get; set; }

        public override DemoItem[] GetChildItems() { return Pages!; }

        public override string GetNavTreeUrl() {
            throw new NotImplementedException();
        }
    }
}
