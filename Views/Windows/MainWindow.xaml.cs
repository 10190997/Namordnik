using System;
using System.Windows;

namespace Namordnik
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Конструктор окна
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Переход на предыдущую страницу
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.GoBack();
        }

        /// <summary>
        /// По загрузке содержимого frame определяет, можно ли вернуться назад
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainFrame_ContentRendered(object sender, EventArgs e)
        {
            btnBack.Visibility = mainFrame.CanGoBack ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
