import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { Router, NavigationEnd, ActivatedRoute } from '@angular/router';
import { MatDialog, MatDialogConfig, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { NotificationService } from 'src/app/shared/Notification.service';
import { DownloadService } from 'src/app/shared/Download.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

import * as XLSX from 'xlsx';
import * as FileSaver from 'file-saver';

import { Inventory } from 'src/app/shared/ERP/Inventory.model';
import { InventoryService } from 'src/app/shared/ERP/Inventory.service';

import { InventoryDetail } from 'src/app/shared/ERP/InventoryDetail.model';
import { InventoryDetailService } from 'src/app/shared/ERP/InventoryDetail.service';

import { InventoryDetailBarcode } from 'src/app/shared/ERP/InventoryDetailBarcode.model';
import { InventoryDetailBarcodeService } from 'src/app/shared/ERP/InventoryDetailBarcode.service';


import { CategoryDepartment } from 'src/app/shared/ERP/CategoryDepartment.model';
import { CategoryDepartmentService } from 'src/app/shared/ERP/CategoryDepartment.service';

import { Company } from 'src/app/shared/ERP/Company.model';
import { CompanyService } from 'src/app/shared/ERP/Company.service';

import { Material } from 'src/app/shared/ERP/Material.model';
import { MaterialService } from 'src/app/shared/ERP/Material.service';

@Component({
  selector: 'app-inventory-info',
  templateUrl: './inventory-info.component.html',
  styleUrls: ['./inventory-info.component.css']
})
export class InventoryInfoComponent {

  @ViewChild('InventoryDetailSort') InventoryDetailSort: MatSort;
  @ViewChild('InventoryDetailPaginator') InventoryDetailPaginator: MatPaginator;

  @ViewChild('InventoryDetailSortFilter') InventoryDetailSortFilter: MatSort;
  @ViewChild('InventoryDetailPaginatorFilter') InventoryDetailPaginatorFilter: MatPaginator;

  @ViewChild('InventoryDetailBarcodeSort') InventoryDetailBarcodeSort: MatSort;
  @ViewChild('InventoryDetailBarcodePaginator') InventoryDetailBarcodePaginator: MatPaginator;

  @ViewChild('InventoryDetailBarcodeSortFilter') InventoryDetailBarcodeSortFilter: MatSort;
  @ViewChild('InventoryDetailBarcodePaginatorFilter') InventoryDetailBarcodePaginatorFilter: MatPaginator;

  @ViewChild('Week') Week!: ElementRef;
  @ViewChild('MaterialID') MaterialID!: ElementRef;
  @ViewChild('MaterialName') MaterialName!: ElementRef;
  @ViewChild('QuantityActual') QuantityActual!: ElementRef;
  @ViewChild('CategoryLocationName') CategoryLocationName!: ElementRef;

  constructor(
    public ActiveRouter: ActivatedRoute,
    public Router: Router,
    public Dialog: MatDialog,

    public NotificationService: NotificationService,
    public DownloadService: DownloadService,

    public InventoryService: InventoryService,
    public InventoryDetailService: InventoryDetailService,
    public InventoryDetailBarcodeService: InventoryDetailBarcodeService,

    public CategoryDepartmentService: CategoryDepartmentService,
    public CompanyService: CompanyService,
    public MaterialService: MaterialService,
  ) {

    this.InventoryDetailBarcodeService.BaseParameter.Active = true;

    this.InventoryService.BaseParameter.BaseModel = {
      ID: environment.InitializationNumber,
      Date: null,
      Code: environment.InitializationString,
    };

    this.InventoryDetailService.List = [];
    this.InventoryDetailService.DataSource = new MatTableDataSource(this.InventoryDetailService.List);
    this.InventoryDetailService.DataSource.sort = this.InventoryDetailSort;
    this.InventoryDetailService.DataSource.paginator = this.InventoryDetailPaginator;

    this.InventoryDetailBarcodeService.List = [];
    this.InventoryDetailBarcodeService.DataSource = new MatTableDataSource(this.InventoryDetailBarcodeService.List);
    this.InventoryDetailBarcodeService.DataSource.sort = this.InventoryDetailBarcodeSort;
    this.InventoryDetailBarcodeService.DataSource.paginator = this.InventoryDetailBarcodePaginator;



    this.CompanySearch();

    this.Router.events.forEach((event) => {
      if (event instanceof NavigationEnd) {
        this.InventoryService.BaseParameter.ID = Number(this.ActiveRouter.snapshot.params.ID);
        this.InventorySearch();
      }
    });
  }

  ngOnInit(): void {

  }
  ngAfterViewInit() {
    this.QuantityActual.nativeElement.focus();

  }
  MaterialSearch() {
    this.MaterialService.BaseParameter.CompanyID = this.InventoryService.BaseParameter.BaseModel.CompanyID;
    this.MaterialService.BaseParameter.GeneralID = 9;
    this.MaterialService.BaseParameter.Active = true;
    this.MaterialService.GetByCompanyIDAndActiveToListAsync().subscribe(
      res => {
        this.MaterialService.List = (res as BaseResult).List;
        this.MaterialService.ListFilter = [];
        this.MaterialFilter(environment.InitializationString);
      },
      err => {
      },
      () => {
        this.InventoryService.IsShowLoading = false;
      }
    );
  }
  MaterialFilter(searchString: string) {
    this.MaterialService.Filter10(searchString);
  }
  MaterialChange(element: InventoryDetail) {
    let List = this.MaterialService.List.filter(o => o.ID == element.MaterialID);
    if (List && List.length > 0) {
      element.Active = true;
      element.MaterialName = List[0].Code;
      let ID = "QuantityActual" + element.ID;
      let elementQuantityActual = document.querySelector("#" + ID);
      elementQuantityActual.scrollIntoView(true);
      //document?.getElementById(ID)?.focus();
      // const elementQuantityActual = document.getElementById(ID) as HTMLElement;
      // console.log(ID);
      // console.log(elementQuantityActual);
      // if (elementQuantityActual) {
      //   elementQuantityActual.focus();
      //   console.log(1);
      // }
    }
  }
  MaterialChange0() {
    let List = this.MaterialService.List.filter(o => o.ID == this.InventoryDetailService.BaseParameter.BaseModel.MaterialID);
    if (List && List.length > 0) {
      this.InventoryDetailService.BaseParameter.BaseModel.Active = true;
      this.InventoryDetailService.BaseParameter.BaseModel.MaterialName = List[0].Code;
      
    }
    this.QuantityActual.nativeElement.focus();    
  }
  InventoryDetailWeek0() {
    //this.MaterialID.nativeElement.focus();
  }
  InventoryDetailMaterialName0() {
    this.QuantityActual.nativeElement.focus();
  }
  InventoryDetailQuantityActual0() {
    this.CategoryLocationName.nativeElement.focus();
  }
  InventoryDetailSave0() {
    //this.InventoryService.IsShowLoading = true;
    this.InventoryDetailService.SaveAsync().subscribe(
      res => {
        this.InventoryDetailSearch();
        this.InventoryDetailService.BaseParameter.BaseModel = {
          ID: 0,
          Active: true
        };
        this.Week.nativeElement.focus();
        this.NotificationService.warn(environment.SaveSuccess);
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.InventoryService.IsShowLoading = false;
      }
    );
  }
  Date(value) {
    this.InventoryService.BaseParameter.BaseModel.Date = new Date(value);
  }
  CompanySearch() {
    this.CompanyService.ComponentGetByMembershipID_ActiveToListAsync(this.InventoryService);
    this.CompanyChange();
  }
  CompanyChange() {
    this.CategoryDepartmentSearch();
  }
  CategoryDepartmentSearch() {
    this.CategoryDepartmentService.BaseParameter.CompanyID = this.InventoryService.BaseParameter.BaseModel.CompanyID;
    this.CategoryDepartmentService.ComponentGetByMembershipID_CompanyID_ActiveToListAsync(this.InventoryService);
    this.CategoryDepartmentService.ComponentGetByCompanyIDAndActiveToList(this.InventoryService);
  }
  InventorySearch() {
    this.InventoryService.IsShowLoading = true;
    this.InventoryService.GetByIDAsync().subscribe(
      res => {
        this.InventoryService.BaseParameter.BaseModel = (res as BaseResult).BaseModel;
        this.CompanyChange();
        this.MaterialSearch();
        this.InventoryDetailSearch();
        this.InventoryDetailBarcodeSearch();
      },
      err => {
      },
      () => {
        this.InventoryService.IsShowLoading = false;
      }
    );
  }
  Add(ID: number) {
    this.Router.navigateByUrl("InventoryInfo/" + ID);
    this.InventoryService.BaseParameter.ID = ID;
    this.InventorySearch();
  }
  Save() {
    this.InventoryService.IsShowLoading = true;
    this.InventoryService.SaveAsync().subscribe(
      res => {
        this.InventoryService.BaseParameter.BaseModel = (res as BaseResult).BaseModel;
        this.Add(this.InventoryService.BaseParameter.BaseModel.ID);
        this.NotificationService.warn(environment.SaveSuccess);
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.InventoryService.IsShowLoading = false;
      }
    );
  }
  InventoryDetailSearch() {

    if (this.InventoryService.BaseParameter.BaseModel.ID > 0) {
      if (this.InventoryDetailService.BaseParameter.SearchString.length > 0) {
        this.InventoryDetailService.BaseParameter.SearchString = this.InventoryDetailService.BaseParameter.SearchString.trim();
        // if (this.InventoryDetailService.DataSource) {
        //   this.InventoryDetailService.DataSource.filter = this.InventoryDetailService.BaseParameter.SearchString.toLowerCase();
        // }
        let SearchString = this.InventoryDetailService.BaseParameter.SearchString;
        this.InventoryDetailService.ListYear = this.InventoryDetailService.List.filter(o => o.ID > 0 && ((o.Week > 0 && o.Week.toString().includes(SearchString)) || (o.MaterialName && o.MaterialName.length > 0 && o.MaterialName.includes(SearchString)) || (o.CategoryLocationName && o.CategoryLocationName.length > 0 && o.CategoryLocationName.includes(SearchString)) || (o.Description && o.Description.length > 0 && o.Description.includes(SearchString))));
        this.InventoryDetailService.DataSource = new MatTableDataSource(this.InventoryDetailService.ListYear);
        this.InventoryDetailService.DataSource.sort = this.InventoryDetailSort;
        this.InventoryDetailService.DataSource.paginator = this.InventoryDetailPaginator;
      }
      else {
        this.InventoryDetailService.BaseParameter.ParentID = this.InventoryService.BaseParameter.BaseModel.ID;
        this.InventoryService.IsShowLoading = true;
        this.InventoryDetailService.GetByParentIDAndEmptyToListAsync().subscribe(
          res => {
            this.InventoryDetailService.List = (res as BaseResult).List.sort((a, b) => (a.UpdateDate < b.UpdateDate ? 1 : -1));

            this.InventoryDetailService.ListYear = this.InventoryDetailService.List;
            this.InventoryDetailService.DataSource = new MatTableDataSource(this.InventoryDetailService.ListYear);
            this.InventoryDetailService.DataSource.sort = this.InventoryDetailSort;
            this.InventoryDetailService.DataSource.paginator = this.InventoryDetailPaginator;
          },
          err => {
          },
          () => {
            this.InventoryService.IsShowLoading = false;
          }
        );
      }

      // if (this.InventoryDetailService.BaseParameter.SearchString.length > 0) {
      //   this.InventoryDetailService.BaseParameter.SearchString = this.InventoryDetailService.BaseParameter.SearchString.trim();
      //   if (this.InventoryDetailService.DataSource) {
      //     this.InventoryDetailService.DataSourceFilter.filter = this.InventoryDetailService.BaseParameter.SearchString.toLowerCase();
      //   }
      // }
      // else {
      //   this.InventoryDetailService.BaseParameter.ParentID = this.InventoryService.BaseParameter.BaseModel.ID;
      //   this.InventoryDetailService.BaseParameter.Active = false;
      //   this.InventoryService.IsShowLoading = true;
      //   this.InventoryDetailService.GetByParentIDAndActiveToListAsync().subscribe(
      //     res => {
      //       this.InventoryDetailService.ListFilter = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
      //       this.InventoryDetailService.DataSourceFilter = new MatTableDataSource(this.InventoryDetailService.ListFilter);
      //       this.InventoryDetailService.DataSourceFilter.sort = this.InventoryDetailSortFilter;
      //       this.InventoryDetailService.DataSourceFilter.paginator = this.InventoryDetailPaginatorFilter;
      //     },
      //     err => {
      //     },
      //     () => {
      //       this.InventoryService.IsShowLoading = false;
      //     }
      //   );
      // }
    }
    else {
      this.InventoryDetailBarcodeService.List = [];
      this.InventoryDetailBarcodeService.DataSource = new MatTableDataSource(this.InventoryDetailBarcodeService.List);
      this.InventoryDetailBarcodeService.DataSource.sort = this.InventoryDetailBarcodeSort;
      this.InventoryDetailBarcodeService.DataSource.paginator = this.InventoryDetailBarcodePaginator;
      this.InventoryService.IsShowLoading = false;
    }


    this.InventoryDetailBarcodeService.BaseParameter.SearchString = this.InventoryDetailService.BaseParameter.SearchString;
    this.InventoryDetailBarcodeSearch();
  }
  InventoryDetailSave(element: InventoryDetail) {
    //this.InventoryService.IsShowLoading = true;
    element.ParentID = this.InventoryService.BaseParameter.BaseModel.ID;
    this.InventoryDetailService.BaseParameter.BaseModel = element;
    this.InventoryDetailService.SaveAsync().subscribe(
      res => {
        this.InventoryDetailSearch();
        this.NotificationService.warn(environment.SaveSuccess);
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.InventoryService.IsShowLoading = false;
      }
    );
  }
  InventoryDetailDelete(element: InventoryDetail) {
    this.InventoryDetailService.BaseParameter.ID = element.ID;
    if (confirm(environment.DeleteConfirm)) {
      this.InventoryService.IsShowLoading = true;
      this.InventoryDetailService.RemoveAsync().subscribe(
        res => {
          this.InventoryDetailSearch();
          this.NotificationService.warn(environment.DeleteSuccess);
        },
        err => {
          this.NotificationService.warn(environment.DeleteNotSuccess);
        },
        () => {
          this.InventoryService.IsShowLoading = false;
        }
      );
    }
  }
  InventoryDetailSync() {
    this.InventoryService.IsShowLoading = true;
    this.InventoryDetailService.BaseParameter.ParentID = this.InventoryService.BaseParameter.BaseModel.ID;
    this.InventoryDetailService.SyncByParentIDToListAsync().subscribe(
      res => {
        this.InventoryDetailSearch();
        this.NotificationService.warn(environment.SaveSuccess);
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.InventoryService.IsShowLoading = false;
      }
    );
  }
  InventoryDetailSyncCategoryLocationName() {
    this.InventoryService.IsShowLoading = true;
    this.InventoryDetailService.BaseParameter.ParentID = this.InventoryService.BaseParameter.BaseModel.ID;
    this.InventoryDetailService.SyncByParentIDCategoryLocationNameToListAsync().subscribe(
      res => {
        this.InventoryDetailSearch();
        this.NotificationService.warn(environment.SaveSuccess);
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.InventoryService.IsShowLoading = false;
      }
    );
  }
  PrintByID(element: InventoryDetail) {
    this.InventoryService.IsShowLoading = true;
    this.InventoryDetailService.BaseParameter.ID = element.ID;
    this.InventoryDetailService.PrintByIDAsync().subscribe(
      res => {
        this.InventoryDetailService.BaseResult = (res as BaseResult);
        this.NotificationService.OpenWindowByURL(this.InventoryDetailService.BaseResult.Message);
      },
      err => {

      },
      () => {
        this.InventoryService.IsShowLoading = false;
      }
    );
  }
  Print2025ByID(element: InventoryDetail) {
    this.InventoryService.IsShowLoading = true;
    this.InventoryDetailService.BaseParameter.ID = element.ID;
    this.InventoryDetailService.Print2025ByIDAsync().subscribe(
      res => {
        this.InventoryDetailService.BaseResult = (res as BaseResult);
        this.NotificationService.OpenWindowByURL(this.InventoryDetailService.BaseResult.Message);
      },
      err => {

      },
      () => {
        this.InventoryService.IsShowLoading = false;
      }
    );
  }
  InventoryDetailBarcodeSearch() {
    if (this.InventoryService.BaseParameter.BaseModel.ID > 0) {
      if (this.InventoryDetailBarcodeService.BaseParameter.SearchString.length > 0) {
        this.InventoryDetailBarcodeService.BaseParameter.SearchString = this.InventoryDetailBarcodeService.BaseParameter.SearchString.trim();
        // if (this.InventoryDetailBarcodeService.DataSource) {
        //   this.InventoryDetailBarcodeService.DataSource.filter = this.InventoryDetailBarcodeService.BaseParameter.SearchString.toLowerCase();
        // }

        let SearchString = this.InventoryDetailBarcodeService.BaseParameter.SearchString;
        this.InventoryDetailBarcodeService.ListYear = this.InventoryDetailBarcodeService.List.filter(o => o.ID > 0 && ((o.Week > 0 && o.Week.toString().includes(SearchString)) || (o.MaterialName && o.MaterialName.length > 0 && o.MaterialName.includes(SearchString)) || (o.CategoryLocationName && o.CategoryLocationName.length > 0 && o.CategoryLocationName.includes(SearchString)) || (o.Barcode && o.Barcode.length > 0 && o.Barcode.includes(SearchString))));
        this.InventoryDetailBarcodeService.DataSource = new MatTableDataSource(this.InventoryDetailBarcodeService.ListYear);
        this.InventoryDetailBarcodeService.DataSource.sort = this.InventoryDetailBarcodeSort;
        this.InventoryDetailBarcodeService.DataSource.paginator = this.InventoryDetailBarcodePaginator;

        this.InventoryDetailBarcodeService.DataSourceFilter = new MatTableDataSource(this.InventoryDetailBarcodeService.ListYear);
        this.InventoryDetailBarcodeService.DataSourceFilter.sort = this.InventoryDetailBarcodeSortFilter;
        this.InventoryDetailBarcodeService.DataSourceFilter.paginator = this.InventoryDetailBarcodePaginatorFilter;
      }
      else {
        this.InventoryDetailBarcodeService.BaseParameter.ParentID = this.InventoryService.BaseParameter.BaseModel.ID;
        this.InventoryService.IsShowLoading = true;
        this.InventoryDetailBarcodeService.GetByParentIDAndActiveToListAsync().subscribe(
          res => {
            this.InventoryDetailBarcodeService.List = (res as BaseResult).List.sort((a, b) => (a.Week < b.Week ? 1 : -1));
            let InventoryDetailBarcode: InventoryDetailBarcode;
            InventoryDetailBarcode = {
              ID: 0,
              ParentID: this.InventoryService.BaseParameter.BaseModel.ID
            };
            this.InventoryDetailBarcodeService.List.push(InventoryDetailBarcode);
            this.InventoryDetailBarcodeService.ListYear = this.InventoryDetailBarcodeService.List.sort((a, b) => (a.Week < b.Week ? 1 : -1));
            this.InventoryDetailBarcodeService.DataSource = new MatTableDataSource(this.InventoryDetailBarcodeService.ListYear);
            this.InventoryDetailBarcodeService.DataSource.sort = this.InventoryDetailBarcodeSort;
            this.InventoryDetailBarcodeService.DataSource.paginator = this.InventoryDetailBarcodePaginator;

            this.InventoryDetailBarcodeService.DataSourceFilter = new MatTableDataSource(this.InventoryDetailBarcodeService.ListYear);
            this.InventoryDetailBarcodeService.DataSourceFilter.sort = this.InventoryDetailBarcodeSortFilter;
            this.InventoryDetailBarcodeService.DataSourceFilter.paginator = this.InventoryDetailBarcodePaginatorFilter;
          },
          err => {
          },
          () => {
            this.InventoryService.IsShowLoading = false;
          }
        );
      }
    }
    else {
      this.InventoryDetailBarcodeService.List = [];
      this.InventoryDetailBarcodeService.DataSource = new MatTableDataSource(this.InventoryDetailBarcodeService.List);
      this.InventoryDetailBarcodeService.DataSource.sort = this.InventoryDetailBarcodeSort;
      this.InventoryDetailBarcodeService.DataSource.paginator = this.InventoryDetailBarcodePaginator;
      this.InventoryService.IsShowLoading = false;
    }
  }
  InventoryDetailBarcodeDelete(element: InventoryDetailBarcode) {
    this.InventoryDetailBarcodeService.BaseParameter.ID = element.ID;
    if (confirm(environment.DeleteConfirm)) {
      this.InventoryService.IsShowLoading = true;
      this.InventoryDetailBarcodeService.RemoveAsync().subscribe(
        res => {
          this.InventoryDetailBarcodeSearch();
          this.NotificationService.warn(environment.DeleteSuccess);
        },
        err => {
          this.NotificationService.warn(environment.DeleteNotSuccess);
        },
        () => {
          this.InventoryService.IsShowLoading = false;
        }
      );
    }
  }
  InventoryDetailBarcodeSave(element: InventoryDetailBarcode) {
    this.InventoryService.IsShowLoading = true;
    element.ParentID = this.InventoryService.BaseParameter.BaseModel.ID;
    this.InventoryDetailBarcodeService.BaseParameter.BaseModel = element;
    this.InventoryDetailBarcodeService.SaveAsync().subscribe(
      res => {
        this.NotificationService.warn(environment.SaveSuccess);
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.InventoryService.IsShowLoading = false;
      }
    );
  }
  @ViewChild("TABLEInventoryDetail") tableInventoryDetail: ElementRef;
  InventoryDetailExcel() {
    const ws: XLSX.WorkSheet = XLSX.utils.table_to_sheet(this.tableInventoryDetail.nativeElement);
    const wb: XLSX.WorkBook = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, "Sheet1");
    let filename = this.InventoryService.BaseParameter.BaseModel.ID + "-" + this.InventoryService.BaseParameter.BaseModel.Code + "-InventoryPARTNO.xlsx";
    XLSX.writeFile(wb, filename);
  }
  @ViewChild("TABLE") table: ElementRef;
  InventoryDetailBarcodeExcel() {
    const ws: XLSX.WorkSheet = XLSX.utils.table_to_sheet(this.table.nativeElement);
    const wb: XLSX.WorkBook = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, "Sheet1");
    let filename = this.InventoryService.BaseParameter.BaseModel.ID + "-" + this.InventoryService.BaseParameter.BaseModel.Code + "-InventoryBarcode.xlsx";
    XLSX.writeFile(wb, filename);
  }
  InventoryDetailBarcodeDownload() {
    this.InventoryService.IsShowLoading = true;
    this.InventoryDetailBarcodeService.ExportToExcelAsync().subscribe(
      res => {
        this.InventoryDetailBarcodeService.BaseResult = (res as BaseResult);
        if (this.InventoryDetailBarcodeService.BaseResult) {
          let url = this.InventoryDetailBarcodeService.BaseResult.Message;
          window.open(url, "_blank");
          url = this.InventoryDetailBarcodeService.BaseResult.Note;
          window.open(url, "_blank");
        }
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.InventoryService.IsShowLoading = false;
      }
    );
  }
  InventoryDetailBarcodeDownloadCategoryLocationName() {
    this.InventoryService.IsShowLoading = true;
    this.InventoryDetailBarcodeService.ExportWithCategoryLocationNameToExcelAsync().subscribe(
      res => {
        this.InventoryDetailBarcodeService.BaseResult = (res as BaseResult);
        if (this.InventoryDetailBarcodeService.BaseResult) {
          let url = this.InventoryDetailBarcodeService.BaseResult.Note;
          window.open(url, "_blank");
        }
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.InventoryService.IsShowLoading = false;
      }
    );
  }
  InventoryDetailBarcodeDownloadQuantity() {
    this.InventoryService.IsShowLoading = true;
    this.InventoryDetailBarcodeService.ExportWithQuantityToExcelAsync().subscribe(
      res => {
        this.InventoryDetailBarcodeService.BaseResult = (res as BaseResult);
        if (this.InventoryDetailBarcodeService.BaseResult) {
          let url = this.InventoryDetailBarcodeService.BaseResult.Note;
          window.open(url, "_blank");
        }
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.InventoryService.IsShowLoading = false;
      }
    );
  }
  InventoryDetailBarcodeDownloadNotExist() {
    this.InventoryService.IsShowLoading = true;
    this.InventoryDetailBarcodeService.ExportWithNotExistToExcelAsync().subscribe(
      res => {
        this.InventoryDetailBarcodeService.BaseResult = (res as BaseResult);
        if (this.InventoryDetailBarcodeService.BaseResult) {
          let url = this.InventoryDetailBarcodeService.BaseResult.Note;
          window.open(url, "_blank");
        }
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.InventoryService.IsShowLoading = false;
      }
    );
  }
  InventoryDetailDownload() {
    this.InventoryService.IsShowLoading = true;
    this.InventoryDetailService.ExportToExcelAsync().subscribe(
      res => {
        this.InventoryDetailService.BaseResult = (res as BaseResult);
        if (this.InventoryDetailService.BaseResult) {
          let url = this.InventoryDetailService.BaseResult.Message;
          window.open(url, "_blank");
        }
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.InventoryService.IsShowLoading = false;
      }
    );
  }
}
