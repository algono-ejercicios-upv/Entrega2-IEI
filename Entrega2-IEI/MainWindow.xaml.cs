using Entrega2_IEI.Library;
using Entrega2_IEI.Library.Scrapers;
using System;
using System.Collections.Generic;
using System.Globalization;
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

        private void BuscarButton_Click(object sender, RoutedEventArgs e)
        {
            BusquedaListBox.Items.Clear();
            foreach (CheckBox box in ScraperBoxes.Children)
            {
                if (box.IsChecked ?? false)
                {
                    if (box.Tag is Type type)
                    {
                        if (typeof(IPhoneScraper).IsAssignableFrom(type))
                        {
                            IPhoneScraper scraper = (IPhoneScraper)Activator.CreateInstance(box.Tag as Type);
                            IList<Phone> phones = scraper.SearchPhone(MarcaBox.Text, ModeloBox.Text);
                            foreach (Phone phone in phones)
                            {
                                BusquedaListBox.Items.Add(phone);
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
            
        }
    }
}
