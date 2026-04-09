namespace MESData.Model
{
    public partial class KOMAXCheckErrorProofHistory : BaseModel
    {
        [Key]
        public long ID { get; set; }
        public long? ParentID { get; set; }
        public string? ParentName { get; set; }
        public DateTime? CreateDate { get; set; }
        public long? CreateUserID { get; set; }
        public string? CreateUserCode { get; set; }
        public string? CreateUserName { get; set; }
        public DateTime? UpdateDate { get; set; }
        public long? UpdateUserID { get; set; }
        public string? UpdateUserCode { get; set; }
        public string? UpdateUserName { get; set; }
        public int? RowVersion { get; set; }
        public int? SortOrder { get; set; }
        public bool? Active { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Display { get; set; }
        public string? Description { get; set; }
        public string? Note { get; set; }
        public string? FileName { get; set; }
        public long? CompanyID { get; set; }
        public string? CompanyName { get; set; }

        public KOMAXCheckErrorProofHistory()
        {
        }
    }
}

