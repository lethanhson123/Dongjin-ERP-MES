namespace Data.Model
{
    public partial class CategoryDepartment : BaseModel
    {
        public int? MESID { get; set; }
        public bool? IsSync { get; set; }
        public bool? IsFinishGoods { get; set; }
        public CategoryDepartment()
        {
        }
    }
}

