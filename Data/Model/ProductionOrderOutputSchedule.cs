namespace Data.Model
{
    public partial class ProductionOrderOutputSchedule : BaseModel
    {       
        public DateTime? Begin { get; set; }
        public DateTime? End { get; set; }

        public ProductionOrderOutputSchedule()
        {
            Begin = GlobalHelper.InitializationDateTime;
            End = GlobalHelper.InitializationDateTime;
        }
    }
}

