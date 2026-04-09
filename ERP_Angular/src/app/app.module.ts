import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { CKEditorModule } from 'ngx-ckeditor';
import { ChartsModule } from 'ng2-charts';
import { CookieService } from 'ngx-cookie-service';
import { MAT_DATE_LOCALE } from '@angular/material/core';
import { MaterialModule } from './material/material.module';
import { GoogleMapsModule } from '@angular/google-maps';
import { NotificationService } from './shared/Notification.service';
import { AppComponent } from './app.component';
import { LoadingComponent } from './loading/loading.component';
import { HomepageComponent } from './homepage/homepage.component';

import { CategoryDepartmentComponent } from './Category/category-department/category-department.component';
import { CategoryLocationComponent } from './Category/category-location/category-location.component';
import { CategoryUnitComponent } from './Category/category-unit/category-unit.component';
import { LoginComponent } from './login/login.component';

import { MembershipComponent } from './Membership/membership/membership.component';
import { MembershipDetailComponent } from './Membership/membership-detail/membership-detail.component';
import { MembershipModalComponent } from './Membership/membership-modal/membership-modal.component';

import { MaterialComponent } from './PC/material/material.component';

import { CategoryMaterialComponent } from './Category/category-material/category-material.component';

import { FactoryComponent } from './PC/factory/factory.component';
import { CategoryInvoiceComponent } from './Category/category-invoice/category-invoice.component';


import { MaterialModalComponent } from './PC/material-modal/material-modal.component';
import { WarehouseInputComponent } from './Warehouse/warehouse-input/warehouse-input.component';
import { WarehouseInputModalComponent } from './Warehouse/warehouse-input-modal/warehouse-input-modal.component';
import { WarehouseOutputComponent } from './Warehouse/warehouse-output/warehouse-output.component';
import { WarehouseOutputModalComponent } from './Warehouse/warehouse-output-modal/warehouse-output-modal.component';
import { WarehouseModalComponent } from './Warehouse/warehouse-modal/warehouse-modal.component';
import { InvoiceInputComponent } from './Invoice/invoice-input/invoice-input.component';
import { InvoiceInputModalComponent } from './Invoice/invoice-input-modal/invoice-input-modal.component';
import { InvoiceOutputModalComponent } from './Invoice/invoice-output-modal/invoice-output-modal.component';
import { InvoiceOutputComponent } from './Invoice/invoice-output/invoice-output.component';
import { InvoiceInputFileModalComponent } from './Invoice/invoice-input-file-modal/invoice-input-file-modal.component';
import { WarehouseRequestComponent } from './Warehouse/warehouse-request/warehouse-request.component';
import { WarehouseRequestModalComponent } from './Warehouse/warehouse-request-modal/warehouse-request-modal.component';
import { InvoiceOutputInfoComponent } from './Invoice/invoice-output-info/invoice-output-info.component';
import { InvoiceInputInfoComponent } from './Invoice/invoice-input-info/invoice-input-info.component';
import { WarehouseOutputInfoComponent } from './Warehouse/warehouse-output-info/warehouse-output-info.component';
import { WarehouseInputInfoComponent } from './Warehouse/warehouse-input-info/warehouse-input-info.component';
import { WarehouseRequestInfoComponent } from './Warehouse/warehouse-request-info/warehouse-request-info.component';
import { WarehouseInventoryComponent } from './Warehouse/warehouse-inventory/warehouse-inventory.component';
import { CategoryCompanyComponent } from './Category/category-company/category-company.component';
import { CompanyComponent } from './PC/company/company.component';
import { CategoryPositionComponent } from './Category/category-position/category-position.component';
import { WarehouseInputDetailComponent } from './Warehouse/warehouse-input-detail/warehouse-input-detail.component';
import { WarehouseOutputDetailComponent } from './Warehouse/warehouse-output-detail/warehouse-output-detail.component';
import { WarehouseOutputDetailFindComponent } from './Warehouse/warehouse-output-detail-find/warehouse-output-detail-find.component';
import { WarehouseOutputDetailBarcodeFindComponent } from './Warehouse/warehouse-output-detail-barcode-find/warehouse-output-detail-barcode-find.component';
import { WarehouseOutputDetailScanComponent } from './Warehouse/warehouse-output-detail-scan/warehouse-output-detail-scan.component';
import { WarehouseInputDetailScanComponent } from './Warehouse/warehouse-input-detail-scan/warehouse-input-detail-scan.component';
import { CategoryDeliveryComponent } from './Category/category-delivery/category-delivery.component';
import { CategoryFamilyComponent } from './Category/category-family/category-family.component';
import { CategoryVehicleComponent } from './Category/category-vehicle/category-vehicle.component';

