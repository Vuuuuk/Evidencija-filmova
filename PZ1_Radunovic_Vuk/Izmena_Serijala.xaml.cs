using Microsoft.Win32;
using PZ1_Radunovic_Vuk.Assets.Klase;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
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
    public partial class Izmena_Serijala : Window
    {
        public int Index { get; }
        TextRange izmena; //globalna za citanje/izmenu iz postojeceg rtf fajla

        public Izmena_Serijala(int index)
        {
            InitializeComponent();

            Index = index; //globalan pristup prosledjenom paramteru/indexu selektovanog serijala

            //FORMATIRANJE RTB-a
            cmb_toolbar_FontFamily.ItemsSource = Fonts.SystemFontFamilies.OrderBy(f => f.Source); //uzimamo sistemske fontove, sortiranje po imenu
            cmb_toolbar_boja.ItemsSource = Toolbar.lista_boja; //kreirana lista boja, proveriti da li postoji neka jednostavnija implementacija
            cmb_toolbar_boja.SelectedValue = Toolbar.lista_boja[0];
            cmb_toolbar_font.ItemsSource = Toolbar.font_velicina;//kreirana lista velicina
            cmb_toolbar_font.SelectedValue = Toolbar.font_velicina[1];
            rtxt_opis.FontSize = double.Parse(cmb_toolbar_font.SelectedValue.ToString());//setovanje na pocetno selektovanu velicinu
            //FORMATIRANJE RTB-a

            //RTF_CITANJE
            FileStream fileStream = new FileStream(MainWindow.lista_serija[Index].Rtb_ime, FileMode.Open);
            izmena = new TextRange(rtxt_opis.Document.ContentStart, rtxt_opis.Document.ContentEnd);
            izmena.Load(fileStream, DataFormats.Rtf);
            fileStream.Close();
            //RTF_CITANJE

            //INICIJALIZACIJA_POLJA
            txt_datum.Text = MainWindow.lista_serija[index].Datum_izlaska;
            txt_naslov.Text = MainWindow.lista_serija[index].Naslov;
            if (MainWindow.lista_serija[index].Broj_sezona == 2)
                rb_2_sezone.IsChecked = true;
            if (MainWindow.lista_serija[index].Broj_sezona == 4)
                rb_4_sezone.IsChecked = true;
            if (MainWindow.lista_serija[index].Broj_sezona == 6)
                rb_6_sezone.IsChecked = true;
            cmb_zanr.ItemsSource = Zanr.lista_zanrova;
            cmb_zanr.SelectedItem = MainWindow.lista_serija[index].Zanr;
            img_poster.Source = new BitmapImage(new Uri(MainWindow.lista_serija[index].Poster));
            //INICIJALIZACIJA_POLJA
        }

        bool Validacija()
        {
            bool provera = true; //optimisticno programiranje xd
            if (txt_datum.Text.Trim().Equals(string.Empty) || txt_datum.Text.Trim().Equals("Unesite datum izdavanja serijala ovde.")
            || new Regex("[0-9]{2}(-)[0-9]{2}(-)[0-9]{4}").IsMatch(txt_datum.Text.Trim()) != true) //test implementacija bez DateTime, proveriti da li je ok
            {
                lbl_datum_greska.Content = "*";
                provera = false;
            }
            else
                lbl_datum_greska.Content = string.Empty;
            if (txt_naslov.Text.Trim().Equals(string.Empty) || txt_naslov.Text.Trim().Equals("Unesite naslov serijala ovde."))
            {
                lbl_naslov_greska.Content = "*";
                provera = false;
            }
            else
                lbl_naslov_greska.Content = string.Empty;
            if (rb_2_sezone.IsChecked == false && rb_4_sezone.IsChecked == false && rb_6_sezone.IsChecked == false)
            {
                lbl_broj_sezona_greska.Content = "*";
                provera = false;
            }
            else
                lbl_broj_sezona_greska.Content = string.Empty;
            if (cmb_zanr.SelectedValue.ToString().Trim().Equals("Odaberite zanr serijala."))
            {
                lbl_zanr_greska.Content = "*";
                provera = false;
            }
            else
                lbl_zanr_greska.Content = string.Empty;
            if (img_poster.Source == null)
            {
                lbl_image_greska.Content = "*";
                provera = false;
            }
            else
                lbl_image_greska.Content = string.Empty;
            if (rtb_text(rtxt_opis).Trim().Equals(string.Empty) || rtb_text(rtxt_opis).Trim().Equals("Unesite opis serijala ovde."))
            {
                lbl_rtxt_greska.Content = "*";
                provera = false;
            }
            else
                lbl_rtxt_greska.Content = string.Empty;
            return provera;
        }

        private void txt_datum_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txt_datum.Text.Trim().Equals("Unesite datum izdavanja serijala ovde."))
                txt_datum.Text = string.Empty;
        }

        private void txt_datum_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txt_datum.Text.Trim().Equals(string.Empty))
                txt_datum.Text = "Unesite datum izdavanja serijala ovde.";
        }
        private void txt_naslov_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txt_naslov.Text.Trim().Equals("Unesite naslov serijala ovde."))
                txt_naslov.Text = string.Empty;
        }

        private void txt_naslov_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txt_naslov.Text.Trim().Equals(string.Empty))
                txt_naslov.Text = "Unesite naslov serijala ovde.";
        }
        private void rtxt_opis_GotFocus(object sender, RoutedEventArgs e)
        {
            if (rtb_text(rtxt_opis).Trim().Equals("Unesite opis serijala ovde."))
                rtxt_opis.Document.Blocks.Clear();
        }

        private void rtxt_opis_LostFocus(object sender, RoutedEventArgs e)
        {
            if (rtb_text(rtxt_opis).Trim().Equals(string.Empty))
                rtxt_opis.Document.Blocks.Add(new Paragraph(new Run("Unesite opis serijala ovde.")));
        }

        private void btn_potvrda_Click(object sender, RoutedEventArgs e)
        {

            Serije nova_serija = new Serije(); //kako bih mogli da pristupimo polju potrebnom za referenciranje RTB-a

            if (Validacija())
            {

                if (rb_2_sezone.IsChecked == true)
                    MainWindow.lista_serija[Index] = (nova_serija = new Serije(img_poster.Source.ToString(), txt_datum.Text, 2, txt_naslov.Text, cmb_zanr.SelectedItem.ToString(), MainWindow.lista_serija[Index].Naslov +
                "_" + MainWindow.lista_serija[Index].Zanr + "_" +
                MainWindow.lista_serija[Index].Datum_izlaska));
                if (rb_4_sezone.IsChecked == true)
                    MainWindow.lista_serija[Index] = (nova_serija = new Serije(img_poster.Source.ToString(), txt_datum.Text, 4, txt_naslov.Text, cmb_zanr.SelectedItem.ToString(), MainWindow.lista_serija[Index].Naslov +
                "_" + MainWindow.lista_serija[Index].Zanr + "_" +
                MainWindow.lista_serija[Index].Datum_izlaska));
                if (rb_6_sezone.IsChecked == true)
                    MainWindow.lista_serija[Index] = (nova_serija = new Serije(img_poster.Source.ToString(), txt_datum.Text, 6, txt_naslov.Text, cmb_zanr.SelectedItem.ToString(), MainWindow.lista_serija[Index].Naslov +
                "_" + MainWindow.lista_serija[Index].Zanr + "_" +
                MainWindow.lista_serija[Index].Datum_izlaska));

                //RTF_IZMENA
                FileStream fileStream = new FileStream(MainWindow.lista_serija[Index].Rtb_ime, FileMode.Create);
                izmena = new TextRange(rtxt_opis.Document.ContentStart, rtxt_opis.Document.ContentEnd);
                izmena.Save(fileStream, DataFormats.Rtf);
                fileStream.Close();
                //RTF_IZMENA

                MessageBox.Show("Serijal uspesno modifikovan!", "Obavestenje", MessageBoxButton.OK, MessageBoxImage.Information);

            }
            else
            {
                MessageBox.Show("Molimo proverite da li ste uneli sve podatke!", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void rtxt_opis_SelectionChanged(object sender, RoutedEventArgs e)
        {
            int broj_reci = Regex.Matches(rtb_text(rtxt_opis), @"[^\s]+").Count; //brojimo sve do space-a, space se izbacuje. Podrazumevan format [Cao. razmak]
            tb_broj_reci.Text = "                                            Trenutan broj reci u tekstu: " + broj_reci;

            object bold = rtxt_opis.Selection.GetPropertyValue(Inline.FontWeightProperty); //proveravamo da li je selektovani tekst boldovan ili ne
            btn_toolbar_bold.IsChecked = (bold != DependencyProperty.UnsetValue) && (bold.Equals(FontWeights.Bold)); //da li je dugme selectovano (true) ili ne
            //DependancyProperty selektuje propertije koji se menjaju kroz neke "animacije", tj. da li je neki stil primenjen na nas text

            object italic = rtxt_opis.Selection.GetPropertyValue(Inline.FontStyleProperty); //proveravamo da li je selektovani tekst italic
            btn_toolbar_italic.IsChecked = (italic != DependencyProperty.UnsetValue) && (italic.Equals(FontStyles.Italic)); //italic

            object underline = rtxt_opis.Selection.GetPropertyValue(Inline.TextDecorationsProperty); //proveravamo da li je selektovani text underline
            btn_toolbar_underline.IsChecked = (underline != DependencyProperty.UnsetValue) && (underline.Equals(TextDecorations.Underline)); //underline

            object font = rtxt_opis.Selection.GetPropertyValue(Inline.FontFamilyProperty); //izvlacimo property fonta iz selektovanog teksa
            cmb_toolbar_FontFamily.SelectedItem = font; //ispisujemo bas taj selektovani font u cb-u

            object velicina = rtxt_opis.Selection.GetPropertyValue(Inline.FontSizeProperty); //izvlacimo propery velicina_fonta iz selktovanog teksta
            cmb_toolbar_font.SelectedItem = velicina.ToString(); //ispisujemo bas tu velicinu u cb-u

            //proveriti i dodati za boju
        }

        private void cmb_FontFamily_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmb_toolbar_FontFamily.SelectedItem != null) //ukoliko cb za font_stila nije prazan
                rtxt_opis.Selection.ApplyPropertyValue(Inline.FontFamilyProperty, cmb_toolbar_FontFamily.SelectedItem); //izabrani font, primenjujemo na text
        }

        private void cmb_toolbar_font_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmb_toolbar_font.SelectedItem != null) //ukoliko cb za velicinu_fonta nije prazan
                rtxt_opis.Selection.ApplyPropertyValue(Inline.FontSizeProperty, cmb_toolbar_font.SelectedItem); //izabranu velicinu, primenjujemo na text
        }

        private void cmb_toolbar_boja_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmb_toolbar_boja.SelectedItem != null) //ukoliko cb za boju nije prazan
                rtxt_opis.Selection.ApplyPropertyValue(Inline.ForegroundProperty, cmb_toolbar_boja.SelectedItem); //izabranu boju, primenjujemo na text
        }

        private void btn_pretraga_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Izaberite sliku";
            if (op.ShowDialog() == true)
                img_poster.Source = new BitmapImage(new Uri(op.FileName));
        }

        string rtb_text(RichTextBox rtb)
        {
            TextRange tr = new TextRange(rtb.Document.ContentStart, rtb.Document.ContentEnd);

            return tr.Text;
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
