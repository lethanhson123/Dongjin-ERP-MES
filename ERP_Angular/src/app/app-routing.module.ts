import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomepageComponent } from './homepage/homepage.component';
import { CategoryDepartmentComponent } from './Category/category-department/category-department.component';
import { CategoryLocationComponent } from './Category/category-location/category-location.component';
import { CategoryUnitComponent } from './Category/category-unit/category-unit.component';
import { LoginComponent } from './login/login.component';
import { MembershipComponent } from './Membership/membership/membership.component';
import { MaterialComponent } from './PC/material/material.component';
import { FactoryComponent } from './PC/factory/factory.component';
import { CategoryMaterialComponent } from './Category/category-material/category-material.component';
import { CategoryInvoiceComponent } from './Category/category-invoice/category-invoice.component';
import { WarehouseOutputComponent } from './Warehouse/warehouse-output/warehouse-output.component';
import { WarehouseInputComponent } from './Warehouse/warehouse-input/warehouse-input.component';
import { InvoiceInputComponent } from './Invoice/invoice-input/invoice-input.component';
import { InvoiceOutputComponent } from './Invoice/invoice-output/invoice-output.component';
import { WarehouseRequestComponent } from './Warehouse/warehouse-request/warehouse-request.component';
import { InvoiceInputInfoComponent } from './Invoice/invoice-input-info/invoice-input-info.component';
import { InvoiceOutputInfoComponent } from './Invoice/invoice-output-info/invoice-output-info.component';
import { WarehouseInputInfoComponent } from './Warehouse/warehouse-input-info/warehouse-input-info.component';
import { WarehouseOutputInfoComponent } from './Warehouse/warehouse-output-info/warehouse-output-info.component';
import { WarehouseRequestInfoComponent } from './Warehouse/warehouse-request-info/warehouse-request-info.component';
import { WarehouseInventoryComponent } from './Warehouse/warehouse-inventory/warehouse-inventory.component';
import { CategoryCompanyComponent } from './Category/category-company/category-company.component';
import { CompanyComponent } from './PC/company/company.component';
import { CategoryPositionComponent } from './Category/category-position/category-position.component';
import { WarehouseOutputDetailComponent } from './Warehouse/warehouse-output-detail/warehouse-output-detail.component';
import { WarehouseInputDetailComponent } from './Warehouse/warehouse-input-detail/warehouse-input-detail.component';
import { WarehouseOutputDetailFindComponent } from './Warehouse/warehouse-output-detail-find/warehouse-output-detail-find.component';
import { WarehouseOutputDetailScanComponent } from './Warehouse/warehouse-output-detail-scan/warehouse-output-detail-scan.component';
import { WarehouseInputDetailScanComponent } from './Warehouse/warehouse-input-detail-scan/warehouse-input-detail-scan.component';
import { CategoryDeliveryComponent } from './Category/category-delivery/category-delivery.component';
import { CategoryFamilyComponent } from './Category/category-family/category-family.component';
import { CategoryVehicleComponent } from './Category/category-vehicle/category-vehicle.component';
import { WarehouseInputDetailBarcodeComponent } from './Warehouse/warehouse-input-detail-barcode/warehouse-input-detail-barcode.component';
import { WarehouseOutputDetailBarcodeComponent } from './Warehouse/warehouse-output-detail-barcode/warehouse-output-detail-barcode.component';
import { CategoryMenuComponent } from './Category/category-menu/category-menu.component';
import { ProductionOrderComponent } from './ProductionOrder/production-order/production-order.component';
import { ProductionOrderBOMComponent } from './ProductionOrder/production-order-bom/production-order-bom.component';
import { ProductionOrderProductionPlanComponent } from './ProductionOrder/production-order-production-plan/production-order-production-plan.component';
import { ProductionOrderProductionPlanMaterialComponent } from './ProductionOrder/production-order-production-plan-material/production-order-production-plan-material.component';
import { ProductionOrderInfoComponent } from './ProductionOrder/production-order-info/production-order-info.component';
import { STOPLINEComponent } from './ProductionOrder/stopline/stopline.component';
import { WarehouseOutputByPOComponent } from './Warehouse/warehouse-output-by-po/warehouse-output-by-po.component';
import { WarehouseInputByPOComponent } from './Warehouse/warehouse-input-by-po/warehouse-input-by-po.component';
import { WarehouseRequestByPOComponent } from './Warehouse/warehouse-request-by-po/warehouse-request-by-po.component';
import { ProductionOrderProductionPlan001Component } from './ProductionOrder/production-order-production-plan001/production-order-production-plan001.component';
import { BOMComponent } from './PC/bom/bom.component';
import { BOMInfoComponent } from './PC/bominfo/bominfo.component';
import { BOMDetailComponent } from './PC/bomdetail/bomdetail.component';
import { WarehouseOutputDetailBarcodeFindComponent } from './Warehouse/warehouse-output-detail-barcode-find/warehouse-output-detail-barcode-find.component';
import { CategoryLayerComponent } from './Category/category-layer/category-layer.component';
import { CategoryRackComponent } from './Category/category-rack/category-rack.component';
import { LocationRuleComponent } from './Warehouse/location-rule/location-rule.component';
import { LocationDiagramComponent } from './Warehouse/location-diagram/location-diagram.component';
import { WarehouseInputDetailBarcodeScanComponent } from './Warehouse/warehouse-input-detail-barcode-scan/warehouse-input-detail-barcode-scan.component';
import { WarehouseInputDetailBarcodeImportComponent } from './Warehouse/warehouse-input-detail-barcode-import/warehouse-input-detail-barcode-import.component';
import { MembershipInfoComponent } from './Membership/membership-info/membership-info.component';
import { Warehouse001Component } from './Dashboard/warehouse001/warehouse001.component';
import { InvoiceInputInventoryComponent } from './Invoice/invoice-input-inventory/invoice-input-inventory.component';
import { WarehouseOutputDetailScanCancelComponent } from './Warehouse/warehouse-output-detail-scan-cancel/warehouse-output-detail-scan-cancel.component';
import { ProductionOrderProductionPlanMaterial001Component } from './ProductionOrder/production-order-production-plan-material001/production-order-production-plan-material001.component';
import { WarehouseInventoryCompanyComponent } from './Warehouse/warehouse-inventory-company/warehouse-inventory-company.component';
import { BOMCompareComponent } from './PC/bomcompare/bomcompare.component';
import { WarehouseStockComponent } from './Warehouse/warehouse-stock/warehouse-stock.component';
import { InvoiceInputAdminComponent } from './Invoice/invoice-input-admin/invoice-input-admin.component';
import { WarehouseInputAdminComponent } from './Warehouse/warehouse-input-admin/warehouse-input-admin.component';
import { WarehouseOutputAdminComponent } from './Warehouse/warehouse-output-admin/warehouse-output-admin.component';
import { WarehouseRequestAdminComponent } from './Warehouse/warehouse-request-admin/warehouse-request-admin.component';
import { ProductionOrderAdminComponent } from './ProductionOrder/production-order-admin/production-order-admin.component';
import { MembershipHistoryURLComponent } from './Membership/membership-history-url/membership-history-url.component';
import { ZaloTokenComponent } from './Membership/zalo-token/zalo-token.component';
import { WarehouseInputDetailBarcodeMobileComponent } from './Warehouse/warehouse-input-detail-barcode-mobile/warehouse-input-detail-barcode-mobile.component';
import { WarehouseOutputReturnComponent } from './Warehouse/warehouse-output-return/warehouse-output-return.component';
import { BOMCompare01Component } from './PC/bomcompare01/bomcompare01.component';
import { BOMCompare02Component } from './PC/bomcompare02/bomcompare02.component';
import { WarehouseCheckComponent } from './Warehouse/warehouse-check/warehouse-check.component';
import { FinishGoodsCheckComponent } from './Warehouse/finish-goods-check/finish-goods-check.component';
import { WarehouseRequestByCategoryDepartmentComponent } from './Warehouse/warehouse-request-by-category-department/warehouse-request-by-category-department.component';
import { WarehouseRequestByCategoryDepartmentInfoComponent } from './Warehouse/warehouse-request-by-category-department-info/warehouse-request-by-category-department-info.component';
import { WarehouseInputDetailBarcodeCompareComponent } from './Warehouse/warehouse-input-detail-barcode-compare/warehouse-input-detail-barcode-compare.component';
import { WarehouseInputInfoAdminComponent } from './Warehouse/warehouse-input-info-admin/warehouse-input-info-admin.component';
import { WarehouseInputDetailBarcodeComparePartNoComponent } from './Warehouse/warehouse-input-detail-barcode-compare-part-no/warehouse-input-detail-barcode-compare-part-no.component';
import { WarehouseCheckDJMComponent } from './Warehouse/warehouse-check-djm/warehouse-check-djm.component';
import { FinishGoodsCheckDJMComponent } from './Warehouse/finish-goods-check-djm/finish-goods-check-djm.component';
import { CutCheckDJMComponent } from './Warehouse/cut-check-djm/cut-check-djm.component';
import { MaterialUnitCovertComponent } from './PC/material-unit-covert/material-unit-covert.component';
import { InvoiceInputInfoAdminComponent } from './Invoice/invoice-input-info-admin/invoice-input-info-admin.component';
import { WarehouseOutputInfoAdminComponent } from './Warehouse/warehouse-output-info-admin/warehouse-output-info-admin.component';
import { CategoryTermComponent } from './Category/category-term/category-term.component';
import { CategoryToleranceComponent } from './Category/category-tolerance/category-tolerance.component';
import { InventoryComponent } from './Inventory/inventory/inventory.component';
import { InventoryScanComponent } from './Inventory/inventory-scan/inventory-scan.component';
import { InventoryInfoComponent } from './Inventory/inventory-info/inventory-info.component';
import { WarehouseStockByInvoiceComponent } from './Warehouse/warehouse-stock-by-invoice/warehouse-stock-by-invoice.component';
import { CategoryParentheseComponent } from './Category/category-parenthese/category-parenthese.component';
import { ProductionSearchComponent } from './ProductionOrder/production-search/production-search.component';
import { ProductionTrackingComponent } from './ProductionOrder/production-tracking/production-tracking.component';
import { ProductionTrackingHOOKRACKComponent } from './ProductionOrder/production-tracking-hookrack/production-tracking-hookrack.component';
import { InventoryScanMobileComponent } from './Inventory/inventory-scan-mobile/inventory-scan-mobile.component';
import { WarehouseStockLongTermComponent } from './Warehouse/warehouse-stock-long-term/warehouse-stock-long-term.component';
import { ProductionOrderProductionPlanSemiComponent } from './ProductionOrder/production-order-production-plan-semi/production-order-production-plan-semi.component';
import { WarehouseStockHOOKRACKComponent } from './ProductionOrder/warehouse-stock-hookrack/warehouse-stock-hookrack.component';
import { InventoryScanDesktopComponent } from './Inventory/inventory-scan-desktop/inventory-scan-desktop.component';
import { HookRackCheckComponent } from './ProductionOrder/hook-rack-check/hook-rack-check.component';
import { WarehouseInputInfoStockComponent } from './Warehouse/warehouse-input-info-stock/warehouse-input-info-stock.component';
import { WarehouseInventoryInvoiceComponent } from './Warehouse/warehouse-inventory-invoice/warehouse-inventory-invoice.component';
import { WarehouseInventoryValueComponent } from './Warehouse/warehouse-inventory-value/warehouse-inventory-value.component';
import { HookRackChangeComponent } from './ProductionOrder/hook-rack-change/hook-rack-change.component';
import { ProductionTracking2026Component } from './ProductionOrder/production-tracking2026/production-tracking2026.component';
import { WarehouseOutputProductionComponent } from './Warehouse/warehouse-output-production/warehouse-output-production.component';
import { WarehouseInputProductionComponent } from './Warehouse/warehouse-input-production/warehouse-input-production.component';
import { WarehouseInputDetailQuantityGAPComponent } from './Warehouse/warehouse-input-detail-quantity-gap/warehouse-input-detail-quantity-gap.component';
import { WarehouseOutputDetailBarcodeFIFOComponent } from './Warehouse/warehouse-output-detail-barcode-fifo/warehouse-output-detail-barcode-fifo.component';
import { CategoryLevelComponent } from './Category/category-level/category-level.component';
import { CategoryStatusComponent } from './Category/category-status/category-status.component';
import { CategoryTypeComponent } from './Category/category-type/category-type.component';
import { ModuleComponent } from './Project/module/module.component';
import { ProjectComponent } from './Project/project/project.component';
import { ProjectTaskComponent } from './Project/project-task/project-task.component';
import { ProjectTaskHistoryComponent } from './Project/project-task-history/project-task-history.component';
import { ProjectInfoComponent } from './Project/project-info/project-info.component';
import { CategorySystemComponent } from './Category/category-system/category-system.component';
import { CategoryConfigComponent } from './Category/category-config/category-config.component';
import { ZaloComponent } from './Membership/zalo/zalo.component';
import { CategorySealKitComponent } from './Category/category-seal-kit/category-seal-kit.component';
import { HookRackComponent } from './ProductionOrder/hook-rack/hook-rack.component';
import { InvoiceInputHistoryComponent } from './Invoice/invoice-input-history/invoice-input-history.component';


