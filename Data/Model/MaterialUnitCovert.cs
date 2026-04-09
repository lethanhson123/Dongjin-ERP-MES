namespace Data.Model
{
    public partial class MaterialUnitCovert : BaseModel
    {
        public long? MaterialID { get; set; }
        public string? MaterialName { get; set; }
        public long? CategoryUnitID01 { get; set; }
        public string? CategoryUnitName01 { get; set; }
        public long? CategoryUnitID02 { get; set; }
        public string? CategoryUnitName02 { get; set; }
        public decimal? Quantity01 { get; set; }
        public decimal? Quantity02 { get; set; }

        public MaterialUnitCovert()
        {
            Active = true;
            CompanyID = 17;
            CategoryUnitID01 = 7;
            CategoryUnitID02 = 13;
            Quantity01 = 1;
            Quantity02 = 1;
        }
    }
}

