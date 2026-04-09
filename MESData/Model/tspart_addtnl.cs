namespace MESData.Model
{
    public partial class tspart_addtnl : BaseModel
    {
        [Key]
        public int? PART_ADDTNL_IDX { get; set; }
public int? PARTNO_IDX { get; set; }
public double? OP_W { get; set; }
public double? OP_L { get; set; }
public double? OP_H { get; set; }
public double? OP_MRP { get; set; }
public string? OP_TMP1 { get; set; }
public string? OP_TMP2 { get; set; }
public string? OP_TMP3 { get; set; }
public double? Weight { get; set; }
public DateTime? CREATE_DTM { get; set; }
public string? CREATE_USER { get; set; }
public DateTime? UPDATE_DTM { get; set; }
public string? UPDATE_USER { get; set; }

        public tspart_addtnl()
        {
        }
    }
}

