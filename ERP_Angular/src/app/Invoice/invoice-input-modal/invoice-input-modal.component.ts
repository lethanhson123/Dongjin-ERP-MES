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


@Component({
  selector: 'app-invoice-input-modal',
  templateUrl: './invoice-input-modal.component.html',
  styleUrls: ['./invoice-input-modal.component.css']
})
export class InvoiceInputModalComponent {

  @ViewChild('InvoiceInputDetailSort') InvoiceInputDetailSort: MatSort;
  @ViewChild('InvoiceInputDetailPaginator') InvoiceInputDetailPaginator: MatPaginator;

  @ViewChild('InvoiceInputFileSort') InvoiceInputFileSort: MatSort;
  @ViewChild('InvoiceInputFilePaginator') InvoiceInputFilePaginator: MatPaginator;


  constructor(
    public Dialog: MatDialog,
    public DialogRef: MatDialogRef<InvoiceInputModalComponent>,
    public NotificationService: NotificationService,
    public DownloadService: DownloadService,

    public InvoiceInputService: InvoiceInputService,
    public InvoiceInputDetailService: InvoiceInputDetailService,
    public InvoiceInputFileService: InvoiceInputFileService,

    public CompanyService: CompanyService,
    public MaterialService: MaterialService,
    public CategoryUnitService: CategoryUnitService,
  ) {
  }

  ngOnInit(): void {

  }
  ngAfterViewInit() {
    this.MaterialSearch();
    this.CategoryUnitSearch();
    this.CompanySearch();
    this.InvoiceInputDetailSearch(0);
    this.InvoiceInputFileSearch();
  }
  Close() {
    this.DialogRef.close();
  }
  CategoryUnitSearch() {
    this.CategoryUnitService.ComponentGetAllToListAsync(this.InvoiceInputService);
  }
  MaterialSearch() {
    this.MaterialService.ComponentGetAllToListAsync(this.InvoiceInputService);
  }
  MaterialFilter(searchString: string) {
    this.MaterialService.Filter(searchString);
  }
  CompanySearch() {
    this.CompanyService.ComponentGetAllToListAsync(this.InvoiceInputService);
  }
  Date(value) {
    this.InvoiceInputService.BaseParameter.BaseModel.Date = new Date(value);
  }
  InvoiceInputChange(files: FileList) {
    if (files) {
      this.InvoiceInputService.FileToUpload = files;
    }
  }
  InvoiceInputFileChange(files: FileList) {
    if (files) {
      this.InvoiceInputFileService.FileToUpload = files;
    }
  }
  Save() {
    this.InvoiceInputService.IsShowLoading = true;
    this.InvoiceInputService.SaveAndUploadFilesAsync().subscribe(
      res => {
        this.InvoiceInputService.BaseParameter.BaseModel = (res as BaseResult).BaseModel;
        this.InvoiceInputDetailSearch(0);
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
        },
        err => {
        },
        () => {
          this.InvoiceInputService.IsShowLoading = false;
        }
      );
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

