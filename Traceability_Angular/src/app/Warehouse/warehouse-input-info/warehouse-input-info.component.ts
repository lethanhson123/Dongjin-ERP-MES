import { Component, OnInit, ViewChild } from '@angular/core';
import { Router, NavigationEnd, ActivatedRoute } from '@angular/router';
import { MatDialog, MatDialogConfig, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { NotificationService } from 'src/app/shared/Notification.service';
import { DownloadService } from 'src/app/shared/Download.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

import { WarehouseInput } from 'src/app/shared/ERP/WarehouseInput.model';
import { WarehouseInputService } from 'src/app/shared/ERP/WarehouseInput.service';

import { WarehouseInputDetail } from 'src/app/shared/ERP/WarehouseInputDetail.model';
import { WarehouseInputDetailService } from 'src/app/shared/ERP/WarehouseInputDetail.service';

import { WarehouseInputDetailBarcode } from 'src/app/shared/ERP/WarehouseInputDetailBarcode.model';
import { WarehouseInputDetailBarcodeService } from 'src/app/shared/ERP/WarehouseInputDetailBarcode.service';


import { WarehouseInputDetailBarcodeMaterial } from 'src/app/shared/ERP/WarehouseInputDetailBarcodeMaterial.model';
import { WarehouseInputDetailBarcodeMaterialService } from 'src/app/shared/ERP/WarehouseInputDetailBarcodeMaterial.service';

import { WarehouseInputMaterial } from 'src/app/shared/ERP/WarehouseInputMaterial.model';
import { WarehouseInputMaterialService } from 'src/app/shared/ERP/WarehouseInputMaterial.service';

import { WarehouseInputDetailCount } from 'src/app/shared/ERP/WarehouseInputDetailCount.model';
import { WarehouseInputDetailCountService } from 'src/app/shared/ERP/WarehouseInputDetailCount.service';

import { WarehouseInputFile } from 'src/app/shared/ERP/WarehouseInputFile.model';
import { WarehouseInputFileService } from 'src/app/shared/ERP/WarehouseInputFile.service';

import { Material } from 'src/app/shared/ERP/Material.model';
import { MaterialService } from 'src/app/shared/ERP/Material.service';

import { InvoiceInput } from 'src/app/shared/ERP/InvoiceInput.model';
import { InvoiceInputService } from 'src/app/shared/ERP/InvoiceInput.service';


@Component({
  selector: 'app-warehouse-input-info',
  templateUrl: './warehouse-input-info.component.html',
  styleUrls: ['./warehouse-input-info.component.css']
})
export class WarehouseInputInfoComponent {

  @ViewChild('WarehouseInputDetailSort') WarehouseInputDetailSort: MatSort;
  @ViewChild('WarehouseInputDetailPaginator') WarehouseInputDetailPaginator: MatPaginator;

  @ViewChild('WarehouseInputDetailBarcodeSort') WarehouseInputDetailBarcodeSort: MatSort;
  @ViewChild('WarehouseInputDetailBarcodePaginator') WarehouseInputDetailBarcodePaginator: MatPaginator;

  @ViewChild('WarehouseInputDetailCountSort') WarehouseInputDetailCountSort: MatSort;
  @ViewChild('WarehouseInputDetailCountPaginator') WarehouseInputDetailCountPaginator: MatPaginator;

  @ViewChild('WarehouseInputMaterialSort') WarehouseInputMaterialSort: MatSort;
  @ViewChild('WarehouseInputMaterialPaginator') WarehouseInputMaterialPaginator: MatPaginator;

  @ViewChild('WarehouseInputFileSort') WarehouseInputFileSort: MatSort;
  @ViewChild('WarehouseInputFilePaginator') WarehouseInputFilePaginator: MatPaginator;

  IsWarehouseInputDetailBarcodeActiveAll: boolean = false;
  URLTemplate: string;

  constructor(
    public ActiveRouter: ActivatedRoute,
    public Router: Router,
    public Dialog: MatDialog,

    public NotificationService: NotificationService,
    public DownloadService: DownloadService,

    public WarehouseInputService: WarehouseInputService,
    public WarehouseInputDetailService: WarehouseInputDetailService,
    public WarehouseInputDetailBarcodeService: WarehouseInputDetailBarcodeService,
    public WarehouseInputDetailBarcodeMaterialService: WarehouseInputDetailBarcodeMaterialService,
    public WarehouseInputMaterialService: WarehouseInputMaterialService,
    public WarehouseInputDetailCountService: WarehouseInputDetailCountService,
    public WarehouseInputFileService: WarehouseInputFileService,


  ) {
    this.URLTemplate = this.WarehouseInputService.APIRootURL + "Download/WarehouseInputDetail.xlsx";
    this.WarehouseInputDetailService.BaseParameter.Active = false;
    this.WarehouseInputDetailCountService.BaseParameter.Active = true;

    this.WarehouseInputService.BaseParameter.BaseModel = {
      ID: environment.InitializationNumber,
      Date: null,
      Code: environment.InitializationString,
    };

    this.WarehouseInputDetailService.List = [];
    this.WarehouseInputDetailService.DataSource = new MatTableDataSource(this.WarehouseInputDetailService.List);
    this.WarehouseInputDetailService.DataSource.sort = this.WarehouseInputDetailSort;
    this.WarehouseInputDetailService.DataSource.paginator = this.WarehouseInputDetailPaginator;

    this.WarehouseInputDetailBarcodeService.List = [];
    this.WarehouseInputDetailBarcodeService.DataSource = new MatTableDataSource(this.WarehouseInputDetailBarcodeService.List);
    this.WarehouseInputDetailBarcodeService.DataSource.sort = this.WarehouseInputDetailBarcodeSort;
    this.WarehouseInputDetailBarcodeService.DataSource.paginator = this.WarehouseInputDetailBarcodePaginator;

    this.WarehouseInputDetailCountService.List = [];
    this.WarehouseInputDetailCountService.DataSource = new MatTableDataSource(this.WarehouseInputDetailCountService.List);
    this.WarehouseInputDetailCountService.DataSource.sort = this.WarehouseInputDetailCountSort;
    this.WarehouseInputDetailCountService.DataSource.paginator = this.WarehouseInputDetailCountPaginator;



    this.Router.events.forEach((event) => {
      if (event instanceof NavigationEnd) {
        this.WarehouseInputService.BaseParameter.ID = Number(this.ActiveRouter.snapshot.params.ID);
        this.WarehouseInputSearch();
      }
    });
  }

  ngOnInit(): void {

  }
  ngAfterViewInit() {

  }

  Date(value) {
    this.WarehouseInputService.BaseParameter.BaseModel.Date = new Date(value);
  }
  WarehouseInputFileNameChange(files: FileList) {
    if (files) {
      this.WarehouseInputService.FileToUpload = files;
      this.WarehouseInputService.BaseParameter.Event = event;
    }
  }
  WarehouseInputSearch() {
    this.WarehouseInputService.IsShowLoading = true;
    this.WarehouseInputService.GetByIDAsync().subscribe(
      res => {
        this.WarehouseInputService.BaseParameter.BaseModel = (res as BaseResult).BaseModel;

        this.WarehouseInputDetailSearch();
        this.WarehouseInputDetailBarcodeSearch();
        this.WarehouseInputDetailCountSearch();
        this.WarehouseInputFileSearch();
      },
      err => {
      },
      () => {
        this.WarehouseInputService.IsShowLoading = false;
      }
    );
  }
  Add(ID: number) {
    this.Router.navigateByUrl("WarehouseInputInfo/" + ID);
    this.WarehouseInputService.BaseParameter.ID = ID;
    this.WarehouseInputSearch();
  }
  Save() {
    this.WarehouseInputService.IsShowLoading = true;
    this.WarehouseInputService.SaveAndUploadFileAsync().subscribe(
      res => {
        this.WarehouseInputService.BaseParameter.BaseModel = (res as BaseResult).BaseModel;
        this.WarehouseInputService.FileToUpload = null;
        this.WarehouseInputService.BaseParameter.Event = null;
        this.Add(this.WarehouseInputService.BaseParameter.BaseModel.ID);
        this.NotificationService.warn(environment.SaveSuccess);
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.WarehouseInputService.IsShowLoading = false;
      }
    );
  }
  WarehouseInputDetailSearch() {
    if (this.WarehouseInputService.BaseParameter.BaseModel.ID > 0) {
      if (this.WarehouseInputDetailService.BaseParameter.SearchString.length > 0) {
        this.WarehouseInputDetailService.BaseParameter.SearchString = this.WarehouseInputDetailService.BaseParameter.SearchString.trim();
        // if (this.WarehouseInputDetailService.DataSource) {
        //   this.WarehouseInputDetailService.DataSource.filter = this.WarehouseInputDetailService.BaseParameter.SearchString.toLowerCase();
        // }
        let SearchString = this.WarehouseInputDetailService.BaseParameter.SearchString;
        this.WarehouseInputDetailService.ListYear = this.WarehouseInputDetailService.List.filter(o => o.ID > 0 && ((o.MaterialName && o.MaterialName.length > 0 && o.MaterialName.includes(SearchString)) || (o.Description && o.Description.length > 0 && o.Description.includes(SearchString))));
        this.WarehouseInputDetailService.DataSource = new MatTableDataSource(this.WarehouseInputDetailService.ListYear);
        this.WarehouseInputDetailService.DataSource.sort = this.WarehouseInputDetailSort;
        this.WarehouseInputDetailService.DataSource.paginator = this.WarehouseInputDetailPaginator;

      }
      else {
        this.WarehouseInputDetailService.BaseParameter.ParentID = this.WarehouseInputService.BaseParameter.BaseModel.ID;
        this.WarehouseInputService.IsShowLoading = true;
        this.WarehouseInputDetailService.GetByParentIDToListAsync().subscribe(
          res => {
            this.WarehouseInputDetailService.List = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
            this.WarehouseInputDetailService.ListYear = this.WarehouseInputDetailService.List;
            this.WarehouseInputDetailService.DataSource = new MatTableDataSource(this.WarehouseInputDetailService.ListYear);
            this.WarehouseInputDetailService.DataSource.sort = this.WarehouseInputDetailSort;
            this.WarehouseInputDetailService.DataSource.paginator = this.WarehouseInputDetailPaginator;
          },
          err => {
          },
          () => {
            this.WarehouseInputService.IsShowLoading = false;
          }
        );
      }
    }
    else {
      this.WarehouseInputDetailService.List = [];
      this.WarehouseInputDetailService.DataSource = new MatTableDataSource(this.WarehouseInputDetailService.List);
      this.WarehouseInputDetailService.DataSource.sort = this.WarehouseInputDetailSort;
      this.WarehouseInputDetailService.DataSource.paginator = this.WarehouseInputDetailPaginator;
    }
  }
  WarehouseInputDetailActiveChange(element: WarehouseInputDetail) {
    if (element.ID > 0) {
      if (element.Active == true) {
        this.WarehouseInputDetailBarcodeService.BaseParameter.ListID.push(element.ID);
        this.WarehouseInputDetailBarcodeService.BaseParameter.SearchString = element.MaterialName;
        this.WarehouseInputDetailBarcodeSearch();
      }
      else {
        this.WarehouseInputDetailBarcodeService.BaseParameter.SearchString = environment.InitializationString;
        this.WarehouseInputDetailBarcodeService.BaseParameter.ListID = this.WarehouseInputDetailBarcodeService.BaseParameter.ListID.filter(o => o != element.ID);
      }
    }
  }
  WarehouseInputDetailActiveAllChange() {
    this.WarehouseInputDetailService.BaseParameter.ListID = [];
    for (let i = 0; i < this.WarehouseInputDetailService.List.length; i++) {
      if (this.WarehouseInputDetailService.List[i].ID > 0) {
        this.WarehouseInputDetailService.List[i].Active = this.WarehouseInputDetailService.BaseParameter.Active;
        if (this.WarehouseInputDetailService.List[i].Active == true) {
          this.WarehouseInputDetailBarcodeService.BaseParameter.ListID.push(this.WarehouseInputDetailService.List[i].ID);
        }
      }
    }
  }
  WarehouseInputDetailSync() {
    for (let i = 0; i < this.WarehouseInputDetailService.List.length; i++) {
      if (this.WarehouseInputDetailService.List[i].ID > 0) {
        this.WarehouseInputDetailService.List[i].Active = true;
      }
    }
    this.WarehouseInputService.IsShowLoading = true;
    this.WarehouseInputDetailService.BaseParameter.List = this.WarehouseInputDetailService.List;
    this.WarehouseInputDetailService.SaveListAndSyncWarehouseInputDetailBarcodeAsync().subscribe(
      res => {
        this.WarehouseInputDetailBarcodeSearch();
        this.NotificationService.warn(environment.SaveSuccess);
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.WarehouseInputService.IsShowLoading = false;
      }
    );
  }
  WarehouseInputDetailDelete(element: WarehouseInputDetail) {
    this.WarehouseInputDetailService.BaseParameter.ID = element.ID;
    if (confirm(environment.DeleteConfirm)) {
      this.WarehouseInputService.IsShowLoading = true;
      this.WarehouseInputDetailService.RemoveAsync().subscribe(
        res => {
          this.WarehouseInputDetailSearch();
          this.NotificationService.warn(environment.DeleteSuccess);
        },
        err => {
          this.NotificationService.warn(environment.DeleteNotSuccess);
        },
        () => {
          this.WarehouseInputService.IsShowLoading = false;
        }
      );
    }
  }
  WarehouseInputDetailSave(element: WarehouseInputDetail) {
    this.WarehouseInputService.IsShowLoading = true;
    element.ParentID = this.WarehouseInputService.BaseParameter.BaseModel.ID;
    this.WarehouseInputDetailService.BaseParameter.BaseModel = element;
    this.WarehouseInputDetailService.SaveAsync().subscribe(
      res => {
        this.WarehouseInputDetailSearch();
        this.WarehouseInputDetailBarcodeSearch();
        this.NotificationService.warn(environment.SaveSuccess);
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.WarehouseInputService.IsShowLoading = false;
      }
    );
  }

  WarehouseInputDetailBarcodeSearch() {
    if (this.WarehouseInputService.BaseParameter.BaseModel.ID > 0) {
      if (this.WarehouseInputDetailBarcodeService.BaseParameter.SearchString.length > 0) {
        this.WarehouseInputDetailBarcodeService.BaseParameter.SearchString = this.WarehouseInputDetailBarcodeService.BaseParameter.SearchString.trim();
        if (this.WarehouseInputDetailBarcodeService.DataSource) {
          this.WarehouseInputDetailBarcodeService.DataSource.filter = this.WarehouseInputDetailBarcodeService.BaseParameter.SearchString.toLowerCase();
        }
      }
      else {
        this.WarehouseInputDetailBarcodeService.BaseParameter.ParentID = this.WarehouseInputService.BaseParameter.BaseModel.ID;
        this.WarehouseInputService.IsShowLoading = true;
        this.WarehouseInputDetailBarcodeService.GetByParentIDToListAsync().subscribe(
          res => {
            this.WarehouseInputDetailBarcodeService.List = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
            this.WarehouseInputDetailBarcodeService.DataSource = new MatTableDataSource(this.WarehouseInputDetailBarcodeService.List);
            this.WarehouseInputDetailBarcodeService.DataSource.sort = this.WarehouseInputDetailBarcodeSort;
            this.WarehouseInputDetailBarcodeService.DataSource.paginator = this.WarehouseInputDetailBarcodePaginator;
          },
          err => {
          },
          () => {
            this.WarehouseInputService.IsShowLoading = false;
          }
        );
      }
    }
    else {
      this.WarehouseInputDetailBarcodeService.List = [];
      this.WarehouseInputDetailBarcodeService.DataSource = new MatTableDataSource(this.WarehouseInputDetailBarcodeService.List);
      this.WarehouseInputDetailBarcodeService.DataSource.sort = this.WarehouseInputDetailBarcodeSort;
      this.WarehouseInputDetailBarcodeService.DataSource.paginator = this.WarehouseInputDetailBarcodePaginator;
    }
  }
  WarehouseInputDetailBarcodeDelete(element: WarehouseInputDetailBarcode) {
    this.WarehouseInputDetailBarcodeService.BaseParameter.ID = element.ID;
    if (confirm(environment.DeleteConfirm)) {
      this.WarehouseInputService.IsShowLoading = true;
      this.WarehouseInputDetailBarcodeService.RemoveAsync().subscribe(
        res => {
          this.WarehouseInputDetailBarcodeSearch();
          this.NotificationService.warn(environment.DeleteSuccess);
        },
        err => {
          this.NotificationService.warn(environment.DeleteNotSuccess);
        },
        () => {
          this.WarehouseInputService.IsShowLoading = false;
        }
      );
    }
  }
  WarehouseInputDetailBarcodeSave(element: WarehouseInputDetailBarcode) {
    this.WarehouseInputService.IsShowLoading = true;
    element.ParentID = this.WarehouseInputService.BaseParameter.BaseModel.ID;
    this.WarehouseInputDetailBarcodeService.BaseParameter.BaseModel = element;
    this.WarehouseInputDetailBarcodeService.SaveAsync().subscribe(
      res => {
        this.WarehouseInputDetailBarcodeSearch();
        this.NotificationService.warn(environment.SaveSuccess);
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.WarehouseInputService.IsShowLoading = false;
      }
    );
  }
  WarehouseInputDetailBarcodePrintByParentID() {
    this.WarehouseInputService.IsShowLoading = true;
    this.WarehouseInputDetailBarcodeService.PrintByParentIDAsync().subscribe(
      res => {
        this.WarehouseInputDetailBarcodeService.BaseResult = (res as BaseResult);
        this.NotificationService.OpenWindowByURL(this.WarehouseInputDetailBarcodeService.BaseResult.Message);
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.WarehouseInputService.IsShowLoading = false;
      }
    );
  }
  WarehouseInputDetailBarcodePrintByListWarehouseInputDetailID() {
    this.WarehouseInputService.IsShowLoading = true;
    this.WarehouseInputDetailBarcodeService.PrintByListWarehouseInputDetailIDAsync().subscribe(
      res => {
        this.WarehouseInputDetailBarcodeService.BaseResult = (res as BaseResult);
        this.NotificationService.OpenWindowByURL(this.WarehouseInputDetailBarcodeService.BaseResult.Message);
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.WarehouseInputService.IsShowLoading = false;
      }
    );
  }
  WarehouseInputDetailPrint(element: WarehouseInputDetail) {
    this.WarehouseInputDetailBarcodeService.BaseParameter.ID = element.ID;
    this.WarehouseInputService.IsShowLoading = true;
    this.WarehouseInputDetailBarcodeService.PrintByWarehouseInputDetailIDAsync().subscribe(
      res => {
        this.WarehouseInputDetailBarcodeService.BaseResult = (res as BaseResult);
        this.NotificationService.OpenWindowByURL(this.WarehouseInputDetailBarcodeService.BaseResult.Message);
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.WarehouseInputService.IsShowLoading = false;
      }
    );
  }
  WarehouseInputDetailSearchPrint() {
    this.WarehouseInputService.IsShowLoading = true;
    this.WarehouseInputDetailBarcodeService.BaseParameter.ListID = [];
    for (let i = 0; i < this.WarehouseInputDetailService.ListYear.length; i++) {
      this.WarehouseInputDetailBarcodeService.BaseParameter.ListID.push(this.WarehouseInputDetailService.ListYear[i].ID);
    }
    this.WarehouseInputDetailBarcodeService.PrintByListWarehouseInputDetailID2025Async().subscribe(
      res => {
        this.WarehouseInputDetailBarcodeService.BaseResult = (res as BaseResult);
        this.NotificationService.OpenWindowByURL(this.WarehouseInputDetailBarcodeService.BaseResult.Message);
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.WarehouseInputService.IsShowLoading = false;
      }
    );
  }
  WarehouseInputDetailBarcodePrint(element: WarehouseInputDetailBarcode) {
    this.WarehouseInputService.IsShowLoading = true;
    this.WarehouseInputDetailBarcodeService.BaseParameter.ID = element.ID;
    this.WarehouseInputDetailBarcodeService.PrintAsync().subscribe(
      res => {
        this.WarehouseInputDetailBarcodeService.BaseResult = (res as BaseResult);
        this.NotificationService.OpenWindowByURL(this.WarehouseInputDetailBarcodeService.BaseResult.Message);
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.WarehouseInputService.IsShowLoading = false;
      }
    );
  }
  IsWarehouseInputDetailBarcodeActiveAllChange() {
    for (let i = 0; i < this.WarehouseInputDetailBarcodeService.List.length; i++) {
      this.WarehouseInputDetailBarcodeService.List[i].Active = this.IsWarehouseInputDetailBarcodeActiveAll;
    }
  }
  WarehouseInputDetailBarcodeSaveAll() {
    this.WarehouseInputService.IsShowLoading = true;
    for (let i = 0; i < this.WarehouseInputDetailBarcodeService.List.length; i++) {
      this.WarehouseInputDetailBarcodeService.List[i].Active = this.IsWarehouseInputDetailBarcodeActiveAll;
    }
    this.WarehouseInputDetailBarcodeService.BaseParameter.List = this.WarehouseInputDetailBarcodeService.List;
    this.WarehouseInputDetailBarcodeService.SaveListAsync().subscribe(
      res => {
        this.NotificationService.warn(environment.SaveSuccess);
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.WarehouseInputService.IsShowLoading = false;
      }
    );
  }
  WarehouseInputDetailCountSearch() {
    if (this.WarehouseInputService.BaseParameter.BaseModel.ID > 0) {
      if (this.WarehouseInputDetailCountService.BaseParameter.SearchString.length > 0) {
        this.WarehouseInputDetailCountService.BaseParameter.SearchString = this.WarehouseInputDetailCountService.BaseParameter.SearchString.trim();
        if (this.WarehouseInputDetailCountService.DataSource) {
          this.WarehouseInputDetailCountService.DataSource.filter = this.WarehouseInputDetailCountService.BaseParameter.SearchString.toLowerCase();
        }
      }
      else {
        this.WarehouseInputDetailCountService.BaseParameter.ParentID = this.WarehouseInputService.BaseParameter.BaseModel.ID;
        this.WarehouseInputService.IsShowLoading = true;
        this.WarehouseInputDetailCountService.GetByParentIDAndEmptyToListAsync().subscribe(
          res => {
            this.WarehouseInputDetailCountService.List = (res as BaseResult).List.sort((a, b) => (a.Date > b.Date ? 1 : -1));


            if (this.WarehouseInputDetailCountService.BaseParameter.Active == true) {
              this.WarehouseInputDetailCountService.List = this.WarehouseInputDetailCountService.List.filter(o => o.Active == true);
            }
            this.WarehouseInputDetailCountService.DataSource = new MatTableDataSource(this.WarehouseInputDetailCountService.List);
            this.WarehouseInputDetailCountService.DataSource.sort = this.WarehouseInputDetailCountSort;
            this.WarehouseInputDetailCountService.DataSource.paginator = this.WarehouseInputDetailCountPaginator;
          },
          err => {
          },
          () => {
            this.WarehouseInputService.IsShowLoading = false;
          }
        );
      }
    }
    else {
      this.WarehouseInputDetailCountService.List = [];
      this.WarehouseInputDetailCountService.DataSource = new MatTableDataSource(this.WarehouseInputDetailCountService.List);
      this.WarehouseInputDetailCountService.DataSource.sort = this.WarehouseInputDetailCountSort;
      this.WarehouseInputDetailCountService.DataSource.paginator = this.WarehouseInputDetailCountPaginator;
    }
  }
  WarehouseInputDetailCountDelete(element: WarehouseInputDetailCount) {
    this.WarehouseInputDetailCountService.BaseParameter.ID = element.ID;
    if (confirm(environment.DeleteConfirm)) {
      this.WarehouseInputService.IsShowLoading = true;
      this.WarehouseInputDetailCountService.RemoveAsync().subscribe(
        res => {
          this.WarehouseInputDetailCountSearch();
          this.NotificationService.warn(environment.DeleteSuccess);
        },
        err => {
          this.NotificationService.warn(environment.DeleteNotSuccess);
        },
        () => {
          this.WarehouseInputService.IsShowLoading = false;
        }
      );
    }
  }
  WarehouseInputDetailCountSave(element: WarehouseInputDetailCount) {
    this.WarehouseInputService.IsShowLoading = true;
    element.ParentID = this.WarehouseInputService.BaseParameter.BaseModel.ID;
    this.WarehouseInputDetailCountService.BaseParameter.BaseModel = element;
    this.WarehouseInputDetailCountService.SaveAsync().subscribe(
      res => {
        this.WarehouseInputDetailCountSearch();
        this.NotificationService.warn(environment.SaveSuccess);
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.WarehouseInputService.IsShowLoading = false;
      }
    );
  }
  WarehouseInputMaterialSearch() {
    if (this.WarehouseInputService.BaseParameter.BaseModel.ID > 0) {
      if (this.WarehouseInputMaterialService.BaseParameter.SearchString.length > 0) {
        this.WarehouseInputMaterialService.BaseParameter.SearchString = this.WarehouseInputMaterialService.BaseParameter.SearchString.trim();
        if (this.WarehouseInputMaterialService.DataSource) {
          this.WarehouseInputMaterialService.DataSource.filter = this.WarehouseInputMaterialService.BaseParameter.SearchString.toLowerCase();
        }
      }
      else {
        this.WarehouseInputMaterialService.BaseParameter.ParentID = this.WarehouseInputService.BaseParameter.BaseModel.ID;
        this.WarehouseInputService.IsShowLoading = true;
        this.WarehouseInputMaterialService.GetByParentIDAndEmptyToListAsync().subscribe(
          res => {
            this.WarehouseInputMaterialService.List = (res as BaseResult).List.sort((a, b) => (a.Date < b.Date ? 1 : -1));
            this.WarehouseInputMaterialService.DataSource = new MatTableDataSource(this.WarehouseInputMaterialService.List);
            this.WarehouseInputMaterialService.DataSource.sort = this.WarehouseInputMaterialSort;
            this.WarehouseInputMaterialService.DataSource.paginator = this.WarehouseInputMaterialPaginator;
          },
          err => {
          },
          () => {
            this.WarehouseInputService.IsShowLoading = false;
          }
        );
      }
    }
    else {
      this.WarehouseInputMaterialService.List = [];
      this.WarehouseInputMaterialService.DataSource = new MatTableDataSource(this.WarehouseInputMaterialService.List);
      this.WarehouseInputMaterialService.DataSource.sort = this.WarehouseInputMaterialSort;
      this.WarehouseInputMaterialService.DataSource.paginator = this.WarehouseInputMaterialPaginator;
    }
  }
  WarehouseInputMaterialDelete(element: WarehouseInputMaterial) {
    this.WarehouseInputDetailCountService.BaseParameter.ID = element.ID;
    if (confirm(environment.DeleteConfirm)) {
      this.WarehouseInputService.IsShowLoading = true;
      this.WarehouseInputMaterialService.RemoveAsync().subscribe(
        res => {
          this.WarehouseInputMaterialSearch();
          this.NotificationService.warn(environment.DeleteSuccess);
        },
        err => {
          this.NotificationService.warn(environment.DeleteNotSuccess);
        },
        () => {
          this.WarehouseInputService.IsShowLoading = false;
        }
      );
    }
  }
  WarehouseInputMaterialSave(element: WarehouseInputMaterial) {
    this.WarehouseInputService.IsShowLoading = true;
    element.ParentID = this.WarehouseInputService.BaseParameter.BaseModel.ID;
    this.WarehouseInputMaterialService.BaseParameter.BaseModel = element;
    this.WarehouseInputMaterialService.SaveAsync().subscribe(
      res => {
        this.WarehouseInputMaterialSearch();
        this.NotificationService.warn(environment.SaveSuccess);
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.WarehouseInputService.IsShowLoading = false;
      }
    );
  }
  WarehouseInputFileChange(event, files: FileList) {
    if (files) {
      this.WarehouseInputFileService.FileToUpload = files;
      this.WarehouseInputFileService.BaseParameter.Event = event;
    }
  }
  WarehouseInputFileSearch() {
    if (this.WarehouseInputService.BaseParameter.BaseModel.ID > 0) {
      if (this.WarehouseInputFileService.BaseParameter.SearchString.length > 0) {
        this.WarehouseInputFileService.BaseParameter.SearchString = this.WarehouseInputFileService.BaseParameter.SearchString.trim();
        if (this.WarehouseInputFileService.DataSource) {
          this.WarehouseInputFileService.DataSource.filter = this.WarehouseInputFileService.BaseParameter.SearchString.toLowerCase();
        }
      }
      else {
        this.WarehouseInputFileService.BaseParameter.ParentID = this.WarehouseInputService.BaseParameter.BaseModel.ID;
        this.WarehouseInputService.IsShowLoading = true;
        this.WarehouseInputFileService.GetByParentIDToListAsync().subscribe(
          res => {
            this.WarehouseInputFileService.List = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
            this.WarehouseInputFileService.DataSource = new MatTableDataSource(this.WarehouseInputFileService.List);
            this.WarehouseInputFileService.DataSource.sort = this.WarehouseInputFileSort;
            this.WarehouseInputFileService.DataSource.paginator = this.WarehouseInputFilePaginator;
          },
          err => {
          },
          () => {
            this.WarehouseInputService.IsShowLoading = false;
          }
        );
      }
    }
    else {
      this.WarehouseInputFileService.List = [];
      this.WarehouseInputFileService.DataSource = new MatTableDataSource(this.WarehouseInputFileService.List);
      this.WarehouseInputFileService.DataSource.sort = this.WarehouseInputFileSort;
      this.WarehouseInputFileService.DataSource.paginator = this.WarehouseInputFilePaginator;
    }
  }
  WarehouseInputFileDelete(element: WarehouseInputFile) {
    this.WarehouseInputFileService.BaseParameter.ID = element.ID;
    if (confirm(environment.DeleteConfirm)) {
      this.WarehouseInputService.IsShowLoading = true;
      this.WarehouseInputFileService.RemoveAsync().subscribe(
        res => {
          this.WarehouseInputFileSearch();
          this.NotificationService.warn(environment.DeleteSuccess);
        },
        err => {
          this.NotificationService.warn(environment.DeleteNotSuccess);
        },
        () => {
          this.WarehouseInputService.IsShowLoading = false;
        }
      );
    }
  }
  WarehouseInputFileSave(element: WarehouseInputFile) {
    this.WarehouseInputService.IsShowLoading = true;
    element.ParentID = this.WarehouseInputService.BaseParameter.BaseModel.ID;
    this.WarehouseInputFileService.BaseParameter.BaseModel = element;
    this.WarehouseInputFileService.SaveAndUploadFilesAsync().subscribe(
      res => {
        this.WarehouseInputFileSearch();
        this.NotificationService.warn(environment.SaveSuccess);
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.WarehouseInputService.IsShowLoading = false;
      }
    );
  }
}
