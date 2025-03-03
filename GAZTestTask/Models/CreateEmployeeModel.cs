using System.ComponentModel.DataAnnotations;

namespace GAZTestTask.Models
{
    public class CreateEmployeeModel
    {
        /// <summary>
        /// ФИО сотрудника
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Не указано наименование сотрудника")]
        [Length(1, 255, ErrorMessage = "Наименование сотрудника должно быть от 1 до 255 символов")]
        public string Name { get; set; }

        /// <summary>
        /// Наименование должности
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Не указана должность сотрудника")]
        [Length(1, 255, ErrorMessage = "Должность сотрудника должна быть от 1 до 255 символов")]
        public string Position { get; set; }

        /// <summary>
        /// ID подразделения
        /// </summary>
        public long? DepartmentId { get; set; } = null;
    }
}