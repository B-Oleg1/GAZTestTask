namespace GAZTestTask.Models
{
    public class EmployeeModel
    {
        /// <summary>
        /// ID сотрудника
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// ФИО сотрудника
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Должность сотрудника в компании
        /// </summary>
        public string Position { get; set; }

        /// <summary>
        /// ID подразделения, в котором работает пользователь
        /// </summary>
        public long? DepartmentId { get; set; }
    }
}