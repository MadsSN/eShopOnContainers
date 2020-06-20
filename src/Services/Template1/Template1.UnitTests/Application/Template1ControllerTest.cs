using Template1.API.IntegrationEvents;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.eShopOnContainers.Services.Template1.API;
using Microsoft.eShopOnContainers.Services.Template1.API.Controllers;
using Microsoft.eShopOnContainers.Services.Template1.API.Infrastructure;
using Microsoft.eShopOnContainers.Services.Template1.API.Model;
using Microsoft.eShopOnContainers.Services.Template1.API.ViewModel;
using Microsoft.Extensions.Options;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace UnitTest.Template1.Application
{
    public class Template1ControllerTest
    {
        private readonly DbContextOptions<Template1Context> _dbOptions;

        public Template1ControllerTest()
        {
            _dbOptions = new DbContextOptionsBuilder<Template1Context>()
                .UseInMemoryDatabase(databaseName: "in-memory")
                .Options;

            using (var dbContext = new Template1Context(_dbOptions))
            {
                dbContext.AddRange(GetFakeTemplate1());
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

            var catalogContext = new Template1Context(_dbOptions);
            var catalogSettings = new TestTemplate1Settings();

            var integrationServicesMock = new Mock<ITemplate1IntegrationEventService>();

            //Act
            var orderController = new Template1Controller(catalogContext, catalogSettings, integrationServicesMock.Object);
            var actionResult = await orderController.ItemsByTypeIdAndBrandIdAsync(typesFilterApplied, brandFilterApplied, pageSize, pageIndex);

            //Assert
            Assert.IsType<ActionResult<PaginatedItemsViewModel<Template1Item>>>(actionResult);
            var page = Assert.IsAssignableFrom<PaginatedItemsViewModel<Template1Item>>(actionResult.Value);
            Assert.Equal(expectedTotalItems, page.Count);
            Assert.Equal(pageIndex, page.PageIndex);
            Assert.Equal(pageSize, page.PageSize);
            Assert.Equal(expectedItemsInPage, page.Data.Count());
        }

        private List<Template1Item> GetFakeTemplate1()
        {
            return new List<Template1Item>()
            {
                new Template1Item()
                {
                    Id = 1,
                    Name = "fakeItemA",
                    Template1TypeId = 2,
                    Template1BrandId = 1,
                    PictureFileName = "fakeItemA.png"
                },
                new Template1Item()
                {
                    Id = 2,
                    Name = "fakeItemB",
                    Template1TypeId = 2,
                    Template1BrandId = 1,
                    PictureFileName = "fakeItemB.png"
                },
                new Template1Item()
                {
                    Id = 3,
                    Name = "fakeItemC",
                    Template1TypeId = 2,
                    Template1BrandId = 1,
                    PictureFileName = "fakeItemC.png"
                },
                new Template1Item()
                {
                    Id = 4,
                    Name = "fakeItemD",
                    Template1TypeId = 2,
                    Template1BrandId = 1,
                    PictureFileName = "fakeItemD.png"
                },
                new Template1Item()
                {
                    Id = 5,
                    Name = "fakeItemE",
                    Template1TypeId = 2,
                    Template1BrandId = 1,
                    PictureFileName = "fakeItemE.png"
                },
                new Template1Item()
                {
                    Id = 6,
                    Name = "fakeItemF",
                    Template1TypeId = 2,
                    Template1BrandId = 1,
                    PictureFileName = "fakeItemF.png"
                }
            };
        }
    }

    public class TestTemplate1Settings : IOptionsSnapshot<Template1Settings>
    {
        public Template1Settings Value => new Template1Settings
        {
            PicBaseUrl = "http://image-server.com/",
            AzureStorageEnabled = true
        };

        public Template1Settings Get(string name) => Value;
    }

}
