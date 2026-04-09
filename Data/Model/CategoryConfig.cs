namespace Data.Model
{
    public partial class CategoryConfig : BaseModel
    {
        public int? NumberView { get; set; }
        public int? NumberTimer { get; set; }
        public string? Font01 { get; set; }
        public int? FontSize01 { get; set; }
        public string? Color01 { get; set; }
        public string? Font02 { get; set; }
        public int? FontSize02 { get; set; }
        public string? Color02 { get; set; }

        public CategoryConfig()
        {
            Active = true;
            NumberView = 5;
            Font01 = "Arial";
            FontSize01 = 30;
            Color01 = "b71c1c";
            Font02 = "Arial";
            FontSize02 = 20;
            Color02 = "827717";
        }
    }
}

