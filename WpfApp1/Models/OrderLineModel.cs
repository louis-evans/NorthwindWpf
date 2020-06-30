using System;

namespace WpfApp1.Models
{
    class OrderLineModel
    {
        public int OrderID { get; set; }
        public string CompanyName { get; set; }
        public DateTime? OrderDate { get; set; }
        public decimal OrderTotal { get; set; }
        public int ItemCount { get; set; }
    }
}
