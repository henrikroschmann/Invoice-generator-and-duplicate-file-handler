using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using DuplicateFilesFinder.Models;
using Application = System.Windows.Application;

namespace DuplicateFilesFinder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<FilesInFolder> _files;
        private List<string> cleanUpList;
        public MainWindow()
        {
            InitializeComponent();
            InitializeGui();
        }

        private void InitializeGui()
        {
            BtDelete.IsEnabled = false;
            RadioLand.IsEnabled = false;
        }

        private void MenuItemExit_OnClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MenuItemFolders_OnClick(object sender, RoutedEventArgs e)
        {
            _files = Services.FileService.OpenFile();
            LwFilesToScan.ItemsSource = _files;
            RadioLand.IsEnabled = true;
            BtMoreFiles.IsEnabled = true;
        }
        
        /// <summary>
        /// Event when sorting on filename
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RadioButtonFileName_OnClick(object sender, RoutedEventArgs e)
        {
            //Empty old filter 
            cleanUpList = new List<string>();
            BtDelete.IsEnabled = true;
            LwSelectedFiles.ItemsSource = _files.GroupBy(x => x.FileName, (key, x) => new { date = key, filename = x.ToList() })
                .Where(x => x.filename.Count > 1).SelectMany(x => x.filename);
        }

        /// <summary>
        /// Event when sorting on modified date
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RadioButtonModified_OnClick(object sender, RoutedEventArgs e)
        {
            //Empty old filter 
            cleanUpList = new List<string>();
            BtDelete.IsEnabled = true;
            LwSelectedFiles.ItemsSource = _files.GroupBy(x => x.ModifiedDateTime, (key, x) => new { date = key, filename = x.ToList() })
                .Where(x => x.filename.Count > 1).SelectMany(x => x.filename);
        }

        /// <summary>
        /// Event when sortin on size 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RadioButtonSize_OnClick(object sender, RoutedEventArgs e)
        {
            //Empty old filter 
            cleanUpList = new List<string>();
            BtDelete.IsEnabled = true;
            LwSelectedFiles.ItemsSource = _files.GroupBy(x => x.Size, (key, x) => new { date = key, filename = x.ToList() })
                .Where(x => x.filename.Count > 1).SelectMany(x => x.filename);
        }

        /// <summary>
        /// Event when sorting on filesType
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RadioButtonFileType_OnClick(object sender, RoutedEventArgs e)
        {
            //Empty old filter 
            cleanUpList = new List<string>();
            BtDelete.IsEnabled = true;
            LwSelectedFiles.ItemsSource = _files.GroupBy(x => x.Type, (key, x) => new { date = key, filename = x.ToList() })
                .Where(x => x.filename.Count > 1).SelectMany(x => x.filename);
        }


        /// <summary>
        /// Delete files from removal list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtDelete_OnClick(object sender, RoutedEventArgs e)
        {
            var a = LwSelectedFiles.Items;
            var messageBoxResult = System.Windows.MessageBox.Show("Are you sure this will remove the selected items in the list?", "Delete Confirmation", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                foreach (var file in cleanUpList)
                {
                    File.Delete(file);
                    _files.RemoveAt(_files.FindIndex(x => x.FullPath == file));
                    LwFilesToScan.ItemsSource = _files;
                }
                cleanUpList = new List<string>();
            }
                
        }

        /// <summary>
        /// Add to potential Removal list.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            var checkBox = e.OriginalSource as CheckBox;
            var item = checkBox?.DataContext as FilesInFolder;
            if (item != null) cleanUpList.Add(item.FullPath);
        }

        /// <summary>
        /// Remove from potential removal list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            var checkBox = e.OriginalSource as CheckBox;
            var item = checkBox?.DataContext as FilesInFolder;
            if (item != null) cleanUpList.Remove(item.FullPath);
        }

        private void BtMoreFiles_OnClick(object sender, RoutedEventArgs e)
        {
            var files = Services.FileService.OpenFile();
            if (files == null) return;
            _files.AddRange(files);
            LwFilesToScan.Items.Refresh();
        }
    }
}
