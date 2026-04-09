namespace MESData.Model
{
    public partial class pd_mc_orderlist : BaseModel
    {
        [Key]
        public int? PD_MC_ORDER_IDX { get; set; }
public int? PD_PART_IDX { get; set; }
public DateTime? PD_PURDATE { get; set; }
public int? PD_MC_NO { get; set; }
public int? PD_MC_QTY { get; set; }
public string? PD_MC_UNIT { get; set; }
public double? PD_MC_COST { get; set; }
public string? PD_BYORDER { get; set; }
public string? PD_MC_CMPYNM { get; set; }
public string? PD_MC_STAY { get; set; }
public string? PD_MC_APP { get; set; }
public string? PD_MC_REMARK { get; set; }
public string? PD_ORDER_NO { get; set; }
public DateTime? PD_MC_ODDATE { get; set; }
public double? PD_PAYCOST { get; set; }
public string? PD_MC_DSN_YN { get; set; }
public DateTime? CREATE_DTM { get; set; }
public string? CREATE_USER { get; set; }
public DateTime? UPDATE_DTM { get; set; }
public string? UPDATE_USER { get; set; }

        public pd_mc_orderlist()
        {
        }
    }
}

