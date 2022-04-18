using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MongoDB.Driver;

namespace MonCine.Data
{
    public class DALSalle : DAL, ICRUD<Salle>
    {
        public string CollectionName { get; set; }

        public DALSalle()
        {
            CollectionName = "Salle";
            AddDefaultsalle();

        }
        private async void AddDefaultsalle()
        {
            List<Salle> salles = new List<Salle>
            {
                new Salle(12),
                new Salle(24),
                new Salle(31)
            };

            try
            {
                var collection = database.GetCollection<Salle>(CollectionName);
                if (collection.CountDocuments(Builders<Salle>.Filter.Empty) <= 0)
                {
                    await collection.InsertManyAsync(salles);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Impossible d'ajouter des salles dans la collection " + ex.Message, "Erreur",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                throw;
            }
        }

        public async Task<bool> AddItem(Salle pSalle)
        {
            if (pSalle is null)
            {
                throw new ArgumentNullException("pSalle", "La salle ne peut pas être null");
            }

            try
            {
                var collection = database.GetCollection<Salle>(CollectionName);
                await collection.InsertOneAsync(pSalle);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Impossible d'ajouter la salle dans la collection {ex.Message}",
                    "Erreur d'ajout", MessageBoxButton.OK, MessageBoxImage.Error);

                throw;
            }

            return true;
        }


        public Task<bool> UpdateItem(Projection pObj)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteItem(Projection pObj)
        {
            throw new NotImplementedException();
        }

        public List<Salle> ReadItems()
        {
            List<Salle> salles = new List<Salle>();

            try
            {
                var collection = database.GetCollection<Salle>("Salle");
                salles = collection.Aggregate().ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Impossible d'obtenir la collection " + ex.Message, "Erreur", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }

            return salles;
        }

        public Task<bool> UpdateItem(Salle pObj)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteItem(Salle pObj)
        {
            throw new NotImplementedException();
        }
    }
}