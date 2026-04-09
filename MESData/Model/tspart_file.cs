namespace MESData.Model
{
    public partial class tspart_file : BaseModel
    {
        [Key]
        public int? PARTFILE_IDX { get; set; }
public string? DWG_FILE_GRP { get; set; }
public int? FILE_NO { get; set; }
public string? FILE_FOLDER { get; set; }
public string? FILE_NAME { get; set; }
public string? FILE_REMARK { get; set; }
public string? FILE_USEYN { get; set; }
public DateTime? CREATE_DTM { get; set; }
public string? CREATE_USER { get; set; }
public DateTime? UPDATE_DTM { get; set; }
public string? UPDATE_USER { get; set; }

        public tspart_file()
        {
        }
    }
}

