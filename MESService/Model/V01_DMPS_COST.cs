namespace MESService.Model
{
    public partial class V01_DMPS_COST : MESData.Model.BaseModel
    {
        public string? PN_NM { get; set; }
        public string? PN_V { get; set; }
        public string? PSPEC_V { get; set; }
        public string? UNIT_V { get; set; }
        public string? PN_K { get; set; }
        public string? PSPEC_K { get; set; }
        public string? UNIT_K { get; set; }
        public string? PQTY { get; set; }
        public DateTime? PD_COST_DATE { get; set; }
        public string? COST { get; set; }
        public string? Company { get; set; }
        public string? CREATE_DTM { get; set; }
        public string? CREATE_USER { get; set; }
        public string? UPDATE_DTM { get; set; }
        public string? UPDATE_USER { get; set; }
        public V01_DMPS_COST()
        {           
        }
    }
}

