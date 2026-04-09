namespace MESService.Model
{
    public partial class B01 : MESData.Model.BaseModel
    {
        public bool? dgvcheck { get; set; }
        public string? NO { get; set; }
        public string? PARTNO { get; set; }
        public string? UNIT { get; set; }
        public string? GOODS { get; set; }
        public int? QUANTITY { get; set; }
        public string? NWEIGHT { get; set; }
        public string? GWEIGHT { get; set; }
        public string? PalletNo { get; set; }
        public string? ShippedNo { get; set; }
        public string? inputdate { get; set; }
        public string? Remark { get; set; }
        public B01()
        {
            NO = GlobalHelper.InitializationString;
            PARTNO = GlobalHelper.InitializationString;
            UNIT = GlobalHelper.InitializationString;
            GOODS = GlobalHelper.InitializationString;
            QUANTITY = GlobalHelper.InitializationNumber;
            NWEIGHT = GlobalHelper.InitializationString;
            GWEIGHT = GlobalHelper.InitializationString;
            PalletNo = GlobalHelper.InitializationString;
            ShippedNo = GlobalHelper.InitializationString;
            inputdate = GlobalHelper.InitializationString;
            Remark = GlobalHelper.InitializationString;
        }
    }
}

