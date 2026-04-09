namespace Data.Model
{
    public partial class BOM : BaseModel
    {
        public DateTime? Date { get; set; }
        public string? Version { get; set; }
        public long? CategoryFamilyID { get; set; }
        public string? CategoryFamilyName { get; set; }
        public long? MaterialID { get; set; }
        public string? MaterialName { get; set; }
        public string? MaterialCode { get; set; }
        public long? ECNDetailID { get; set; }
        public decimal? Quantity { get; set; }
        public long? BOMParentID { get; set; }
        public long? BOMComponentID { get; set; }
        public int? BundleSize { get; set; }
        public long? ParentID01 { get; set; }
        public string? ParentName01 { get; set; }
        public long? ParentID02 { get; set; }
        public string? ParentName02 { get; set; }
        public long? ParentID03 { get; set; }
        public string? ParentName03 { get; set; }
        public long? ParentID04 { get; set; }
        public string? ParentName04 { get; set; }
        public string? LeadNo { get; set; }
        public string? Project { get; set; }
        public string? Item { get; set; }
        public string? Group { get; set; }
        public string? DirT1 { get; set; }
        public string? DirT2 { get; set; }
        public string? NoT1 { get; set; }
        public string? NoT2 { get; set; }
        public string? AutoT1 { get; set; }
        public string? AutoT2 { get; set; }
        public string? WLink { get; set; }
        public string? Combination { get; set; }
        public string? Stage { get; set; }
        public string? CategoryLocationName { get; set; }
        public decimal? Strip1 { get; set; }
        public decimal? Strip2 { get; set; }
        public int? Level { get; set; }
        public int? RawMaterialCount { get; set; }
        public int? BOMCount { get; set; }
        public bool? IsLeadNo { get; set; }
        public bool? IsSPST { get; set; }
        public BOM()
        {
            Date = GlobalHelper.InitializationDateTime;
            Active = true;
        }
    }
}

