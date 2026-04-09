namespace MESData.Model
{
    public partial class tspart : BaseModel
    {
        [Key]
        public int? PART_IDX { get; set; }
        public string? PART_NO { get; set; }
        public string? PART_NM { get; set; }
        public string? BOM_GRP { get; set; }
        public string? PART_CAR { get; set; }
        public string? PART_FML { get; set; }
        public int? PART_SNP { get; set; }
        public int? PART_SCN { get; set; }
        public string? PART_LOC { get; set; }
        public string? PART_USENY { get; set; }
        public string? PART_RMK { get; set; }
        public string? PART_SUPL { get; set; }
        public DateTime? CREATE_DTM { get; set; }
        public string? CREATE_USER { get; set; }
        public DateTime? UPDATE_DTM { get; set; }
        public string? UPDATE_USER { get; set; }

        public tspart()
        {
        }
    }
}

