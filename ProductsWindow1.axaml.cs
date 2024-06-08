using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data.Converters;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace AnyaProject
{
    public partial class ProductsWindow1 : Window
    {
        private User _currentUser;
        private SortOrder currentSortOrder = SortOrder.Ascending;

        public enum SortOrder
        {
            Ascending,
            Descending,
            Alphabetical,
            ReverseAlphabetical
        }

        public static List<Product> ProductsList = new List<Product>();

        public ProductsWindow1()
        {
            InitializeComponent();
            DataContext = this;
        }

        public ProductsWindow1(User currentUser, List<Product> productslist)
        {
            _currentUser = currentUser;
            ProductsList = productslist;

            InitializeComponent();
            DataContext = this;

            queenStatus.Text = "Статус: " + _currentUser.UserStatus;
            queenName.Text = "Имя: " + _currentUser.UserName;

            Search.TextChanged += SearchTextBox_TextChanged;

            foreach (var product in ProductsList)
            {
                product.Otobrazhenie = _currentUser.UserStatus == "Queen";
            }

            foreach (var product in ProductsList)
            {
                if (product.Stock == 0)
                {
                    Color customColor = Color.FromRgb(102, 98, 103);
                    SolidColorBrush brush = new SolidColorBrush(customColor);
                    product.change_color = brush;
                }

                Tovarslistbox.Items.Add(product);
            }

            Button addProductButton = this.FindControl<Button>("DobavitTovar");
            if (_currentUser.UserStatus != "Queen")
            {
                addProductButton.IsVisible = false;
            }
            else if (_currentUser.UserStatus == "гость")
            {
                queenName.Text = "";
            }
            else
            {
                addProductButton.IsVisible = true;
            }

            foreach (var product in ProductsList)
            {
                product.AddVKorzinuVisibility = true;
            }
        }

        private void SearchTextBox_TextChanged(object sender, EventArgs e)
        {
            string searchText = Search.Text.ToLower();
            UpdateListWithSearchAndSort(searchText, null, ProductsList);
        }

        public void UpdateProductsList(Product newProduct)
        {
            ProductsList.Add(newProduct);

            string manufacturer = newProduct.Manufacturer;

            if (!ProizvoditeliList.Items.Cast<ComboBoxItem>().Any(item => string.Equals(item.Content?.ToString(), manufacturer, StringComparison.OrdinalIgnoreCase)))
            {
                ProizvoditeliList.Items.Add(new ComboBoxItem { Content = manufacturer });
            }

            newProduct.Otobrazhenie = _currentUser.UserStatus == "Queen";
            newProduct.AddVKorzinuVisibility = _currentUser.UserStatus == "Queen";

            if (newProduct.Stock == 0)
            {
                Color customColor = Color.FromRgb(102, 98, 103);
                SolidColorBrush brush = new SolidColorBrush(customColor);
                newProduct.change_color = brush;
            }
            else
            {
                SolidColorBrush brush = new SolidColorBrush(Colors.Transparent);
                newProduct.change_color = brush;
            }
            Tovarslistbox.Items.Add(newProduct);
        }

        private void UpdateListWithSearchAndSort(string searchText, string selectedManufacturer, List<Product> productList)
        {
            string[] searchWords = searchText.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            var filteredProducts = productList.Where(product =>
                (string.IsNullOrWhiteSpace(searchText) ||
                    searchWords.Any(word =>
                        product.TovarName.ToLower().Contains(word) ||
                        product.Manufacturer.ToLower().Contains(word) ||
                        product.Description.ToLower().Contains(word)
                    )
                ) &&
                (string.IsNullOrWhiteSpace(selectedManufacturer) ||
                    string.Equals(product.Manufacturer, selectedManufacturer, StringComparison.OrdinalIgnoreCase)
                )
            ).ToList();

            List<Product> sortedList;
            switch (currentSortOrder)
            {
                case SortOrder.Ascending:
                    sortedList = filteredProducts.OrderBy(item => item.Price).ToList();
                    break;
                case SortOrder.Descending:
                    sortedList = filteredProducts.OrderByDescending(item => item.Price).ToList();
                    break;
                case SortOrder.Alphabetical:
                    sortedList = filteredProducts.OrderBy(item => item.Manufacturer).ToList();
                    break;
                case SortOrder.ReverseAlphabetical:
                    sortedList = filteredProducts.OrderByDescending(item => item.Manufacturer).ToList();
                    break;
                default:
                    sortedList = filteredProducts;
                    break;
            }

            UpdateListBox(sortedList);
        }

        private void UpdateListBox(List<Product> ListToAdd)
        {
            Tovarslistbox.Items.Clear();

            foreach (var item in ListToAdd)
            {
                Tovarslistbox.Items.Add(item);
            }
        }

        private void ProizvoditeliList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            if (comboBox.SelectedItem != null)
            {
                string searchText = Search.Text.ToLower();
                string selectedManufacturer = comboBox.SelectedItem?.ToString();
                UpdateListWithSearchAndSort(searchText, selectedManufacturer, ProductsList);
            }
        }

        private void AddTovar_Button(object sender, RoutedEventArgs e)
        {
            Add ap = new Add(this);
            ap.Show();
        }

        private void DeleteTovar_Button(object sender, RoutedEventArgs e)
        {
            Button deleteButton = (Button)sender;
            Product product = (Product)deleteButton.DataContext;

            if (_currentUser.Products.Contains(product))
            {
                Message message = new Message();
                message.oshibka1.Text = "Ошибка!!! Вы не можете удалить товар из ассортимента, т.к он присутствует в корзине у пользователя!";
                message.Show();
            }
            else
            {
                ProductsList.Remove(product);
                Tovarslistbox.Items.Remove(product);
            }
        }

        private void RedactButton(object sender, RoutedEventArgs e)
        {
            Button edittovar = (Button)sender;
            Product editTovar = (Product)edittovar.DataContext;
            Redactirovanie tovar = new Redactirovanie(editTovar, this, ProductsList);
            tovar.Show();
        }

        private void DobavitKorzinu(object sender, RoutedEventArgs e)
        {
            Button addtobasket = (Button)sender;
            Product product = (Product)addtobasket.DataContext;

            if (product.Stock == 0)
            {
                Message message = new Message();
                message.oshibka1.Text = "ОШИБКА!!! Вы не можете добавить в корзину товар, которого нет на складе!!";
                message.Show();
            }
            else
            {
                _currentUser.Products.Add(product);
            }
        }

        private void PoitiKorzinu(object sender, RoutedEventArgs e)
        {
            Korzina basket = new Korzina(_currentUser);
            basket.Show();
        }

        private void PriceUbivanie(object sender, RoutedEventArgs e)
        {
            List<Product> SortedList = ProductsList.OrderByDescending(item => item.Price).ToList();
            UpdateListBox(SortedList);
            UpdateListWithSearchAndSort(Search.Text.ToLower(), null, SortedList);
        }

        private void PriceVozrastanie(object sender, RoutedEventArgs e)
        {
            List<Product> SortedList = ProductsList.OrderBy(item => item.Price).ToList();
            UpdateListBox(SortedList);
            UpdateListWithSearchAndSort(Search.Text.ToLower(), null, SortedList);
        }

        private void ProizvoditelAlfavit(object sender, RoutedEventArgs e)
        {
            List<Product> SortedList = ProductsList.OrderBy(item => item.Manufacturer).ToList();
            UpdateListBox(SortedList);
            UpdateListWithSearchAndSort(Search.Text.ToLower(), null, SortedList);
        }

        private void ProizvoditelObratno(object sender, RoutedEventArgs e)
        {
            List<Product> SortedList = ProductsList.OrderByDescending(item => item.Manufacturer).ToList();
            UpdateListBox(SortedList);
            UpdateListWithSearchAndSort(Search.Text.ToLower(), null, SortedList);
        }

        private void Exit_ButtonClick(object sender, RoutedEventArgs e)
        {
            MainWindow auth = new MainWindow(ProductsList);
            auth.Show();
            this.Close();
        }
    }
}
