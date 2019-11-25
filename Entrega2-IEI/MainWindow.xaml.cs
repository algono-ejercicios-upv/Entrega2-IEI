using Entrega2_IEI.Library;
using Entrega2_IEI.Library.Scrapers;
using System;
using System.Collections;
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

            IList resultados = null;
            string brand = MarcaBox.Text, model = ModeloBox.Text;

            IList<IPhoneScraper> scrapers = ObtenerScrapers();

            await Task.Run(() =>
            {
                resultados = Buscar(brand, model, scrapers);
            });

            BusquedaListBox.ItemsSource = resultados;

            SetBuscando(false);
        }

        private IList<IPhoneScraper> ObtenerScrapers()
        {
            IList<IPhoneScraper> scrapers = new List<IPhoneScraper>();

            foreach (CheckBox box in ScraperBoxes.Children)
            {
                if (box.IsChecked ?? false)
                {
                    if (box.Tag is Type type)
                    {
                        if (typeof(IPhoneScraper).IsAssignableFrom(type))
                        {
                            IPhoneScraper scraper = (IPhoneScraper)Activator.CreateInstance(box.Tag as Type);
                            scrapers.Add(scraper);
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

            return scrapers;
        }

        private IList Buscar(string brand, string model, IEnumerable<IPhoneScraper> scrapers)
        {
            IList resultados = new List<object>();

            foreach (IPhoneScraper scraper in scrapers)
            {
                // Chapuza hasta poder hacerlo de una forma mejor
                // TODO: Mejorar esto
                string webPageName = scraper.GetType().Name;
                webPageName = webPageName.Remove(webPageName.Length - "Scraper".Length);

                resultados.Add($"--------- {webPageName} ----------");

                IList<Phone> phones = scraper.SearchPhone(brand, model);

                foreach (Phone phone in phones)
                {
                    resultados.Add(phone);
                }
            }

            if (resultados.Count > 0)
            {
                const string separator = "---------------------------------------------";
                resultados.Insert(0, separator);
                resultados.Insert(0, "Resultados de la búsqueda:");

                resultados.Add(separator);
            }

            return resultados;
        }

        private void SetBuscando(bool buscando)
        {
            BuscarButton.IsEnabled = !buscando;
            BuscandoText.Visibility = buscando ? Visibility.Visible : Visibility.Hidden;
        }
    }
}
