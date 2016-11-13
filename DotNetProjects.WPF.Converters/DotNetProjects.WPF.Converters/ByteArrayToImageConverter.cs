using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace DotNetProjects.WPF.Converters
{
    public class ByteArrayToImageConverter : ValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return null;

#if !SILVERLIGHT

            try
            {
                MemoryStream byteStream = new MemoryStream((byte[]) value);
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.StreamSource = byteStream;
                image.EndInit();                

                return image;                
            }
            catch (Exception)
            { }

            return null;
#else
            BitmapImage imageSource = null;

            try
            {
                using (MemoryStream stream = new MemoryStream((byte[])value))
                {
                    stream.Seek(0, SeekOrigin.Begin);
                    BitmapImage b = new BitmapImage();
                    b.SetSource(stream);
                    imageSource = b;
                }
            }
            catch (System.Exception)
            { }

            return imageSource;
#endif
        }

        public override object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }   
}