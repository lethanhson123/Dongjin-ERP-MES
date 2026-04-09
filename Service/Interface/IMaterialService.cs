namespace Service.Interface
{
    public interface IMaterialService : IBaseService<Material>
    {       
        Task<BaseResult<Material>> CreateAutoAsync(BaseParameter<Material> BaseParameter);
        Task<BaseResult<Material>> SyncParentChildAsync(BaseParameter<Material> BaseParameter);
        Task<BaseResult<Material>> SyncByWarehouseAsync(BaseParameter<Material> BaseParameter);
        Task<BaseResult<Material>> GetByWarehouseInputIDToListAsync(BaseParameter<Material> BaseParameter);
        Task<BaseResult<Material>> GetByWarehouseOutputIDToListAsync(BaseParameter<Material> BaseParameter);
      
        Task<BaseResult<Material>> GetByCategoryMaterialIDToListAsync(BaseParameter<Material> BaseParameter);
        Task<BaseResult<Material>> GetByCategoryMaterialID_ActiveToListAsync(BaseParameter<Material> BaseParameter);
        Task<BaseResult<Material>> GetByParentID_ActiveToListAsync(BaseParameter<Material> BaseParameter);
        Task<BaseResult<Material>> PrintAsync(BaseParameter<Material> BaseParameter);
        Task<BaseResult<Material>> GetByCompanyID_CategoryMaterialID_SearchStringToListAsync(BaseParameter<Material> BaseParameter);
        Task<BaseResult<Material>> GetByCompanyID_CategoryMaterialID_ActiveToListAsync(BaseParameter<Material> BaseParameter);
        Task<BaseResult<Material>> GetFromtmmtinByIDToListAsync(BaseParameter<Material> BaseParameter);
        Task<BaseResult<Material>> ExportToExcelAsync(BaseParameter<Material> BaseParameter);
    }
}

