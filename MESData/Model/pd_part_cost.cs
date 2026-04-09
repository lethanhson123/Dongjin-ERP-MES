namespace MESData.Model
{
    public partial class pd_part_cost : BaseModel
    {
        [Key]
        public int? PDCOST_IDX { get; set; }
public int? PDPART_IDX { get; set; }
public int? CMPNY_IDX { get; set; }
public DateTime? PD_COST_DATE { get; set; }
public string? PD_COST { get; set; }
public DateTime? CREATE_DTM { get; set; }
public string? CREATE_USER { get; set; }
public DateTime? UPDATE_DTM { get; set; }
public string? UPDATE_USER { get; set; }

        public pd_part_cost()
        {
        }
    }
}

