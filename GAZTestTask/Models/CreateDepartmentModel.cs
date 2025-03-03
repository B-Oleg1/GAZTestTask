using System.ComponentModel.DataAnnotations;

namespace GAZTestTask.Models
{
    public class CreateDepartmentModel
    {
        /// <summary>
        /// Наименование подразделения
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Не указано наименование подразделения")]
        [Length(1, 255, ErrorMessage = "Наименование подразделения должно быть от 1 до 255 символов")]
        public string Name { get; set; }

        /// <summary>
        /// ID родительского подразделения
        /// </summary>
        public long? ParentDepartmentId { get; set; } = null;
    }
}