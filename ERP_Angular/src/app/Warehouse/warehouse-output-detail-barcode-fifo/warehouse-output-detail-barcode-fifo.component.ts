import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { NotificationService } from 'src/app/shared/Notification.service';
import { DownloadService } from 'src/app/shared/Download.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

import * as XLSX from 'xlsx';
import * as FileSaver from 'file-saver';

import { WarehouseOutputDetailBarcode } from 'src/app/shared/ERP/WarehouseOutputDetailBarcode.model';
import { WarehouseOutputDetailBarcodeService } from 'src/app/shared/ERP/WarehouseOutputDetailBarcode.service';


import { CategoryDepartment } from 'src/app/shared/ERP/CategoryDepartment.model';
import { CategoryDepartmentService } from 'src/app/shared/ERP/CategoryDepartment.service';

import { Company } from 'src/app/shared/ERP/Company.model';
import { CompanyService } from 'src/app/shared/ERP/Company.service';

@Component({
  selector: 'app-warehouse-output-detail-barcode-fifo',
  templateUrl: './warehouse-output-detail-barcode-fifo.component.html',
  styleUrls: ['./warehouse-output-detail-barcode-fifo.component.css']
})
export class WarehouseOutputDetailBarcodeFIFOComponent implements OnInit {

  @ViewChild('WarehouseOutputDetailBarcodeSort') WarehouseOutputDetailBarcodeSort: MatSort;
  @ViewChild('WarehouseOutputDetailBarcodePaginator') WarehouseOutputDetailBarcodePaginator: MatPaginator;

  constructor(
    public Dialog: MatDialog,
    public NotificationService: NotificationService,
    public DownloadService: DownloadService,

    public WarehouseOutputDetailBarcodeService: WarehouseOutputDetailBarcodeService,
    public CompanyService: CompanyService,
    public CategoryDepartmentService: CategoryDepartmentService,

  ) {
    this.CompanySearch();
    this.ComponentGetYearList();
    this.ComponentGetMonthList();
  }

  ngOnInit(): void {

  }
  ngAfterViewInit() {
    //this.WarehouseOutputDetailBarcodeSearch();
  }
  ComponentGetYearList() {
    this.WarehouseOutputDetailBarcodeService.ComponentGetYearList();
  }
  ComponentGetMonthList() {
    this.WarehouseOutputDetailBarcodeService.ComponentGetMonthList();
  }
  CompanySearch() {
    this.CompanyService.ComponentGetByMembershipID_ActiveToListAsync(this.WarehouseOutputDetailBarcodeService);
    this.CompanyChange();
  }
  CompanyChange() {
    this.CategoryDepartmentSearch();
  }
  CategoryDepartmentSearch() {
    this.CategoryDepartmentService.BaseParameter.CompanyID = this.WarehouseOutputDetailBarcodeService.BaseParameter.CompanyID;
    this.CategoryDepartmentService.ComponentGetByMembershipID_CompanyID_ActiveToListAsync(this.WarehouseOutputDetailBarcodeService);
    this.CategoryDepartmentService.ComponentGetByCompanyIDAndActiveToList(this.WarehouseOutputDetailBarcodeService);
  }
  WarehouseOutputDetailBarcodeSearch() {
    if (this.WarehouseOutputDetailBarcodeService.BaseParameter.SearchString && this.WarehouseOutputDetailBarcodeService.BaseParameter.SearchString.length > 0) {
      this.WarehouseOutputDetailBarcodeService.BaseParameter.SearchString = this.WarehouseOutputDetailBarcodeService.BaseParameter.SearchString.trim();
      if (this.WarehouseOutputDetailBarcodeService.DataSource) {
        this.WarehouseOutputDetailBarcodeService.DataSource.filter = this.WarehouseOutputDetailBarcodeService.BaseParameter.SearchString.toLowerCase();
      }
    }
    else {
      this.WarehouseOutputDetailBarcodeService.IsShowLoading = true;
      this.WarehouseOutputDetailBarcodeService.BaseParameter.MembershipID = Number(localStorage.getItem(environment.UserID));
      this.WarehouseOutputDetailBarcodeService.GetByCategoryDepartmentID_Active_FIFOToListAsync().subscribe(
        res => {
          this.WarehouseOutputDetailBarcodeService.List = (res as BaseResult).List;          
          this.WarehouseOutputDetailBarcodeService.DataSource = new MatTableDataSource(this.WarehouseOutputDetailBarcodeService.List);
          this.WarehouseOutputDetailBarcodeService.DataSource.sort = this.WarehouseOutputDetailBarcodeSort;
          this.WarehouseOutputDetailBarcodeService.DataSource.paginator = this.WarehouseOutputDetailBarcodePaginator;
        },
        err => {
        },
        () => {
          this.WarehouseOutputDetailBarcodeService.IsShowLoading = false;
        }
      );
    }    
  }
  @ViewChild("TABLE") table: ElementRef;
    WarehouseOutputDetailBarcodeExcel() {
      const ws: XLSX.WorkSheet = XLSX.utils.table_to_sheet(this.table.nativeElement);
      const wb: XLSX.WorkBook = XLSX.utils.book_new();
      XLSX.utils.book_append_sheet(wb, ws, "Sheet1");
  
      let ListCategoryDepartment = this.CategoryDepartmentService.ListFilter.filter(o => o.ID == this.WarehouseOutputDetailBarcodeService.BaseParameter.CategoryDepartmentID);
      if (ListCategoryDepartment && ListCategoryDepartment.length > 0) {
        this.CategoryDepartmentService.BaseParameter.BaseModel = ListCategoryDepartment[0];
      }
      let ListCompany = this.CompanyService.ListFilter.filter(o => o.ID == this.WarehouseOutputDetailBarcodeService.BaseParameter.CompanyID);
      if (ListCompany && ListCompany.length > 0) {
        this.CompanyService.BaseParameter.BaseModel = ListCompany[0];
      }
  
      let filename = this.CompanyService.BaseParameter.BaseModel.Name + "-" + this.CategoryDepartmentService.BaseParameter.BaseModel.Name + "-" + this.WarehouseOutputDetailBarcodeService.BaseParameter.Year + "-" + this.WarehouseOutputDetailBarcodeService.BaseParameter.Month + "-OutputFIFO.xlsx";
      XLSX.writeFile(wb, filename);
    }
}