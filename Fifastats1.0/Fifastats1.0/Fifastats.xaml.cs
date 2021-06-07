using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Fifastats1._0
{
    /// <summary>
    /// Interaktionslogik für Fifastats.xaml
    /// </summary>
    public partial class Fifastats : Window
    {
        ICollectionView collectionView;
        FifaStatsEntities1 FifaStatsEntities = new FifaStatsEntities1();
        ICollectionView collectionView2;
        ICollectionView collectionView3;
        public Fifastats()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            FifaStatsEntities.Mannschaften.Load();
            collectionView = CollectionViewSource.GetDefaultView(FifaStatsEntities.Mannschaften.Local);
            MainGrid.DataContext = collectionView;
            ErgebnisseGrid.DataContext = collectionView;
            FifaStatsEntities.Formationen.Load();
            collectionView2 = CollectionViewSource.GetDefaultView(FifaStatsEntities.Formationen.Local);
            Formation_Datagrid.ItemsSource = collectionView2;
            FifaStatsEntities.Spielergebnisse.Load();
            collectionView3 = CollectionViewSource.GetDefaultView(FifaStatsEntities.Spielergebnisse.Local);

            collectionView3.SortDescriptions.Add(new SortDescription("SpielID", ListSortDirection.Descending));
            Spielhistorie.ItemsSource = collectionView3;
            Statsgrid.DataContext = collectionView;

            Formationen_Statistiken_Datagrid.ItemsSource = collectionView2;



        }

        private void TextBox_Torwart_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Textbox_Torwart.Text = "";
        }
        private void TextBox_Defensive_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Textbox_Defensive.Text = "";
        }

        private void TextBox_Mittelfeld_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Textbox_Mittelfeld.Text = "";
        }

        private void TextBox_Sturm_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Textbox_Sturm.Text = "";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string Torwart = Textbox_Torwart.Text;
            string Defensive = Textbox_Defensive.Text;
            string Mittelfeld = Textbox_Mittelfeld.Text;
            string Sturm = Textbox_Sturm.Text;

            Mannschaften mannschaft = new Mannschaften();
            mannschaft.Torwart = Torwart;
            mannschaft.Defensive = Defensive;
            mannschaft.Mittelfeld = Mittelfeld;
            mannschaft.Sturm = Sturm;

            FifaStatsEntities.Mannschaften.Add(mannschaft);
            FifaStatsEntities.SaveChanges();
        }

        public void Button_egebniss_speichern_Click(object sender, RoutedEventArgs e)
        {
            int Tore = Convert.ToInt32(Tb_tore.Text);
            int Gegentore = Convert.ToInt32(Tb_gegentore.Text);
            object o = Datagrid_Mannschaft.SelectedItem;
            object u = Formation_Datagrid.SelectedItem;

            Spielergebnisse neuesspiel = new Spielergebnisse();
            neuesspiel.Tore = Tore;
            neuesspiel.Gegentore = Gegentore;

            if (o != null)
            {
                Mannschaften mannschaften = (Mannschaften)o;
                int MannschaftsID = mannschaften.MannschaftsID;
                neuesspiel.MannschaftsID = MannschaftsID;
            }

            if (u != null)
            {
                Formationen formationen = (Formationen)u;
                int FormationsID = formationen.FormationsID;
                neuesspiel.FormationsID = FormationsID;
            }


            FifaStatsEntities.Spielergebnisse.Add(neuesspiel);
            FifaStatsEntities.SaveChanges();

            Tb_tore.Clear();
            Tb_gegentore.Clear();

        }

        public void Datagrid_Mannschaft_Statistiken_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            object p = Datagrid_Mannschaft_Statistiken.SelectedItem;
            if (p is Mannschaften)
            {
                Mannschaften mannschaftP = (Mannschaften)p;
                int MannschaftsID_P = mannschaftP.MannschaftsID;

                var list = collectionView3;
                list.Filter = (x => ((Spielergebnisse)x).MannschaftsID.Equals(MannschaftsID_P));
                decimal Toregesamt = 0;
                decimal Spielanzahl = 0;
                decimal Gegentoregesamt = 0;
                decimal Gewonnen = 0;
                decimal Verloren = 0;
                foreach (Spielergebnisse item in list)
                {
                    Toregesamt = (decimal)item.Tore + Toregesamt;
                    Gegentoregesamt = (decimal)item.Gegentore + Gegentoregesamt;
                    Spielanzahl++;
                    if (item.Tore > item.Gegentore)
                    {
                        Gewonnen = Gewonnen + 1;
                    }
                    else if (item.Gegentore > item.Tore)
                    {
                        Verloren = Verloren + 1;
                    }



                }
                decimal ToreproSpieldurschnitt = Toregesamt / Spielanzahl;
                ToreproSpieldurschnitt = Math.Round(ToreproSpieldurschnitt, 3);
                decimal GegentoreproSpieldurschnitt = Gegentoregesamt / Spielanzahl;
                GegentoreproSpieldurschnitt = Math.Round(GegentoreproSpieldurschnitt, 3);
                TB_Tore_proSpiel.Text = Convert.ToString(ToreproSpieldurschnitt);
                TB_Gegntore_proSpiel.Text = Convert.ToString(GegentoreproSpieldurschnitt);
                if (Verloren == 0)
                {
                    Verloren = 1;
                }
                decimal SN_Verhältnis = Gewonnen / Verloren;
                SN_Verhältnis = Math.Round(SN_Verhältnis, 3);
                TB_SN_Verhältnis.Text = Convert.ToString(SN_Verhältnis);



            }


        }

        public void Formationen_Statistiken_Datagrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            object p = Formationen_Statistiken_Datagrid.SelectedItem;
            Formationen formationp = (Formationen)p;
            int FormationsID = formationp.FormationsID;

            object t = Datagrid_Mannschaft_Statistiken.SelectedItem;
            if (t is Mannschaften)
            {
                Mannschaften mannschaftP = (Mannschaften)t;

                int MannschaftsID_P = mannschaftP.MannschaftsID; ;

                var list = collectionView3;
                list.Filter = (x => ((Spielergebnisse)x).MannschaftsID.Equals(MannschaftsID_P) && ((Spielergebnisse)x).FormationsID.Equals(FormationsID));

                if (list != null)
                {

                    decimal Toregesamt = 0;
                    decimal Spielanzahl = 0;
                    decimal Gegentoregesamt = 0;
                    decimal Gewonnen = 0;
                    decimal Verloren = 0;
                    foreach (Spielergebnisse item in list)
                    {
                        Toregesamt = (decimal)item.Tore + Toregesamt;
                        Gegentoregesamt = (decimal)item.Gegentore + Gegentoregesamt;
                        Spielanzahl++;
                        if (item.Tore > item.Gegentore)
                        {
                            Gewonnen = Gewonnen + 1;
                        }
                        else if (item.Gegentore > item.Tore)
                        {
                            Verloren = Verloren + 1;
                        }

                        decimal ToreproSpieldurschnitt = Toregesamt / Spielanzahl;
                        ToreproSpieldurschnitt = Math.Round(ToreproSpieldurschnitt, 3);
                        decimal GegentoreproSpieldurschnitt = Gegentoregesamt / Spielanzahl;
                        GegentoreproSpieldurschnitt = Math.Round(GegentoreproSpieldurschnitt, 3);
                        Formationen_Tore_pro_Spiel.Text = Convert.ToString(ToreproSpieldurschnitt);
                        if (Verloren == 0)
                        {
                            Verloren = 1;
                        }
                        decimal SN_Verhältnis = Gewonnen / Verloren;
                        SN_Verhältnis = Math.Round(SN_Verhältnis, 3);
                        TB_SN_Verhältnis_Formationen.Text = Convert.ToString(SN_Verhältnis);
                        Formationen_Gegentore_pro_Spiel.Text = Convert.ToString(GegentoreproSpieldurschnitt);

                    }



                }
                if (list == null)
                {
                    TB_SN_Verhältnis_Formationen.Text = "Keine Daten";
                    Formationen_Gegentore_pro_Spiel.Text = "Keine Daten";
                    Formationen_Tore_pro_Spiel.Text = "Keine Daten";
                }

            }

        }


    }
}
