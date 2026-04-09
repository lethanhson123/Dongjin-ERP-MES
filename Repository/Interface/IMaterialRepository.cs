namespace Repository.Interface
{
    public interface IMaterialRepository : IBaseRepository<Material>
    {
        Material GetByDescription(string Description, long? CompanyID);
        Task<Material> GetByDescriptionAsync(string Description, long? CompanyID);
        Material GetByDescription_CompanyID_QuantitySNP(string Description, long? CompanyID, int? QuantitySNP);
        Material GetByDescription_CompanyID_QuantitySNP_Name(string Description, long? CompanyID, int? QuantitySNP, string Name);
    }
}

