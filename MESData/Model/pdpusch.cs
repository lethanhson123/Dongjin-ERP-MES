namespace MESData.Model
{
    public partial class pdpusch : BaseModel
    {
        [Key]
        public int? PDPUSCH_IDX { get; set; }
public string? PDP_CONF { get; set; }
public string? PDP_FACT { get; set; }
public long? PDP_NO { get; set; }
public string? PDP_COMPY_NO { get; set; }
public DateTime? PDP_DATE1 { get; set; }
public int? PDP_DEPA { get; set; }
public int? PDP_PART { get; set; }
public int? PDP_QTY { get; set; }
public string? PDP_MEMO { get; set; }
public string? PDP_REC_YN { get; set; }
public int? PDP_CMPY { get; set; }
public DateTime? PDP_DATE2 { get; set; }
public double? PDP_COST { get; set; }
public double? PDP_VAT { get; set; }
public double? PDP_ECTCOST { get; set; }
public double? PDP_TOTCOST { get; set; }
public DateTime? PDP_CNF_DATE { get; set; }
public string? PDP_CNF_YN { get; set; }
public string? PDP_ORD_ST { get; set; }
public string? PDP_REMARK { get; set; }
public double? PDP_BE_COST { get; set; }
public DateTime? PDP_INCO_DT { get; set; }
public int? PDP_IN_QTY { get; set; }
public string? PDP_FIFO { get; set; }
public string? PDP_PRIENT { get; set; }
public DateTime? CREATE_DTM { get; set; }
public string? CREATE_USER { get; set; }
public DateTime? UPDATE_DTM { get; set; }
public string? UPDATE_USER { get; set; }

        public pdpusch()
        {
        }
    }
}

