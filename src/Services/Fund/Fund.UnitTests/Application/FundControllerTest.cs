using Fund.API.IntegrationEvents;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.eShopOnContainers.Services.Fund.API;
using Microsoft.eShopOnContainers.Services.Fund.API.Controllers;
using Microsoft.eShopOnContainers.Services.Fund.API.Infrastructure;
using Microsoft.eShopOnContainers.Services.Fund.API.Model;
using Microsoft.eShopOnContainers.Services.Fund.API.ViewModel;
using Microsoft.Extensions.Options;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace UnitTest.Fund.Application
{
    public class FundControllerTest
    {
        private readonly DbContextOptions<FundContext> _dbOptions;

        public FundControllerTest()
        {
            _dbOptions = new DbContextOptionsBuilder<FundContext>()
                .UseInMemoryDatabase(databaseName: "in-memory")
                .Options;

            using (var dbContext = new FundContext(_dbOptions))
            {
                dbContext.AddRange(GetFakeFund());
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

            var catalogContext = new FundContext(_dbOptions);
            var catalogSettings = new TestFundSettings();

            var integrationServicesMock = new Mock<IFundIntegrationEventService>();

            //Act
            var orderController = new FundController(catalogContext, catalogSettings, integrationServicesMock.Object);
            var actionResult = await orderController.ItemsByTypeIdAndBrandIdAsync(typesFilterApplied, brandFilterApplied, pageSize, pageIndex);

            //Assert
            Assert.IsType<ActionResult<PaginatedItemsViewModel<FundItem>>>(actionResult);
            var page = Assert.IsAssignableFrom<PaginatedItemsViewModel<FundItem>>(actionResult.Value);
            Assert.Equal(expectedTotalItems, page.Count);
            Assert.Equal(pageIndex, page.PageIndex);
            Assert.Equal(pageSize, page.PageSize);
            Assert.Equal(expectedItemsInPage, page.Data.Count());
        }

        private List<FundItem> GetFakeFund()
        {
            return new List<FundItem>()
            {
                new FundItem()
                {
                    Id = 1,
                    Name = "fakeItemA",
                    FundTypeId = 2,
                    FundBrandId = 1,
                    PictureFileName = "fakeItemA.png"
                },
                new FundItem()
                {
                    Id = 2,
                    Name = "fakeItemB",
                    FundTypeId = 2,
                    FundBrandId = 1,
                    PictureFileName = "fakeItemB.png"
                },
                new FundItem()
                {
                    Id = 3,
                    Name = "fakeItemC",
                    FundTypeId = 2,
                    FundBrandId = 1,
                    PictureFileName = "fakeItemC.png"
                },
                new FundItem()
                {
                    Id = 4,
                    Name = "fakeItemD",
                    FundTypeId = 2,
                    FundBrandId = 1,
                    PictureFileName = "fakeItemD.png"
                },
                new FundItem()
                {
                    Id = 5,
                    Name = "fakeItemE",
                    FundTypeId = 2,
                    FundBrandId = 1,
                    PictureFileName = "fakeItemE.png"
                },
                new FundItem()
                {
                    Id = 6,
                    Name = "fakeItemF",
                    FundTypeId = 2,
                    FundBrandId = 1,
                    PictureFileName = "fakeItemF.png"
                }
            };
        }
    }

    public class TestFundSettings : IOptionsSnapshot<FundSettings>
    {
        public FundSettings Value => new FundSettings
        {
            PicBaseUrl = "http://image-server.com/",
            AzureStorageEnabled = true
        };

        public FundSettings Get(string name) => Value;
    }

}
