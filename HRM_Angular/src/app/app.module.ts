import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule, HttpClient } from '@angular/common/http';
import { TranslateModule, TranslateLoader } from '@ngx-translate/core';
import { HttpLoaderFactory } from './app.translate-loader';
import { AppRoutingModule } from './app-routing.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { CKEditorModule } from 'ngx-ckeditor';
import { ChartsModule } from 'ng2-charts';
import { CookieService } from 'ngx-cookie-service';
import { MAT_DATE_LOCALE } from '@angular/material/core';
import { MaterialModule } from './material/material.module';
import { GoogleMapsModule } from '@angular/google-maps';
import { NotificationService } from './shared/Notification.service';
import { LanguageService } from './shared/Language.service';
import { AppComponent } from './app.component';
import { LoadingComponent } from './loading/loading.component';
import { HomepageComponent } from './homepage/homepage.component';
import { MembershipComponent } from './HRM/membership/membership.component';
import { CategoryHRMGioiTinhComponent } from './HRM/category-hrmgioi-tinh/category-hrmgioi-tinh.component';

import { CategoryHRMChucDanhComponent } from './HRM/category-hrmchuc-danh/category-hrmchuc-danh.component';
import { CategoryHRMTrinhDoComponent } from './HRM/category-hrmtrinh-do/category-hrmtrinh-do.component';
import { CategoryHRMChuyenNganhComponent } from './HRM/category-hrmchuyen-nganh/category-hrmchuyen-nganh.component';
import { CategoryHRMDanTocComponent } from './HRM/category-hrmdan-toc/category-hrmdan-toc.component';
import { CategoryHRMQuocTichComponent } from './HRM/category-hrmquoc-tich/category-hrmquoc-tich.component';

import { CategoryHRMTinhThanhComponent } from './HRM/category-hrmtinh-thanh/category-hrmtinh-thanh.component';
import { CategoryHRMXaPhuongComponent } from './HRM/category-hrmxa-phuong/category-hrmxa-phuong.component';
import { CategoryHRMCaLamViecComponent } from './HRM/category-hrmca-lam-viec/category-hrmca-lam-viec.component';

import { HRMChamCongLichSuModalComponent } from './HRM/hrmcham-cong-lich-su-modal/hrmcham-cong-lich-su-modal.component';
import { CategoryHRMSuatAnComponent } from './HRM/category-hrmsuat-an/category-hrmsuat-an.component';
import { HRMChamCongLichSuComponent } from './HRM/hrmcham-cong-lich-su/hrmcham-cong-lich-su.component';
import { HRMChamCongNhaAnComponent } from './HRM/hrmcham-cong-nha-an/hrmcham-cong-nha-an.component';
import { HrmChamCongLichSu001Component } from './HRM/hrm-cham-cong-lich-su001/hrm-cham-cong-lich-su001.component';
import { AnnualLeaveRequestComponent } from './AnnualLeave/annual-leave-request/annual-leave-request.component';
import { AnnualLeaveRequest001Component } from './AnnualLeave/annual-leave-request001/annual-leave-request001.component';
import { AnnualLeaveManagementComponent } from './AnnualLeave/annual-leave-management/annual-leave-management.component';
import { AnnualLeaveBalanceComponent } from './AnnualLeave/annual-leave-balance/annual-leave-balance.component';
import { AnnualLeaveBalanceEditModalComponent } from './AnnualLeave/annual-leave-balance-edit-modal/annual-leave-balance-edit-modal.component';
import { AnnualLeaveBalanceImportModalComponent } from './AnnualLeave/annual-leave-balance-import-modal/annual-leave-balance-import-modal.component';
import { AnnualLeaveHistoryComponent } from './AnnualLeave/annual-leave-history/annual-leave-history.component';
import { AnnualLeaveTypeComponent } from './AnnualLeave/annual-leave-type/annual-leave-type.component';
import { AnnualLeaveOverviewComponent } from './AnnualLeave/annual-leave-overview/annual-leave-overview.component';
import { AnnualLeaveActualWorkdaysComponent } from './AnnualLeave/annual-leave-actual-workdays/annual-leave-actual-workdays.component';
import { HrmQuanLyCongViecComponent } from './HRM/hrm-quan-ly-cong-viec/hrm-quan-ly-cong-viec.component';
import { HrmQuanLyCongViecModalComponent } from './HRM/hrm-quan-ly-cong-viec-modal/hrm-quan-ly-cong-viec-modal.component';
import { CategoryStatusComponent } from './HRM/category-status/category-status.component';
import { HrmQuanLyCongViec001Component } from './HRM/hrm-quan-ly-cong-viec001/hrm-quan-ly-cong-viec001.component';
import { HrmThongKeKPIComponent } from './HRM/hrm-thong-ke-kpi/hrm-thong-ke-kpi.component';
import { HrmThongKeKpiModalComponent } from './HRM/hrm-thong-ke-kpi-modal/hrm-thong-ke-kpi-modal.component';

