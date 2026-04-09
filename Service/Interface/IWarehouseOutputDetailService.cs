namespace Service.Interface
{
    public interface IWarehouseOutputDetailService : IBaseService<WarehouseOutputDetail>
    {
        Task<BaseResult<WarehouseOutputDetail>> GetByCategoryDepartmentID_Year_Month_Day_SearchString_InventoryToListAsync(BaseParameter<WarehouseOutputDetail> BaseParameter);
        Task<BaseResult<WarehouseOutputDetail>> GetByActive_IsComplete_MembershipAsync(BaseParameter<WarehouseOutputDetail> BaseParameter);
    }
}

