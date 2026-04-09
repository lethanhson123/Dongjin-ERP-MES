namespace MESService.Interface
{
    public interface IC05_DC_READService : IBaseService<torderlist>
    {
        Task<BaseResult> TB_BARCODE_KeyDown(BaseParameter BaseParameter);
        Task<BaseResult> BARCODE_LOAD2(BaseParameter BaseParameter);
    }
}


