using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApp.Controllers;
using WebApp.Models;
using Xunit;

namespace WebApp.Test
{
    public class WebAppTest
    {
        [Fact]
        public void RetutnViewAllEmployeesTest()
        {
            // Arrange
            var mock = new Mock<IStore>();
            mock.Setup(store => store.GetAll())
                .Returns(EmployeesListTest());
            var controller = new HomeController(mock.Object);

            // Action
            var result = controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Employee>>(viewResult.Model);
            Assert.Equal(EmployeesListTest().Count(), model.Count());
        }

        [Fact]
        public void ReturnRedirectViewAddEmployeeTest()
        {
            // Arrange
            var mock = new Mock<IStore>();
            var controller = new HomeController(mock.Object);
            var employeeNew = new Employee() { Name = "Jenny" };

            // Action
            var result = controller.AddEmployee(employeeNew);

            // Assert
            var redirectionToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectionToActionResult.ActionName);
            mock.Verify(s => s.Create(employeeNew));
        }

        [Fact]
        public void ReturnModelViewAddEmployeeTest()
        {
            //Arrange
            var mock = new Mock<IStore>();
            var controller = new HomeController(mock.Object);
            controller.ModelState.AddModelError("Name", "Required");
            Employee employeeNew = new Employee();

            //Action
            var result = controller.AddEmployee(employeeNew);

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(employeeNew, viewResult.Model);
        }

        [Fact]
        public void ReturnBadRequestViewGetEmplyeeTest()
        {
            var mock = new Mock<IStore>();
            var controller = new HomeController(mock.Object);

            var result = controller.GetEmployee(null);

            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void ReturnNotFoundViewGetEmplyeeTest()
        {
            var mock = new Mock<IStore>();
            int id = 10;
            mock.Setup(s => s.GetById(id))
                .Returns(null as Employee);
            var controller = new HomeController(mock.Object);


            var result = controller.GetEmployee(id);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void ReturnGoodResultViewGetEmplyeeTest()
        {
            var mock = new Mock<IStore>();
            int id = 3;
            mock.Setup(s => s.GetById(id))
                .Returns(EmployeesListTest().FirstOrDefault(e => e.Id == id));
            var controller = new HomeController(mock.Object);


            var result = controller.GetEmployee(id);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<Employee>(viewResult.ViewData.Model);
            Assert.Equal(id, model.Id);
            Assert.Equal("Jimmy", model.Name);
            Assert.Equal(27, model.Age);
        }

        public List<Employee> EmployeesListTest()
        {
            return new()
            {
                new(){ Id = 1, Name = "Bobby", Age = 34 },
                new(){ Id = 2, Name = "Sammy", Age = 41 },
                new(){ Id = 3, Name = "Jimmy", Age = 27 },
                new(){ Id = 4, Name = "Lenny", Age = 19 },
                new(){ Id = 5, Name = "Poppy", Age = 25 },
            };
        }

    }
}
