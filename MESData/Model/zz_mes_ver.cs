namespace MESData.Model
{
    public partial class zz_mes_ver : BaseModel
    {
        [Key]
        public int? VER_IDX { get; set; }
public int? VER_MAJOR { get; set; }
public int? VER_MINJOR { get; set; }
public int? VER_BUILD { get; set; }
public int? VER_REVISION { get; set; }

        public zz_mes_ver()
        {
        }
    }
}

