namespace BlazorDemo.OfficeFileApi {
    public class EnumWrapper<T> where T:Enum {
        public T Value { get; }
        public string DisplayName { get; }

        public EnumWrapper(T value, string displayName) {
            Value = value;
            DisplayName = displayName;
        }
    }
}
