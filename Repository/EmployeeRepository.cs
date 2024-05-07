using Employee.Controllers;
using Employee.Interface;
using Employee.Models;
using System.Data.Common;
using System.Data.SqlTypes;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System.Data;
using Dapper;
namespace Employee.Repository
{
    public class EmployeeRepository:IEmployee
    {
        private readonly DbContext _dbContext;
        public EmployeeRepository(DbContext dbContext) {
        _dbContext= dbContext;
        }

        public async Task<IEnumerable<Employees>> Get()
        {
            var query = "SELECT * FROM tbl_Employee";
            using (var connection = _dbContext.CreateConnection())
            {
                var companies = await connection.QueryAsync<Employees>(query);
                return companies.ToList();
            }
                
        }

        public async Task<Employees> GetEmployeeByEmp_Id(int empId)
        {
            var query = "SELECT * FROM tbl_Employee WHERE EmployeeId = @empId";

            using (var connection = _dbContext.CreateConnection())
            {
                var employee = await connection.QuerySingleOrDefaultAsync<Employees>(query, new { empId });

                return employee;
            }
        }
        public async Task<Employees> CreateEmployee(Employees employee)
        {
            var query = "INSERT INTO tbl_Employee (First_Name, Last_Name, Department) VALUES (@First_Name, @Last_Name, @Department)";

            var parameters = new DynamicParameters();
            parameters.Add("First_Name", employee.First_Name, DbType.String);
            parameters.Add("Last_Name", employee.Last_Name, DbType.String);
            parameters.Add("Department", employee.Department, DbType.String);

            using (var connection = _dbContext.CreateConnection())
            {
                var id = await connection.ExecuteAsync(query, parameters);
                var createdEmployee = new Employees
                {
                    First_Name = employee.First_Name,
                    Last_Name = employee.Last_Name,
                    Department = employee.Department
                };
                return createdEmployee;
            }
        }
        public async Task<Employees> UpdateEmployee(int emp_Id, Employees employee)
        {
            var query = "UPDATE tbl_Employee SET First_Name = @First_Name, Last_Name = @Last_Name, Department = @Department WHERE EmployeeId = @EmployeeId";
            var parameters = new DynamicParameters();
            parameters.Add("@EmployeeId", emp_Id, DbType.Int32);
            parameters.Add("@First_Name", employee.First_Name, DbType.String);
            parameters.Add("@Last_Name", employee.Last_Name, DbType.String);
            parameters.Add("@Department", employee.Department, DbType.String);
            using (var connection = _dbContext.CreateConnection())
            {
                var id = await connection.ExecuteAsync(query, parameters);
                var updateEmployee = new Employees
                {
                    EmployeeId= emp_Id,
                    First_Name = employee.First_Name,
                    Last_Name = employee.Last_Name,
                    Department = employee.Department
                };
                return updateEmployee;
            }
        }
        public async Task<int> DeleteEmployee(int Emp_Id)
        {
            var query = "DELETE FROM tbl_Employee WHERE EmployeeId = @Emp_Id";
            using (var connection = _dbContext.CreateConnection())
            {
               var emp_id= await connection.ExecuteAsync(query, new { Emp_Id });
                return emp_id;
            }
        }
        public async Task<IEnumerable<Employees>> GetEmployees(int pageNumber, int pageSize)
        {
            // Calculate the number of rows to skip
            int rowsToSkip = (pageNumber - 1) * pageSize;

            var query = $"SELECT * FROM tbl_Employee ORDER BY EmployeeId OFFSET {rowsToSkip} ROWS FETCH NEXT {pageSize} ROWS ONLY";

            using (var connection = _dbContext.CreateConnection())
            {
                return await connection.QueryAsync<Employees>(query);
            }
        }
        public async Task<int> GetTotalCount()
        {
            var query = "SELECT COUNT(*) FROM tbl_Employee"; // Assuming tbl_Employee is your employee table
            using (var connection = _dbContext.CreateConnection())
            {
                // ExecuteScalarAsync returns the first column of the first row in the result set as an object
                var result = await connection.ExecuteScalarAsync<int>(query);
                return result;
            }
        }

    }
}
