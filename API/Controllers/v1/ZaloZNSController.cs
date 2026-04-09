namespace API.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class ZaloZNSController : BaseController<ZaloZNS, IZaloZNSService>
    {
        private readonly IZaloZNSService _ZaloZNSService;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public ZaloZNSController(IZaloZNSService ZaloZNSService, IWebHostEnvironment WebHostEnvironment) : base(ZaloZNSService, WebHostEnvironment)
        {
            _ZaloZNSService = ZaloZNSService;
            _WebHostEnvironment = WebHostEnvironment;
        }
        [HttpPost]
        [RequestSizeLimit(512 * 1024 * 1024)]
        [Route("SaveAndUploadFilesAsync")]
        public override async Task<BaseResult<ZaloZNS>> SaveAndUploadFilesAsync()
        {
            var result = new BaseResult<ZaloZNS>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<ZaloZNS>>(Request.Form["BaseParameter"]);
                if (BaseParameter != null)
                {
                    BaseParameter.BaseModel = new ZaloZNS();
                    if (Request.Form.Files.Count > 0)
                    {
                        var file = Request.Form.Files[0];
                        if (file == null || file.Length == 0)
                        {
                        }
                        if (file != null)
                        {
                            BaseParameter.BaseModel.Display = Path.GetExtension(file.FileName);
                            BaseParameter.BaseModel.FileName = BaseParameter.BaseModel.ID + "_" + BaseParameter.BaseModel.GetType().Name + "_" + GlobalHelper.InitializationDateTimeCode0001 + "_" + BaseParameter.BaseModel.Display;

                            string folderPath = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.Upload, BaseParameter.BaseModel.GetType().Name);
                            bool isFolderExists = System.IO.Directory.Exists(folderPath);
                            if (!isFolderExists)
                            {
                                System.IO.Directory.CreateDirectory(folderPath);
                            }
                            var physicalPath = Path.Combine(folderPath, BaseParameter.BaseModel.FileName);
                            using (var stream = new FileStream(physicalPath, FileMode.Create))
                            {
                                file.CopyTo(stream);
                            }

                            FileInfo fileLocation = new FileInfo(physicalPath);
                            if (fileLocation.Length > 0)
                            {
                                using (ExcelPackage package = new ExcelPackage(fileLocation))
                                {
                                    if (package.Workbook.Worksheets.Count > 0)
                                    {
                                        ExcelWorksheet workSheet = package.Workbook.Worksheets[1];
                                        if (workSheet != null)
                                        {
                                            int totalRows = workSheet.Dimension.Rows;
                                            BaseParameter.List = new List<ZaloZNS>();
                                            result.Note = GlobalHelper.InitializationDateTimeCode;
                                            for (int j = 2; j <= totalRows; j++)
                                            {
                                                ZaloZNS ZaloZNS = new ZaloZNS();
                                                ZaloZNS.Active = true;
                                                ZaloZNS.Code = result.Note;
                                                if (workSheet.Cells["A" + j].Value != null)
                                                {
                                                    ZaloZNS.Display = workSheet.Cells["A" + j].Value.ToString().Trim();
                                                }
                                                BaseParameter.List.Add(ZaloZNS);
                                            }
                                            await _ZaloZNSService.AddRangeAsync(BaseParameter);
                                            foreach (var ZaloZNS in BaseParameter.List)
                                            {
                                                BaseParameter.SearchString = ZaloZNS.Display;
                                                await _ZaloZNSService.SendZaloTuyenDungCongNhan2026Async(BaseParameter);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.Message = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("SendZaloTuyenDungCongNhan2026Async")]
        public virtual async Task<BaseResult<ZaloZNS>> SendZaloTuyenDungCongNhan2026Async()
        {
            var result = new BaseResult<ZaloZNS>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<ZaloZNS>>(Request.Form["BaseParameter"]);
                result = await _ZaloZNSService.SendZaloTuyenDungCongNhan2026Async(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
    }
}

