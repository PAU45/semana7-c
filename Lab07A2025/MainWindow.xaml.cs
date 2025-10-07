using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Business;
using Entity;

namespace Lab07A2025
{
    public partial class MainWindow : Window
    {
        private readonly BProduct _bProduct = new BProduct();
        private ObservableCollection<Product> _products = new();

        public MainWindow()
        {
            InitializeComponent();
            LoadProducts();
        }

        private void LoadProducts()
        {
            var list = _bProduct.Read();
            _products = new ObservableCollection<Product>(list);
            dgProducts.ItemsSource = _products;
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e) => LoadProducts();

        private void BtnNuevo_Click(object sender, RoutedEventArgs e)
        {
            txtId.Text = string.Empty;
            txtName.Text = string.Empty;
            txtPrice.Text = string.Empty;
            txtStock.Text = string.Empty;
            dgProducts.SelectedItem = null;
        }

        private void BtnInsert_Click(object sender, RoutedEventArgs e)
        {
            if (!decimal.TryParse(txtPrice.Text, out var price) || !int.TryParse(txtStock.Text, out var stock))
            {
                MessageBox.Show("Precio o stock inválidos.");
                return;
            }
            var p = new Product
            {
                Name = txtName.Text.Trim(),
                Price = price,
                Stock = stock
            };
            int newId = _bProduct.Insert(p);
            if (newId > 0)
            {
                p.ProductID = newId;
                p.Active = true;
                _products.Insert(0, p);
                MessageBox.Show("Insertado.");
                BtnNuevo_Click(sender, e);
            }
            else
            {
                MessageBox.Show("No se insertó.");
            }
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(txtId.Text, out int id))
            {
                MessageBox.Show("Seleccione un producto.");
                return;
            }
            if (!decimal.TryParse(txtPrice.Text, out var price) || !int.TryParse(txtStock.Text, out var stock))
            {
                MessageBox.Show("Valores inválidos.");
                return;
            }
            var p = new Product
            {
                ProductID = id,
                Name = txtName.Text.Trim(),
                Price = price,
                Stock = stock,
                Active = true
            };
            if (_bProduct.Update(p))
            {
                var local = _products.FirstOrDefault(x => x.ProductID == id);
                if (local != null)
                {
                    local.Name = p.Name;
                    local.Price = p.Price;
                    local.Stock = p.Stock;
                    dgProducts.Items.Refresh();
                }
                MessageBox.Show("Actualizado.");
            }
            else
            {
                MessageBox.Show("No se actualizó (posible inactivo).");
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(txtId.Text, out int id))
            {
                MessageBox.Show("Seleccione un producto.");
                return;
            }
            if (_bProduct.Delete(id))
            {
                var local = _products.FirstOrDefault(x => x.ProductID == id);
                if (local != null) local.Active = false;
                dgProducts.Items.Refresh();
                MessageBox.Show("Eliminación lógica aplicada.");
            }
            else
            {
                MessageBox.Show("No se eliminó.");
            }
        }

        private void dgProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgProducts.SelectedItem is Product p)
            {
                txtId.Text = p.ProductID.ToString();
                txtName.Text = p.Name;
                txtPrice.Text = p.Price.ToString("0.##");
                txtStock.Text = p.Stock.ToString();
            }
        }

        // Hover effects for DataGrid rows
        private void DataGridRow_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (sender is DataGridRow row)
            {
                row.Background = new SolidColorBrush(Color.FromRgb(237, 242, 255)); // light primary tint
            }
        }

        private void DataGridRow_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (sender is DataGridRow row)
            {
                row.ClearValue(BackgroundProperty);
            }
        }
    }
}