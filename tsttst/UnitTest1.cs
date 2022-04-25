using System;
using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MonCine.Data;
using MongoDB.Bson;
using MongoDB.Driver;
using Moq;
using Xunit;

namespace tsttst
{
    public class UnitTest1
    {
            
        private Mock<IMongoClient> mongoClient;
        private Mock<IMongoDatabase> mongodb;

        private Mock<IMongoCollection<Abonne>> abonneCollection;
        private List<Abonne> abonneList;
        private Mock<IAsyncCursor<Abonne>> abonneCursor;

        public UnitTest1()
        {
            mongoClient = new Mock<IMongoClient>();
            mongodb = new Mock<IMongoDatabase>();

            abonneCollection = new Mock<IMongoCollection<Abonne>>();
            abonneCursor = new Mock<IAsyncCursor<Abonne>>();


            abonneList = new List<Abonne>
        {
            new Abonne("Abonne 1"),
            new Abonne("Abonne 2"),
            new Abonne("Abonne 3")
        };
        }

        private void InitializeMongoDb()
        {
            mongoClient.Setup(x => x.GetDatabase(It.IsAny<string>(), default)).Returns(mongodb.Object);
            mongodb.Setup(x => x.GetCollection<Abonne>("Abonne", default)).Returns(abonneCollection.Object);

        }

        private void InitializeMongoAbonneCollection()
        {
            abonneCursor.Setup(x => x.Current).Returns(abonneList);

            abonneCursor.SetupSequence(x => x.MoveNext(It.IsAny<CancellationToken>())).Returns(true).Returns(false);

            abonneCollection.Setup(x => x.FindSync(Builders<Abonne>.Filter.Empty, It.IsAny<FindOptions<Abonne>>(), default)).Returns(abonneCursor.Object);


            InitializeMongoDb();
        }




        [Fact]
        public void ReadItems_moqFindSyncCall()
        {
            // Arrange
            InitializeMongoAbonneCollection();

            var dal = new DALAbonne(mongoClient.Object);

            // Act : appel de la méthode qui contient le faux objet
            var documents = dal.ReadItems();

            // Assert
            Assert.Equal(abonneList, documents);

        }

        [Fact]
        public void AddItem_moqInsertOne_ReturnTrueWhenAbonneInserted()
        {
            // Arrange
            InitializeMongoAbonneCollection();

            var dal = new DALAbonne(mongoClient.Object);

            Abonne abonne = new Abonne("Abonne moq");

            // Act
            bool result = dal.AddItem(abonne);

            // Assert
            Assert.True(result);
        }


        [Fact]
        public void AddItem_moqInsertOne_ThrowsArgumentNullExceptionIfAbonneIsNull()
        {
            // Arrange
            InitializeMongoAbonneCollection();

            var dal = new DALAbonne(mongoClient.Object);

            Abonne abonne = null;

            // Act and Assert
            ExceptionUtil.AssertThrows<ArgumentNullException>(delegate
            {
                dal.AddItem(abonne);
            });
        }

    }
}

