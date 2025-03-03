using Dapper;
using GAZTestTask.Models;
using Npgsql;

namespace GAZTestTask.Repositories
{
    public class EmployeeRepository
    {
        private readonly string _connectionString = string.Empty;

        public EmployeeRepository(IConfiguration configuration)
        {
            _connectionString = configuration["Database:DefaultConnection"] ?? throw new ArgumentNullException(nameof(configuration));
        }

        /// <summary>
        /// Создания новых сотрудников
        /// </summary>
        /// <param name="createEmployeeModel">Модель пользователя</param>
        public async Task<long?> CreateEmployee(CreateEmployeeModel createEmployeeModel)
        {
            long? createdEmployeeId = null;

            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    string query = "INSERT INTO employees (name, position, department_id) " +
                                   "VALUES (@Name, @Position, @DepartmentId) " +
                                   "RETURNING id;";

                    createdEmployeeId = await connection.ExecuteScalarAsync<long?>
                    (
                        query,
                        new
                        {
                            Name = createEmployeeModel.Name,
                            Position = createEmployeeModel.Position,
                            DepartmentId = createEmployeeModel.DepartmentId
                        }
                    );
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return createdEmployeeId;
        }

        /// <summary>
        /// Получение сотрудника по указанному ID
        /// </summary>
        /// <param name="employeeId">ID сотрудника</param>
        public async Task<EmployeeModel?> GetEmployeeById(long employeeId)
        {
            EmployeeModel? employee = null;

            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    string query = "SELECT id, name, position, department_id AS departmentId " +
                                   "FROM employees " +
                                   "WHERE id = @EmployeeId " +
                                   "LIMIT 1;";

                    employee = await connection.QuerySingleOrDefaultAsync<EmployeeModel>
                    (
                        query,
                        new
                        {
                            EmployeeId = employeeId
                        }
                    );
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return employee;
        }

        /// <summary>
        /// Получение всех сотрудников, работающих в указанном подразделении
        /// </summary>
        /// <param name="departmentId">ID подразделения</param>
        public async Task<IEnumerable<EmployeeModel>> GetEmployeesInSpecifiedDepartment(long departmentId)
        {
            IEnumerable<EmployeeModel> employees = [];

            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    string query = "SELECT id, name, position, department_id AS departmentId " +
                                   "FROM employees " +
                                   "WHERE department_id = @DepartmentId " +
                                   "ORDER BY id ASC;";

                    employees = await connection.QueryAsync<EmployeeModel>
                    (
                        query,
                        new
                        {
                            DepartmentId = departmentId
                        }
                    );
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return employees;
        }

        /// <summary>
        /// Получение количества сотрудников, работающих в указанном подразделении
        /// </summary>
        /// <param name="departmentId">ID подразделения</param>
        public async Task<long> GetCountEmployeesInSpecifiedDepartment(long departmentId)
        {
            long countRows = 0;

            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    string query = "SELECT COUNT(*) " +
                                   "FROM employees " +
                                   "WHERE department_id = @DepartmentId;";

                    countRows = await connection.QueryFirstOrDefaultAsync<long>
                    (
                        query,
                        new
                        {
                            DepartmentId = departmentId
                        }
                    );
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return countRows;
        }

        /// <summary>
        /// Получение всех сотрудников, работающих в указанном и вложенных подразделениях
        /// </summary>
        /// <param name="departmentId">ID подразделения</param>
        public async Task<IEnumerable<EmployeeModel>> GetEmployeesInSpecifiedAndNestedDepartments(long departmentId)
        {
            IEnumerable<EmployeeModel> employees = [];

            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    string query = "WITH RECURSIVE nested_departments AS ( " +
                                   "    SELECT id FROM departments WHERE id = @DepartmentId " +
                                   "    UNION " +
                                   "    SELECT d.id FROM departments d " +
                                   "    INNER JOIN nested_departments nd ON d.parent_department_id = nd.id" +
                                   ") " +
                                   "" +
                                   "SELECT id, name, position, department_id AS departmentId " +
                                   "FROM employees " +
                                   "WHERE department_id IN (SELECT id FROM nested_departments) " +
                                   "ORDER BY id ASC;";

                    employees = await connection.QueryAsync<EmployeeModel>
                    (
                        query,
                        new
                        {
                            DepartmentId = departmentId
                        }
                    );
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return employees;
        }

        /// <summary>
        /// Получение количества сотрудников, работающих в указанном и вложенных подразделениях
        /// </summary>
        /// <param name="departmentId">ID подразделения</param>
        public async Task<long> GetCountEmployeesInSpecifiedAndNestedDepartments(long departmentId)
        {
            long countRows = 0;

            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    string query = "WITH RECURSIVE nested_departments AS ( " +
                                   "    SELECT id FROM departments WHERE id = @DepartmentId " +
                                   "    UNION " +
                                   "    SELECT d.id FROM departments d " +
                                   "    INNER JOIN nested_departments nd ON d.parent_department_id = nd.id" +
                                   ") " +
                                   "" +
                                   "SELECT COUNT(*) " +
                                   "FROM employees " +
                                   "WHERE department_id IN (SELECT id FROM nested_departments);";

                    countRows = await connection.QueryFirstOrDefaultAsync<long>
                    (
                        query,
                        new
                        {
                            DepartmentId = departmentId
                        }
                    );
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return countRows;
        }
    }
}
