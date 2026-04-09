namespace MESService.Interface
{
    public interface ID04_PNO_CHKService : IBaseService<torderlist>
    {
        Task<BaseResult> PO_CODE(BaseParameter BaseParameter);
        Task<BaseResult> TextBoxA2_KeyDown(BaseParameter BaseParameter);
    }
}


