namespace MESService.Interface
{
    public interface IC02_ERRORService : IBaseService<torderlist>
    {       
        Task<BaseResult> C02_ERROR_Load(BaseParameter BaseParameter);
        Task<BaseResult> SW_TIME(BaseParameter BaseParameter);
        Task<BaseResult> EW_TIME(BaseParameter BaseParameter);
        Task<BaseResult> C02_ERROR_FormClosed(BaseParameter BaseParameter);
        Task<BaseResult> Button2_Click(BaseParameter BaseParameter);
        Task<BaseResult> Button1_Click(BaseParameter BaseParameter);
        Task<BaseResult> Button1_ClickSub001(BaseParameter BaseParameter);
        Task<BaseResult> Button1_ClickSub002(BaseParameter BaseParameter);
    }
}


