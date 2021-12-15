using Namordnik.Model;
using Namordnik.Views.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace Namordnik.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        private const int productsPerPage = 20;
        private int countProducts;
        private const int pageNumbers = 4;
        private int selectedPage = 1;

        /// <summary>
        /// Коснтруктор страницы
        /// </summary>
        public MainPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// По загрузке страницы заполняет элементы данными
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            cbFilter.Items.Add("Все типы");
            foreach (var item in DB.entities.ProductTypes)
            {
                cbFilter.Items.Add(item.Title);
            }
            cbFilter.SelectedIndex = 0;
            cbSort.SelectedIndex = 0;
            DB.entities.ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
            dgProducts.ItemsSource = GetProducts(tbSearch.Text, cbSort.Text, cbFilter.Text);
        }

        /// <summary>
        /// Создает список продукции с учетом фильтрации, сортировки и поиска, а также пагинации
        /// </summary>
        /// <param name="search">Строка поиска</param>
        /// <param name="sort">Сортировка</param>
        /// <param name="filter">Фильтрация типов продукции</param>
        /// <returns>List<> продукции</returns>
        public List<Product> GetProducts(string search = "", string sort = "", string filter = "")
        {
            var products = DB.entities.Products.ToList();
            switch (sort)
            {
                case "По наименованию, от А до Я":
                    {
                        products = products.OrderBy(p => p.Title).ToList();
                        break;
                    }
                case "По наименованию, от Я до А":
                    {
                        products = products.OrderByDescending(p => p.Title).ToList();
                        break;
                    }
                case "По возрастанию номера производственного цеха":
                    {
                        products = products.OrderBy(p => p.ProductionWorkshopNumber).ToList();
                        break;
                    }
                case "По убыванию номера производственного цеха":
                    {
                        products = products.OrderByDescending(p => p.ProductionWorkshopNumber).ToList();
                        break;
                    }
                case "По возрастанию минимальной стоимости для агента":
                    {
                        products = products.OrderBy(p => p.MinCostForAgent).ToList();
                        break;
                    }
                case "По убыванию минимальной стоимости для агента":
                    {
                        products = products.OrderByDescending(p => p.MinCostForAgent).ToList();
                        break;
                    }
                default:
                    break;
            }
            if (!string.IsNullOrEmpty(filter) && filter != "Все типы")
            {
                products = products.Where(p => p.ProductType.Title == filter).ToList();
            }
            if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
            {
                products = products.Where(p => p.Title.ToLower().Contains(search.ToLower())).ToList();
            }
            countProducts = products.Count;
            if (countProducts == 0)
            {
                return null;
            }
            CreatePages();
            products = Paginate(products)[selectedPage - 1];
            return products;
        }

        /// <summary>
        /// Создает номера страниц
        /// </summary>
        private void CreatePages()
        {
            spPages.Children.Clear();
            int pagesIterator = selectedPage;
            var tbPrev = new TextBlock
            {
                Text = " < ",
                FontSize = 30,
                Margin = new Thickness(5)
            };
            tbPrev.MouseLeftButtonDown += TbPrev_MouseLeftButtonDown;
            spPages.Children.Add(tbPrev);

            for (int i = 0; i < pageNumbers; i++)
            {
                if (pagesIterator > countProducts / productsPerPage)
                {
                    break;
                }
                var tbPage = new TextBlock
                {
                    Text = pagesIterator.ToString(),
                    FontSize = 30,
                    Margin = new Thickness(5)
                };
                tbPage.MouseLeftButtonDown += TbPage_MouseLeftButtonDown;
                if (pagesIterator == selectedPage)
                {
                    tbPage.TextDecorations = TextDecorations.Underline;
                }
                pagesIterator++;
                spPages.Children.Add(tbPage);
            }

            var tbNext = new TextBlock
            {
                Text = " > ",
                FontSize = 30,
                Margin = new Thickness(5)
            };
            tbNext.MouseLeftButtonDown += TbNext_MouseLeftButtonDown;
            spPages.Children.Add(tbNext);
        }

        /// <summary>
        /// По нажатию на номер страницы выводит ее
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TbPage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            selectedPage = Convert.ToInt32((sender as TextBlock).Text);
            dgProducts.ItemsSource = GetProducts(tbSearch.Text, cbSort.Text, cbFilter.Text);
        }

        /// <summary>
        /// По нажатию кнопки далее увеличивает номер страницы и выводит ее
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TbNext_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (selectedPage < countProducts / productsPerPage)
            {
                selectedPage++;
                dgProducts.ItemsSource = GetProducts(tbSearch.Text, cbSort.Text, cbFilter.Text);
            }
        }

        /// <summary>
        /// По нажатию кнопки предыдущая страница уменьшает номер страницы и выводит ее
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TbPrev_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (selectedPage > 1)
            {
                selectedPage--;
                dgProducts.ItemsSource = GetProducts(tbSearch.Text, cbSort.Text, cbFilter.Text);
            }
        }

        /// <summary>
        /// Создает разбитый на страницы список продукции
        /// </summary>
        /// <param name="products">Список продукции</param>
        /// <returns>Dictionary<> с номерами страниц и соответствующей продукцией</returns>
        private Dictionary<int, List<Product>> Paginate(List<Product> products)
        {
            var paginatedProducts = new Dictionary<int, List<Product>>();
            for (int i = 0; i <= countProducts / productsPerPage; i++)
            {
                var productsOnPage = new List<Product>();
                for (int j = i * productsPerPage; j < (i * productsPerPage) + productsPerPage; j++)
                {
                    if (j == countProducts)
                    {
                        break;
                    }
                    productsOnPage.Add(products[j]);
                }
                if (productsOnPage.Count == 0)
                {
                    break;
                }
                paginatedProducts.Add(i, productsOnPage);
            }
            return paginatedProducts;
        }

        /// <summary>
        /// Ищет введенную строку в БД
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            selectedPage = 1;
            dgProducts.ItemsSource = GetProducts(tbSearch.Text, cbSort.Text, cbFilter.Text);
        }

        /// <summary>
        /// Сортирует данные по выбранному параметру
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedPage = 1;
            dgProducts.ItemsSource = GetProducts(tbSearch.Text, ((ComboBoxItem)cbSort.SelectedValue).Content.ToString(), cbFilter.Text);
        }

        /// <summary>
        /// Сортирует данные по выбранному типу продукции
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedPage = 1;
            dgProducts.ItemsSource = GetProducts(tbSearch.Text, cbSort.Text, cbFilter.SelectedValue.ToString());
        }

        /// <summary>
        /// По нажатию нескольких элементов таблицы с помощью ctrl показывает кнопку изменения цен
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgProducts.SelectedItems.Count > 1)
            {
                btnChangeCost.Visibility = Visibility.Visible;
            }
            else
            {
                btnChangeCost.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// По нажатию на продукцию в таблице переходит на форму редактирования
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgProducts_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (dgProducts.SelectedItem is Product)
            {
                NavigationService.Navigate(new AddEditPage(dgProducts.SelectedItem as Product));
            }
        }

        /// <summary>
        /// По нажатию на кнопку Добавить переходит на форму создания продукции
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddEditPage(new Product()));
        }

        /// <summary>
        /// По нажатию кнопки выводит окно изменения цен
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnChangeCost_Click(object sender, RoutedEventArgs e)
        {
            var selectedProducts = new List<Product>();
            foreach (var item in dgProducts.SelectedItems)
            {
                if (item is Product)
                {
                    selectedProducts.Add(item as Product);
                }
            }
            if (selectedProducts.Count > 1)
            {
                var window = new EditCostWindow(selectedProducts);
                window.ShowDialog();
            }
        }
    }
}
