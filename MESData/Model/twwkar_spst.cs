namespace MESData.Model
{
    public partial class twwkar_spst : BaseModel
    {
        [Key]
        public int? WK_IDX { get; set; }
public int? TORDER_IDX { get; set; }
public string? PART_IDX { get; set; }
public string? MC_NO { get; set; }
public double? WK_QTY { get; set; }
public string? WK_CM { get; set; }
public DateTime? CREATE_DTM { get; set; }
public string? CREATE_USER { get; set; }
public DateTime? UPDATE_DTM { get; set; }
public string? UPDATE_USER { get; set; }

        public twwkar_spst()
        {
        }
    }
}

