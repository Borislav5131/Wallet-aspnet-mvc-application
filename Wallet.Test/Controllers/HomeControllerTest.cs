using Microsoft.AspNetCore.Mvc;
using Wallet.Controllers;
using MyTested.AspNetCore.Mvc;
using Wallet.Core.ViewModels.Home;

namespace Wallet.Test.Controllers
{
    using Xunit;

    public class HomeControllerTest
    {
        [Fact]
        public void IndexShouldReturnViewWithModel()
            => MyMvc
                .Pipeline()
                .ShouldMap("/")
                .To<HomeController>(c=>c.Index())
                .Which()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<HomeViewModel>());

                [Fact]
        public void ErrorShouldReturnView()
        {
            //Arrange
            var homeController = new HomeController(null, null, null,  null);

            //Act
            var result = homeController.Error();

            //Assert
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
        }
    }
}
