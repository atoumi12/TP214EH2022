using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MonCine.Data;

namespace MonCine.Vues
{
    /// <summary>
    /// Logique d'interaction pour FAbonnes.xaml
    /// </summary>
    public partial class FAbonne : Page
    {
        private List<Abonne> abonnes;
        private DAL Dal { get; set; }
        private Abonne abonne;

        public FAbonne(DAL dal, Abonne unAbonne)
        {
            InitializeComponent();
            abonnes = dal.ReadAbonnes();
            Dal = dal;
            abonne = unAbonne;
            InitialConfiguration();
        }



        /// <summary>
        /// Définit l'état inital du form
        /// </summary>
        private void InitialConfiguration()
        {
            UserInfos.Text = $"{abonne.FirstName} {abonne.LastName} :" +
                $"\n" +
                $"\n" +
                $"Acteur favorie : {abonne.ActeurFavorie}" +
                $"\n" +
                $"\n" +
                $"Realisateur favorie : {abonne.RealisateurFavorie}" +
                $"\n" +
                $"\n" +
                $"Seance assister : {abonne.nbSeanceAssistees}";
        }


        private void BtnReturn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.NavigationService.Navigate(new FAbonnes(new DALAbonne()));
        }



        private async void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
        //    if (LstAbonnes.SelectedIndex == -1)
        //    {
        //        MessageBox.Show("Veuillez choisir un Abonne pour le modifier", "Erreur",
        //            MessageBoxButton.OK, MessageBoxImage.Information);
        //    }
        //    else
        //    {

        //        //Abonne abonne = (Abonne)LstAbonnes.SelectedItem;
        //        //UpdateAbonne(abonne);
        //        //var result = await Dal.UpdateAbonne(abonne);

        //        //if (result)
        //        //{
        //        //    NameField.Text = "";
        //        //    UpdateItems();
        //        //    MessageBox.Show($"Le film {abonne.Username} a été mis à jour avec succès !", "Modification", MessageBoxButton.OK, MessageBoxImage.None);
        //        //}

        //    }
        }

        private void UpdateAbonne(Abonne pAbonne)
        {
            //pAbonne.Username = NameField.Text;
        }

        /// <summary>
        /// Permet de mettre à jour les données des éléments affichés
        /// </summary>
        private void UpdateItems()
        {
           // LstAbonnes.ItemsSource = Dal.ReadAbonnes();
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {

        }

        public override string ToString()
        {
            return $"{Name}";
        }

        private void NameField_TextChanged(object sender, TextChangedEventArgs e)
        {


        }

        private void BtnOffrirReconpense_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}