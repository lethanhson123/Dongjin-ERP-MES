import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
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

import { CategoryHRMSuatAnComponent } from './HRM/category-hrmsuat-an/category-hrmsuat-an.component';
import { HRMChamCongLichSuComponent } from './HRM/hrmcham-cong-lich-su/hrmcham-cong-lich-su.component';
import { HRMChamCongNhaAnComponent } from './HRM/hrmcham-cong-nha-an/hrmcham-cong-nha-an.component';
import { HrmChamCongLichSu001Component } from './HRM/hrm-cham-cong-lich-su001/hrm-cham-cong-lich-su001.component';
import { AnnualLeaveRequestComponent } from './AnnualLeave/annual-leave-request/annual-leave-request.component';
import { AnnualLeaveRequest001Component } from './AnnualLeave/annual-leave-request001/annual-leave-request001.component';
import { HrmQuanLyCongViecComponent } from './HRM/hrm-quan-ly-cong-viec/hrm-quan-ly-cong-viec.component';
import { CategoryStatusComponent } from './HRM/category-status/category-status.component';
import { HrmQuanLyCongViec001Component } from './HRM/hrm-quan-ly-cong-viec001/hrm-quan-ly-cong-viec001.component';
import { HrmThongKeKPIComponent } from './HRM/hrm-thong-ke-kpi/hrm-thong-ke-kpi.component';
import { HrmDanhGiaTongTheHieuSuatComponent } from './HRM/hrm-danh-gia-tong-the-hieu-suat/hrm-danh-gia-tong-the-hieu-suat.component';
import { CategoryPositionComponent } from './Category/category-position/category-position.component';
import { CategoryDepartmentComponent } from './Category/category-department/category-department.component';
import { HRMSoDoToChucComponent } from './HRMSoDoToChuc/hrmso-do-to-chuc/hrmso-do-to-chuc.component';
import { HRMSoDoToChuc001Component } from './HRMSoDoToChuc/hrmso-do-to-chuc001/hrmso-do-to-chuc001.component';
import { MembershipListComponent } from './HRM/membership-list/membership-list.component';
import { HRMCaLamViecKeHoachComponent } from './HRM/hrm-ca-lam-viec-ke-hoach/hrm-ca-lam-viec-ke-hoach.component';
import { CategoryHRMPhongBanComponent } from './HRM/category-hrmphong-ban/category-hrmphong-ban.component';
import { CategoryHRMMayChamCongLichSuComponent } from './HRM/category-hrmmay-cham-cong-lich-su/category-hrmmay-cham-cong-lich-su.component';
import { HRMTangCaManagementComponent } from './HRMTangCa/hrm-tang-ca-management/hrm-tang-ca-management.component';
import { HRMTangCaOrdersComponent } from './HRMTangCa/hrm-tang-ca-orders/hrm-tang-ca-orders.component';
import { HRMTangCaEmployeesComponent } from './HRMTangCa/hrm-tang-ca-employees/hrm-tang-ca-employees.component';
import { HRMTangCaReportsComponent } from './HRMTangCa/hrm-tang-ca-reports/hrm-tang-ca-reports.component';
import { AnnualLeaveManagementComponent } from './AnnualLeave/annual-leave-management/annual-leave-management.component';
import { AnnualLeaveBalanceComponent } from './AnnualLeave/annual-leave-balance/annual-leave-balance.component';
import { AnnualLeaveHistoryComponent } from './AnnualLeave/annual-leave-history/annual-leave-history.component';
import { AnnualLeaveTypeComponent } from './AnnualLeave/annual-leave-type/annual-leave-type.component';
import { AnnualLeaveOverviewComponent } from './AnnualLeave/annual-leave-overview/annual-leave-overview.component';
import { AnnualLeaveActualWorkdaysComponent } from './AnnualLeave/annual-leave-actual-workdays/annual-leave-actual-workdays.component';
import { HRMChamCongTongHopComponent } from './HRM/hrmcham-cong-tong-hop/hrmcham-cong-tong-hop.component';
import { HrmChamCongTongHop001Component } from './HRM/hrm-cham-cong-tong-hop001/hrm-cham-cong-tong-hop001.component';
import { HRMNhaAnThucDonComponent } from './HRM/hrmnha-an-thuc-don/hrmnha-an-thuc-don.component';
import { HRMNhaAnFileComponent } from './HRM/hrmnha-an-file/hrmnha-an-file.component';
import { HRMChamCongNhaAn001Component } from './HRM/hrmcham-cong-nha-an001/hrmcham-cong-nha-an001.component';

