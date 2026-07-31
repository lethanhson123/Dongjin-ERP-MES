import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomepageComponent } from './homepage/homepage.component';
import { BarcodeComponent } from './barcode/barcode.component';
import { EmployeeInfoComponent } from './HRM/employee-info/employee-info.component';
import { BOMInfoComponent } from './PC/bominfo/bominfo.component';
import { WarehouseInputInfoComponent } from './Warehouse/warehouse-input-info/warehouse-input-info.component';
import { WarehouseInputDetailBarcodeComponent } from './Warehouse/warehouse-input-detail-barcode/warehouse-input-detail-barcode.component';
import { InvoiceInputInfoComponent } from './Invoice/invoice-input-info/invoice-input-info.component';
import { MaterialInfoComponent } from './PC/material-info/material-info.component';
import { CategoryLocationComponent } from './category-location/category-location.component';

const routes: Routes = [
  { path: '', redirectTo: '/Homepage', pathMatch: 'full' },
  {
    path: 'Homepage', component: HomepageComponent, data: { title: 'Homepage' }
  },
  {
    path: 'Barcode/:SearchString', component: BarcodeComponent, data: { title: 'Barcode' }
  },
  {
    path: 'EmployeeInfo/:SearchString', component: EmployeeInfoComponent, data: { title: 'HRM Nhân viên - Thông tin' }
  },
  {
    path: 'BOMInfo/:ID', component: BOMInfoComponent, data: { title: 'BOM Info' }
  },
  {
    path: 'WarehouseInputDetailBarcodeInfo/:ID', component: WarehouseInputDetailBarcodeComponent, data: { title: 'Warehouse Input Info' }
  },
  {
    path: 'WarehouseInputInfo/:ID', component: WarehouseInputInfoComponent, data: { title: 'Warehouse Input Info' }
  },
  {
    path: 'InvoiceInputInfo/:ID', component: InvoiceInputInfoComponent, data: { title: 'Invoice Input Info' }
  },
  {
    path: 'MaterialInfo/:ID', component: MaterialInfoComponent, data: { title: 'Material Info' }
  },
  {
    path: 'CategoryLocation/:CategoryDepartmentID/:Name', component: CategoryLocationComponent, data: { title: 'Location' }
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { useHash: true, initialNavigation: 'enabled' })],
  exports: [RouterModule]
})
export class AppRoutingModule { }









































































