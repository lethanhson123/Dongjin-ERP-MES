namespace Data.Model
{
    public partial class MaterialReplace : BaseModel
    {
        public long? BOMID { get; set; }
        public string? ECN { get; set; }
        public long? MaterialID01 { get; set; }
        public string? MaterialName01 { get; set; }
        public decimal? Quantity01 { get; set; }
        public long? MaterialID02 { get; set; }
        public string? MaterialName02 { get; set; }
        public decimal? Quantity02 { get; set; }     
        public MaterialReplace()
        {
            Active = true;
            Quantity01 = 1;
            Quantity02 = 1;
        }
    }
}

