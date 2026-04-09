namespace Data.Model
{
    public partial class WarehouseRequestConfirm : BaseModel
    {

        public DateTime? Date { get; set; }
        public long? MembershipID { get; set; }
        public string? MembershipName { get; set; }

        public WarehouseRequestConfirm()
        {
            Date = GlobalHelper.InitializationDateTime;
        }
    }
}

