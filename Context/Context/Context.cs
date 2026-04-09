
namespace Context.Context
{
    public partial class Context : DbContext
    {
        public Context()
        {
        }
        public Context(DbContextOptions<Context>
    options)
    : base(options)
        {
        }
        public virtual DbSet<CategoryConfig> CategoryConfig { get; set; }
        public virtual DbSet<CategoryLevel> CategoryLevel { get; set; }
        public virtual DbSet<CategoryStatus> CategoryStatus { get; set; }
        public virtual DbSet<CategorySystem> CategorySystem { get; set; }
        public virtual DbSet<CategoryType> CategoryType { get; set; }
        public virtual DbSet<Module> Module { get; set; }
        public virtual DbSet<Project> Project { get; set; }
        public virtual DbSet<ProjectFile> ProjectFile { get; set; }
        public virtual DbSet<ProjectTask> ProjectTask { get; set; }
        public virtual DbSet<ProjectTaskHistory> ProjectTaskHistory { get; set; }
        public virtual DbSet<ProjectTaskMembership> ProjectTaskMembership { get; set; }
        public virtual DbSet<Membership> Membership { get; set; }
        public virtual DbSet<MembershipHistoryURL> MembershipHistoryURL { get; set; }
        public virtual DbSet<MembershipMenu> MembershipMenu { get; set; }
        public virtual DbSet<MembershipCompany> MembershipCompany { get; set; }
        public virtual DbSet<MembershipDepartment> MembershipDepartment { get; set; }
        public virtual DbSet<MembershipToken> MembershipToken { get; set; }

        public virtual DbSet<CategoryPosition> CategoryPosition { get; set; }
        public virtual DbSet<CategoryDepartment> CategoryDepartment { get; set; }
        public virtual DbSet<CategoryRack> CategoryRack { get; set; }
        public virtual DbSet<CategoryLayer> CategoryLayer { get; set; }
        public virtual DbSet<CategoryLocation> CategoryLocation { get; set; }
        public virtual DbSet<CategoryLocationMaterial> CategoryLocationMaterial { get; set; }
        public virtual DbSet<CategoryMenu> CategoryMenu { get; set; }
        public virtual DbSet<CategoryUnit> CategoryUnit { get; set; }
        public virtual DbSet<CategoryDelivery> CategoryDelivery { get; set; }
        public virtual DbSet<CategoryFamily> CategoryFamily { get; set; }
        public virtual DbSet<CategoryVehicle> CategoryVehicle { get; set; }
        public virtual DbSet<CategoryMaterial> CategoryMaterial { get; set; }
        public virtual DbSet<CategoryTerm> CategoryTerm { get; set; }
        public virtual DbSet<CategoryTolerance> CategoryTolerance { get; set; }
        public virtual DbSet<CategoryParenthese> CategoryParenthese { get; set; }
        public virtual DbSet<CategorySealKit> CategorySealKit { get; set; }

        public virtual DbSet<Inventory> Inventory { get; set; }
        public virtual DbSet<InventoryDetail> InventoryDetail { get; set; }
        public virtual DbSet<InventoryDetailBarcode> InventoryDetailBarcode { get; set; }    
        

       
        public virtual DbSet<Material> Material { get; set; }
        public virtual DbSet<MaterialConvert> MaterialConvert { get; set; }
        public virtual DbSet<MaterialReplace> MaterialReplace { get; set; }
        public virtual DbSet<MaterialUnitCovert> MaterialUnitCovert { get; set; }
        public virtual DbSet<Factory> Factory { get; set; }
        public virtual DbSet<CategoryCompany> CategoryCompany { get; set; }
        public virtual DbSet<CategoryInvoice> CategoryInvoice { get; set; }
        public virtual DbSet<Company> Company { get; set; }
        public virtual DbSet<InvoiceInput> InvoiceInput { get; set; }
        public virtual DbSet<InvoiceInputDetail> InvoiceInputDetail { get; set; }
        public virtual DbSet<InvoiceInputFile> InvoiceInputFile { get; set; }
        public virtual DbSet<InvoiceInputHistory> InvoiceInputHistory { get; set; }
        public virtual DbSet<InvoiceOutput> InvoiceOutput { get; set; }
        public virtual DbSet<InvoiceOutputDetail> InvoiceOutputDetail { get; set; }
        public virtual DbSet<InvoiceOutputFile> InvoiceOutputFile { get; set; }
        public virtual DbSet<InvoiceInputInventory> InvoiceInputInventory { get; set; }


