using Employee.Interface;
using Employee.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Employee.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployee _employee;
        public EmployeeController(IEmployee employee)
        {
            _employee = employee;
        }
        [HttpGet("GetEmployee")]
        public async Task<IActionResult> GetEmployeey()
        {
           try {
                var company = await _employee.Get();
                if (company == null)
                {
                    return NotFound();
                }
                else
                {

                    return Ok(company);
                }
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }


        [HttpGet("GetEmployeeByEmpId")]
        public async Task<IActionResult> GetEmployeeByEmp_ID(int id)
        {
            try
            {
                var company = await _employee.GetEmployeeByEmp_Id(id);
                if (company == null)
                {
                 return NotFound();
                }
                else
                {

                return Ok(company);
                }
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPost("CreateEmployees")]
        public async Task<IActionResult> CreateEmployees(Employees employees)
        {
            try
            {

                var employee = await _employee.CreateEmployee(employees);
                return CreatedAtAction(nameof(CreateEmployees), new { id = employee.EmployeeId }, employee);

            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPut("UpdateEmployeeById")]
        public async Task<IActionResult> UpdateEmployeeById(int id, Employees company)
        {
            try
            {
                var dbCompany = await _employee.UpdateEmployee(id, company);
                if (dbCompany == null)
                    return NotFound();

                await _employee.UpdateEmployee(id, company);
                return Ok(dbCompany);
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("DeleteEmployee")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            try
            {
                var emp_Id = await _employee.DeleteEmployee(id);
                return Ok(emp_Id);

            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet("GetEmployeesPagination")]
        public async Task<IActionResult> GetEmployeesPagination(int pageNumber = 1, int pageSize = 10)
        {
            var employees = await _employee.GetEmployees(pageNumber, pageSize);

            if (employees.Any())
            {
                // Calculate total count of records (assuming _employeeService.GetTotalCount() retrieves the total count)
                var totalCount = await _employee.GetTotalCount();
                var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

                // Return data with pagination metadata
                var response = new
                {
                    TotalCount = totalCount,
                    TotalPages = totalPages,
                    PageSize = pageSize,
                    CurrentPage = pageNumber,
                    Employees = employees
                };

                return Ok(response);
            }
            else
            {
                return NotFound();
            }
        }

    }
}
