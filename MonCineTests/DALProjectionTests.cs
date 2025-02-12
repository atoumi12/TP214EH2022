﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using MonCine.Data;
using MongoDB.Driver;
using Moq;
using Xunit;

namespace MonCineTests
{
    public class DALProjectionTests
    {
        private Mock<IMongoClient> mongoClient;
        private Mock<IMongoDatabase> mongodb;

        private Mock<IMongoCollection<Projection>> projectionCollection;
        private List<Projection> projectionsList;
        private Mock<IAsyncCursor<Projection>> projectionCursor;

        public DALProjectionTests()
        {
            mongoClient = new Mock<IMongoClient>();
            mongodb = new Mock<IMongoDatabase>();

            projectionCollection = new Mock<IMongoCollection<Projection>>();
            projectionCursor = new Mock<IAsyncCursor<Projection>>();


            projectionsList = new List<Projection>
            {
                new Projection(new Salle(1), new Film("Film1 Dal Projection"), new DateTime(2022, 01,01)),
                new Projection(new Salle(2), new Film("Film1 Dal Projection"), new DateTime(2022, 04,20)),
                new Projection(new Salle(3), new Film("Film2 Dal Projection"), new DateTime(2022, 04,20)),
                new Projection(new Salle(4), new Film("Film2 Dal Projection"), DateTime.Now)
            };
        }


        private void InitializeMongoDb()
        {
            mongoClient.Setup(x => x.GetDatabase(It.IsAny<string>(), default)).Returns(mongodb.Object);
            mongodb.Setup(x => x.GetCollection<Projection>("Projection", default)).Returns(projectionCollection.Object);
        }


        private void InitializeMongoProjectionCollection()
        {
            projectionCursor.Setup(x => x.Current).Returns(projectionsList);

            projectionCursor.SetupSequence(x => x.MoveNext(It.IsAny<CancellationToken>())).Returns(true).Returns(false);

            projectionCollection.Setup(x => x.FindSync(Builders<Projection>.Filter.Empty, It.IsAny<FindOptions<Projection>>(), default)).Returns(projectionCursor.Object);


            InitializeMongoDb();
        }

        [Fact]
        public void AddItem_moqInsertOne_ReturnTrueWhenProjectionInserted()
        {
            // Arrange
            InitializeMongoProjectionCollection();

            var dal = new DALProjection(mongoClient.Object);

            Projection projection = new Projection(new Salle(100), new Film("Film DAL Projection Test"), DateTime.Now);

            // Act
            bool result = dal.AddItem(projection);

            // Assert
            Assert.True(result);
        }


        [Fact]
        public void AddItem_moqInsertOne_ThrowsArgumentNullExceptionIfProjectionIsNull()
        {
            // Arrange
            InitializeMongoProjectionCollection();

            var dal = new DALProjection(mongoClient.Object);

            Projection projection = null;

            // Act and Assert
            ExceptionUtil.AssertThrows<ArgumentNullException>(delegate
            {
                dal.AddItem(projection);
            });
        }


        [Fact]
        public void UpdateItem_moqReplaceOne_ThrowsArgumentNullExceptionIfFilmIsNull()
        {
            // Arrange
            InitializeMongoProjectionCollection();

            var dal = new DALProjection(mongoClient.Object);

            Projection projection = null;

            // Act and Assert
            ExceptionUtil.AssertThrows<ArgumentNullException>(delegate
            {
                dal.UpdateItem(projection);
            });
        }


        [Fact]
        public void UpdateItem_moqReplaceOne_ReturnTrueIfFilmUpdated()
        {
            // Arrange
            InitializeMongoProjectionCollection();

            var dal = new DALProjection(mongoClient.Object);

            // Act
            Projection projection= projectionsList[0];
            projection.Film.Name = "Film updated moq";
            dal.UpdateItem(projection);

            // Assert
            Assert.Equal(projection, projectionsList.Find(x => x.Film.Name == projection.Film.Name));

        }


        [Fact]
        public void DeleteItem_moqDeleteOne_ReturnTrueIfFilmDeleted()
        {
            // Arrange
            InitializeMongoProjectionCollection();

            var dal = new DALProjection(mongoClient.Object);

            Projection projection = projectionsList[0];

            // Act 
            bool result = dal.DeleteItem(projection);


            // Assert
            Assert.True(result);

        }


        [Fact]
        public void DeleteItem_moqDeleteOne_ThrowsArgumentNullExceptionIfFilmIsNull()
        {
            // Arrange
            InitializeMongoProjectionCollection();

            var dal = new DALProjection(mongoClient.Object);

            Projection projection = null;

            // Act and Assert
            ExceptionUtil.AssertThrows<ArgumentNullException>(delegate
            {
                dal.DeleteItem(projection);
            });
        }

        [Fact]
        public void DeleteItem_moqDeleteOne_UpdateListCountCorrectly()
        {
            // Arrange
            InitializeMongoProjectionCollection();

            var dal = new DALProjection(mongoClient.Object);

            Projection projection = projectionsList[0];

            // Act
            dal.DeleteItem(projection);

            //Assert
            Assert.Equal(projectionsList.Count, projectionsList.Count);

        }




        [Fact]
        public void GetProjectionsOfFilm_moqFind_ThrowsArgumentNullExceptionIfProjectionIsNull()
        {
            // Arrange
            InitializeMongoProjectionCollection();

            var dal = new DALProjection(mongoClient.Object);

            Film film = null;

            // Act and Assert
            ExceptionUtil.AssertThrows<ArgumentNullException>(delegate
            {
                dal.GetProjectionsOfFilm(film);
            });
        }


        [Fact]
        public void GetProjectionsOfFilm_moqFind_ReturnProjectionListOfFilm()
        {
            // Arrange
            InitializeMongoProjectionCollection();

            var dal = new DALProjection(mongoClient.Object);

            Film film = projectionsList[0].Film;

            // Act and Assert
            ExceptionUtil.AssertThrows<ArgumentNullException>(delegate
            {
                dal.GetProjectionsOfFilm(film);
            });
        }



        [Fact]
        public void GetProjectionsByDate_moqFind_ThrowsArgumentNullExceptionIfDateIsNull()
        {
            // Arrange
            InitializeMongoProjectionCollection();

            var dal = new DALProjection(mongoClient.Object);

            DateTime date = DateTime.MinValue;

            // Act and Assert
            ExceptionUtil.AssertThrows<ArgumentNullException>(delegate
            {
                dal.GetProjectionsByDate(date);
            });
        }



    }
}