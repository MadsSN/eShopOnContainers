using Order.API.IntegrationEvents;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.eShopOnContainers.Services.Order.API;
using Microsoft.eShopOnContainers.Services.Order.API.Controllers;
using Microsoft.eShopOnContainers.Services.Order.API.Infrastructure;
using Microsoft.eShopOnContainers.Services.Order.API.Model;
using Microsoft.eShopOnContainers.Services.Order.API.ViewModel;
using Microsoft.Extensions.Options;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace UnitTest.Order.Application
{
    public class OrderControllerTest
    {
        private readonly DbContextOptions<OrderContext> _dbOptions;

        public OrderControllerTest()
        {
            _dbOptions = new DbContextOptionsBuilder<OrderContext>()
                .UseInMemoryDatabase(databaseName: "in-memory")
                .Options;

            using (var dbContext = new OrderContext(_dbOptions))
            {
                dbContext.AddRange(GetFakeOrder());
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

            var catalogContext = new OrderContext(_dbOptions);
            var catalogSettings = new TestOrderSettings();

            var integrationServicesMock = new Mock<IOrderIntegrationEventService>();

            //Act
            var orderController = new OrderController(catalogContext, catalogSettings, integrationServicesMock.Object);
            var actionResult = await orderController.ItemsByTypeIdAndBrandIdAsync(typesFilterApplied, brandFilterApplied, pageSize, pageIndex);

            //Assert
            Assert.IsType<ActionResult<PaginatedItemsViewModel<OrderItem>>>(actionResult);
            var page = Assert.IsAssignableFrom<PaginatedItemsViewModel<OrderItem>>(actionResult.Value);
            Assert.Equal(expectedTotalItems, page.Count);
            Assert.Equal(pageIndex, page.PageIndex);
            Assert.Equal(pageSize, page.PageSize);
            Assert.Equal(expectedItemsInPage, page.Data.Count());
        }

        private List<OrderItem> GetFakeOrder()
        {
            return new List<OrderItem>()
            {
                new OrderItem()
                {
                    Id = 1,
                    Name = "fakeItemA",
                    OrderTypeId = 2,
                    OrderBrandId = 1,
                    PictureFileName = "fakeItemA.png"
                },
                new OrderItem()
                {
                    Id = 2,
                    Name = "fakeItemB",
                    OrderTypeId = 2,
                    OrderBrandId = 1,
                    PictureFileName = "fakeItemB.png"
                },
                new OrderItem()
                {
                    Id = 3,
                    Name = "fakeItemC",
                    OrderTypeId = 2,
                    OrderBrandId = 1,
                    PictureFileName = "fakeItemC.png"
                },
                new OrderItem()
                {
                    Id = 4,
                    Name = "fakeItemD",
                    OrderTypeId = 2,
                    OrderBrandId = 1,
                    PictureFileName = "fakeItemD.png"
                },
                new OrderItem()
                {
                    Id = 5,
                    Name = "fakeItemE",
                    OrderTypeId = 2,
                    OrderBrandId = 1,
                    PictureFileName = "fakeItemE.png"
                },
                new OrderItem()
                {
                    Id = 6,
                    Name = "fakeItemF",
                    OrderTypeId = 2,
                    OrderBrandId = 1,
                    PictureFileName = "fakeItemF.png"
                }
            };
        }
    }

    public class TestOrderSettings : IOptionsSnapshot<OrderSettings>
    {
        public OrderSettings Value => new OrderSettings
        {
            PicBaseUrl = "http://image-server.com/",
            AzureStorageEnabled = true
        };

        public OrderSettings Get(string name) => Value;
    }

}
