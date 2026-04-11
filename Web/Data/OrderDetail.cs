namespace BlazorDemo.OfficeFileApi {
    public class OrderDetail {
        public int OrderID { get; }
        public int ProductID { get; }
        public short Quantity { get; }
        public decimal UnitPrice { get; }
        public float Discount { get; }
        public string ProductName { get; }
        public string Supplier { get; }

        public OrderDetail(int orderId, int productId, string productName, string supplier, decimal unitPrice, short quantity, float discount) {
            OrderID = orderId;
            ProductID = productId;
            ProductName = productName;
            Supplier = supplier;
            UnitPrice = unitPrice;
            Quantity = quantity;
            Discount = discount;
        }
    }
}
