using Entrega2_IEI.Library;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;

namespace Entrega2_IEI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            CultureInfo.CurrentCulture = new CultureInfo("es-ES");
        }

        private void BuscarButton_Click(object sender, RoutedEventArgs e)
        {
            BusquedaListBox.Items.Clear();
            List<Movil> phones;
            if (AmazonBox.IsChecked ?? false)
            {
                phones = BusquedaMoviles.BuscarAmazon(MarcaBox.Text, ModeloBox.Text);
                foreach (Movil phone in phones)
                {
                    BusquedaListBox.Items.Add(phone);
                }
            }
            
        }
    }
}
