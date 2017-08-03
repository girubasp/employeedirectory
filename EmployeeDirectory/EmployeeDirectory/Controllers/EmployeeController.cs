using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using Employee.Common;
using Employee.DomainObject;
using Employee.Service;
using EmployeeDirectory.Models;

namespace EmployeeDirectory.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ISearchEmployeeService _searchEmployee;
        private readonly IEmployeeService _employeeService;
        private readonly ILogger<EmployeeController> _logger;
        private readonly IMapper _mapper;
        private const int DEFAULTPAGESIZE = 10;
        public EmployeeController(ISearchEmployeeService searchEmployee, IEmployeeService employeeService, ILogger<EmployeeController> logger, IMapper mapper)
        {
            _searchEmployee = searchEmployee;
            _employeeService = employeeService;
            _logger = logger;
            _mapper = mapper;
        }
        // GET: EmployeeSearch
        
        public ActionResult Index()
        {
            return View(_searchEmployee.SearchEmployees(SearchBy.All, String.Empty, 0, DEFAULTPAGESIZE));
        }
        [HttpPost]
        public ActionResult SearchResults(SearchBy? searchBy, string searchTerm, int page)
        {
            return View(_searchEmployee.SearchEmployees(searchBy ?? SearchBy.All, searchTerm, page, DEFAULTPAGESIZE));
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Create(string name,string jobTitle,string location , string email,List<PhoneNumberViewModel> phoneNumber)
        {
            var mapped = _mapper.Map<List<PhoneNumber>>(phoneNumber.Where(p=> !string.IsNullOrEmpty(p.Number)));
            _employeeService.Create(new Employee.DomainObject.Employee()
            {
                Name = name,
                JobTitle = jobTitle,
                Location = location,
                Email = email,
                PhoneNumbers = mapped
            });
            return View("SearchResults", _searchEmployee.SearchEmployees(SearchBy.All, String.Empty, 0, DEFAULTPAGESIZE));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Update(int id, string name, string jobTitle, string location, string email, List<PhoneNumberViewModel> phoneNumber)
        {
            var employee = _employeeService.GetById(id);
            if (employee == null)
            {
                var ex = new Exception("Employee not found");
                _logger.LogError("Employee not found in controller" , ex);
                throw ex;
            }
            employee.Name = name;
            employee.JobTitle = jobTitle;
            employee.Location = location;
            employee.Email = email;
            foreach (var p in phoneNumber)
            {
                if (employee.PhoneNumbers.Any(existing => existing.Type == p.Type))
                {
                    if (!string.IsNullOrEmpty(p.Number))
                        employee.PhoneNumbers.First(existing => existing.Type == p.Type).Number = p.Number;
                    else
                        employee.PhoneNumbers.Remove(employee.PhoneNumbers.First(existing => existing.Type == p.Type));
                }
                else if(!string.IsNullOrEmpty(p.Number))
                {
                    employee.PhoneNumbers.Add(_mapper.Map<PhoneNumber>(p));
                }
            }

            _employeeService.Update(employee);
            return View("SearchResults",_searchEmployee.SearchEmployees(SearchBy.All, String.Empty, 0, DEFAULTPAGESIZE));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            var employee = _employeeService.GetById(id);
            if (employee == null)
            {
                var ex = new Exception("Employee not found");
                _logger.LogError("Employee not found in controller", ex);
                throw ex;
            }
            _employeeService.Delete(employee);
            return View("SearchResults", _searchEmployee.SearchEmployees(SearchBy.All, String.Empty, 0, DEFAULTPAGESIZE));
        }
    }
}