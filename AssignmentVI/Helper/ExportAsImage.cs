using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Win32;

namespace AssignmentVI.Helper
{
    internal static class ExportAsImage
    {

        internal static void SaveToPng(FrameworkElement visual)
        {
            var encoder = new PngBitmapEncoder();
            SaveUsingEncoder(visual, encoder);
        }

        private static void SaveUsingEncoder(FrameworkElement visual, BitmapEncoder encoder)
        {
            var bitmap = new RenderTargetBitmap((int)visual.ActualWidth+100, (int)visual.ActualHeight+100, 96, 96, PixelFormats.Pbgra32);
            bitmap.Render(visual);
            var frame = BitmapFrame.Create(bitmap);
            encoder.Frames.Add(frame);

            var saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() != true) return;
            using var stream = File.Create(saveFileDialog.FileName);
            encoder.Save(stream);
        }
    }
}
