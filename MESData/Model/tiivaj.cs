namespace MESData.Model
{
    public partial class tiivaj : BaseModel
    {
        [Key]
        public int? IVAJ_IDX { get; set; }
public int? PART_IDX { get; set; }
public int? ADJ_SCN { get; set; }
public DateTime? ADJ_DTM { get; set; }
public double? ADJ_QTY { get; set; }
public double? ADJ_BF_QTY { get; set; }
public double? ADJ_AF_QTY { get; set; }
public string? ADJ_RSON { get; set; }
public DateTime? CREATE_DTM { get; set; }
public string? CREATE_USER { get; set; }
public DateTime? UPDATE_DTM { get; set; }
public string? UPDATE_USER { get; set; }

        public tiivaj()
        {
        }
    }
}

