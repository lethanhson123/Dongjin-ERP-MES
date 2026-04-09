namespace MESData.Model
{
    public partial class tsuser : BaseModel
    {
        [Key]
        public int? USER_IDX { get; set; }
        public string? USER_ID { get; set; }
        public string? USER_NM { get; set; }
        public string? PW { get; set; }
        public string? Dept { get; set; }
        public string? Note { get; set; }
        public string? DESC_YN { get; set; }
        public DateTime? CREATE_DTM { get; set; }
        public string? CREATE_USER { get; set; }
        public DateTime? UPDATE_DTM { get; set; }
        public string? UPDATE_USER { get; set; }

        public tsuser()
        {
        }
    }
}

