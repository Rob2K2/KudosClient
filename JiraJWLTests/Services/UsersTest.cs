using JiraJWL.Controllers;
using JiraJWL.Modelos;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JiraJWLTests.Services
{
    [TestClass]
    public class UsersTest
    {
        [TestMethod]
        public void UserWithAgeLessThanZeroIsInvalid()
        {
            var homeController = new HomeController();
            var user = new User { Edad = -1 };

            var result = homeController.EsValido(user);


            Assert.IsFalse(result);
        }

        [TestMethod]
        public void UserWithAgeMoreThanTwoHundredIsInvalid()
        {
            var homeController = new HomeController();
            var user = new User { Edad = 201 };

            var result = homeController.EsValido(user);


            Assert.IsFalse(result);
        }

        [TestMethod]
        public void UserWithAgeBetweenZeroAndOneHundredIsValid()
        {
            var homeController = new HomeController();
            var user = new User { Edad = 69 };

            var result = homeController.EsValido(user);


            Assert.IsTrue(result);
        }
    }
}
