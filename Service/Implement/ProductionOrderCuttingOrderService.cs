namespace Service.Implement
{
    public class ProductionOrderCuttingOrderService : BaseService<ProductionOrderCuttingOrder, IProductionOrderCuttingOrderRepository>
    , IProductionOrderCuttingOrderService
    {
        private readonly IProductionOrderCuttingOrderRepository _ProductionOrderCuttingOrderRepository;
        private readonly IWebHostEnvironment _WebHostEnvironment;

        private readonly IProductionOrderRepository _ProductionOrderRepository;
        private readonly IProductionOrderProductionPlanSemiRepository _ProductionOrderProductionPlanSemiRepository;
        private readonly IMaterialRepository _MaterialRepository;
        private readonly IBOMRepository _BOMRepository;
        private readonly IBOMDetailRepository _BOMDetailRepository;
        private readonly IBOMTermRepository _BOMTermRepository;
        private readonly ICategoryToleranceRepository _CategoryToleranceRepository;
        private readonly ICategoryParentheseRepository _CategoryParentheseRepository;
        private readonly IMembershipRepository _MembershipRepository;

        public ProductionOrderCuttingOrderService(IProductionOrderCuttingOrderRepository ProductionOrderCuttingOrderRepository

            , IWebHostEnvironment webHostEnvironment
            , IProductionOrderRepository ProductionOrderRepository
            , IProductionOrderProductionPlanSemiRepository ProductionOrderProductionPlanSemiRepository
            , IMaterialRepository MaterialRepository
            , IBOMRepository BomRepository
            , IBOMDetailRepository BOMDetailRepository
            , IBOMTermRepository bOMTermRepository
            , ICategoryToleranceRepository CategoryToleranceRepository
            , ICategoryParentheseRepository CategoryParentheseRepository
            , IMembershipRepository MembershipRepository

            ) : base(ProductionOrderCuttingOrderRepository)
        {
            _ProductionOrderCuttingOrderRepository = ProductionOrderCuttingOrderRepository;
            _WebHostEnvironment = webHostEnvironment;
            _ProductionOrderRepository = ProductionOrderRepository;
            _ProductionOrderProductionPlanSemiRepository = ProductionOrderProductionPlanSemiRepository;
            _MaterialRepository = MaterialRepository;
            _BOMRepository = BomRepository;
            _BOMDetailRepository = BOMDetailRepository;
            _BOMTermRepository = bOMTermRepository;
            _CategoryToleranceRepository = CategoryToleranceRepository;
            _CategoryParentheseRepository = CategoryParentheseRepository;
            _MembershipRepository = MembershipRepository;
        }
        public override void Initialization(ProductionOrderCuttingOrder model)
        {
            BaseInitialization(model);
            if (model.MaterialID > 0)
            {
                var Material = _MaterialRepository.GetByID(model.MaterialID.Value);
                model.MaterialCode = Material.Code;
                model.MaterialName = Material.Name;
                model.HookRack = model.HookRack ?? Material.CategoryLocationName;
                if (!string.IsNullOrEmpty(model.MaterialCode))
                {
                    model.Machine = model.MaterialCode.Split('.')[model.MaterialCode.Split('.').Length - 1];
                    try
                    {
                        if (model.MaterialCode.Split('.').Length > 1)
                        {
                            model.Machine = model.MaterialCode.Split('.')[model.MaterialCode.Split('.').Length - 2] + "." + model.MaterialCode.Split('.')[model.MaterialCode.Split('.').Length - 1];
                        }
                    }
                    catch (Exception ex)
                    {
                        string msg = ex.Message;
                    }
                }
            }
            if (model.BOMID01 > 0)
            {
                var BOM = _BOMRepository.GetByID(model.BOMID01.Value);
                model.BOMECN01 = BOM.Code;
                model.BOMECNVersion01 = BOM.Version;
                model.BOMDate01 = BOM.Date;
            }
            if (model.BOMID > 0 && model.CompanyID > 0)
            {
                var ListCategoryParenthese = _CategoryParentheseRepository.GetByCompanyIDToList(model.CompanyID.Value);
                var BOM = _BOMRepository.GetByID(model.BOMID.Value);
                model.BOMECN = BOM.Code;
                model.BOMECNVersion = BOM.Version;
                model.BOMDate = BOM.Date;
                model.Project = BOM.Project;
                model.T1Dir = BOM.DirT1;
                model.T2Dir = BOM.DirT2;
                model.BundleSize = BOM.BundleSize;
                model.CTLeadsPr = model.BundleSize + "EA";
                try
                {
                    if (BOM.Strip1 > 0)
                    {
                        model.Strip1 = BOM.Strip1.Value.ToString("N1");
                    }
                    if (BOM.Strip2 > 0)
                    {
                        model.Strip2 = BOM.Strip2.Value.ToString("N1");
                    }

                    var block = model.QuantityTotal / model.BundleSize;
                    var remainder = model.QuantityTotal % model.BundleSize;
                    if (remainder > 0)
                    {
                        block = block + 1;
                    }
                    block = block * model.BundleSize;
                    model.CTLeads = block.ToString();
                }
                catch (Exception ex)
                {
                    string error = ex.Message;
                }
                decimal? Diameter = 0;
                var BOMDetail = _BOMDetailRepository.GetByCondition(o => o.ParentID == BOM.ID && o.Note == GlobalHelper.WIRE).FirstOrDefault();
                if (BOMDetail != null && BOMDetail.ID > 0)
                {
                    Diameter = BOMDetail.Diameter;
                    model.Wire = BOMDetail.MaterialCode02 + " ";
                    if (BOMDetail.QuantityActual > 0)
                    {
                        model.Wire = model.Wire + BOMDetail.QuantityActual.Value.ToString("N0");
                    }
                    else
                    {
                        model.Wire = model.Wire + "0";
                    }

                }

                BOMDetail = _BOMDetailRepository.GetByCondition(o => o.ParentID == BOM.ID && o.Note == GlobalHelper.SS1).FirstOrDefault();
                if (BOMDetail != null && BOMDetail.ID > 0)
                {
                    model.Seal1 = BOMDetail.MaterialCode02;
                }
                BOMDetail = _BOMDetailRepository.GetByCondition(o => o.ParentID == BOM.ID && o.Note == GlobalHelper.SS2).FirstOrDefault();
                if (BOMDetail != null && BOMDetail.ID > 0)
                {
                    model.Seal2 = BOMDetail.MaterialCode02;
                }

                var ListBOMTerm = _BOMTermRepository.GetByCondition(o => o.ParentID == BOM.ID).ToList();
                var ListCategoryTolerance = _CategoryToleranceRepository.GetByCondition(o => o.CompanyID == model.CompanyID && o.Active == true).ToList();
                foreach (var BOMTerm in ListBOMTerm)
                {
                    var CCH1Sub = "";
                    var CategoryTolerance = ListCategoryTolerance.Where(o => o.Begin <= BOMTerm.CCH1 && o.End >= BOMTerm.CCH1).FirstOrDefault();
                    if (CategoryTolerance != null && CategoryTolerance.ID > 0 && CategoryTolerance.CCH1 > 0)
                    {
                        CCH1Sub = CategoryTolerance.CCH1.Value.ToString("N2");
                    }
                    var CCW1Sub = "";
                    CategoryTolerance = ListCategoryTolerance.Where(o => o.Begin <= BOMTerm.CCW1 && o.End >= BOMTerm.CCW1).FirstOrDefault();
                    if (CategoryTolerance != null && CategoryTolerance.ID > 0 && CategoryTolerance.CCW1 > 0)
                    {
                        CCW1Sub = CategoryTolerance.CCW1.Value.ToString("N2");
                    }
                    var ICH1Sub = "";
                    CategoryTolerance = ListCategoryTolerance.Where(o => o.Begin <= BOMTerm.ICH1 && o.End >= BOMTerm.ICH1).FirstOrDefault();
                    if (CategoryTolerance != null && CategoryTolerance.ID > 0 && CategoryTolerance.ICH1 > 0)
                    {
                        ICH1Sub = CategoryTolerance.ICH1.Value.ToString("N2");
                    }
                    var ICW1Sub = "";
                    CategoryTolerance = ListCategoryTolerance.Where(o => o.Begin <= BOMTerm.ICW1 && o.End >= BOMTerm.ICW1).FirstOrDefault();
                    if (CategoryTolerance != null && CategoryTolerance.ID > 0 && CategoryTolerance.ICW1 > 0)
                    {
                        ICW1Sub = CategoryTolerance.ICW1.Value.ToString("N2");
                    }
                    int IsCheck01 = 0;
                    if (BOMTerm.CCH1 != null && BOMTerm.CCH2 == null)
                    {
                        model.Term1 = BOMTerm.Code;
                    }
                    int IsCheck02 = 0;
                    if (BOMTerm.CCH1 != null && BOMTerm.CCH2 != null)
                    {
                        model.Term2 = BOMTerm.Code;
                        //if (!string.IsNullOrEmpty(model.Term2))
                        //{
                        //    var CategoryParenthese = ListCategoryParenthese.Where(o => o.Code == model.Term2 && string.IsNullOrEmpty(o.Name) && string.IsNullOrEmpty(o.Display)).FirstOrDefault();
                        //    if (CategoryParenthese != null && CategoryParenthese.ID > 0)
                        //    {
                        //        if (CategoryParenthese.Active == true)
                        //        {
                        //            IsCheck02 = 1;
                        //        }
                        //    }

                        //    CategoryParenthese = ListCategoryParenthese.Where(o => o.Code == model.Term2 && o.Name == model.Seal1 && o.Display == model.Seal2).FirstOrDefault();
                        //    if (CategoryParenthese != null && CategoryParenthese.ID > 0)
                        //    {
                        //        IsCheck02 = -1;
                        //        if (CategoryParenthese.Active == true)
                        //        {
                        //            IsCheck02 = 1;
                        //        }
                        //    }

                        //    CategoryParenthese = ListCategoryParenthese.Where(o => o.Name == model.Seal1 && o.Display == model.Seal2).FirstOrDefault();
                        //    if (CategoryParenthese != null && CategoryParenthese.ID > 0)
                        //    {
                        //        IsCheck02 = -1;
                        //        if (CategoryParenthese.Active == true)
                        //        {
                        //            IsCheck02 = 1;
                        //        }
                        //        if (CategoryParenthese.Code == model.Seal2)
                        //        {
                        //            IsCheck02 = 1;
                        //        }
                        //    }
                        //    CategoryParenthese = ListCategoryParenthese.Where(o => o.Code == model.MaterialCode).FirstOrDefault();
                        //    if (CategoryParenthese != null && CategoryParenthese.ID > 0)
                        //    {
                        //        if (CategoryParenthese.Active == true)
                        //        {
                        //            IsCheck02 = 1;
                        //        }
                        //    }

                        //    if (!string.IsNullOrEmpty(model.T2Dir))
                        //    {
                        //        if (IsCheck01 == 0)
                        //        {
                        //            IsCheck02 = 1;
                        //        }
                        //    }
                        //    if (Diameter >= 4)
                        //    {
                        //        IsCheck02 = 1;
                        //    }
                        //    if (IsCheck02 > 0)
                        //    {
                        //        model.Term2 = "(" + model.Term2 + ")";
                        //    }

                        //    model.Term2 = model.Term2.Replace("((", "(");
                        //    model.Term2 = model.Term2.Replace("))", ")");
                        //}
                    }

                    if (string.IsNullOrEmpty(model.Term1))
                    {
                        model.Term1 = GlobalHelper.InitializationString;
                    }
                    if (string.IsNullOrEmpty(model.Term2))
                    {
                        model.Term2 = GlobalHelper.InitializationString;
                    }
                    List<torderlist> Listtorderlist = new List<torderlist>();
                    string MariaDBConectionString = GlobalHelper.MESConectionStringByCompanyID(model.CompanyID.Value);
                    string sql = @"SELECT *  FROM torderlist WHERE LEAD_NO='" + model.MaterialCode + "' AND TERM1 LIKE '%" + model.Term1 + "%' AND TERM2 LIKE '%" + model.Term2 + "%' ORDER BY Update_DTM DESC ";
                    DataSet ds = MySQLHelper.FillDataSetBySQL(MariaDBConectionString, sql);
                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        DataTable dt = ds.Tables[i];
                        Listtorderlist.AddRange(SQLHelper.ToList<torderlist>(dt));
                    }
                    if (Listtorderlist.Count > 0)
                    {
                        var Term1 = "(" + model.Term1 + ")";
                        var torderlistTerm1 = Listtorderlist.Where(o => o.TERM1 == Term1).FirstOrDefault();
                        if (torderlistTerm1 != null && torderlistTerm1.ORDER_IDX > 0)
                        {
                            model.Term1 = Term1;
                        }
                        var Term2 = "(" + model.Term2 + ")";
                        var torderlistTerm2 = Listtorderlist.Where(o => o.TERM2 == Term2).FirstOrDefault();
                        if (torderlistTerm2 != null && torderlistTerm2.ORDER_IDX > 0)
                        {
                            model.Term2 = Term2;
                        }
                    }
                    model.Term1 = model.Term1.Replace("((", "(");
                    model.Term1 = model.Term1.Replace("))", ")");
                    model.Term1 = model.Term1.Replace("((", "(");
                    model.Term1 = model.Term1.Replace("))", ")");
                    model.Term2 = model.Term2.Replace("((", "(");
                    model.Term2 = model.Term2.Replace("))", ")");
                    model.Term2 = model.Term2.Replace("((", "(");
                    model.Term2 = model.Term2.Replace("))", ")");


                    if (BOMTerm.CCH1 != null)
                    {
                        model.CCHW1 = BOMTerm.CCH1.Value.ToString("N2") + "±" + CCH1Sub + "/ ";
                    }
                    if (BOMTerm.CCW1 != null)
                    {
                        model.CCHW1 = model.CCHW1 + BOMTerm.CCW1.Value.ToString("N2") + "±" + CCW1Sub;
                    }
                    if (BOMTerm.ICH1 != null)
                    {
                        model.ICHW1 = BOMTerm.ICH1.Value.ToString("N2") + "±" + ICH1Sub + "/ ";
                    }
                    if (BOMTerm.ICW1 != null)
                    {
                        model.ICHW1 = model.ICHW1 + BOMTerm.ICW1.Value.ToString("N2") + "±" + ICW1Sub;
                    }
                    var CCH2Sub = "";
                    CategoryTolerance = ListCategoryTolerance.Where(o => o.Begin <= BOMTerm.CCH2 && o.End >= BOMTerm.CCH2).FirstOrDefault();
                    if (CategoryTolerance != null && CategoryTolerance.ID > 0 && CategoryTolerance.CCH1 > 0)
                    {
                        CCH2Sub = CategoryTolerance.CCH1.Value.ToString("N2");
                    }
                    var CCW2Sub = "";
                    CategoryTolerance = ListCategoryTolerance.Where(o => o.Begin <= BOMTerm.CCW2 && o.End >= BOMTerm.CCW2).FirstOrDefault();
                    if (CategoryTolerance != null && CategoryTolerance.ID > 0 && CategoryTolerance.CCW1 > 0)
                    {
                        CCW2Sub = CategoryTolerance.CCW1.Value.ToString("N2");
                    }
                    var ICH2Sub = "";
                    CategoryTolerance = ListCategoryTolerance.Where(o => o.Begin <= BOMTerm.ICH2 && o.End >= BOMTerm.ICH2).FirstOrDefault();
                    if (CategoryTolerance != null && CategoryTolerance.ID > 0 && CategoryTolerance.ICH1 > 0)
                    {
                        ICH2Sub = CategoryTolerance.ICH1.Value.ToString("N2");
                    }
                    var ICW2Sub = "";
                    CategoryTolerance = ListCategoryTolerance.Where(o => o.Begin <= BOMTerm.ICW2 && o.End >= BOMTerm.ICW2).FirstOrDefault();
                    if (CategoryTolerance != null && CategoryTolerance.ID > 0 && CategoryTolerance.ICW1 > 0)
                    {
                        ICW2Sub = CategoryTolerance.ICW1.Value.ToString("N2");
                    }

                    if (BOMTerm.CCH2 != null)
                    {
                        model.CCHW2 = BOMTerm.CCH2.Value.ToString("N2") + "±" + CCH2Sub + "/ ";
                    }
                    if (BOMTerm.CCW2 != null)
                    {
                        model.CCHW2 = model.CCHW2 + BOMTerm.CCW2.Value.ToString("N2") + "±" + CCW2Sub;
                    }
                    if (BOMTerm.ICH2 != null)
                    {
                        model.ICHW2 = BOMTerm.ICH2.Value.ToString("N2") + "±" + ICH2Sub + "/ ";
                    }
                    if (BOMTerm.ICW2 != null)
                    {
                        model.ICHW2 = model.ICHW2 + BOMTerm.ICW2.Value.ToString("N2") + "±" + ICW2Sub;
                    }
                }
                model.SPST = "";
                var SPST = "";
                if (BOM.ParentID01 > 0)
                {
                    var BOMSub = _BOMRepository.GetByID(BOM.ParentID01.Value);
                    if (!string.IsNullOrEmpty(BOMSub.MaterialCode))
                    {
                        model.SPST = model.SPST + BOMSub.MaterialCode.Split('.')[0] + " / ";
                        SPST = BOMSub.MaterialCode.Substring(0, 2);
                    }
                    else
                    {
                        model.SPST = model.SPST + " / ";
                    }
                }
                if (BOM.ParentID02 > 0)
                {
                    var BOMSub = _BOMRepository.GetByID(BOM.ParentID02.Value);
                    if (!string.IsNullOrEmpty(BOMSub.MaterialCode))
                    {
                        model.SPST = model.SPST + BOMSub.MaterialCode.Split('.')[0] + " / ";
                    }
                    else
                    {
                        model.SPST = model.SPST + " / ";
                    }
                }
                if (BOM.ParentID03 > 0)
                {
                    var BOMSub = _BOMRepository.GetByID(BOM.ParentID03.Value);
                    if (!string.IsNullOrEmpty(BOMSub.MaterialCode))
                    {
                        model.SPST = model.SPST + BOMSub.MaterialCode.Split('.')[0] + " / ";
                    }
                    else
                    {
                        model.SPST = model.SPST + " / ";
                    }
                }
                if (BOM.ParentID04 > 0)
                {
                    var BOMSub = _BOMRepository.GetByID(BOM.ParentID04.Value);
                    if (!string.IsNullOrEmpty(BOMSub.MaterialCode))
                    {
                        model.SPST = model.SPST + BOMSub.MaterialCode.Split('.')[0] + " / ";
                    }
                    else
                    {
                        model.SPST = model.SPST + " / ";
                    }
                }
                model.SPST = model.SPST + SPST + "(";
                if (!string.IsNullOrEmpty(model.T1Dir))
                {
                    model.SPST = model.SPST + "A";
                }
                model.SPST = model.SPST + ",";
                if (!string.IsNullOrEmpty(model.T2Dir))
                {
                    model.SPST = model.SPST + "B";
                }
                model.SPST = model.SPST + ")";

                BOMDetail = _BOMDetailRepository.GetByCondition(o => o.ParentID == BOM.ID && o.Note == GlobalHelper.Tube1).FirstOrDefault();
                if (BOMDetail != null && BOMDetail.ID > 0)
                {
                    model.SPST = BOMDetail.MaterialCode02 + " / ";
                }

                BOMDetail = _BOMDetailRepository.GetByCondition(o => o.ParentID == BOM.ID && o.Note == GlobalHelper.Tube2).FirstOrDefault();
                if (BOMDetail != null && BOMDetail.ID > 0)
                {
                    model.SPST = BOMDetail.MaterialCode02 + " / ";
                }
            }
        }
        public virtual void InitializationSaveList(ProductionOrderCuttingOrder model, List<Material> ListMaterial, List<BOM> ListBOM, List<BOMDetail> ListBOMDetail, List<BOMTerm> ListBOMTermInput, List<CategoryParenthese> ListCategoryParenthese, List<CategoryTolerance> ListCategoryTolerance)
        {
            BaseInitialization(model);
            if (model.MaterialID > 0)
            {
                var Material = ListMaterial.Where(o => o.ID == model.MaterialID.Value).FirstOrDefault();
                if (Material != null && Material.ID > 0)
                {
                    model.MaterialCode = Material.Code;
                    model.MaterialName = Material.Name;
                    model.HookRack = model.HookRack ?? Material.CategoryLocationName;
                    if (!string.IsNullOrEmpty(model.MaterialCode))
                    {
                        model.Machine = model.MaterialCode.Split('.')[model.MaterialCode.Split('.').Length - 1];
                        try
                        {
                            if (model.MaterialCode.Split('.').Length > 1)
                            {
                                model.Machine = model.MaterialCode.Split('.')[model.MaterialCode.Split('.').Length - 2] + "." + model.MaterialCode.Split('.')[model.MaterialCode.Split('.').Length - 1];
                            }
                        }
                        catch (Exception ex)
                        {
                            string msg = ex.Message;
                        }
                    }
                }
            }
            if (model.BOMID01 > 0)
            {
                var BOM = ListBOM.Where(o => o.ID == model.BOMID01.Value).FirstOrDefault();
                if (BOM != null && BOM.ID > 0)
                {
                    model.BOMECN01 = BOM.Code;
                    model.BOMECNVersion01 = BOM.Version;
                    model.BOMDate01 = BOM.Date;
                }
            }
            if (model.BOMID > 0 && model.CompanyID > 0)
            {
                var BOM = ListBOM.Where(o => o.ID == model.BOMID.Value).FirstOrDefault();
                if (BOM == null)
                {
                    BOM = new BOM();
                }
                model.BOMECN = BOM.Code;
                model.BOMECNVersion = BOM.Version;
                model.BOMDate = BOM.Date;
                model.Project = BOM.Project;
                model.T1Dir = BOM.DirT1;
                model.T2Dir = BOM.DirT2;
                model.BundleSize = BOM.BundleSize;
                model.CTLeadsPr = model.BundleSize + "EA";
                try
                {
                    if (BOM.Strip1 > 0)
                    {
                        model.Strip1 = BOM.Strip1.Value.ToString("N1");
                    }
                    if (BOM.Strip2 > 0)
                    {
                        model.Strip2 = BOM.Strip2.Value.ToString("N1");
                    }

                    var block = model.QuantityTotal / model.BundleSize;
                    var remainder = model.QuantityTotal % model.BundleSize;
                    if (remainder > 0)
                    {
                        block = block + 1;
                    }
                    block = block * model.BundleSize;
                    model.CTLeads = block.ToString();
                }
                catch (Exception ex)
                {
                    string error = ex.Message;
                }
                decimal? Diameter = 0;
                var BOMDetail = ListBOMDetail.Where(o => o.ParentID == BOM.ID && o.Note == GlobalHelper.WIRE).FirstOrDefault();
                if (BOMDetail != null && BOMDetail.ID > 0)
                {
                    Diameter = BOMDetail.Diameter;
                    model.Wire = BOMDetail.MaterialCode02 + " ";
                    if (BOMDetail.QuantityActual > 0)
                    {
                        model.Wire = model.Wire + BOMDetail.QuantityActual.Value.ToString("N0");
                    }
                    else
                    {
                        model.Wire = model.Wire + "0";
                    }

                }

                BOMDetail = ListBOMDetail.Where(o => o.ParentID == BOM.ID && o.Note == GlobalHelper.SS1).FirstOrDefault();
                if (BOMDetail != null && BOMDetail.ID > 0)
                {
                    model.Seal1 = BOMDetail.MaterialCode02;
                }
                BOMDetail = ListBOMDetail.Where(o => o.ParentID == BOM.ID && o.Note == GlobalHelper.SS2).FirstOrDefault();
                if (BOMDetail != null && BOMDetail.ID > 0)
                {
                    model.Seal2 = BOMDetail.MaterialCode02;
                }

                var ListBOMTerm = ListBOMTermInput.Where(o => o.ParentID == BOM.ID).ToList();
                foreach (var BOMTerm in ListBOMTerm)
                {
                    var CCH1Sub = "";
                    var CategoryTolerance = ListCategoryTolerance.Where(o => o.Begin <= BOMTerm.CCH1 && o.End >= BOMTerm.CCH1).FirstOrDefault();
                    if (CategoryTolerance != null && CategoryTolerance.ID > 0 && CategoryTolerance.CCH1 > 0)
                    {
                        CCH1Sub = CategoryTolerance.CCH1.Value.ToString("N2");
                    }
                    var CCW1Sub = "";
                    CategoryTolerance = ListCategoryTolerance.Where(o => o.Begin <= BOMTerm.CCW1 && o.End >= BOMTerm.CCW1).FirstOrDefault();
                    if (CategoryTolerance != null && CategoryTolerance.ID > 0 && CategoryTolerance.CCW1 > 0)
                    {
                        CCW1Sub = CategoryTolerance.CCW1.Value.ToString("N2");
                    }
                    var ICH1Sub = "";
                    CategoryTolerance = ListCategoryTolerance.Where(o => o.Begin <= BOMTerm.ICH1 && o.End >= BOMTerm.ICH1).FirstOrDefault();
                    if (CategoryTolerance != null && CategoryTolerance.ID > 0 && CategoryTolerance.ICH1 > 0)
                    {
                        ICH1Sub = CategoryTolerance.ICH1.Value.ToString("N2");
                    }
                    var ICW1Sub = "";
                    CategoryTolerance = ListCategoryTolerance.Where(o => o.Begin <= BOMTerm.ICW1 && o.End >= BOMTerm.ICW1).FirstOrDefault();
                    if (CategoryTolerance != null && CategoryTolerance.ID > 0 && CategoryTolerance.ICW1 > 0)
                    {
                        ICW1Sub = CategoryTolerance.ICW1.Value.ToString("N2");
                    }
                    int IsCheck01 = 0;
                    if (BOMTerm.CCH1 != null && BOMTerm.CCH2 == null)
                    {
                        model.Term1 = BOMTerm.Code;
                    }
                    int IsCheck02 = 0;
                    if (BOMTerm.CCH1 != null && BOMTerm.CCH2 != null)
                    {
                        model.Term2 = BOMTerm.Code;
                    }

                    if (string.IsNullOrEmpty(model.Term1))
                    {
                        model.Term1 = GlobalHelper.InitializationString;
                    }
                    if (string.IsNullOrEmpty(model.Term2))
                    {
                        model.Term2 = GlobalHelper.InitializationString;
                    }
                    List<torderlist> Listtorderlist = new List<torderlist>();
                    string MariaDBConectionString = GlobalHelper.MESConectionStringByCompanyID(model.CompanyID.Value);
                    string sql = @"SELECT *  FROM torderlist WHERE LEAD_NO='" + model.MaterialCode + "' AND TERM1 LIKE '%" + model.Term1 + "%' AND TERM2 LIKE '%" + model.Term2 + "%' ORDER BY Update_DTM DESC ";
                    DataSet ds = MySQLHelper.FillDataSetBySQL(MariaDBConectionString, sql);
                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        DataTable dt = ds.Tables[i];
                        Listtorderlist.AddRange(SQLHelper.ToList<torderlist>(dt));
                    }
                    if (Listtorderlist.Count > 0)
                    {
                        var Term1 = "(" + model.Term1 + ")";
                        var torderlistTerm1 = Listtorderlist.Where(o => o.TERM1 == Term1).FirstOrDefault();
                        if (torderlistTerm1 != null && torderlistTerm1.ORDER_IDX > 0)
                        {
                            model.Term1 = Term1;
                        }
                        var Term2 = "(" + model.Term2 + ")";
                        var torderlistTerm2 = Listtorderlist.Where(o => o.TERM2 == Term2).FirstOrDefault();
                        if (torderlistTerm2 != null && torderlistTerm2.ORDER_IDX > 0)
                        {
                            model.Term2 = Term2;
                        }
                    }
                    model.Term1 = model.Term1.Replace("((", "(");
                    model.Term1 = model.Term1.Replace("))", ")");
                    model.Term1 = model.Term1.Replace("((", "(");
                    model.Term1 = model.Term1.Replace("))", ")");
                    model.Term2 = model.Term2.Replace("((", "(");
                    model.Term2 = model.Term2.Replace("))", ")");
                    model.Term2 = model.Term2.Replace("((", "(");
                    model.Term2 = model.Term2.Replace("))", ")");


                    if (BOMTerm.CCH1 != null)
                    {
                        model.CCHW1 = BOMTerm.CCH1.Value.ToString("N2") + "±" + CCH1Sub + "/ ";
                    }
                    if (BOMTerm.CCW1 != null)
                    {
                        model.CCHW1 = model.CCHW1 + BOMTerm.CCW1.Value.ToString("N2") + "±" + CCW1Sub;
                    }
                    if (BOMTerm.ICH1 != null)
                    {
                        model.ICHW1 = BOMTerm.ICH1.Value.ToString("N2") + "±" + ICH1Sub + "/ ";
                    }
                    if (BOMTerm.ICW1 != null)
                    {
                        model.ICHW1 = model.ICHW1 + BOMTerm.ICW1.Value.ToString("N2") + "±" + ICW1Sub;
                    }
                    var CCH2Sub = "";
                    CategoryTolerance = ListCategoryTolerance.Where(o => o.Begin <= BOMTerm.CCH2 && o.End >= BOMTerm.CCH2).FirstOrDefault();
                    if (CategoryTolerance != null && CategoryTolerance.ID > 0 && CategoryTolerance.CCH1 > 0)
                    {
                        CCH2Sub = CategoryTolerance.CCH1.Value.ToString("N2");
                    }
                    var CCW2Sub = "";
                    CategoryTolerance = ListCategoryTolerance.Where(o => o.Begin <= BOMTerm.CCW2 && o.End >= BOMTerm.CCW2).FirstOrDefault();
                    if (CategoryTolerance != null && CategoryTolerance.ID > 0 && CategoryTolerance.CCW1 > 0)
                    {
                        CCW2Sub = CategoryTolerance.CCW1.Value.ToString("N2");
                    }
                    var ICH2Sub = "";
                    CategoryTolerance = ListCategoryTolerance.Where(o => o.Begin <= BOMTerm.ICH2 && o.End >= BOMTerm.ICH2).FirstOrDefault();
                    if (CategoryTolerance != null && CategoryTolerance.ID > 0 && CategoryTolerance.ICH1 > 0)
                    {
                        ICH2Sub = CategoryTolerance.ICH1.Value.ToString("N2");
                    }
                    var ICW2Sub = "";
                    CategoryTolerance = ListCategoryTolerance.Where(o => o.Begin <= BOMTerm.ICW2 && o.End >= BOMTerm.ICW2).FirstOrDefault();
                    if (CategoryTolerance != null && CategoryTolerance.ID > 0 && CategoryTolerance.ICW1 > 0)
                    {
                        ICW2Sub = CategoryTolerance.ICW1.Value.ToString("N2");
                    }

                    if (BOMTerm.CCH2 != null)
                    {
                        model.CCHW2 = BOMTerm.CCH2.Value.ToString("N2") + "±" + CCH2Sub + "/ ";
                    }
                    if (BOMTerm.CCW2 != null)
                    {
                        model.CCHW2 = model.CCHW2 + BOMTerm.CCW2.Value.ToString("N2") + "±" + CCW2Sub;
                    }
                    if (BOMTerm.ICH2 != null)
                    {
                        model.ICHW2 = BOMTerm.ICH2.Value.ToString("N2") + "±" + ICH2Sub + "/ ";
                    }
                    if (BOMTerm.ICW2 != null)
                    {
                        model.ICHW2 = model.ICHW2 + BOMTerm.ICW2.Value.ToString("N2") + "±" + ICW2Sub;
                    }
                }
                model.SPST = "";
                var SPST = "";
                if (BOM.ParentID01 > 0)
                {
                    var BOMSub = ListBOM.Where(o => o.ID == BOM.ParentID01.Value).FirstOrDefault();
                    if (BOMSub == null)
                    {
                        BOMSub = new BOM();
                    }
                    if (!string.IsNullOrEmpty(BOMSub.MaterialCode))
                    {
                        model.SPST = model.SPST + BOMSub.MaterialCode.Split('.')[0] + " / ";
                        SPST = BOMSub.MaterialCode.Substring(0, 2);
                    }
                    else
                    {
                        model.SPST = model.SPST + " / ";
                    }
                }
                if (BOM.ParentID02 > 0)
                {
                    var BOMSub = ListBOM.Where(o => o.ID == BOM.ParentID02.Value).FirstOrDefault();
                    if (BOMSub == null)
                    {
                        BOMSub = new BOM();
                    }
                    if (!string.IsNullOrEmpty(BOMSub.MaterialCode))
                    {
                        model.SPST = model.SPST + BOMSub.MaterialCode.Split('.')[0] + " / ";
                    }
                    else
                    {
                        model.SPST = model.SPST + " / ";
                    }
                }
                if (BOM.ParentID03 > 0)
                {
                    var BOMSub = ListBOM.Where(o => o.ID == BOM.ParentID03.Value).FirstOrDefault();
                    if (BOMSub == null)
                    {
                        BOMSub = new BOM();
                    }
                    if (!string.IsNullOrEmpty(BOMSub.MaterialCode))
                    {
                        model.SPST = model.SPST + BOMSub.MaterialCode.Split('.')[0] + " / ";
                    }
                    else
                    {
                        model.SPST = model.SPST + " / ";
                    }
                }
                if (BOM.ParentID04 > 0)
                {
                    var BOMSub = ListBOM.Where(o => o.ID == BOM.ParentID04.Value).FirstOrDefault();
                    if (BOMSub == null)
                    {
                        BOMSub = new BOM();
                    }
                    if (!string.IsNullOrEmpty(BOMSub.MaterialCode))
                    {
                        model.SPST = model.SPST + BOMSub.MaterialCode.Split('.')[0] + " / ";
                    }
                    else
                    {
                        model.SPST = model.SPST + " / ";
                    }
                }
                model.SPST = model.SPST + SPST + "(";
                if (!string.IsNullOrEmpty(model.T1Dir))
                {
                    model.SPST = model.SPST + "A";
                }
                model.SPST = model.SPST + ",";
                if (!string.IsNullOrEmpty(model.T2Dir))
                {
                    model.SPST = model.SPST + "B";
                }
                model.SPST = model.SPST + ")";

                BOMDetail = ListBOMDetail.Where(o => o.ParentID == BOM.ID && o.Note == GlobalHelper.Tube1).FirstOrDefault();
                if (BOMDetail != null && BOMDetail.ID > 0)
                {
                    model.SPST = BOMDetail.MaterialCode02 + " / ";
                }

                BOMDetail = ListBOMDetail.Where(o => o.ParentID == BOM.ID && o.Note == GlobalHelper.Tube2).FirstOrDefault();
                if (BOMDetail != null && BOMDetail.ID > 0)
                {
                    model.SPST = BOMDetail.MaterialCode02 + " / ";
                }
            }
        }
        public override async Task<BaseResult<ProductionOrderCuttingOrder>> SaveAsync(BaseParameter<ProductionOrderCuttingOrder> BaseParameter)
        {
            var result = new BaseResult<ProductionOrderCuttingOrder>();
            if (BaseParameter != null && BaseParameter.BaseModel != null)
            {
                var ModelCheck = await GetByCondition(o => o.ParentID == BaseParameter.BaseModel.ParentID && o.MaterialID01 == BaseParameter.BaseModel.MaterialID01 && o.MaterialID == BaseParameter.BaseModel.MaterialID && o.Date != null && BaseParameter.BaseModel.Date != null && o.Date.Value.Date == BaseParameter.BaseModel.Date.Value.Date).FirstOrDefaultAsync();
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
        public virtual async Task<BaseResult<ProductionOrderCuttingOrder>> GetByParentID_DateToListAsync(BaseParameter<ProductionOrderCuttingOrder> BaseParameter)
        {
            var result = new BaseResult<ProductionOrderCuttingOrder>();
            if (BaseParameter.ParentID > 0 && BaseParameter.Date != null)
            {
                result.List = await GetByCondition(o => o.ParentID == BaseParameter.ParentID && o.Date != null && BaseParameter.Date != null && o.Date.Value.Date == BaseParameter.Date.Value.Date).ToListAsync();
            }
            return result;
        }
        public virtual async Task<BaseResult<ProductionOrderCuttingOrder>> SyncByParentID_DateToListAsync(BaseParameter<ProductionOrderCuttingOrder> BaseParameter)
        {
            var result = new BaseResult<ProductionOrderCuttingOrder>();
            if (BaseParameter.ParentID > 0 && BaseParameter.Date != null)
            {
                long? CompanyID = 16;
                var ListProductionOrderCuttingOrder = GetByCondition(o => o.ParentID == BaseParameter.ParentID && o.Date != null && BaseParameter.Date != null && o.Date.Value.Date == BaseParameter.Date.Value.Date).ToList();
                if (ListProductionOrderCuttingOrder.Count > 0)
                {
                    await _ProductionOrderCuttingOrderRepository.RemoveRangeAsync(ListProductionOrderCuttingOrder);
                }
                var ListProductionOrderCuttingOrderAdd = new List<ProductionOrderCuttingOrder>();
                var ListProductionOrderProductionPlanSemi = await _ProductionOrderProductionPlanSemiRepository.GetByCondition(o => o.Active == true && o.ParentID == BaseParameter.ParentID && o.MaterialID01 > 0 && o.MaterialID > 0 && o.IsLeadNo == true).OrderBy(o => o.MaterialCode).ToListAsync();
                if (ListProductionOrderProductionPlanSemi.Count > 0)
                {
                    CompanyID = ListProductionOrderProductionPlanSemi[0].CompanyID;
                    if (CompanyID > 0)
                    {
                        string MariaDBConectionString = GlobalHelper.MESConectionStringByCompanyID(CompanyID.Value);
                        await MySQLHelper.MESSyncAsync(MariaDBConectionString);
                    }
                }
                List<Material> ListMaterial = await _MaterialRepository.GetByCondition(o => o.CompanyID == CompanyID).ToListAsync();
                List<BOM> ListBOM = await _BOMRepository.GetByCondition(o => o.CompanyID == CompanyID).ToListAsync();
                List<BOMDetail> ListBOMDetail = await _BOMDetailRepository.GetByCondition(o => o.CompanyID == CompanyID).ToListAsync();
                List<BOMTerm> ListBOMTerm = await _BOMTermRepository.GetByCondition(o => o.CompanyID == CompanyID).ToListAsync();
                List<CategoryParenthese> ListCategoryParenthese = await _CategoryParentheseRepository.GetByCondition(o => o.CompanyID == CompanyID).ToListAsync();
                List<CategoryTolerance> ListCategoryTolerance = await _CategoryToleranceRepository.GetByCondition(o => o.CompanyID == CompanyID).ToListAsync();


                foreach (var ProductionOrderProductionPlanSemi in ListProductionOrderProductionPlanSemi)
                {
                    var Index = 1;
                    foreach (PropertyInfo proDate in ProductionOrderProductionPlanSemi.GetType().GetProperties())
                    {
                        var IndexName = Index.ToString();
                        if (Index < 10)
                        {
                            IndexName = "0" + IndexName;
                        }
                        var DateName = "Date" + IndexName;
                        var QuantityGAPName = "QuantityGAP" + IndexName;
                        if (proDate.Name == DateName)
                        {
                            if (proDate.GetValue(ProductionOrderProductionPlanSemi) != null)
                            {
                                var DateValue = (DateTime?)proDate.GetValue(ProductionOrderProductionPlanSemi);
                                if (DateValue != null && DateValue.Value.Date == BaseParameter.Date.Value.Date)
                                {
                                    foreach (PropertyInfo proQuantityGAP in ProductionOrderProductionPlanSemi.GetType().GetProperties())
                                    {
                                        if (proQuantityGAP.Name == QuantityGAPName)
                                        {
                                            if (proQuantityGAP.GetValue(ProductionOrderProductionPlanSemi) != null)
                                            {
                                                var QuantityGAP = (int?)proQuantityGAP.GetValue(ProductionOrderProductionPlanSemi);
                                                if (QuantityGAP > 0)
                                                {
                                                    var ProductionOrderCuttingOrder = new ProductionOrderCuttingOrder();
                                                    ProductionOrderCuttingOrder.SortOrder = 10;
                                                    ProductionOrderCuttingOrder.ParentID = BaseParameter.ParentID;
                                                    ProductionOrderCuttingOrder.Code = ProductionOrderProductionPlanSemi.Code;
                                                    ProductionOrderCuttingOrder.CompanyID = ProductionOrderProductionPlanSemi.CompanyID;
                                                    ProductionOrderCuttingOrder.CompanyName = ProductionOrderProductionPlanSemi.CompanyName;
                                                    ProductionOrderCuttingOrder.ProductionOrderProductionPlanSemiID = ProductionOrderProductionPlanSemi.ID;
                                                    ProductionOrderCuttingOrder.BOMID01 = ProductionOrderProductionPlanSemi.BOMID01;
                                                    ProductionOrderCuttingOrder.BOMID = ProductionOrderProductionPlanSemi.BOMID;
                                                    ProductionOrderCuttingOrder.MaterialID01 = ProductionOrderProductionPlanSemi.MaterialID01;
                                                    ProductionOrderCuttingOrder.MaterialCode01 = ProductionOrderProductionPlanSemi.MaterialCode01;
                                                    ProductionOrderCuttingOrder.MaterialName01 = ProductionOrderProductionPlanSemi.MaterialName01;
                                                    ProductionOrderCuttingOrder.MaterialID = ProductionOrderProductionPlanSemi.MaterialID;
                                                    ProductionOrderCuttingOrder.MaterialCode = ProductionOrderProductionPlanSemi.MaterialCode;
                                                    ProductionOrderCuttingOrder.MaterialName = ProductionOrderProductionPlanSemi.MaterialName;
                                                    ProductionOrderCuttingOrder.Date = BaseParameter.Date;
                                                    ProductionOrderCuttingOrder.QuantityTotal = QuantityGAP;
                                                    InitializationSaveList(ProductionOrderCuttingOrder, ListMaterial, ListBOM, ListBOMDetail, ListBOMTerm, ListCategoryParenthese, ListCategoryTolerance);
                                                    ListProductionOrderCuttingOrderAdd.Add(ProductionOrderCuttingOrder);
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
                if (ListProductionOrderCuttingOrderAdd.Count > 0)
                {
                    ListProductionOrderCuttingOrderAdd = ListProductionOrderCuttingOrderAdd.OrderBy(o => o.MaterialCode).ToList();
                    await _ProductionOrderCuttingOrderRepository.AddRangeAsync(ListProductionOrderCuttingOrderAdd);
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<ProductionOrderCuttingOrder>> SyncMESByParentID_DateToListAsync(BaseParameter<ProductionOrderCuttingOrder> BaseParameter)
        {
            var result = new BaseResult<ProductionOrderCuttingOrder>();
            if (BaseParameter.ParentID > 0 && BaseParameter.Date != null)
            {
                Membership Membership = new Membership();
                string? UserName = "";
                if (BaseParameter.UpdateUserID > 0)
                {
                    Membership = await _MembershipRepository.GetByIDAsync(BaseParameter.UpdateUserID.Value);
                    UserName = Membership.UserName;
                }
                var ProductionOrder = _ProductionOrderRepository.GetByID(BaseParameter.ParentID.Value);
                if (ProductionOrder.ID > 0 && ProductionOrder.CompanyID > 0)
                {
                    result.List = await GetByCondition(o => o.ParentID == BaseParameter.ParentID && o.Date != null && BaseParameter.Date != null && o.Date.Value.Date == BaseParameter.Date.Value.Date).ToListAsync();
                    if (result.List.Count > 0)
                    {
                        string MariaDBConectionString = GlobalHelper.MESConectionStringByCompanyID(ProductionOrder.CompanyID.Value);
                        string FACTORY_NM = "Factory 1";
                        switch (ProductionOrder.CompanyID)
                        {
                            case 17:
                                FACTORY_NM = "Factory 2";
                                break;
                        }
                        string sql = @"INSERT INTO TORDERLIST(`LEAD_NO`, `PROJECT`, `TOT_QTY`, `ADJ_AF_QTY`, `CUR_LEADS`, `CT_LEADS`, `CT_LEADS_PR`, `GRP`, `PRT`, `DT`, `MC`, `BUNDLE_SIZE`, `HOOK_RACK`, `WIRE`, `T1_DIR`, `TERM1`, `STRIP1`, `SEAL1`, `CCH_W1`, `ICH_W1`, `T2_DIR`, `TERM2`, `STRIP2`, `SEAL2`, `CCH_W2`, `ICH_W2`, `SP_ST`, `REP`, `DSCN_YN`, `CONDITION`, `CREATE_DTM`, `CREATE_USER`, `WORK_WEEK`, `OR_NO`, `FCTRY_NM`, `TORDER_FG`, `TOEXCEL_QTY`) VALUES ";
                        string VALUESSUM = "";
                        foreach (var item in result.List)
                        {
                            if (item.Date == null)
                            {
                                item.Date = GlobalHelper.InitializationDateTime;
                            }
                            string Date = item.Date.Value.ToString("yyyy-MM-dd");
                            int Week = GlobalHelper.GetWeekByDateTime(item.Date.Value);
                            item.MaterialCode = item.MaterialCode ?? "";
                            item.Project = item.Project ?? "";
                            item.QuantityTotal = item.QuantityTotal ?? 0;
                            item.Quantity = item.Quantity ?? 0;
                            item.CurrentLeads = item.CurrentLeads ?? "";
                            item.CTLeads = item.CTLeads ?? "";
                            item.CTLeadsPr = item.CTLeadsPr ?? "";
                            item.Group = item.Group ?? "";
                            item.Print = item.Print ?? "";
                            item.Machine = item.Machine ?? "";
                            item.BundleSize = item.BundleSize ?? 0;
                            item.HookRack = item.HookRack ?? "";
                            item.Wire = item.Wire ?? "";
                            item.T1Dir = item.T1Dir ?? "";
                            item.Term1 = item.Term1 ?? "";
                            item.Strip1 = item.Strip1 ?? "";
                            item.Seal1 = item.Seal1 ?? "";
                            item.CCHW1 = item.CCHW1 ?? "";
                            item.ICHW1 = item.ICHW1 ?? "";
                            item.T2Dir = item.T2Dir ?? "";
                            item.Term2 = item.Term2 ?? "";
                            item.Strip2 = item.Strip2 ?? "";
                            item.Seal2 = item.Seal2 ?? "";
                            item.CCHW2 = item.CCHW2 ?? "";
                            item.ICHW2 = item.ICHW2 ?? "";
                            item.SPST = item.SPST ?? "";
                            item.MaterialCode01 = item.MaterialCode01 ?? "";

                            string VALUES = "('" + item.MaterialCode + "', '" + item.Project + "', " + item.QuantityTotal + ", " + item.Quantity + ", '" + item.CurrentLeads + "', '" + item.CTLeads + "', '" + item.CTLeadsPr
                                + "', '" + item.Group + "', '" + item.Print + "', '" + Date + "', '" + item.Machine + "', " + item.BundleSize + ", '" + item.HookRack + "', '" + item.Wire + "', '" + item.T1Dir + "', '" + item.Term1 + "', '" + item.Strip1
                                + "', '" + item.Seal1 + "', '" + item.CCHW1 + "', '" + item.ICHW1 + "', '" + item.T2Dir + "', '" + item.Term2 + "', '" + item.Strip2 + "', '" + item.Seal2 + "', '" + item.CCHW2 + "', '" + item.ICHW2 + "', '" + item.SPST + "', '" + "" + "', 'N', 'Stay', NOW(), '" + UserName + "'," + Week + ", 'EVENT', '" + FACTORY_NM + "','" + item.MaterialCode01 + "' , " + item.QuantityTotal + ")";
                            if (VALUESSUM == "")
                            {
                                VALUESSUM = VALUES;
                            }
                            else
                            {
                                VALUESSUM = VALUESSUM + ", " + VALUES;
                            }
                        }
                        sql = sql + VALUESSUM;
                        string sqlResult = await MySQLHelper.ExecuteNonQueryAsync(MariaDBConectionString, sql);
                    }
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<ProductionOrderCuttingOrder>> ExportToExcelAsync(BaseParameter<ProductionOrderCuttingOrder> BaseParameter)
        {
            var result = new BaseResult<ProductionOrderCuttingOrder>();
            if (BaseParameter.ParentID > 0 && BaseParameter.Date != null)
            {
                result.List = await GetByCondition(o => o.ParentID == BaseParameter.ParentID && o.Date != null && BaseParameter.Date != null && o.Date.Value.Date == BaseParameter.Date.Value.Date).ToListAsync();
                if (result.List.Count > 0)
                {
                    var ProductionOrderCuttingOrder = result.List[0];
                    string fileName = BaseParameter.ParentID + "-CuttingOrder-" + BaseParameter.Date.Value.Date.ToString("yyyyMMdd") + "-" + GlobalHelper.InitializationDateTimeCode + ".xlsx";
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
        private void InitializationExcel(List<ProductionOrderCuttingOrder> list, MemoryStream streamExport)
        {
            using (var package = new ExcelPackage(streamExport))
            {
                var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                int row = 1;
                int column = 1;
                workSheet.Cells[row, column].Value = "PO Code";
                column = column + 1;
                workSheet.Cells[row, column].Value = "ECN";
                column = column + 1;
                workSheet.Cells[row, column].Value = "ASSY NO (Finish Goods)";
                column = column + 1;              
                workSheet.Cells[row, column].Value = "LEAD NO";
                column = column + 1;
                workSheet.Cells[row, column].Value = "Project";
                column = column + 1;
                workSheet.Cells[row, column].Value = "Total Quantity";
                column = column + 1;
                workSheet.Cells[row, column].Value = "Quantity";
                column = column + 1;
                workSheet.Cells[row, column].Value = "Current Leads";
                column = column + 1;
                workSheet.Cells[row, column].Value = "CT Leads";
                column = column + 1;
                workSheet.Cells[row, column].Value = "CT Leads Pr";
                column = column + 1;
                workSheet.Cells[row, column].Value = "Group";
                column = column + 1;
                workSheet.Cells[row, column].Value = "Print";
                column = column + 1;
                workSheet.Cells[row, column].Value = "Date";
                column = column + 1;
                workSheet.Cells[row, column].Value = "Machine";
                column = column + 1;
                workSheet.Cells[row, column].Value = "Bundle Size";
                column = column + 1;
                workSheet.Cells[row, column].Value = "Hook Rack";
                column = column + 1;
                workSheet.Cells[row, column].Value = "Wire";
                column = column + 1;
                workSheet.Cells[row, column].Value = "T1.Dir";
                column = column + 1;
                workSheet.Cells[row, column].Value = "Term1";
                column = column + 1;
                workSheet.Cells[row, column].Value = "Strip1";
                column = column + 1;
                workSheet.Cells[row, column].Value = "Seal1";
                column = column + 1;
                workSheet.Cells[row, column].Value = "CCH/W1";
                column = column + 1;
                workSheet.Cells[row, column].Value = "ICH/W1";
                column = column + 1;
                workSheet.Cells[row, column].Value = "T2.Dir";
                column = column + 1;
                workSheet.Cells[row, column].Value = "Term2";
                column = column + 1;
                workSheet.Cells[row, column].Value = "Strip2";
                column = column + 1;
                workSheet.Cells[row, column].Value = "Seal2";
                column = column + 1;
                workSheet.Cells[row, column].Value = "CCH/W2";
                column = column + 1;
                workSheet.Cells[row, column].Value = "ICH/W2";
                column = column + 1;
                workSheet.Cells[row, column].Value = "SPST";

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
                foreach (ProductionOrderCuttingOrder item in list)
                {
                    workSheet.Cells[row, 1].Value = item.Code;
                    workSheet.Cells[row, 2].Value = item.BOMECN01;
                    workSheet.Cells[row, 3].Value = item.MaterialCode01;                    
                    workSheet.Cells[row, 4].Value = item.MaterialCode;
                    workSheet.Cells[row, 5].Value = item.Project;
                    workSheet.Cells[row, 6].Value = item.QuantityTotal;
                    workSheet.Cells[row, 7].Value = item.Quantity;
                    workSheet.Cells[row, 8].Value = item.CurrentLeads;
                    workSheet.Cells[row, 9].Value = item.CTLeads;
                    workSheet.Cells[row, 10].Value = item.CTLeadsPr;
                    workSheet.Cells[row, 11].Value = item.Group;
                    workSheet.Cells[row, 12].Value = item.Print;
                    try
                    {
                        workSheet.Cells[row, 13].Value = item.Date.Value.ToString("dd/MM/yyyy");
                    }
                    catch (Exception ex)
                    {
                        string msg = ex.Message;
                        workSheet.Cells[row, 13].Value = item.Date;
                    }

                    workSheet.Cells[row, 14].Value = item.Machine;
                    workSheet.Cells[row, 15].Value = item.BundleSize;
                    workSheet.Cells[row, 16].Value = item.HookRack;
                    workSheet.Cells[row, 17].Value = item.Wire;
                    workSheet.Cells[row, 18].Value = item.T1Dir;
                    workSheet.Cells[row, 19].Value = item.Term1;
                    workSheet.Cells[row, 20].Value = item.Strip1;
                    workSheet.Cells[row, 21].Value = item.Seal1;
                    workSheet.Cells[row, 22].Value = item.CCHW1;
                    workSheet.Cells[row, 23].Value = item.ICHW1;
                    workSheet.Cells[row, 24].Value = item.T2Dir;
                    workSheet.Cells[row, 25].Value = item.Term2;
                    workSheet.Cells[row, 26].Value = item.Strip2;
                    workSheet.Cells[row, 27].Value = item.Seal2;
                    workSheet.Cells[row, 28].Value = item.CCHW2;
                    workSheet.Cells[row, 29].Value = item.ICHW2;
                    workSheet.Cells[row, 30].Value = item.SPST;

                    for (int i = 1; i <= column; i++)
                    {
                        workSheet.Cells[row, i].Style.Font.Name = "Times New Roman";
                        workSheet.Cells[row, i].Style.Font.Size = 14;
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

