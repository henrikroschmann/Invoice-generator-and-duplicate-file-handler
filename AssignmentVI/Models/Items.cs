using System;
using System.Reflection;

namespace AssignmentVI.Models
{
    internal class Items
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal PricePerUnit { get; set; }
        public decimal Tax { get; set; }

        public decimal TotalTax => Math.Round(Quantity * PricePerUnit * Tax / 100, 2);
        public decimal Total => Math.Round(Quantity * PricePerUnit + TotalTax, 2);
    }
}