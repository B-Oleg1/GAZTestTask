using GAZTestTask.Models;
using GAZTestTask.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace GAZTestTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private readonly EmployeeRepository _employeeRepository;
        private readonly DepartmentRepository _departmentRepository;

        public DepartmentsController(EmployeeRepository employeeRepository,
                                     DepartmentRepository departmentRepository)
        {
            _employeeRepository = employeeRepository;
            _departmentRepository = departmentRepository;
        }

        /// <summary>
        /// Создание нового подразделения
        /// </summary>
        /// <param name="createDepartmentModel">Модель подразделения</param>
        [HttpPost]
        public async Task<ActionResult<DepartmentModel>> CreateDepartment([FromBody] CreateDepartmentModel createDepartmentModel)
        {
            var createdDepartmentId = await _departmentRepository.CreateDepartment(createDepartmentModel);
            if (createdDepartmentId == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "Произошла ошибка при добавлении подразделения" });
            }

            var createdDepartment = await _departmentRepository.GetDepartmentById((long)createdDepartmentId);
            return Ok(createdDepartment);
        }

        /// <summary>
        /// Получение всех подразделений
        /// </summary>
        [HttpGet("All")]
        public async Task<ActionResult<IEnumerable<DepartmentModel>>> GetAllDepartments()
        {
            IEnumerable<DepartmentModel> departments = await _departmentRepository.GetAllDepartments();
            return Ok(departments);
        }

        /// <summary>
        /// Получение информации о сотрудниках в указанном подразделении
        /// </summary>
        /// <param name="departmentId">ID подразделения</param>
        /// <param name="withNested">Нужно ли учитывать вложенные подразделения</param>
        [HttpGet("{departmentId}/Employees")]
        public async Task<ActionResult<IEnumerable<EmployeeModel>>> GetEmployees(long departmentId,
                                                                                  [FromQuery] bool withNested)
        {
            IEnumerable<EmployeeModel> employees = [];
            if (withNested)
            {
                employees = await _employeeRepository.GetEmployeesInSpecifiedAndNestedDepartments(departmentId);
            }
            else
            {
                employees = await _employeeRepository.GetEmployeesInSpecifiedDepartment(departmentId);
            }

            return Ok(employees);
        }

        /// <summary>
        /// Получение дочерних подразделений у указанного подразделения
        /// </summary>
        /// <param name="departmentId">ID подразделения, для которого необходимо получить дочерние</param>
        [HttpGet("{departmentId}/Subdepartments")]
        public async Task<ActionResult<IEnumerable<DepartmentModel>>> GetAllNestedDepartments(long departmentId)
        {
            IEnumerable<DepartmentModel> departments = await _departmentRepository.GetAllNestedDepartments(departmentId);
            return Ok(departments);
        }
    }
}