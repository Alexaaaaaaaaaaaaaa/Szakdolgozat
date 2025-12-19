using System;

namespace MobileApp.Models
{
    public class Item
    {
        public DateTime Date { get; set; }

        public int Quantity { get; set; }

        public string QuantityMeasure { get; set; }

        public string Food { get; set; }
        public bool IsOpened { get; set; } 
        public int Id { get; set; }
    }
}