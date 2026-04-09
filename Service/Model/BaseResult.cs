namespace Service.Model
{
    public partial class BaseResult<T>
    {
        public int? StatusCode { get; set; }
        public string? Message { get; set; }
        public string? Note { get; set; }
        public T? BaseModel { get; set; }
        public List<T>? List { get; set; }
        public int? Count { get; set; }
        public bool? IsCheck { get; set; }         
        public BaseResult()
        {
            StatusCode = 200;
            BaseModel = (T)Activator.CreateInstance(typeof(T));
            List = new List<T>();            
        }
    }
}

