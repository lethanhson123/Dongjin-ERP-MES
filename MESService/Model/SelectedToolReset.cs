namespace MESService.Model
{
    public partial class SelectedToolReset : MESData.Model.BaseModel
    {
        public int TOOLMASTER_IDX { get; set; }
        public int? WK_CNT { get; set; }
        public int? TOT_WK_CNT { get; set; }
        public string? APPLICATOR { get; set; }
        public string? SEQ { get; set; }
        public string? CREATE_DTM { get; set; }
        public string? CREATE_USER { get; set; }
        public string? UPDATE_DTM { get; set; }
        public string? UPDATE_USER { get; set; }

        public SelectedToolReset()
        {
        }
    }
}