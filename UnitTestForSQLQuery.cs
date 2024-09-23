using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;

[TestFixture]
public class MyRepositoryTests
{
    private Mock<MyDbContext> _mockContext;
    private MyRepository _repository;

    [SetUp]
    public void SetUp()
    {
        _mockContext = new Mock<MyDbContext>();
        _repository = new MyRepository(_mockContext.Object);
    }

    [Test]
    public void GetEntitiesFromStoredProcedure_ShouldReturnEntities()
    {
        // Arrange
        var data = new List<MyEntity>
        {
            new MyEntity { Id = 1, Name = "Entity1" },
            new MyEntity { Id = 2, Name = "Entity2" }
        }.AsQueryable();

        var mockDbQuery = new Mock<DbRawSqlQuery<MyEntity>>();
        mockDbQuery.As<IQueryable<MyEntity>>().Setup(m => m.Provider).Returns(data.Provider);
        mockDbQuery.As<IQueryable<MyEntity>>().Setup(m => m.Expression).Returns(data.Expression);
        mockDbQuery.As<IQueryable<MyEntity>>().Setup(m => m.ElementType).Returns(data.ElementType);
        mockDbQuery.As<IQueryable<MyEntity>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

        _mockContext.Setup(m => m.Database.SqlQuery<MyEntity>(It.IsAny<string>(), It.IsAny<object[]>()))
                    .Returns(mockDbQuery.Object);

        // Act
        var result = _repository.GetEntitiesFromStoredProcedure();

        // Assert
        Assert.AreEqual(2, result.Count());
        Assert.AreEqual("Entity1", result.First().Name);
    }
}
