namespace MESData.Model
{
    public partial class tsyear_group_inv_hist : BaseModel
    {
        [Key]
        public int? TSYEAR_GROUP_IDX { get; set; }
public string? TSYEAR_YEAR { get; set; }
public int? TSYEAR_MESNO { get; set; }
public string? TSYEAR_DEPART { get; set; }
public string? TSYEAR_PKILOC { get; set; }
public string? TSYEAR_INPUTER { get; set; }
public int? TSYEAR_SERIAL_NO1 { get; set; }
public int? TSYEAR_SERIAL_NO2 { get; set; }
public DateTime? CREATE_DTM { get; set; }
public string? CREATE_USER { get; set; }
public DateTime? UPDATE_DTM { get; set; }
public string? UPDATE_USER { get; set; }

        public tsyear_group_inv_hist()
        {
        }
    }
}

