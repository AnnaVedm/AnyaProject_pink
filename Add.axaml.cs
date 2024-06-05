using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using System.Collections;
using System.Collections.Generic;
using Avalonia.Media;

namespace AnyaProject
{
    public partial class Add : Window
    {
        public List<Product> tovars2 = new List<Product>();
        private User _user;
        private ProductsWindow1 _listupdate;

        public Add()
        {
            InitializeComponent();
        }

        public Add(User user, List<Product> Tovars /*ProductsWindow1 forupdate*/)
        {
            InitializeComponent();
            DataContext = this;

            //_listupdate = forupdate;
            _user = user;
            tovars2 = Tovars;
        }

        private void DobavitTovar_Button(object sender, RoutedEventArgs e)
        {

            Product product = new Product()
            {
                TovarName = TovarName.Text,
                Manufacturer = TovarProizvoditel.Text,
                Description = TovarOpisanie.Text,
                Price = Convert.ToDouble(TovarPrice.Text),
                Stock = Convert.ToInt32(TovarOstatok.Text)
            };

            tovars2.Add(product);

            //_listupdate.Tovarslistbox.Items.Clear();

            //foreach (var tovar in tovars2)
            //{
            //    _listupdate.Tovarslistbox.Items.Add(tovar);
            //}

            ////при добавлении товара запсукаем метод updateItemsList и передаем туда добавленного производителя
            //_listupdate.UpdateItemsList(product.Manufacturer);


            ProductsWindow1 productsWindow1 = new ProductsWindow1(_user, tovars2);
            productsWindow1.Show();

            this.Close();
        }
    }
}
