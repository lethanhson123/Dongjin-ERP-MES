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


import { Inventory } from 'src/app/shared/ERP/Inventory.model';
import { InventoryService } from 'src/app/shared/ERP/Inventory.service';

import { InventoryDetail } from 'src/app/shared/ERP/InventoryDetail.model';
import { InventoryDetailService } from 'src/app/shared/ERP/InventoryDetail.service';

import { InventoryDetailBarcode } from 'src/app/shared/ERP/InventoryDetailBarcode.model';
import { InventoryDetailBarcodeService } from 'src/app/shared/ERP/InventoryDetailBarcode.service';


import { CategoryDepartment } from 'src/app/shared/ERP/CategoryDepartment.model';
import { CategoryDepartmentService } from 'src/app/shared/ERP/CategoryDepartment.service';

@Component({
  selector: 'app-inventory-scan-desktop',
  templateUrl: './inventory-scan-desktop.component.html',
  styleUrls: ['./inventory-scan-desktop.component.css']
})
export class InventoryScanDesktopComponent {

  @ViewChild('InventoryDetailBarcodeSort') InventoryDetailBarcodeSort: MatSort;
  @ViewChild('InventoryDetailBarcodePaginator') InventoryDetailBarcodePaginator: MatPaginator;

  @ViewChild('InventoryDetailSort') InventoryDetailSort: MatSort;
  @ViewChild('InventoryDetailPaginator') InventoryDetailPaginator: MatPaginator;

  @ViewChild('SearchString') SearchString!: ElementRef;
  @ViewChild('SearchStringFilter') SearchStringFilter!: ElementRef;

  QuantityActive: number = 0;

