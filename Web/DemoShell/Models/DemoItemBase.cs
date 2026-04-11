namespace BlazorDemo.DemoData {
    public class DemoItemBase {
        public string Title { get; set; }
        public string Icon { get; set; }
        public virtual IEnumerable<DemoItem> GetNavTreeChildren(bool demoMode) { return Enumerable.Empty<DemoItem>(); }
    }
}
