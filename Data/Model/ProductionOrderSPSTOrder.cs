namespace Data.Model
{
    public partial class ProductionOrderSPSTOrder : BaseModel
    {
        public long? ProductionOrderProductionPlanSemiID { get; set; }
        public long? BOMID01 { get; set; }
        public string? BOMECN01 { get; set; }
        public string? BOMECNVersion01 { get; set; }
        public DateTime? BOMDate01 { get; set; }
        public int? BOMQuantity01 { get; set; }
        public long? BOMID { get; set; }
        public string? BOMECN { get; set; }
        public string? BOMECNVersion { get; set; }
        public DateTime? BOMDate { get; set; }
        public int? BOMQuantity { get; set; }
        public long? MaterialID01 { get; set; }
        public string? MaterialCode01 { get; set; }
        public string? MaterialName01 { get; set; }
        public long? MaterialID { get; set; }
        public string? MaterialCode { get; set; }
        public string? MaterialName { get; set; }
        public string? Project { get; set; }
        public int? QuantityTotal { get; set; }
        public int? Quantity { get; set; }
        public string? CurrentLeads { get; set; }
        public string? CTLeads { get; set; }
        public string? CTLeadsPr { get; set; }
        public string? Group { get; set; }
        public string? Print { get; set; }
        public DateTime? Date { get; set; }
        public string? Machine { get; set; }
        public int? BundleSize { get; set; }
        public string? HookRack { get; set; }
        public string? Wire { get; set; }
        public string? T1Dir { get; set; }
        public string? Term1 { get; set; }
        public string? Strip1 { get; set; }
        public string? Seal1 { get; set; }
        public string? CCHW1 { get; set; }
        public string? ICHW1 { get; set; }
        public string? T2Dir { get; set; }
        public string? Term2 { get; set; }
        public string? Strip2 { get; set; }
        public string? Seal2 { get; set; }
        public string? CCHW2 { get; set; }
        public string? ICHW2 { get; set; }
        public string? SPST { get; set; }
        public ProductionOrderSPSTOrder()
        {
            Active = true;
            Date = GlobalHelper.InitializationDateTime;
        }
    }
}

