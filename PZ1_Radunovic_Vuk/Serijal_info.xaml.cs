using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PZ1_Radunovic_Vuk
{
    public partial class Serijal_info : Window
    {

        TextRange ispis; //globalna promenjiva za ucitavanje u rtb

        public Serijal_info(int index)
        {
            InitializeComponent();

            //RTF_CITANJE
            FileStream fileStream = new FileStream(MainWindow.lista_serija[index].Naslov +
            "_" + MainWindow.lista_serija[index].Zanr + "_" +
            MainWindow.lista_serija[index].Datum_izlaska, FileMode.Open);
            ispis = new TextRange(rtxt_opis.Document.ContentStart, rtxt_opis.Document.ContentEnd);
            ispis.Load(fileStream, DataFormats.Rtf);
            fileStream.Close();
            //RTF_CITANJE

            //INICIJALIZACIJA_POLJA
            img_poster.Source = new BitmapImage(new Uri(MainWindow.lista_serija[index].Poster));
            //INICIJALIZACIJA_POLJA

        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void btn_izlaz_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
