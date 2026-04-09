namespace Service.Model
{
    public partial class BaseParameter<T> : Data.Model.BaseModel
    {       

        public T? BaseModel { get; set; }
        public List<T>? List { get; set; }
        public List<long>? ListID { get; set; }
        public string? SearchString { get; set; }        
        public string? SearchStringFilter { get; set; }
        public string? SearchStringFilter01 { get; set; }
        public string? SearchStringFilter02 { get; set; }
        public List<string>? ListSearchString { get; set; }
        public int? Page { get; set; }
        public int? PageSize { get; set; }
        public int? Year { get; set; }
        public int? Month { get; set; }
        public int? Day { get; set; }
        public int? Quarter { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? Token { get; set; }
        public bool? IsUpload { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? DateBegin { get; set; }
        public DateTime? DateEnd { get; set; }
        public int? Action { get; set; }
        public long? CategoryDepartmentID { get; set; }
        public long? CategoryPositionID { get; set; }
        public bool? IsComplete { get; set; }
        public long? MembershipID { get; set; }
        public int? Quantity { get; set; }
        public decimal? QuantityDecimal { get; set; }
        public long? GeneralID { get; set; }        
        public string? Barcode { get; set; }
        public BaseParameter()
        {            
            BaseModel = (T)Activator.CreateInstance(typeof(T));
            List = new List<T>();
        }

    }
}
