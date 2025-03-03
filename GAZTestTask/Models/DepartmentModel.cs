namespace GAZTestTask.Models
{
    public class DepartmentModel
    {
        /// <summary>
        /// ID подразделения
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Наименование подразделения
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// ID родительского подразделения
        /// </summary>
        public long? ParentDepartmentId { get; set; }
    }
}