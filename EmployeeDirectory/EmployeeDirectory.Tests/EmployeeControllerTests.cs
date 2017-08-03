using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Employee.Common;
using Employee.DomainObject;
using Employee.Service;
using EmployeeDirectory.Controllers;
using EmployeeDirectory.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using NSubstitute.Proxies.CastleDynamicProxy;
using NSubstitute.ReturnsExtensions;

namespace EmployeeDirectory.Tests
{
    [TestClass]
    public class EmployeeControllerTests
    {
        private ISearchEmployeeService _searchEmployee;
        private IEmployeeService _employeeService;
        private ILogger<EmployeeController> _logger;
        private IMapper _mapper;
        [TestInitialize]
        public void TestInitialize()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PhoneNumberViewModel, PhoneNumber>();
                cfg.CreateMap<PhoneNumber, PhoneNumberViewModel>();
            });
            IMapper mapper = config.CreateMapper();
            //Arrange
            _searchEmployee = Substitute.For<ISearchEmployeeService>();
            _employeeService = Substitute.For<IEmployeeService>();
            _logger = Substitute.For<ILogger<EmployeeController>>();
            _mapper = mapper;

        }
        [TestMethod]
        public void when_search_employees_calls_the_service_with_right_parameters()
        {
            //arrange
            var controller = new EmployeeController(_searchEmployee, _employeeService,_logger, _mapper);
            //act 
            controller.SearchResults(SearchBy.Email, "test@test.com", 1);
            //assert
            _searchEmployee.Received(1).SearchEmployees(Arg.Is(SearchBy.Email), Arg.Is("test@test.com"), Arg.Is(1), Arg.Is(10));
        }

        [TestMethod]
        public void when_create_employee_calls_the_service_with_right_parameters()
        {
            //arrange
            var controller = new EmployeeController(_searchEmployee, _employeeService, _logger, _mapper);
            //act 
            var phonemodel =new[]{ new PhoneNumberViewModel() {Number = "123-123-1234", Type = "work"}}.ToList();
            controller.Create("John Doe", "Analyst","dallas", "test@test.com", phonemodel);
            //assert
            _employeeService.Received(1)
                .Create(Arg.Is<Employee.DomainObject.Employee>(e => e.Name == "John Doe" && e.JobTitle == "Analyst" &&
                                                                    e.Location == "dallas" &&
                                                                    e.Email == "test@test.com" && 
                                                                    e.PhoneNumbers.Any(p=>p.Number == phonemodel[0].Number && p.Type == phonemodel[0].Type)));
        }
        [TestMethod]
        public void throws_exception_when_employee_not_found_during_update()
        {
            //arrange
            var controller = new EmployeeController(_searchEmployee, _employeeService, _logger, _mapper);
            _employeeService.GetById(100).ReturnsNull();
            //act 
            var phonemodel = new[] { new PhoneNumberViewModel() { Number = "123-123-1234", Type = "work" } }.ToList();
            Assert.ThrowsException<Exception>(() => controller.Update(100, "John Doe", "Analyst", "dallas",
                "test@test.com", phonemodel));
            //assert
            _employeeService.DidNotReceive().Create(Arg.Any<Employee.DomainObject.Employee>());
        }
        [TestMethod]
        public void when_update_employee_calls_the_service_with_right_parameters()
        {
            //arrange
            var controller = new EmployeeController(_searchEmployee, _employeeService, _logger, _mapper);
            var phonedomainmodel = new[] { new PhoneNumber() { Number = "123-123-1234", Type = "work" } }.ToList();
            _employeeService.GetById(100).Returns(new Employee.DomainObject.Employee(){Name = "John Doe",Email = "test@email.com",Id = 100,Location = "dallas",PhoneNumbers = phonedomainmodel, JobTitle = "Analyst"});
            //act 
            var phonemodel = new[] { new PhoneNumberViewModel() { Number = "123-123-1234 update", Type = "work" } }.ToList();
            controller.Update(100, "John Doe update", "Analyst update", "dallas update", "test@test.com update", phonemodel);
            //assert
            _employeeService.Received(1)
                .Update(Arg.Is<Employee.DomainObject.Employee>(e => e.Name == "John Doe update" && 
                                                                    e.Id == 100 && 
                                                                    e.JobTitle == "Analyst update" &&
                                                                    e.Location == "dallas update" &&
                                                                    e.Email == "test@test.com update" &&
                                                                    e.PhoneNumbers.Any(p => p.Number == phonemodel[0].Number && p.Type == phonemodel[0].Type)));
        }
        [TestMethod]
        public void throws_exception_when_employee_not_found_during_delete()
        {
            //arrange
            var controller = new EmployeeController(_searchEmployee, _employeeService, _logger, _mapper);
            _employeeService.GetById(100).ReturnsNull();
            //act 
            Assert.ThrowsException<Exception>(() => controller.Delete(100));
            //assert
            _employeeService.DidNotReceive().Delete(Arg.Any<Employee.DomainObject.Employee>());
        }
        [TestMethod]
        public void when_delete_employee_calls_the_service_with_right_parameters()
        {
            //arrange
            var controller = new EmployeeController(_searchEmployee, _employeeService, _logger, _mapper);
            _employeeService.GetById(100).Returns(new Employee.DomainObject.Employee() { Name = "John Doe", Email = "test@email.com", Id = 100, Location = "dallas", PhoneNumbers = new List<PhoneNumber>(), JobTitle = "Analyst" });
            //act 
            controller.Delete(100);
            //assert
            _employeeService.Received(1)
                .Delete(Arg.Is<Employee.DomainObject.Employee>(e => e.Name == "John Doe" &&
                                                                    e.Id == 100 &&
                                                                    e.JobTitle == "Analyst" &&
                                                                    e.Location == "dallas" &&
                                                                    e.Email == "test@email.com"
                                                                    ));
        }
    }
}
