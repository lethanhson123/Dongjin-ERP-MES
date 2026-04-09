namespace Service.Implement
{
    public class InventoryDetailService : BaseService<InventoryDetail, IInventoryDetailRepository>
    , IInventoryDetailService
    {
        private readonly IInventoryDetailRepository _InventoryDetailRepository;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        private readonly IInventoryDetailBarcodeRepository _InventoryDetailBarcodeRepository;
        private readonly IInventoryRepository _InventoryRepository;

        public InventoryDetailService(IInventoryDetailRepository InventoryDetailRepository
            , IWebHostEnvironment WebHostEnvironment
            , IInventoryDetailBarcodeRepository inventoryDetailBarcodeRepository
            , IInventoryRepository InventoryRepository

            ) : base(InventoryDetailRepository)
        {
            _InventoryDetailRepository = InventoryDetailRepository;
            _WebHostEnvironment = WebHostEnvironment;
            _InventoryDetailBarcodeRepository = inventoryDetailBarcodeRepository;
            _InventoryRepository = InventoryRepository;
        }
        public override void InitializationSave(InventoryDetail model)
        {
            BaseInitialization(model);
            if (model.IsExport == true)
            {

            }
            else
            {
                var ListInventoryDetailBarcode = _InventoryDetailBarcodeRepository.GetByCondition(o => o.ParentID == model.ParentID && o.Active == true && o.MaterialName == model.MaterialName && o.CategoryLocationName == model.CategoryLocationName).ToList();
                if (ListInventoryDetailBarcode.Count > 0)
                {
                    model.UpdateUserCode = ListInventoryDetailBarcode[0].UpdateUserCode01;
                    model.UpdateUserName = ListInventoryDetailBarcode[0].UpdateUserName01;
                    model.CreateDate = ListInventoryDetailBarcode[0].Date01;
                    model.MaterialName = string.Join(",", ListInventoryDetailBarcode.Select(o => o.MaterialName).Distinct().OrderBy(o => o).ToList());
                    model.CategoryLocationName = string.Join(",", ListInventoryDetailBarcode.Select(o => o.CategoryLocationName).Distinct().OrderBy(o => o).ToList());
                    model.FileName = string.Join(",", ListInventoryDetailBarcode.Select(o => o.FileName).Distinct().OrderBy(o => o).ToList());
                    model.Note = string.Join(",", ListInventoryDetailBarcode.Select(o => o.Note).Distinct().OrderBy(o => o).ToList());
                    model.Quantity = ListInventoryDetailBarcode.Sum(o => o.Quantity) ?? 0;
                    model.QuantityActual = ListInventoryDetailBarcode.Sum(o => o.Quantity01 ?? 0);
                }
                model.QuantityGAP = model.Quantity - model.QuantityActual;
                if (model.Week == null)
                {
                    if (model.ParentID > 0)
                    {
                        var List = _InventoryDetailRepository.GetByParentIDToList(model.ParentID.Value);
                        if (List.Count > 0)
                        {
                            model.Week = List.Max(o => o.Week) ?? 0;
                            model.Week = model.Week + 1;
                        }
                        else
                        {
                            var Inventory = _InventoryRepository.GetByID(model.ParentID.Value);
                            model.Week = Inventory.QuantityBegin ?? 1;
                        }
                    }
                }
            }
            model.IsExport = false;
        }

        public override async Task<BaseResult<InventoryDetail>> SaveAsync(BaseParameter<InventoryDetail> BaseParameter)
        {
            var result = new BaseResult<InventoryDetail>();
            if (BaseParameter.BaseModel != null)
            {
                InitializationSave(BaseParameter.BaseModel);
                var ModelCheck = await GetByCondition(o => o.ParentID == BaseParameter.BaseModel.ParentID && o.MaterialName == BaseParameter.BaseModel.MaterialName && o.CategoryLocationName == BaseParameter.BaseModel.CategoryLocationName).FirstOrDefaultAsync();
                SetModelByModelCheck(BaseParameter.BaseModel, ModelCheck);
                if (BaseParameter.BaseModel.ID > 0)
                {
                    result = await UpdateAsync(BaseParameter);
                }
                else
                {
                    result = await AddAsync(BaseParameter);
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<InventoryDetail>> SyncByParentIDToListAsync(BaseParameter<InventoryDetail> BaseParameter)
        {
            var result = new BaseResult<InventoryDetail>();
            result.List = new List<InventoryDetail>();
            if (BaseParameter.ParentID > 0)
            {
                var ListInventoryDetailBarcode = new List<InventoryDetailBarcode>();

                if (!string.IsNullOrEmpty(BaseParameter.SearchString))
                {
                    BaseParameter.SearchString = BaseParameter.SearchString.Trim();
                    ListInventoryDetailBarcode = await _InventoryDetailBarcodeRepository.GetByCondition(o => o.ParentID == BaseParameter.ParentID && o.Active == true && o.Week > 0 && o.Week.ToString() == BaseParameter.SearchString).ToListAsync();
                }
                else
                {
                    ListInventoryDetailBarcode = await _InventoryDetailBarcodeRepository.GetByCondition(o => o.ParentID == BaseParameter.ParentID && o.Active == true).ToListAsync();
                }
                if (ListInventoryDetailBarcode.Count > 0)
                {
                    string sql = @"DELETE from InventoryDetail WHERE ParentID=" + BaseParameter.ParentID;
                    await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.ERP_MariaDBConectionString, sql);
                    var ListInventoryDetailBarcodeWeek = ListInventoryDetailBarcode.Select(o => o.Week).Distinct().OrderBy(o => o).ToList();
                    foreach (var Week in ListInventoryDetailBarcodeWeek)
                    {
                        var ListInventoryDetailBarcodeSub = ListInventoryDetailBarcode.Where(o => o.Week == Week).OrderByDescending(o => o.Date01).ToList();
                        if (ListInventoryDetailBarcodeSub.Count > 0)
                        {
                            InventoryDetail InventoryDetail = new InventoryDetail();
                            InventoryDetail.IsExport = true;
                            InventoryDetail.Active = true;
                            InventoryDetail.ParentID = BaseParameter.ParentID;
                            InventoryDetail.UpdateUserCode = ListInventoryDetailBarcodeSub[0].UpdateUserCode01;
                            InventoryDetail.UpdateUserName = ListInventoryDetailBarcodeSub[0].UpdateUserName01;
                            InventoryDetail.CreateDate = ListInventoryDetailBarcodeSub[0].Date01;
                            var ListInventoryDetailBarcodeSubMaterialName = ListInventoryDetailBarcodeSub.Select(o => o.MaterialName).Distinct().OrderBy(o => o).ToList();
                            if (ListInventoryDetailBarcodeSubMaterialName.Count > 1)
                            {
                                InventoryDetail.Active = false;
                            }
                            InventoryDetail.MaterialName = string.Join(",", ListInventoryDetailBarcodeSub.Select(o => o.MaterialName).Distinct().OrderBy(o => o).ToList());
                            InventoryDetail.CategoryLocationName = string.Join(",", ListInventoryDetailBarcodeSub.Select(o => o.CategoryLocationName).Distinct().OrderBy(o => o).ToList());
                            InventoryDetail.FileName = string.Join(",", ListInventoryDetailBarcodeSub.Select(o => o.FileName).Distinct().OrderBy(o => o).ToList());
                            InventoryDetail.Note = string.Join(",", ListInventoryDetailBarcodeSub.Select(o => o.Note).Distinct().OrderBy(o => o).ToList());
                            InventoryDetail.Week = Week;
                            InventoryDetail.Quantity = ListInventoryDetailBarcodeSub.Sum(o => o.Quantity) ?? 0;
                            InventoryDetail.QuantityActual = ListInventoryDetailBarcodeSub.Sum(o => o.Quantity01 ?? 0);
                            InventoryDetail.QuantityGAP = InventoryDetail.Quantity - InventoryDetail.QuantityActual;
                            result.List.Add(InventoryDetail);
                            //BaseParameter<InventoryDetail> BaseParameterInventoryDetail = new BaseParameter<InventoryDetail>();
                            //BaseParameterInventoryDetail.BaseModel = InventoryDetail;
                            //await SaveAsync(BaseParameterInventoryDetail);
                        }
                    }
                    await _InventoryDetailRepository.AddRangeAsync(result.List);
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<InventoryDetail>> SyncByParentIDCategoryLocationNameToListAsync(BaseParameter<InventoryDetail> BaseParameter)
        {
            var result = new BaseResult<InventoryDetail>();
            result.List = new List<InventoryDetail>();
            if (BaseParameter.ParentID > 0)
            {
                var ListInventoryDetailBarcode = new List<InventoryDetailBarcode>();

                if (!string.IsNullOrEmpty(BaseParameter.SearchString))
                {
                    BaseParameter.SearchString = BaseParameter.SearchString.Trim();
                    ListInventoryDetailBarcode = await _InventoryDetailBarcodeRepository.GetByCondition(o => o.ParentID == BaseParameter.ParentID && o.Active == true && o.Week > 0 && o.Week.ToString() == BaseParameter.SearchString).ToListAsync();
                }
                else
                {
                    ListInventoryDetailBarcode = await _InventoryDetailBarcodeRepository.GetByCondition(o => o.ParentID == BaseParameter.ParentID && o.Active == true).ToListAsync();
                }
                if (ListInventoryDetailBarcode.Count > 0)
                {
                    string sql = @"DELETE from InventoryDetail WHERE ParentID=" + BaseParameter.ParentID;
                    await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.ERP_MariaDBConectionString, sql);
                    var ListInventoryDetailBarcodeCategoryLocationName = ListInventoryDetailBarcode.Select(o => o.CategoryLocationName).Distinct().OrderBy(o => o).ToList();
                    var ListInventoryDetailBarcodeMaterialName = ListInventoryDetailBarcode.Select(o => o.MaterialName).Distinct().OrderBy(o => o).ToList();
                    foreach (var CategoryLocationName in ListInventoryDetailBarcodeCategoryLocationName)
                    {
                        foreach (var MaterialName in ListInventoryDetailBarcodeMaterialName)
                        {
                            var ListInventoryDetailBarcodeSub = ListInventoryDetailBarcode.Where(o => o.CategoryLocationName == CategoryLocationName && o.MaterialName == MaterialName).OrderByDescending(o => o.Date01).ToList();
                            if (ListInventoryDetailBarcodeSub.Count > 0)
                            {
                                InventoryDetail InventoryDetail = new InventoryDetail();
                                InventoryDetail.IsExport = true;
                                InventoryDetail.Active = true;
                                InventoryDetail.ParentID = BaseParameter.ParentID;
                                InventoryDetail.UpdateUserCode = ListInventoryDetailBarcodeSub[0].UpdateUserCode01;
                                InventoryDetail.UpdateUserName = ListInventoryDetailBarcodeSub[0].UpdateUserName01;
                                InventoryDetail.CreateDate = ListInventoryDetailBarcodeSub[0].Date01;
                                var ListInventoryDetailBarcodeSubMaterialName = ListInventoryDetailBarcodeSub.Select(o => o.MaterialName).Distinct().OrderBy(o => o).ToList();
                                if (ListInventoryDetailBarcodeSubMaterialName.Count > 1)
                                {
                                    InventoryDetail.Active = false;
                                }
                                InventoryDetail.MaterialName = string.Join(",", ListInventoryDetailBarcodeSub.Select(o => o.MaterialName).Distinct().OrderBy(o => o).ToList());
                                InventoryDetail.CategoryLocationName = string.Join(",", ListInventoryDetailBarcodeSub.Select(o => o.CategoryLocationName).Distinct().OrderBy(o => o).ToList());
                                InventoryDetail.FileName = string.Join(",", ListInventoryDetailBarcodeSub.Select(o => o.FileName).Distinct().OrderBy(o => o).ToList());
                                InventoryDetail.Note = string.Join(",", ListInventoryDetailBarcodeSub.Select(o => o.Note).Distinct().OrderBy(o => o).ToList());
                                InventoryDetail.Week = 0;
                                InventoryDetail.Display = string.Join(",", ListInventoryDetailBarcodeSub.Select(o => o.Week).Distinct().OrderBy(o => o).ToList());
                                InventoryDetail.Quantity = ListInventoryDetailBarcodeSub.Sum(o => o.Quantity) ?? 0;
                                InventoryDetail.QuantityActual = ListInventoryDetailBarcodeSub.Sum(o => o.Quantity01 ?? 0);
                                InventoryDetail.QuantityGAP = InventoryDetail.Quantity - InventoryDetail.QuantityActual;
                                result.List.Add(InventoryDetail);
                                //BaseParameter<InventoryDetail> BaseParameterInventoryDetail = new BaseParameter<InventoryDetail>();
                                //BaseParameterInventoryDetail.BaseModel = InventoryDetail;
                                //await SaveAsync(BaseParameterInventoryDetail);
                            }
                        }
                    }
                    await _InventoryDetailRepository.AddRangeAsync(result.List);
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<InventoryDetail>> PrintByIDAsync(BaseParameter<InventoryDetail> BaseParameter)
        {
            var result = new BaseResult<InventoryDetail>();
            string SheetName = this.GetType().Name;
            if (BaseParameter != null)
            {
                if (BaseParameter.ID > 0)
                {
                    StringBuilder HTMLContent = new StringBuilder();
                    var ListSub = await _InventoryDetailRepository.GetByIDToListAsync(BaseParameter.ID);
                    result.Message = await PrintInventoryByListAsync(ListSub);
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<InventoryDetail>> Print2025ByIDAsync(BaseParameter<InventoryDetail> BaseParameter)
        {
            var result = new BaseResult<InventoryDetail>();
            string SheetName = this.GetType().Name;
            if (BaseParameter != null)
            {
                if (BaseParameter.ID > 0)
                {
                    StringBuilder HTMLContent = new StringBuilder();
                    var ListSub = await _InventoryDetailRepository.GetByIDToListAsync(BaseParameter.ID);
                    result.Message = await PrintInventory2025ByListAsync(ListSub);
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<InventoryDetail>> PrintByListAsync(BaseParameter<InventoryDetail> BaseParameter)
        {
            var result = new BaseResult<InventoryDetail>();
            string SheetName = this.GetType().Name;
            if (BaseParameter != null)
            {
                if (BaseParameter.List != null && BaseParameter.List.Count > 0)
                {
                    var ListSub = BaseParameter.List.OrderBy(o => o.MaterialName).ToList();
                    result.Message = await PrintInventoryByListAsync(ListSub);
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<InventoryDetail>> Print2025ByListAsync(BaseParameter<InventoryDetail> BaseParameter)
        {
            var result = new BaseResult<InventoryDetail>();
            string SheetName = this.GetType().Name;
            if (BaseParameter != null)
            {
                if (BaseParameter.List != null && BaseParameter.List.Count > 0)
                {
                    var ListSub = BaseParameter.List.OrderBy(o => o.MaterialName).ToList();
                    result.Message = await PrintInventory2025ByListAsync(ListSub);
                }
            }
            return result;
        }

        private async Task<string> PrintInventoryByListAsync(List<InventoryDetail> List)
        {
            string result = GlobalHelper.InitializationString;
            string SheetName = this.GetType().Name;
            if (List.Count > 0)
            {
                string PageTile = GlobalHelper.InitializationString;
                StringBuilder HTMLContent = new StringBuilder();
                foreach (var item in List)
                {
                    var MaterialName = item.MaterialName ?? GlobalHelper.InitializationString;
                    var Quantity = item.Quantity ?? GlobalHelper.InitializationNumber;
                    if (Quantity == 0)
                    {
                        Quantity = item.QuantityActual ?? GlobalHelper.InitializationNumber;
                    }
                    var CategoryLocationName = item.CategoryLocationName ?? GlobalHelper.InitializationString;
                    var CreateUserCode = item.UpdateUserCode ?? GlobalHelper.InitializationString;
                    var CreateUserName = item.UpdateUserName ?? GlobalHelper.InitializationString;
                    var SeriNumber = item.Week ?? GlobalHelper.InitializationNumber;
                    var CreateDate = GlobalHelper.InitializationDateTime;
                    PageTile = PageTile + "-" + SeriNumber;
                    HTMLContent.AppendLine(GlobalHelper.CreateHTMLInventory(_WebHostEnvironment.WebRootPath, MaterialName, CategoryLocationName, Quantity.ToString("N0"), CreateUserCode, CreateUserName, SeriNumber.ToString("N0"), CreateDate));
                }
                string HTMLEmpty = GlobalHelper.InitializationString;
                string physicalPathOpen = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.HTML, "Empty.html");
                using (FileStream fs = new FileStream(physicalPathOpen, FileMode.Open))
                {
                    using (StreamReader r = new StreamReader(fs, Encoding.UTF8))
                    {
                        HTMLEmpty = r.ReadToEnd();
                    }
                }
                HTMLEmpty = HTMLEmpty.Replace(@"[Content]", HTMLContent.ToString());
                HTMLEmpty = HTMLEmpty.Replace(@"[PageTile]", PageTile);

                string fileName = SheetName + "_" + GlobalHelper.InitializationDateTimeCode + ".html";
                string physicalPathCreate = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.Download, SheetName);
                Directory.CreateDirectory(physicalPathCreate);
                GlobalHelper.DeleteFilesByPath(physicalPathCreate);
                string filePath = Path.Combine(physicalPathCreate, fileName);
                using (FileStream fs = new FileStream(filePath, FileMode.Create))
                {
                    using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                    {
                        await w.WriteLineAsync(HTMLEmpty);
                    }
                }
                result = GlobalHelper.URLSite + "/" + GlobalHelper.Download + "/" + SheetName + "/" + fileName;
            }
            return result;
        }
        private async Task<string> PrintInventory2025ByListAsync(List<InventoryDetail> List)
        {
            string result = GlobalHelper.InitializationString;
            string SheetName = this.GetType().Name;
            if (List.Count > 0)
            {
                string PageTile = GlobalHelper.InitializationString;
                StringBuilder HTMLContent = new StringBuilder();
                foreach (var item in List)
                {
                    var MaterialName = item.MaterialName ?? GlobalHelper.InitializationString;
                    var Quantity = item.Quantity ?? GlobalHelper.InitializationNumber;
                    if (Quantity == 0)
                    {
                        Quantity = item.QuantityActual ?? GlobalHelper.InitializationNumber;
                    }
                    var CategoryLocationName = item.CategoryLocationName ?? GlobalHelper.InitializationString;
                    var CreateUserCode = item.UpdateUserCode ?? GlobalHelper.InitializationString;
                    var CreateUserName = item.UpdateUserName ?? GlobalHelper.InitializationString;
                    var SeriNumber = item.Week ?? GlobalHelper.InitializationNumber;
                    var CreateDate = GlobalHelper.InitializationDateTime;
                    PageTile = PageTile + "-" + SeriNumber;
                    HTMLContent.AppendLine(GlobalHelper.CreateHTMLInventory2025(_WebHostEnvironment.WebRootPath, MaterialName, CategoryLocationName, Quantity.ToString("N0"), CreateUserCode, CreateUserName, SeriNumber.ToString("N0"), CreateDate));
                }
                string HTMLEmpty = GlobalHelper.InitializationString;
                string physicalPathOpen = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.HTML, "Empty.html");
                using (FileStream fs = new FileStream(physicalPathOpen, FileMode.Open))
                {
                    using (StreamReader r = new StreamReader(fs, Encoding.UTF8))
                    {
                        HTMLEmpty = r.ReadToEnd();
                    }
                }
                HTMLEmpty = HTMLEmpty.Replace(@"[Content]", HTMLContent.ToString());
                HTMLEmpty = HTMLEmpty.Replace(@"[PageTile]", PageTile);

                string fileName = SheetName + "_" + GlobalHelper.InitializationDateTimeCode + ".html";
                string physicalPathCreate = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.Download, SheetName);
                Directory.CreateDirectory(physicalPathCreate);
                GlobalHelper.DeleteFilesByPath(physicalPathCreate);
                string filePath = Path.Combine(physicalPathCreate, fileName);
                using (FileStream fs = new FileStream(filePath, FileMode.Create))
                {
                    using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                    {
                        await w.WriteLineAsync(HTMLEmpty);
                    }
                }
                result = GlobalHelper.URLSite + "/" + GlobalHelper.Download + "/" + SheetName + "/" + fileName;
            }
            return result;
        }

        public virtual async Task<BaseResult<InventoryDetail>> ExportToExcelAsync(BaseParameter<InventoryDetail> BaseParameter)
        {
            var result = new BaseResult<InventoryDetail>();
            if (BaseParameter.ParentID > 0)
            {
                var ListInventoryDetail = await GetByCondition(o => o.ParentID == BaseParameter.ParentID && o.Active == true).ToListAsync();

                var Inventory = await _InventoryRepository.GetByIDAsync(BaseParameter.ParentID.Value);
                string fileName = BaseParameter.ParentID + "-" + Inventory.Code + @"-DetailActive-" + GlobalHelper.InitializationDateTimeCode + ".xlsx";
                var streamExport = new MemoryStream();
                InitializationExcel(ListInventoryDetail, streamExport);
                var physicalPathCreate = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.Download, fileName);
                using (var stream = new FileStream(physicalPathCreate, FileMode.Create))
                {
                    streamExport.CopyTo(stream);
                }
                result.Message = GlobalHelper.URLSite + "/" + GlobalHelper.Download + "/" + fileName;
            }
            return result;
        }
        private void InitializationExcel(List<InventoryDetail> list, MemoryStream streamExport)
        {
            using (var package = new ExcelPackage(streamExport))
            {
                var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                int row = 1;
                int column = 1;
                workSheet.Cells[row, column].Value = "No";
                column = column + 1;
                workSheet.Cells[row, column].Value = "ID";
                column = column + 1;
                workSheet.Cells[row, column].Value = "Tag";
                column = column + 1;
                workSheet.Cells[row, column].Value = "OEM";
                column = column + 1;
                workSheet.Cells[row, column].Value = "PART NO";
                column = column + 1;
                workSheet.Cells[row, column].Value = "Location";
                column = column + 1;
                workSheet.Cells[row, column].Value = "Description";
                column = column + 1;
                workSheet.Cells[row, column].Value = "Note";
                column = column + 1;
                workSheet.Cells[row, column].Value = "ERP";
                column = column + 1;
                workSheet.Cells[row, column].Value = "Actual";
                column = column + 1;
                workSheet.Cells[row, column].Value = "GAP";
                column = column + 1;
                workSheet.Cells[row, column].Value = "Tag List";

                for (int i = 1; i <= column; i++)
                {
                    workSheet.Cells[1, i].Style.Font.Bold = true;
                    workSheet.Cells[1, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Cells[1, i].Style.Font.Name = "Times New Roman";
                    workSheet.Cells[1, i].Style.Font.Size = 16;
                    workSheet.Cells[1, i].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells[1, i].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells[1, i].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells[1, i].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                }

                //column = column + 1;

                row = row + 1;
                int no = 1;
                foreach (InventoryDetail item in list)
                {
                    workSheet.Cells[row, 1].Value = no;
                    workSheet.Cells[row, 2].Value = item.ID;
                    workSheet.Cells[row, 3].Value = item.Week;
                    workSheet.Cells[row, 4].Value = item.FileName;
                    workSheet.Cells[row, 5].Value = item.MaterialName;
                    workSheet.Cells[row, 6].Value = item.CategoryLocationName;
                    workSheet.Cells[row, 7].Value = item.Description;
                    workSheet.Cells[row, 8].Value = item.Note;
                    workSheet.Cells[row, 9].Value = item.Quantity;
                    workSheet.Cells[row, 10].Value = item.QuantityActual;
                    workSheet.Cells[row, 11].Value = item.QuantityGAP;
                    workSheet.Cells[row, 12].Value = item.Display;

                    no= no + 1;

                    for (int i = 1; i <= column; i++)
                    {
                        workSheet.Cells[row, i].Style.Font.Name = "Times New Roman";
                        workSheet.Cells[row, i].Style.Font.Size = 16;
                        workSheet.Cells[row, i].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        workSheet.Cells[row, i].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        workSheet.Cells[row, i].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        workSheet.Cells[row, i].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    }
                    row = row + 1;
                }
                for (int i = 1; i <= column; i++)
                {
                    workSheet.Column(i).AutoFit();
                }
                package.Save();
            }
            streamExport.Position = 0;

        }
    }
}

