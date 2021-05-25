using System;
using AssignmentVI.Services;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using AssignmentVI.Helper;
using AssignmentVI.Models;
using Microsoft.Win32;

namespace AssignmentVI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Invoice invoice;
        public MainWindow()
        {
            InitializeComponent();
            InitializeGui();
        }

        private void InitializeGui()
        {
            MenuLoadLogo.IsEnabled = false;
            MenuPrint.IsEnabled = false;
            MenuSaveImage.IsEnabled = false;
            GridInvoice.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// Menu item Exit.
        /// Exit application on click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExitApplication_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        /// <summary>
        /// Open new invoice file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Open_Invoice_Click(object sender, RoutedEventArgs e)
        {
            invoice = FileService.OpenFile();
            if (invoice == null) return;
            MenuLoadLogo.IsEnabled = true;
            MenuPrint.IsEnabled = true;
            MenuSaveImage.IsEnabled = true;
            TbDiscount.Text = "0.0";

            TbInvoiceNumber.Text = invoice.InvoiceNumber.ToString();
            DpInvoiceDate.Text = invoice.InvoiceDate.ToString(CultureInfo.InvariantCulture);
            DpDueDate.Text = invoice.DueDate.ToString(CultureInfo.InvariantCulture);
            TbCompName.Text = invoice.Company.CompanyName;
            TbAddressFields.Text = Helper.StringBuilderFromArray.ArrayToMultiLineString(invoice.Customer);
            TbCompAddress.Text = Helper.StringBuilderFromArray.ArrayToMultiLineString(invoice.Company);
            TbContact.Text = Helper.StringBuilderFromArray.ArrayToMultiLineString(invoice.Contact);
            LwItemsList.ItemsSource = invoice.ItemsList;
            TbTotal.Text = invoice.ItemsList.Sum(x => x.Total).ToString();
            GridInvoice.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Load images to enhance user experience use filters. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuLoadLogo_OnClick(object sender, RoutedEventArgs e)
        {
            // open file dialog   
            var open = new OpenFileDialog
            {
                Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp)|*.jpg; *.jpeg; *.gif; *.bmp"
            };
            if (open.ShowDialog() != true) { }
            ImgLogo.Source = new BitmapImage(new Uri(open.FileName));
        }

        /// <summary>
        /// Click event for saving images. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuSaveImage_OnClick(object sender, RoutedEventArgs e)
        {
            ExportAsImage.SaveToPng(GridInvoice);
        }

        /// <summary>
        /// Click event for print file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuPrint_OnClick(object sender, RoutedEventArgs e)
        {
            var dlg = new PrintDialog();
            dlg.PrintVisual(GridInvoice, Title);
        }

        private void TbDiscount_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (TbDiscount.Text != "")
                TbTotal.Text = (invoice.ItemsList.Sum(x => x.Total) - decimal.Parse(TbDiscount.Text.Replace(".", ","))).ToString();
        }
    }
}