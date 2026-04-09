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

import { InvoiceOutput } from 'src/app/shared/ERP/InvoiceOutput.model';
import { InvoiceOutputService } from 'src/app/shared/ERP/InvoiceOutput.service';

import { InvoiceOutputDetail } from 'src/app/shared/ERP/InvoiceOutputDetail.model';
import { InvoiceOutputDetailService } from 'src/app/shared/ERP/InvoiceOutputDetail.service';

import { Company } from 'src/app/shared/ERP/Company.model';
import { CompanyService } from 'src/app/shared/ERP/Company.service';

import { CategoryUnit } from 'src/app/shared/ERP/CategoryUnit.model';
import { CategoryUnitService } from 'src/app/shared/ERP/CategoryUnit.service';

import { Material } from 'src/app/shared/ERP/Material.model';
import { MaterialService } from 'src/app/shared/ERP/Material.service';
import { MaterialModalComponent } from 'src/app/PC/material-modal/material-modal.component';

import { InvoiceOutputFile } from 'src/app/shared/ERP/InvoiceOutputFile.model';
import { InvoiceOutputFileService } from 'src/app/shared/ERP/InvoiceOutputFile.service';

import { WarehouseOutput } from 'src/app/shared/ERP/WarehouseOutput.model';
import { WarehouseOutputService } from 'src/app/shared/ERP/WarehouseOutput.service';
import { WarehouseOutputModalComponent } from 'src/app/Warehouse/warehouse-output-modal/warehouse-output-modal.component';


@Component({
  selector: 'app-invoice-output-modal',
  templateUrl: './invoice-output-modal.component.html',
  styleUrls: ['./invoice-output-modal.component.css']
})
export class InvoiceOutputModalComponent {

  @ViewChild('InvoiceOutputDetailSort') InvoiceOutputDetailSort: MatSort;
  @ViewChild('InvoiceOutputDetailPaginator') InvoiceOutputDetailPaginator: MatPaginator;

  @ViewChild('InvoiceOutputFileSort') InvoiceOutputFileSort: MatSort;
  @ViewChild('InvoiceOutputFilePaginator') InvoiceOutputFilePaginator: MatPaginator;


  constructor(
    public Dialog: MatDialog,
    public DialogRef: MatDialogRef<InvoiceOutputModalComponent>,
    public NotificationService: NotificationService,
    public DownloadService: DownloadService,

    public InvoiceOutputService: InvoiceOutputService,
    public InvoiceOutputDetailService: InvoiceOutputDetailService,
    public InvoiceOutputFileService: InvoiceOutputFileService,

    public WarehouseOutputService: WarehouseOutputService,

    public CompanyService: CompanyService,
    public MaterialService: MaterialService,
    public CategoryUnitService: CategoryUnitService,
  ) {
  }

