using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Namordnik.Views.Windows
{
    /// <summary>
    /// Логика взаимодействия для ImagesWindow.xaml
    /// </summary>
    public partial class ImagesWindow : Window
    {
        private const int imagesPerRow = 4;
        public string ImgUri { get; private set; }

        /// <summary>
        /// Конструктор окна
        /// </summary>
        public ImagesWindow()
        {
            InitializeComponent();
            ShowImages();
        }

        /// <summary>
        /// Получает изображения из папки products
        /// </summary>
        /// <returns></returns>
        private string[] GetImages()
        {
            var files = Directory.GetFiles("../../products");
            for (int i = 0; i < files.Length; i++)
            {
                files[i] = files[i].Remove(0, 5);
            }
            return files;
        }

        /// <summary>
        /// Выводит изображения из папки products
        /// </summary>
        private void ShowImages()
        {
            var files = GetImages();
            var column = new StackPanel
            {
                Orientation = Orientation.Vertical,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
            for (int i = 0; i <= files.Length / imagesPerRow; i++)
            {
                var row = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                };
                for (int j = i * imagesPerRow; j < (i * imagesPerRow) + imagesPerRow; j++)
                {
                    if (j >= files.Length)
                    {
                        break;
                    }
                    var image = new Image
                    {
                        Source = new BitmapImage(new Uri(files[j], UriKind.Relative)),
                        Height = 70,
                        Tag = files[j],
                        Cursor = Cursors.Hand
                    };
                    image.MouseLeftButtonDown += Image_MouseLeftButtonDown;
                    row.Children.Add(image);
                }
                column.Children.Add(row);
            }
            gPictures.Children.Add(column);
        }

        /// <summary>
        /// По нажатию на картинку сохраняет ее адрес
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ImgUri = (sender as Image).Tag.ToString();
            DialogResult = true;
            Close();
        }
    }
}
