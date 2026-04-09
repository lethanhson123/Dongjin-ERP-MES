namespace MESData.Model
{
    public partial class zadmin_function : BaseModel
    {
        [Key]
        public int? ZADMIN_FUNCTION_IDX { get; set; }
public string? ZADMIN_FUNCTION_CODE { get; set; }
public string? ZADMIN_FUNCTION_NAME { get; set; }
public string? ZADMIN_FUNCTION_YN { get; set; }
public string? ZADMIN_FUNCTION_DATE { get; set; }
public string? ZADMIN_FUNCTION_REMARK { get; set; }

        public zadmin_function()
        {
        }
    }
}