import { WarehouseInputDetailBarcodeComponent } from './Warehouse/warehouse-input-detail-barcode/warehouse-input-detail-barcode.component';
import { WarehouseOutputDetailBarcodeComponent } from './Warehouse/warehouse-output-detail-barcode/warehouse-output-detail-barcode.component';
import { WarehouseOutputDetailBarcodeModalComponent } from './Warehouse/warehouse-output-detail-barcode-modal/warehouse-output-detail-barcode-modal.component';
import { WarehouseInputDetailBarcodeModalComponent } from './Warehouse/warehouse-input-detail-barcode-modal/warehouse-input-detail-barcode-modal.component';
import { WarehouseOutputDetailBarcodeHistoryModalComponent } from './Warehouse/warehouse-output-detail-barcode-history-modal/warehouse-output-detail-barcode-history-modal.component';
import { CategoryMenuComponent } from './Category/category-menu/category-menu.component';


import { ProductionOrderComponent } from './ProductionOrder/production-order/production-order.component';
import { ProductionOrderInfoComponent } from './ProductionOrder/production-order-info/production-order-info.component';
import { ProductionOrderProductionPlanComponent } from './ProductionOrder/production-order-production-plan/production-order-production-plan.component';
import { ProductionOrderProductionPlanMaterialComponent } from './ProductionOrder/production-order-production-plan-material/production-order-production-plan-material.component';
import { ProductionOrderBOMComponent } from './ProductionOrder/production-order-bom/production-order-bom.component';
import { ProductionOrderModalComponent } from './ProductionOrder/production-order-modal/production-order-modal.component';
import { STOPLINEComponent } from './ProductionOrder/stopline/stopline.component';
import { WarehouseRequestByPOComponent } from './Warehouse/warehouse-request-by-po/warehouse-request-by-po.component';
import { WarehouseInputByPOComponent } from './Warehouse/warehouse-input-by-po/warehouse-input-by-po.component';
import { WarehouseOutputByPOComponent } from './Warehouse/warehouse-output-by-po/warehouse-output-by-po.component';
import { ProductionOrderProductionPlan001Component } from './ProductionOrder/production-order-production-plan001/production-order-production-plan001.component';
import { BOMComponent } from './PC/bom/bom.component';
import { BOMInfoComponent } from './PC/bominfo/bominfo.component';
import { BOMDetailComponent } from './PC/bomdetail/bomdetail.component';
import { CategoryLocationModalComponent } from './Category/category-location-modal/category-location-modal.component';
import { CategoryLayerComponent } from './Category/category-layer/category-layer.component';
import { CategoryRackComponent } from './Category/category-rack/category-rack.component';
import { LocationRuleComponent } from './Warehouse/location-rule/location-rule.component';
import { LocationDiagramComponent } from './Warehouse/location-diagram/location-diagram.component';
import { WarehouseInputDetailBarcodeDiagramComponent } from './Warehouse/warehouse-input-detail-barcode-diagram/warehouse-input-detail-barcode-diagram.component';
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
import { BOMCompare02Component } from './PC/bomcompare02/bomcompare02.component';
import { BOMCompare01Component } from './PC/bomcompare01/bomcompare01.component';
import { WarehouseCheckComponent } from './Warehouse/warehouse-check/warehouse-check.component';
import { FinishGoodsCheckComponent } from './Warehouse/finish-goods-check/finish-goods-check.component';
import { WarehouseRequestByCategoryDepartmentComponent } from './Warehouse/warehouse-request-by-category-department/warehouse-request-by-category-department.component';
import { WarehouseRequestByCategoryDepartmentInfoComponent } from './Warehouse/warehouse-request-by-category-department-info/warehouse-request-by-category-department-info.component';
import { WarehouseInputDetailBarcodeCompareComponent } from './Warehouse/warehouse-input-detail-barcode-compare/warehouse-input-detail-barcode-compare.component';
import { WarehouseInputInfoAdminComponent } from './Warehouse/warehouse-input-info-admin/warehouse-input-info-admin.component';
import { WarehouseInputDetailBarcodeComparePartNoComponent } from './Warehouse/warehouse-input-detail-barcode-compare-part-no/warehouse-input-detail-barcode-compare-part-no.component';
import { FinishGoodsCheckDJMComponent } from './Warehouse/finish-goods-check-djm/finish-goods-check-djm.component';
import { WarehouseCheckDJMComponent } from './Warehouse/warehouse-check-djm/warehouse-check-djm.component';
import { CutCheckDJMComponent } from './Warehouse/cut-check-djm/cut-check-djm.component';
import { MaterialUnitCovertComponent } from './PC/material-unit-covert/material-unit-covert.component';
import { InvoiceInputInfoAdminComponent } from './Invoice/invoice-input-info-admin/invoice-input-info-admin.component';
import { WarehouseInputDetailBarcodeMaterialModalComponent } from './Warehouse/warehouse-input-detail-barcode-material-modal/warehouse-input-detail-barcode-material-modal.component';
import { ProductionOrderInfoBackupComponent } from './ProductionOrder/production-order-info-backup/production-order-info-backup.component';
import { WarehouseOutputInfoAdminComponent } from './Warehouse/warehouse-output-info-admin/warehouse-output-info-admin.component';
import { CategoryToleranceComponent } from './Category/category-tolerance/category-tolerance.component';
import { CategoryTermComponent } from './Category/category-term/category-term.component';
import { InventoryComponent } from './Inventory/inventory/inventory.component';
import { InventoryScanComponent } from './Inventory/inventory-scan/inventory-scan.component';
import { InventoryInfoComponent } from './Inventory/inventory-info/inventory-info.component';
import { WarehouseStockByInvoiceComponent } from './Warehouse/warehouse-stock-by-invoice/warehouse-stock-by-invoice.component';
import { CategoryParentheseComponent } from './Category/category-parenthese/category-parenthese.component';
import { ProductionSearchComponent } from './ProductionOrder/production-search/production-search.component';
import { ProductionTrackingComponent } from './ProductionOrder/production-tracking/production-tracking.component';
import { ProductionTrackingKOMAXComponent } from './ProductionOrder/production-tracking-komax/production-tracking-komax.component';
import { ProductionTrackingLPComponent } from './ProductionOrder/production-tracking-lp/production-tracking-lp.component';
import { ProductionTrackingSPSTComponent } from './ProductionOrder/production-tracking-spst/production-tracking-spst.component';
import { ProductionTrackingSHIELDWIREComponent } from './ProductionOrder/production-tracking-shieldwire/production-tracking-shieldwire.component';
import { ProductionTrackingHOOKRACKComponent } from './ProductionOrder/production-tracking-hookrack/production-tracking-hookrack.component';
import { ProductionTrackingHookRackModalComponent } from './ProductionOrder/production-tracking-hook-rack-modal/production-tracking-hook-rack-modal.component';
import { InventoryScanMobileComponent } from './Inventory/inventory-scan-mobile/inventory-scan-mobile.component';
import { WarehouseStockLongTermComponent } from './Warehouse/warehouse-stock-long-term/warehouse-stock-long-term.component';
import { ProductionOrderProductionPlanSemiComponent } from './ProductionOrder/production-order-production-plan-semi/production-order-production-plan-semi.component';
import { WarehouseStockHOOKRACKComponent } from './ProductionOrder/warehouse-stock-hookrack/warehouse-stock-hookrack.component';
import { InventoryScanDesktopComponent } from './Inventory/inventory-scan-desktop/inventory-scan-desktop.component';
import { ProductionOrderProductionPlanBackupModalComponent } from './ProductionOrder/production-order-production-plan-backup-modal/production-order-production-plan-backup-modal.component';
import { HookRackCheckComponent } from './ProductionOrder/hook-rack-check/hook-rack-check.component';
import { WarehouseInputInfoStockComponent } from './Warehouse/warehouse-input-info-stock/warehouse-input-info-stock.component';
import { WarehouseInventoryInvoiceComponent } from './Warehouse/warehouse-inventory-invoice/warehouse-inventory-invoice.component';
import { ProductionOrderProductionPlanBackupComponent } from './ProductionOrder/production-order-production-plan-backup/production-order-production-plan-backup.component';
import { WarehouseInventoryValueComponent } from './Warehouse/warehouse-inventory-value/warehouse-inventory-value.component';
import { HookRackChangeComponent } from './ProductionOrder/hook-rack-change/hook-rack-change.component';
import { ProductionTracking2026Component } from './ProductionOrder/production-tracking2026/production-tracking2026.component';
import { WarehouseOutputProductionComponent } from './Warehouse/warehouse-output-production/warehouse-output-production.component';
import { WarehouseInputProductionComponent } from './Warehouse/warehouse-input-production/warehouse-input-production.component';
import { WarehouseInputDetailQuantityGAPComponent } from './Warehouse/warehouse-input-detail-quantity-gap/warehouse-input-detail-quantity-gap.component';
import { WarehouseOutputDetailBarcodeFIFOComponent } from './Warehouse/warehouse-output-detail-barcode-fifo/warehouse-output-detail-barcode-fifo.component';
import { ProjectComponent } from './Project/project/project.component';
import { ProjectInfoComponent } from './Project/project-info/project-info.component';
import { ProjectTaskComponent } from './Project/project-task/project-task.component';
import { ProjectTaskHistoryComponent } from './Project/project-task-history/project-task-history.component';
import { CategoryLevelComponent } from './Category/category-level/category-level.component';
import { CategoryStatusComponent } from './Category/category-status/category-status.component';
import { CategorySystemComponent } from './Category/category-system/category-system.component';
import { CategoryTypeComponent } from './Category/category-type/category-type.component';
import { ModuleComponent } from './Project/module/module.component';
import { CategoryConfigComponent } from './Category/category-config/category-config.component';
import { WarehouseOutputDetailBarcodeMaterialModalComponent } from './Warehouse/warehouse-output-detail-barcode-material-modal/warehouse-output-detail-barcode-material-modal.component';
import { ZaloComponent } from './Membership/zalo/zalo.component';
import { CategorySealKitComponent } from './Category/category-seal-kit/category-seal-kit.component';
import { HookRackComponent } from './ProductionOrder/hook-rack/hook-rack.component';
import { HookRackDetailComponent } from './ProductionOrder/hook-rack-detail/hook-rack-detail.component';
import { InvoiceInputHistoryComponent } from './Invoice/invoice-input-history/invoice-input-history.component';










