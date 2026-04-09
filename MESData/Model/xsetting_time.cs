namespace MESData.Model
{
    public partial class xsetting_time : BaseModel
    {
        [Key]
        public int? SETTING_IDX { get; set; }
public string? TABLE_NM { get; set; }
public string? SUB_NM { get; set; }
public string? REMARK { get; set; }
public double? BASIC_TIME { get; set; }
public string? UNIT { get; set; }
public double? TIME { get; set; }

        public xsetting_time()
        {
        }
    }
}