const routes: Routes = [
  { path: '', redirectTo: '/Homepage', pathMatch: 'full' },
  {
    path: 'Homepage', component: HomepageComponent, data: { title: 'Homepage' }
  },
  {
    path: 'Login', component: LoginComponent, data: { title: 'Login' }
  },
  {
    path: 'Login/:ID', component: LoginComponent, data: { title: 'Login' }
  },

  {
    path: 'Membership', component: MembershipComponent, data: { title: 'Membership' }
  },
  {
    path: 'MembershipInfo', component: MembershipInfoComponent, data: { title: 'Membership Info' }
  },
  {
    path: 'MembershipHistoryURL', component: MembershipHistoryURLComponent, data: { title: 'Membership History' }
  },
  {
    path: 'CategoryPosition', component: CategoryPositionComponent, data: { title: 'Category Position' }
  },
  {
    path: 'CategoryDepartment', component: CategoryDepartmentComponent, data: { title: 'Category Department' }
  },
  {
    path: 'CategoryLocation', component: CategoryLocationComponent, data: { title: 'Category Location' }
  },
  {
    path: 'CategoryUnit', component: CategoryUnitComponent, data: { title: 'Category Unit' }
  },
  {
    path: 'CategoryMaterial', component: CategoryMaterialComponent, data: { title: 'Category Material' }
  },
  {
    path: 'CategoryInvoice', component: CategoryInvoiceComponent, data: { title: 'Category Invoice' }
  },
  {
    path: 'CategoryCompany', component: CategoryCompanyComponent, data: { title: 'Category Company' }
  },
  {
    path: 'CategoryDelivery', component: CategoryDeliveryComponent, data: { title: 'Category Delivery' }
  },
  {
    path: 'CategoryFamily', component: CategoryFamilyComponent, data: { title: 'Category Family' }
  },
  {
    path: 'CategoryVehicle', component: CategoryVehicleComponent, data: { title: 'Category Vehicle' }
  },
  {
    path: 'CategoryMenu', component: CategoryMenuComponent, data: { title: 'Category Menu' }
  },
  {
    path: 'CategoryLayer', component: CategoryLayerComponent, data: { title: 'Category Layer' }
  },
  {
    path: 'CategoryRack', component: CategoryRackComponent, data: { title: 'Category Rack' }
  },
  {
    path: 'CategoryTerm', component: CategoryTermComponent, data: { title: 'GOP - Thông số' }
  },
  {
    path: 'CategoryTolerance', component: CategoryToleranceComponent, data: { title: 'Category Tolerance' }
  },
  {
    path: 'CategoryParenthese', component: CategoryParentheseComponent, data: { title: 'Category Parenthese' }
  },
  {
    path: 'CategoryLevel', component: CategoryLevelComponent, data: { title: 'Category Level' }
  },
  {
    path: 'CategoryStatus', component: CategoryStatusComponent, data: { title: 'Category Status' }
  },
  {
    path: 'CategoryType', component: CategoryTypeComponent, data: { title: 'Category Type' }
  },
  {
    path: 'CategorySystem', component: CategorySystemComponent, data: { title: 'Category System' }
  },
   {
    path: 'CategoryConfig', component: CategoryConfigComponent, data: { title: 'Category Config' }
  },
     {
    path: 'CategorySealKit', component: CategorySealKitComponent, data: { title: 'Category Seal Kit' }
  },

  {
    path: 'Module', component: ModuleComponent, data: { title: 'Module' }
  },
  {
    path: 'Project', component: ProjectComponent, data: { title: 'Project' }
  },
  {
    path: 'ProjectInfo/:ID', component: ProjectInfoComponent, data: { title: 'Project Info' }
  },
  {
    path: 'ProjectTask', component: ProjectTaskComponent, data: { title: 'Project Task' }
  },
  {
    path: 'ProjectTaskHistory', component: ProjectTaskHistoryComponent, data: { title: 'Project Task History' }
  },

  {
    path: 'Company', component: CompanyComponent, data: { title: 'Company' }
  },
  {
    path: 'Factory', component: FactoryComponent, data: { title: 'Factory' }
  },



  {
    path: 'Material', component: MaterialComponent, data: { title: 'Material' }
  },
  {
    path: 'MaterialUnitCovert', component: MaterialUnitCovertComponent, data: { title: 'Material Unit Covert' }
  },


  {
    path: 'BOM', component: BOMComponent, data: { title: 'BOM' }
  },
  {
    path: 'BOMInfo/:ID', component: BOMInfoComponent, data: { title: 'BOM Info' }
  },
  {
    path: 'BOMDetail', component: BOMDetailComponent, data: { title: 'BOM Detail' }
  },
  {
    path: 'BOMCompare', component: BOMCompareComponent, data: { title: 'BOM Compare' }
  },
  {
    path: 'BOMCompare01', component: BOMCompare01Component, data: { title: 'BOM Compare by ECN' }
  },
  {
    path: 'BOMCompare02', component: BOMCompare02Component, data: { title: 'BOM Compare by ECN' }
  },

  {
    path: 'WarehouseInput', component: WarehouseInputComponent, data: { title: 'Input (B01)' }
  },
  {
    path: 'WarehouseInputProduction', component: WarehouseInputProductionComponent, data: { title: 'Input Production' }
  },
  {
    path: 'WarehouseOutput', component: WarehouseOutputComponent, data: { title: 'Output Release Order' }
  },
  {
    path: 'WarehouseOutputProduction', component: WarehouseOutputProductionComponent, data: { title: 'Output Production' }
  },
  {
    path: 'WarehouseRequest', component: WarehouseRequestComponent, data: { title: 'Material Request' }
  },
  {
    path: 'WarehouseRequestByCategoryDepartment', component: WarehouseRequestByCategoryDepartmentComponent, data: { title: 'Request By Department' }
  },
  {
    path: 'WarehouseInputDetailQuantityGAP', component: WarehouseInputDetailQuantityGAPComponent, data: { title: 'Input GAP' }
  },
  {
    path: 'WarehouseOutputDetailBarcodeFIFO', component: WarehouseOutputDetailBarcodeFIFOComponent, data: { title: 'Output FIFO' }
  },


  {
    path: 'WarehouseInputAdmin', component: WarehouseInputAdminComponent, data: { title: 'Admin: Warehouse Input' }
  },
  {
    path: 'WarehouseOutputAdmin', component: WarehouseOutputAdminComponent, data: { title: 'Admin: Warehouse Output' }
  },
  {
    path: 'WarehouseRequestAdmin', component: WarehouseRequestAdminComponent, data: { title: 'Admin: Warehouse Request' }
  },


  {
    path: 'WarehouseInputDetail', component: WarehouseInputDetailComponent, data: { title: 'Input History' }
  },
  {
    path: 'WarehouseOutputDetail', component: WarehouseOutputDetailComponent, data: { title: 'Output History' }
  },
  {
    path: 'WarehouseInputDetailBarcode', component: WarehouseInputDetailBarcodeComponent, data: { title: 'Input Barcode (B11)' }
  },
  {
    path: 'WarehouseInputDetailBarcode/:SearchString', component: WarehouseInputDetailBarcodeComponent, data: { title: 'Input Barcode (B11)' }
  },
  {
    path: 'WarehouseInputDetailBarcodeMobile', component: WarehouseInputDetailBarcodeMobileComponent, data: { title: 'Input Barcode (B11) Mobile' }
  },
  {
    path: 'WarehouseOutputDetailBarcode', component: WarehouseOutputDetailBarcodeComponent, data: { title: 'Output Barcode' }
  },
  {
    path: 'WarehouseOutputDetailScan', component: WarehouseOutputDetailScanComponent, data: { title: 'Scan out (B10)' }
  },
  {
    path: 'WarehouseOutputDetailScanCancel', component: WarehouseOutputDetailScanCancelComponent, data: { title: 'Scan out (Cancel)' }
  },
  {
    path: 'WarehouseInputDetailScan', component: WarehouseInputDetailScanComponent, data: { title: 'Scan in (Line)' }
  },
  {
    path: 'WarehouseInputDetailBarcodeImport', component: WarehouseInputDetailBarcodeImportComponent, data: { title: 'Scan in + Print (B03)' }
  },
  {
    path: 'WarehouseInputDetailBarcodeCompare', component: WarehouseInputDetailBarcodeCompareComponent, data: { title: 'Barcode Compare MES and ERP' }
  },
  {
    path: 'WarehouseInputDetailBarcodeComparePartNo', component: WarehouseInputDetailBarcodeComparePartNoComponent, data: { title: 'PART NO Compare MES and ERP' }
  },
  {
    path: 'WarehouseInputInfo/:ID', component: WarehouseInputInfoComponent, data: { title: 'Warehouse Input Info' }
  },
  {
    path: 'WarehouseInputInfoAdmin/:ID', component: WarehouseInputInfoAdminComponent, data: { title: 'Admin: Warehouse Input Info' }
  },
  {
    path: 'WarehouseInputInfoStock/:ID', component: WarehouseInputInfoStockComponent, data: { title: 'Stock: Warehouse Input Info' }
  },
  {
    path: 'WarehouseOutputInfo/:ID', component: WarehouseOutputInfoComponent, data: { title: 'Warehouse Output Info' }
  },
  {
    path: 'WarehouseOutputInfoAdmin/:ID', component: WarehouseOutputInfoAdminComponent, data: { title: 'Admin Warehouse Output Info' }
  },
  {
    path: 'WarehouseRequestInfo/:ID', component: WarehouseRequestInfoComponent, data: { title: 'Warehouse Request Info' }
  },
  {
    path: 'WarehouseRequestByCategoryDepartmentInfo/:ID', component: WarehouseRequestByCategoryDepartmentInfoComponent, data: { title: 'Warehouse Request By Department Info' }
  },
  {
    path: 'WarehouseOutputDetailBarcodeFind/:ID', component: WarehouseOutputDetailBarcodeFindComponent, data: { title: 'Warehouse Output Find' }
  },
  {
    path: 'Find/:ID', component: WarehouseOutputDetailBarcodeFindComponent, data: { title: 'Warehouse Output Find' }
  },
  {
    path: 'Scan', component: WarehouseInputDetailBarcodeScanComponent, data: { title: 'Scan Location' }
  },
  {
    path: 'WarehouseOutputReturn', component: WarehouseOutputReturnComponent, data: { title: 'Warehouse Return' }
  },
  {
    path: 'WHFind', component: WarehouseCheckComponent, data: { title: 'DJG - Warehouse: Kiểm kê' }
  },
  {
    path: 'FGFind', component: FinishGoodsCheckComponent, data: { title: 'DJG - Finish Goods: Kiểm kê' }
  },
  {
    path: 'WHFind2', component: WarehouseCheckDJMComponent, data: { title: 'DJM - Warehouse: Kiểm kê' }
  },
  {
    path: 'FGFind2', component: FinishGoodsCheckDJMComponent, data: { title: 'DJM - Finish Goods: Kiểm kê' }
  },
  {
    path: 'CUTFind2', component: CutCheckDJMComponent, data: { title: 'DJM - CUT: Kiểm kê' }
  },

  {
    path: 'WarehouseRequestByPO', component: WarehouseRequestByPOComponent, data: { title: 'Request by PO' }
  },
  {
    path: 'WarehouseInputByPO', component: WarehouseInputByPOComponent, data: { title: 'Input by PO' }
  },
  {
    path: 'WarehouseOutputByPO', component: WarehouseOutputByPOComponent, data: { title: 'Output by PO' }
  },
  {
    path: 'LocationRule', component: LocationRuleComponent, data: { title: 'Location Rule' }
  },
  {
    path: 'LocationDiagram', component: LocationDiagramComponent, data: { title: 'Diagram' }
  },

  {
    path: 'InvoiceInput', component: InvoiceInputComponent, data: { title: 'Invoice Input' }
  },
  {
    path: 'InvoiceInputAdmin', component: InvoiceInputAdminComponent, data: { title: 'Admin: Invoice Input' }
  },
  {
    path: 'InvoiceOutput', component: InvoiceOutputComponent, data: { title: 'Invoice Output' }
  },
  {
    path: 'InvoiceInputInfo/:ID', component: InvoiceInputInfoComponent, data: { title: 'Invoice Input Info' }
  },
  {
    path: 'InvoiceInputInfoAdmin/:ID', component: InvoiceInputInfoAdminComponent, data: { title: 'Admin: Invoice Input Info' }
  },
  {
    path: 'InvoiceOutputInfo/:ID', component: InvoiceOutputInfoComponent, data: { title: 'Invoice Output Info' }
  },
  {
    path: 'InvoiceInputInventory', component: InvoiceInputInventoryComponent, data: { title: 'Invoice Input Inventory' }
  },
    {
    path: 'InvoiceInputHistory', component: InvoiceInputHistoryComponent, data: { title: 'Invoice Input History' }
  },





  {
    path: 'ProductionOrder', component: ProductionOrderComponent, data: { title: 'Production Order' }
  },
  {
    path: 'ProductionOrderAdmin', component: ProductionOrderAdminComponent, data: { title: 'Admin: Production Order' }
  },
  {
    path: 'ProductionOrderBOM', component: ProductionOrderBOMComponent, data: { title: 'Production Order: BOM' }
  },
  {
    path: 'ProductionOrderProductionPlan', component: ProductionOrderProductionPlanComponent, data: { title: 'Production Order: Plan' }
  },
  {
    path: 'ProductionOrderProductionPlanMaterial', component: ProductionOrderProductionPlanMaterialComponent, data: { title: 'Production Order: Material' }
  },
  {
    path: 'ProductionOrderInfo/:ID', component: ProductionOrderInfoComponent, data: { title: 'Production Order Info' }
  },
  {
    path: 'STOPLINE', component: STOPLINEComponent, data: { title: 'Production Order: STOPLINE' }
  },
  {
    path: 'ProductionOrderProductionPlan001', component: ProductionOrderProductionPlan001Component, data: { title: 'Customer: Production Order Plan' }
  },
  {
    path: 'ProductionOrderProductionPlanMaterial001', component: ProductionOrderProductionPlanMaterial001Component, data: { title: 'Customer: Production Order Material' }
  },
  {
    path: 'ProductionSearch', component: ProductionSearchComponent, data: { title: 'Production Search' }
  },
  {
    path: 'ProductionTracking', component: ProductionTrackingComponent, data: { title: 'Production Tracking' }
  },
  {
    path: 'ProductionTracking2026', component: ProductionTracking2026Component, data: { title: 'Production Tracking 2026' }
  },
  {
    path: 'ProductionTrackingHOOKRACK', component: ProductionTrackingHOOKRACKComponent, data: { title: 'Production Tracking: HOOKRACK' }
  },
  {
    path: 'ProductionOrderProductionPlanSemi', component: ProductionOrderProductionPlanSemiComponent, data: { title: 'Production Semi-Finished' }
  },
  {
    path: 'HookRackCheck', component: HookRackCheckComponent, data: { title: 'HookRack Check' }
  },
  {
    path: 'HookRackChange', component: HookRackChangeComponent, data: { title: 'HookRack Change' }
  },
    {
    path: 'HookRack', component: HookRackComponent, data: { title: 'HookRack' }
  },



  {
    path: 'Warehouse001', component: Warehouse001Component, data: { title: 'Dashbroad: Warehouse 001' }
  },
  {
    path: 'WarehouseInventory', component: WarehouseInventoryComponent, data: { title: 'Dashbroad: Stock By Date scan' }
  },
  {
    path: 'WarehouseInventoryInvoice', component: WarehouseInventoryInvoiceComponent, data: { title: 'Dashbroad: Stock By Invoice ETA' }
  },
  {
    path: 'WarehouseInventoryCompany', component: WarehouseInventoryCompanyComponent, data: { title: 'Dashbroad: Inventory By Company' }
  },
  {
    path: 'WarehouseInventoryValue', component: WarehouseInventoryValueComponent, data: { title: 'Dashbroad: Stock By Value' }
  },
  {
    path: 'WarehouseStock', component: WarehouseStockComponent, data: { title: 'Stock by time' }
  },
  {
    path: 'WarehouseStockByInvoice', component: WarehouseStockByInvoiceComponent, data: { title: 'Stock by Invoice' }
  },
  {
    path: 'WarehouseStockLongTerm', component: WarehouseStockLongTermComponent, data: { title: 'Stock long-term' }
  },
  {
    path: 'WarehouseStockHOOKRACK', component: WarehouseStockHOOKRACKComponent, data: { title: 'Stock of HOOKRACK' }
  },
  {
    path: 'ZaloSetting', component: ZaloTokenComponent, data: { title: 'Zalo Setting' }
  },
    {
    path: 'Zalo', component: ZaloComponent, data: { title: 'Zalo' }
  },

  {
    path: 'Inventory', component: InventoryComponent, data: { title: 'Inventory' }
  },
  {
    path: 'InventoryInfo/:ID', component: InventoryInfoComponent, data: { title: 'Inventory Info' }
  },
  {
    path: 'KiemKe', component: InventoryScanComponent, data: { title: 'Kiểm kê' }
  },
  {
    path: 'KiemKeMobile', component: InventoryScanMobileComponent, data: { title: 'Kiểm kê mobile' }
  },
  {
    path: 'KiemKeDesktop', component: InventoryScanDesktopComponent, data: { title: 'Kiểm kê desktop' }
  },

];

@NgModule({
  imports: [RouterModule.forRoot(routes, { useHash: true, initialNavigation: 'enabled' })],
  exports: [RouterModule]
})
export class AppRoutingModule { }









































































