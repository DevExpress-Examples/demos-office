namespace BlazorDemo.DemoData {
    public class DemoGroup : DemoItemBase {
        public string Id { get; set; }
        public string NavigationAriaLabel { get; set; }
        public DemoPage[] Pages { get; set; }
        public override IEnumerable<DemoItem> GetNavTreeChildren(bool demoMode) {
            return Pages.Where(p => p.IsNavTreeVisible(demoMode));
        }
    }
}
