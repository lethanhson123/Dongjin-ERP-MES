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

import { InvoiceInput } from 'src/app/shared/ERP/InvoiceInput.model';
import { InvoiceInputService } from 'src/app/shared/ERP/InvoiceInput.service';

import { InvoiceInputDetail } from 'src/app/shared/ERP/InvoiceInputDetail.model';
import { InvoiceInputDetailService } from 'src/app/shared/ERP/InvoiceInputDetail.service';

import { Company } from 'src/app/shared/ERP/Company.model';
import { CompanyService } from 'src/app/shared/ERP/Company.service';

import { CategoryUnit } from 'src/app/shared/ERP/CategoryUnit.model';
import { CategoryUnitService } from 'src/app/shared/ERP/CategoryUnit.service';

import { Material } from 'src/app/shared/ERP/Material.model';
import { MaterialService } from 'src/app/shared/ERP/Material.service';
import { MaterialModalComponent } from 'src/app/PC/material-modal/material-modal.component';

import { InvoiceInputFile } from 'src/app/shared/ERP/InvoiceInputFile.model';
import { InvoiceInputFileService } from 'src/app/shared/ERP/InvoiceInputFile.service';

import { ProductionOrder } from 'src/app/shared/ERP/ProductionOrder.model';
import { ProductionOrderService } from 'src/app/shared/ERP/ProductionOrder.service';

@Component({
  selector: 'app-invoice-input-info',
  templateUrl: './invoice-input-info.component.html',
  styleUrls: ['./invoice-input-info.component.css']
})
export class InvoiceInputInfoComponent {

  @ViewChild('InvoiceInputDetailSort') InvoiceInputDetailSort: MatSort;
  @ViewChild('InvoiceInputDetailPaginator') InvoiceInputDetailPaginator: MatPaginator;

  @ViewChild('InvoiceInputFileSort') InvoiceInputFileSort: MatSort;
  @ViewChild('InvoiceInputFilePaginator') InvoiceInputFilePaginator: MatPaginator;

  URLTemplate: string;


  constructor(
    public ActiveRouter: ActivatedRoute,
    public Router: Router,
    public Dialog: MatDialog,
    public NotificationService: NotificationService,
    public DownloadService: DownloadService,

    public InvoiceInputService: InvoiceInputService,
    public InvoiceInputDetailService: InvoiceInputDetailService,
    public InvoiceInputFileService: InvoiceInputFileService,

    public CompanyService: CompanyService,
    public MaterialService: MaterialService,
    public CategoryUnitService: CategoryUnitService,
    public ProductionOrderService: ProductionOrderService,
  ) {
    this.URLTemplate = this.InvoiceInputService.APIRootURL + "Download/InvoiceInput.xlsx";

    this.InvoiceInputService.BaseParameter.BaseModel = {
    };

    this.InvoiceInputDetailService.List = [];
    this.InvoiceInputDetailService.DataSource = new MatTableDataSource(this.InvoiceInputDetailService.List);
    this.InvoiceInputDetailService.DataSource.sort = this.InvoiceInputDetailSort;
    this.InvoiceInputDetailService.DataSource.paginator = this.InvoiceInputDetailPaginator;

    this.InvoiceInputFileService.List = [];
    this.InvoiceInputFileService.DataSource = new MatTableDataSource(this.InvoiceInputFileService.List);
    this.InvoiceInputFileService.DataSource.sort = this.InvoiceInputFileSort;
    this.InvoiceInputFileService.DataSource.paginator = this.InvoiceInputFilePaginator;

    //this.ProductionOrderSearch();
    this.CompanySearch();
    //this.MaterialSearch();
    this.CategoryUnitSearch();

    this.Router.events.forEach((event) => {
      if (event instanceof NavigationEnd) {
        this.InvoiceInputService.BaseParameter.ID = Number(this.ActiveRouter.snapshot.params.ID);
        this.InvoiceInputSearch();
      }
    });
  }

