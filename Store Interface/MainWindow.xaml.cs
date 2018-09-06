using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;


namespace Store_Interface
{
    public partial class MainWindow : Window
    {
        public List<StoreItem> list;
        public List<StoreItem> shoppingcart;
        public string fPath = "Tekst.txt";

        public MainWindow()
        {
            InitializeComponent();

            if (!File.Exists("Tekst.txt"))
            {
                using (var myFile = File.Create("Tekst.txt"))
                {

                }

            }
            openFile();
            saveFile();
            list = new List<StoreItem>();
            shoppingcart = new List<StoreItem>();
        }

        private void btnAddToShoppingCart(object sender, RoutedEventArgs e)
        {
            if (listview_StoreItems.SelectedItem != null)
            {
                if ((listview_StoreItems.SelectedItem as StoreItem).Amount <= 0)
                {
                    MessageBox.Show("Sorry, that item isnt in stock.");
                    listview_StoreItems.Items.Remove(listview_StoreItems.SelectedItem);
                    saveFile();
                    listview_StoreItems.Items.Clear();
                    openFile();

                }
                else
                {
                    listview_ShoppingCart.Items.Add(new StoreItem
                    {
                        Name = (listview_StoreItems.SelectedItem as StoreItem).Name,
                        Price = (listview_StoreItems.SelectedItem as StoreItem).Price,
                        Amount = 1
                    });
                    shoppingcart.Add(new StoreItem
                    {
                        Name = (listview_StoreItems.SelectedItem as StoreItem).Name,
                        Price = (listview_StoreItems.SelectedItem as StoreItem).Price,
                        Amount = 1
                    });
                }
                foreach (StoreItem item in listview_StoreItems.Items)
                {
                    if (item.Name == (listview_StoreItems.SelectedItem as StoreItem).Name)
                    {
                        item.Amount--;
                        saveFile();
                        listview_StoreItems.Items.Clear();
                        openFile();
                        return;
                    }
                }

            }
        }

        private void btnBuy_Click(object sender, RoutedEventArgs e)
        {
            if (!listview_ShoppingCart.HasItems)
            {
                MessageBox.Show("You need to select an item!");
            }
            else
            {
                var cheque = new Cheque();
                double total = 0;
                cheque.Print(shoppingcart, total);
            }
        }

        private void btnAddItem(object sender, RoutedEventArgs e)
        {
            bool cont = true;
            if (txt_Price.Text != "")
            {
                foreach (StoreItem cItem in listview_StoreItems.Items)
                {
                    if (cItem.Name == txt_Name.Text && cItem.Price == double.Parse(((txt_Price.Text)), System.Globalization.CultureInfo.InvariantCulture))
                    {
                        cItem.Amount++;
                        cont = false;
                    }
                }
                if (cont == true)
                {
                    listview_StoreItems.Items.Add(new StoreItem
                    {
                        Name = (txt_Name.Text).ToString(),
                        Price = double.Parse(((txt_Price.Text)), System.Globalization.CultureInfo.InvariantCulture),
                        Amount = 1
                    });
                }
                saveFile();
                listview_StoreItems.Items.Clear();
                openFile();
            }
        }

        private void btnRemoveItem(object sender, RoutedEventArgs e)
        {
            listview_ShoppingCart.Items.Remove(listview_ShoppingCart.Items);
            shoppingcart = new List<StoreItem>();
            Refresh();
        }

        public void openFile()
        {
            using (var file = File.OpenText(fPath))
            {
                while (!file.EndOfStream)
                {
                    var line = file.ReadLine();
                    var line2 = line.Split(';').ToList();
                    listview_StoreItems.Items.Add(new StoreItem
                    {
                        Name = Convert.ToString(line2[0]),
                        Price = double.Parse((line2[1])),
                        Amount = Int32.Parse(line2[2])
                    });
                }
            }
        }

        public void saveFile()
        {
            using (StreamWriter file = new StreamWriter(fPath, false))
            {
                foreach (StoreItem item in listview_StoreItems.Items)
                {
                    file.WriteLine(item.Name + ";" + item.Price + ";" + item.Amount);
                }
            }
        }

        public void Refresh()
        {
            listview_ShoppingCart.Items.Clear();
            saveFile();
            listview_StoreItems.Items.Clear();
            openFile();
        }
    }
}
public class StoreItem
{
    public string Name { get; set; }
    public double Price { get; set; }
    public int Amount { get; set; }
}