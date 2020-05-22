using ShareOwner.API.IntegrationEvents;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.eShopOnContainers.Services.ShareOwner.API;
using Microsoft.eShopOnContainers.Services.ShareOwner.API.Controllers;
using Microsoft.eShopOnContainers.Services.ShareOwner.API.Infrastructure;
using Microsoft.eShopOnContainers.Services.ShareOwner.API.Model;
using Microsoft.eShopOnContainers.Services.ShareOwner.API.ViewModel;
using Microsoft.Extensions.Options;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace UnitTest.ShareOwner.Application
{
    public class ShareOwnerControllerTest
    {
        private readonly DbContextOptions<ShareOwnerContext> _dbOptions;

        public ShareOwnerControllerTest()
        {
            _dbOptions = new DbContextOptionsBuilder<ShareOwnerContext>()
                .UseInMemoryDatabase(databaseName: "in-memory")
                .Options;

            using (var dbContext = new ShareOwnerContext(_dbOptions))
            {
                dbContext.AddRange(GetFakeShareOwner());
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

            var catalogContext = new ShareOwnerContext(_dbOptions);
            var catalogSettings = new TestShareOwnerSettings();

            var integrationServicesMock = new Mock<IShareOwnerIntegrationEventService>();

            //Act
            var orderController = new ShareOwnerController(catalogContext, catalogSettings, integrationServicesMock.Object);
            var actionResult = await orderController.ItemsByTypeIdAndBrandIdAsync(typesFilterApplied, brandFilterApplied, pageSize, pageIndex);

            //Assert
            Assert.IsType<ActionResult<PaginatedItemsViewModel<ShareOwnerItem>>>(actionResult);
            var page = Assert.IsAssignableFrom<PaginatedItemsViewModel<ShareOwnerItem>>(actionResult.Value);
            Assert.Equal(expectedTotalItems, page.Count);
            Assert.Equal(pageIndex, page.PageIndex);
            Assert.Equal(pageSize, page.PageSize);
            Assert.Equal(expectedItemsInPage, page.Data.Count());
        }

        private List<ShareOwnerItem> GetFakeShareOwner()
        {
            return new List<ShareOwnerItem>()
            {
                new ShareOwnerItem()
                {
                    Id = 1,
                    Name = "fakeItemA",
                    ShareOwnerTypeId = 2,
                    ShareOwnerBrandId = 1,
                    PictureFileName = "fakeItemA.png"
                },
                new ShareOwnerItem()
                {
                    Id = 2,
                    Name = "fakeItemB",
                    ShareOwnerTypeId = 2,
                    ShareOwnerBrandId = 1,
                    PictureFileName = "fakeItemB.png"
                },
                new ShareOwnerItem()
                {
                    Id = 3,
                    Name = "fakeItemC",
                    ShareOwnerTypeId = 2,
                    ShareOwnerBrandId = 1,
                    PictureFileName = "fakeItemC.png"
                },
                new ShareOwnerItem()
                {
                    Id = 4,
                    Name = "fakeItemD",
                    ShareOwnerTypeId = 2,
                    ShareOwnerBrandId = 1,
                    PictureFileName = "fakeItemD.png"
                },
                new ShareOwnerItem()
                {
                    Id = 5,
                    Name = "fakeItemE",
                    ShareOwnerTypeId = 2,
                    ShareOwnerBrandId = 1,
                    PictureFileName = "fakeItemE.png"
                },
                new ShareOwnerItem()
                {
                    Id = 6,
                    Name = "fakeItemF",
                    ShareOwnerTypeId = 2,
                    ShareOwnerBrandId = 1,
                    PictureFileName = "fakeItemF.png"
                }
            };
        }
    }

    public class TestShareOwnerSettings : IOptionsSnapshot<ShareOwnerSettings>
    {
        public ShareOwnerSettings Value => new ShareOwnerSettings
        {
            PicBaseUrl = "http://image-server.com/",
            AzureStorageEnabled = true
        };

        public ShareOwnerSettings Get(string name) => Value;
    }

}
