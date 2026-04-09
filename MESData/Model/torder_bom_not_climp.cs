namespace MESData.Model
{
    public partial class torder_bom_not_climp : BaseModel
    {
        [Key]
        public int? CLIMP_IDX { get; set; }
public string? CLIMP_TERM { get; set; }

        public torder_bom_not_climp()
        {
        }
    }
}

