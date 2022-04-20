using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MongoDB.Driver;

namespace MonCine.Data
{
    public class DALProjection : DAL, ICRUD<Projection>
    {
        public string CollectionName { get; set; }

        public DALProjection(IMongoClient client = null):base(client)
        {
            CollectionName = "Projection";
        }




        /// <summary>
        /// Récupère l'ensemble des projections de la BD
        /// </summary>
        /// <returns>Liste de Projection</returns>
        public List<Projection> ReadItems()
        {
            return null;
        }



        public bool AddItem(Projection pProjection)
        {
            if (pProjection is null)
            {
                throw new ArgumentNullException("pProjection", "La projection ne peut pas être null");
            }

            try
            {
                var collection = database.GetCollection<Projection>(CollectionName);
                collection.InsertOneAsync(pProjection);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Impossible d'ajouter la projection {pProjection.Id} dans la collection {ex.Message}",
                    "Erreur d'ajout", MessageBoxButton.OK, MessageBoxImage.Error);

                throw;
            }

            return true;
        }

        public List<Projection> GetProjectionsOfFilm(Film pFilm)
        {
            if (pFilm is null)
            {
                throw new ArgumentNullException("pFilm", "Le film ne peut pas être null");
            }

            List<Projection> projections = new List<Projection>();
            try
            {
                var collection = database.GetCollection<Projection>(CollectionName);
                projections = collection.Find(Builders<Projection>.Filter.Eq(x=>x.Film.Id , pFilm.Id)).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Impossible d'ajouter la projection {pFilm.Id} dans la collection {ex.Message}",
                    "Erreur d'ajout", MessageBoxButton.OK, MessageBoxImage.Error);

                throw;
            }

            return projections;
        }


        public bool UpdateItem(Projection pObj)
        {
            throw new NotImplementedException();
        }

        public bool DeleteItem(Projection pObj)
        {
            throw new NotImplementedException();
        }
    }
}