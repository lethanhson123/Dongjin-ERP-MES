namespace Service.Implement
{
    public class ProductionOrderProductionPlanSemiService : BaseService<ProductionOrderProductionPlanSemi, IProductionOrderProductionPlanSemiRepository>
    , IProductionOrderProductionPlanSemiService
    {
        private readonly IWebHostEnvironment _WebHostEnvironment;
        private readonly IProductionOrderProductionPlanSemiRepository _ProductionOrderProductionPlanSemiRepository;
        private readonly IProductionOrderRepository _ProductionOrderRepository;
        private readonly IProductionOrderProductionPlanRepository _ProductionOrderProductionPlanRepository;

        private readonly IMaterialRepository _MaterialRepository;
        private readonly IBOMRepository _BOMRepository;


        public ProductionOrderProductionPlanSemiService(IProductionOrderProductionPlanSemiRepository ProductionOrderProductionPlanSemiRepository

            , IWebHostEnvironment WebHostEnvironment
            , IProductionOrderRepository ProductionOrderRepository
            , IProductionOrderProductionPlanRepository ProductionOrderProductionPlanRepository
            , IMaterialRepository MaterialRepository
            , IBOMRepository BOMRepository


            ) : base(ProductionOrderProductionPlanSemiRepository)
        {
            _WebHostEnvironment = WebHostEnvironment;
            _ProductionOrderProductionPlanSemiRepository = ProductionOrderProductionPlanSemiRepository;
            _ProductionOrderRepository = ProductionOrderRepository;
            _ProductionOrderProductionPlanRepository = ProductionOrderProductionPlanRepository;
            _MaterialRepository = MaterialRepository;
            _BOMRepository = BOMRepository;

        }
        public override void Initialization(ProductionOrderProductionPlanSemi model)
        {
            BaseInitialization(model);
            model.BOMQuantity = model.BOMQuantity ?? 1;
            if (model.ProductionOrderProductionPlanID > 0)
            {
                var ProductionOrderProductionPlan = _ProductionOrderProductionPlanRepository.GetByID(model.ProductionOrderProductionPlanID.Value);
                model.ParentID = ProductionOrderProductionPlan.ParentID;
                model.ParentName = ProductionOrderProductionPlan.ParentName;
                model.Code = ProductionOrderProductionPlan.Code;
                model.Name = ProductionOrderProductionPlan.Name;
                model.CompanyID = ProductionOrderProductionPlan.CompanyID;
                model.CompanyName = ProductionOrderProductionPlan.CompanyName;
                model.Active = model.Active ?? ProductionOrderProductionPlan.Active;
                model.ProductionOrderDetailID = ProductionOrderProductionPlan.ProductionOrderDetailID;
                model.MaterialID01 = ProductionOrderProductionPlan.MaterialID;
                model.MaterialCode01 = ProductionOrderProductionPlan.MaterialCode;
                model.MaterialName01 = ProductionOrderProductionPlan.MaterialName;

                int Index = 1;
                foreach (PropertyInfo ProductionOrderProductionPlanPropertyInfo in ProductionOrderProductionPlan.GetType().GetProperties())
                {
                    foreach (PropertyInfo ProductionOrderProductionPlanSemiPropertyInfo in model.GetType().GetProperties())
                    {
                        string IndexName = Index.ToString();
                        if (Index < 10)
                        {
                            IndexName = "0" + Index.ToString();
                        }
                        string DateString = "Date" + IndexName;
                        string DateNameString = "Date" + IndexName + "Name";
                        string QuantityNameString = "Quantity" + IndexName;
                        string QuantityCutNameString = "QuantityCut" + IndexName;
                        string ActiveNameString = "Active" + IndexName;
                        string QuantityActualNameString = "QuantityActual" + IndexName;
                        if (ProductionOrderProductionPlanPropertyInfo.Name == ProductionOrderProductionPlanSemiPropertyInfo.Name)
                        {
                            if (ProductionOrderProductionPlanPropertyInfo.Name == DateString)
                            {
                                ProductionOrderProductionPlanSemiPropertyInfo.SetValue(model, ProductionOrderProductionPlanPropertyInfo.GetValue(ProductionOrderProductionPlan), null);
                            }

                            if (ProductionOrderProductionPlanPropertyInfo.Name == DateNameString)
                            {
                                ProductionOrderProductionPlanSemiPropertyInfo.SetValue(model, ProductionOrderProductionPlanPropertyInfo.GetValue(ProductionOrderProductionPlan), null);
                            }

                            if (ProductionOrderProductionPlanPropertyInfo.Name == QuantityNameString && ProductionOrderProductionPlanSemiPropertyInfo.GetValue(model) == null && ProductionOrderProductionPlanPropertyInfo.GetValue(ProductionOrderProductionPlan) != null)
                            {
                                try
                                {
                                    foreach (PropertyInfo ProductionOrderProductionPlanPropertyInfoCut in ProductionOrderProductionPlan.GetType().GetProperties())
                                    {
                                        if (ProductionOrderProductionPlanPropertyInfoCut.Name == QuantityCutNameString)
                                        {
                                            int? ProductionPlanQuantityCut = (int?)ProductionOrderProductionPlanPropertyInfoCut.GetValue(ProductionOrderProductionPlan);
                                            int? Quantity = model.BOMQuantity * ProductionPlanQuantityCut;
                                            ProductionOrderProductionPlanSemiPropertyInfo.SetValue(model, Quantity, null);
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    string mes = ex.Message;
                                }
                                Index = Index + 1;
                            }
                        }
                    }
                }
            }

            if (model.MaterialID > 0)
            {
            }
            else
            {
                var Material = _MaterialRepository.GetByDescription(model.MaterialCode, model.CompanyID);
                model.MaterialID = Material.ID;
            }
            if (model.MaterialID > 0)
            {
                var Material = _MaterialRepository.GetByID(model.MaterialID.Value);
                model.MaterialID = Material.ID;
                model.MaterialCode = Material.Code;
                model.MaterialName = Material.Name;
                model.QuantitySNP = Material.QuantitySNP;
            }

            model.Priority = model.Priority ?? 1;

            model.Quantity00 = GlobalHelper.InitializationNumber;
            model.QuantityActual00 = GlobalHelper.InitializationNumber;

            SQLHelper.InitializationQuantityGAP<ProductionOrderProductionPlanSemi>(model);
            SQLHelper.InitializationQuantity00<ProductionOrderProductionPlanSemi>(model);
            SQLHelper.InitializationQuantityActual00<ProductionOrderProductionPlanSemi>(model);

            ProductionOrderProductionPlanSemiSetQuantityGAP(model);
            model.QuantityGAP00 = model.Quantity00 - model.QuantityActual00;
        }
        public override async Task<BaseResult<ProductionOrderProductionPlanSemi>> SaveAsync(BaseParameter<ProductionOrderProductionPlanSemi> BaseParameter)
        {
            var result = new BaseResult<ProductionOrderProductionPlanSemi>();
            if (BaseParameter != null && BaseParameter.BaseModel != null)
            {
                var ModelCheck = await GetByCondition(o => o.ProductionOrderProductionPlanID == BaseParameter.BaseModel.ProductionOrderProductionPlanID && o.BOMID == BaseParameter.BaseModel.BOMID).FirstOrDefaultAsync();
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
                    try
                    {
                        BaseParameter.BaseModel = result.BaseModel;
                        await SyncAsync(BaseParameter);
                    }
                    catch (Exception ex)
                    {
                        string mes = ex.Message;
                    }
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<ProductionOrderProductionPlanSemi>> SyncAsync(BaseParameter<ProductionOrderProductionPlanSemi> BaseParameter)
        {
            var result = new BaseResult<ProductionOrderProductionPlanSemi>();
            await SyncProductionOrderProductionPlanSemiAsync(BaseParameter);
            await SyncProductionOrderProductionPlanAsync(BaseParameter);
            //await SyncProductionOrderProductionPlanSemiSumAsync(BaseParameter);
            return result;
        }
        public virtual async Task<BaseResult<ProductionOrderProductionPlanSemi>> SyncProductionOrderProductionPlanAsync(BaseParameter<ProductionOrderProductionPlanSemi> BaseParameter)
        {
            var result = new BaseResult<ProductionOrderProductionPlanSemi>();
            try
            {
                if (BaseParameter.BaseModel != null)
                {
                    if (BaseParameter.BaseModel.ID > 0)
                    {
                        if (BaseParameter.BaseModel.ParentID > 0)
                        {
                            if (BaseParameter.BaseModel.SortOrder > 1)
                            {
                                if (BaseParameter.BaseModel.ProductionOrderProductionPlanID > 0)
                                {
                                    ProductionOrderProductionPlan ProductionOrderProductionPlan = await _ProductionOrderProductionPlanRepository.GetByIDAsync(BaseParameter.BaseModel.ProductionOrderProductionPlanID.Value);
                                    if (ProductionOrderProductionPlan.ID > 0)
                                    {
                                        var ListBOM = await _BOMRepository.GetByCondition(o => o.Active == true && o.ParentID == ProductionOrderProductionPlan.BOMID).OrderBy(o => o.MaterialCode).ToListAsync();
                                        var ListProductionOrderProductionPlanSemi = await GetByCondition(o => o.ProductionOrderProductionPlanID == ProductionOrderProductionPlan.ID && o.SortOrder > 1).ToListAsync();
                                        if (ListBOM.Count == ListProductionOrderProductionPlanSemi.Count)
                                        {
                                            List<int> ListQuantityActual = new List<int>();
                                            ListProductionOrderProductionPlanSemi = ListProductionOrderProductionPlanSemi.Where(o => o.IsLeadNo == true).ToList();
                                            foreach (var ProductionOrderProductionPlanSemi in ListProductionOrderProductionPlanSemi)
                                            {
                                                if (ProductionOrderProductionPlanSemi.IsLeadNo == true)
                                                {
                                                    int Index = 1;
                                                    foreach (PropertyInfo ProductionOrderProductionPlanSemiPropertyInfo in ProductionOrderProductionPlanSemi.GetType().GetProperties())
                                                    {
                                                        string IndexName = Index.ToString();
                                                        if (Index < 10)
                                                        {
                                                            IndexName = "0" + Index.ToString();
                                                        }
                                                        string DateString = "Date" + IndexName;
                                                        string QuantityActualNameString = "QuantityActual" + IndexName;
                                                        if (ProductionOrderProductionPlanSemiPropertyInfo.Name == DateString)
                                                        {
                                                            DateTime ProductionOrderProductionPlanSemiDateTime = (DateTime)ProductionOrderProductionPlanSemiPropertyInfo.GetValue(ProductionOrderProductionPlanSemi);
                                                            if (ProductionOrderProductionPlanSemiDateTime != null)
                                                            {
                                                                if (ProductionOrderProductionPlanSemiDateTime.Date == GlobalHelper.InitializationDateTime.Date)
                                                                {
                                                                    foreach (PropertyInfo ProductionOrderProductionPlanSemiPropertyInfoQuantityActual in ProductionOrderProductionPlanSemi.GetType().GetProperties())
                                                                    {
                                                                        if (ProductionOrderProductionPlanSemiPropertyInfoQuantityActual.Name == QuantityActualNameString)
                                                                        {
                                                                            int ProductionOrderProductionPlanSemiQuantityActual = (int)ProductionOrderProductionPlanSemiPropertyInfoQuantityActual.GetValue(ProductionOrderProductionPlanSemi);
                                                                            ListQuantityActual.Add(ProductionOrderProductionPlanSemiQuantityActual);
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            Index = Index + 1;
                                                        }
                                                    }
                                                }
                                            }
                                            if (ListQuantityActual.Count > 0)
                                            {
                                                int QuantityActual = ListQuantityActual.Min();
                                                if (QuantityActual > 0)
                                                {
                                                    int Index = 0;
                                                    foreach (PropertyInfo ProductionOrderProductionPlanPropertyInfo in ProductionOrderProductionPlan.GetType().GetProperties())
                                                    {
                                                        string IndexName = Index.ToString();
                                                        if (Index < 10)
                                                        {
                                                            IndexName = "0" + Index.ToString();
                                                        }
                                                        string DateString = "Date" + IndexName;
                                                        string QuantityActualNameString = "QuantityActual" + IndexName;
                                                        if (ProductionOrderProductionPlanPropertyInfo.Name == DateString)
                                                        {
                                                            DateTime ProductionOrderProductionPlanDateTime = (DateTime)ProductionOrderProductionPlanPropertyInfo.GetValue(ProductionOrderProductionPlan);
                                                            if (ProductionOrderProductionPlanDateTime != null)
                                                            {
                                                                if (ProductionOrderProductionPlanDateTime.Date == GlobalHelper.InitializationDateTime.Date)
                                                                {
                                                                    foreach (PropertyInfo ProductionOrderProductionPlanPropertyInfoQuantityActual in ProductionOrderProductionPlan.GetType().GetProperties())
                                                                    {
                                                                        if (ProductionOrderProductionPlanPropertyInfoQuantityActual.Name == QuantityActualNameString)
                                                                        {
                                                                            ProductionOrderProductionPlanPropertyInfoQuantityActual.SetValue(ProductionOrderProductionPlan, QuantityActual, null);
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            Index = Index + 1;
                                                        }
                                                    }
                                                }
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
                string mes = ex.Message;
            }
            return result;
        }
        public virtual async Task<BaseResult<ProductionOrderProductionPlanSemi>> SyncProductionOrderProductionPlanSemiAsync(BaseParameter<ProductionOrderProductionPlanSemi> BaseParameter)
        {
            var result = new BaseResult<ProductionOrderProductionPlanSemi>();
            try
            {
                if (BaseParameter.BaseModel != null)
                {
                    if (BaseParameter.BaseModel.ID > 0)
                    {
                        if (BaseParameter.BaseModel.ParentID > 0)
                        {
                            if (BaseParameter.BaseModel.SortOrder > 1)
                            {
                                var ProductionOrderProductionPlanSemi = await _ProductionOrderProductionPlanSemiRepository.GetByIDAsync(BaseParameter.BaseModel.ID);
                                ProductionOrderProductionPlanSemi.ID = 0;
                                ProductionOrderProductionPlanSemi.MaterialID01 = GlobalHelper.InitializationNumber;
                                ProductionOrderProductionPlanSemi.MaterialID = GlobalHelper.InitializationNumber;
                                ProductionOrderProductionPlanSemi.MaterialCode = GlobalHelper.InitializationString;
                                ProductionOrderProductionPlanSemi.MaterialCode01 = GlobalHelper.InitializationString;
                                ProductionOrderProductionPlanSemi.SortOrder = 1;
                                ProductionOrderProductionPlanSemi.IsLeadNo = null;
                                ProductionOrderProductionPlanSemi.IsSPST = null;
                                var ModelCheck = await GetByCondition(o => o.ParentID == ProductionOrderProductionPlanSemi.ParentID && o.SortOrder == ProductionOrderProductionPlanSemi.SortOrder).FirstOrDefaultAsync();
                                if (ModelCheck == null)
                                {
                                    await _ProductionOrderProductionPlanSemiRepository.AddAsync(ProductionOrderProductionPlanSemi);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        public virtual async Task<BaseResult<ProductionOrderProductionPlanSemi>> SyncProductionOrderProductionPlanSemiSumAsync(BaseParameter<ProductionOrderProductionPlanSemi> BaseParameter)
        {
            var result = new BaseResult<ProductionOrderProductionPlanSemi>();
            try
            {
                if (BaseParameter.BaseModel != null)
                {
                    if (BaseParameter.BaseModel.ID > 0)
                    {
                        if (BaseParameter.BaseModel.ParentID > 0)
                        {
                            if (BaseParameter.BaseModel.MaterialID01 > 0)
                            {
                                if (BaseParameter.BaseModel.MaterialID > 0)
                                {
                                    if (BaseParameter.BaseModel.SortOrder > 2)
                                    {
                                        var ListProductionOrderProductionPlanSemi = await GetByCondition(o => o.ParentID == BaseParameter.BaseModel.ParentID && o.MaterialID == BaseParameter.BaseModel.MaterialID).ToListAsync();
                                        var ProductionOrderProductionPlanSemi = await _ProductionOrderProductionPlanSemiRepository.GetByIDAsync(BaseParameter.BaseModel.ID);
                                        ProductionOrderProductionPlanSemi.ID = 0;
                                        ProductionOrderProductionPlanSemi.ProductionOrderProductionPlanID = 0;
                                        ProductionOrderProductionPlanSemi.MaterialID01 = 0;
                                        ProductionOrderProductionPlanSemi.MaterialCode01 = "";
                                        ProductionOrderProductionPlanSemi.MaterialName01 = "";
                                        ProductionOrderProductionPlanSemi.SortOrder = 2;
                                        int Index = 1;
                                        foreach (PropertyInfo ProductionOrderProductionPlanSemiPropertyInfo in ProductionOrderProductionPlanSemi.GetType().GetProperties())
                                        {
                                            string IndexName = Index.ToString();
                                            if (Index < 10)
                                            {
                                                IndexName = "0" + Index.ToString();
                                            }
                                            string QuantityNameString = "Quantity" + IndexName;

                                            if (ProductionOrderProductionPlanSemiPropertyInfo.Name == QuantityNameString)
                                            {
                                                try
                                                {
                                                    int? Quantity = 0;
                                                    foreach (var ProductionOrderProductionPlanSemiSub in ListProductionOrderProductionPlanSemi)
                                                    {
                                                        foreach (PropertyInfo ProductionOrderProductionPlanSemiSubPropertyInfo in ProductionOrderProductionPlanSemiSub.GetType().GetProperties())
                                                        {
                                                            if (ProductionOrderProductionPlanSemiSubPropertyInfo.Name == QuantityNameString && ProductionOrderProductionPlanSemiSubPropertyInfo.GetValue(ProductionOrderProductionPlanSemiSub) != null)
                                                            {
                                                                Quantity = Quantity + (int?)ProductionOrderProductionPlanSemiSubPropertyInfo.GetValue(ProductionOrderProductionPlanSemiSub);
                                                                break;
                                                            }
                                                        }
                                                    }
                                                    ProductionOrderProductionPlanSemiPropertyInfo.SetValue(ProductionOrderProductionPlanSemi, Quantity, null);
                                                }
                                                catch (Exception ex)
                                                {
                                                    string mes = ex.Message;
                                                }
                                                Index = Index + 1;
                                            }
                                        }
                                        var BaseParameterProductionOrderProductionPlanSemiSum = new BaseParameter<ProductionOrderProductionPlanSemi>();
                                        BaseParameterProductionOrderProductionPlanSemiSum.BaseModel = ProductionOrderProductionPlanSemi;
                                        await SaveAsync(BaseParameterProductionOrderProductionPlanSemiSum);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        public virtual async Task<BaseResult<ProductionOrderProductionPlanSemi>> SyncByQuantityActualAsync(BaseParameter<ProductionOrderProductionPlanSemi> BaseParameter)
        {
            var result = new BaseResult<ProductionOrderProductionPlanSemi>();
            try
            {
                if (BaseParameter.BaseModel != null)
                {
                    if (BaseParameter.ParentID > 0)
                    {
                        var ListProductionOrderProductionPlanSemi = await _ProductionOrderProductionPlanSemiRepository.GetByCondition(o => o.ParentID == BaseParameter.ParentID && o.SortOrder > 1).ToListAsync();
                        var ListProductionOrderProductionPlan = await _ProductionOrderProductionPlanRepository.GetByCondition(o => o.ParentID == BaseParameter.ParentID).ToListAsync();
                        if (ListProductionOrderProductionPlanSemi.Count > 0)
                        {
                            var CompanyID = ListProductionOrderProductionPlanSemi[0].CompanyID.Value;

                            var ListProductionOrderProductionPlanSemiMaterialCode = ListProductionOrderProductionPlanSemi.Select(o => o.MaterialCode).Distinct().ToList();

                            var ListMaterialCode = string.Join("','", ListProductionOrderProductionPlanSemiMaterialCode);
                            ListMaterialCode = "'" + ListMaterialCode + "'";
                            string MariaDBConectionString = GlobalHelper.MESConectionStringByCompanyID(CompanyID);
                            string sql = @"select * from trackmtim where RACKCODE NOT IN ('OUTPUT') AND LEAD_NM in (" + ListMaterialCode + ")";
                            DataSet ds = MySQLHelper.FillDataSetBySQL(MariaDBConectionString, sql);

                            var Listtrackmtim = new List<trackmtim>();
                            if (ds.Tables.Count > 0)
                            {
                                Listtrackmtim.AddRange(SQLHelper.ToList<trackmtim>(ds.Tables[0]));
                            }

                            for (int i = 0; i < ListProductionOrderProductionPlanSemi.Count; i++)
                            {
                                var ProductionOrderProductionPlan = ListProductionOrderProductionPlan.Where(o => o.ID == ListProductionOrderProductionPlanSemi[i].ProductionOrderProductionPlanID).FirstOrDefault();
                                if (ProductionOrderProductionPlan != null && ProductionOrderProductionPlan.ID > 0)
                                {
                                    ListProductionOrderProductionPlanSemi[i].Active = ProductionOrderProductionPlan.Active;
                                }
                                int Index = 1;
                                foreach (PropertyInfo proDate in ListProductionOrderProductionPlanSemi[i].GetType().GetProperties())
                                {
                                    string IndexName = Index.ToString();
                                    if (Index < 10)
                                    {
                                        IndexName = "0" + IndexName;
                                    }
                                    string DateString = "Date" + IndexName;
                                    string QuantityActualNameString = "QuantityActual" + IndexName;
                                    if (proDate.Name == DateString)
                                    {
                                        DateTime ProductionOrderProductionPlanSemiDateTime = (DateTime)proDate.GetValue(ListProductionOrderProductionPlanSemi[i]);
                                        if (ProductionOrderProductionPlanSemiDateTime != null)
                                        {
                                            if (ProductionOrderProductionPlanSemiDateTime.Date == GlobalHelper.InitializationDateTime.Date)
                                            {
                                                foreach (PropertyInfo proQuantityActual in ListProductionOrderProductionPlanSemi[i].GetType().GetProperties())
                                                {
                                                    if (proQuantityActual.Name == QuantityActualNameString)
                                                    {
                                                        int QuantityActual = 0;
                                                        var ProductionOrderProductionPlanSemiCheck = ListProductionOrderProductionPlanSemi.Where(o => o.Active == true && o.MaterialCode == ListProductionOrderProductionPlanSemi[i].MaterialCode).OrderBy(o => o.ID).FirstOrDefault();
                                                        if (ProductionOrderProductionPlanSemiCheck == null)
                                                        {
                                                            ProductionOrderProductionPlanSemiCheck = new ProductionOrderProductionPlanSemi();
                                                        }
                                                        if (ListProductionOrderProductionPlanSemi[i].ID == ProductionOrderProductionPlanSemiCheck.ID)
                                                        {
                                                            //string MariaDBConectionString = GlobalHelper.MESConectionStringByCompanyID(ListProductionOrderProductionPlanSemi[i].CompanyID.Value);
                                                            //string sql = @"select * from trackmtim where RACKCODE NOT IN ('OUTPUT') AND LEAD_NM='" + ListProductionOrderProductionPlanSemi[i].MaterialCode + "'";
                                                            //DataSet ds = MySQLHelper.FillDataSetBySQL(MariaDBConectionString, sql);

                                                            //var Listtrackmtim = new List<trackmtim>();
                                                            //if (ds.Tables.Count > 0)
                                                            //{
                                                            //    Listtrackmtim.AddRange(SQLHelper.ToList<trackmtim>(ds.Tables[0]));
                                                            //}
                                                            var ListtrackmtimSub = Listtrackmtim.Where(o => o.LEAD_NM == ListProductionOrderProductionPlanSemi[i].MaterialCode).ToList();
                                                            QuantityActual = ListtrackmtimSub.Sum(o => (int)(o.QTY ?? 0));
                                                        }
                                                        proQuantityActual.SetValue(ListProductionOrderProductionPlanSemi[i], QuantityActual, null);
                                                    }
                                                }
                                            }
                                        }
                                        Index = Index + 1;
                                    }
                                }
                                SQLHelper.InitializationQuantityGAP<ProductionOrderProductionPlanSemi>(ListProductionOrderProductionPlanSemi[i]);
                                //SQLHelper.InitializationQuantity00<ProductionOrderProductionPlanSemi>(ListProductionOrderProductionPlanSemi[i]);
                                //SQLHelper.InitializationQuantityActual00<ProductionOrderProductionPlanSemi>(ListProductionOrderProductionPlanSemi[i]);
                                //ListProductionOrderProductionPlanSemi[i].QuantityGAP00 = ListProductionOrderProductionPlanSemi[i].Quantity00 - ListProductionOrderProductionPlanSemi[i].QuantityActual00;
                            }
                            await _ProductionOrderProductionPlanSemiRepository.UpdateRangeAsync(ListProductionOrderProductionPlanSemi);

                            var ListProductionOrderProductionPlanSemiSPST = ListProductionOrderProductionPlanSemi.Where(o => o.IsSPST == true).ToList();
                            var ListProductionOrderProductionPlanSemiLeadNo = ListProductionOrderProductionPlanSemi.Where(o => o.IsLeadNo == true).ToList();
                            var ListProductionOrderProductionPlanSemiBOMID = ListProductionOrderProductionPlanSemi.Select(o => o.BOMID).Distinct().ToList();
                            var ListBOM = await _BOMRepository.GetByCondition(o => ListProductionOrderProductionPlanSemiBOMID.Contains(o.ID) && o.IsLeadNo == true).ToListAsync();
                            for (int i = 0; i < ListProductionOrderProductionPlanSemiSPST.Count; i++)
                            {
                                int Index = 1;
                                foreach (PropertyInfo proDate in ListProductionOrderProductionPlanSemiSPST[i].GetType().GetProperties())
                                {
                                    string IndexName = Index.ToString();
                                    if (Index < 10)
                                    {
                                        IndexName = "0" + IndexName;
                                    }
                                    string DateName = "Date" + IndexName;
                                    string QuantityName = "Quantity" + IndexName;
                                    string QuantityActualName = "QuantityActual" + IndexName;
                                    if (proDate.Name == DateName)
                                    {
                                        DateTime ProductionOrderProductionPlanSemiDateTime = (DateTime)proDate.GetValue(ListProductionOrderProductionPlanSemiSPST[i]);
                                        if (ProductionOrderProductionPlanSemiDateTime != null)
                                        {
                                            if (ProductionOrderProductionPlanSemiDateTime.Date == GlobalHelper.InitializationDateTime.Date)
                                            {
                                                foreach (PropertyInfo proQuantityActual in ListProductionOrderProductionPlanSemiSPST[i].GetType().GetProperties())
                                                {
                                                    if (proQuantityActual.Name == QuantityActualName)
                                                    {
                                                        int QuantityActual = (int)proQuantityActual.GetValue(ListProductionOrderProductionPlanSemiSPST[i]);
                                                        if (QuantityActual > 0)
                                                        {
                                                            var ListBOMSub = ListBOM.Where(o => (o.ParentID01 == ListProductionOrderProductionPlanSemiSPST[i].BOMID) || (o.ParentID02 == ListProductionOrderProductionPlanSemiSPST[i].BOMID) || (o.ParentID03 == ListProductionOrderProductionPlanSemiSPST[i].BOMID) || (o.ParentID04 == ListProductionOrderProductionPlanSemiSPST[i].BOMID)).ToList();
                                                            if (ListBOMSub.Count > 0)
                                                            {
                                                                var ListBOMSubMaterialCode = ListBOMSub.Select(o => o.MaterialCode).Distinct().ToList();
                                                                var ListProductionOrderProductionPlanSemiLeadNoSub = ListProductionOrderProductionPlanSemiLeadNo.Where(o => ListBOMSubMaterialCode.Contains(o.MaterialCode)).ToList();
                                                                for (int j = 0; j < ListProductionOrderProductionPlanSemiLeadNoSub.Count; j++)
                                                                {
                                                                    foreach (PropertyInfo proQuantity in ListProductionOrderProductionPlanSemiLeadNoSub[j].GetType().GetProperties())
                                                                    {
                                                                        if (proQuantity.Name == QuantityName)
                                                                        {
                                                                            int Quantity = (int)proQuantity.GetValue(ListProductionOrderProductionPlanSemiLeadNoSub[j]);
                                                                            if (Quantity > 0)
                                                                            {
                                                                                foreach (PropertyInfo proQuantityActualLeadNo in ListProductionOrderProductionPlanSemiLeadNoSub[j].GetType().GetProperties())
                                                                                {
                                                                                    if (proQuantityActualLeadNo.Name == QuantityActualName)
                                                                                    {
                                                                                        int QuantityActualLeadNo = (int)proQuantityActualLeadNo.GetValue(ListProductionOrderProductionPlanSemiLeadNoSub[j]);
                                                                                        QuantityActualLeadNo = QuantityActualLeadNo + QuantityActual;
                                                                                        proQuantityActualLeadNo.SetValue(ListProductionOrderProductionPlanSemiLeadNoSub[j], QuantityActualLeadNo, null);
                                                                                        break;
                                                                                    }
                                                                                }
                                                                            }
                                                                            break;
                                                                        }
                                                                    }
                                                                    SQLHelper.InitializationQuantityGAP<ProductionOrderProductionPlanSemi>(ListProductionOrderProductionPlanSemiLeadNoSub[j]);
                                                                    //SQLHelper.InitializationQuantity00<ProductionOrderProductionPlanSemi>(ListProductionOrderProductionPlanSemiLeadNoSub[j]);
                                                                    //SQLHelper.InitializationQuantityActual00<ProductionOrderProductionPlanSemi>(ListProductionOrderProductionPlanSemiLeadNoSub[j]);
                                                                }
                                                                await _ProductionOrderProductionPlanSemiRepository.UpdateRangeAsync(ListProductionOrderProductionPlanSemiLeadNoSub);
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        Index = Index + 1;
                                    }
                                }
                            }

                            for (int i = 0; i < ListProductionOrderProductionPlanSemi.Count; i++)
                            {
                                ProductionOrderProductionPlanSemiSetQuantityGAP(ListProductionOrderProductionPlanSemi[i]);
                                SQLHelper.InitializationQuantity00<ProductionOrderProductionPlanSemi>(ListProductionOrderProductionPlanSemi[i]);
                                SQLHelper.InitializationQuantityActual00<ProductionOrderProductionPlanSemi>(ListProductionOrderProductionPlanSemi[i]);
                                ListProductionOrderProductionPlanSemi[i].QuantityGAP00 = ListProductionOrderProductionPlanSemi[i].Quantity00 - ListProductionOrderProductionPlanSemi[i].QuantityActual00;
                            }
                            await _ProductionOrderProductionPlanSemiRepository.UpdateRangeAsync(ListProductionOrderProductionPlanSemi);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        private void ProductionOrderProductionPlanSemiSetQuantityGAP(ProductionOrderProductionPlanSemi model)
        {
            var Index01 = 1;
            foreach (PropertyInfo ProductionOrderProductionPlanSemiPropertyInfo in model.GetType().GetProperties())
            {

                string IndexName = Index01.ToString();
                if (Index01 < 10)
                {
                    IndexName = "0" + Index01.ToString();
                }
                string QuantityGAPName = "QuantityGAP" + IndexName;
                if (ProductionOrderProductionPlanSemiPropertyInfo.Name == QuantityGAPName)
                {
                    int QuantityGAP = 0;
                    if (ProductionOrderProductionPlanSemiPropertyInfo.GetValue(model) != null)
                    {
                        QuantityGAP = (int)ProductionOrderProductionPlanSemiPropertyInfo.GetValue(model);
                    }
                    if (Index01 > 1)
                    {
                        var Index02 = Index01 - 1;
                        IndexName = Index02.ToString();
                        if (Index02 < 10)
                        {
                            IndexName = "0" + Index02.ToString();
                        }
                        QuantityGAPName = "QuantityGAP" + IndexName;
                        foreach (PropertyInfo ProductionOrderProductionPlanSemiPropertyInfoSub in model.GetType().GetProperties())
                        {
                            if (ProductionOrderProductionPlanSemiPropertyInfoSub.Name == QuantityGAPName)
                            {
                                if (ProductionOrderProductionPlanSemiPropertyInfoSub.GetValue(model) != null)
                                {
                                    QuantityGAP = QuantityGAP + (int)ProductionOrderProductionPlanSemiPropertyInfoSub.GetValue(model);
                                    break;
                                }
                            }
                        }
                    }
                    ProductionOrderProductionPlanSemiPropertyInfo.SetValue(model, QuantityGAP, null);
                    Index01 = Index01 + 1;
                }
            }
        }
        public override async Task<BaseResult<ProductionOrderProductionPlanSemi>> GetByParentIDToListAsync(BaseParameter<ProductionOrderProductionPlanSemi> BaseParameter)
        {
            var result = new BaseResult<ProductionOrderProductionPlanSemi>();
            if (BaseParameter.ParentID > 0)
            {
                result.List = await GetByCondition(o => o.ParentID == BaseParameter.ParentID).ToListAsync();
            }
            return result;
        }
        public virtual async Task<BaseResult<ProductionOrderProductionPlanSemi>> GetByParentIDAndSearchStringToListAsync(BaseParameter<ProductionOrderProductionPlanSemi> BaseParameter)
        {
            var result = new BaseResult<ProductionOrderProductionPlanSemi>();
            result.List = new List<ProductionOrderProductionPlanSemi>();
            if (BaseParameter.ParentID > 0)
            {
                var ListAll = await GetByCondition(o => o.ParentID == BaseParameter.ParentID).ToListAsync();
                var List = ListAll;
                if (!string.IsNullOrEmpty(BaseParameter.SearchString))
                {
                    List = ListAll.Where(o => !string.IsNullOrEmpty(o.MaterialCode) && o.MaterialCode.Contains(BaseParameter.SearchString)).ToList();
                    if (List.Count == 0)
                    {
                        List = ListAll.Where(o => !string.IsNullOrEmpty(o.MaterialCode01) && o.MaterialCode01.Contains(BaseParameter.SearchString)).ToList();
                    }
                    if (List.Count == 0)
                    {
                        List = ListAll.Where(o => !string.IsNullOrEmpty(o.ParentName01) && o.ParentName01.Contains(BaseParameter.SearchString)).ToList();
                    }
                    if (List.Count == 0)
                    {
                        List = ListAll.Where(o => !string.IsNullOrEmpty(o.ParentName02) && o.ParentName02.Contains(BaseParameter.SearchString)).ToList();
                    }
                    if (List.Count == 0)
                    {
                        List = ListAll.Where(o => !string.IsNullOrEmpty(o.ParentName03) && o.ParentName03.Contains(BaseParameter.SearchString)).ToList();
                    }
                    if (List.Count == 0)
                    {
                        List = ListAll.Where(o => !string.IsNullOrEmpty(o.ParentName04) && o.ParentName04.Contains(BaseParameter.SearchString)).ToList();
                    }
                }
                result.List = List;
                ProductionOrderProductionPlanSemi ProductionOrderProductionPlanSemiSum = new ProductionOrderProductionPlanSemi();
                ProductionOrderProductionPlanSemiSum.MaterialCode = "Sum";
                ProductionOrderProductionPlanSemiSum.Priority = 0;
                ProductionOrderProductionPlanSemiSum.Quantity00 = result.List.Sum(o => o.Quantity00);
                ProductionOrderProductionPlanSemiSum.QuantityActual00 = result.List.Sum(o => o.QuantityActual00);
                ProductionOrderProductionPlanSemiSum.QuantityGAP00 = result.List.Sum(o => o.QuantityGAP00);

                ProductionOrderProductionPlanSemiSum.Quantity01 = result.List.Sum(o => o.Quantity01);
                ProductionOrderProductionPlanSemiSum.QuantityActual01 = result.List.Sum(o => o.QuantityActual01);
                ProductionOrderProductionPlanSemiSum.QuantityGAP01 = result.List.Sum(o => o.QuantityGAP01);

                ProductionOrderProductionPlanSemiSum.Quantity02 = result.List.Sum(o => o.Quantity02);
                ProductionOrderProductionPlanSemiSum.QuantityActual02 = result.List.Sum(o => o.QuantityActual02);
                ProductionOrderProductionPlanSemiSum.QuantityGAP02 = result.List.Sum(o => o.QuantityGAP02);

                ProductionOrderProductionPlanSemiSum.Quantity03 = result.List.Sum(o => o.Quantity03);
                ProductionOrderProductionPlanSemiSum.QuantityActual03 = result.List.Sum(o => o.QuantityActual03);
                ProductionOrderProductionPlanSemiSum.QuantityGAP03 = result.List.Sum(o => o.QuantityGAP03);

                ProductionOrderProductionPlanSemiSum.Quantity04 = result.List.Sum(o => o.Quantity04);
                ProductionOrderProductionPlanSemiSum.QuantityActual04 = result.List.Sum(o => o.QuantityActual04);
                ProductionOrderProductionPlanSemiSum.QuantityGAP04 = result.List.Sum(o => o.QuantityGAP04);

                ProductionOrderProductionPlanSemiSum.Quantity05 = result.List.Sum(o => o.Quantity05);
                ProductionOrderProductionPlanSemiSum.QuantityActual05 = result.List.Sum(o => o.QuantityActual05);
                ProductionOrderProductionPlanSemiSum.QuantityGAP05 = result.List.Sum(o => o.QuantityGAP05);

                ProductionOrderProductionPlanSemiSum.Quantity06 = result.List.Sum(o => o.Quantity06);
                ProductionOrderProductionPlanSemiSum.QuantityActual06 = result.List.Sum(o => o.QuantityActual06);
                ProductionOrderProductionPlanSemiSum.QuantityGAP06 = result.List.Sum(o => o.QuantityGAP06);

                ProductionOrderProductionPlanSemiSum.Quantity07 = result.List.Sum(o => o.Quantity07);
                ProductionOrderProductionPlanSemiSum.QuantityActual07 = result.List.Sum(o => o.QuantityActual07);
                ProductionOrderProductionPlanSemiSum.QuantityGAP07 = result.List.Sum(o => o.QuantityGAP07);

                ProductionOrderProductionPlanSemiSum.Quantity08 = result.List.Sum(o => o.Quantity08);
                ProductionOrderProductionPlanSemiSum.QuantityActual08 = result.List.Sum(o => o.QuantityActual08);
                ProductionOrderProductionPlanSemiSum.QuantityGAP08 = result.List.Sum(o => o.QuantityGAP08);

                ProductionOrderProductionPlanSemiSum.Quantity09 = result.List.Sum(o => o.Quantity09);
                ProductionOrderProductionPlanSemiSum.QuantityActual09 = result.List.Sum(o => o.QuantityActual09);
                ProductionOrderProductionPlanSemiSum.QuantityGAP09 = result.List.Sum(o => o.QuantityGAP09);

                ProductionOrderProductionPlanSemiSum.Quantity10 = result.List.Sum(o => o.Quantity10);
                ProductionOrderProductionPlanSemiSum.QuantityActual10 = result.List.Sum(o => o.QuantityActual10);
                ProductionOrderProductionPlanSemiSum.QuantityGAP10 = result.List.Sum(o => o.QuantityGAP10);

                ProductionOrderProductionPlanSemiSum.Quantity11 = result.List.Sum(o => o.Quantity11);
                ProductionOrderProductionPlanSemiSum.QuantityActual11 = result.List.Sum(o => o.QuantityActual11);
                ProductionOrderProductionPlanSemiSum.QuantityGAP11 = result.List.Sum(o => o.QuantityGAP11);

                ProductionOrderProductionPlanSemiSum.Quantity12 = result.List.Sum(o => o.Quantity12);
                ProductionOrderProductionPlanSemiSum.QuantityActual12 = result.List.Sum(o => o.QuantityActual12);
                ProductionOrderProductionPlanSemiSum.QuantityGAP12 = result.List.Sum(o => o.QuantityGAP12);

                ProductionOrderProductionPlanSemiSum.Quantity13 = result.List.Sum(o => o.Quantity13);
                ProductionOrderProductionPlanSemiSum.QuantityActual13 = result.List.Sum(o => o.QuantityActual13);
                ProductionOrderProductionPlanSemiSum.QuantityGAP13 = result.List.Sum(o => o.QuantityGAP13);

                ProductionOrderProductionPlanSemiSum.Quantity14 = result.List.Sum(o => o.Quantity14);
                ProductionOrderProductionPlanSemiSum.QuantityActual14 = result.List.Sum(o => o.QuantityActual14);
                ProductionOrderProductionPlanSemiSum.QuantityGAP14 = result.List.Sum(o => o.QuantityGAP14);

                ProductionOrderProductionPlanSemiSum.Quantity15 = result.List.Sum(o => o.Quantity15);
                ProductionOrderProductionPlanSemiSum.QuantityActual15 = result.List.Sum(o => o.QuantityActual15);
                ProductionOrderProductionPlanSemiSum.QuantityGAP15 = result.List.Sum(o => o.QuantityGAP15);

                ProductionOrderProductionPlanSemiSum.Quantity16 = result.List.Sum(o => o.Quantity16);
                ProductionOrderProductionPlanSemiSum.QuantityActual16 = result.List.Sum(o => o.QuantityActual16);
                ProductionOrderProductionPlanSemiSum.QuantityGAP16 = result.List.Sum(o => o.QuantityGAP16);

                ProductionOrderProductionPlanSemiSum.Quantity17 = result.List.Sum(o => o.Quantity17);
                ProductionOrderProductionPlanSemiSum.QuantityActual17 = result.List.Sum(o => o.QuantityActual17);
                ProductionOrderProductionPlanSemiSum.QuantityGAP17 = result.List.Sum(o => o.QuantityGAP17);

                ProductionOrderProductionPlanSemiSum.Quantity18 = result.List.Sum(o => o.Quantity18);
                ProductionOrderProductionPlanSemiSum.QuantityActual18 = result.List.Sum(o => o.QuantityActual18);
                ProductionOrderProductionPlanSemiSum.QuantityGAP18 = result.List.Sum(o => o.QuantityGAP18);

                ProductionOrderProductionPlanSemiSum.Quantity19 = result.List.Sum(o => o.Quantity19);
                ProductionOrderProductionPlanSemiSum.QuantityActual19 = result.List.Sum(o => o.QuantityActual19);
                ProductionOrderProductionPlanSemiSum.QuantityGAP19 = result.List.Sum(o => o.QuantityGAP19);

                ProductionOrderProductionPlanSemiSum.Quantity20 = result.List.Sum(o => o.Quantity20);
                ProductionOrderProductionPlanSemiSum.QuantityActual20 = result.List.Sum(o => o.QuantityActual20);
                ProductionOrderProductionPlanSemiSum.QuantityGAP20 = result.List.Sum(o => o.QuantityGAP20);

                ProductionOrderProductionPlanSemiSum.Quantity21 = result.List.Sum(o => o.Quantity21);
                ProductionOrderProductionPlanSemiSum.QuantityActual21 = result.List.Sum(o => o.QuantityActual21);
                ProductionOrderProductionPlanSemiSum.QuantityGAP21 = result.List.Sum(o => o.QuantityGAP21);

                ProductionOrderProductionPlanSemiSum.Quantity22 = result.List.Sum(o => o.Quantity22);
                ProductionOrderProductionPlanSemiSum.QuantityActual22 = result.List.Sum(o => o.QuantityActual22);
                ProductionOrderProductionPlanSemiSum.QuantityGAP22 = result.List.Sum(o => o.QuantityGAP22);

                ProductionOrderProductionPlanSemiSum.Quantity23 = result.List.Sum(o => o.Quantity23);
                ProductionOrderProductionPlanSemiSum.QuantityActual23 = result.List.Sum(o => o.QuantityActual23);
                ProductionOrderProductionPlanSemiSum.QuantityGAP23 = result.List.Sum(o => o.QuantityGAP23);

                ProductionOrderProductionPlanSemiSum.Quantity24 = result.List.Sum(o => o.Quantity24);
                ProductionOrderProductionPlanSemiSum.QuantityActual24 = result.List.Sum(o => o.QuantityActual24);
                ProductionOrderProductionPlanSemiSum.QuantityGAP24 = result.List.Sum(o => o.QuantityGAP24);

                ProductionOrderProductionPlanSemiSum.Quantity25 = result.List.Sum(o => o.Quantity25);
                ProductionOrderProductionPlanSemiSum.QuantityActual25 = result.List.Sum(o => o.QuantityActual25);
                ProductionOrderProductionPlanSemiSum.QuantityGAP25 = result.List.Sum(o => o.QuantityGAP25);

                ProductionOrderProductionPlanSemiSum.Quantity26 = result.List.Sum(o => o.Quantity26);
                ProductionOrderProductionPlanSemiSum.QuantityActual26 = result.List.Sum(o => o.QuantityActual26);
                ProductionOrderProductionPlanSemiSum.QuantityGAP26 = result.List.Sum(o => o.QuantityGAP26);

                ProductionOrderProductionPlanSemiSum.Quantity27 = result.List.Sum(o => o.Quantity27);
                ProductionOrderProductionPlanSemiSum.QuantityActual27 = result.List.Sum(o => o.QuantityActual27);
                ProductionOrderProductionPlanSemiSum.QuantityGAP27 = result.List.Sum(o => o.QuantityGAP27);

                ProductionOrderProductionPlanSemiSum.Quantity28 = result.List.Sum(o => o.Quantity28);
                ProductionOrderProductionPlanSemiSum.QuantityActual28 = result.List.Sum(o => o.QuantityActual28);
                ProductionOrderProductionPlanSemiSum.QuantityGAP28 = result.List.Sum(o => o.QuantityGAP28);

                ProductionOrderProductionPlanSemiSum.Quantity29 = result.List.Sum(o => o.Quantity29);
                ProductionOrderProductionPlanSemiSum.QuantityActual29 = result.List.Sum(o => o.QuantityActual29);
                ProductionOrderProductionPlanSemiSum.QuantityGAP29 = result.List.Sum(o => o.QuantityGAP29);

                ProductionOrderProductionPlanSemiSum.Quantity30 = result.List.Sum(o => o.Quantity30);
                ProductionOrderProductionPlanSemiSum.QuantityActual30 = result.List.Sum(o => o.QuantityActual30);
                ProductionOrderProductionPlanSemiSum.QuantityGAP30 = result.List.Sum(o => o.QuantityGAP30);

                ProductionOrderProductionPlanSemiSum.Quantity31 = result.List.Sum(o => o.Quantity31);
                ProductionOrderProductionPlanSemiSum.QuantityActual31 = result.List.Sum(o => o.QuantityActual31);
                ProductionOrderProductionPlanSemiSum.QuantityGAP31 = result.List.Sum(o => o.QuantityGAP31);

                ProductionOrderProductionPlanSemiSum.Quantity32 = result.List.Sum(o => o.Quantity32);
                ProductionOrderProductionPlanSemiSum.QuantityActual32 = result.List.Sum(o => o.QuantityActual32);
                ProductionOrderProductionPlanSemiSum.QuantityGAP32 = result.List.Sum(o => o.QuantityGAP32);

                ProductionOrderProductionPlanSemiSum.Quantity33 = result.List.Sum(o => o.Quantity33);
                ProductionOrderProductionPlanSemiSum.QuantityActual33 = result.List.Sum(o => o.QuantityActual33);
                ProductionOrderProductionPlanSemiSum.QuantityGAP33 = result.List.Sum(o => o.QuantityGAP33);

                ProductionOrderProductionPlanSemiSum.Quantity34 = result.List.Sum(o => o.Quantity34);
                ProductionOrderProductionPlanSemiSum.QuantityActual34 = result.List.Sum(o => o.QuantityActual34);
                ProductionOrderProductionPlanSemiSum.QuantityGAP34 = result.List.Sum(o => o.QuantityGAP34);

                ProductionOrderProductionPlanSemiSum.Quantity35 = result.List.Sum(o => o.Quantity35);
                ProductionOrderProductionPlanSemiSum.QuantityActual35 = result.List.Sum(o => o.QuantityActual35);
                ProductionOrderProductionPlanSemiSum.QuantityGAP35 = result.List.Sum(o => o.QuantityGAP35);

                ProductionOrderProductionPlanSemiSum.Quantity36 = result.List.Sum(o => o.Quantity36);
                ProductionOrderProductionPlanSemiSum.QuantityActual36 = result.List.Sum(o => o.QuantityActual36);
                ProductionOrderProductionPlanSemiSum.QuantityGAP36 = result.List.Sum(o => o.QuantityGAP36);

                ProductionOrderProductionPlanSemiSum.Quantity37 = result.List.Sum(o => o.Quantity37);
                ProductionOrderProductionPlanSemiSum.QuantityActual37 = result.List.Sum(o => o.QuantityActual37);
                ProductionOrderProductionPlanSemiSum.QuantityGAP37 = result.List.Sum(o => o.QuantityGAP37);

                ProductionOrderProductionPlanSemiSum.Quantity38 = result.List.Sum(o => o.Quantity38);
                ProductionOrderProductionPlanSemiSum.QuantityActual38 = result.List.Sum(o => o.QuantityActual38);
                ProductionOrderProductionPlanSemiSum.QuantityGAP38 = result.List.Sum(o => o.QuantityGAP38);

                ProductionOrderProductionPlanSemiSum.Quantity39 = result.List.Sum(o => o.Quantity39);
                ProductionOrderProductionPlanSemiSum.QuantityActual39 = result.List.Sum(o => o.QuantityActual39);
                ProductionOrderProductionPlanSemiSum.QuantityGAP39 = result.List.Sum(o => o.QuantityGAP39);

                ProductionOrderProductionPlanSemiSum.Quantity40 = result.List.Sum(o => o.Quantity40);
                ProductionOrderProductionPlanSemiSum.QuantityActual40 = result.List.Sum(o => o.QuantityActual40);
                ProductionOrderProductionPlanSemiSum.QuantityGAP40 = result.List.Sum(o => o.QuantityGAP40);

                ProductionOrderProductionPlanSemiSum.Quantity41 = result.List.Sum(o => o.Quantity41);
                ProductionOrderProductionPlanSemiSum.QuantityActual41 = result.List.Sum(o => o.QuantityActual41);
                ProductionOrderProductionPlanSemiSum.QuantityGAP41 = result.List.Sum(o => o.QuantityGAP41);

                ProductionOrderProductionPlanSemiSum.Quantity42 = result.List.Sum(o => o.Quantity42);
                ProductionOrderProductionPlanSemiSum.QuantityActual42 = result.List.Sum(o => o.QuantityActual42);
                ProductionOrderProductionPlanSemiSum.QuantityGAP42 = result.List.Sum(o => o.QuantityGAP42);

                ProductionOrderProductionPlanSemiSum.Quantity43 = result.List.Sum(o => o.Quantity43);
                ProductionOrderProductionPlanSemiSum.QuantityActual43 = result.List.Sum(o => o.QuantityActual43);
                ProductionOrderProductionPlanSemiSum.QuantityGAP43 = result.List.Sum(o => o.QuantityGAP43);

                ProductionOrderProductionPlanSemiSum.Quantity44 = result.List.Sum(o => o.Quantity44);
                ProductionOrderProductionPlanSemiSum.QuantityActual44 = result.List.Sum(o => o.QuantityActual44);
                ProductionOrderProductionPlanSemiSum.QuantityGAP44 = result.List.Sum(o => o.QuantityGAP44);

                ProductionOrderProductionPlanSemiSum.Quantity45 = result.List.Sum(o => o.Quantity45);
                ProductionOrderProductionPlanSemiSum.QuantityActual45 = result.List.Sum(o => o.QuantityActual45);
                ProductionOrderProductionPlanSemiSum.QuantityGAP45 = result.List.Sum(o => o.QuantityGAP45);

                ProductionOrderProductionPlanSemiSum.Quantity46 = result.List.Sum(o => o.Quantity46);
                ProductionOrderProductionPlanSemiSum.QuantityActual46 = result.List.Sum(o => o.QuantityActual46);
                ProductionOrderProductionPlanSemiSum.QuantityGAP46 = result.List.Sum(o => o.QuantityGAP46);

                ProductionOrderProductionPlanSemiSum.Quantity47 = result.List.Sum(o => o.Quantity47);
                ProductionOrderProductionPlanSemiSum.QuantityActual47 = result.List.Sum(o => o.QuantityActual47);
                ProductionOrderProductionPlanSemiSum.QuantityGAP47 = result.List.Sum(o => o.QuantityGAP47);

                ProductionOrderProductionPlanSemiSum.Quantity48 = result.List.Sum(o => o.Quantity48);
                ProductionOrderProductionPlanSemiSum.QuantityActual48 = result.List.Sum(o => o.QuantityActual48);
                ProductionOrderProductionPlanSemiSum.QuantityGAP48 = result.List.Sum(o => o.QuantityGAP48);

                ProductionOrderProductionPlanSemiSum.Quantity49 = result.List.Sum(o => o.Quantity49);
                ProductionOrderProductionPlanSemiSum.QuantityActual49 = result.List.Sum(o => o.QuantityActual49);
                ProductionOrderProductionPlanSemiSum.QuantityGAP49 = result.List.Sum(o => o.QuantityGAP49);

                ProductionOrderProductionPlanSemiSum.Quantity50 = result.List.Sum(o => o.Quantity50);
                ProductionOrderProductionPlanSemiSum.QuantityActual50 = result.List.Sum(o => o.QuantityActual50);
                ProductionOrderProductionPlanSemiSum.QuantityGAP50 = result.List.Sum(o => o.QuantityGAP50);

                ProductionOrderProductionPlanSemiSum.Quantity51 = result.List.Sum(o => o.Quantity51);
                ProductionOrderProductionPlanSemiSum.QuantityActual51 = result.List.Sum(o => o.QuantityActual51);
                ProductionOrderProductionPlanSemiSum.QuantityGAP51 = result.List.Sum(o => o.QuantityGAP51);

                ProductionOrderProductionPlanSemiSum.Quantity52 = result.List.Sum(o => o.Quantity52);
                ProductionOrderProductionPlanSemiSum.QuantityActual52 = result.List.Sum(o => o.QuantityActual52);
                ProductionOrderProductionPlanSemiSum.QuantityGAP52 = result.List.Sum(o => o.QuantityGAP52);

                ProductionOrderProductionPlanSemiSum.Quantity53 = result.List.Sum(o => o.Quantity53);
                ProductionOrderProductionPlanSemiSum.QuantityActual53 = result.List.Sum(o => o.QuantityActual53);
                ProductionOrderProductionPlanSemiSum.QuantityGAP53 = result.List.Sum(o => o.QuantityGAP53);

                ProductionOrderProductionPlanSemiSum.Quantity54 = result.List.Sum(o => o.Quantity54);
                ProductionOrderProductionPlanSemiSum.QuantityActual54 = result.List.Sum(o => o.QuantityActual54);
                ProductionOrderProductionPlanSemiSum.QuantityGAP54 = result.List.Sum(o => o.QuantityGAP54);

                ProductionOrderProductionPlanSemiSum.Quantity55 = result.List.Sum(o => o.Quantity55);
                ProductionOrderProductionPlanSemiSum.QuantityActual55 = result.List.Sum(o => o.QuantityActual55);
                ProductionOrderProductionPlanSemiSum.QuantityGAP55 = result.List.Sum(o => o.QuantityGAP55);

                ProductionOrderProductionPlanSemiSum.Quantity56 = result.List.Sum(o => o.Quantity56);
                ProductionOrderProductionPlanSemiSum.QuantityActual56 = result.List.Sum(o => o.QuantityActual56);
                ProductionOrderProductionPlanSemiSum.QuantityGAP56 = result.List.Sum(o => o.QuantityGAP56);

                ProductionOrderProductionPlanSemiSum.Quantity57 = result.List.Sum(o => o.Quantity57);
                ProductionOrderProductionPlanSemiSum.QuantityActual57 = result.List.Sum(o => o.QuantityActual57);
                ProductionOrderProductionPlanSemiSum.QuantityGAP57 = result.List.Sum(o => o.QuantityGAP57);

                ProductionOrderProductionPlanSemiSum.Quantity58 = result.List.Sum(o => o.Quantity58);
                ProductionOrderProductionPlanSemiSum.QuantityActual58 = result.List.Sum(o => o.QuantityActual58);
                ProductionOrderProductionPlanSemiSum.QuantityGAP58 = result.List.Sum(o => o.QuantityGAP58);

                ProductionOrderProductionPlanSemiSum.Quantity59 = result.List.Sum(o => o.Quantity59);
                ProductionOrderProductionPlanSemiSum.QuantityActual59 = result.List.Sum(o => o.QuantityActual59);
                ProductionOrderProductionPlanSemiSum.QuantityGAP59 = result.List.Sum(o => o.QuantityGAP59);

                ProductionOrderProductionPlanSemiSum.Quantity60 = result.List.Sum(o => o.Quantity60);
                ProductionOrderProductionPlanSemiSum.QuantityActual60 = result.List.Sum(o => o.QuantityActual60);
                ProductionOrderProductionPlanSemiSum.QuantityGAP60 = result.List.Sum(o => o.QuantityGAP60);

                ProductionOrderProductionPlanSemiSum.Quantity61 = result.List.Sum(o => o.Quantity61);
                ProductionOrderProductionPlanSemiSum.QuantityActual61 = result.List.Sum(o => o.QuantityActual61);
                ProductionOrderProductionPlanSemiSum.QuantityGAP61 = result.List.Sum(o => o.QuantityGAP61);

                ProductionOrderProductionPlanSemiSum.Quantity62 = result.List.Sum(o => o.Quantity62);
                ProductionOrderProductionPlanSemiSum.QuantityActual62 = result.List.Sum(o => o.QuantityActual62);
                ProductionOrderProductionPlanSemiSum.QuantityGAP62 = result.List.Sum(o => o.QuantityGAP62);

                ProductionOrderProductionPlanSemiSum.Quantity63 = result.List.Sum(o => o.Quantity63);
                ProductionOrderProductionPlanSemiSum.QuantityActual63 = result.List.Sum(o => o.QuantityActual63);
                ProductionOrderProductionPlanSemiSum.QuantityGAP63 = result.List.Sum(o => o.QuantityGAP63);

                ProductionOrderProductionPlanSemiSum.Quantity64 = result.List.Sum(o => o.Quantity64);
                ProductionOrderProductionPlanSemiSum.QuantityActual64 = result.List.Sum(o => o.QuantityActual64);
                ProductionOrderProductionPlanSemiSum.QuantityGAP64 = result.List.Sum(o => o.QuantityGAP64);

                ProductionOrderProductionPlanSemiSum.Quantity65 = result.List.Sum(o => o.Quantity65);
                ProductionOrderProductionPlanSemiSum.QuantityActual65 = result.List.Sum(o => o.QuantityActual65);
                ProductionOrderProductionPlanSemiSum.QuantityGAP65 = result.List.Sum(o => o.QuantityGAP65);

                ProductionOrderProductionPlanSemiSum.Quantity66 = result.List.Sum(o => o.Quantity66);
                ProductionOrderProductionPlanSemiSum.QuantityActual66 = result.List.Sum(o => o.QuantityActual66);
                ProductionOrderProductionPlanSemiSum.QuantityGAP66 = result.List.Sum(o => o.QuantityGAP66);

                ProductionOrderProductionPlanSemiSum.Quantity67 = result.List.Sum(o => o.Quantity67);
                ProductionOrderProductionPlanSemiSum.QuantityActual67 = result.List.Sum(o => o.QuantityActual67);
                ProductionOrderProductionPlanSemiSum.QuantityGAP67 = result.List.Sum(o => o.QuantityGAP67);

                ProductionOrderProductionPlanSemiSum.Quantity68 = result.List.Sum(o => o.Quantity68);
                ProductionOrderProductionPlanSemiSum.QuantityActual68 = result.List.Sum(o => o.QuantityActual68);
                ProductionOrderProductionPlanSemiSum.QuantityGAP68 = result.List.Sum(o => o.QuantityGAP68);

                ProductionOrderProductionPlanSemiSum.Quantity69 = result.List.Sum(o => o.Quantity69);
                ProductionOrderProductionPlanSemiSum.QuantityActual69 = result.List.Sum(o => o.QuantityActual69);
                ProductionOrderProductionPlanSemiSum.QuantityGAP69 = result.List.Sum(o => o.QuantityGAP69);

                ProductionOrderProductionPlanSemiSum.Quantity70 = result.List.Sum(o => o.Quantity70);
                ProductionOrderProductionPlanSemiSum.QuantityActual70 = result.List.Sum(o => o.QuantityActual70);
                ProductionOrderProductionPlanSemiSum.QuantityGAP70 = result.List.Sum(o => o.QuantityGAP70);

                ProductionOrderProductionPlanSemiSum.Quantity71 = result.List.Sum(o => o.Quantity71);
                ProductionOrderProductionPlanSemiSum.QuantityActual71 = result.List.Sum(o => o.QuantityActual71);
                ProductionOrderProductionPlanSemiSum.QuantityGAP71 = result.List.Sum(o => o.QuantityGAP71);

                ProductionOrderProductionPlanSemiSum.Quantity72 = result.List.Sum(o => o.Quantity72);
                ProductionOrderProductionPlanSemiSum.QuantityActual72 = result.List.Sum(o => o.QuantityActual72);
                ProductionOrderProductionPlanSemiSum.QuantityGAP72 = result.List.Sum(o => o.QuantityGAP72);

                ProductionOrderProductionPlanSemiSum.Quantity73 = result.List.Sum(o => o.Quantity73);
                ProductionOrderProductionPlanSemiSum.QuantityActual73 = result.List.Sum(o => o.QuantityActual73);
                ProductionOrderProductionPlanSemiSum.QuantityGAP73 = result.List.Sum(o => o.QuantityGAP73);

                ProductionOrderProductionPlanSemiSum.Quantity74 = result.List.Sum(o => o.Quantity74);
                ProductionOrderProductionPlanSemiSum.QuantityActual74 = result.List.Sum(o => o.QuantityActual74);
                ProductionOrderProductionPlanSemiSum.QuantityGAP74 = result.List.Sum(o => o.QuantityGAP74);

                ProductionOrderProductionPlanSemiSum.Quantity75 = result.List.Sum(o => o.Quantity75);
                ProductionOrderProductionPlanSemiSum.QuantityActual75 = result.List.Sum(o => o.QuantityActual75);
                ProductionOrderProductionPlanSemiSum.QuantityGAP75 = result.List.Sum(o => o.QuantityGAP75);

                ProductionOrderProductionPlanSemiSum.Quantity76 = result.List.Sum(o => o.Quantity76);
                ProductionOrderProductionPlanSemiSum.QuantityActual76 = result.List.Sum(o => o.QuantityActual76);
                ProductionOrderProductionPlanSemiSum.QuantityGAP76 = result.List.Sum(o => o.QuantityGAP76);

                ProductionOrderProductionPlanSemiSum.Quantity77 = result.List.Sum(o => o.Quantity77);
                ProductionOrderProductionPlanSemiSum.QuantityActual77 = result.List.Sum(o => o.QuantityActual77);
                ProductionOrderProductionPlanSemiSum.QuantityGAP77 = result.List.Sum(o => o.QuantityGAP77);

                ProductionOrderProductionPlanSemiSum.Quantity78 = result.List.Sum(o => o.Quantity78);
                ProductionOrderProductionPlanSemiSum.QuantityActual78 = result.List.Sum(o => o.QuantityActual78);
                ProductionOrderProductionPlanSemiSum.QuantityGAP78 = result.List.Sum(o => o.QuantityGAP78);

                ProductionOrderProductionPlanSemiSum.Quantity79 = result.List.Sum(o => o.Quantity79);
                ProductionOrderProductionPlanSemiSum.QuantityActual79 = result.List.Sum(o => o.QuantityActual79);
                ProductionOrderProductionPlanSemiSum.QuantityGAP79 = result.List.Sum(o => o.QuantityGAP79);

                ProductionOrderProductionPlanSemiSum.Quantity80 = result.List.Sum(o => o.Quantity80);
                ProductionOrderProductionPlanSemiSum.QuantityActual80 = result.List.Sum(o => o.QuantityActual80);
                ProductionOrderProductionPlanSemiSum.QuantityGAP80 = result.List.Sum(o => o.QuantityGAP80);

                ProductionOrderProductionPlanSemiSum.Quantity81 = result.List.Sum(o => o.Quantity81);
                ProductionOrderProductionPlanSemiSum.QuantityActual81 = result.List.Sum(o => o.QuantityActual81);
                ProductionOrderProductionPlanSemiSum.QuantityGAP81 = result.List.Sum(o => o.QuantityGAP81);

                ProductionOrderProductionPlanSemiSum.Quantity82 = result.List.Sum(o => o.Quantity82);
                ProductionOrderProductionPlanSemiSum.QuantityActual82 = result.List.Sum(o => o.QuantityActual82);
                ProductionOrderProductionPlanSemiSum.QuantityGAP82 = result.List.Sum(o => o.QuantityGAP82);

                ProductionOrderProductionPlanSemiSum.Quantity83 = result.List.Sum(o => o.Quantity83);
                ProductionOrderProductionPlanSemiSum.QuantityActual83 = result.List.Sum(o => o.QuantityActual83);
                ProductionOrderProductionPlanSemiSum.QuantityGAP83 = result.List.Sum(o => o.QuantityGAP83);

                ProductionOrderProductionPlanSemiSum.Quantity84 = result.List.Sum(o => o.Quantity84);
                ProductionOrderProductionPlanSemiSum.QuantityActual84 = result.List.Sum(o => o.QuantityActual84);
                ProductionOrderProductionPlanSemiSum.QuantityGAP84 = result.List.Sum(o => o.QuantityGAP84);

                ProductionOrderProductionPlanSemiSum.Quantity85 = result.List.Sum(o => o.Quantity85);
                ProductionOrderProductionPlanSemiSum.QuantityActual85 = result.List.Sum(o => o.QuantityActual85);
                ProductionOrderProductionPlanSemiSum.QuantityGAP85 = result.List.Sum(o => o.QuantityGAP85);

                ProductionOrderProductionPlanSemiSum.Quantity86 = result.List.Sum(o => o.Quantity86);
                ProductionOrderProductionPlanSemiSum.QuantityActual86 = result.List.Sum(o => o.QuantityActual86);
                ProductionOrderProductionPlanSemiSum.QuantityGAP86 = result.List.Sum(o => o.QuantityGAP86);

                ProductionOrderProductionPlanSemiSum.Quantity87 = result.List.Sum(o => o.Quantity87);
                ProductionOrderProductionPlanSemiSum.QuantityActual87 = result.List.Sum(o => o.QuantityActual87);
                ProductionOrderProductionPlanSemiSum.QuantityGAP87 = result.List.Sum(o => o.QuantityGAP87);

                ProductionOrderProductionPlanSemiSum.Quantity88 = result.List.Sum(o => o.Quantity88);
                ProductionOrderProductionPlanSemiSum.QuantityActual88 = result.List.Sum(o => o.QuantityActual88);
                ProductionOrderProductionPlanSemiSum.QuantityGAP88 = result.List.Sum(o => o.QuantityGAP88);

                ProductionOrderProductionPlanSemiSum.Quantity89 = result.List.Sum(o => o.Quantity89);
                ProductionOrderProductionPlanSemiSum.QuantityActual89 = result.List.Sum(o => o.QuantityActual89);
                ProductionOrderProductionPlanSemiSum.QuantityGAP89 = result.List.Sum(o => o.QuantityGAP89);

                ProductionOrderProductionPlanSemiSum.Quantity90 = result.List.Sum(o => o.Quantity90);
                ProductionOrderProductionPlanSemiSum.QuantityActual90 = result.List.Sum(o => o.QuantityActual90);
                ProductionOrderProductionPlanSemiSum.QuantityGAP90 = result.List.Sum(o => o.QuantityGAP90);

                ProductionOrderProductionPlanSemiSum.Quantity91 = result.List.Sum(o => o.Quantity91);
                ProductionOrderProductionPlanSemiSum.QuantityActual91 = result.List.Sum(o => o.QuantityActual91);
                ProductionOrderProductionPlanSemiSum.QuantityGAP91 = result.List.Sum(o => o.QuantityGAP91);

                ProductionOrderProductionPlanSemiSum.Quantity92 = result.List.Sum(o => o.Quantity92);
                ProductionOrderProductionPlanSemiSum.QuantityActual92 = result.List.Sum(o => o.QuantityActual92);
                ProductionOrderProductionPlanSemiSum.QuantityGAP92 = result.List.Sum(o => o.QuantityGAP92);

                ProductionOrderProductionPlanSemiSum.Quantity93 = result.List.Sum(o => o.Quantity93);
                ProductionOrderProductionPlanSemiSum.QuantityActual93 = result.List.Sum(o => o.QuantityActual93);
                ProductionOrderProductionPlanSemiSum.QuantityGAP93 = result.List.Sum(o => o.QuantityGAP93);

                ProductionOrderProductionPlanSemiSum.Quantity94 = result.List.Sum(o => o.Quantity94);
                ProductionOrderProductionPlanSemiSum.QuantityActual94 = result.List.Sum(o => o.QuantityActual94);
                ProductionOrderProductionPlanSemiSum.QuantityGAP94 = result.List.Sum(o => o.QuantityGAP94);

                ProductionOrderProductionPlanSemiSum.Quantity95 = result.List.Sum(o => o.Quantity95);
                ProductionOrderProductionPlanSemiSum.QuantityActual95 = result.List.Sum(o => o.QuantityActual95);
                ProductionOrderProductionPlanSemiSum.QuantityGAP95 = result.List.Sum(o => o.QuantityGAP95);

                ProductionOrderProductionPlanSemiSum.Quantity96 = result.List.Sum(o => o.Quantity96);
                ProductionOrderProductionPlanSemiSum.QuantityActual96 = result.List.Sum(o => o.QuantityActual96);
                ProductionOrderProductionPlanSemiSum.QuantityGAP96 = result.List.Sum(o => o.QuantityGAP96);

                ProductionOrderProductionPlanSemiSum.Quantity97 = result.List.Sum(o => o.Quantity97);
                ProductionOrderProductionPlanSemiSum.QuantityActual97 = result.List.Sum(o => o.QuantityActual97);
                ProductionOrderProductionPlanSemiSum.QuantityGAP97 = result.List.Sum(o => o.QuantityGAP97);

                ProductionOrderProductionPlanSemiSum.Quantity98 = result.List.Sum(o => o.Quantity98);
                ProductionOrderProductionPlanSemiSum.QuantityActual98 = result.List.Sum(o => o.QuantityActual98);
                ProductionOrderProductionPlanSemiSum.QuantityGAP98 = result.List.Sum(o => o.QuantityGAP98);

                ProductionOrderProductionPlanSemiSum.Quantity99 = result.List.Sum(o => o.Quantity99);
                ProductionOrderProductionPlanSemiSum.QuantityActual99 = result.List.Sum(o => o.QuantityActual99);
                ProductionOrderProductionPlanSemiSum.QuantityGAP99 = result.List.Sum(o => o.QuantityGAP99);

                ProductionOrderProductionPlanSemiSum.Quantity100 = result.List.Sum(o => o.Quantity100);
                ProductionOrderProductionPlanSemiSum.QuantityActual100 = result.List.Sum(o => o.QuantityActual100);
                ProductionOrderProductionPlanSemiSum.QuantityGAP100 = result.List.Sum(o => o.QuantityGAP100);

                ProductionOrderProductionPlanSemiSum.Quantity101 = result.List.Sum(o => o.Quantity101);
                ProductionOrderProductionPlanSemiSum.QuantityActual101 = result.List.Sum(o => o.QuantityActual101);
                ProductionOrderProductionPlanSemiSum.QuantityGAP101 = result.List.Sum(o => o.QuantityGAP101);

                ProductionOrderProductionPlanSemiSum.Quantity102 = result.List.Sum(o => o.Quantity102);
                ProductionOrderProductionPlanSemiSum.QuantityActual102 = result.List.Sum(o => o.QuantityActual102);
                ProductionOrderProductionPlanSemiSum.QuantityGAP102 = result.List.Sum(o => o.QuantityGAP102);

                ProductionOrderProductionPlanSemiSum.Quantity103 = result.List.Sum(o => o.Quantity103);
                ProductionOrderProductionPlanSemiSum.QuantityActual103 = result.List.Sum(o => o.QuantityActual103);
                ProductionOrderProductionPlanSemiSum.QuantityGAP103 = result.List.Sum(o => o.QuantityGAP103);

                ProductionOrderProductionPlanSemiSum.Quantity104 = result.List.Sum(o => o.Quantity104);
                ProductionOrderProductionPlanSemiSum.QuantityActual104 = result.List.Sum(o => o.QuantityActual104);
                ProductionOrderProductionPlanSemiSum.QuantityGAP104 = result.List.Sum(o => o.QuantityGAP104);

                ProductionOrderProductionPlanSemiSum.Quantity105 = result.List.Sum(o => o.Quantity105);
                ProductionOrderProductionPlanSemiSum.QuantityActual105 = result.List.Sum(o => o.QuantityActual105);
                ProductionOrderProductionPlanSemiSum.QuantityGAP105 = result.List.Sum(o => o.QuantityGAP105);

                result.List.Add(ProductionOrderProductionPlanSemiSum);
            }
            result.List = result.List.OrderBy(o => o.Priority).ThenByDescending(o => o.IsLeadNo).ThenBy(o => o.ParentName01).ThenBy(o => o.ParentName02).ThenBy(o => o.ParentName03).ThenBy(o => o.ParentName04).ThenBy(o => o.MaterialCode).ToList(); ;
            return result;
        }
        private List<ProductionOrderProductionPlanSemi> ListSort(List<ProductionOrderProductionPlanSemi> List)
        {
            List<ProductionOrderProductionPlanSemi> result = new List<ProductionOrderProductionPlanSemi>();
            var ListSortOrder = List.Where(o => o.SortOrder < 10).ToList();
            result.AddRange(ListSortOrder);
            List = List.Where(o => o.SortOrder > 1).ToList();
            var ListMaterialCode01 = List.Select(o => o.MaterialCode01).Distinct().OrderBy(o => o).ToList();
            foreach (var MaterialCode01 in ListMaterialCode01)
            {
                var ListLeadNo = List.Where(o => o.MaterialCode01 == MaterialCode01 && o.IsLeadNo == true && o.ParentID01 == null).OrderBy(o => o.MaterialCode01).ThenBy(o => o.MaterialCode).ToList();
                foreach (ProductionOrderProductionPlanSemi LeadNo in ListLeadNo)
                {
                    result.Add(LeadNo);
                }
                var ListSPST = List.Where(o => o.MaterialCode01 == MaterialCode01 && o.IsSPST == true && o.ParentID01 == null).OrderBy(o => o.MaterialCode01).ThenBy(o => o.MaterialCode).ToList();
                foreach (ProductionOrderProductionPlanSemi SPST in ListSPST)
                {
                    result.Add(SPST);
                    var ListLeadNo01 = List.Where(o => o.MaterialCode01 == MaterialCode01 && o.IsLeadNo == true && o.ParentID01 == SPST.BOMID && o.ParentID02 == null && o.ParentID03 == null && o.ParentID04 == null).OrderBy(o => o.MaterialCode01).ThenBy(o => o.MaterialCode).ToList();
                    foreach (ProductionOrderProductionPlanSemi LeadNo01 in ListLeadNo01)
                    {
                        result.Add(LeadNo01);
                    }
                    var ListSPSTParentID01 = List.Where(o => o.MaterialCode01 == MaterialCode01 && o.IsSPST == true && o.ParentID01 == SPST.BOMID).OrderBy(o => o.MaterialCode01).ThenBy(o => o.MaterialCode).ToList();
                    foreach (ProductionOrderProductionPlanSemi SPSTParentID01 in ListSPSTParentID01)
                    {
                        result.Add(SPSTParentID01);
                        var ListLeadNo02 = List.Where(o => o.MaterialCode01 == MaterialCode01 && o.IsLeadNo == true && o.ParentID02 == SPSTParentID01.BOMID && o.ParentID03 == null && o.ParentID04 == null).OrderBy(o => o.MaterialCode01).ThenBy(o => o.MaterialCode).ToList();
                        foreach (ProductionOrderProductionPlanSemi LeadNo02 in ListLeadNo02)
                        {
                            result.Add(LeadNo02);
                        }
                        var ListSPSTParentID02 = List.Where(o => o.MaterialCode01 == MaterialCode01 && o.IsSPST == true && o.ParentID02 == SPSTParentID01.ID).OrderBy(o => o.MaterialCode01).ThenBy(o => o.MaterialCode).ToList();
                        foreach (ProductionOrderProductionPlanSemi SPSTParentID02 in ListSPSTParentID02)
                        {
                            result.Add(SPSTParentID02);
                            var ListLeadNo03 = List.Where(o => o.MaterialCode01 == MaterialCode01 && o.IsLeadNo == true && o.ParentID03 == SPSTParentID02.BOMID && o.ParentID04 == null).OrderBy(o => o.MaterialCode01).ThenBy(o => o.MaterialCode).ToList();
                            foreach (ProductionOrderProductionPlanSemi LeadNo03 in ListLeadNo03)
                            {
                                result.Add(LeadNo03);
                            }
                            var ListSPSTParentID03 = List.Where(o => o.MaterialCode01 == MaterialCode01 && o.IsSPST == true && o.ParentID03 == SPSTParentID02.BOMID).OrderBy(o => o.MaterialCode01).ThenBy(o => o.MaterialCode).ToList();
                            foreach (ProductionOrderProductionPlanSemi SPSTParentID03 in ListSPSTParentID03)
                            {
                                result.Add(SPSTParentID03);
                                var ListLeadNo04 = List.Where(o => o.MaterialCode01 == MaterialCode01 && o.IsLeadNo == true && o.ParentID04 == SPSTParentID03.BOMID).OrderBy(o => o.MaterialCode01).ThenBy(o => o.MaterialCode).ToList();
                                foreach (ProductionOrderProductionPlanSemi LeadNo04 in ListLeadNo04)
                                {
                                    result.Add(LeadNo04);
                                }
                                var ListSPSTParentID04 = List.Where(o => o.MaterialCode01 == MaterialCode01 && o.IsSPST == true && o.ParentID04 == SPSTParentID03.BOMID).OrderBy(o => o.MaterialCode01).ThenBy(o => o.MaterialCode).ToList();
                                foreach (ProductionOrderProductionPlanSemi SPSTParentID04 in ListSPSTParentID04)
                                {
                                    result.Add(SPSTParentID04);
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<ProductionOrderProductionPlanSemi>> SortByListAsync(BaseParameter<ProductionOrderProductionPlanSemi> BaseParameter)
        {
            var result = new BaseResult<ProductionOrderProductionPlanSemi>();
            result.List = new List<ProductionOrderProductionPlanSemi>();
            if (BaseParameter.List != null && BaseParameter.List.Count > 0)
            {
                result.List = BaseParameter.List;
                result.List = result.List.OrderBy(o => o.Priority).ThenByDescending(o => o.IsLeadNo).ThenBy(o => o.ParentName01).ThenBy(o => o.ParentName02).ThenBy(o => o.ParentName03).ThenBy(o => o.ParentName04).ThenBy(o => o.MaterialCode).ToList();
            }
            return result;
        }
        public virtual async Task<BaseResult<ProductionOrderProductionPlanSemi>> ExportByParentIDToExcelAsync(BaseParameter<ProductionOrderProductionPlanSemi> BaseParameter)
        {
            var result = new BaseResult<ProductionOrderProductionPlanSemi>();
            result.List = new List<ProductionOrderProductionPlanSemi>();
            if (BaseParameter.ParentID > 0)
            {
                result.List = await GetByCondition(o => o.ParentID == BaseParameter.ParentID).ToListAsync();
                if (result.List.Count > 0)
                {
                    string fileName = "ProductionOrderProductionPlanSemi-" + BaseParameter.ParentID + "-" + GlobalHelper.InitializationDateTimeCode0001 + ".xlsx";
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

        private void InitializationExcel(List<ProductionOrderProductionPlanSemi> list, MemoryStream streamExport)
        {
            using (var package = new ExcelPackage(streamExport))
            {
                var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                int row = 1;
                int column = 1;
                workSheet.Cells[row, column].Value = "ID";
                column = column + 1;
                workSheet.Cells[row, column].Value = "Finish Goods";
                column = column + 1;
                workSheet.Cells[row, column].Value = "Parent 01";
                column = column + 1;
                workSheet.Cells[row, column].Value = "Parent 02";
                column = column + 1;
                workSheet.Cells[row, column].Value = "Parent 03";
                column = column + 1;
                workSheet.Cells[row, column].Value = "Parent 04";
                column = column + 1;
                workSheet.Cells[row, column].Value = "PART NO";
                column = column + 1;
                workSheet.Cells[row, column].Value = "Priority";
                column = column + 1;
                workSheet.Cells[row, column].Value = "LeadNo";
                column = column + 1;
                workSheet.Cells[row, column].Value = "SPST";
                column = column + 1;
                workSheet.Cells[row, column].Value = "Total";
                column = column + 1;
                workSheet.Cells[row, column].Value = "Actual";
                column = column + 1;
                workSheet.Cells[row, column].Value = "GAP";

                list = list.OrderBy(o => o.ID).ToList();
                var ProductionOrderProductionPlan = list[0];
                int Index = 1;
                foreach (PropertyInfo proProductionOrderProductionPlan in ProductionOrderProductionPlan.GetType().GetProperties())
                {
                    string IndexName = Index.ToString();
                    if (Index < 10)
                    {
                        IndexName = "0" + IndexName;
                    }
                    string DateString = "Date" + IndexName;
                    if (proProductionOrderProductionPlan.Name == DateString)
                    {
                        try
                        {
                            DateTime DateTime = (DateTime)proProductionOrderProductionPlan.GetValue(ProductionOrderProductionPlan);
                            column = column + 1;
                            workSheet.Cells[row, column].Value = DateTime.ToString("MM/dd/yyyy");
                            column = column + 1;
                            workSheet.Cells[row, column].Value = "HookRack";
                            column = column + 1;
                            workSheet.Cells[row, column].Value = "GAP";
                        }
                        catch (Exception ex)
                        {
                            string mes = ex.Message;
                        }
                        Index = Index + 1;
                    }
                }
                row = row + 1;
                list = list.OrderBy(o => o.Priority).ThenByDescending(o => o.IsLeadNo).ThenBy(o => o.ParentName01).ThenBy(o => o.ParentName02).ThenBy(o => o.ParentName03).ThenBy(o => o.ParentName04).ThenBy(o => o.MaterialCode).ToList();
                foreach (ProductionOrderProductionPlanSemi item in list)
                {
                    workSheet.Cells[row, 1].Value = item.ID;
                    workSheet.Cells[row, 2].Value = item.MaterialCode01;
                    workSheet.Cells[row, 3].Value = item.ParentName01;
                    workSheet.Cells[row, 4].Value = item.ParentName02;
                    workSheet.Cells[row, 5].Value = item.ParentName03;
                    workSheet.Cells[row, 6].Value = item.ParentName04;
                    workSheet.Cells[row, 7].Value = item.MaterialCode;
                    workSheet.Cells[row, 8].Value = item.Priority;
                    workSheet.Cells[row, 9].Value = item.IsLeadNo;
                    workSheet.Cells[row, 10].Value = item.IsSPST;
                    workSheet.Cells[row, 11].Value = item.Quantity00;
                    workSheet.Cells[row, 12].Value = item.QuantityActual00;
                    workSheet.Cells[row, 13].Value = item.QuantityGAP00;


                    Index = 1;
                    int columnIndex = 14;
                    foreach (PropertyInfo proProductionOrderProductionPlan in item.GetType().GetProperties())
                    {
                        string IndexName = Index.ToString();
                        if (Index < 10)
                        {
                            IndexName = "0" + IndexName;
                        }
                        string QuantityString = "Quantity" + IndexName;
                        if (proProductionOrderProductionPlan.Name == QuantityString)
                        {
                            try
                            {
                                int Quantity = (int)proProductionOrderProductionPlan.GetValue(item);
                                workSheet.Cells[row, columnIndex].Value = Quantity;
                            }
                            catch (Exception ex)
                            {
                                string mes = ex.Message;
                            }
                            columnIndex = columnIndex + 1;
                        }
                        string QuantityActualString = "QuantityActual" + IndexName;
                        if (proProductionOrderProductionPlan.Name == QuantityActualString)
                        {
                            try
                            {
                                int QuantityActual = (int)proProductionOrderProductionPlan.GetValue(item);
                                workSheet.Cells[row, columnIndex].Value = QuantityActual;
                            }
                            catch (Exception ex)
                            {
                                string mes = ex.Message;
                            }
                            columnIndex = columnIndex + 1;
                        }
                        string QuantityGAPString = "QuantityGAP" + IndexName;
                        if (proProductionOrderProductionPlan.Name == QuantityGAPString)
                        {
                            try
                            {
                                int QuantityGAP = (int)proProductionOrderProductionPlan.GetValue(item);
                                workSheet.Cells[row, columnIndex].Value = QuantityGAP;
                            }
                            catch (Exception ex)
                            {
                                string mes = ex.Message;
                            }
                            Index = Index + 1;
                            columnIndex = columnIndex + 1;
                        }
                    }
                    row = row + 1;
                }

               
                row = row + 1;

                for (int i = 1; i <= column; i++)
                {
                    workSheet.Cells[1, i].Style.Font.Bold = true;
                    workSheet.Cells[1, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Cells[1, i].Style.Font.Name = "Times New Roman";
                    workSheet.Cells[1, i].Style.Font.Size = 12;
                    workSheet.Cells[1, i].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells[1, i].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells[1, i].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells[1, i].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                    workSheet.Column(i).AutoFit();
                }
                package.Save();
            }
            streamExport.Position = 0;
        }
    }
}