const routes: Routes = [
  { path: '', redirectTo: '/Homepage', pathMatch: 'full' },
  {
    path: 'Homepage',
    component: HomepageComponent,
    data: { title: 'Homepage' },
  },
  {
    path: 'Membership',
    component: MembershipComponent,
    data: { title: 'Membership' },
  },
  {
    path: 'HRMNghiPhep',
    component: AnnualLeaveManagementComponent,
    children: [
      {
        path: '',
        redirectTo: 'Overview',
        pathMatch: 'full',
      },
      {
        path: 'Overview',
        component: AnnualLeaveOverviewComponent,
        data: { title: 'Overview' },
      },
      {
        path: 'AnnualLeaveRequest',
        component: AnnualLeaveRequestComponent,
        data: { title: 'Leave Requests' },
      },
      {
        path: 'ActualWorkdays',
        component: AnnualLeaveActualWorkdaysComponent,
        data: { title: 'Actual Workdays' },
      },
      {
        path: 'Balance',
        component: AnnualLeaveBalanceComponent,
        data: { title: 'Opening Balance' },
      },
      {
        path: 'History',
        component: AnnualLeaveHistoryComponent,
        data: { title: 'Balance History' },
      },
      {
        path: 'LeaveType',
        component: AnnualLeaveTypeComponent,
        data: { title: 'Leave Types' },
      },
    ],
  },
  {
    path: 'AnnualLeaveRequest',
    redirectTo: 'HRMNghiPhep/AnnualLeaveRequest',
    pathMatch: 'full',
  },
  {
    path: 'AnnualLeaveRequest001',
    component: AnnualLeaveRequest001Component,
    data: { title: 'Nghỉ phép' },
  },
  {
    path: 'HRMChamCongLichSu',
    component: HRMChamCongLichSuComponent,
    data: { title: 'Chấm công' },
  },
  {
    path: 'HRMTangCa',
    component: HRMTangCaManagementComponent,
    children: [
      {
        path: '',
        redirectTo: 'Orders',
        pathMatch: 'full',
      },
      {
        path: 'Orders',
        component: HRMTangCaOrdersComponent,
        data: { title: 'Overtime Orders' },
      },
      {
        path: 'Employees',
        component: HRMTangCaEmployeesComponent,
        data: { title: 'Overtime by Employee' },
      },
      {
        path: 'Reports',
        component: HRMTangCaReportsComponent,
        data: { title: 'Overtime Reports' },
      },
    ],
  },
  {
    path: 'HRMChamCongLichSu001',
    component: HrmChamCongLichSu001Component,
    data: { title: 'Chấm công' },
  },
  {
    path: 'HRMChamCongLichSu002',
    component: HrmChamCongLichSu001Component,
    data: { title: 'Chấm công' },
  },
  {
    path: 'HRMChamCongNhaAn',
    component: HRMChamCongNhaAnComponent,
    data: { title: 'Nhà ăn' },
  },
  {
    path: 'HRMChamCongNhaAn001',
    component: HRMChamCongNhaAn001Component,
    data: { title: 'Nhà ăn' },
  },
  {
    path: 'CategoryHRMGioiTinh',
    component: CategoryHRMGioiTinhComponent,
    data: { title: 'Gender' },
  },
  {
    path: 'CategoryHRMChucDanh',
    component: CategoryHRMChucDanhComponent,
    data: { title: 'Chức danh' },
  },
  {
    path: 'CategoryHRMTrinhDo',
    component: CategoryHRMTrinhDoComponent,
    data: { title: 'CategoryHRMTrinhDo' },
  },
  {
    path: 'CategoryHRMChuyenNganh',
    component: CategoryHRMChuyenNganhComponent,
    data: { title: 'CategoryHRMChuyenNganh' },
  },
  {
    path: 'CategoryHRMDanToc',
    component: CategoryHRMDanTocComponent,
    data: { title: 'CategoryHRMDanToc' },
  },
  {
    path: 'CategoryHRMQuocTich',
    component: CategoryHRMQuocTichComponent,
    data: { title: 'CategoryHRMQuocTich' },
  },
  {
    path: 'CategoryHRMTinhThanh',
    component: CategoryHRMTinhThanhComponent,
    data: { title: 'CategoryHRMTinhThanh' },
  },
  {
    path: 'CategoryHRMXaPhuong',
    component: CategoryHRMXaPhuongComponent,
    data: { title: 'CategoryHRMXaPhuong' },
  },
  {
    path: 'CategoryHRMCaLamViec',
    component: CategoryHRMCaLamViecComponent,
    data: { title: 'CategoryHRMCaLamViec' },
  },
  {
    path: 'CategoryHRMSuatAn',
    component: CategoryHRMSuatAnComponent,
    data: { title: 'CategoryHRMSuatAn' },
  },
  {
    path: 'CategoryHRMSuatAn',
    component: CategoryHRMSuatAnComponent,
    data: { title: 'CategoryHRMSuatAn' },
  },
  {
    path: 'CategoryStatus',
    component: CategoryStatusComponent,
    data: { title: 'CategoryStatus' },
  },
  {
    path: 'HRMQuanLyCongViec',
    component: HrmQuanLyCongViecComponent,
    data: { title: 'Quản lý công việc' },
  },
  {
    path: 'HRMQuanLyCongViec001',
    component: HrmQuanLyCongViec001Component,
    data: { title: 'Công việc' },
  },
  {
    path: 'HRMThongKeKPI',
    component: HrmThongKeKPIComponent,
    data: { title: 'Thống Kê KPI' },
  },
  {
    path: 'HRMDanhGiaTongTheHieuSuat',
    component: HrmDanhGiaTongTheHieuSuatComponent,
    data: { title: 'Đánh giá tổng thể hiệu suất' },
  },
  {
    path: 'HRMSoDoToChuc',
    component: HRMSoDoToChucComponent,
    data: { title: 'Sơ đồ tổ chức' },
  },
  {
    path: 'HRMSoDoToChuc001',
    component: HRMSoDoToChuc001Component,
    data: { title: 'Sơ đồ tổ chức' },
  },
  {
    path: 'HRMCaLamViecKeHoach',
    component: HRMCaLamViecKeHoachComponent,
    data: { title: 'Ca Làm Việc Kế Hoạch' },
  },
  {
    path: 'HRMChamCongTongHop',
    component: HRMChamCongTongHopComponent,
    data: { title: 'Chấm Công Tổng Hợp' },
  },
  {
    path: 'HRMChamCongTongHop001',
    component: HrmChamCongTongHop001Component,
    data: { title: 'Chấm Công Tổng Hợp Cá Nhân' },
  },
  {
    path: 'CategoryPosition',
    component: CategoryPositionComponent,
    data: { title: 'Vị trí' },
  },
  {
    path: 'CategoryDepartment',
    component: CategoryDepartmentComponent,
    data: { title: 'Bộ phận' },
  },
  {
    path: 'CategoryHRMPhongBan',
    component: CategoryHRMPhongBanComponent,
    data: { title: 'Phòng Ban' },
  },
  {
    path: 'CategoryHRMMayChamCongLichSu',
    component: CategoryHRMMayChamCongLichSuComponent,
    data: { title: 'Máy Chấm Công Lịch Sử' },
  },
  {
    path: 'EmployeeList',
    component: MembershipListComponent,
    data: { title: 'Danh sách nhân viên' },
  },

  {
    path: 'HRMNhaAnThucDon',
    component: HRMNhaAnThucDonComponent,
    data: { title: 'HRM Nhà ăn Thực đơn' },
  },
  {
    path: 'HRMNhaAnFile',
    component: HRMNhaAnFileComponent,
    data: { title: 'HRM Nhà ăn file' },
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { useHash: true, initialNavigation: 'enabled' })],
  exports: [RouterModule],
})
export class AppRoutingModule {}
