namespace Data.Model
{
    public partial class MaterialConvert : BaseModel
    {
        public long? MaterialID { get; set; }

        public MaterialConvert()
        {
            Active = true;
        }
    }
}