  ngOnInit(): void {

  }
  ngAfterViewInit() {
  }
  ProductionOrderSearch() {
    this.ProductionOrderService.ComponentGetByMembershipID_Active_IsCompleteToListAsync(this.InvoiceInputService);
  }
  CategoryUnitSearch() {
    this.CategoryUnitService.BaseParameter.Active = true;
    this.CategoryUnitService.ComponentGetByActiveToListAsync(this.InvoiceInputService);
  }
  MaterialSearch() {
    this.MaterialService.BaseParameter.Active = true;
    this.MaterialService.ComponentGetByActiveToListAsync(this.InvoiceInputService);
  }
  MaterialFilter(searchString: string) {
    this.MaterialService.Filter(searchString);
  }
  CompanySearch() {
    this.CompanyService.ComponentGetByMembershipID_ActiveToListAsync(this.InvoiceInputService);
  }
  DateETD(value) {
    this.InvoiceInputService.BaseParameter.BaseModel.DateETD = new Date(value);
  }
  DateETA(value) {
    this.InvoiceInputService.BaseParameter.BaseModel.DateETA = new Date(value);
  }
  InvoiceInputChange(event, files: FileList) {
    if (files) {
      this.InvoiceInputService.FileToUpload = files;
      this.InvoiceInputService.BaseParameter.Event = event;
    }
  }
  InvoiceInputFileChange(event, files: FileList) {
    if (files) {
      this.InvoiceInputFileService.FileToUpload = files;
      this.InvoiceInputFileService.BaseParameter.Event = event;
    }
  }
  InvoiceInputSearch() {
    this.InvoiceInputService.IsShowLoading = true;
    this.InvoiceInputService.GetByIDAsync().subscribe(
      res => {
        this.InvoiceInputService.BaseParameter.BaseModel = (res as BaseResult).BaseModel;
        this.InvoiceInputDetailSearch(0);
        this.InvoiceInputFileSearch();
      },
      err => {
      },
      () => {
        this.InvoiceInputService.IsShowLoading = false;
      }
    );
  }
  Add(ID: number) {
    this.Router.navigateByUrl("InvoiceInputInfo/" + ID);
    this.InvoiceInputService.BaseParameter.ID = ID;
    this.InvoiceInputSearch();
  }
  Save() {
    this.InvoiceInputService.IsShowLoading = true;
    this.InvoiceInputService.SaveAndUploadFilesAsync().subscribe(
      res => {
        this.InvoiceInputService.BaseParameter.BaseModel = (res as BaseResult).BaseModel;
        this.InvoiceInputService.FileToUpload = null;
        this.InvoiceInputService.BaseParameter.Event = null;
        this.Add(this.InvoiceInputService.BaseParameter.BaseModel.ID);
        this.NotificationService.warn(environment.SaveSuccess);
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.InvoiceInputService.IsShowLoading = false;
      }
    );
  }
  InvoiceInputDetailSearch(Action: number) {
    if (Action > 0) {
      this.InvoiceInputService.IsShowLoading = true;
      this.InvoiceInputService.BaseParameter.ID = this.InvoiceInputService.BaseParameter.BaseModel.ID;
      this.InvoiceInputService.GetByIDAsync().subscribe(
        res => {
          this.InvoiceInputService.BaseParameter.BaseModel = (res as BaseResult).BaseModel;
          this.InvoiceInputDetailSearchSub();
        },
        err => {
        },
        () => {
          this.InvoiceInputService.IsShowLoading = false;
        }
      );
    }
    else {
      this.InvoiceInputDetailSearchSub();
    }
  }
  InvoiceInputDetailSearchSub() {
    this.InvoiceInputDetailService.List = [];
    if (this.InvoiceInputService.BaseParameter.BaseModel.ID > 0) {
      if (this.InvoiceInputDetailService.BaseParameter.SearchString.length > 0) {
        this.InvoiceInputDetailService.BaseParameter.SearchString = this.InvoiceInputDetailService.BaseParameter.SearchString.trim();
        if (this.InvoiceInputDetailService.DataSource) {
          this.InvoiceInputDetailService.DataSource.filter = this.InvoiceInputDetailService.BaseParameter.SearchString.toLowerCase();
        }
      }
      else {
        this.InvoiceInputDetailService.BaseParameter.ParentID = this.InvoiceInputService.BaseParameter.BaseModel.ID;
        this.InvoiceInputService.IsShowLoading = true;
        this.InvoiceInputDetailService.GetByParentIDAndEmptyToListAsync().subscribe(
          res => {
            this.InvoiceInputDetailService.List = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
            this.InvoiceInputDetailService.DataSource = new MatTableDataSource(this.InvoiceInputDetailService.List);
            this.InvoiceInputDetailService.DataSource.sort = this.InvoiceInputDetailSort;
            this.InvoiceInputDetailService.DataSource.paginator = this.InvoiceInputDetailPaginator;

            this.InvoiceInputService.BaseParameter.BaseModel.Total = environment.InitializationNumber;
            this.InvoiceInputService.BaseParameter.BaseModel.TotalQuantity = environment.InitializationNumber;
            this.InvoiceInputService.BaseParameter.BaseModel.TotalInvoice = environment.InitializationNumber;
            for (let i = 0; i < this.InvoiceInputDetailService.List.length; i++) {
              this.InvoiceInputService.BaseParameter.BaseModel.TotalQuantity = this.InvoiceInputService.BaseParameter.BaseModel.TotalQuantity + this.InvoiceInputDetailService.List[i].Quantity;
              this.InvoiceInputService.BaseParameter.BaseModel.TotalInvoice = this.InvoiceInputService.BaseParameter.BaseModel.TotalInvoice + this.InvoiceInputDetailService.List[i].QuantityInvoice;
              this.InvoiceInputService.BaseParameter.BaseModel.Total = this.InvoiceInputService.BaseParameter.BaseModel.Total + this.InvoiceInputDetailService.List[i].Total;
            }
          },
          err => {
          },
          () => {
            this.InvoiceInputService.IsShowLoading = false;
          }
        );
      }
    }
    else {
      this.InvoiceInputDetailService.List = [];
      this.InvoiceInputDetailService.DataSource = new MatTableDataSource(this.InvoiceInputDetailService.List);
      this.InvoiceInputDetailService.DataSource.sort = this.InvoiceInputDetailSort;
      this.InvoiceInputDetailService.DataSource.paginator = this.InvoiceInputDetailPaginator;
    }
  }
  InvoiceInputDetailDelete(element: InvoiceInputDetail) {
    this.InvoiceInputDetailService.BaseParameter.ID = element.ID;
    if (confirm(environment.DeleteConfirm)) {
      this.InvoiceInputService.IsShowLoading = true;
      this.InvoiceInputDetailService.RemoveAsync().subscribe(
        res => {
          this.InvoiceInputDetailSearch(2);
          this.NotificationService.warn(environment.DeleteSuccess);
        },
        err => {
          this.NotificationService.warn(environment.DeleteNotSuccess);
        },
        () => {
          this.InvoiceInputService.IsShowLoading = false;
        }
      );
    }
  }
  InvoiceInputDetailSave(element: InvoiceInputDetail) {
    this.InvoiceInputService.IsShowLoading = true;
    element.ParentID = this.InvoiceInputService.BaseParameter.BaseModel.ID;
    this.InvoiceInputDetailService.BaseParameter.BaseModel = element;
    this.InvoiceInputDetailService.SaveAsync().subscribe(
      res => {
        this.InvoiceInputDetailSearch(1);
        this.NotificationService.warn(environment.SaveSuccess);
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.InvoiceInputService.IsShowLoading = false;
      }
    );
  }
  MaterialModalOpen(element: Material) {
    this.MaterialService.BaseParameter.BaseModel = element;
    const dialogConfig = new MatDialogConfig();
    dialogConfig.disableClose = true;
    dialogConfig.autoFocus = true;
    dialogConfig.width = environment.DialogConfigWidth;
    const dialog = this.Dialog.open(MaterialModalComponent, dialogConfig);
    dialog.afterClosed().subscribe(() => {

    });
  }
  MaterialModal(element: InvoiceInputDetail) {    
    this.InvoiceInputService.IsShowLoading = true;
    this.MaterialService.BaseParameter.ID = element.MaterialID;
    this.MaterialService.GetByIDAsync().subscribe(
      res => {
        this.MaterialService.BaseParameter.BaseModel = (res as BaseResult).BaseModel;
        console.log(this.MaterialService.BaseParameter.BaseModel);
        this.MaterialModalOpen(this.MaterialService.BaseParameter.BaseModel);
      },
      err => {
      },
      () => {
        this.InvoiceInputService.IsShowLoading = false;
      }
    );
  }
  InvoiceInputFileSearch() {
    if (this.InvoiceInputService.BaseParameter.BaseModel.ID > 0) {
      if (this.InvoiceInputFileService.BaseParameter.SearchString.length > 0) {
        this.InvoiceInputFileService.BaseParameter.SearchString = this.InvoiceInputFileService.BaseParameter.SearchString.trim();
        if (this.InvoiceInputFileService.DataSource) {
          this.InvoiceInputFileService.DataSource.filter = this.InvoiceInputFileService.BaseParameter.SearchString.toLowerCase();
        }
      }
      else {
        this.InvoiceInputFileService.BaseParameter.ParentID = this.InvoiceInputService.BaseParameter.BaseModel.ID;
        this.InvoiceInputService.IsShowLoading = true;
        this.InvoiceInputFileService.GetByParentIDAndEmptyToListAsync().subscribe(
          res => {
            this.InvoiceInputFileService.List = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
            this.InvoiceInputFileService.DataSource = new MatTableDataSource(this.InvoiceInputFileService.List);
            this.InvoiceInputFileService.DataSource.sort = this.InvoiceInputFileSort;
            this.InvoiceInputFileService.DataSource.paginator = this.InvoiceInputFilePaginator;
          },
          err => {
          },
          () => {
            this.InvoiceInputService.IsShowLoading = false;
          }
        );
      }
    }
    else {
      this.InvoiceInputFileService.List = [];
      this.InvoiceInputFileService.DataSource = new MatTableDataSource(this.InvoiceInputFileService.List);
      this.InvoiceInputFileService.DataSource.sort = this.InvoiceInputFileSort;
      this.InvoiceInputFileService.DataSource.paginator = this.InvoiceInputFilePaginator;
    }
  }
  InvoiceInputFileDelete(element: InvoiceInputFile) {
    this.InvoiceInputFileService.BaseParameter.ID = element.ID;
    if (confirm(environment.DeleteConfirm)) {
      this.InvoiceInputService.IsShowLoading = true;
      this.InvoiceInputFileService.RemoveAsync().subscribe(
        res => {
          this.InvoiceInputFileSearch();
          this.NotificationService.warn(environment.DeleteSuccess);
        },
        err => {
          this.NotificationService.warn(environment.DeleteNotSuccess);
        },
        () => {
          this.InvoiceInputService.IsShowLoading = false;
        }
      );
    }
  }
  InvoiceInputFileSave(element: InvoiceInputFile) {
    this.InvoiceInputService.IsShowLoading = true;
    element.ParentID = this.InvoiceInputService.BaseParameter.BaseModel.ID;
    this.InvoiceInputFileService.BaseParameter.BaseModel = element;
    this.InvoiceInputFileService.SaveAndUploadFilesAsync().subscribe(
      res => {
        this.InvoiceInputFileSearch();
        this.NotificationService.warn(environment.SaveSuccess);
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.InvoiceInputService.IsShowLoading = false;
      }
    );
  }
}