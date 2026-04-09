namespace MESService.Implement
{
    public class B09Service : BaseService<torderlist, ItorderlistRepository>
    , IB09Service
    {
        private readonly ItorderlistRepository _torderlistRepository;
        private readonly ICategoryDepartmentRepository _CategoryDepartmentRepository;
        private readonly IMaterialRepository _MaterialRepository;
        private readonly IMaterialConvertRepository _MaterialConvertRepository;
        private readonly IWarehouseInventoryRepository _WarehouseInventoryRepository;
        private readonly IWarehouseRequestDetailService _WarehouseRequestDetailService;
        private readonly IWarehouseRequestDetailRepository _WarehouseRequestDetailRepository;
        private readonly IWarehouseRequestService _WarehouseRequestService;
        private readonly IWarehouseInputRepository _WarehouseInputRepository;
        private readonly IWarehouseInputService _WarehouseInputService;
        private readonly IWarehouseInputDetailRepository _WarehouseInputDetailRepository;
        private readonly IWarehouseInputDetailBarcodeRepository _WarehouseInputDetailBarcodeRepository;
        private readonly IWarehouseOutputService _WarehouseOutputService;
        private readonly IWarehouseOutputDetailBarcodeService _WarehouseOutputDetailBarcodeService;
        private readonly IWarehouseOutputDetailRepository _WarehouseOutputDetailRepository;
        private readonly IMembershipRepository _MembershipRepository;
        public B09Service(ItorderlistRepository torderlistRepository
            , ICategoryDepartmentRepository CategoryDepartmentRepository
            , IMaterialRepository materialRepository
            , IMaterialConvertRepository materialConvertRepository
            , IWarehouseInventoryRepository warehouseInventoryRepository
            , IWarehouseRequestDetailService WarehouseRequestDetailService
            , IWarehouseRequestDetailRepository WarehouseRequestDetailRepository
            , IWarehouseRequestService warehouseRequestService
            , IWarehouseInputRepository WarehouseInputRepository
            , IWarehouseInputDetailBarcodeRepository WarehouseInputDetailBarcodeRepository
            , IWarehouseOutputService WarehouseOutputService
            , IWarehouseOutputDetailRepository WarehouseOutputDetailRepository
            , IWarehouseInputService WarehouseInputService
            , IWarehouseOutputDetailBarcodeService WarehouseOutputDetailBarcodeService
            , IWarehouseInputDetailRepository WarehouseInputDetailRepository
            , IMembershipRepository MembershipRepository

        ) : base(torderlistRepository)
        {
            _torderlistRepository = torderlistRepository;
            _CategoryDepartmentRepository = CategoryDepartmentRepository;
            _MaterialRepository = materialRepository;
            _MaterialConvertRepository = materialConvertRepository;
            _WarehouseInventoryRepository = warehouseInventoryRepository;
            _WarehouseRequestDetailService = WarehouseRequestDetailService;
            _WarehouseRequestDetailRepository = WarehouseRequestDetailRepository;
            _WarehouseRequestService = warehouseRequestService;
            _WarehouseInputRepository = WarehouseInputRepository;
            _WarehouseInputDetailBarcodeRepository = WarehouseInputDetailBarcodeRepository;
            _WarehouseOutputService = WarehouseOutputService;
            _WarehouseOutputDetailBarcodeService = WarehouseOutputDetailBarcodeService;
            _WarehouseOutputDetailRepository = WarehouseOutputDetailRepository;
            _WarehouseInputService = WarehouseInputService;
            _WarehouseInputDetailRepository = WarehouseInputDetailRepository;
            _MembershipRepository = MembershipRepository;
        }
        public override void Initialization(torderlist model)
        {
            BaseInitialization(model);
        }
        public virtual async Task<BaseResult> Load()
        {
            BaseResult result = new BaseResult();
            try
            {
                await Task.Run(() => { });
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        public virtual async Task<BaseResult> Buttonfind_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    if (BaseParameter.Action == 1)
                    {
                        if (BaseParameter.ListSearchString != null)
                        {
                            string TextBox1 = BaseParameter.ListSearchString[0];
                            string TextBox2 = BaseParameter.ListSearchString[1];

                            if (BaseParameter.CheckBox1 == true)
                            {
                                string sql = @"SELECT 0 AS 'QTY', 'ORDER' AS 'ORDER', `TTC_PART_IDX` AS `PART_IDX`,'TUBE' AS `TMMTIN_CODE`,  `TC_PART_NM` AS `PART_NO`, `TC_DESC` AS `PART_NM`, `TC_SIZE` AS `PART_FML`, IFNULL(`TC_PACKUNIT`, 0) AS `PART_SNP`, 0  AS `STOCK`  FROM TTC_PART
                                WHERE `TC_PART_NM` LIKE '%" + TextBox1 + "%' AND (`TC_DESC` LIKE '%" + TextBox2 + "%' OR `TC_SIZE` LIKE '%" + TextBox2 + "%')";

                                sql = sql + " LIMIT " + GlobalHelper.ListCount;

                                DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                                result.DataGridView1 = new List<SuperResultTranfer>();
                                for (int i = 0; i < ds.Tables.Count; i++)
                                {
                                    DataTable dt = ds.Tables[i];
                                    result.DataGridView1.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                                }
                            }
                            else
                            {
                                string sql = @"SELECT 0 AS 'QTY', 'ORDER' AS 'ORDER', `PART_IDX`, 'Material' AS `TMMTIN_CODE`, `PART_NO`, `PART_NM`, `PART_FML`, IFNULL(`PART_SNP`, 0) AS `PART_SNP`,  
                                IFNULL((SELECT tiivtr.QTY FROM tiivtr WHERE tiivtr.PART_IDX = tspart.`PART_IDX` AND tiivtr.LOC_IDX='1'), 0) AS `STOCK`
                                FROM tspart 
                                WHERE `PART_SCN` = '5' AND `PART_USENY` = 'Y' 
                                AND `PART_NO` LIKE '%" + TextBox1 + "%' AND (`PART_NM` LIKE '%" + TextBox2 + "%' OR `PART_FML` LIKE '%" + TextBox2 + "%')";

                                sql = sql + " LIMIT " + GlobalHelper.ListCount;

                                DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                                result.DataGridView1 = new List<SuperResultTranfer>();
                                for (int i = 0; i < ds.Tables.Count; i++)
                                {
                                    DataTable dt = ds.Tables[i];
                                    result.DataGridView1.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                                }
                            }
                        }
                    }
                    if (BaseParameter.Action == 2)
                    {
                        var CB_DATE = "";
                        var ComboBox2 = BaseParameter.ListSearchString[0];
                        if (ComboBox2 == "ALL")
                        {
                            CB_DATE = "";
                        }
                        if (ComboBox2 == "Y")
                        {
                            CB_DATE = " AND  `TMMTIN_DSCN_YN` = 'Y'";
                        }
                        if (ComboBox2 == "N")
                        {
                            CB_DATE = " AND  `TMMTIN_DSCN_YN` = 'N'";
                        }
                        var CB_DATE_1 = "";
                        var ComboBox3 = BaseParameter.ListSearchString[1];
                        if (ComboBox3 == "ALL")
                        {
                            CB_DATE_1 = "";
                        }
                        if (ComboBox3 == "Material")
                        {
                            CB_DATE_1 = " AND  `TMMTIN_CODE` = 'Material'  ";
                        }
                        if (ComboBox3 == "TUBE")
                        {
                            CB_DATE_1 = " AND  `TMMTIN_CODE` = 'TUBE'  ";
                        }

                        var T2_S1 = BaseParameter.ListSearchString[2];
                        var T2_S2 = BaseParameter.ListSearchString[3];
                        var T2_S3 = BaseParameter.ListSearchString[4];
                        var T2_S4 = BaseParameter.ListSearchString[5];
                        T2_S2 = DateTime.Parse(T2_S2).ToString("yyyy-MM-dd");

                        string sql = @"SELECT 
                            `TMMTIN_CNF_YN`, `TMMTIN_DSCN_YN`, 
                            IF(`TMMTIN_CODE` = 'TUBE', (SELECT `TC_PART_NM` FROM TTC_PART WHERE `TTC_PART_IDX` = `TMMTIN_PART`), (SELECT `PART_NO` FROM tspart WHERE `PART_IDX` = `TMMTIN_PART`)) AS `PART_NO`,
                            `TMMTIN_CODE`, `TMMTIN_PART_SNP` AS `SNP`,
                            `TMMTIN_QTY` AS `QTY`, 
                            IF(`TMMTIN_CODE` = 'TUBE', (SELECT `TC_DESC` FROM TTC_PART WHERE `TTC_PART_IDX` = `TMMTIN_PART`), (SELECT `PART_NM` FROM tspart WHERE `PART_IDX` = `TMMTIN_PART`)) AS `PART_NAME`,
                            `CREATE_DTM`, 
                            IF(`TMMTIN_CODE` = 'TUBE', (SELECT `TC_SIZE` FROM TTC_PART WHERE `TTC_PART_IDX` = `TMMTIN_PART`), (SELECT `PART_FML` FROM tspart WHERE `PART_IDX` = `TMMTIN_PART`)) AS `FAMILY`,
                            `TMMTIN_DATE` AS `DATE`, 
                            `TMMTIN_DMM_IDX` AS `CODE`, (SELECT `CD_NM_EN` FROM TSCODE WHERE `CD_IDX` = `TMMTIN_DMM_STGC` AND `CDGR_IDX` = '17') AS `STAGE`, 
                            `TMMTIN_PART` AS `DJG_CODE`

                            FROM TMMTIN_DMM 

                            WHERE `TMMTIN_REC_YN` = 'Y' AND `TMMTIN_DATE` = '" + T2_S2 + "' " + CB_DATE + CB_DATE_1 + "HAVING `STAGE` LIKE '%" + T2_S1 + "%' AND `PART_NO` LIKE '%" + T2_S3 + "%' AND (`PART_NAME` LIKE '%" + T2_S4 + "%' OR `FAMILY` LIKE '%" + T2_S4 + "%') ORDER BY `CREATE_DTM` DESC";

                        DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                        result.T2_DGV1 = new List<SuperResultTranfer>();
                        for (int i = 0; i < ds.Tables.Count; i++)
                        {
                            DataTable dt = ds.Tables[i];
                            result.T2_DGV1.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        public virtual async Task<BaseResult> Buttonadd_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                await Task.Run(() => { });
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        public virtual async Task<BaseResult> Buttonsave_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    string USER_IDX = BaseParameter.USER_IDX;
                    if (BaseParameter.Action == 1)
                    {
                        if (BaseParameter.DataGridView2 != null)
                        {
                            if (BaseParameter.DataGridView2.Count > 0)
                            {
                                string VAL_DT = "";
                                string VAL_SUM = "";
                                for (int i = 0; i < BaseParameter.DataGridView2.Count; i++)
                                {
                                    var AAA = BaseParameter.DataGridView2[i].STAGE;
                                    var BBB = BaseParameter.DataGridView2[i].DATE.Value.ToString("yyyy-MM-dd");
                                    var CCC = BaseParameter.DataGridView2[i].DJG_CODE;
                                    var DDD = BaseParameter.DataGridView2[i].PART_SNP;
                                    var EEE = BaseParameter.DataGridView2[i].QTY;
                                    var GGG = BaseParameter.DataGridView2[i].TYPE;

                                    if (EEE > 0)
                                    {
                                        VAL_DT = "((SELECT `CD_IDX` FROM TSCODE WHERE `CD_NM_EN` ='" + AAA + "' AND `CDGR_IDX` = '17'), '" + BBB + "','" + CCC + "', '" + DDD + "', '" + EEE + "', '" + GGG + "',  'Y', 'N', 'N', NOW(), '" + USER_IDX + "')";
                                    }
                                    if (VAL_SUM.Length == 0)
                                    {
                                        VAL_SUM = VAL_DT;
                                    }
                                    else
                                    {
                                        VAL_SUM = VAL_SUM + ", " + VAL_DT;
                                    }
                                }
                                if (VAL_SUM.Length > 0)
                                {
                                    string sql = @"INSERT INTO `TMMTIN_DMM` (`TMMTIN_DMM_STGC`, `TMMTIN_DATE`, `TMMTIN_PART`, `TMMTIN_PART_SNP`, `TMMTIN_QTY`, `TMMTIN_CODE`, `TMMTIN_REC_YN`, `TMMTIN_CNF_YN`, `TMMTIN_DSCN_YN`, `CREATE_DTM`, `CREATE_USER`) VALUES  " + VAL_SUM;
                                    string sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        public virtual async Task<BaseResult> Buttondelete_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    if (BaseParameter.Action == 2)
                    {
                        string D3_D01 = BaseParameter.SearchString;
                        string sql = @"UPDATE `TMMTIN_DMM` SET `TMMTIN_REC_YN`='N'    WHERE   `TMMTIN_DMM_IDX` ='" + D3_D01 + "'";
                        await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
                    }
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        public virtual async Task<BaseResult> Buttoncancel_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                await Task.Run(() => { });
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        public virtual async Task<BaseResult> Buttoninport_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                await Task.Run(() => { });
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        public virtual async Task<BaseResult> Buttonexport_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                await Task.Run(() => { });
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        public virtual async Task<BaseResult> Buttonprint_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                await Task.Run(() => { });
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        public virtual async Task<BaseResult> Buttonhelp_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                await Task.Run(() => { });
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        public virtual async Task<BaseResult> Buttonclose_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                await Task.Run(() => { });
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        public virtual async Task<BaseResult> COMLIST_LINE(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    string sql = @"SELECT `CD_IDX`, `CD_SYS_NOTE` , `CD_NM_HAN`, `CD_NM_EN` FROM TSCODE WHERE `CD_SYS_NOTE` = 'Material Payment 01' 
UNION  SELECT `CD_IDX`, `CD_SYS_NOTE` , `CD_NM_HAN`, `CD_NM_EN` FROM TSCODE WHERE `CDGR_IDX` = '17'  AND  RIGHT(`CD_SYS_NOTE`, 2) > 60  ORDER BY `CD_SYS_NOTE`";

                    if (BaseParameter.RadioButton1 == true)
                    {
                        sql = @"SELECT `CD_IDX`, `CD_SYS_NOTE` , `CD_NM_HAN`, `CD_NM_EN` FROM TSCODE WHERE `CDGR_IDX` = '17'  AND  RIGHT(`CD_SYS_NOTE`, 2) > 0  AND  RIGHT(`CD_SYS_NOTE`, 2) <= 60  ORDER BY `CD_SYS_NOTE`";
                    }
                    DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                    result.cb_Stage1 = new List<SuperResultTranfer>();
                    result.T2_S1 = new List<SuperResultTranfer>();
                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        DataTable dt = ds.Tables[i];
                        result.cb_Stage1.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                        result.T2_S1.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                    }
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }

        public virtual async Task<BaseResult> GetListCategoryDepartmentByMembershipID_CompanyID_ActiveToListAsync(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            result.ListCategoryDepartment = new List<CategoryDepartment>();
            if (BaseParameter.MembershipID > 0 && BaseParameter.CompanyID > 0)
            {
                BaseParameter.Active = true;
                result.ListCategoryDepartment = await _CategoryDepartmentRepository.GetByMembershipID_CompanyID_ActiveToListAsync(BaseParameter.MembershipID, BaseParameter.CompanyID, BaseParameter.Active);
            }
            return result;
        }
        public virtual async Task<BaseResult> GetListMaterialBySearchString_ActiveToListAsync(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            if (!string.IsNullOrEmpty(BaseParameter.SearchString) && BaseParameter.CompanyID > 0)
            {
                BaseParameter.Active = true;
                result.ListMaterial = await _MaterialRepository.GetByCondition(o => o.CompanyID == BaseParameter.CompanyID && o.Code == BaseParameter.SearchString).ToListAsync();
                if (result.ListMaterial.Count == 0)
                {
                    result.ListMaterial = await _MaterialRepository.GetByCondition(o => o.CompanyID == BaseParameter.CompanyID && !string.IsNullOrEmpty(o.Code) && o.Code.Contains(BaseParameter.SearchString)).ToListAsync();
                }
                if (result.ListMaterial.Count == 0)
                {
                    result.ListMaterial = await _MaterialRepository.GetByCondition(o => o.CompanyID == BaseParameter.CompanyID && !string.IsNullOrEmpty(o.Name) && o.Name.Contains(BaseParameter.SearchString)).ToListAsync();
                }
                if (result.ListMaterial.Count > 0)
                {
                    var ListMaterialCode = result.ListMaterial.Select(o => o.Code).ToList();
                    var CategoryDepartmentID = 23;
                    switch (BaseParameter.CompanyID)
                    {
                        case 17:
                            CategoryDepartmentID = 188;
                            break;
                    }
                    var ListWarehouseInputDetailBarcode = new List<WarehouseInputDetailBarcode>();
                    var ListWarehouseInput = await _WarehouseInputRepository.GetByCondition(o => o.Active == true && o.CustomerID == CategoryDepartmentID).ToListAsync();
                    if (ListWarehouseInput.Count > 0)
                    {
                        var ListWarehouseInputID = ListWarehouseInput.Select(o => o.ID).ToList();
                        ListWarehouseInputDetailBarcode = await _WarehouseInputDetailBarcodeRepository.GetByCondition(o => o.Active == true && o.ParentID > 0 && ListWarehouseInputID.Contains(o.ParentID.Value) && ListMaterialCode.Contains(o.MaterialName)).ToListAsync();
                    }
                    for (int i = 0; i < result.ListMaterial.Count; i++)
                    {
                        var ListWarehouseInputDetailBarcodeSub = ListWarehouseInputDetailBarcode.Where(o => o.MaterialName == result.ListMaterial[i].Code).ToList();
                        result.ListMaterial[i].QuantityInput = ListWarehouseInputDetailBarcodeSub.Sum(o => o.QuantityInventory);
                    }
                }
            }
            return result;
        }

        public virtual async Task<BaseResult> SaveListWarehouseRequestDetailAsync(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            if (BaseParameter.MembershipID > 0 && BaseParameter.CompanyID > 0 && BaseParameter.CategoryDepartmentID > 0 && BaseParameter.ListWarehouseRequestDetail != null && BaseParameter.ListWarehouseRequestDetail.Count > 0)
            {
                var Membership = await _MembershipRepository.GetByCondition(o => o.CompanyID == BaseParameter.CompanyID && o.USER_IDX == BaseParameter.MembershipID && o.Active == true).FirstOrDefaultAsync();
                if (Membership != null && Membership.ID > 0)
                {
                    var WarehouseRequestDetail = BaseParameter.ListWarehouseRequestDetail[0];
                    var WarehouseRequest = await _WarehouseRequestService.GetByCondition(o => o.Active == true && o.IsManagerSupplier != true && o.CreateUserID == Membership.ID && o.CustomerID == BaseParameter.CategoryDepartmentID && o.Date != null && o.Date.Value.Date == GlobalHelper.InitializationDateTime.Date).OrderBy(o => o.Date).FirstOrDefaultAsync();
                    if (WarehouseRequest == null)
                    {
                        WarehouseRequest = new WarehouseRequest();
                    }
                    WarehouseRequest.Active = true;
                    WarehouseRequest.CustomerID = BaseParameter.CategoryDepartmentID;
                    WarehouseRequest.CreateUserID = Membership.ID;
                    WarehouseRequest.CreateUserCode = Membership.UserName;
                    WarehouseRequest.CreateUserName = Membership.Name;
                    WarehouseRequest.CompanyID = BaseParameter.CompanyID;
                    WarehouseRequest.Date = GlobalHelper.InitializationDateTime;
                    WarehouseRequest.Description = "MES";
                    WarehouseRequest.Display = BaseParameter.GroupCode;
                    string CategoryDepartmentCode = "";
                    if (WarehouseRequest.CustomerID > 0)
                    {
                        var CategoryDepartment = await _CategoryDepartmentRepository.GetByIDAsync(WarehouseRequest.CustomerID.Value);
                        if (CategoryDepartment != null && CategoryDepartment.ID > 0)
                        {
                            CategoryDepartmentCode = CategoryDepartment.Code;
                        }
                    }
                    WarehouseRequest.Note = Membership.UserName + "-" + Membership.Name + "-" + CategoryDepartmentCode + "-" + WarehouseRequest.Display + "-" + GlobalHelper.InitializationDateTimeCode0001;
                    WarehouseRequest.Code = Membership.Name + "-" + CategoryDepartmentCode + "-" + WarehouseRequest.Display + "-" + GlobalHelper.InitializationDateTimeCode0001;
                    var BaseParameterWarehouseRequest = new BaseParameter<WarehouseRequest>();
                    BaseParameterWarehouseRequest.BaseModel = WarehouseRequest;
                    await _WarehouseRequestService.SaveAsync(BaseParameterWarehouseRequest);
                    //WarehouseRequest = BaseParameterWarehouseRequest.BaseModel;
                    if (WarehouseRequest.ID > 0)
                    {
                        foreach (var WarehouseRequestDetailSub in BaseParameter.ListWarehouseRequestDetail)
                        {
                            WarehouseRequestDetailSub.ID = 0;
                            WarehouseRequestDetailSub.Active = true;
                            WarehouseRequestDetailSub.ParentID = WarehouseRequest.ID;
                            WarehouseRequestDetailSub.Date = GlobalHelper.InitializationDateTime;
                            var BaseParameterWarehouseRequestDetail = new BaseParameter<WarehouseRequestDetail>();
                            BaseParameterWarehouseRequestDetail.BaseModel = WarehouseRequestDetailSub;
                            await _WarehouseRequestDetailService.SaveAsync(BaseParameterWarehouseRequestDetail);
                        }
                    }
                }
            }
            return result;
        }
        public virtual async Task<BaseResult> GetWarehouseRequestDetailToListAsync(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            result.ListWarehouseRequestDetail = new List<WarehouseRequestDetail>();
            if (BaseParameter.CategoryDepartmentID > 0 && BaseParameter.Date != null)
            {

                result.ListWarehouseRequestDetail = await _WarehouseRequestDetailService.GetByCondition(o => o.ProductID == BaseParameter.CategoryDepartmentID && o.Date != null && o.Date.Value.Date == BaseParameter.Date.Value.Date).ToListAsync();
                switch (BaseParameter.Code)
                {
                    case "Y":
                        result.ListWarehouseRequestDetail = result.ListWarehouseRequestDetail.Where(o => o.Active == true).ToList();
                        break;
                    case "N":
                        result.ListWarehouseRequestDetail = result.ListWarehouseRequestDetail.Where(o => o.Active == false).ToList();
                        break;
                }
                if (!string.IsNullOrEmpty(BaseParameter.SearchString))
                {
                    result.ListWarehouseRequestDetail = result.ListWarehouseRequestDetail.Where(o => !string.IsNullOrEmpty(o.MaterialName) && o.MaterialName.Contains(BaseParameter.SearchString)).ToList();
                }
            }
            result.ListWarehouseRequestDetail = result.ListWarehouseRequestDetail.OrderByDescending(o => o.UpdateDate).ToList();
            return result;
        }
        public virtual async Task<BaseResult> RemoveWarehouseRequestDetailByIDAsync(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            if (BaseParameter.IDERP > 0)
            {
                await _WarehouseRequestDetailRepository.RemoveAsync(BaseParameter.IDERP.Value);
            }
            return result;
        }
        public virtual async Task<BaseResult> GetWarehouseOutputByCategoryDepartmentIDAndDateToListAsync(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            result.ListWarehouseOutput = new List<WarehouseOutput>();
            if (BaseParameter.CategoryDepartmentID > 0)
            {
                if (!string.IsNullOrEmpty(BaseParameter.SearchString))
                {
                    BaseParameter.SearchString = BaseParameter.SearchString.Trim();
                    result.ListWarehouseOutput = await _WarehouseOutputService.GetByCondition(o => o.CustomerID == BaseParameter.CategoryDepartmentID && !string.IsNullOrEmpty(o.Code) && o.Code.Contains(BaseParameter.SearchString)).ToListAsync();
                }
                else
                {
                    if (BaseParameter.Date != null)
                    {
                        result.ListWarehouseOutput = await _WarehouseOutputService.GetByCondition(o => o.CustomerID == BaseParameter.CategoryDepartmentID && o.Date != null && o.Date.Value.Date == BaseParameter.Date.Value.Date).ToListAsync();
                    }
                }
                var ListWarehouseInput = await _WarehouseInputService.GetByCondition(o => o.CustomerID == BaseParameter.CategoryDepartmentID && o.Active != true && o.WarehouseOutputID > 0).ToListAsync();
                if (ListWarehouseInput.Count > 0)
                {
                    var ListWarehouseOutputID = ListWarehouseInput.Select(o => o.WarehouseOutputID).ToList();
                    var ListWarehouseOutputIDSub = result.ListWarehouseOutput.Select(o => o.ID).ToList();
                    ListWarehouseOutputID = ListWarehouseOutputID.Where(o => o != null && !ListWarehouseOutputIDSub.Contains(o.Value)).ToList();
                    var ListWarehouseOutput = await _WarehouseOutputService.GetByCondition(o => ListWarehouseOutputID.Contains(o.ID)).ToListAsync();
                    result.ListWarehouseOutput.AddRange(ListWarehouseOutput);
                }
            }
            result.ListWarehouseOutput = result.ListWarehouseOutput.OrderByDescending(o => o.Date).ThenBy(o => o.Active).ToList();
            return result;
        }
        public virtual async Task<BaseResult> GetWarehouseRequestByCategoryDepartmentIDAndDateToListAsync(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            result.ListWarehouseRequest = new List<WarehouseRequest>();
            if (BaseParameter.CategoryDepartmentID > 0)
            {
                if (BaseParameter.Date != null)
                {
                    result.ListWarehouseRequest = await _WarehouseRequestService.GetByCondition(o => o.Active == true && o.IsManagerSupplier != true && o.CustomerID == BaseParameter.CategoryDepartmentID && o.Date != null && o.Date.Value.Date == BaseParameter.Date.Value.Date).ToListAsync();
                }
            }
            if (!string.IsNullOrEmpty(BaseParameter.SearchString))
            {
                result.ListWarehouseRequest = result.ListWarehouseRequest.Where(o => !string.IsNullOrEmpty(o.Code) && o.Code.Contains(BaseParameter.SearchString)).ToList();
            }
            result.ListWarehouseRequest = result.ListWarehouseRequest.OrderByDescending(o => o.Date).ThenBy(o => o.Active).ToList();
            return result;
        }
        public virtual async Task<BaseResult> GetWarehouseOutputByCompanyIDAndMembershipIDAndCategoryDepartmentIDAndDateToListAsync(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            result.ListWarehouseOutput = new List<WarehouseOutput>();
            if (BaseParameter.CompanyID > 0 && BaseParameter.MembershipID > 0 && BaseParameter.CategoryDepartmentID > 0)
            {
                var Membership = await _MembershipRepository.GetByCondition(o => o.CompanyID == BaseParameter.CompanyID && o.USER_IDX == BaseParameter.MembershipID && o.Active == true).FirstOrDefaultAsync();
                if (Membership != null && Membership.ID > 0)
                {
                    if (!string.IsNullOrEmpty(BaseParameter.SearchString))
                    {
                        BaseParameter.SearchString = BaseParameter.SearchString.Trim();
                        result.ListWarehouseOutput = await _WarehouseOutputService.GetByCondition(o => o.CreateUserID == Membership.ID && o.CustomerID == BaseParameter.CategoryDepartmentID && !string.IsNullOrEmpty(o.Code) && o.Code.Contains(BaseParameter.SearchString)).ToListAsync();
                    }
                    else
                    {
                        if (BaseParameter.Date != null)
                        {
                            result.ListWarehouseOutput = await _WarehouseOutputService.GetByCondition(o => o.CreateUserID == Membership.ID && o.CustomerID == BaseParameter.CategoryDepartmentID && o.Date != null && o.Date.Value.Date == BaseParameter.Date.Value.Date).ToListAsync();
                        }
                    }
                    var ListWarehouseInput = await _WarehouseInputService.GetByCondition(o => o.CreateUserID == Membership.ID && o.CustomerID == BaseParameter.CategoryDepartmentID && o.Active != true && o.WarehouseOutputID > 0).ToListAsync();
                    if (ListWarehouseInput.Count > 0)
                    {
                        var ListWarehouseOutputID = ListWarehouseInput.Select(o => o.WarehouseOutputID).ToList();
                        var ListWarehouseOutputIDSub = result.ListWarehouseOutput.Select(o => o.ID).ToList();
                        ListWarehouseOutputID = ListWarehouseOutputID.Where(o => o != null && !ListWarehouseOutputIDSub.Contains(o.Value)).ToList();
                        var ListWarehouseOutput = await _WarehouseOutputService.GetByCondition(o => ListWarehouseOutputID.Contains(o.ID)).ToListAsync();
                        result.ListWarehouseOutput.AddRange(ListWarehouseOutput);
                    }
                }
            }
            result.ListWarehouseOutput = result.ListWarehouseOutput.OrderByDescending(o => o.Date).ThenBy(o => o.Active).ToList();
            return result;
        }
        public virtual async Task<BaseResult> GetWarehouseOutputDetailByParentIDToListAsync(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            result.ListWarehouseOutputDetail = new List<WarehouseOutputDetail>();
            result.ListWarehouseOutput = new List<WarehouseOutput>();
            if (!string.IsNullOrEmpty(BaseParameter.SearchString))
            {
                BaseParameter.SearchString = BaseParameter.SearchString.Trim();
                if (BaseParameter.CategoryDepartmentID > 0)
                {
                    if (BaseParameter.Date != null)
                    {
                        result.ListWarehouseOutput = await _WarehouseOutputService.GetByCondition(o => o.CustomerID == BaseParameter.CategoryDepartmentID && o.Date != null && o.Date.Value.Date == BaseParameter.Date.Value.Date).ToListAsync();
                    }
                }
                if (result.ListWarehouseOutput.Count > 0)
                {
                    var ListWarehouseOutputID = result.ListWarehouseOutput.Select(o => o.ID).ToList();
                    result.ListWarehouseOutputDetail = await _WarehouseOutputDetailRepository.GetByCondition(o => o.ParentID > 0 && ListWarehouseOutputID.Contains(o.ParentID ?? 0) && o.MaterialName == BaseParameter.SearchString).ToListAsync();
                }
            }
            else
            {
                if (BaseParameter.ID > 0)
                {
                    result.ListWarehouseOutputDetail = await _WarehouseOutputDetailRepository.GetByCondition(o => o.ParentID == BaseParameter.ID).ToListAsync();
                }
            }
            result.ListWarehouseOutputDetail = result.ListWarehouseOutputDetail.OrderByDescending(o => o.QuantityGAP).ThenByDescending(o => o.UpdateDate).ToList();
            return result;
        }
        public virtual async Task<BaseResult> GetWarehouseRequestDetailByParentIDToListAsync(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            result.ListWarehouseRequestDetail1 = new List<WarehouseRequestDetail>();
            result.ListWarehouseRequest = new List<WarehouseRequest>();
            if (!string.IsNullOrEmpty(BaseParameter.SearchString))
            {
                BaseParameter.SearchString = BaseParameter.SearchString.Trim();
                if (BaseParameter.CategoryDepartmentID > 0)
                {
                    if (BaseParameter.Date != null)
                    {
                        result.ListWarehouseRequest = await _WarehouseRequestService.GetByCondition(o => o.CustomerID == BaseParameter.CategoryDepartmentID && o.Date != null && o.Date.Value.Date == BaseParameter.Date.Value.Date).ToListAsync();
                    }
                }
                if (result.ListWarehouseRequest.Count > 0)
                {
                    var ListWarehouseRequestID = result.ListWarehouseRequest.Select(o => o.ID).ToList();
                    result.ListWarehouseRequestDetail1 = await _WarehouseRequestDetailRepository.GetByCondition(o => o.ParentID > 0 && ListWarehouseRequestID.Contains(o.ParentID ?? 0) && o.MaterialName == BaseParameter.SearchString).ToListAsync();
                }
            }
            else
            {
                if (BaseParameter.ID > 0)
                {
                    result.ListWarehouseRequestDetail1 = await _WarehouseRequestDetailRepository.GetByCondition(o => o.ParentID == BaseParameter.ID).ToListAsync();
                }
            }
            result.ListWarehouseRequestDetail1 = result.ListWarehouseRequestDetail1.OrderByDescending(o => o.QuantityGAP).ThenByDescending(o => o.UpdateDate).ToList();
            return result;
        }
        public virtual async Task<BaseResult> SaveWarehouseInputByWarehouseOutputIDAsync(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            if (BaseParameter.ID > 0)
            {
                var WarehouseInput = await _WarehouseInputService.GetByCondition(o => o.WarehouseOutputID == BaseParameter.ID && o.Active != true).OrderByDescending(o => o.Date).FirstOrDefaultAsync();
                if (WarehouseInput != null && WarehouseInput.ID > 0)
                {
                    WarehouseInput.Active = true;
                    WarehouseInput.IsSync = false;
                    WarehouseInput.IsComplete = true;
                    var BaseParameterWarehouseInput = new BaseParameter<WarehouseInput>();
                    BaseParameterWarehouseInput.BaseModel = WarehouseInput;
                    await _WarehouseInputService.SaveAsync(BaseParameterWarehouseInput);
                }
            }
            return result;
        }
        public virtual async Task<BaseResult> SaveWarehouseOutputDetailByIDAsync(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            if (BaseParameter.ID > 0 && BaseParameter.Qty > 0)
            {
                var WarehouseOutputDetail = await _WarehouseOutputDetailRepository.GetByCondition(o => o.ID == BaseParameter.ID).FirstOrDefaultAsync();
                if (WarehouseOutputDetail != null && WarehouseOutputDetail.ID > 0)
                {
                    WarehouseOutputDetail.QuantityActual = BaseParameter.Qty;
                    await _WarehouseOutputDetailRepository.UpdateAsync(WarehouseOutputDetail);
                }
            }
            return result;
        }
        public virtual async Task<BaseResult> SaveWarehouseOutputDetailBarcodeAsync(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            result.ListWarehouseOutputDetailBarcode = new List<WarehouseOutputDetailBarcode>();
            if (BaseParameter.CompanyID > 0 && BaseParameter.CategoryDepartmentID > 0 && BaseParameter.IDERP > 0 && BaseParameter.MembershipID > 0 && !string.IsNullOrEmpty(BaseParameter.SearchString))
            {
                WarehouseOutput WarehouseOutput = new WarehouseOutput();
                WarehouseOutput.CompanyID = BaseParameter.CompanyID;
                WarehouseOutput.Date = GlobalHelper.InitializationDateTime;
                WarehouseOutput.Code = "METoKOMAX" + "-" + GlobalHelper.InitializationDateTime.ToString("yyyyMMdd");
                WarehouseOutput.SupplierID = BaseParameter.CategoryDepartmentID;
                WarehouseOutput.CustomerID = BaseParameter.IDERP;
                WarehouseOutput.UpdateUserID = BaseParameter.MembershipID;
                BaseParameter<WarehouseOutput> BaseParameterWarehouseOutputSave = new BaseParameter<WarehouseOutput>();
                BaseParameterWarehouseOutputSave.BaseModel = WarehouseOutput;
                await _WarehouseOutputService.SaveHookRackAsync(BaseParameterWarehouseOutputSave);
                if (WarehouseOutput.ID > 0)
                {
                    var WarehouseOutputDetailBarcode = new WarehouseOutputDetailBarcode();
                    BaseParameter.SearchString = BaseParameter.SearchString.Trim();
                    WarehouseOutputDetailBarcode.Barcode = BaseParameter.SearchString;
                    var WarehouseInputDetailBarcode = await _WarehouseInputDetailBarcodeRepository.GetByCondition(o => o.CategoryDepartmentID == BaseParameter.CategoryDepartmentID && o.Active == true && o.Barcode == WarehouseOutputDetailBarcode.Barcode).FirstOrDefaultAsync();
                    if (WarehouseInputDetailBarcode != null && WarehouseInputDetailBarcode.ID > 0 )
                    {
                        await MySQLHelper.ERPSyncAsync02(GlobalHelper.ERP_MariaDBConectionString);
                        WarehouseOutputDetailBarcode.Quantity = WarehouseInputDetailBarcode.QuantityInventory;                        
                        WarehouseOutputDetailBarcode.ParentID = WarehouseOutput.ID;
                        WarehouseOutputDetailBarcode.ParentName = WarehouseOutput.Code;
                        WarehouseOutputDetailBarcode.SupplierID = WarehouseOutput.SupplierID;
                        WarehouseOutputDetailBarcode.CustomerID = WarehouseOutput.CustomerID;
                        WarehouseOutputDetailBarcode.CategoryDepartmentID = WarehouseOutput.SupplierID;
                        WarehouseOutputDetailBarcode.Active = true;
                        WarehouseOutputDetailBarcode.CompanyID = BaseParameter.CompanyID;
                        BaseParameter<WarehouseOutputDetailBarcode> BaseParameterWarehouseOutputDetailBarcode = new BaseParameter<WarehouseOutputDetailBarcode>();
                        BaseParameterWarehouseOutputDetailBarcode.BaseModel = WarehouseOutputDetailBarcode;
                        await _WarehouseOutputDetailBarcodeService.SaveHookRackAsync(BaseParameterWarehouseOutputDetailBarcode);
                    }
                    result.ListWarehouseOutputDetailBarcode.Add(WarehouseOutputDetailBarcode);
                }
            }
            return result;
        }
    }
}

