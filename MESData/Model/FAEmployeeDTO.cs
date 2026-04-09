namespace MESData.Model
{
    public class FAEmployeeDTO
    {
        public long ID { get; set; }
        public string? Name { get; set; }
        public string? EmpCode { get; set; }
        public string? CurrentAssignment { get; set; }
        public bool IsAssigned { get; set; }
    }
}

