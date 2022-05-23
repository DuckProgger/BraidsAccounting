using BraidsAccounting.DAL.Entities;
using BraidsAccounting.DAL.Repositories;
using BraidsAccounting.Services;
using BraidsAccounting.Services.Interfaces;
using MockQueryable.Moq;

namespace BraidsAccounting.Tests
{  
    public class CatalogueServiceTest
    {
        private readonly CatalogueService sut;
        private readonly Mock<IRepository<Item>> repository = new();
        private readonly Mock<IHistoryService> historyService = new();

        public CatalogueServiceTest()
        {
            sut = new(repository.Object, historyService.Object);
        }

        [Fact]
        public async Task GetAsync_ShouldReturnItem_WhenItemExists()
        {       
            // Arrange
            var data = new List<Item>()
            {
                new Item()
                {
                    Article = "article",
                    Color = "color",
                    Manufacturer = new()
                    {
                        Name = "manufacturer"
                    }
                }
            };

            var mock = data.BuildMock();
            repository.Setup(x => x.Items).Returns(mock);

            // Act
            var item = await sut.GetAsync("manufacturer", "article", "color");

            // Assert		
            Assert.Equal(data[0], item);
        }

        [Fact]
        public async Task GetAsync_ShouldReturnNull_WhenItemNotExists()
        {
            // Arrange
            var data = new List<Item>();
            var mock = data.BuildMock();
            repository.Setup(x => x.Items).Returns(mock);

            // Act
            var item = await sut.GetAsync("manufacturer", "article", "color");

            // Assert		
            Assert.Null(item);
        }
    }
}