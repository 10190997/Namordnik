using Namordnik.Model;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;

namespace Namordnik.Views.Windows
{
    /// <summary>
    /// Логика взаимодействия для EditCostWindow.xaml
    /// </summary>
    public partial class EditCostWindow : Window
    {
        private List<Product> products;

        /// <summary>
        /// Конструктор окна, выводит среднюю цену для выбранной продукции
        /// </summary>
        /// <param name="products">Список продукции</param>
        public EditCostWindow(List<Product> products)
        {
            InitializeComponent();
            this.products = products;
            decimal cost = 0;
            int count = 0;
            foreach (var item in products)
            {
                cost += item.MinCostForAgent;
                count++;
            }
            tbCost.Text = (cost / count).ToString();
        }

        /// <summary>
        /// Меняет цены продукции на указанное значение
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (decimal.TryParse(tbCost.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal cost))
            {
                foreach (var item in products)
                {
                    item.MinCostForAgent += cost;
                }
            }
            else
            {
                MessageBox.Show("Введите дробное число");
                return;
            }

        }
    }
}
