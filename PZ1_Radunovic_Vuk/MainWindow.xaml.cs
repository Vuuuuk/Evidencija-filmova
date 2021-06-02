using PZ1_Radunovic_Vuk.Assets.Klase;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PZ1_Radunovic_Vuk
{
    public partial class MainWindow : Window
    {
        public static BindingList<Serije> lista_serija { get; set; }
        //public static int indentifikator = 0; //jedinstveni identifikator za svaki RTF fajl, kako bi izbegli mesanje
        public MainWindow()
        {
            if(lista_serija == null)
                lista_serija = new BindingList<Serije>();

            DataContext = this;
            InitializeComponent();
        }

        private void btn_dodaj_Click(object sender, RoutedEventArgs e)
        {
            Unos_Serijala us = new Unos_Serijala();
            us.ShowDialog();
        }

        private void btn_info_Click(object sender, RoutedEventArgs e)
        {
            Informacije inf = new Informacije();
            inf.ShowDialog();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void btn_izlaz_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Da li ste sigurni da želite da napustite program?", "Provera", MessageBoxButton.YesNo, MessageBoxImage.Information);
            if (result.Equals(MessageBoxResult.Yes))
                this.Close();
        }

        private void btn_procitaj_Click(object sender, RoutedEventArgs e)
        {
            Serijal_info si = new Serijal_info(dg_serije.SelectedIndex);
            si.ShowDialog();
        }

        private void btn_izmeni_Click(object sender, RoutedEventArgs e)
        {
            Izmena_Serijala iz = new Izmena_Serijala(dg_serije.SelectedIndex);
            iz.ShowDialog();
        }

        private void btn_obrisi_Click(object sender, RoutedEventArgs e)
        {
            lista_serija.RemoveAt(dg_serije.SelectedIndex);
        }

    }
}
