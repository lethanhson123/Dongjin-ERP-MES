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
import { BarcodeComponent } from './barcode/barcode.component';
import { WarehouseInputDetailBarcodeComponent } from './Warehouse/warehouse-input-detail-barcode/warehouse-input-detail-barcode.component';
import { WarehouseInputInfoComponent } from './Warehouse/warehouse-input-info/warehouse-input-info.component';
import { BOMInfoComponent } from './PC/bominfo/bominfo.component';
import { EmployeeInfoComponent } from './HRM/employee-info/employee-info.component';
import { MaterialInfoComponent } from './PC/material-info/material-info.component';
import { InvoiceInputInfoComponent } from './Invoice/invoice-input-info/invoice-input-info.component';
import { CategoryLocationComponent } from './category-location/category-location.component';












@NgModule({
  declarations: [
    AppComponent,
    LoadingComponent,
    HomepageComponent,
    BarcodeComponent,
    WarehouseInputDetailBarcodeComponent,
    WarehouseInputInfoComponent,
    BOMInfoComponent,
    EmployeeInfoComponent,
    MaterialInfoComponent,
    InvoiceInputInfoComponent,
    CategoryLocationComponent,        
    
    
    

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
