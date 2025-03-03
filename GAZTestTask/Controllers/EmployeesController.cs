using GAZTestTask.Models;
using GAZTestTask.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace GAZTestTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly EmployeeRepository _employeeRepository;

        public EmployeesController(EmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        /// <summary>
        /// Создание нового сотрудника
        /// </summary>
        /// <param name="createEmployeeModel">Модель сотрудника</param>
        [HttpPost]
        public async Task<ActionResult<EmployeeModel>> CreateEmployee([FromBody] CreateEmployeeModel createEmployeeModel)
        {
            var createdEmployeeId = await _employeeRepository.CreateEmployee(createEmployeeModel);
            if (createdEmployeeId == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "Произошла ошибка при добавлении пользователя"});
            }

            var createdEmployee = await _employeeRepository.GetEmployeeById((long)createdEmployeeId);
            return Ok(createdEmployee);
        }
    }
}