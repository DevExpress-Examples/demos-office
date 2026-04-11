namespace BlazorDemo.OfficeFileApi.Models {
    public class DefaultFileInfo {
        public string Name { get; set; }
        public byte[] Content { get; set; }
        public DefaultFileInfo(string name, byte[] content) {
            Name = name;
            Content = content;
        }
    }
}
