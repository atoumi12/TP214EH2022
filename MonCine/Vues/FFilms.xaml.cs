using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Logique d'interaction pour Films.xaml
    /// </summary>
    public partial class FFilms : Page
    {
        private List<Film> Films { get; set; }
        private DALFilm Dal { get; set; }


        public FFilms(DALFilm pDal)
        {
            InitializeComponent();
            Dal = pDal;


            InitialConfiguration();
        }

        /// <summary>
        /// Définit l'état inital du form
        /// </summary>
        private void InitialConfiguration()
        {
            InitialiseListView();
            PopulateCategory();

            BtnDelete.IsEnabled = false;
            BtnUpdate.IsEnabled = false;
            NameField.Text = "";
        }

        private void InitialiseListView()
        {
            Films = Dal.ReadItems();
            LstFilms.ItemsSource = Films;

            GridViewColumnNote.DisplayMemberBinding = new Binding("CalculerMoyennesNotes");
        }

        private void PopulateCategory()
        {
            List<String> categories = typeof(Categorie).GetEnumNames().ToList();

            foreach (string cat in categories)
            {
                CategoryCombobox.Items.Add(cat);
            }
        }


        /// <summary>
        /// Permet de mettre à jour les données des éléments affichés
        /// </summary>
        private void RefreshItems()
        {
            LstFilms.ItemsSource = Dal.ReadItems();
        }

        /// <summary>
        /// Permet de retourn à l'accueil.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnReturn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            NavigationService?.Navigate(new Accueil());
        }


        private void LstFilms_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Film film = (Film)LstFilms.SelectedItem;

            NameField.Text = film?.Name;
            CategoryCombobox.SelectedIndex = film != null ? (int)(Categorie)film?.Categories[0] : -1;

            BtnDelete.IsEnabled = film != null;
            BtnUpdate.IsEnabled = film != null;
        }


        // Add
        private async void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (NameField.Text.Length == 0)
            {
                MessageBox.Show("Veuillez remplir les champs nécéssaires", "Création", MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            else
            {
                Film film = CreateFilmToAdd();
                var result = await Dal.AddItem(film);
                if (result)
                {
                    RefreshItems();
                    MessageBox.Show($"Le film {film.Name} a été crée avec succès !", "Création de film",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private Film CreateFilmToAdd()
        {
            // Recuperer les champs
            string nom = NameField.Text;
            Categorie categorie = (Categorie)CategoryCombobox.SelectedIndex;

            // Vider les champs
            NameField.Text = "";
            CategoryCombobox.SelectedIndex = -1;


            Film film = new Film(nom,
                new List<Categorie>() { categorie }
            );

            return film;
        }


        // Update
        private async void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (LstFilms.SelectedIndex == -1)
            {
                MessageBox.Show("Veuillez choisir un film pour le modifier", "Modification",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                Film film = (Film)LstFilms.SelectedItem;
                UpdateFilm(film);
                var result = await Dal.UpdateItem(film);

                if (result)
                {
                    NameField.Text = "";
                    RefreshItems();
                    MessageBox.Show($"Le film {film.Name} a été mis à jour avec succès !", "Modification",
                        MessageBoxButton.OK, MessageBoxImage.None);
                }
            }
        }

        private void UpdateFilm(Film pFilm)
        {
            pFilm.Name = NameField.Text;
            pFilm.Categories[0] = (Categorie)CategoryCombobox.SelectedIndex;
        }


        // Delete
        private async void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (LstFilms.SelectedIndex == -1)
            {
                MessageBox.Show("Veuillez choisir un film pour le supprimer", "Suppression",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                Film film = (Film)LstFilms.SelectedItem;
                var result = await Dal.DeleteItem(film);

                if (result)
                {
                    NameField.Text = "";
                    RefreshItems();
                    MessageBox.Show($"Le film {film.Name} a été supprimé avec succès !", "Suppression",
                        MessageBoxButton.OK, MessageBoxImage.None);
                }
            }
        }
    }
}