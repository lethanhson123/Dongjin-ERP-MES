namespace MESService.Interface
{
    public interface IB09Service : IBaseService<torderlist>
    {
        Task<BaseResult> Load();
        Task<BaseResult> Buttonfind_Click(BaseParameter BaseParameter);
        Task<BaseResult> Buttonadd_Click(BaseParameter BaseParameter);
        Task<BaseResult> Buttonsave_Click(BaseParameter BaseParameter);
        Task<BaseResult> Buttondelete_Click(BaseParameter BaseParameter);
        Task<BaseResult> Buttoncancel_Click(BaseParameter BaseParameter);
        Task<BaseResult> Buttoninport_Click(BaseParameter BaseParameter);
        Task<BaseResult> Buttonexport_Click(BaseParameter BaseParameter);
        Task<BaseResult> Buttonprint_Click(BaseParameter BaseParameter);
        Task<BaseResult> Buttonhelp_Click(BaseParameter BaseParameter);
        Task<BaseResult> Buttonclose_Click(BaseParameter BaseParameter);
        Task<BaseResult> COMLIST_LINE(BaseParameter BaseParameter);
        Task<BaseResult> GetListCategoryDepartmentByMembershipID_CompanyID_ActiveToListAsync(BaseParameter BaseParameter);
        Task<BaseResult> GetListMaterialBySearchString_ActiveToListAsync(BaseParameter BaseParameter);
        Task<BaseResult> SaveListWarehouseRequestDetailAsync(BaseParameter BaseParameter);
        Task<BaseResult> GetWarehouseRequestDetailToListAsync(BaseParameter BaseParameter);
        Task<BaseResult> RemoveWarehouseRequestDetailByIDAsync(BaseParameter BaseParameter);
        Task<BaseResult> GetWarehouseOutputByCategoryDepartmentIDAndDateToListAsync(BaseParameter BaseParameter);
        Task<BaseResult> GetWarehouseOutputByCompanyIDAndMembershipIDAndCategoryDepartmentIDAndDateToListAsync(BaseParameter BaseParameter);
        Task<BaseResult> GetWarehouseOutputDetailByParentIDToListAsync(BaseParameter BaseParameter);
        Task<BaseResult> SaveWarehouseInputByWarehouseOutputIDAsync(BaseParameter BaseParameter);
        Task<BaseResult> SaveWarehouseOutputDetailByIDAsync(BaseParameter BaseParameter);
        Task<BaseResult> GetWarehouseRequestByCategoryDepartmentIDAndDateToListAsync(BaseParameter BaseParameter);
        Task<BaseResult> GetWarehouseRequestDetailByParentIDToListAsync(BaseParameter BaseParameter);
        Task<BaseResult> SaveWarehouseOutputDetailBarcodeAsync(BaseParameter BaseParameter);
    }
}


