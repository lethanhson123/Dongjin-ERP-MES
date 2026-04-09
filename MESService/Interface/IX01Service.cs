namespace MESService.Interface
{
    public interface IX01Service : IBaseService<PartSpare>
    {
        // Main CRUD Operations
        Task<BaseResult> Load();
        Task<BaseResult> Buttonfind_Click(BaseParameter BaseParameter);
        Task<BaseResult> Buttonadd_Click(BaseParameter BaseParameter);
        Task<BaseResult> Buttonsave_Click(BaseParameter BaseParameter);
        Task<BaseResult> Buttondelete_Click(BaseParameter BaseParameter);
        Task<BaseResult> Buttoncancel_Click(BaseParameter BaseParameter);

        // Import/Export
        Task<BaseResult> Buttoninport_Click(BaseParameter BaseParameter);
        Task<BaseResult> Buttonexport_Click(BaseParameter BaseParameter);

        // Print QR Code
        Task<BaseResult> Buttonprint_Click(BaseParameter BaseParameter);

        // Help/Close
        Task<BaseResult> Buttonhelp_Click(BaseParameter BaseParameter);
        Task<BaseResult> Buttonclose_Click(BaseParameter BaseParameter);

        // Image Upload
        Task<BaseResult> UploadPartImage(IFormFile file, BaseParameter BaseParameter);

        // Scan In Operations
        Task<BaseResult> LoadScanInData(BaseParameter BaseParameter);
        Task<BaseResult> ButtonScanIn_Click(BaseParameter BaseParameter);

        // Scan Out Operations
        Task<BaseResult> LoadScanOutData(BaseParameter BaseParameter);
        Task<BaseResult> ButtonScanOut_Click(BaseParameter BaseParameter);

        // Helper Methods
        Task<BaseResult> GetPartInfoByCode(BaseParameter BaseParameter);
    }
}