import { CategoryDepartmentComponent } from './Category/category-department/category-department.component';
import { CategoryPositionComponent } from './Category/category-position/category-position.component';

import { HRMSoDoToChucComponent } from './HRMSoDoToChuc/hrmso-do-to-chuc/hrmso-do-to-chuc.component';
import { HRMSoDoToChuc001Component } from './HRMSoDoToChuc/hrmso-do-to-chuc001/hrmso-do-to-chuc001.component';
import { HRMSoDoToChucModalComponent } from './HRMSoDoToChuc/hrmso-do-to-chuc-modal/hrmso-do-to-chuc-modal.component';


import { MembershipListComponent } from './HRM/membership-list/membership-list.component';
import { HRMChamCongLichSu002Component } from './HRM/hrmcham-cong-lich-su002/hrmcham-cong-lich-su002.component';

import { HrmDanhGiaTongTheHieuSuatComponent } from './HRM/hrm-danh-gia-tong-the-hieu-suat/hrm-danh-gia-tong-the-hieu-suat.component';
import { MemberShipListModalComponent } from './HRM/member-ship-list-modal/member-ship-list-modal.component';

import { HRMCaLamViecKeHoachComponent } from './HRM/hrm-ca-lam-viec-ke-hoach/hrm-ca-lam-viec-ke-hoach.component';
import { HrmCaLamViecKeHoachModalComponent } from './HRM/hrm-ca-lam-viec-ke-hoach-modal/hrm-ca-lam-viec-ke-hoach-modal.component';

import { MemberShipListModal01Component } from './HRM/member-ship-list-modal01/member-ship-list-modal01.component';
import { MemberShipListModal02Component } from './HRM/member-ship-list-modal02/member-ship-list-modal02.component';
import { HrmCaLamViecKeHoachModal01Component } from './HRM/hrm-ca-lam-viec-ke-hoach-modal01/hrm-ca-lam-viec-ke-hoach-modal01.component';
import { HrmCaLamViecKeHoachModal02Component } from './HRM/hrm-ca-lam-viec-ke-hoach-modal02/hrm-ca-lam-viec-ke-hoach-modal02.component';
import { MemberShipModal03Component } from './HRM/member-ship-modal03/member-ship-modal03.component';
import { AnnualLeaveRequestModal001Component } from './AnnualLeave/annual-leave-request-modal001/annual-leave-request-modal001.component';
import { CategoryHRMPhongBanComponent } from './HRM/category-hrmphong-ban/category-hrmphong-ban.component';
import { CategoryHRMMayChamCongLichSuComponent } from './HRM/category-hrmmay-cham-cong-lich-su/category-hrmmay-cham-cong-lich-su.component';
import { HRMTangCaManagementComponent } from './HRMTangCa/hrm-tang-ca-management/hrm-tang-ca-management.component';
import { HRMTangCaOrdersComponent } from './HRMTangCa/hrm-tang-ca-orders/hrm-tang-ca-orders.component';
import { HRMTangCaEmployeesComponent } from './HRMTangCa/hrm-tang-ca-employees/hrm-tang-ca-employees.component';
import { HRMTangCaReportsComponent } from './HRMTangCa/hrm-tang-ca-reports/hrm-tang-ca-reports.component';
import { HRMChamCongTongHopComponent } from './HRM/hrmcham-cong-tong-hop/hrmcham-cong-tong-hop.component';
import { HrmChamCongTongHop001Component } from './HRM/hrm-cham-cong-tong-hop001/hrm-cham-cong-tong-hop001.component';
import { HRMNhaAnFileComponent } from './HRM/hrmnha-an-file/hrmnha-an-file.component';
import { HRMNhaAnThucDonComponent } from './HRM/hrmnha-an-thuc-don/hrmnha-an-thuc-don.component';
import { HRMChamCongNhaAn001Component } from './HRM/hrmcham-cong-nha-an001/hrmcham-cong-nha-an001.component';





