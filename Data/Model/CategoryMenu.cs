namespace Data.Model
{
    public partial class CategoryMenu : BaseModel
    {
        public string? NameVietNam { get; set; }
        public string? NameEnglish { get; set; }
        public string? NameKorea { get; set; }
        public CategoryMenu()
        {
            ParentID = GlobalHelper.InitializationNumber;
            Active = true;
            Code = "#";
            Display = "chrome_reader_mode";
        }
    }
}

