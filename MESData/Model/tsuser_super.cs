namespace MESData.Model
{
    public partial class tsuser_super : BaseModel
    {
        [Key]
        public int? USER_IDX { get; set; }
public string? USER_ID_NAME { get; set; }
public string? NAS_TEST_SER { get; set; }
public string? LOC_TEST_SER { get; set; }
public string? DESC_YN { get; set; }
public DateTime? CREATE_DTM { get; set; }
public string? CREATE_USER { get; set; }
public DateTime? UPDATE_DTM { get; set; }
public string? UPDATE_USER { get; set; }

        public tsuser_super()
        {
        }
    }
}

