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

        public DALProjection()
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



        public async Task<bool> AddItem(Projection pProjection)
        {
            if (pProjection is null)
            {
                throw new ArgumentNullException("pProjection", "La projection ne peut pas être null");
            }

            try
            {
                var collection = database.GetCollection<Projection>(CollectionName);
                await collection.InsertOneAsync(pProjection);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Impossible d'ajouter la projection {pProjection.Id} dans la collection {ex.Message}",
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
    }
}