using Employee.Models;

namespace Employee.Interface
{
    public interface IEmployee
    {
        public Task<IEnumerable<Employees>> Get();
        public Task<Employees> GetEmployeeByEmp_Id(int id);
        public Task<Employees> CreateEmployee(Employees employee);
        public Task<Employees> UpdateEmployee(int id, Employees employee);
        public Task<int> DeleteEmployee(int emp_Id);
        public Task<IEnumerable<Employees>> GetEmployees(int pageNumber, int pageSize);
        public Task<int> GetTotalCount();
    }
}
