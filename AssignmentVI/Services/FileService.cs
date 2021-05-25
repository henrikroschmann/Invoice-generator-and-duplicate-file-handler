using AssignmentVI.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

namespace AssignmentVI.Services
{
    internal static class FileService
    {
        internal static Invoice OpenFile()
        {
            var openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() != true) return null;
            var fileDatAllText = File.ReadAllLines(openFileDialog.FileName);
            return fileDatAllText.Length > 0 ? ProcessFile(fileDatAllText) : null;
        }

        internal static Invoice ProcessFile(string[] fileData)
        {
            try
            {
                var invoiceInfo = fileData.Take(3).ToArray();
                var customerInfo = fileData.Skip(3).Take(6).ToArray();
                var amountOfItems = int.Parse(fileData.Skip(9).Take(1).ToArray()[0]);
                var items = fileData.Skip(10).Take(amountOfItems * 4).ToArray();
                var itemsList = new List<Items>();
                for (var i = 0; i < amountOfItems * 4; i += 4)
                {
                    itemsList.Add(new Items
                    {
                        Name = items[i],
                        Quantity = int.Parse(items[i + 1]),
                        PricePerUnit = decimal.Parse(items[i + 2].Replace('.', ',')),
                        Tax = decimal.Parse(items[i + 3])
                    });
                }
                var companyInfo = fileData.Skip(10 + (amountOfItems * 4)).Take(5).ToArray();
                var contactInfo = fileData.Skip(10 + (amountOfItems * 4) + 5).ToArray();

                return new Invoice
                {
                    InvoiceNumber = int.Parse(invoiceInfo[0]),
                    InvoiceDate = DateTime.Parse(invoiceInfo[1]),
                    DueDate = DateTime.Parse(invoiceInfo[2]),
                    Customer = new Customer
                    {
                        Company = customerInfo[0],
                        ContactPerson = customerInfo[1],
                        StreetAddress = customerInfo[2],
                        ZipCode = $"{customerInfo[3]} {customerInfo[4]}",
                        Country = customerInfo[5]
                    },
                    Contact = new Contact
                    {
                        Phone = contactInfo[0],
                        HomePage = contactInfo[1]
                    },
                    ItemsList = itemsList,
                    Company = new Company
                    {
                        CompanyName = companyInfo[0],
                        Address = companyInfo[1],
                        ZipCode = companyInfo[2],
                        City = companyInfo[3],
                        Country = companyInfo[4]
                    }
                };
            }
            catch (Exception e)
            {
                MessageBox.Show($"Unable to parse file the following exception occurred {e.Message}");
            }

            return null;
        }
    }
}