﻿using Entrega2_IEI.Library;
using Entrega2_IEI.Library.Scrapers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
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

            IList<object> resultados = new BindingList<object>();
            string brand = MarcaBox.Text, model = ModeloBox.Text;

            IList<PhoneScraper> scrapers = ObtenerScrapers(brand, model);

            BusquedaListBox.ItemsSource = resultados;

            await Task.Run(() =>
            {
                Buscar(scrapers, resultados);
            });

            SetBuscando(false);
        }

        private IList<PhoneScraper> ObtenerScrapers(string brand, string model)
        {
            IList<PhoneScraper> scrapers = new List<PhoneScraper>();

            foreach (CheckBox box in ScraperBoxes.Children)
            {
                if (box.IsChecked ?? false)
                {
                    if (box.Tag is Type type)
                    {
                        if (typeof(PhoneScraper).IsAssignableFrom(type))
                        {
                            bool showBrowser = ShowBrowserBox.IsChecked ?? false;
                            PhoneScraper scraper = (PhoneScraper)Activator.CreateInstance(box.Tag as Type, brand, model);
                            scraper.Config.ShowBrowser = showBrowser;
                            scrapers.Add(scraper);
                        }
                        else
                        {
                            throw new ArgumentOutOfRangeException(
                                $"El objeto de la {nameof(CheckBox)} con contenido '{box.Content}' tiene un valor de un {nameof(Type)} " +
                                $"que no implementa la interfaz {nameof(PhoneScraper)}.");
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

        private void Buscar(IEnumerable<PhoneScraper> scrapers, IList<object> resultados)
        {
            #region Local methods called from main thread
            void AddToResultados(object item)
            {
                Dispatcher.Invoke(() => resultados.Add(item));
            }
            #endregion

            foreach (PhoneScraper scraper in scrapers)
            {
                // Chapuza hasta poder hacerlo de una forma mejor
                // TODO: Mejorar esto
                string webPageName = scraper.GetType().Name;
                webPageName = webPageName.Remove(webPageName.Length - "Scraper".Length);

                AddToResultados($"--------- {webPageName} ----------");

                foreach (Phone phone in scraper.SearchPhone())
                {
                    AddToResultados(phone);
                }
            }
        }

        private void SetBuscando(bool buscando)
        {
            BuscarButton.IsEnabled = !buscando;
            ShowBrowserBox.IsEnabled = !buscando;
            BuscandoText.Visibility = buscando ? Visibility.Visible : Visibility.Hidden;
        }
    }
}
