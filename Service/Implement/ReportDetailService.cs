namespace Service.Implement
{
    public class ReportDetailService : BaseService<ReportDetail, IReportDetailRepository>
    , IReportDetailService
    {
        private readonly IReportDetailRepository _ReportDetailRepository;
        private readonly IWebHostEnvironment _WebHostEnvironment;

        private readonly IReportRepository _ReportRepository;
        private readonly IBOMRepository _BOMRepository;

        public ReportDetailService(IReportDetailRepository ReportDetailRepository
            , IWebHostEnvironment webHostEnvironment
            , IReportRepository ReportRepository
            , IBOMRepository BOMRepository) : base(ReportDetailRepository)
        {
            _ReportDetailRepository = ReportDetailRepository;
            _WebHostEnvironment = webHostEnvironment;
            _ReportRepository = ReportRepository;
            _BOMRepository = BOMRepository;
        }
        public override void InitializationSave(ReportDetail model)
        {
            BaseInitialization(model);

            model.Quantity01 = model.Quantity01 ?? GlobalHelper.InitializationNumber;
            model.Quantity02 = model.Quantity02 ?? GlobalHelper.InitializationNumber;
            model.Quantity03 = model.Quantity03 ?? GlobalHelper.InitializationNumber;
            model.Quantity04 = model.Quantity04 ?? GlobalHelper.InitializationNumber;
            model.Quantity05 = model.Quantity05 ?? GlobalHelper.InitializationNumber;
            model.Quantity06 = model.Quantity06 ?? GlobalHelper.InitializationNumber;
            model.Quantity07 = model.Quantity07 ?? GlobalHelper.InitializationNumber;
            model.Quantity08 = model.Quantity08 ?? GlobalHelper.InitializationNumber;
            model.Quantity09 = model.Quantity09 ?? GlobalHelper.InitializationNumber;
            model.Quantity10 = model.Quantity10 ?? GlobalHelper.InitializationNumber;
            model.Quantity11 = model.Quantity11 ?? GlobalHelper.InitializationNumber;
            model.Quantity12 = model.Quantity12 ?? GlobalHelper.InitializationNumber;
            model.Quantity13 = model.Quantity13 ?? GlobalHelper.InitializationNumber;
            model.Quantity14 = model.Quantity14 ?? GlobalHelper.InitializationNumber;
            model.Quantity15 = model.Quantity15 ?? GlobalHelper.InitializationNumber;
            model.Quantity16 = model.Quantity16 ?? GlobalHelper.InitializationNumber;
            model.Quantity17 = model.Quantity17 ?? GlobalHelper.InitializationNumber;
            model.Quantity18 = model.Quantity18 ?? GlobalHelper.InitializationNumber;
            model.Quantity19 = model.Quantity19 ?? GlobalHelper.InitializationNumber;
            model.Quantity20 = model.Quantity20 ?? GlobalHelper.InitializationNumber;
            model.Quantity21 = model.Quantity21 ?? GlobalHelper.InitializationNumber;
            model.Quantity22 = model.Quantity22 ?? GlobalHelper.InitializationNumber;
            model.Quantity23 = model.Quantity23 ?? GlobalHelper.InitializationNumber;
            model.Quantity24 = model.Quantity24 ?? GlobalHelper.InitializationNumber;
            model.Quantity25 = model.Quantity25 ?? GlobalHelper.InitializationNumber;
            model.Quantity26 = model.Quantity26 ?? GlobalHelper.InitializationNumber;
            model.Quantity27 = model.Quantity27 ?? GlobalHelper.InitializationNumber;
            model.Quantity28 = model.Quantity28 ?? GlobalHelper.InitializationNumber;
            model.Quantity29 = model.Quantity29 ?? GlobalHelper.InitializationNumber;
            model.Quantity30 = model.Quantity30 ?? GlobalHelper.InitializationNumber;
            model.Quantity31 = model.Quantity31 ?? GlobalHelper.InitializationNumber;

            model.QuantityActual01 = model.QuantityActual01 ?? GlobalHelper.InitializationNumber;
            model.QuantityActual02 = model.QuantityActual02 ?? GlobalHelper.InitializationNumber;
            model.QuantityActual03 = model.QuantityActual03 ?? GlobalHelper.InitializationNumber;
            model.QuantityActual04 = model.QuantityActual04 ?? GlobalHelper.InitializationNumber;
            model.QuantityActual05 = model.QuantityActual05 ?? GlobalHelper.InitializationNumber;
            model.QuantityActual06 = model.QuantityActual06 ?? GlobalHelper.InitializationNumber;
            model.QuantityActual07 = model.QuantityActual07 ?? GlobalHelper.InitializationNumber;
            model.QuantityActual08 = model.QuantityActual08 ?? GlobalHelper.InitializationNumber;
            model.QuantityActual09 = model.QuantityActual09 ?? GlobalHelper.InitializationNumber;
            model.QuantityActual10 = model.QuantityActual10 ?? GlobalHelper.InitializationNumber;
            model.QuantityActual11 = model.QuantityActual11 ?? GlobalHelper.InitializationNumber;
            model.QuantityActual12 = model.QuantityActual12 ?? GlobalHelper.InitializationNumber;
            model.QuantityActual13 = model.QuantityActual13 ?? GlobalHelper.InitializationNumber;
            model.QuantityActual14 = model.QuantityActual14 ?? GlobalHelper.InitializationNumber;
            model.QuantityActual15 = model.QuantityActual15 ?? GlobalHelper.InitializationNumber;
            model.QuantityActual16 = model.QuantityActual16 ?? GlobalHelper.InitializationNumber;
            model.QuantityActual17 = model.QuantityActual17 ?? GlobalHelper.InitializationNumber;
            model.QuantityActual18 = model.QuantityActual18 ?? GlobalHelper.InitializationNumber;
            model.QuantityActual19 = model.QuantityActual19 ?? GlobalHelper.InitializationNumber;
            model.QuantityActual20 = model.QuantityActual20 ?? GlobalHelper.InitializationNumber;
            model.QuantityActual21 = model.QuantityActual21 ?? GlobalHelper.InitializationNumber;
            model.QuantityActual22 = model.QuantityActual22 ?? GlobalHelper.InitializationNumber;
            model.QuantityActual23 = model.QuantityActual23 ?? GlobalHelper.InitializationNumber;
            model.QuantityActual24 = model.QuantityActual24 ?? GlobalHelper.InitializationNumber;
            model.QuantityActual25 = model.QuantityActual25 ?? GlobalHelper.InitializationNumber;
            model.QuantityActual26 = model.QuantityActual26 ?? GlobalHelper.InitializationNumber;
            model.QuantityActual27 = model.QuantityActual27 ?? GlobalHelper.InitializationNumber;
            model.QuantityActual28 = model.QuantityActual28 ?? GlobalHelper.InitializationNumber;
            model.QuantityActual29 = model.QuantityActual29 ?? GlobalHelper.InitializationNumber;
            model.QuantityActual30 = model.QuantityActual30 ?? GlobalHelper.InitializationNumber;
            model.QuantityActual31 = model.QuantityActual31 ?? GlobalHelper.InitializationNumber;

            //model.QuantityGAP01 = model.Quantity01 - model.QuantityActual01;
            //model.QuantityGAP02 = model.Quantity02 - model.QuantityActual02;
            //model.QuantityGAP03 = model.Quantity03 - model.QuantityActual03;
            //model.QuantityGAP04 = model.Quantity04 - model.QuantityActual04;
            //model.QuantityGAP05 = model.Quantity05 - model.QuantityActual05;
            //model.QuantityGAP06 = model.Quantity06 - model.QuantityActual06;
            //model.QuantityGAP07 = model.Quantity07 - model.QuantityActual07;
            //model.QuantityGAP08 = model.Quantity08 - model.QuantityActual08;
            //model.QuantityGAP09 = model.Quantity09 - model.QuantityActual09;
            //model.QuantityGAP10 = model.Quantity10 - model.QuantityActual10;
            //model.QuantityGAP11 = model.Quantity11 - model.QuantityActual11;
            //model.QuantityGAP12 = model.Quantity12 - model.QuantityActual12;
            //model.QuantityGAP13 = model.Quantity13 - model.QuantityActual13;
            //model.QuantityGAP14 = model.Quantity14 - model.QuantityActual14;
            //model.QuantityGAP15 = model.Quantity15 - model.QuantityActual15;
            //model.QuantityGAP16 = model.Quantity16 - model.QuantityActual16;
            //model.QuantityGAP17 = model.Quantity17 - model.QuantityActual17;
            //model.QuantityGAP18 = model.Quantity18 - model.QuantityActual18;
            //model.QuantityGAP19 = model.Quantity19 - model.QuantityActual19;
            //model.QuantityGAP20 = model.Quantity20 - model.QuantityActual20;
            //model.QuantityGAP21 = model.Quantity21 - model.QuantityActual21;
            //model.QuantityGAP22 = model.Quantity22 - model.QuantityActual22;
            //model.QuantityGAP23 = model.Quantity23 - model.QuantityActual23;
            //model.QuantityGAP24 = model.Quantity24 - model.QuantityActual24;
            //model.QuantityGAP25 = model.Quantity25 - model.QuantityActual25;
            //model.QuantityGAP26 = model.Quantity26 - model.QuantityActual26;
            //model.QuantityGAP27 = model.Quantity27 - model.QuantityActual27;
            //model.QuantityGAP28 = model.Quantity28 - model.QuantityActual28;
            //model.QuantityGAP29 = model.Quantity29 - model.QuantityActual29;
            //model.QuantityGAP30 = model.Quantity30 - model.QuantityActual30;
            //model.QuantityGAP31 = model.Quantity31 - model.QuantityActual31;


            model.Date00 = model.QuantityGAP31 < 0 ? model.Date31 : model.Date00;
            model.Date00 = model.QuantityGAP30 < 0 ? model.Date30 : model.Date00;
            model.Date00 = model.QuantityGAP29 < 0 ? model.Date29 : model.Date00;
            model.Date00 = model.QuantityGAP28 < 0 ? model.Date28 : model.Date00;
            model.Date00 = model.QuantityGAP27 < 0 ? model.Date27 : model.Date00;
            model.Date00 = model.QuantityGAP26 < 0 ? model.Date26 : model.Date00;
            model.Date00 = model.QuantityGAP25 < 0 ? model.Date25 : model.Date00;
            model.Date00 = model.QuantityGAP24 < 0 ? model.Date24 : model.Date00;
            model.Date00 = model.QuantityGAP23 < 0 ? model.Date23 : model.Date00;
            model.Date00 = model.QuantityGAP22 < 0 ? model.Date22 : model.Date00;
            model.Date00 = model.QuantityGAP21 < 0 ? model.Date21 : model.Date00;
            model.Date00 = model.QuantityGAP20 < 0 ? model.Date20 : model.Date00;
            model.Date00 = model.QuantityGAP19 < 0 ? model.Date19 : model.Date00;
            model.Date00 = model.QuantityGAP18 < 0 ? model.Date18 : model.Date00;
            model.Date00 = model.QuantityGAP17 < 0 ? model.Date17 : model.Date00;
            model.Date00 = model.QuantityGAP16 < 0 ? model.Date16 : model.Date00;
            model.Date00 = model.QuantityGAP15 < 0 ? model.Date15 : model.Date00;
            model.Date00 = model.QuantityGAP14 < 0 ? model.Date14 : model.Date00;
            model.Date00 = model.QuantityGAP13 < 0 ? model.Date13 : model.Date00;
            model.Date00 = model.QuantityGAP12 < 0 ? model.Date12 : model.Date00;
            model.Date00 = model.QuantityGAP11 < 0 ? model.Date11 : model.Date00;
            model.Date00 = model.QuantityGAP10 < 0 ? model.Date10 : model.Date00;
            model.Date00 = model.QuantityGAP09 < 0 ? model.Date09 : model.Date00;
            model.Date00 = model.QuantityGAP08 < 0 ? model.Date08 : model.Date00;
            model.Date00 = model.QuantityGAP07 < 0 ? model.Date07 : model.Date00;
            model.Date00 = model.QuantityGAP06 < 0 ? model.Date06 : model.Date00;
            model.Date00 = model.QuantityGAP05 < 0 ? model.Date05 : model.Date00;
            model.Date00 = model.QuantityGAP04 < 0 ? model.Date04 : model.Date00;
            model.Date00 = model.QuantityGAP03 < 0 ? model.Date03 : model.Date00;
            model.Date00 = model.QuantityGAP02 < 0 ? model.Date01 : model.Date00;
            model.Date00 = model.QuantityGAP01 < 0 ? model.Date01 : model.Date00;

            model.Quantity00 = model.Quantity01 + model.Quantity02 + model.Quantity03 + model.Quantity04
                + model.Quantity05 + model.Quantity06 + model.Quantity07 + model.Quantity08
                + model.Quantity09 + model.Quantity10 + model.Quantity11 + model.Quantity12
                + model.Quantity13 + model.Quantity14 + model.Quantity15 + model.Quantity16
                + model.Quantity17 + model.Quantity18 + model.Quantity19 + model.Quantity20
                + model.Quantity21 + model.Quantity22 + model.Quantity23 + model.Quantity24
                + model.Quantity25 + model.Quantity26 + model.Quantity27 + model.Quantity28
                + model.Quantity29 + model.Quantity30 + model.Quantity31;

            model.QuantityActual00 = model.QuantityActual01 + model.QuantityActual02 + model.QuantityActual03 + model.QuantityActual04 +
                model.QuantityActual05 + model.QuantityActual06 + model.QuantityActual07 + model.QuantityActual08 +
                model.QuantityActual09 + model.QuantityActual10 + model.QuantityActual11 + model.QuantityActual12 +
                model.QuantityActual13 + model.QuantityActual14 + model.QuantityActual15 + model.QuantityActual16 +
                model.QuantityActual17 + model.QuantityActual18 + model.QuantityActual19 + model.QuantityActual20 +
                model.QuantityActual21 + model.QuantityActual22 + model.QuantityActual23 + model.QuantityActual24 +
                model.QuantityActual25 + model.QuantityActual26 + model.QuantityActual27 + model.QuantityActual28 +
                model.QuantityActual29 + model.QuantityActual30 + model.QuantityActual31;

            model.QuantityGAP00 = model.Quantity00 - model.QuantityActual00;
        }
        public override async Task<BaseResult<ReportDetail>> SaveAsync(BaseParameter<ReportDetail> BaseParameter)
        {
            var result = new BaseResult<ReportDetail>();
            InitializationSave(BaseParameter.BaseModel);
            var ModelCheck = await GetByCondition(o => o.ParentID == BaseParameter.BaseModel.ParentID && o.Code == BaseParameter.BaseModel.Code && o.Name == BaseParameter.BaseModel.Name).FirstOrDefaultAsync();
            SetModelByModelCheck(BaseParameter.BaseModel, ModelCheck);
            if (BaseParameter.BaseModel.ID > 0)
            {
                result = await UpdateAsync(BaseParameter);
            }
            else
            {
                result = await AddAsync(BaseParameter);
            }
            if (result.BaseModel.ID > 0)
            {
            }
            return result;
        }
        public virtual async Task<BaseResult<ReportDetail>> GetProductionTracking2026ByParentIDToListAsync(BaseParameter<ReportDetail> BaseParameter)
        {
            var result = new BaseResult<ReportDetail>();
            result.List = new List<ReportDetail>();
            if (BaseParameter.ParentID > 0)
            {
                var Report = await _ReportRepository.GetByIDAsync(BaseParameter.ParentID.Value);
                if (Report.ID > 0)
                {
                    result.List = await GetByCondition(o => o.ParentID == BaseParameter.ParentID).ToListAsync();
                    for (int i = 0; i < result.List.Count; i++)
                    {
                        result.List[i].Quantity30 = result.List[i].Quantity30 ?? 0;
                        result.List[i].Quantity31 = result.List[i].Quantity31 ?? 0;
                    }
                    if (!string.IsNullOrEmpty(BaseParameter.Name))
                    {
                        BaseParameter.Name = BaseParameter.Name.Trim();
                        var BOM = new BOM();
                        if (!string.IsNullOrEmpty(BaseParameter.Display))
                        {
                            BaseParameter.Display = BaseParameter.Display.Trim();
                            BOM = await _BOMRepository.GetByCondition(o => o.Active == true && o.CompanyID == Report.CompanyID && o.MaterialCode == BaseParameter.Name && o.Code == BaseParameter.Display).FirstOrDefaultAsync();
                        }
                        else
                        {
                            BOM = await _BOMRepository.GetByCondition(o => o.Active == true && o.CompanyID == Report.CompanyID && o.MaterialCode == BaseParameter.Name).FirstOrDefaultAsync();
                        }
                        if (BOM != null && BOM.ID > 0)
                        {
                            var ListBOM = await _BOMRepository.GetByCondition(o => o.Active == true && o.ParentID == BOM.ID).ToListAsync();
                            if (ListBOM.Count > 0)
                            {
                                var ListBOMMaterialCode = ListBOM.Select(o => o.MaterialCode).Distinct().ToList();
                                var ListReportDetail = new List<ReportDetail>();
                                foreach (var MaterialCode in ListBOMMaterialCode)
                                {
                                    var ReportDetail = result.List.Where(o => o.Code == MaterialCode).FirstOrDefault();
                                    if (ReportDetail == null)
                                    {
                                        ReportDetail = new ReportDetail();
                                    }
                                    var BOMSub = ListBOM.Where(o => o.MaterialCode == MaterialCode).FirstOrDefault();
                                    if (BOMSub != null && BOMSub.ID > 0)
                                    {
                                        if (BOMSub.IsLeadNo == true)
                                        {
                                            ReportDetail.Active = true;
                                        }
                                        if (BOMSub.IsSPST == true)
                                        {
                                            ReportDetail.Active = false;
                                        }
                                    }
                                    ReportDetail.Code = MaterialCode;
                                    ReportDetail.Name = BaseParameter.Name;
                                    ReportDetail.Display = BaseParameter.Display;
                                    ListReportDetail.Add(ReportDetail);
                                }
                                for (int i = 0; i < ListReportDetail.Count; i++)
                                {
                                    ListReportDetail[i].Quantity30 = ListReportDetail[i].Quantity30 ?? 0;
                                    ListReportDetail[i].Quantity31 = ListReportDetail[i].Quantity31 ?? 0;
                                    if (ListReportDetail[i].Active == true)
                                    {
                                        ListReportDetail[i].Quantity02 = 0;
                                        var BOMSub = ListBOM.Where(o => o.MaterialCode == ListReportDetail[i].Code).FirstOrDefault();
                                        if (BOMSub != null && BOMSub.ID > 0)
                                        {
                                            if (!string.IsNullOrEmpty(BOMSub.ParentName01))
                                            {
                                                var ListReportDetailSub = ListReportDetail.Where(o => o.Code == BOMSub.ParentName01).ToList();
                                                ListReportDetail[i].Quantity02 = ListReportDetail[i].Quantity02 + ListReportDetailSub.Sum(o => o.Quantity01 ?? 0);
                                            }
                                            if (!string.IsNullOrEmpty(BOMSub.ParentName02))
                                            {
                                                var ListReportDetailSub = ListReportDetail.Where(o => o.Code == BOMSub.ParentName02).ToList();
                                                ListReportDetail[i].Quantity02 = ListReportDetail[i].Quantity02 + ListReportDetailSub.Sum(o => o.Quantity01 ?? 0);
                                            }
                                            if (!string.IsNullOrEmpty(BOMSub.ParentName03))
                                            {
                                                var ListReportDetailSub = ListReportDetail.Where(o => o.Code == BOMSub.ParentName03).ToList();
                                                ListReportDetail[i].Quantity02 = ListReportDetail[i].Quantity02 + ListReportDetailSub.Sum(o => o.Quantity01 ?? 0);
                                            }
                                            if (!string.IsNullOrEmpty(BOMSub.ParentName04))
                                            {
                                                var ListReportDetailSub = ListReportDetail.Where(o => o.Code == BOMSub.ParentName04).ToList();
                                                ListReportDetail[i].Quantity02 = ListReportDetail[i].Quantity02 + ListReportDetailSub.Sum(o => o.Quantity01 ?? 0);
                                            }
                                            ListReportDetail[i].Quantity30 = ListReportDetail[i].Quantity30 + (ListReportDetail[i].Quantity02 ?? 0);
                                        }
                                    }
                                }
                                for (int i = 0; i < ListReportDetail.Count; i++)
                                {
                                    ListReportDetail[i].Quantity31 = ListReportDetail.Min(o => o.Quantity30);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (BaseParameter.Active == true)
                        {
                            var ListCode = new List<string?>();
                            string MariaDBConectionString = GlobalHelper.MESConectionStringByCompanyID(BaseParameter.CompanyID.Value);
                            string DateEnd = GlobalHelper.InitializationDateTime.ToString("yyyy-MM-dd 23:59:59");
                            string DateBegin = GlobalHelper.InitializationDateTime.AddDays(0).ToString("yyyy-MM-dd 00:00:00");
                            List<torderlist> Listtorderlist = new List<torderlist>();
                            string sql = @"SELECT DISTINCT LEAD_NO from torderlist WHERE UPDATE_DTM >= '" + DateBegin + "' AND UPDATE_DTM <= '" + DateEnd + "' AND `CONDITION` IN ('Complete', 'Working') AND (MC NOT IN ('SHIELD WIRE') OR MC NOT IN ('SHIELD WIRE'))";
                            DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(MariaDBConectionString, sql);
                            for (int i = 0; i < ds.Tables.Count; i++)
                            {
                                DataTable dt = ds.Tables[i];
                                Listtorderlist.AddRange(SQLHelper.ToList<torderlist>(dt));
                            }
                            ListCode.AddRange(Listtorderlist.Select(o => o.LEAD_NO).Distinct().ToList());
                            List<torderlist_lp> Listtorderlist_lp = new List<torderlist_lp>();
                            sql = @"SELECT DISTINCT TORDERLIST.LEAD_NO from TORDERLIST_LP JOIN TORDERLIST ON TORDERLIST_LP.ORDER_IDX=TORDERLIST.ORDER_IDX WHERE TORDERLIST_LP.UPDATE_DTM >= '" + DateBegin + "' AND TORDERLIST_LP.UPDATE_DTM <= '" + DateEnd + "' AND TORDERLIST_LP.`CONDITION` IN ('Complete', 'Working')";
                            ds = await MySQLHelper.FillDataSetBySQLAsync(MariaDBConectionString, sql);
                            for (int i = 0; i < ds.Tables.Count; i++)
                            {
                                DataTable dt = ds.Tables[i];
                                Listtorderlist_lp.AddRange(SQLHelper.ToList<torderlist_lp>(dt));
                            }
                            ListCode.AddRange(Listtorderlist_lp.Select(o => o.LEAD_NO).Distinct().ToList());
                            List<torderlist_spst> Listtorderlist_spst = new List<torderlist_spst>();
                            sql = @"SELECT LEAD_NO from torderlist_spst WHERE UPDATE_DTM >= '" + DateBegin + "' AND UPDATE_DTM <= '" + DateEnd + "' AND `CONDITION` IN ('Complete', 'Working')";
                            ds = await MySQLHelper.FillDataSetBySQLAsync(MariaDBConectionString, sql);
                            for (int i = 0; i < ds.Tables.Count; i++)
                            {
                                DataTable dt = ds.Tables[i];
                                Listtorderlist_spst.AddRange(SQLHelper.ToList<torderlist_spst>(dt));
                            }
                            ListCode.AddRange(Listtorderlist_spst.Select(o => o.LEAD_NO).Distinct().ToList());
                            result.List = result.List.Where(o => ListCode.Contains(o.Code)).ToList();
                        }
                        if (!string.IsNullOrEmpty(BaseParameter.Code))
                        {
                            BaseParameter.Code = BaseParameter.Code.Trim();
                            result.List = result.List.Where(o => o.Code == BaseParameter.Code).ToList();
                        }
                    }
                }
            }
            result.List = result.List.OrderByDescending(o => o.Active).ThenBy(o => o.Code).ToList();
            return result;
        }
        public virtual async Task<BaseResult<ReportDetail>> GetWarehouseStockLongTermByParentIDToListAsync(BaseParameter<ReportDetail> BaseParameter)
        {
            var result = new BaseResult<ReportDetail>();
            result.List = new List<ReportDetail>();
            if (BaseParameter.ParentID > 0)
            {
                var Report = await _ReportRepository.GetByIDAsync(BaseParameter.ParentID.Value);
                if (Report.ID > 0)
                {
                    result.List = await GetByCondition(o => o.ParentID == BaseParameter.ParentID && o.SortOrder < 1000).ToListAsync();
                    result.List = result.List.OrderBy(o => o.SortOrder).ThenByDescending(o => o.Display).ThenBy(o => o.Name).ThenBy(o => o.Code).ToList();
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<ReportDetail>> GetWarehouseStockLongTerm1000ByParentIDToListAsync(BaseParameter<ReportDetail> BaseParameter)
        {
            var result = new BaseResult<ReportDetail>();
            result.List = new List<ReportDetail>();
            if (BaseParameter.ParentID > 0)
            {
                var Report = await _ReportRepository.GetByIDAsync(BaseParameter.ParentID.Value);
                if (Report.ID > 0)
                {
                    result.List = await GetByCondition(o => o.ParentID == BaseParameter.ParentID && o.SortOrder == 1000).ToListAsync();
                    result.List = result.List.OrderBy(o => o.SortOrder).ThenByDescending(o => o.Display).ThenBy(o => o.Name).ThenBy(o => o.Code).ToList();
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<ReportDetail>> HookRackByParentIDExportToExcelAsync(BaseParameter<ReportDetail> BaseParameter)
        {
            var result = new BaseResult<ReportDetail>();
            if (BaseParameter.ParentID > 0)
            {
                result.List = await GetByCondition(o => o.ParentID == BaseParameter.ParentID).ToListAsync();
                if (result.List.Count > 0)
                {
                    var ReportDetail = result.List[0];
                    string fileName = ReportDetail.ParentName + "-" + GlobalHelper.InitializationDateTimeCode + ".xlsx";
                    var streamExport = new MemoryStream();
                    InitializationExcel(result.List, streamExport);
                    var physicalPathCreate = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.Download, fileName);
                    using (var stream = new FileStream(physicalPathCreate, FileMode.Create))
                    {
                        streamExport.CopyTo(stream);
                    }
                    result.Message = GlobalHelper.URLSite + "/" + GlobalHelper.Download + "/" + fileName;
                }
            }
            return result;
        }
        private void InitializationExcel(List<ReportDetail> list, MemoryStream streamExport)
        {
            using (var package = new ExcelPackage(streamExport))
            {
                var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                int row = 1;
                int column = 1;
                workSheet.Cells[row, column].Value = "No";
                column = column + 1;
                workSheet.Cells[row, column].Value = "Lead No";
                column = column + 1;
                workSheet.Cells[row, column].Value = "Stock";
                column = column + 1;
                workSheet.Cells[row, column].Value = "FIFO Begin";
                column = column + 1;
                workSheet.Cells[row, column].Value = "FIFO End";
                column = column + 1;
                workSheet.Cells[row, column].Value = "Finish Goods";
                column = column + 1;
                workSheet.Cells[row, column].Value = "ECN";
                column = column + 1;
                workSheet.Cells[row, column].Value = "PO";
                column = column + 1;
                workSheet.Cells[row, column].Value = "BOM";
                column = column + 1;

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

                column = column + 1;



                row = row + 1;                
                foreach (ReportDetail item in list)
                {
                    workSheet.Cells[row, 1].Value = row - 1;
                    workSheet.Cells[row, 2].Value = item.Code;
                    try
                    {
                        workSheet.Cells[row, 3].Value = item.Quantity00.Value.ToString("N0");
                    }
                    catch (Exception ex)
                    {
                        string msg = ex.Message;
                        workSheet.Cells[row, 3].Value = "";
                    }
                    try
                    {
                        workSheet.Cells[row, 4].Value = item.Date01.Value.ToString("yyyy-mm-dd");
                    }
                    catch (Exception ex)
                    {
                        string msg = ex.Message;
                        workSheet.Cells[row, 4].Value = "";
                    }
                    try
                    {
                        workSheet.Cells[row, 5].Value = item.Date02.Value.ToString("yyyy-mm-dd");
                    }
                    catch (Exception ex)
                    {
                        string msg = ex.Message;
                        workSheet.Cells[row, 5].Value = "";
                    }
                    workSheet.Cells[row, 6].Value = item.Name;
                    workSheet.Cells[row, 7].Value = item.Display;
                    workSheet.Cells[row, 8].Value = item.Description;
                    workSheet.Cells[row, 9].Value = item.Note;

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