@NgModule({
  declarations: [
    AppComponent,
    LoadingComponent,
    HomepageComponent,        
    CategoryDepartmentComponent,
    CategoryLocationComponent,
    CategoryUnitComponent,    
    LoginComponent,    
    MembershipComponent,
    MembershipDetailComponent,
    MembershipModalComponent,
    
    MaterialComponent,    
    CategoryMaterialComponent,    
    FactoryComponent,
    CategoryInvoiceComponent,    
    MaterialModalComponent,    
    WarehouseInputComponent,
    WarehouseInputModalComponent,
    WarehouseOutputComponent,
    WarehouseOutputModalComponent,
    WarehouseModalComponent,
    InvoiceInputComponent,
    InvoiceInputModalComponent,
    InvoiceOutputModalComponent,
    InvoiceOutputComponent,
    InvoiceInputFileModalComponent,    
    WarehouseRequestComponent,
    WarehouseRequestModalComponent,
    InvoiceOutputInfoComponent,
    InvoiceInputInfoComponent,
    WarehouseOutputInfoComponent,
    WarehouseInputInfoComponent,
    WarehouseRequestInfoComponent,
    WarehouseInventoryComponent,
    CategoryCompanyComponent,
    CompanyComponent,
    CategoryPositionComponent,
    WarehouseInputDetailComponent,
    WarehouseOutputDetailComponent,
    WarehouseOutputDetailFindComponent,
    WarehouseOutputDetailScanComponent,
    WarehouseInputDetailScanComponent,
    CategoryDeliveryComponent,
    CategoryFamilyComponent,
    CategoryVehicleComponent,
    
    WarehouseInputDetailBarcodeComponent,
    WarehouseOutputDetailBarcodeComponent,
    WarehouseOutputDetailBarcodeModalComponent,
    WarehouseInputDetailBarcodeModalComponent,
    WarehouseOutputDetailBarcodeHistoryModalComponent,
    CategoryMenuComponent,        
    ProductionOrderComponent,
    ProductionOrderInfoComponent,
    ProductionOrderProductionPlanComponent,
    ProductionOrderProductionPlanMaterialComponent,
    ProductionOrderBOMComponent,
    ProductionOrderModalComponent,
    STOPLINEComponent,
    WarehouseRequestByPOComponent,
    WarehouseInputByPOComponent,
    WarehouseOutputByPOComponent,
    ProductionOrderProductionPlan001Component,
    BOMComponent,
    BOMInfoComponent,
    BOMDetailComponent,
    WarehouseOutputDetailBarcodeFindComponent,
    CategoryLocationModalComponent,
    CategoryLayerComponent,
    CategoryRackComponent,    
    LocationRuleComponent,
    LocationDiagramComponent,
    WarehouseInputDetailBarcodeDiagramComponent,
    WarehouseInputDetailBarcodeScanComponent,
    WarehouseInputDetailBarcodeImportComponent,
    MembershipInfoComponent,
    Warehouse001Component,
    InvoiceInputInventoryComponent,
    WarehouseOutputDetailScanCancelComponent,
    ProductionOrderProductionPlanMaterial001Component,
    WarehouseInventoryCompanyComponent,
    BOMCompareComponent,
    WarehouseStockComponent,
    InvoiceInputAdminComponent,
    WarehouseInputAdminComponent,
    WarehouseOutputAdminComponent,
    WarehouseRequestAdminComponent,
    ProductionOrderAdminComponent,
    MembershipHistoryURLComponent,
    ZaloTokenComponent,
    WarehouseInputDetailBarcodeMobileComponent,
    WarehouseOutputReturnComponent,
    BOMCompare02Component,
    BOMCompare01Component,
    WarehouseCheckComponent,
    FinishGoodsCheckComponent,
    WarehouseRequestByCategoryDepartmentComponent,
    WarehouseRequestByCategoryDepartmentInfoComponent,
    WarehouseInputDetailBarcodeCompareComponent,    
    WarehouseInputInfoAdminComponent, WarehouseInputDetailBarcodeComparePartNoComponent, FinishGoodsCheckDJMComponent, WarehouseCheckDJMComponent, CutCheckDJMComponent, MaterialUnitCovertComponent, InvoiceInputInfoAdminComponent, WarehouseInputDetailBarcodeMaterialModalComponent, ProductionOrderInfoBackupComponent, 
    WarehouseOutputInfoAdminComponent, CategoryToleranceComponent, CategoryTermComponent, InventoryComponent, InventoryScanComponent, InventoryInfoComponent, WarehouseStockByInvoiceComponent, CategoryParentheseComponent, ProductionSearchComponent, ProductionTrackingComponent, ProductionTrackingKOMAXComponent, ProductionTrackingLPComponent, ProductionTrackingSPSTComponent, ProductionTrackingSHIELDWIREComponent, ProductionTrackingHOOKRACKComponent, ProductionTrackingHookRackModalComponent, InventoryScanMobileComponent, WarehouseStockLongTermComponent, ProductionOrderProductionPlanSemiComponent, WarehouseStockHOOKRACKComponent, InventoryScanDesktopComponent
    , ProductionOrderProductionPlanBackupModalComponent, HookRackCheckComponent, WarehouseInputInfoStockComponent, WarehouseInventoryInvoiceComponent, ProductionOrderProductionPlanBackupComponent, WarehouseInventoryValueComponent, HookRackChangeComponent, ProductionTracking2026Component, WarehouseOutputProductionComponent, WarehouseInputProductionComponent, WarehouseInputDetailQuantityGAPComponent, WarehouseOutputDetailBarcodeFIFOComponent, ProjectComponent, ProjectInfoComponent, ProjectTaskComponent, ProjectTaskHistoryComponent, CategoryLevelComponent, CategoryStatusComponent, CategorySystemComponent, CategoryTypeComponent, ModuleComponent, CategoryConfigComponent, WarehouseOutputDetailBarcodeMaterialModalComponent, ZaloComponent, CategorySealKitComponent, HookRackComponent, HookRackDetailComponent, InvoiceInputHistoryComponent,
    
    
    

  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'serverApp' }),
    FormsModule,
    HttpClientModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    MaterialModule,
    GoogleMapsModule,
    ChartsModule,
    CKEditorModule,
  ],
  providers: [
    CookieService,
    NotificationService,
    { provide: MAT_DATE_LOCALE, useValue: 'en-GB' }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
