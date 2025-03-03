using Dapper;
using GAZTestTask.Models;
using Npgsql;

namespace GAZTestTask.Repositories
{
    public class DepartmentRepository
    {
        private readonly string _connectionString = string.Empty;

        public DepartmentRepository(IConfiguration configuration)
        {
            _connectionString = configuration["Database:DefaultConnection"] ?? throw new ArgumentNullException(nameof(configuration));
        }

        /// <summary>
        /// Получение всех подразделений
        /// </summary>
        public async Task<long?> CreateDepartment(CreateDepartmentModel createDepartmentModel)
        {
            long? createdDepartmentId = null;

            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    string query = "INSERT INTO departments (name, parent_department_id) " +
                                   "VALUES (@Name, @ParentDepartmentId) " +
                                   "RETURNING id;";

                    createdDepartmentId = await connection.ExecuteScalarAsync<long?>
                    (
                        query,
                        new
                        {
                            Name = createDepartmentModel.Name,
                            ParentDepartmentId = createDepartmentModel.ParentDepartmentId,
                        }
                    );
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return createdDepartmentId;
        }

        /// <summary>
        /// Получение подразделения по указанному ID
        /// </summary>
        /// <param name="departmentId">ID подразделения</param>
        public async Task<DepartmentModel?> GetDepartmentById(long departmentId)
        {
            DepartmentModel? department = null;

            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    string query = "SELECT id, name, parent_department_id AS parentDepartmentId " +
                                   "FROM departments " +
                                   "WHERE id = @DepartmentId " +
                                   "LIMIT 1;";

                    department = await connection.QuerySingleOrDefaultAsync<DepartmentModel>
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

            return department;
        }

        /// <summary>
        /// Получение всех подразделений
        /// </summary>
        public async Task<IEnumerable<DepartmentModel>> GetAllDepartments()
        {
            IEnumerable<DepartmentModel> departments = [];

            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    string query = "SELECT id, name, parent_department_id AS parentDepartmentId " +
                                   "FROM departments " +
                                   "ORDER BY id ASC;";

                    departments = await connection.QueryAsync<DepartmentModel>
                    (
                        query
                    );
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return departments;
        }

        /// <summary>
        /// Получение всех вложенных подразделений у указанного ID подразделения
        /// </summary>
        /// <param name="departmentId">ID подразделения, для которого вернутся вложенные в него подразделения</param>
        public async Task<IEnumerable<DepartmentModel>> GetAllNestedDepartments(long departmentId)
        {
            IEnumerable<DepartmentModel> nestedDepartments = [];

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
                                   "SELECT id, name, parent_department_id AS parentDepartmentId " +
                                   "FROM departments " +
                                   "WHERE id IN (SELECT id FROM nested_departments) " +
                                   "ORDER BY id ASC;";

                    nestedDepartments = await connection.QueryAsync<DepartmentModel>
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

            return nestedDepartments;
        }
    }
}
