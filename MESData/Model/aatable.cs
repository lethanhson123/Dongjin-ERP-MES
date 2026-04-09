namespace MESData.Model
{
    public partial class aatable : BaseModel
    {
        [Key]
        public int? ID { get; set; }
public string? Name { get; set; }
public string? Email { get; set; }

        public aatable()
        {
        }
    }
}

