using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using System.Windows;
using System.Xaml;

namespace MonCine.Data
{
    public class DAL
    {
        private IMongoClient mongoDBClient;
        private IMongoDatabase database;

        public DAL(IMongoClient client = null)
        {
            mongoDBClient = client ?? OuvrirConnexion();
            database = ConnectDatabase();

            AddDefaultFilms();
            AddDefaultAbo();
        }

        private IMongoClient OuvrirConnexion()
        {
            MongoClient dbClient = null;
            try
            {
                dbClient = new MongoClient("mongodb://localhost:27017/");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Impossible de se connecter à la base de données " + ex.Message, "Erreur");
            }

            return dbClient;
        }

        private IMongoDatabase ConnectDatabase()
        {
            IMongoDatabase db = null;
            try
            {
                db = mongoDBClient.GetDatabase("TP2DB");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Impossible de se connecter à la base de données " + ex.Message, "Erreur");
            }

            return db;
        }

        public List<Abonne> ReadAbonnes()
        {
            List<Abonne> abonnes = new List<Abonne>();

            try
            {
                var collection = database.GetCollection<Abonne>("Abonnes");
                abonnes = collection.Aggregate().ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Impossible d'obtenir la collection " + ex.Message, "Erreur", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }

            return abonnes;
        }


        #region Films

        /// <summary>
        /// Récupère l'ensemble des films de la BD
        /// </summary>
        /// <returns>Liste de films</returns>
        public List<Film> ReadFilm()
        {
            List<Film> films = new List<Film>();

            try
            {
                var collection = database.GetCollection<Film>("Films");
                films = collection.Aggregate().ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Impossible d'obtenir la collection " + ex.Message, "Erreur", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }

            return films;
        }


        /// <summary>
        /// Populer la collections films au départ
        /// </summary>
        private async void AddDefaultFilms()
        {
            List<Film> films = new List<Film>
            {
                new Film("Film 1"),
                new Film("Film 2"),
                new Film("Film 3")
            };

            try
            {
                var collection = database.GetCollection<Film>("Films");
                if (collection.CountDocuments(Builders<Film>.Filter.Empty) <= 0)
                {
                    await collection.InsertManyAsync(films);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Impossible d'ajouter des films dans la collection " + ex.Message, "Erreur",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                throw;
            }
        }

        private async void AddDefaultAbo()
        { 
            List<Abonne> abonnes = new List<Abonne>
            {
                new Abonne("Abonne 1"),
                new Abonne("Abonne 2"),
                new Abonne("Abonne 3")
            };

            try
            {
                database.DropCollection("Abonnes");
                var collection = database.GetCollection<Abonne>("Abonnes");
                if (collection.CountDocuments(Builders<Abonne>.Filter.Empty) <= 0)
                {
                    await collection.InsertManyAsync(abonnes);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Impossible d'ajouter des abonnes dans la collection " + ex.Message, "Erreur",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                throw;
            }
        }



        public async Task<bool> UpdateFilm(Film pFilm)
        {
            if (pFilm is null)
            {
                throw new ArgumentNullException("pFilm", "Le film ne peut pas être null");
            }

            try
            {
                var collection = database.GetCollection<Film>("Films");
                await collection.ReplaceOneAsync(x => x.Id == pFilm.Id, pFilm);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Impossible de mettre à jour le film {pFilm.Name} dans la collection {ex.Message}", "Erreur de mise à jour", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }

            return true;
        }
        #endregion
    }
}