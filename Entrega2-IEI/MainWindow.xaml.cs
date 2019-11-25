using Entrega2_IEI.Library;
using Entrega2_IEI.Library.Scrapers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

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

        private async void BuscarButton_Click(object sender, RoutedEventArgs e)
        {
            SetBuscando(true);
            
            await Task.Run(() => Buscar(BusquedaListBox.Items));

            SetBuscando(false);
        }

        private void Buscar(ItemCollection items)
        {
            items.Clear();

            string brand = MarcaBox.Text, model = ModeloBox.Text;

            foreach (CheckBox box in ScraperBoxes.Children)
            {
                if (box.IsChecked ?? false)
                {
                    if (box.Tag is Type type)
                    {
                        if (typeof(IPhoneScraper).IsAssignableFrom(type))
                        {
                            IPhoneScraper scraper = (IPhoneScraper)Activator.CreateInstance(box.Tag as Type);
                            items.Add($"--------- {box.Content} ----------");

                            IList<Phone> phones = scraper.SearchPhone(brand, model);

                            foreach (Phone phone in phones)
                            {
                                items.Add(phone);
                            }
                        }
                        else
                        {
                            throw new ArgumentOutOfRangeException(
                                $"El objeto de la {nameof(CheckBox)} con contenido '{box.Content}' tiene un valor de un {nameof(Type)} " +
                                $"que no implementa la interfaz {nameof(IPhoneScraper)}.");
                        }
                    }
                    else
                    {
                        throw new ArgumentOutOfRangeException($"El objeto de la {nameof(CheckBox)} con contenido '{box.Content}' " +
                            $"tiene un valor que no es de tipo {nameof(Type)}.");
                    }
                }
            }

            if (items.Count > 0)
            {
                const string separator = "---------------------------------------------";
                items.Insert(0, separator);
                items.Insert(0, "Resultados de la búsqueda:");

                items.Add(separator);
            }
        }

        private void SetBuscando(bool buscando)
        {
            BuscarButton.IsEnabled = !buscando;
            BuscandoText.Visibility = buscando ? Visibility.Visible : Visibility.Hidden;
        }
    }
}
