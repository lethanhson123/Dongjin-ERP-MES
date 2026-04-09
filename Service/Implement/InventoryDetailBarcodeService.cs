namespace Service.Implement
{
    public class InventoryDetailBarcodeService : BaseService<InventoryDetailBarcode, IInventoryDetailBarcodeRepository>
    , IInventoryDetailBarcodeService
    {
        private readonly IWebHostEnvironment _WebHostEnvironment;
        private readonly IInventoryDetailBarcodeRepository _InventoryDetailBarcodeRepository;
        private readonly IInventoryRepository _InventoryRepository;
        private readonly IMaterialRepository _MaterialRepository;
        private readonly ICategoryLocationRepository _CategoryLocationRepository;
        private readonly IMembershipRepository _MembershipRepository;
        private readonly IWarehouseInputRepository _WarehouseInputRepository;
        private readonly IWarehouseInputDetailBarcodeRepository _WarehouseInputDetailBarcodeRepository;
        private readonly IWarehouseInputDetailBarcodeService _WarehouseInputDetailBarcodeService;

        public InventoryDetailBarcodeService(IInventoryDetailBarcodeRepository InventoryDetailBarcodeRepository
            , IWebHostEnvironment WebHostEnvironment
            , IInventoryRepository InventoryRepository
            , IMaterialRepository MaterialRepository
            , ICategoryLocationRepository CategoryLocationRepository
            , IMembershipRepository MembershipRepository
            , IWarehouseInputRepository WarehouseInputRepository
            , IWarehouseInputDetailBarcodeRepository WarehouseInputDetailBarcodeRepository
            , IWarehouseInputDetailBarcodeService WarehouseInputDetailBarcodeService

            ) : base(InventoryDetailBarcodeRepository)
        {

            _InventoryDetailBarcodeRepository = InventoryDetailBarcodeRepository;
            _WebHostEnvironment = WebHostEnvironment;
            _InventoryRepository = InventoryRepository;
            _MaterialRepository = MaterialRepository;
            _CategoryLocationRepository = CategoryLocationRepository;
            _MembershipRepository = MembershipRepository;

            _WarehouseInputRepository = WarehouseInputRepository;
            _WarehouseInputDetailBarcodeRepository = WarehouseInputDetailBarcodeRepository;
            _WarehouseInputDetailBarcodeService = WarehouseInputDetailBarcodeService;

        }
        public override void Initialization(InventoryDetailBarcode model)
        {

            BaseInitialization(model);
            if (model.ParentID > 0)
            {
                if (string.IsNullOrEmpty(model.ParentName))
                {
                    var Parent = _InventoryRepository.GetByID(model.ParentID.Value);
                    model.ParentName = Parent.Code;
                    model.CompanyID = Parent.CompanyID;
                }
            }

            if (!string.IsNullOrEmpty(model.CategoryLocationName))
            {
                var CategoryLocation = _CategoryLocationRepository.GetByCondition(o => o.CompanyID == model.CompanyID && o.Name == model.CategoryLocationName).FirstOrDefault();
                if (CategoryLocation == null)
                {
                    CategoryLocation = new CategoryLocation();
                    CategoryLocation.Name = model.CategoryLocationName;
                    CategoryLocation.CompanyID = model.CompanyID;
                    _CategoryLocationRepository.Add(CategoryLocation);
                }
                model.CategoryLocationID = CategoryLocation.ID;
            }
            if (model.CategoryLocationID > 0)
            {
                var CategoryLocation = _CategoryLocationRepository.GetByID(model.CategoryLocationID.Value);
                model.CategoryLocationName = CategoryLocation.Name;
            }
            if (!string.IsNullOrEmpty(model.Barcode))
            {
                if (string.IsNullOrEmpty(model.MaterialName))
                {
                    if (model.Barcode.Contains("$"))
                    {
                        model.MaterialName = model.Barcode.Split('$')[0];
                    }
                }
            }
            var Material = _MaterialRepository.GetByDescription(model.MaterialName, model.CompanyID);
            model.MaterialID = Material.ID;
            model.MaterialName = Material.Code;
            model.FileName = Material.CategoryLineName;

            model.QuantityGAP01 = model.Quantity - model.Quantity01;
            model.QuantityGAP02 = model.Quantity - model.Quantity02;
            model.QuantityGAP03 = model.Quantity - model.Quantity03;

            if (model.UpdateUserID > 0)
            {
                model.UpdateDate = DateTime.Now;
                var Membership = _MembershipRepository.GetByID(model.UpdateUserID.Value);
                model.UpdateUserCode = Membership.UserName;
                model.UpdateUserName = Membership.Name;
                if (model.Quantity01 != null)
                {
                    model.Date01 = model.Date01 ?? model.UpdateDate;
                    model.UpdateUserID01 = model.UpdateUserID01 ?? model.UpdateUserID;
                    model.UpdateUserCode01 = model.UpdateUserCode01 ?? model.UpdateUserCode;
                    model.UpdateUserName01 = model.UpdateUserName01 ?? model.UpdateUserName;
                }
                if (model.Quantity02 != null)
                {
                    model.Date02 = model.Date02 ?? model.UpdateDate;
                    model.UpdateUserID02 = model.UpdateUserID02 ?? model.UpdateUserID;
                    model.UpdateUserCode02 = model.UpdateUserCode02 ?? model.UpdateUserCode;
                    model.UpdateUserName02 = model.UpdateUserName02 ?? model.UpdateUserName;
                }
                if (model.Quantity03 != null)
                {
                    model.Date03 = model.Date03 ?? model.UpdateDate;
                    model.UpdateUserID03 = model.UpdateUserID03 ?? model.UpdateUserID;
                    model.UpdateUserCode03 = model.UpdateUserCode03 ?? model.UpdateUserCode;
                    model.UpdateUserName03 = model.UpdateUserName03 ?? model.UpdateUserName;
                }
            }
        }

        public override async Task<BaseResult<InventoryDetailBarcode>> SaveAsync(BaseParameter<InventoryDetailBarcode> BaseParameter)
        {
            var result = new BaseResult<InventoryDetailBarcode>();
            if (BaseParameter != null && BaseParameter.BaseModel != null)
            {
                if (!string.IsNullOrEmpty(BaseParameter.BaseModel.Barcode))
                {
                    BaseParameter.BaseModel.Barcode = BaseParameter.BaseModel.Barcode.Trim();
                    if (BaseParameter.BaseModel.Barcode.Contains("$$"))
                    {
                        if (BaseParameter.BaseModel.ParentID > 0)
                        {
                            var ModelCheck = await GetByCondition(o => o.ParentID == BaseParameter.BaseModel.ParentID && o.Barcode == BaseParameter.BaseModel.Barcode && o.CategoryLocationName == BaseParameter.BaseModel.CategoryLocationName).FirstOrDefaultAsync();
                            if (ModelCheck == null)
                            {
                                ModelCheck = await GetByCondition(o => o.ParentID == BaseParameter.BaseModel.ParentID && o.Barcode == BaseParameter.BaseModel.Barcode).FirstOrDefaultAsync();
                            }
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
                    }
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<InventoryDetailBarcode>> GetByCategoryDepartmentIDToListAsync(BaseParameter<InventoryDetailBarcode> BaseParameter)
        {
            var result = new BaseResult<InventoryDetailBarcode>();
            if (BaseParameter.CategoryDepartmentID > 0)
            {
                var Inventory = await _InventoryRepository.GetByCondition(o => o.SupplierID == BaseParameter.CategoryDepartmentID && o.Active == true && o.IsSync == true && o.IsComplete != true).OrderByDescending(o => o.Date).FirstOrDefaultAsync();
                if (Inventory != null && Inventory.ID > 0)
                {
                    result.List = await GetByCondition(o => o.ParentID == Inventory.ID).OrderBy(o => o.CategoryLocationName).ToListAsync();
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<InventoryDetailBarcode>> ExportToExcelAsync(BaseParameter<InventoryDetailBarcode> BaseParameter)
        {
            var result = new BaseResult<InventoryDetailBarcode>();
            if (BaseParameter.ParentID > 0)
            {


                var ListInventoryDetailBarcodeActiveFull = await GetByCondition(o => o.ParentID == BaseParameter.ParentID && o.Active == true).ToListAsync();
                var ListInventoryDetailBarcodeActive = new List<InventoryDetailBarcode>();
                var ListInventoryDetailBarcodeActiveDuplicate = new List<InventoryDetailBarcode>();
                foreach (var InventoryDetailBarcode in ListInventoryDetailBarcodeActiveFull)
                {
                    var ListInventoryDetailBarcodeSub = ListInventoryDetailBarcodeActiveFull.Where(o => o.Barcode == InventoryDetailBarcode.Barcode).ToList();
                    if (ListInventoryDetailBarcodeSub.Count > 1)
                    {
                        ListInventoryDetailBarcodeActiveDuplicate.AddRange(ListInventoryDetailBarcodeSub);
                    }
                }
                var ListInventoryDetailBarcodeActiveDuplicateID = ListInventoryDetailBarcodeActiveDuplicate.Select(o => o.ID).ToList();
                ListInventoryDetailBarcodeActive = ListInventoryDetailBarcodeActiveFull.Where(o => !ListInventoryDetailBarcodeActiveDuplicateID.Contains(o.ID)).ToList();

                var Inventory = await _InventoryRepository.GetByIDAsync(BaseParameter.ParentID.Value);
                string fileName = BaseParameter.ParentID + "-" + Inventory.Code + @"-BarcodeActive-" + GlobalHelper.InitializationDateTimeCode + ".xlsx";
                var streamExport = new MemoryStream();
                InitializationExcel(ListInventoryDetailBarcodeActive, streamExport);
                var physicalPathCreate = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.Download, fileName);
                using (var stream = new FileStream(physicalPathCreate, FileMode.Create))
                {
                    streamExport.CopyTo(stream);
                }
                result.Message = GlobalHelper.URLSite + "/" + GlobalHelper.Download + "/" + fileName;
                fileName = BaseParameter.ParentID + "-" + Inventory.Code + @"-BarcodeActiveDuplicate-" + GlobalHelper.InitializationDateTimeCode + ".xlsx";
                streamExport = new MemoryStream();
                InitializationExcel(ListInventoryDetailBarcodeActiveDuplicate, streamExport);
                physicalPathCreate = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.Download, fileName);
                using (var stream = new FileStream(physicalPathCreate, FileMode.Create))
                {
                    streamExport.CopyTo(stream);
                }
                result.Note = GlobalHelper.URLSite + "/" + GlobalHelper.Download + "/" + fileName;
            }
            return result;
        }
        private void InitializationExcel(List<InventoryDetailBarcode> list, MemoryStream streamExport)
        {
            using (var package = new ExcelPackage(streamExport))
            {
                var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                int row = 1;
                int column = 1;
                workSheet.Cells[row, column].Value = "ID";
                column = column + 1;
                workSheet.Cells[row, column].Value = "Tag";
                column = column + 1;
                workSheet.Cells[row, column].Value = "OEM";
                column = column + 1;
                workSheet.Cells[row, column].Value = "PART NO";
                column = column + 1;
                workSheet.Cells[row, column].Value = "Barcode";
                column = column + 1;
                workSheet.Cells[row, column].Value = "Location";
                column = column + 1;
                workSheet.Cells[row, column].Value = "Description";
                column = column + 1;
                workSheet.Cells[row, column].Value = "Note";
                column = column + 1;
                workSheet.Cells[row, column].Value = "ERP";
                column = column + 1;
                workSheet.Cells[row, column].Value = "Actual 01";
                column = column + 1;
                workSheet.Cells[row, column].Value = "GAP";
                column = column + 1;
                workSheet.Cells[row, column].Value = "Date";
                column = column + 1;
                workSheet.Cells[row, column].Value = "User";
                column = column + 1;
                workSheet.Cells[row, column].Value = "Actual 02";
                column = column + 1;
                workSheet.Cells[row, column].Value = "GAP";
                column = column + 1;
                workSheet.Cells[row, column].Value = "Date";
                column = column + 1;
                workSheet.Cells[row, column].Value = "User";
                column = column + 1;
                workSheet.Cells[row, column].Value = "Actual 03";
                column = column + 1;
                workSheet.Cells[row, column].Value = "GAP";
                column = column + 1;
                workSheet.Cells[row, column].Value = "Date";
                column = column + 1;
                workSheet.Cells[row, column].Value = "User";

                for (int i = 1; i <= column; i++)
                {
                    workSheet.Cells[1, i].Style.Font.Bold = true;
                    workSheet.Cells[1, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Cells[1, i].Style.Font.Name = "Times New Roman";
                    workSheet.Cells[1, i].Style.Font.Size = 11;
                    workSheet.Cells[1, i].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells[1, i].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells[1, i].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells[1, i].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                }



                row = row + 1;
                foreach (InventoryDetailBarcode item in list)
                {
                    workSheet.Cells[row, 1].Value = item.ID;
                    workSheet.Cells[row, 2].Value = item.Week;
                    workSheet.Cells[row, 3].Value = item.FileName;
                    workSheet.Cells[row, 4].Value = item.MaterialName;
                    workSheet.Cells[row, 5].Value = item.Barcode;
                    workSheet.Cells[row, 6].Value = item.CategoryLocationName;
                    workSheet.Cells[row, 7].Value = item.Description;
                    workSheet.Cells[row, 8].Value = item.Note;
                    workSheet.Cells[row, 9].Value = item.Quantity;
                    workSheet.Cells[row, 10].Value = item.Quantity01;
                    workSheet.Cells[row, 11].Value = item.QuantityGAP01;
                    workSheet.Cells[row, 12].Value = item.Date01 != null ? item.Date01.Value.ToString("dd/MM/yyyy HH:mm:ss") : "";
                    workSheet.Cells[row, 13].Value = item.UpdateUserCode01 + " | " + item.UpdateUserName01;
                    workSheet.Cells[row, 14].Value = item.Quantity02;
                    workSheet.Cells[row, 15].Value = item.QuantityGAP02;
                    workSheet.Cells[row, 16].Value = item.Date02 != null ? item.Date02.Value.ToString("dd/MM/yyyy HH:mm:ss") : "";
                    workSheet.Cells[row, 17].Value = item.UpdateUserCode02 + " | " + item.UpdateUserName02;
                    workSheet.Cells[row, 18].Value = item.Quantity03;
                    workSheet.Cells[row, 19].Value = item.QuantityGAP03;
                    workSheet.Cells[row, 20].Value = item.Date03 != null ? item.Date03.Value.ToString("dd/MM/yyyy HH:mm:ss") : "";
                    workSheet.Cells[row, 21].Value = item.UpdateUserCode03 + " | " + item.UpdateUserName03;


                    for (int i = 1; i <= column; i++)
                    {
                        workSheet.Cells[row, i].Style.Font.Name = "Times New Roman";
                        workSheet.Cells[row, i].Style.Font.Size = 11;
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

        public virtual async Task<BaseResult<InventoryDetailBarcode>> ExportWithCategoryLocationNameToExcelAsync(BaseParameter<InventoryDetailBarcode> BaseParameter)
        {
            var result = new BaseResult<InventoryDetailBarcode>();
            if (BaseParameter.ParentID > 0)
            {
                var Inventory = await _InventoryRepository.GetByIDAsync(BaseParameter.ParentID.Value);
                if (Inventory.ID > 0 && Inventory.SupplierID > 0)
                {
                    MySQLHelper.ERPSyncAsync02(GlobalHelper.ERP_MariaDBConectionString);
                    var ListInventoryDetailBarcodeUpdate = new List<InventoryDetailBarcode>();
                    var ListInventoryDetailBarcode = await GetByCondition(o => o.ParentID == BaseParameter.ParentID && o.Active == true && string.IsNullOrEmpty(o.ProductName)).OrderByDescending(o => o.DateScan).ToListAsync();
                    if (ListInventoryDetailBarcode.Count > 0)
                    {
                        var ListInventoryDetailBarcodeBarcode = ListInventoryDetailBarcode.Select(o => o.Barcode).Distinct().ToList();
                        var ListWarehouseInputDetailBarcode = await _WarehouseInputDetailBarcodeRepository.GetByCondition(o => o.CategoryDepartmentID == Inventory.SupplierID && ListInventoryDetailBarcodeBarcode.Contains(o.Barcode) && o.Active == true).ToListAsync();
                        foreach (var WarehouseInputDetailBarcode in ListWarehouseInputDetailBarcode)
                        {
                            var InventoryDetailBarcode = ListInventoryDetailBarcode.Where(o => o.Barcode == WarehouseInputDetailBarcode.Barcode).OrderByDescending(o => o.DateScan).FirstOrDefault();
                            if (InventoryDetailBarcode != null && InventoryDetailBarcode.ID > 0)
                            {
                                InventoryDetailBarcode.ProductName = WarehouseInputDetailBarcode.CategoryLocationName;
                                ListInventoryDetailBarcodeUpdate.Add(InventoryDetailBarcode);
                            }
                        }
                        await _InventoryDetailBarcodeRepository.UpdateRangeAsync(ListInventoryDetailBarcodeUpdate);
                    }
                    result.List = await GetByCondition(o => o.ParentID == BaseParameter.ParentID && o.Active == true && o.ProductName != o.CategoryLocationName).OrderByDescending(o => o.ProductName).ThenBy(o => o.Barcode).ToListAsync();
                    string fileName = BaseParameter.ParentID + "-" + Inventory.Code + @"-CategoryLocationName-" + GlobalHelper.InitializationDateTimeCode + ".xlsx";
                    var streamExport = new MemoryStream();
                    InitializationExcelWithCategoryLocationName(result.List, streamExport);
                    var physicalPathCreate = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.Download, fileName);
                    using (var stream = new FileStream(physicalPathCreate, FileMode.Create))
                    {
                        streamExport.CopyTo(stream);
                    }
                    result.Note = GlobalHelper.URLSite + "/" + GlobalHelper.Download + "/" + fileName;
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<InventoryDetailBarcode>> ExportWithQuantityToExcelAsync(BaseParameter<InventoryDetailBarcode> BaseParameter)
        {
            var result = new BaseResult<InventoryDetailBarcode>();
            if (BaseParameter.ParentID > 0)
            {
                var Inventory = await _InventoryRepository.GetByIDAsync(BaseParameter.ParentID.Value);
                if (Inventory.ID > 0 && Inventory.SupplierID > 0)
                {
                    MySQLHelper.ERPSyncAsync02(GlobalHelper.ERP_MariaDBConectionString);
                    result.List = await GetByCondition(o => o.ParentID == BaseParameter.ParentID && o.Active == true && o.Quantity != o.Quantity01).OrderBy(o => o.Barcode).ThenByDescending(o => o.QuantityGAP01).ToListAsync();

                    //var ListInventoryDetailBarcodeBarcode = result.List.Select(o => o.Barcode).Distinct().ToList();
                    //var ListWarehouseInputDetailBarcode = await _WarehouseInputDetailBarcodeRepository.GetByCondition(o => o.CategoryDepartmentID == Inventory.SupplierID && ListInventoryDetailBarcodeBarcode.Contains(o.Barcode)).ToListAsync();

                    //foreach (var InventoryDetailBarcode in result.List)
                    //{
                    //    var WarehouseInputDetailBarcode = ListWarehouseInputDetailBarcode.Where(o => o.Barcode == InventoryDetailBarcode.Barcode).FirstOrDefault();
                    //    if (WarehouseInputDetailBarcode != null && WarehouseInputDetailBarcode.ID > 0)
                    //    {
                    //        if (WarehouseInputDetailBarcode.QuantityInventory != InventoryDetailBarcode.Quantity01)
                    //        {
                    //            WarehouseInputDetailBarcode.Quantity = 0;
                    //        }
                    //    }
                    //}
                    string fileName = BaseParameter.ParentID + "-" + Inventory.Code + @"-Quantity-" + GlobalHelper.InitializationDateTimeCode + ".xlsx";
                    var streamExport = new MemoryStream();
                    InitializationExcelWithQuantity(result.List, streamExport);
                    var physicalPathCreate = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.Download, fileName);
                    using (var stream = new FileStream(physicalPathCreate, FileMode.Create))
                    {
                        streamExport.CopyTo(stream);
                    }
                    result.Note = GlobalHelper.URLSite + "/" + GlobalHelper.Download + "/" + fileName;
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<InventoryDetailBarcode>> ExportWithNotExistToExcelAsync(BaseParameter<InventoryDetailBarcode> BaseParameter)
        {
            var result = new BaseResult<InventoryDetailBarcode>();
            if (BaseParameter.ParentID > 0)
            {
                var Inventory = await _InventoryRepository.GetByIDAsync(BaseParameter.ParentID.Value);
                if (Inventory.ID > 0 && Inventory.SupplierID > 0)
                {
                    MySQLHelper.ERPSyncAsync02(GlobalHelper.ERP_MariaDBConectionString);
                    var ListWarehouseInputDetailBarcode = await _WarehouseInputDetailBarcodeRepository.GetByCondition(o => o.CategoryDepartmentID == Inventory.SupplierID && o.Active == true).ToListAsync();
                    var ListWarehouseInputDetailBarcodeBarcode = ListWarehouseInputDetailBarcode.Select(o => o.Barcode).Distinct().ToList();
                    var ListInventoryDetailBarcode = await GetByCondition(o => o.ParentID == BaseParameter.ParentID && o.Active == true && !ListWarehouseInputDetailBarcodeBarcode.Contains(o.Barcode)).OrderBy(o => o.Barcode).ThenByDescending(o => o.Quantity01).ToListAsync();
                    result.List = ListInventoryDetailBarcode;
                    string fileName = BaseParameter.ParentID + "-" + Inventory.Code + @"-NotExist-" + GlobalHelper.InitializationDateTimeCode + ".xlsx";
                    var streamExport = new MemoryStream();
                    InitializationExcelWithNotExist(result.List, streamExport);
                    var physicalPathCreate = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.Download, fileName);
                    using (var stream = new FileStream(physicalPathCreate, FileMode.Create))
                    {
                        streamExport.CopyTo(stream);
                    }
                    result.Note = GlobalHelper.URLSite + "/" + GlobalHelper.Download + "/" + fileName;
                }
            }
            return result;
        }
        private void InitializationExcelWithCategoryLocationName(List<InventoryDetailBarcode> list, MemoryStream streamExport)
        {
            using (var package = new ExcelPackage(streamExport))
            {
                var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                int row = 1;
                int column = 1;
                workSheet.Cells[row, column].Value = "ID";
                column = column + 1;
                workSheet.Cells[row, column].Value = "Barcode";
                column = column + 1;
                workSheet.Cells[row, column].Value = "ERP";
                column = column + 1;
                workSheet.Cells[row, column].Value = "Location";
                column = column + 1;
                workSheet.Cells[row, column].Value = "Date";
                column = column + 1;
                workSheet.Cells[row, column].Value = "User";

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


                row = row + 1;
                foreach (InventoryDetailBarcode item in list)
                {
                    workSheet.Cells[row, 1].Value = item.ID;
                    workSheet.Cells[row, 2].Value = item.Barcode;
                    workSheet.Cells[row, 3].Value = item.ProductName;
                    workSheet.Cells[row, 4].Value = item.CategoryLocationName;
                    workSheet.Cells[row, 5].Value = item.Date01 != null ? item.Date01.Value.ToString("dd/MM/yyyy HH:mm:ss") : "";
                    workSheet.Cells[row, 6].Value = item.UpdateUserCode01 + " | " + item.UpdateUserName01;

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
        private void InitializationExcelWithQuantity(List<InventoryDetailBarcode> list, MemoryStream streamExport)
        {
            using (var package = new ExcelPackage(streamExport))
            {
                var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                int row = 1;
                int column = 1;
                workSheet.Cells[row, column].Value = "ID";
                column = column + 1;
                workSheet.Cells[row, column].Value = "Barcode";
                column = column + 1;
                workSheet.Cells[row, column].Value = "ERP";
                column = column + 1;
                workSheet.Cells[row, column].Value = "Actual";
                column = column + 1;
                workSheet.Cells[row, column].Value = "GAP";
                column = column + 1;
                workSheet.Cells[row, column].Value = "Date";
                column = column + 1;
                workSheet.Cells[row, column].Value = "User";

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

                row = row + 1;
                foreach (InventoryDetailBarcode item in list)
                {
                    workSheet.Cells[row, 1].Value = item.ID;
                    workSheet.Cells[row, 2].Value = item.Barcode;
                    workSheet.Cells[row, 3].Value = item.Quantity;
                    workSheet.Cells[row, 4].Value = item.Quantity01;
                    workSheet.Cells[row, 5].Value = item.QuantityGAP01;
                    workSheet.Cells[row, 6].Value = item.Date01 != null ? item.Date01.Value.ToString("dd/MM/yyyy HH:mm:ss") : "";
                    workSheet.Cells[row, 7].Value = item.UpdateUserCode01 + " | " + item.UpdateUserName01;

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
        private void InitializationExcelWithNotExist(List<InventoryDetailBarcode> list, MemoryStream streamExport)
        {
            using (var package = new ExcelPackage(streamExport))
            {
                var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                int row = 1;
                int column = 1;
                workSheet.Cells[row, column].Value = "ID";
                column = column + 1;
                workSheet.Cells[row, column].Value = "Barcode";
                column = column + 1;
                workSheet.Cells[row, column].Value = "Location";
                column = column + 1;
                workSheet.Cells[row, column].Value = "Actual";
                column = column + 1;
                workSheet.Cells[row, column].Value = "Date";
                column = column + 1;
                workSheet.Cells[row, column].Value = "User";

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

                row = row + 1;
                foreach (InventoryDetailBarcode item in list)
                {
                    workSheet.Cells[row, 1].Value = item.ID;
                    workSheet.Cells[row, 2].Value = item.Barcode;
                    workSheet.Cells[row, 3].Value = item.CategoryLocationName;
                    workSheet.Cells[row, 4].Value = item.Quantity01;
                    workSheet.Cells[row, 5].Value = item.Date01 != null ? item.Date01.Value.ToString("dd/MM/yyyy HH:mm:ss") : "";
                    workSheet.Cells[row, 6].Value = item.UpdateUserCode01 + " | " + item.UpdateUserName01;

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
        public virtual async Task<BaseResult<InventoryDetailBarcode>> CreateAutoAsync(BaseParameter<InventoryDetailBarcode> BaseParameter)
        {
            var result = new BaseResult<InventoryDetailBarcode>();
            //var List = await _InventoryDetailBarcodeRepository.GetByParentIDAndActiveToListAsync(1119, true);
            //for (int i = 0; i < List.Count; i++)
            //{
            //    if (!List[i].Barcode.ToLower().Contains(List[i].MaterialName.ToLower()))
            //    {
            //        await _InventoryDetailBarcodeRepository.UpdateAsync(List[i]);
            //    }
            //    //    if (!string.IsNullOrEmpty(List[i].Barcode))
            //    //{
            //    //    if (List[i].Barcode.Contains("$"))
            //    //    {
            //    //        List[i].MaterialName = List[i].Barcode.Split('$')[0];
            //    //    }
            //    //}
            //}
            //await _InventoryDetailBarcodeRepository.UpdateRangeAsync(List);
            return result;
        }
    }
}