        public virtual DbSet<BOM> BOM { get; set; }
        public virtual DbSet<BOMDetail> BOMDetail { get; set; }
        public virtual DbSet<BOMTerm> BOMTerm { get; set; }
        public virtual DbSet<BOMStage> BOMStage { get; set; }
        public virtual DbSet<BOMCompare> BOMCompare { get; set; }
        public virtual DbSet<BOMFile> BOMFile { get; set; }


        public virtual DbSet<ProductionOrder> ProductionOrder { get; set; }
        public virtual DbSet<ProductionOrderBOM> ProductionOrderBOM { get; set; }
        public virtual DbSet<ProductionOrderBOMDetail> ProductionOrderBOMDetail { get; set; }
        public virtual DbSet<ProductionOrderDetail> ProductionOrderDetail { get; set; }
        public virtual DbSet<ProductionOrderProductionPlan> ProductionOrderProductionPlan { get; set; }
        public virtual DbSet<ProductionOrderProductionPlanBackup> ProductionOrderProductionPlanBackup { get; set; }
        public virtual DbSet<ProductionOrderProductionPlanSemi> ProductionOrderProductionPlanSemi { get; set; }
        public virtual DbSet<ProductionOrderCuttingOrder> ProductionOrderCuttingOrder { get; set; }
        public virtual DbSet<ProductionOrderSPSTOrder> ProductionOrderSPSTOrder { get; set; }
        public virtual DbSet<ProductionOrderProductionPlanMaterial> ProductionOrderProductionPlanMaterial { get; set; }
        public virtual DbSet<ProductionOrderMaterial> ProductionOrderMaterial { get; set; }
        public virtual DbSet<ProductionOrderOutputSchedule> ProductionOrderOutputSchedule { get; set; }
        public virtual DbSet<ProductionOrderFile> ProductionOrderFile { get; set; }


        public virtual DbSet<WarehouseInput> WarehouseInput { get; set; }
        public virtual DbSet<WarehouseInputDetail> WarehouseInputDetail { get; set; }
        public virtual DbSet<WarehouseInputDetailBarcode> WarehouseInputDetailBarcode { get; set; }
        public virtual DbSet<WarehouseInputDetailBarcodeMaterial> WarehouseInputDetailBarcodeMaterial { get; set; }
        public virtual DbSet<WarehouseInputMaterial> WarehouseInputMaterial { get; set; }        
        public virtual DbSet<WarehouseInputDetailCount> WarehouseInputDetailCount { get; set; }
        public virtual DbSet<WarehouseInputFile> WarehouseInputFile { get; set; }
        public virtual DbSet<WarehouseOutput> WarehouseOutput { get; set; }
        public virtual DbSet<WarehouseOutputMaterial> WarehouseOutputMaterial { get; set; }
        public virtual DbSet<WarehouseOutputDetail> WarehouseOutputDetail { get; set; }
        public virtual DbSet<WarehouseOutputDetailBarcode> WarehouseOutputDetailBarcode { get; set; }
        public virtual DbSet<WarehouseOutputDetailBarcodeMaterial> WarehouseOutputDetailBarcodeMaterial { get; set; }
        public virtual DbSet<WarehouseOutputFile> WarehouseOutputFile { get; set; }
        public virtual DbSet<WarehouseInventory> WarehouseInventory { get; set; }
        public virtual DbSet<WarehouseStock> WarehouseStock { get; set; }
        public virtual DbSet<WarehouseStockDetail> WarehouseStockDetail { get; set; }
        public virtual DbSet<WarehouseRequest> WarehouseRequest { get; set; }
        public virtual DbSet<WarehouseRequestDetail> WarehouseRequestDetail { get; set; }
        public virtual DbSet<WarehouseRequestConfirm> WarehouseRequestConfirm { get; set; }
        public virtual DbSet<WarehouseRequestFile> WarehouseRequestFile { get; set; }

        public virtual DbSet<Report> Report { get; set; }
        public virtual DbSet<ReportDetail> ReportDetail { get; set; }

        public virtual DbSet<ZaloToken> ZaloToken { get; set; }
        public virtual DbSet<ZaloZNS> ZaloZNS { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var decimalProps = modelBuilder.Model.GetEntityTypes().SelectMany(t => t.GetProperties()).Where(p => (System.Nullable.GetUnderlyingType(p.ClrType) ?? p.ClrType) == typeof(decimal));
            foreach (var property in decimalProps)
            {
                property.SetPrecision(18);
                property.SetScale(2);
            }
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