  ngOnInit(): void {

  }
  ngAfterViewInit() {
    this.WarehouseOutputSearch();
    this.MaterialSearch();
    this.CategoryUnitSearch();
    this.CompanySearch();
    this.InvoiceOutputDetailSearch(0);
    this.InvoiceOutputFileSearch();
  }
  Close() {
    this.DialogRef.close();
  }
  WarehouseOutputSearch() {
    this.WarehouseOutputService.ComponentGetAllToListAsync(this.InvoiceOutputService);
  }
  CategoryUnitSearch() {
    this.CategoryUnitService.ComponentGetAllToListAsync(this.InvoiceOutputService);
  }
  MaterialSearch() {
    this.MaterialService.ComponentGetAllToListAsync(this.InvoiceOutputService);
  }
  MaterialFilter(searchString: string) {
    this.MaterialService.Filter(searchString);
  }
  CompanySearch() {
    this.CompanyService.ComponentGetAllToListAsync(this.InvoiceOutputService);
  }
  Date(value) {
    this.InvoiceOutputService.BaseParameter.BaseModel.Date = new Date(value);
  }
  InvoiceOutputChange(files: FileList) {
    if (files) {
      this.InvoiceOutputService.FileToUpload = files;
    }
  }
  InvoiceOutputFileChange(files: FileList) {
    if (files) {
      this.InvoiceOutputFileService.FileToUpload = files;
    }
  }
  Save() {
    this.InvoiceOutputService.IsShowLoading = true;
    this.InvoiceOutputService.SaveAndUploadFilesAsync().subscribe(
      res => {
        this.InvoiceOutputService.BaseParameter.BaseModel = (res as BaseResult).BaseModel;
        this.InvoiceOutputDetailSearch(0);
        this.NotificationService.warn(environment.SaveSuccess);
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.InvoiceOutputService.IsShowLoading = false;
      }
    );
  }
  InvoiceOutputDetailSearch(Action: number) {
    if (Action > 0) {
      this.InvoiceOutputService.IsShowLoading = true;
      this.InvoiceOutputService.BaseParameter.ID = this.InvoiceOutputService.BaseParameter.BaseModel.ID;
      this.InvoiceOutputService.GetByIDAsync().subscribe(
        res => {
          this.InvoiceOutputService.BaseParameter.BaseModel = (res as BaseResult).BaseModel;
          this.InvoiceOutputDetailSearchSub();
        },
        err => {
        },
        () => {
          this.InvoiceOutputService.IsShowLoading = false;
        }
      );
    }
    else {
      this.InvoiceOutputDetailSearchSub();
    }
  }
  InvoiceOutputDetailSearchSub() {
    if (this.InvoiceOutputDetailService.BaseParameter.SearchString.length > 0) {
      this.InvoiceOutputDetailService.BaseParameter.SearchString = this.InvoiceOutputDetailService.BaseParameter.SearchString.trim();
      if (this.InvoiceOutputDetailService.DataSource) {
        this.InvoiceOutputDetailService.DataSource.filter = this.InvoiceOutputDetailService.BaseParameter.SearchString.toLowerCase();
      }
    }
    else {
      this.InvoiceOutputDetailService.BaseParameter.ParentID = this.InvoiceOutputService.BaseParameter.BaseModel.ID;
      this.InvoiceOutputService.IsShowLoading = true;
      this.InvoiceOutputDetailService.GetByParentIDAndEmptyToListAsync().subscribe(
        res => {
          this.InvoiceOutputDetailService.List = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
          this.InvoiceOutputDetailService.DataSource = new MatTableDataSource(this.InvoiceOutputDetailService.List);
          this.InvoiceOutputDetailService.DataSource.sort = this.InvoiceOutputDetailSort;
          this.InvoiceOutputDetailService.DataSource.paginator = this.InvoiceOutputDetailPaginator;
        },
        err => {
        },
        () => {
          this.InvoiceOutputService.IsShowLoading = false;
        }
      );
    }
  }
  InvoiceOutputDetailDelete(element: InvoiceOutputDetail) {
    this.InvoiceOutputDetailService.BaseParameter.ID = element.ID;
    if (confirm(environment.DeleteConfirm)) {
      this.InvoiceOutputService.IsShowLoading = true;
      this.InvoiceOutputDetailService.RemoveAsync().subscribe(
        res => {
          this.InvoiceOutputDetailSearch(2);
          this.NotificationService.warn(environment.DeleteSuccess);
        },
        err => {
          this.NotificationService.warn(environment.DeleteNotSuccess);
        },
        () => {
          this.InvoiceOutputService.IsShowLoading = false;
        }
      );
    }
  }
  InvoiceOutputDetailSave(element: InvoiceOutputDetail) {
    this.InvoiceOutputService.IsShowLoading = true;
    element.ParentID = this.InvoiceOutputService.BaseParameter.BaseModel.ID;
    this.InvoiceOutputDetailService.BaseParameter.BaseModel = element;
    this.InvoiceOutputDetailService.SaveAsync().subscribe(
      res => {
        this.InvoiceOutputDetailSearch(1);
        this.NotificationService.warn(environment.SaveSuccess);
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.InvoiceOutputService.IsShowLoading = false;
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
  MaterialModal(element: InvoiceOutputDetail) {
    this.InvoiceOutputService.IsShowLoading = true;
    this.MaterialService.BaseParameter.ID = element.MaterialID;
    this.MaterialService.GetByIDAsync().subscribe(
      res => {
        this.MaterialService.BaseParameter.BaseModel = (res as BaseResult).BaseModel;
        this.MaterialModalOpen(this.MaterialService.BaseParameter.BaseModel);
      },
      err => {
      },
      () => {
        this.InvoiceOutputService.IsShowLoading = false;
      }
    );
  }
  InvoiceOutputFileSearch() {
    if (this.InvoiceOutputFileService.BaseParameter.SearchString.length > 0) {
      this.InvoiceOutputFileService.BaseParameter.SearchString = this.InvoiceOutputFileService.BaseParameter.SearchString.trim();
      if (this.InvoiceOutputFileService.DataSource) {
        this.InvoiceOutputFileService.DataSource.filter = this.InvoiceOutputFileService.BaseParameter.SearchString.toLowerCase();
      }
    }
    else {
      this.InvoiceOutputFileService.BaseParameter.ParentID = this.InvoiceOutputService.BaseParameter.BaseModel.ID;
      this.InvoiceOutputService.IsShowLoading = true;
      this.InvoiceOutputFileService.GetByParentIDAndEmptyToListAsync().subscribe(
        res => {
          this.InvoiceOutputFileService.List = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
          this.InvoiceOutputFileService.DataSource = new MatTableDataSource(this.InvoiceOutputFileService.List);
          this.InvoiceOutputFileService.DataSource.sort = this.InvoiceOutputFileSort;
          this.InvoiceOutputFileService.DataSource.paginator = this.InvoiceOutputFilePaginator;
        },
        err => {
        },
        () => {
          this.InvoiceOutputService.IsShowLoading = false;
        }
      );
    }
  }
  InvoiceOutputFileDelete(element: InvoiceOutputFile) {
    this.InvoiceOutputFileService.BaseParameter.ID = element.ID;
    if (confirm(environment.DeleteConfirm)) {
      this.InvoiceOutputService.IsShowLoading = true;
      this.InvoiceOutputFileService.RemoveAsync().subscribe(
        res => {
          this.InvoiceOutputFileSearch();
          this.NotificationService.warn(environment.DeleteSuccess);
        },
        err => {
          this.NotificationService.warn(environment.DeleteNotSuccess);
        },
        () => {
          this.InvoiceOutputService.IsShowLoading = false;
        }
      );
    }
  }
  InvoiceOutputFileSave(element: InvoiceOutputFile) {
    this.InvoiceOutputService.IsShowLoading = true;
    element.ParentID = this.InvoiceOutputService.BaseParameter.BaseModel.ID;
    this.InvoiceOutputFileService.BaseParameter.BaseModel = element;
    this.InvoiceOutputFileService.SaveAndUploadFilesAsync().subscribe(
      res => {
        this.InvoiceOutputFileSearch();
        this.NotificationService.warn(environment.SaveSuccess);
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.InvoiceOutputService.IsShowLoading = false;
      }
    );
  }  
  WarehouseOutputModal() {
    this.InvoiceOutputService.IsShowLoading = true;
    this.WarehouseOutputService.BaseParameter.ID = environment.InitializationNumber;
    this.WarehouseOutputService.GetByIDAsync().subscribe(
      res => {
        this.WarehouseOutputService.BaseParameter.BaseModel = (res as BaseResult).BaseModel;
        const dialogConfig = new MatDialogConfig();
        dialogConfig.disableClose = true;
        dialogConfig.autoFocus = true;
        dialogConfig.width = environment.DialogConfigWidth;
        const dialog = this.Dialog.open(WarehouseOutputModalComponent, dialogConfig);
        dialog.afterClosed().subscribe(() => {
        });
      },
      err => {
      },
      () => {
        this.InvoiceOutputService.IsShowLoading = false;
      }
    );
  }
}
