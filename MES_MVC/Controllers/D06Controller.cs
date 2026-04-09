namespace MES.Controllers
{
    public class D06Controller : Controller
    {
        private readonly ID06Service _D06Service;
        public D06Controller(ID06Service D06Service)
        {
            _D06Service = D06Service;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Buttonfind_Click()
        {
            try
            {
                var baseParameter = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                var result = await _D06Service.Buttonfind_Click(baseParameter);

                return Json(new
                {
                    draw = Request.Form["draw"],
                    recordsTotal = result.TotalCount,
                    recordsFiltered = result.TotalCount,
                    data = result.DataGridView8
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    draw = Request.Form["draw"],
                    recordsTotal = 0,
                    recordsFiltered = 0,
                    data = new List<object>(),
                    error = ex.Message
                });
            }
        }


        [HttpPost]
        public async Task<BaseResult> Buttonadd_Click()
        {
            BaseResult BaseResult = new BaseResult();
            try
            {
                BaseParameter BaseParameter = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                BaseResult = await _D06Service.Buttonadd_Click(BaseParameter);
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
                BaseResult = await _D06Service.Buttonsave_Click(BaseParameter);
            }
            catch (Exception ex)
            {
                BaseResult.Error = ex.Message;
            }
            return BaseResult;
        }
        [HttpPost]
        public async Task<BaseResult> Buttondelete_Click()
        {
            BaseResult BaseResult = new BaseResult();
            try
            {
                BaseParameter BaseParameter = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                BaseResult = await _D06Service.Buttondelete_Click(BaseParameter);
            }
            catch (Exception ex)
            {
                BaseResult.Error = ex.Message;
            }
            return BaseResult;
        }
        [HttpPost]
        public async Task<BaseResult> Buttoncancel_Click()
        {
            BaseResult BaseResult = new BaseResult();
            try
            {
                BaseParameter BaseParameter = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                BaseResult = await _D06Service.Buttoncancel_Click(BaseParameter);
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
            try
            {
                BaseParameter BaseParameter = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                BaseResult = await _D06Service.Buttoninport_Click(BaseParameter);
            }
            catch (Exception ex)
            {
                BaseResult.Error = ex.Message;
            }
            return BaseResult;
        }
        [HttpPost]
        public async Task<IActionResult> Buttonexport_Click()
        {
            try
            {
                var form = Request.Form["BaseParameter"];
                BaseParameter baseParameter = JsonConvert.DeserializeObject<BaseParameter>(form);

                // Lấy dữ liệu từ service
                var result = await _D06Service.Buttonexport_Click(baseParameter);
                var dataList = result.DataGridView8;

                if (dataList == null || !dataList.Any())
                    return BadRequest("No data to export");

                // Tạo file Excel bằng EPPlus
                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("ShippingHistory");

                    // Header
                    worksheet.Cells[1, 1].Value = "Code";
                    worksheet.Cells[1, 2].Value = "Shipping Code";
                    worksheet.Cells[1, 3].Value = "Pallet NO";
                    worksheet.Cells[1, 4].Value = "CM Pallet NO";
                    worksheet.Cells[1, 5].Value = "PART NO";
                    worksheet.Cells[1, 6].Value = "Group";
                    worksheet.Cells[1, 7].Value = "PART Name";
                    worksheet.Cells[1, 8].Value = "SNP";
                    worksheet.Cells[1, 9].Value = "Packing Lot";
                    worksheet.Cells[1, 10].Value = "PO_QTY";
                    worksheet.Cells[1, 11].Value = "QTY";
                    worksheet.Cells[1, 12].Value = "BOX_QTY";
                    worksheet.Cells[1, 13].Value = "Not yet packing";
                    worksheet.Cells[1, 14].Value = "Inventory";
                    worksheet.Cells[1, 15].Value = "Packing Time";
                    worksheet.Cells[1, 16].Value = "Packing By";
                    worksheet.Cells[1, 17].Value = "Shipped Time";
                    worksheet.Cells[1, 18].Value = "Shipped By";

                    // Dữ liệu
                    for (int i = 0; i < dataList.Count; i++)
                    {
                        var item = dataList[i];
                        worksheet.Cells[i + 2, 1].Value = item.CODE;
                        worksheet.Cells[i + 2, 2].Value = item.PO_CODE;
                        worksheet.Cells[i + 2, 3].Value = item.PLET_NO;
                        worksheet.Cells[i + 2, 4].Value = item.CM_PALLET_NO;
                        worksheet.Cells[i + 2, 5].Value = item.PART_NO;
                        worksheet.Cells[i + 2, 6].Value = item.PART_GRP;
                        worksheet.Cells[i + 2, 7].Value = item.PART_NM;
                        worksheet.Cells[i + 2, 8].Value = item.PART_SNP;
                        worksheet.Cells[i + 2, 9].Value = item.VLID_GRP;
                        worksheet.Cells[i + 2, 10].Value = item.PO_QTY;
                        worksheet.Cells[i + 2, 11].Value = item.QTY;
                        worksheet.Cells[i + 2, 12].Value = item.BOX_QTY;
                        worksheet.Cells[i + 2, 13].Value = item.Not_yet_packing;
                        worksheet.Cells[i + 2, 14].Value = item.Inventory;
                        worksheet.Cells[i + 2, 15].Value = item.CREATE_DTM?.ToString("yyyy-MM-dd HH:mm:ss");
                        worksheet.Cells[i + 2, 16].Value = item.CREATE_USER;
                        worksheet.Cells[i + 2, 17].Value = item.UPDATE_DTM?.ToString("yyyy-MM-dd HH:mm:ss");
                        worksheet.Cells[i + 2, 18].Value = item.UPDATE_USER;
                    }

                    var stream = new MemoryStream();
                    package.SaveAs(stream);
                    stream.Position = 0;

                    return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "D06_Shipping_History.xlsx");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Export failed: {ex.Message}");
            }
        }



        [HttpPost]
        public async Task<BaseResult> Buttonprint_Click()
        {
            BaseResult BaseResult = new BaseResult();
            try
            {
                BaseParameter BaseParameter = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                BaseResult = await _D06Service.Buttonprint_Click(BaseParameter);
            }
            catch (Exception ex)
            {
                BaseResult.Error = ex.Message;
            }
            return BaseResult;
        }
        [HttpPost]
        public async Task<BaseResult> Buttonhelp_Click()
        {
            BaseResult BaseResult = new BaseResult();
            try
            {
                BaseParameter BaseParameter = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                BaseResult = await _D06Service.Buttonhelp_Click(BaseParameter);
            }
            catch (Exception ex)
            {
                BaseResult.Error = ex.Message;
            }
            return BaseResult;
        }
        [HttpPost]
        public async Task<BaseResult> Buttonclose_Click()
        {
            BaseResult BaseResult = new BaseResult();
            try
            {
                BaseParameter BaseParameter = JsonConvert.DeserializeObject<BaseParameter>(Request.Form["BaseParameter"]);
                BaseResult = await _D06Service.Buttonclose_Click(BaseParameter);
            }
            catch (Exception ex)
            {
                BaseResult.Error = ex.Message;
            }
            return BaseResult;
        }
    }
}

