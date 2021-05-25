using System;
using System.Collections.Generic;

namespace AssignmentVI.Models
{
    internal class Invoice
    {
        public int InvoiceNumber { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime DueDate { get; set; }
        public Customer Customer { get; set; }
        public Contact Contact { get; set; }
        public List<Items> ItemsList { get; set; }
        public Company Company { get; set; }
    }
}