  PrintURL: string = environment.PrintURL;

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

  ) {
    this.InventoryDetailService.BaseParameter.Active = false;
    this.InventoryDetailActiveChange();
    this.InventoryDetailBarcodeService.BaseParameter.Action = environment.InitializationNumber;
    this.InventoryDetailBarcodeService.BaseParameter.Page = environment.InitializationNumber;
    this.InventoryDetailBarcodeService.BaseParameter.PageSize = environment.InitializationNumber;
    this.CategoryDepartmentSearch();
  }

  ngOnInit(): void {
  }

  ngAfterViewInit() {
    this.SearchString.nativeElement.focus();
    this.InventoryDetailBarcodeService.ListFilter = [];
    this.InventoryDetailBarcodeService.ListParent = [];

    this.InventoryDetailService.ListFilter = [];

    this.StartTimer();
  }
  StartTimer() {
    setInterval(() => {
      this.InventoryService.IsShowLoading = false;
    }, environment.TimerMax)
  }
  InventoryDetailActiveChange() {
    if (this.InventoryDetailService.BaseParameter.Active == true) {
      this.InventoryDetailService.APIURL = environment.PrintURL;
      this.InventoryDetailService.APIRootURL = environment.PrintRootURL;
    }
    else {
      this.InventoryDetailService.APIURL = environment.APIReportURL;
      this.InventoryDetailService.APIRootURL = environment.APIReportRootURL;
    }
  }
  CategoryDepartmentSearch() {
    this.CategoryDepartmentService.BaseParameter.Active = true;
    this.CategoryDepartmentService.BaseParameter.MembershipID = Number(localStorage.getItem(environment.UserID));
    this.CategoryDepartmentService.GetByMembershipID_ActiveToListAsync().subscribe(
      res => {
        this.CategoryDepartmentService.ListFilter = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
        if (this.CategoryDepartmentService.ListFilter && this.CategoryDepartmentService.ListFilter.length > 0) {
          this.InventoryDetailBarcodeService.BaseParameter.CategoryDepartmentID = this.CategoryDepartmentService.ListFilter[0].ID;
          if (this.CategoryDepartmentService.ListFilter.length > 1) {
            let List = this.CategoryDepartmentService.ListFilter.filter(o => o.Code.includes("Warehouse") || o.Code.includes("FinishGoods") || o.Code.includes("HOOKRACK"));
            if (List && List.length > 0) {
              this.InventoryDetailBarcodeService.BaseParameter.CategoryDepartmentID = List[0].ID;
              this.CategoryDepartmentChange();
            }
          }
        }
      },
      err => {
      },
      () => {
      }
    );
  }
  CategoryDepartmentChange() {
    this.InventorySearch();
  }
  InventorySearch() {
    this.InventoryService.BaseParameter.CategoryDepartmentID = this.InventoryDetailBarcodeService.BaseParameter.CategoryDepartmentID;
    this.InventoryService.GetByCategoryDepartmentIDToListAsync().subscribe(
      res => {
        this.InventoryService.ListFilter = (res as BaseResult).List.sort((a, b) => (a.Date < b.Date ? 1 : -1));
        if (this.InventoryService.ListFilter && this.InventoryService.ListFilter.length > 0) {
          this.InventoryDetailBarcodeService.BaseParameter.ParentID = this.InventoryService.ListFilter[0].ID;
        }
      },
      err => {
      },
      () => {
      }
    );
  }
  InventoryDetailBarcodeSearchString() {
    if (this.InventoryDetailBarcodeService.BaseParameter.Action == environment.InitializationNumber) {
      this.InventoryDetailBarcodeService.BaseParameter.Action = this.InventoryDetailBarcodeService.BaseParameter.Page;
    }
    if (this.InventoryDetailBarcodeService.BaseParameter.SearchString && this.InventoryDetailBarcodeService.BaseParameter.SearchString.length > 0) {
      this.InventoryDetailBarcodeService.BaseParameter.SearchString = this.InventoryDetailBarcodeService.BaseParameter.SearchString.trim();
    }
    let audio = new Audio("/Media/Success.wav");
    audio.play();
    this.SearchStringFilter.nativeElement.focus();

    this.InventoryDetailService.ListFilter = [];
    this.InventoryDetailService.DataSource = new MatTableDataSource(this.InventoryDetailService.ListFilter);
    this.InventoryDetailService.DataSource.sort = this.InventoryDetailSort;
    this.InventoryDetailService.DataSource.paginator = this.InventoryDetailPaginator;
  }
  InventoryDetailBarcodeSearchStringFilter() {

    if (this.InventoryDetailBarcodeService.BaseParameter.SearchStringFilter && this.InventoryDetailBarcodeService.BaseParameter.SearchStringFilter.length > 0) {
      if (this.InventoryDetailBarcodeService.BaseParameter.SearchStringFilter == this.InventoryDetailBarcodeService.BaseParameter.SearchString) {
        this.Save();
      }
      else {
        this.InventoryDetailBarcodeService.BaseParameter.SearchStringFilter = this.InventoryDetailBarcodeService.BaseParameter.SearchStringFilter.trim();
        if (this.InventoryDetailBarcodeService.BaseParameter.SearchStringFilter.includes("$$")) {
          this.InventoryDetailBarcodeCheck();
        }
        else {
          if (this.InventoryDetailBarcodeService.ListParent && this.InventoryDetailBarcodeService.ListParent.length > 0) {
            let List = this.InventoryDetailBarcodeService.ListParent.filter(o => o.Description == this.InventoryDetailBarcodeService.BaseParameter.SearchStringFilter && o.CategoryLocationName == this.InventoryDetailBarcodeService.BaseParameter.SearchString);
            if (List && List.length > 0) {
            }
            else {
              List = this.InventoryDetailBarcodeService.ListParent.filter(o => o.Description == this.InventoryDetailBarcodeService.BaseParameter.SearchStringFilter);
            }
            if (List && List.length > 0) {
            }
            else {
              List = this.InventoryDetailBarcodeService.ListParent.filter(o => o.Barcode == this.InventoryDetailBarcodeService.BaseParameter.SearchStringFilter && o.CategoryLocationName == this.InventoryDetailBarcodeService.BaseParameter.SearchString);
            }
            if (List && List.length > 0) {
            }
            else {
              List = this.InventoryDetailBarcodeService.ListParent.filter(o => o.Barcode == this.InventoryDetailBarcodeService.BaseParameter.SearchStringFilter);
            }
            console.log(this.InventoryDetailBarcodeService.BaseParameter.SearchStringFilter);
            console.log(List);
            if (List && List.length > 0) {
              let audio = new Audio("/Media/Success.wav");
              audio.play();
              for (let i = 0; i < List.length; i++) {
                let InventoryDetailBarcode = List[i];
                InventoryDetailBarcode.ParentID = this.InventoryDetailBarcodeService.BaseParameter.ParentID;
                InventoryDetailBarcode.UpdateUserID = Number(localStorage.getItem(environment.UserID));
                InventoryDetailBarcode.CategoryLocationName = this.InventoryDetailBarcodeService.BaseParameter.SearchString;
                InventoryDetailBarcode.ProductID = 1;
                if (this.InventoryDetailBarcodeService.ListFilter && this.InventoryDetailBarcodeService.ListFilter.length > 0) {
                  InventoryDetailBarcode.ProductID = this.InventoryDetailBarcodeService.ListFilter.length + 1;
                }
                InventoryDetailBarcode.Active = true;
                InventoryDetailBarcode.Quantity01 = InventoryDetailBarcode.Quantity;
                let ListCheck = this.InventoryDetailBarcodeService.ListFilter.filter(o => o.Barcode == InventoryDetailBarcode.Barcode);
                if (ListCheck && ListCheck.length > 0) {
                }
                else {
                  this.InventoryDetailBarcodeService.ListFilter.push(InventoryDetailBarcode);
                }
              }
              this.InventoryDetailBarcodeService.ListFilter = this.InventoryDetailBarcodeService.ListFilter.sort((a, b) => (a.ProductID < b.ProductID ? 1 : -1));
              this.InventoryDetailBarcodeService.DataSource = new MatTableDataSource(this.InventoryDetailBarcodeService.ListFilter);
              this.InventoryDetailBarcodeService.DataSource.sort = this.InventoryDetailBarcodeSort;
              this.InventoryDetailBarcodeService.DataSource.paginator = this.InventoryDetailBarcodePaginator;
              this.InventoryDetail(0);
              this.InventoryDetailBarcodeService.BaseParameter.SearchStringFilter = environment.InitializationString;
              this.SearchStringFilter.nativeElement.focus();
            }
            else {
              this.InventoryDetailBarcodeCheck();
            }
          }
        }
      }
    }
  }
  InventoryDetailBarcodeCheck() {
    let InventoryDetailBarcode: InventoryDetailBarcode;
    InventoryDetailBarcode = {
    };
    InventoryDetailBarcode.IsScan = true;
    InventoryDetailBarcode.Quantity = 0;
    InventoryDetailBarcode.ParentID = this.InventoryDetailBarcodeService.BaseParameter.ParentID;
    if (this.InventoryDetailBarcodeService.ListParent && this.InventoryDetailBarcodeService.ListParent.length > 0) {
      let List = this.InventoryDetailBarcodeService.ListParent.filter(o => o.Barcode == this.InventoryDetailBarcodeService.BaseParameter.SearchStringFilter && o.CategoryLocationName == this.InventoryDetailBarcodeService.BaseParameter.SearchString);
      if (List && List.length > 0) {
      }
      else {
        List = this.InventoryDetailBarcodeService.ListParent.filter(o => o.Barcode == this.InventoryDetailBarcodeService.BaseParameter.SearchStringFilter);
      }
      if (List && List.length > 0) {
        List = List.sort((a, b) => (a.Quantity < b.Quantity ? 1 : -1));
        InventoryDetailBarcode = List[0];
      }
    }
    this.InventoryDetailService.BaseParameter.Active = InventoryDetailBarcode.IsScan;
    InventoryDetailBarcode.UpdateUserID = Number(localStorage.getItem(environment.UserID));
    InventoryDetailBarcode.CategoryLocationName = this.InventoryDetailBarcodeService.BaseParameter.SearchString;
    if (InventoryDetailBarcode.Description && InventoryDetailBarcode.Description.length > 0) {
    }
    else {
      InventoryDetailBarcode.Description = InventoryDetailBarcode.CategoryLocationName;
    }
    InventoryDetailBarcode.Barcode = this.InventoryDetailBarcodeService.BaseParameter.SearchStringFilter;
    if (InventoryDetailBarcode.MaterialName && InventoryDetailBarcode.MaterialName.length > 0) {
    }
    else {
      var List = InventoryDetailBarcode.Barcode.split("$$");
      if (List.length > 0) {
        InventoryDetailBarcode.MaterialName = List[0];
      }
    }
    if (InventoryDetailBarcode.ID > 0) {
      InventoryDetailBarcode.Quantity01 = InventoryDetailBarcode.Quantity;
    }
    else {
      var List = InventoryDetailBarcode.Barcode.split("$$");
      if (List.length > 1) {
        let Quantity01 = List[1];
        Quantity01 = Quantity01.replace(",", "");
        Quantity01 = Quantity01.replace(".", "");
        InventoryDetailBarcode.Quantity01 = Number(Quantity01);
      }
    }
    if (InventoryDetailBarcode.Quantity01 == null || InventoryDetailBarcode.Quantity01 == 0) {
      var List = InventoryDetailBarcode.Barcode.split("$$");
      if (List.length > 1) {
        let Quantity01 = List[1];
        Quantity01 = Quantity01.replace(",", "");
        Quantity01 = Quantity01.replace(".", "");
        InventoryDetailBarcode.Quantity01 = Number(Quantity01);
      }
    }
    InventoryDetailBarcode.ProductID = 1;
    if (this.InventoryDetailBarcodeService.ListFilter && this.InventoryDetailBarcodeService.ListFilter.length > 0) {
      InventoryDetailBarcode.ProductID = this.InventoryDetailBarcodeService.ListFilter.length + 1;
    }
    let ListCheck = this.InventoryDetailBarcodeService.ListFilter.filter(o => o.Barcode == InventoryDetailBarcode.Barcode);
    if (ListCheck && ListCheck.length > 0) {
      let audio = new Audio("/Media/SuccessNot.wav");
      audio.play();
    }
    else {
      if (InventoryDetailBarcode.Active == true) {
        let audio = new Audio("/Media/SuccessNot.wav");
        audio.play();
      }
      else {
        let audio = new Audio("/Media/Success.wav");
        audio.play();
      }
      InventoryDetailBarcode.Active = true;
      this.InventoryDetailBarcodeService.ListFilter.push(InventoryDetailBarcode);
    }
    this.InventoryDetailBarcodeService.ListFilter = this.InventoryDetailBarcodeService.ListFilter.sort((a, b) => (a.ProductID < b.ProductID ? 1 : -1));
    this.InventoryDetailBarcodeService.DataSource = new MatTableDataSource(this.InventoryDetailBarcodeService.ListFilter);
    this.InventoryDetailBarcodeService.DataSource.sort = this.InventoryDetailBarcodeSort;
    this.InventoryDetailBarcodeService.DataSource.paginator = this.InventoryDetailBarcodePaginator;
    this.InventoryDetail(0);
    this.InventoryDetailBarcodeService.BaseParameter.SearchStringFilter = environment.InitializationString;
    this.SearchStringFilter.nativeElement.focus();
  }
  InventoryDetail(Event: number) {
    this.InventoryDetailService.List = this.InventoryDetailService.ListFilter;
    this.InventoryDetailService.ListFilter = [];

    let key = 'MaterialName';

    let ListInventoryDetailBarcodeMaterialName = [...new Map(this.InventoryDetailBarcodeService.ListFilter.map(item =>
      [item[key], item])).values()];

    if (ListInventoryDetailBarcodeMaterialName && ListInventoryDetailBarcodeMaterialName.length > 0) {
      for (let i = 0; i < ListInventoryDetailBarcodeMaterialName.length; i++) {
        let InventoryDetail: InventoryDetail;
        InventoryDetail = {
        };
        InventoryDetail.CreateDate = new Date();
        InventoryDetail.CreateUserID = Number(localStorage.getItem(environment.UserID));
        InventoryDetail.CreateUserCode = localStorage.getItem(environment.UserName);
        InventoryDetail.CreateUserName = localStorage.getItem(environment.FullName);
        InventoryDetail.UpdateUserID = InventoryDetail.CreateUserID
        InventoryDetail.UpdateUserCode = InventoryDetail.CreateUserCode;
        InventoryDetail.UpdateUserName = InventoryDetail.CreateUserName
        InventoryDetail.ParentID = this.InventoryService.BaseParameter.BaseModel.ID;
        InventoryDetail.MaterialName = ListInventoryDetailBarcodeMaterialName[i].MaterialName;
        if (InventoryDetail.CategoryLocationName && InventoryDetail.CategoryLocationName.length > 0) {
        }
        else {
          InventoryDetail.CategoryLocationName = this.InventoryDetailBarcodeService.BaseParameter.SearchString;
        }
        InventoryDetail.Quantity = 0;
        let ListInventoryDetailBarcode = this.InventoryDetailBarcodeService.ListFilter.filter(o => o.MaterialName == InventoryDetail.MaterialName);
        if (ListInventoryDetailBarcode && ListInventoryDetailBarcode.length > 0) {
          InventoryDetail.Count = ListInventoryDetailBarcode.length;
          for (let j = 0; j < ListInventoryDetailBarcode.length; j++) {
            if (ListInventoryDetailBarcode[j].Quantity01 > 0) {
              InventoryDetail.Quantity = InventoryDetail.Quantity + ListInventoryDetailBarcode[j].Quantity01;
            }
          }
        }
        this.InventoryDetailService.ListFilter.push(InventoryDetail);

      }
    }

    this.InventoryDetailService.DataSource = new MatTableDataSource(this.InventoryDetailService.ListFilter);
    this.InventoryDetailService.DataSource.sort = this.InventoryDetailSort;
    this.InventoryDetailService.DataSource.paginator = this.InventoryDetailPaginator;
  }
  InventoryDetailSave(Event: number) {
    this.InventoryDetail(Event);
    this.InventoryDetailService.ListFilter = this.InventoryDetailService.ListFilter.sort((a, b) => (a.MaterialName > b.MaterialName ? 1 : -1));
    for (let j = 0; j < this.InventoryDetailService.ListFilter.length; j++) {
      this.InventoryDetailService.ListFilter[j].Week = this.InventoryDetailBarcodeService.BaseParameter.Action;
      this.InventoryDetailBarcodeService.BaseParameter.Action = this.InventoryDetailBarcodeService.BaseParameter.Action + 1;
    }
    this.InventoryDetailService.DataSource = new MatTableDataSource(this.InventoryDetailService.ListFilter);
    this.InventoryDetailService.DataSource.sort = this.InventoryDetailSort;
    this.InventoryDetailService.DataSource.paginator = this.InventoryDetailPaginator;

  }
  Save() {
    this.InventoryDetailSave(1);

    //this.InventoryDetailBarcodeService.IsShowLoading = true;
    for (let i = 0; i < this.InventoryDetailBarcodeService.ListFilter.length; i++) {
      let IsSave = false;
      for (let j = 0; j < this.InventoryDetailBarcodeService.ListParent.length; j++) {
        if (this.InventoryDetailBarcodeService.ListFilter[i].Barcode == this.InventoryDetailBarcodeService.ListParent[j].Barcode) {
          this.InventoryDetailBarcodeService.ListParent[j] = this.InventoryDetailBarcodeService.ListFilter[i];
          IsSave = true;
          j = this.InventoryDetailBarcodeService.ListParent.length;
        }
      }
      if (IsSave == false) {
        this.InventoryDetailBarcodeService.ListParent.push(this.InventoryDetailBarcodeService.ListFilter[i]);
      }
    }

    for (let j = 0; j < this.InventoryDetailBarcodeService.ListParent.length; j++) {
      let List = this.InventoryDetailService.ListFilter.filter(o => o.MaterialName == this.InventoryDetailBarcodeService.ListParent[j].MaterialName && o.CategoryLocationName == this.InventoryDetailBarcodeService.ListParent[j].CategoryLocationName);
      if (List && List.length > 0) {
        this.InventoryDetailBarcodeService.ListParent[j].Week = List[0].Week;
      }
    }

    this.InventoryDetailBarcodeService.ListChild = this.InventoryDetailBarcodeService.ListFilter;
    this.InventoryDetailBarcodeService.ListFilter = [];
    this.InventoryDetailBarcodeService.DataSource = new MatTableDataSource(this.InventoryDetailBarcodeService.ListFilter);
    this.InventoryDetailBarcodeService.DataSource.sort = this.InventoryDetailBarcodeSort;
    this.InventoryDetailBarcodeService.DataSource.paginator = this.InventoryDetailBarcodePaginator;
    this.InventoryDetailBarcodeService.BaseParameter.SearchString = environment.InitializationString;
    this.InventoryDetailBarcodeService.BaseParameter.SearchStringFilter = environment.InitializationString;
    this.SearchString.nativeElement.focus();
    this.InventoryDetailBarcodeService.IsShowLoading = false;


    //this.Print();
    //this.PrintLocal();
    this.SaveSync();
  }

  Download() {
    this.InventoryDetailBarcodeService.IsShowLoading = true;
    this.InventoryService.GetByCategoryDepartmentIDAsync().subscribe(
      res => {
        this.InventoryService.BaseParameter.BaseModel = (res as BaseResult).BaseModel;
        this.InventoryDetailBarcodeService.GetByCategoryDepartmentIDToListAsync().subscribe(
          res => {
            this.InventoryDetailBarcodeService.ListParent = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
            let ListActive = this.InventoryDetailBarcodeService.ListParent.filter(o => o.Active == true);
            if (ListActive && ListActive.length > 0) {
              this.QuantityActive = ListActive.length;
            }
          },
          err => {
          },
          () => {
            this.InventoryDetailBarcodeService.IsShowLoading = false;
          }
        );
      },
      err => {
      },
      () => {
      }
    );
    this.SearchString.nativeElement.focus();
  }
  DownloadByParrentID() {
    this.InventoryDetailBarcodeService.IsShowLoading = true;
    this.InventoryService.BaseParameter.ID = this.InventoryDetailBarcodeService.BaseParameter.ParentID;
    this.InventoryService.SyncByIDAsync().subscribe(
      res => {
        this.InventoryDetailBarcodeService.GetByParentIDToListAsync().subscribe(
          res => {
            this.InventoryDetailBarcodeService.ListParent = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
            let ListActive = this.InventoryDetailBarcodeService.ListParent.filter(o => o.Active == true);
            if (ListActive && ListActive.length > 0) {
              this.QuantityActive = ListActive.length;
            }
          },
          err => {
          },
          () => {
            this.InventoryDetailBarcodeService.IsShowLoading = false;
          }
        );
      },
      err => {
      },
      () => {
        //this.InventoryDetailBarcodeService.IsShowLoading = false;
      }
    );
    this.SearchString.nativeElement.focus();
  }
  Sync() {
    //this.InventoryDetailBarcodeService.IsShowLoading = true;
    this.InventoryDetailBarcodeService.BaseParameter.List = this.InventoryDetailBarcodeService.ListParent.filter(o => o.Active == true);
    this.InventoryDetailBarcodeService.SaveListAsync().subscribe(
      res => {
        this.NotificationService.warn(environment.SaveSuccess);
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.InventoryDetailBarcodeService.IsShowLoading = false;
      }
    );
    this.SearchString.nativeElement.focus();
  }
  LocalStorageSync() {

    // let InventoryDetailBarcodeLocalStorageName = "InventoryDetailBarcode";
    // let InventoryDetailBarcodeLocalStorageNameValue = localStorage.getItem(InventoryDetailBarcodeLocalStorageName);
    // if (InventoryDetailBarcodeLocalStorageNameValue && InventoryDetailBarcodeLocalStorageNameValue.length > 0) {
    //   let ListInventoryDetailBarcode = JSON.parse(InventoryDetailBarcodeLocalStorageNameValue);
    //   if (ListInventoryDetailBarcode && ListInventoryDetailBarcode.length > 0) {
    //     for (let j = 0; j < ListInventoryDetailBarcode.length; j++) {
    //       let ListFilter = this.InventoryDetailBarcodeService.ListParent.filter(o => o.ID == ListInventoryDetailBarcode[j].ID);
    //       if (ListFilter && ListFilter.length > 0) {
    //       }
    //       else {
    //         this.InventoryDetailBarcodeService.ListParent.push(ListInventoryDetailBarcode[j]);
    //       }
    //     }
    //   }
    // }
    // let ListActive = this.InventoryDetailBarcodeService.ListParent.filter(o => o.Active == true);
    // console.log("LocalStorageSync");
    // console.log(ListActive);
    // this.Sync();


    let InventoryDetailBarcodeLocalStorageName = "InventoryDetailBarcode";
    let InventoryDetailBarcodeLocalStorageNameValue = localStorage.getItem(InventoryDetailBarcodeLocalStorageName);
    if (InventoryDetailBarcodeLocalStorageNameValue && InventoryDetailBarcodeLocalStorageNameValue.length > 0) {
      this.InventoryDetailBarcodeService.BaseParameter.List = JSON.parse(InventoryDetailBarcodeLocalStorageNameValue);
    }
    console.log("LocalStorageSync");
    console.log(this.InventoryDetailBarcodeService.BaseParameter.List);
    this.InventoryDetailBarcodeService.IsShowLoading = true;
    this.InventoryDetailBarcodeService.SaveListAsync().subscribe(
      res => {
        this.NotificationService.warn(environment.SaveSuccess);
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.InventoryDetailBarcodeService.IsShowLoading = false;
      }
    );

    // for (let i = 0; i < this.InventoryDetailBarcodeService.BaseParameter.List.length; i++) {
    //   this.InventoryDetailBarcodeService.IsShowLoading = true;
    //   this.InventoryDetailBarcodeService.BaseParameter.BaseModel = this.InventoryDetailBarcodeService.BaseParameter.List[i];
    //   console.log(i);
    //   console.log(this.InventoryDetailBarcodeService.BaseParameter.BaseModel);
    //   this.InventoryDetailBarcodeService.SaveAsync().subscribe(
    //     res => {
    //       this.NotificationService.warn(environment.SaveSuccess);
    //     },
    //     err => {
    //       this.NotificationService.warn(environment.SaveNotSuccess);
    //     },
    //     () => {
    //       this.InventoryDetailBarcodeService.IsShowLoading = false;
    //     }
    //   );
    // }

    this.SearchString.nativeElement.focus();
  }
  SaveSync() {
    this.InventoryDetailBarcodeService.BaseParameter.List = this.InventoryDetailBarcodeService.ListChild.filter(o => o.Active == true);
    console.log("SaveSync");
    console.log(this.InventoryDetailBarcodeService.BaseParameter.List);
    this.InventoryDetailBarcodeService.SaveListAsync().subscribe(
      res => {
        this.NotificationService.warn(environment.SaveSuccess);
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.InventoryDetailBarcodeService.IsShowLoading = false;
      }
    );
    this.SearchString.nativeElement.focus();

    let Now = new Date();
    let Year = Now.getFullYear();
    let Month = Now.getMonth() + 1;
    let Day = Now.getDate();
    //let ListActive = this.InventoryDetailBarcodeService.ListParent.filter(o => o.Active == true && o.DateScan != null && o.DateScan?.getFullYear() == Year && (o.DateScan?.getDate() == Day || o.DateScan?.getDate() == Day - 1 || o.DateScan?.getDate() == Day - 2) && (o.DateScan?.getMonth() + 1 == Month));
    let ListActive = this.InventoryDetailBarcodeService.ListParent.filter(o => o.Active == true).sort((a, b) => (a.DateScan < b.DateScan ? 1 : -1));
    let ListActiveLocalStorage = [];
    for (let i = 0; i < 2500; i++) {
      ListActiveLocalStorage.push(ListActive[i]);
    }
    let InventoryDetailBarcodeLocalStorageName = "InventoryDetailBarcode";
    localStorage.setItem(InventoryDetailBarcodeLocalStorageName, JSON.stringify(ListActiveLocalStorage));

  }

  Print() {
    this.InventoryDetailService.BaseParameter.List = this.InventoryDetailService.ListFilter;

    this.InventoryDetailService.PrintByListAsync().subscribe(
      res => {
        this.InventoryDetailService.BaseResult = (res as BaseResult);
        this.NotificationService.OpenWindowByURL2025(this.InventoryDetailService.BaseResult.Message);
      },
      err => {

      },
      () => {
        this.InventoryDetailBarcodeService.IsShowLoading = false;
      }
    );
  }
  HTML() {

  }
  PrintLocal() {
    let ListWeek = environment.InitializationString;
    let ListMaterialName = environment.InitializationString;
    let ListCategoryLocationName = environment.InitializationString;
    let ListQuantity = environment.InitializationString;
    let ListCreateUserCode = environment.InitializationString;
    let ListCreateUserName = environment.InitializationString;

    for (let i = 0; i < this.InventoryDetailService.ListFilter.length; i++) {
      ListWeek = ListWeek + this.InventoryDetailService.ListFilter[i].Week.toString() + ",";
      ListMaterialName = ListMaterialName + this.InventoryDetailService.ListFilter[i].MaterialName.toString() + ",";
      ListCategoryLocationName = ListCategoryLocationName + this.InventoryDetailService.ListFilter[i].CategoryLocationName.toString() + ",";
      ListQuantity = ListQuantity + this.InventoryDetailService.ListFilter[i].Quantity.toString() + ",";
      ListCreateUserCode = this.InventoryDetailService.ListFilter[0].CreateUserCode.toString() + ",";
      ListCreateUserName = this.InventoryDetailService.ListFilter[0].CreateUserName.toString() + ",";
    }
    this.PrintURL = this.PrintURL + "/" + ListWeek + "/" + ListMaterialName + "/" + ListCategoryLocationName + "/" + ListQuantity + "/" + ListCreateUserCode + "/" + ListCreateUserName;
    this.NotificationService.OpenWindowByURL2025(this.PrintURL);
  }
}