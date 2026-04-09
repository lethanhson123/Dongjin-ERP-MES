namespace MES.Controllers
{
    public class B07_3Controller : Controller
    {
        private readonly IB07_3Service _B07_3Service;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public B07_3Controller(IB07_3Service B07_3Service, IWebHostEnvironment webHostEnvironment)
        {
            _B07_3Service = B07_3Service;
            _WebHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<BaseResult> Buttonfind_Click()
        {
            BaseResult BaseResult = new BaseResult();
            try
            {
                BaseParameter BaseParameter = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                BaseResult = await _B07_3Service.Buttonfind_Click(BaseParameter);
            }
            catch (Exception ex)
            {
                BaseResult.Error = ex.Message;
            }
            return BaseResult;
        }
        [HttpPost]
        public async Task<BaseResult> Buttonsave_Click()
        {
            BaseResult BaseResult = new BaseResult();
            try
            {
                BaseParameter BaseParameter = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                BaseResult = await _B07_3Service.Buttonsave_Click(BaseParameter);
            }
            catch (Exception ex)
            {
                BaseResult.Error = ex.Message;
            }
            return BaseResult;
        }
        [HttpPost]
        public async Task<BaseResult> Buttoninport_Click()
        {
            BaseResult BaseResult = new BaseResult();
            BaseResult.ErrorNumber = GlobalHelper.InitializationNumber;
            BaseResult.DataGridView1 = new List<SuperResultTranfer>();
            try
            {               
                BaseParameter BaseParameter = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                //if (Request.Form.Files.Count > 0)
                //{
                //    for (int i = 0; i < Request.Form.Files.Count; i++)
                //    {
                //        var file = Request.Form.Files[i];
                //        if (file == null || file.Length == 0)
                //        {
                //        }
                //        if (file != null)
                //        {
                //            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                //            string fileExtension = Path.GetExtension(file.FileName);
                //            if (fileExtension == ".xlsx" || fileExtension == ".xls")
                //            {
                //                string fileName = controllerName + "_" + GlobalHelper.InitializationDateTimeCode0001 + fileExtension;
                //                string physicalPath = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.Download, controllerName);
                //                Directory.CreateDirectory(physicalPath);
                //                string filePath = Path.Combine(physicalPath, fileName);
                //                using (var stream = new FileStream(filePath, FileMode.Create))
                //                {
                //                    file.CopyTo(stream);
                //                }
                //                FileInfo fileLocation = new FileInfo(filePath);
                //                try
                //                {
                //                    if (fileLocation.Length > 0)
                //                    {
                //                        if (fileExtension == ".xlsx" || fileExtension == ".xls")
                //                        {
                //                            using (ExcelPackage package = new ExcelPackage(fileLocation))
                //                            {
                //                                if (package.Workbook.Worksheets.Count > 0)
                //                                {
                //                                    ExcelWorksheet workSheet = package.Workbook.Worksheets[1];
                //                                    if (workSheet != null)
                //                                    {
                //                                        int totalRows = workSheet.Dimension.Rows;
                //                                        for (int j = 2; j <= totalRows; j++)
                //                                        {
                //                                            var item = new SuperResultTranfer();
                //                                            item.CHK = true;
                //                                            if (workSheet.Cells[j, 1].Value != null)
                //                                            {
                //                                                item.NO = workSheet.Cells[j, 1].Value.ToString().Trim();
                //                                            }
                //                                            if (workSheet.Cells[j, 2].Value != null)
                //                                            {
                //                                                item.Tube_Cutting_Part_No = workSheet.Cells[j, 2].Value.ToString().Trim();
                //                                            }
                //                                            if (workSheet.Cells[j, 3].Value != null)
                //                                            {
                //                                                item.Description = workSheet.Cells[j, 3].Value.ToString().Trim();
                //                                            }
                //                                            if (workSheet.Cells[j, 4].Value != null)
                //                                            {
                //                                                item.Raw_Material_Part = workSheet.Cells[j, 4].Value.ToString().Trim();
                //                                            }
                //                                            if (workSheet.Cells[j, 5].Value != null)
                //                                            {
                //                                                item.Size = workSheet.Cells[j, 5].Value.ToString().Trim();
                //                                            }
                //                                            if (workSheet.Cells[j, 6].Value != null)
                //                                            {
                //                                                item.Machine = workSheet.Cells[j, 6].Value.ToString().Trim();
                //                                            }                                                           
                //                                            if (workSheet.Cells[j, 7].Value != null)
                //                                            {
                //                                                item.Packing_unit = workSheet.Cells[j, 7].Value.ToString().Trim();
                //                                            }
                //                                            if (workSheet.Cells[j, 8].Value != null)
                //                                            {
                //                                                item.Location = workSheet.Cells[j, 8].Value.ToString().Trim();
                //                                            }                                                           
                //                                            BaseResult.DataGridView1.Add(item);
                //                                        }
                //                                    }
                //                                }
                //                            }
                //                        }
                //                    }
                //                }
                //                catch (Exception ex)
                //                {
                //                    BaseResult.Error = ex.Message;
                //                    BaseResult.ErrorNumber = 1;
                //                }
                //                fileLocation.Delete();
                //            }
                //            else
                //            {
                //                BaseResult.ErrorNumber = 1;
                //            }
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                BaseResult.Error = ex.Message;
                BaseResult.ErrorNumber = 2;
            }
            return BaseResult;
        }
    }
}

