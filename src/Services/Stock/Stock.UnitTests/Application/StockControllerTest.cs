using Stock.API.IntegrationEvents;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.eShopOnContainers.Services.Stock.API;
using Microsoft.eShopOnContainers.Services.Stock.API.Controllers;
using Microsoft.eShopOnContainers.Services.Stock.API.Infrastructure;
using Microsoft.eShopOnContainers.Services.Stock.API.Model;
using Microsoft.eShopOnContainers.Services.Stock.API.ViewModel;
using Microsoft.Extensions.Options;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace UnitTest.Stock.Application
{
    public class StockControllerTest
    {
        private readonly DbContextOptions<StockContext> _dbOptions;

        public StockControllerTest()
        {
            _dbOptions = new DbContextOptionsBuilder<StockContext>()
                .UseInMemoryDatabase(databaseName: "in-memory")
                .Options;

            using (var dbContext = new StockContext(_dbOptions))
            {
                dbContext.AddRange(GetFakeStock());
                dbContext.SaveChanges();
            }
        }

        [Fact]
        public async Task Get_catalog_items_success()
        {
            //Arrange
            var brandFilterApplied = 1;
            var typesFilterApplied = 2;
            var pageSize = 4;
            var pageIndex = 1;

            var expectedItemsInPage = 2;
            var expectedTotalItems = 6;

            var catalogContext = new StockContext(_dbOptions);
            var catalogSettings = new TestStockSettings();

            var integrationServicesMock = new Mock<IStockIntegrationEventService>();

            //Act
            var orderController = new StockController(catalogContext, catalogSettings, integrationServicesMock.Object);
            var actionResult = await orderController.ItemsByTypeIdAndBrandIdAsync(typesFilterApplied, brandFilterApplied, pageSize, pageIndex);

            //Assert
            Assert.IsType<ActionResult<PaginatedItemsViewModel<StockItem>>>(actionResult);
            var page = Assert.IsAssignableFrom<PaginatedItemsViewModel<StockItem>>(actionResult.Value);
            Assert.Equal(expectedTotalItems, page.Count);
            Assert.Equal(pageIndex, page.PageIndex);
            Assert.Equal(pageSize, page.PageSize);
            Assert.Equal(expectedItemsInPage, page.Data.Count());
        }

        private List<StockItem> GetFakeStock()
        {
            return new List<StockItem>()
            {
                new StockItem()
                {
                    Id = 1,
                    Name = "fakeItemA",
                    StockTypeId = 2,
                    StockBrandId = 1,
                    PictureFileName = "fakeItemA.png"
                },
                new StockItem()
                {
                    Id = 2,
                    Name = "fakeItemB",
                    StockTypeId = 2,
                    StockBrandId = 1,
                    PictureFileName = "fakeItemB.png"
                },
                new StockItem()
                {
                    Id = 3,
                    Name = "fakeItemC",
                    StockTypeId = 2,
                    StockBrandId = 1,
                    PictureFileName = "fakeItemC.png"
                },
                new StockItem()
                {
                    Id = 4,
                    Name = "fakeItemD",
                    StockTypeId = 2,
                    StockBrandId = 1,
                    PictureFileName = "fakeItemD.png"
                },
                new StockItem()
                {
                    Id = 5,
                    Name = "fakeItemE",
                    StockTypeId = 2,
                    StockBrandId = 1,
                    PictureFileName = "fakeItemE.png"
                },
                new StockItem()
                {
                    Id = 6,
                    Name = "fakeItemF",
                    StockTypeId = 2,
                    StockBrandId = 1,
                    PictureFileName = "fakeItemF.png"
                }
            };
        }
    }

    public class TestStockSettings : IOptionsSnapshot<StockSettings>
    {
        public StockSettings Value => new StockSettings
        {
            PicBaseUrl = "http://image-server.com/",
            AzureStorageEnabled = true
        };

        public StockSettings Get(string name) => Value;
    }

}
