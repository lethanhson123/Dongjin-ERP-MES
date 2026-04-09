namespace Service
{
    public static class ConfigureService
    {
        public static IServiceCollection AddJWT(this IServiceCollection services)
        {

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = GlobalHelper.Audience,
                    ValidIssuer = GlobalHelper.Issuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(GlobalHelper.Key))
                };
            });
            return services;
        }
        public static IServiceCollection AddContext(this IServiceCollection services)
        {
            services.AddDbContext<Context.Context.Context>(opts => opts.UseMySql(GlobalHelper.ERP_MariaDBConectionString, ServerVersion.AutoDetect(GlobalHelper.ERP_MariaDBConectionString)));            

            return services;
        }
        public static IServiceCollection AddService(this IServiceCollection services)
        {
            services.AddTransient<ICategoryConfigService, CategoryConfigService>();
            services.AddTransient<ICategoryLevelService, CategoryLevelService>();
            services.AddTransient<ICategoryStatusService, CategoryStatusService>();
            services.AddTransient<ICategorySystemService, CategorySystemService>();
            services.AddTransient<ICategoryTypeService, CategoryTypeService>();
            services.AddTransient<IModuleService, ModuleService>();
            services.AddTransient<IProjectService, ProjectService>();
            services.AddTransient<IProjectFileService, ProjectFileService>();
            services.AddTransient<IProjectTaskService, ProjectTaskService>();
            services.AddTransient<IProjectTaskHistoryService, ProjectTaskHistoryService>();
            services.AddTransient<IProjectTaskMembershipService, ProjectTaskMembershipService>();

            services.AddTransient<IMembershipService, MembershipService>();
            services.AddTransient<IMembershipTokenService, MembershipTokenService>();
            services.AddTransient<IMembershipHistoryURLService, MembershipHistoryURLService>();
            services.AddTransient<IMembershipMenuService, MembershipMenuService>();
            services.AddTransient<IMembershipCompanyService, MembershipCompanyService>();
            services.AddTransient<IMembershipDepartmentService, MembershipDepartmentService>();


            services.AddTransient<ICategoryPositionService, CategoryPositionService>();
            services.AddTransient<ICategoryDepartmentService, CategoryDepartmentService>();
            services.AddTransient<ICategoryRackService, CategoryRackService>();
            services.AddTransient<ICategoryLayerService, CategoryLayerService>();
            services.AddTransient<ICategoryLocationService, CategoryLocationService>();
            services.AddTransient<ICategoryLocationMaterialService, CategoryLocationMaterialService>();
            services.AddTransient<ICategoryMenuService, CategoryMenuService>();
            services.AddTransient<ICategoryUnitService, CategoryUnitService>();
            services.AddTransient<ICategoryDeliveryService, CategoryDeliveryService>();
            services.AddTransient<ICategoryFamilyService, CategoryFamilyService>();
            services.AddTransient<ICategoryVehicleService, CategoryVehicleService>();
            services.AddTransient<ICategoryMaterialService, CategoryMaterialService>();
            services.AddTransient<ICategoryTermService, CategoryTermService>();
            services.AddTransient<ICategoryToleranceService, CategoryToleranceService>();
            services.AddTransient<ICategoryParentheseService, CategoryParentheseService>();
            services.AddTransient<ICategorySealKitService, CategorySealKitService>();

            services.AddTransient<IInventoryService, InventoryService>();
            services.AddTransient<IInventoryDetailService, InventoryDetailService>();
            services.AddTransient<IInventoryDetailBarcodeService, InventoryDetailBarcodeService>();            
            
            
            services.AddTransient<IMaterialService, MaterialService>();
            services.AddTransient<IMaterialConvertService, MaterialConvertService>();
            services.AddTransient<IMaterialReplaceService, MaterialReplaceService>();
            services.AddTransient<IMaterialUnitCovertService, MaterialUnitCovertService>();
            services.AddTransient<IFactoryService, FactoryService>();
            services.AddTransient<ICompanyService, CompanyService>();

            services.AddTransient<IBOMService, BOMService>();
            services.AddTransient<IBOMDetailService, BOMDetailService>();
            services.AddTransient<IBOMTermService, BOMTermService>();
            services.AddTransient<IBOMStageService, BOMStageService>();
            services.AddTransient<IBOMCompareService, BOMCompareService>();
            services.AddTransient<IBOMFileService, BOMFileService>();


            services.AddTransient<IProductionOrderService, ProductionOrderService>();
            services.AddTransient<IProductionOrderBOMService, ProductionOrderBOMService>();
            services.AddTransient<IProductionOrderBOMDetailService, ProductionOrderBOMDetailService>();
            services.AddTransient<IProductionOrderDetailService, ProductionOrderDetailService>();
            services.AddTransient<IProductionOrderProductionPlanService, ProductionOrderProductionPlanService>();
            services.AddTransient<IProductionOrderProductionPlanBackupService, ProductionOrderProductionPlanBackupService>();
            services.AddTransient<IProductionOrderProductionPlanSemiService, ProductionOrderProductionPlanSemiService>();
            services.AddTransient<IProductionOrderCuttingOrderService, ProductionOrderCuttingOrderService>();
            services.AddTransient<IProductionOrderSPSTOrderService, ProductionOrderSPSTOrderService>();
            services.AddTransient<IProductionOrderProductionPlanMaterialService, ProductionOrderProductionPlanMaterialService>();
            services.AddTransient<IProductionOrderMaterialService, ProductionOrderMaterialService>();
            services.AddTransient<IProductionOrderOutputScheduleService, ProductionOrderOutputScheduleService>();
            services.AddTransient<IProductionOrderFileService, ProductionOrderFileService>();

            services.AddTransient<ICategoryCompanyService, CategoryCompanyService>();
            services.AddTransient<ICategoryInvoiceService, CategoryInvoiceService>();            
          
            services.AddTransient<IWarehouseInputService, WarehouseInputService>();
            services.AddTransient<IWarehouseInputDetailService, WarehouseInputDetailService>();
            services.AddTransient<IWarehouseInputDetailBarcodeService, WarehouseInputDetailBarcodeService>();
            services.AddTransient<IWarehouseInputDetailBarcodeMaterialService, WarehouseInputDetailBarcodeMaterialService>();
            services.AddTransient<IWarehouseInputMaterialService, WarehouseInputMaterialService>();
            services.AddTransient<IWarehouseInputDetailCountService, WarehouseInputDetailCountService>();
            services.AddTransient<IWarehouseInputFileService, WarehouseInputFileService>();
            services.AddTransient<IWarehouseOutputService, WarehouseOutputService>();
            services.AddTransient<IWarehouseOutputDetailService, WarehouseOutputDetailService>();
            services.AddTransient<IWarehouseOutputDetailBarcodeService, WarehouseOutputDetailBarcodeService>();
            services.AddTransient<IWarehouseOutputDetailBarcodeMaterialService, WarehouseOutputDetailBarcodeMaterialService>();
            services.AddTransient<IWarehouseOutputMaterialService, WarehouseOutputMaterialService>();
            services.AddTransient<IWarehouseOutputFileService, WarehouseOutputFileService>();
            services.AddTransient<IWarehouseInventoryService, WarehouseInventoryService>();
            services.AddTransient<IWarehouseStockService, WarehouseStockService>();
            services.AddTransient<IWarehouseStockDetailService, WarehouseStockDetailService>();
            services.AddTransient<IWarehouseRequestService, WarehouseRequestService>();
            services.AddTransient<IWarehouseRequestDetailService, WarehouseRequestDetailService>();
            services.AddTransient<IWarehouseRequestConfirmService, WarehouseRequestConfirmService>();
            services.AddTransient<IWarehouseRequestFileService, WarehouseRequestFileService>();


            services.AddTransient<IInvoiceInputService, InvoiceInputService>();
            services.AddTransient<IInvoiceInputDetailService, InvoiceInputDetailService>();
            services.AddTransient<IInvoiceInputFileService, InvoiceInputFileService>();
            services.AddTransient<IInvoiceInputHistoryService, InvoiceInputHistoryService>();
            services.AddTransient<IInvoiceOutputService, InvoiceOutputService>();
            services.AddTransient<IInvoiceOutputDetailService, InvoiceOutputDetailService>();
            services.AddTransient<IInvoiceOutputFileService, InvoiceOutputFileService>();
            services.AddTransient<IInvoiceInputInventoryService, InvoiceInputInventoryService>();

            services.AddTransient<IReportService, ReportService>();
            services.AddTransient<IReportDetailService, ReportDetailService>();

            services.AddTransient<IZaloTokenService, ZaloTokenService>();
            services.AddTransient<IZaloZNSService, ZaloZNSService>();

            services.AddTransient<INotificationService, NotificationService>();

            services.AddTransient<ItrackmtimService, trackmtimService>();

            services.AddSingleton(HtmlEncoder.Create(allowedRanges: new[] { UnicodeRanges.All }));
            return services;
        }

        public static IServiceCollection AddRepository(this IServiceCollection services)
        {
            services.AddTransient<ICategoryConfigRepository, CategoryConfigRepository>();
            services.AddTransient<ICategoryLevelRepository, CategoryLevelRepository>();
            services.AddTransient<ICategoryStatusRepository, CategoryStatusRepository>();
            services.AddTransient<ICategorySystemRepository, CategorySystemRepository>();
            services.AddTransient<ICategoryTypeRepository, CategoryTypeRepository>();
            services.AddTransient<IModuleRepository, ModuleRepository>();
            services.AddTransient<IProjectRepository, ProjectRepository>();
            services.AddTransient<IProjectFileRepository, ProjectFileRepository>();
            services.AddTransient<IProjectTaskRepository, ProjectTaskRepository>();
            services.AddTransient<IProjectTaskHistoryRepository, ProjectTaskHistoryRepository>();
            services.AddTransient<IProjectTaskMembershipRepository, ProjectTaskMembershipRepository>();


            services.AddTransient<IMembershipRepository, MembershipRepository>();
            services.AddTransient<IMembershipTokenRepository, MembershipTokenRepository>();
            services.AddTransient<IMembershipHistoryURLRepository, MembershipHistoryURLRepository>();
            services.AddTransient<IMembershipMenuRepository, MembershipMenuRepository>();
            services.AddTransient<IMembershipCompanyRepository, MembershipCompanyRepository>();
            services.AddTransient<IMembershipDepartmentRepository, MembershipDepartmentRepository>();


            services.AddTransient<ICategoryPositionRepository, CategoryPositionRepository>();
            services.AddTransient<ICategoryDepartmentRepository, CategoryDepartmentRepository>();
            services.AddTransient<ICategoryRackRepository, CategoryRackRepository>();
            services.AddTransient<ICategoryLayerRepository, CategoryLayerRepository>();
            services.AddTransient<ICategoryLocationRepository, CategoryLocationRepository>();
            services.AddTransient<ICategoryLocationMaterialRepository, CategoryLocationMaterialRepository>();
            services.AddTransient<ICategoryMenuRepository, CategoryMenuRepository>();
            services.AddTransient<ICategoryUnitRepository, CategoryUnitRepository>();
            services.AddTransient<ICategoryDeliveryRepository, CategoryDeliveryRepository>();
            services.AddTransient<ICategoryFamilyRepository, CategoryFamilyRepository>();
            services.AddTransient<ICategoryVehicleRepository, CategoryVehicleRepository>();
            services.AddTransient<ICategoryMaterialRepository, CategoryMaterialRepository>();
            services.AddTransient<ICategoryToleranceRepository, CategoryToleranceRepository>();
            services.AddTransient<ICategoryTermRepository, CategoryTermRepository>();
            services.AddTransient<ICategoryParentheseRepository, CategoryParentheseRepository>();
            services.AddTransient<ICategorySealKitRepository, CategorySealKitRepository>();

            services.AddTransient<IInventoryRepository, InventoryRepository>();
            services.AddTransient<IInventoryDetailRepository, InventoryDetailRepository>();
            services.AddTransient<IInventoryDetailBarcodeRepository, InventoryDetailBarcodeRepository>();           
            
            
            services.AddTransient<IMaterialRepository, MaterialRepository>();
            services.AddTransient<IMaterialConvertRepository, MaterialConvertRepository>();
            services.AddTransient<IMaterialReplaceRepository, MaterialReplaceRepository>();
            services.AddTransient<IMaterialUnitCovertRepository, MaterialUnitCovertRepository>();
            services.AddTransient<IFactoryRepository, FactoryRepository>();
            services.AddTransient<ICompanyRepository, CompanyRepository>();

            services.AddTransient<IBOMRepository, BOMRepository>();
            services.AddTransient<IBOMDetailRepository, BOMDetailRepository>();
            services.AddTransient<IBOMTermRepository, BOMTermRepository>();
            services.AddTransient<IBOMStageRepository, BOMStageRepository>();
            services.AddTransient<IBOMCompareRepository, BOMCompareRepository>();
            services.AddTransient<IBOMFileRepository, BOMFileRepository>();


            services.AddTransient<IProductionOrderRepository, ProductionOrderRepository>();
            services.AddTransient<IProductionOrderBOMRepository, ProductionOrderBOMRepository>();
            services.AddTransient<IProductionOrderBOMDetailRepository, ProductionOrderBOMDetailRepository>();
            services.AddTransient<IProductionOrderDetailRepository, ProductionOrderDetailRepository>();
            services.AddTransient<IProductionOrderProductionPlanRepository, ProductionOrderProductionPlanRepository>();
            services.AddTransient<IProductionOrderProductionPlanBackupRepository, ProductionOrderProductionPlanBackupRepository>();
            services.AddTransient<IProductionOrderProductionPlanSemiRepository, ProductionOrderProductionPlanSemiRepository>();
            services.AddTransient<IProductionOrderCuttingOrderRepository, ProductionOrderCuttingOrderRepository>();
            services.AddTransient<IProductionOrderSPSTOrderRepository, ProductionOrderSPSTOrderRepository>();
            services.AddTransient<IProductionOrderProductionPlanMaterialRepository, ProductionOrderProductionPlanMaterialRepository>();
            services.AddTransient<IProductionOrderMaterialRepository, ProductionOrderMaterialRepository>();
            services.AddTransient<IProductionOrderOutputScheduleRepository, ProductionOrderOutputScheduleRepository>();
            services.AddTransient<IProductionOrderFileRepository, ProductionOrderFileRepository>();

            services.AddTransient<ICategoryCompanyRepository, CategoryCompanyRepository>();
            services.AddTransient<ICategoryInvoiceRepository, CategoryInvoiceRepository>();            
           

            services.AddTransient<IWarehouseInputRepository, WarehouseInputRepository>();
            services.AddTransient<IWarehouseInputDetailRepository, WarehouseInputDetailRepository>();
            services.AddTransient<IWarehouseInputDetailBarcodeRepository, WarehouseInputDetailBarcodeRepository>();
            services.AddTransient<IWarehouseInputDetailBarcodeMaterialRepository, WarehouseInputDetailBarcodeMaterialRepository>();
            services.AddTransient<IWarehouseInputMaterialRepository, WarehouseInputMaterialRepository>();
            services.AddTransient<IWarehouseInputDetailCountRepository, WarehouseInputDetailCountRepository>();
            services.AddTransient<IWarehouseInputFileRepository, WarehouseInputFileRepository>();
            services.AddTransient<IWarehouseOutputRepository, WarehouseOutputRepository>();
            services.AddTransient<IWarehouseOutputDetailRepository, WarehouseOutputDetailRepository>();
            services.AddTransient<IWarehouseOutputDetailBarcodeRepository, WarehouseOutputDetailBarcodeRepository>();
            services.AddTransient<IWarehouseOutputDetailBarcodeMaterialRepository, WarehouseOutputDetailBarcodeMaterialRepository>();
            services.AddTransient<IWarehouseOutputMaterialRepository, WarehouseOutputMaterialRepository>();
            services.AddTransient<IWarehouseOutputFileRepository, WarehouseOutputFileRepository>();
            services.AddTransient<IWarehouseInventoryRepository, WarehouseInventoryRepository>();
            services.AddTransient<IWarehouseStockRepository, WarehouseStockRepository>();
            services.AddTransient<IWarehouseStockDetailRepository, WarehouseStockDetailRepository>();
            services.AddTransient<IWarehouseRequestRepository, WarehouseRequestRepository>();
            services.AddTransient<IWarehouseRequestDetailRepository, WarehouseRequestDetailRepository>();
            services.AddTransient<IWarehouseRequestConfirmRepository, WarehouseRequestConfirmRepository>();
            services.AddTransient<IWarehouseRequestFileRepository, WarehouseRequestFileRepository>();

            services.AddTransient<IInvoiceInputRepository, InvoiceInputRepository>();
            services.AddTransient<IInvoiceInputDetailRepository, InvoiceInputDetailRepository>();
            services.AddTransient<IInvoiceInputFileRepository, InvoiceInputFileRepository>();
            services.AddTransient<IInvoiceInputHistoryRepository, InvoiceInputHistoryRepository>();
            services.AddTransient<IInvoiceOutputRepository, InvoiceOutputRepository>();
            services.AddTransient<IInvoiceOutputDetailRepository, InvoiceOutputDetailRepository>();
            services.AddTransient<IInvoiceOutputFileRepository, InvoiceOutputFileRepository>();
            services.AddTransient<IInvoiceInputInventoryRepository, InvoiceInputInventoryRepository>();

            services.AddTransient<IReportRepository, ReportRepository>();
            services.AddTransient<IReportDetailRepository, ReportDetailRepository>();

            services.AddTransient<IZaloTokenRepository, ZaloTokenRepository>();
            services.AddTransient<IZaloZNSRepository, ZaloZNSRepository>();            

            return services;
        }
    }
}

