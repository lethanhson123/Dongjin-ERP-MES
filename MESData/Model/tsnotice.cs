namespace MESData.Model
{
    public partial class tsnotice : BaseModel
    {
        [Key]
        public int? Notice_IDX { get; set; }
public string? Title { get; set; }
public string? Contents { get; set; }
public DateTime? CREATE_DTM { get; set; }
public string? CREATE_USER { get; set; }

        public tsnotice()
        {
        }
    }
}

