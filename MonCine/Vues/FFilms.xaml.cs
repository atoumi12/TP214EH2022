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
        private DALFilm _dalFilm { get; set; }
        private DALActeur _dalActeur { get; set; }
        private DALRealisateur _dalRealisateur { get; set; }
        private DALProjection _dalProjection { get; set; }


        public FFilms(DALFilm pDalFilm, DALActeur pDalActeur, DALRealisateur pDalRealisateur,
            DALProjection pDalProjection)
        {
            _dalFilm = pDalFilm;
            _dalActeur = pDalActeur;
            _dalRealisateur = pDalRealisateur;
            _dalProjection = pDalProjection;

            InitializeComponent();

            InitialItemConfiguration();
        }

        /// <summary>
        /// Définit l'état inital du form
        /// </summary>
        private void InitialItemConfiguration()
        {
            InitialiseListView();
            PopulateCheckBoxesInStackPannel();

            ClearFields();

            BtnDelete.IsEnabled = false;
            BtnUpdate.IsEnabled = false;
            BtnAfficherProjections.IsEnabled = false;
        }

        private void ClearFields()
        {
            // Vider les champs
            NameField.Text = "";
            CkbAffiche.IsChecked = false;


            foreach (StackPanel sp in CategoryStackPanel.Children)
            {
                foreach (CheckBox checkbox in sp.Children)
                {
                    checkbox.IsChecked = false;
                }
            }

            foreach (CheckBox checkBox in ActeurStackPanel.Children)
            {
                checkBox.IsChecked = false;
            }

            foreach (CheckBox checkbox in RealisateurStackPanel.Children)
            {
                checkbox.IsChecked = false;
            }
        }

        private void InitialiseListView()
        {
            Films = _dalFilm.ReadItems();
            LstFilms.ItemsSource = Films;
        }


        /// <summary>
        /// Permet de populer l'ensemble des checkbox (Catégorie, Acteur, Réalisateur)
        /// </summary>
        private void PopulateCheckBoxesInStackPannel()
        {
            // Categorie
            List<String> categories = typeof(Categorie).GetEnumNames().ToList();
            for (int i = 0; i < categories.Count; i += 3)
            {
                StackPanel sp = new StackPanel();
                sp.Orientation = Orientation.Horizontal;
                sp.Margin = new Thickness(0, 2, 0, 2);
                for (int j = i; j < i + 3; j++)
                {
                    CheckBox cb = new CheckBox();
                    cb.Margin = new Thickness(2, 0, 2, 0);
                    cb.Content = categories[j];
                    sp.Children.Add(cb);
                }

                CategoryStackPanel.Children.Add(sp);
            }

            // Acteurs
            List<Acteur> acteurs = _dalActeur.ReadItems();
            foreach (Acteur acteur in acteurs)
            {
                CheckBox chkb = new CheckBox();
                chkb.Content = acteur.ToString();
                chkb.Tag = acteur;

                ActeurStackPanel.Children.Add(chkb);
            }

            // Realisateurs
            List<Realisateur> realisateurs = _dalRealisateur.ReadItems();
            foreach (Realisateur realisateur in realisateurs)
            {
                CheckBox chkb = new CheckBox();
                chkb.Content = realisateur.ToString();
                chkb.Tag = realisateur;

                RealisateurStackPanel.Children.Add(chkb);
            }
        }


        /// <summary>
        /// Permet de mettre à jour les données des éléments affichés et récupérer la nouvelle liste des films
        /// </summary>
        private void RefreshItems()
        {
            LstFilms.ItemsSource = _dalFilm.ReadItems();
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

            if (film != null)
            {
                NameField.Text = film.Name;

                // Categories
                foreach (StackPanel sp in CategoryStackPanel.Children)
                {
                    foreach (CheckBox cb in sp.Children)
                    {
                        bool exists = film.Categories.Exists(cat => cat.ToString() == cb.Content.ToString());
                        cb.IsChecked = exists;
                    }
                }

                // Acteurs
                foreach (CheckBox checkbox in ActeurStackPanel.Children)
                {
                    Acteur acteurTag = checkbox.Tag as Acteur;
                    bool exists = film.Acteurs.Exists(acteur => acteur.Id == acteurTag.Id);
                    checkbox.IsChecked = exists;
                }

                // Realisateurs
                foreach (CheckBox checkbox in RealisateurStackPanel.Children)
                {
                    Realisateur realisateurTag = checkbox.Tag as Realisateur;
                    bool exists = film.Realisateurs.Exists(realisateur => realisateur.Id == realisateurTag.Id);
                    checkbox.IsChecked = exists;
                }

                CkbAffiche.IsChecked = film.SurAffiche;
            }


            BtnDelete.IsEnabled = film != null;
            BtnUpdate.IsEnabled = film != null;
            BtnAfficherProjections.IsEnabled = film != null;
        }

        /// <summary>
        /// Permet de créer un objet film à partir des valeurs saisies dans les champs
        /// </summary>
        /// <returns>Film avec les valeurs saisies</returns>
        private Film GetFilmFromValues()
        {
            // Recuperer les champs
            string nom = NameField.Text;
            bool surAffiche = CkbAffiche.IsChecked.Value;


            List<Categorie> categories = new List<Categorie>();
            foreach (StackPanel sp in CategoryStackPanel.Children)
            {
                foreach (CheckBox checkBox in sp.Children)
                {
                    if (checkBox.IsChecked.Value)
                    {
                        Categorie cat;
                        if (Enum.TryParse<Categorie>(checkBox.Content.ToString(), true, out cat))
                        {
                            categories.Add(cat);
                        }
                    }
                }
            }


            List<Acteur> acteurs = new List<Acteur>();
            foreach (CheckBox checkBox in ActeurStackPanel.Children)
            {
                if (checkBox.IsChecked.Value)
                {
                    acteurs.Add(checkBox.Tag as Acteur);
                }
            }


            List<Realisateur> realisateurs = new List<Realisateur>();
            foreach (CheckBox checkbox in RealisateurStackPanel.Children)
            {
                if (checkbox.IsChecked.Value)
                {
                    realisateurs.Add(checkbox.Tag as Realisateur);
                }
            }


            ClearFields();

            Film film = new Film(nom,
                categories,
                acteurs,
                realisateurs,
                surAffiche);

            return film;
        }


        /// <summary>
        /// Permet la création d'un film
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            var (valide, erreurs) = FilmIsValid();

            if (!valide)
            {
                MessageBox.Show($"Veuillez remplir les champs nécéssaires \n\n\n{erreurs}", "Création d'un film",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            else
            {
                Film film = GetFilmFromValues();
                var result = _dalFilm.AddItem(film);
                if (result)
                {
                    RefreshItems();
                    MessageBox.Show($"Le film '{film.Name}' a été crée avec succès !", "Création de film",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }


        /// <summary>
        /// Permet de valider les données insérées dans les champs d'un film
        /// </summary>
        /// <returns></returns>
        private (bool, string) FilmIsValid()
        {
            bool valide = true;
            string erreurs = "";

            // Nom
            if (NameField.Text.Length == 0)
            {
                erreurs += " - Veuillez remplir le nom du film \n";
            }

            // Categories
            int nbCategories = 0;
            foreach (StackPanel sp in CategoryStackPanel.Children)
            {
                foreach (CheckBox checkBox in sp.Children)
                {
                    if (checkBox.IsChecked.Value)
                    {
                        nbCategories++;
                    }
                }
            }

            if (nbCategories <= 0)
            {
                erreurs += " - Veuillez choisir au moins une catégorie du film \n";
            }

            // Acteurs
            int nbActeurs = 0;
            foreach (CheckBox checkBox in ActeurStackPanel.Children)
            {
                if (checkBox.IsChecked.Value)
                {
                    nbActeurs++;
                }
            }

            if (nbActeurs <= 0)
            {
                erreurs += " - Veuillez choisir au moins un acteur pour le film \n";
            }


            // Realisateurs
            int nbRealisateurs = 0;
            foreach (CheckBox checkBox in ActeurStackPanel.Children)
            {
                if (checkBox.IsChecked.Value)
                {
                    nbRealisateurs++;
                }
            }

            if (nbRealisateurs <= 0)
            {
                erreurs += " - Veuillez choisir au moins un réalisateur pour le film \n";
            }


            if (!string.IsNullOrWhiteSpace(erreurs))
            {
                valide = false;
            }

            return (valide, erreurs);
        }


        /// <summary>
        /// Permet la mise à jour d'un film
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (LstFilms.SelectedIndex == -1)
            {
                MessageBox.Show("Veuillez choisir un film pour le modifier", "Modification",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                var (valide, erreurs) = FilmIsValid();

                if (!valide)
                {
                    MessageBox.Show($"Veuillez remplir les champs nécéssaires \n\n\n{erreurs}",
                        "Modification d'un film", MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
                else
                {
                    Film selectedFilm = (Film)LstFilms.SelectedItem;
                    Film updatedFilm = GetFilmFromValues();
                    updatedFilm.Id = selectedFilm.Id;

                    var result = _dalFilm.UpdateItem(updatedFilm);

                    if (result)
                    {
                        RefreshItems();
                        MessageBox.Show($"Le film '{updatedFilm.Name}' a été mis à jour avec succès !", "Modification",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
        }


        /// <summary>
        /// Permet la suppression d'un film
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (LstFilms.SelectedIndex == -1)
            {
                MessageBox.Show("Veuillez choisir un film pour le supprimer", "Suppression",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                Film film = (Film)LstFilms.SelectedItem;
                var result = _dalFilm.DeleteItem(film);

                if (result)
                {
                    NameField.Text = "";
                    RefreshItems();
                    MessageBox.Show($"Le film '{film.Name}' a été supprimé avec succès !", "Suppression",
                        MessageBoxButton.OK, MessageBoxImage.None);
                }
            }
        }


        /// <summary>
        /// Accède à la fenêtre des projections
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAfficherProjections_Click(object sender, RoutedEventArgs e)
        {
            Film film = LstFilms.SelectedItem as Film;
            if (film is null)
            {
                MessageBox.Show("Veuillez sélectionnez un film", "Affichage de projection", MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            else
            {
                FProjectionFilm projectionFilm = new FProjectionFilm(_dalProjection, film);
                projectionFilm.Show();
            }
        }



        private void BtnViderChamps_Click(object sender, RoutedEventArgs e)
        {
            ClearFields();

            LstFilms.SelectedIndex = -1;
            BtnDelete.IsEnabled = false;
            BtnUpdate.IsEnabled = false;
        }
    }
}