@NgModule({
  declarations: [
    AppComponent,
    LoadingComponent,
    HomepageComponent,
    MembershipComponent,
    CategoryHRMGioiTinhComponent,
    CategoryHRMChucDanhComponent,
    CategoryHRMTrinhDoComponent,
    CategoryHRMChuyenNganhComponent,
    CategoryHRMDanTocComponent,
    CategoryHRMQuocTichComponent,
    CategoryHRMTinhThanhComponent,
    CategoryHRMXaPhuongComponent,
    CategoryHRMCaLamViecComponent,
    HRMChamCongLichSuComponent,
    HRMChamCongLichSuModalComponent,
    CategoryHRMSuatAnComponent,
    HRMChamCongNhaAnComponent,
    HrmChamCongLichSu001Component,
    AnnualLeaveRequestComponent,
    AnnualLeaveRequest001Component,
    AnnualLeaveManagementComponent,
    AnnualLeaveBalanceComponent,
    AnnualLeaveBalanceEditModalComponent,
    AnnualLeaveBalanceImportModalComponent,
    AnnualLeaveHistoryComponent,
    AnnualLeaveTypeComponent,
    AnnualLeaveOverviewComponent,
    AnnualLeaveActualWorkdaysComponent,
    HrmQuanLyCongViecComponent,
    HrmQuanLyCongViecModalComponent,
    CategoryStatusComponent,
    HrmQuanLyCongViec001Component,
    HrmThongKeKPIComponent,
    HrmDanhGiaTongTheHieuSuatComponent,
    CategoryDepartmentComponent,
    CategoryPositionComponent,
    HRMSoDoToChucComponent,
    HRMSoDoToChucModalComponent,
    HRMSoDoToChuc001Component,
    MembershipListComponent,
    HrmThongKeKpiModalComponent,
    HRMChamCongLichSu002Component,
    MemberShipListModalComponent,
    HRMCaLamViecKeHoachComponent,
    HrmCaLamViecKeHoachModalComponent,
    MemberShipListModalComponent,
    MemberShipListModal01Component,
    MemberShipListModal02Component,
    HrmCaLamViecKeHoachModal01Component,
    HrmCaLamViecKeHoachModal02Component,
    MemberShipModal03Component,
    AnnualLeaveRequestModal001Component,
    CategoryHRMPhongBanComponent,
    CategoryHRMMayChamCongLichSuComponent,
    HRMTangCaManagementComponent,
    HRMTangCaOrdersComponent,
    HRMTangCaEmployeesComponent,
    HRMTangCaReportsComponent,
    HRMChamCongTongHopComponent,
    HrmChamCongTongHop001Component,
    HRMNhaAnFileComponent,
    HRMNhaAnThucDonComponent,
    HRMChamCongNhaAn001Component,
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
    TranslateModule.forRoot({
      loader: {
        provide: TranslateLoader,
        useFactory: HttpLoaderFactory,
        deps: [HttpClient],
      },
    }),
    TranslateModule,
  ],
  providers: [
    CookieService,
    NotificationService,
    LanguageService,
    { provide: MAT_DATE_LOCALE, useValue: 'vi-VN' }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }

