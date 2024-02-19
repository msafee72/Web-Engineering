namespace QueenLocalDataHandling
{
    class Order
    {
        public int OrderID { get; set; }
        public string CNIC { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerAddress { get; set; }
        public int ProductID { get; set; }
        public decimal Price { get; set; }
        public string Size { get; set; }

        public Order(int orderID, string cnic, string customerName, string customerPhone, string customerAddress, int productID, decimal price, string size)
        {
            OrderID = orderID;
            CNIC = cnic;
            CustomerName = customerName;
            CustomerPhone = customerPhone;
            CustomerAddress = customerAddress;
            ProductID = productID;
            Price = price;
            Size = size;
        }
    }
}
