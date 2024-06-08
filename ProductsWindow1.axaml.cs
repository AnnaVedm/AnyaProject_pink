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
using System.Media;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace AnyaProject
{
    public partial class ProductsWindow1 : Window
    {
        private User _currentUser;

        //������ ���������, ������� �� ����� ����� ���������� � ������ ������
        public static List<Product> ProductsList = new List<Product> { };

        //������ �������, ������� � �������
        //public List<Product> ProductToBasket = new List<Product>() { };

        public ProductsWindow1()
        {
            InitializeComponent();

            //��������� ��������� ������ ���� �� ������� ������
            DataContext = this;
        }

        public ProductsWindow1(User currentUser, List<Product> productslist)
        {
            ProductsList = productslist;

            DataContext = this;
            _currentUser = currentUser; // ������������� _currentUser
            InitializeComponent(); // ������������� �����������

            queenStatus.Text = "������: " + _currentUser.UserStatus;
            queenName.Text = "���: " + _currentUser.UserName;

            ProductsList.Add(
               new Product()
               {
                   TovarName = "Samsung Galaxy A72",
                   Manufacturer = "Samsung",
                   Description = "������������ ����� AMOLED 6.7', 256��, ������",
                   Stock = 34,
                   Price = 33990
               });

            ProductsList.Add(
               new Product()
               {
                   TovarName = "Poco F5 Pro",
                   Manufacturer = "Xiaomi",
                   Description = "256/8��, �����, AMOLED FHD+, 6.67', 2G/3G/4G (LTE)/5G",
                   Stock = 34,
                   Price = 33990
               });

            ProductsList.Add(
               new Product()
               {
                   TovarName = "ITEL A70",
                   Manufacturer = "ATEL",
                   Description = "IPS, 6.6' (1612x720), Unisoc T603, 2G/3G/4G (LTE)",
                   Stock = 34,
                   Price = 33990
               });

            //������ ��������� ������ ������������� � ������� ��� ������� ������ � ����������� �� �������
            foreach (var product in ProductsList)
            {
                product.Otobrazhenie = _currentUser.UserStatus == "Queen";
            }

            //-------------------------------------------
            foreach (var product in ProductsList) //��������� ��� ������ �� ������ � Listbox
            {
                if (product.Stock == 0)
                {
                    Color customColor = Color.FromRgb(102, 98, 103);
                    SolidColorBrush brush = new SolidColorBrush(customColor);
                    product.change_color = brush;
                }

                Tovarslistbox.Items.Add(product);
            }

            //-------------------------------------------

            // ��������� ������� � ������
            Button addProductButton = this.FindControl<Button>("DobavitTovar");
            if (_currentUser.UserStatus != "Queen")
            {
                addProductButton.IsVisible = false;
            }

            else if (_currentUser.UserStatus == "�����")
            {
                queenName.Text = "";
            }
            else
            {
                addProductButton.IsVisible = true;
            }
        }

        //������� �����
        private void DeleteTovar_Button(object sender, RoutedEventArgs e)
        {
            // �������� ������, �� ������� ���� �������
            Button deleteButton = (Button)sender;

            // �������� �������� ������ ���� ������ (�.�. ������� Products, � �������� ��������� ������)
            Product product = (Product)deleteButton.DataContext;

            //���� ��������� ����� ���� � �������, �� ������� ��������� 
            if (_currentUser.Products.Contains(product))
            {
                Message message = new Message();

                message.oshibka1.Text = "������!!! �� �� ������ ������� ����� �� ������������, �.� �� ������������ � ������� � ������������!";

                message.Show();
            }
            else
            {
                //���� ��� � �������, �� ������� �� ������ �������
                ProductsList.Remove(product);
                Tovarslistbox.Items.Remove(product);
            }
        }

        //��������������� �����
        public void RedactButton(object sender, RoutedEventArgs e)
        {
            Button edittovar = (Button)sender;

            Product editTovar = (Product)edittovar.DataContext;

            Redactirovanie tovar = new Redactirovanie(editTovar, this, ProductsList);
            tovar.Show();
        }

        //��� ��������� � ���� AddProduct, ����� ����� � ���� ������ �������� �����
        public void UpdateProductsList(Product newProduct)
        {
            ProductsList.Add(newProduct);
            newProduct.Otobrazhenie = _currentUser.UserStatus == "Queen";

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

        //��������� ����� � ��� ������
        private void AddTovar_Button(object sender, RoutedEventArgs e)
        {
            // �������� ������ �� �������� ������ ������� � ���� ���������� ������
            Add ap = new Add(this);
            ap.Show();
        }

        //������������ �� ���� �����������
        private void Exit_ButtonClick(object sender, RoutedEventArgs e)
        {
            MainWindow auth = new MainWindow(ProductsList);
            auth.Show();

            this.Close();
        }

        private void PriceUbivanie(object sender, RoutedEventArgs e)
        {
            List<Product> SortedList = ProductsList.OrderByDescending(item => item.Price).ToList();
            UpdateListBox(SortedList);
        }

        private void PriceVozrastanie(object sender, RoutedEventArgs e)
        {
            List<Product> SortedList = ProductsList.OrderBy(item => item.Price).ToList();
            UpdateListBox(SortedList);
        }

        private void ProizvoditelAlfavit(object sender, RoutedEventArgs e)
        {
            List<Product> SortedList = ProductsList.OrderBy(item => item.Manufacturer).ToList();
            UpdateListBox(SortedList);
        }

        private void ProizvoditelObratno(object sender, RoutedEventArgs e)
        {
            List<Product> SortedList = ProductsList.OrderByDescending(item => item.Manufacturer).ToList();
            UpdateListBox(SortedList);
        }

        private void UpdateListBox(List<Product> ListToAdd) //��������� ������ �������
        {
            Tovarslistbox.Items.Clear();

            foreach (var item in ListToAdd)
            {
                Tovarslistbox.Items.Add(item);
            }
        }

        private void DobavitKorzinu(object sender, RoutedEventArgs e)
        {
            // �������� ������, �� ������� ���� �������
            Button addtobasket = (Button)sender;

            // �������� �������� ������ ���� ������ (�.�. ������� Products, � �������� ��������� ������)
            Product product = (Product)addtobasket.DataContext;

            //�������� �� ���������� 
            if (product.Stock == 0)
            {
                Message message = new Message();
                message.oshibka1.Text = "������!!! �� �� ������ �������� � ������� �����, �������� ��� �� ������!!";

                message.Show();
            }

            else
            {
                _currentUser.Products.Add(product);

                //Korzina basket = new Korzina(_currentUser);
                //basket.Show();
            }

        }

        //������� � �������
        private void PoitiKorzinu(object sender, RoutedEventArgs e)
        {
            Korzina basket = new Korzina(_currentUser);
            basket.Show();
        }
    }
}

