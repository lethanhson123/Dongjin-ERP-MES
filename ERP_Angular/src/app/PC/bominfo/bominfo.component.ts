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

import { BOM } from 'src/app/shared/ERP/BOM.model';
import { BOMService } from 'src/app/shared/ERP/BOM.service';

import { BOMDetail } from 'src/app/shared/ERP/BOMDetail.model';
import { BOMDetailService } from 'src/app/shared/ERP/BOMDetail.service';

import { BOMTerm } from 'src/app/shared/ERP/BOMTerm.model';
import { BOMTermService } from 'src/app/shared/ERP/BOMTerm.service';

import { BOMStage } from 'src/app/shared/ERP/BOMStage.model';
import { BOMStageService } from 'src/app/shared/ERP/BOMStage.service';

import { BOMFile } from 'src/app/shared/ERP/BOMFile.model';
import { BOMFileService } from 'src/app/shared/ERP/BOMFile.service';

import { CategoryUnit } from 'src/app/shared/ERP/CategoryUnit.model';
import { CategoryUnitService } from 'src/app/shared/ERP/CategoryUnit.service';
import { MaterialModalComponent } from 'src/app/PC/material-modal/material-modal.component';

import { Material } from 'src/app/shared/ERP/Material.model';
import { MaterialService } from 'src/app/shared/ERP/Material.service';

import { Company } from 'src/app/shared/ERP/Company.model';
import { CompanyService } from 'src/app/shared/ERP/Company.service';

@Component({
  selector: 'app-bominfo',
  templateUrl: './bominfo.component.html',
  styleUrls: ['./bominfo.component.css']
})
export class BOMInfoComponent {

  @ViewChild('BOMSortFilter') BOMSortFilter: MatSort;
  @ViewChild('BOMPaginatorFilter') BOMPaginatorFilter: MatPaginator;

  @ViewChild('BOMDetailSort') BOMDetailSort: MatSort;
  @ViewChild('BOMDetailPaginator') BOMDetailPaginator: MatPaginator;

  @ViewChild('BOMTermSort') BOMTermSort: MatSort;
  @ViewChild('BOMTermPaginator') BOMTermPaginator: MatPaginator;

  @ViewChild('BOMStageSort') BOMStageSort: MatSort;
  @ViewChild('BOMStagePaginator') BOMStagePaginator: MatPaginator;

  @ViewChild('BOMFileSort') BOMFileSort: MatSort;
  @ViewChild('BOMFilePaginator') BOMFilePaginator: MatPaginator;

  URLTemplate: string;

  constructor(
    public ActiveRouter: ActivatedRoute,
    public Router: Router,
    public Dialog: MatDialog,

    public NotificationService: NotificationService,
    public DownloadService: DownloadService,

    public BOMService: BOMService,
    public BOMDetailService: BOMDetailService,
    public BOMTermService: BOMTermService,
    public BOMStageService: BOMStageService,
    public BOMFileService: BOMFileService,


    public CategoryUnitService: CategoryUnitService,
    public MaterialService: MaterialService,
    public CompanyService: CompanyService,
  ) {
    this.URLTemplate = this.BOMService.APIRootURL + "Download/BOM.xlsx";
    this.BOMService.BaseParameter.SearchString = environment.InitializationString;
    this.BOMDetailService.BaseParameter.SearchString = environment.InitializationString;
    this.CompanySearch();
    this.CategoryUnitSearch();
    this.MaterialSearch();
    this.Router.events.forEach((event) => {
      if (event instanceof NavigationEnd) {
        this.BOMService.BaseParameter.ID = Number(this.ActiveRouter.snapshot.params.ID);
        this.BOMSearch();
      }
    });
  }

  ngOnInit(): void {

  }
  ngAfterViewInit() {

  }
  CompanySearch() {
    this.CompanyService.ComponentGetByMembershipID_ActiveToListAsync(this.BOMService);
  }
  MaterialSearch() {
    this.MaterialService.BaseParameter.Active = true;
    this.MaterialService.ComponentGetByActiveToListAsync(this.BOMService);
  }
  MaterialFilter01(searchString: string) {
    this.MaterialService.Filter01(searchString);
  }
  MaterialFilter02(searchString: string) {
    this.MaterialService.Filter02(searchString);
  }
  MaterialFilter03(searchString: string) {
    this.MaterialService.Filter03(searchString);
  }
  CategoryUnitSearch() {
    this.CategoryUnitService.BaseParameter.Active = true;
    this.CategoryUnitService.ComponentGetByActiveToListAsync(this.BOMService);
  }
  Date(value) {
    this.BOMService.BaseParameter.BaseModel.Date = new Date(value);
  }
  BOMDate(value, element: BOM) {
    element.Date = new Date(value);
  }

  BOMSearch() {
    this.BOMService.IsShowLoading = true;
    this.BOMService.GetByIDAsync().subscribe(
      res => {
        this.BOMService.BaseParameter.BaseModel = (res as BaseResult).BaseModel;
        this.MaterialService.BaseParameter.ID = this.BOMService.BaseParameter.BaseModel.MaterialID;
        this.BOMDetailSearch();
        this.BOMTermSearch();
        this.BOMStageSearch();
        this.BOMSearch001();
        this.BOMFileSearch();
      },
      err => {
      },
      () => {
      }
    );
  }
  BOMSearch001() {
    if (this.BOMService.BaseParameter.SearchString.length > 0) {
      this.BOMService.BaseParameter.SearchString = this.BOMService.BaseParameter.SearchString.trim();
      if (this.BOMService.DataSourceFilter) {
        this.BOMService.DataSourceFilter.filter = this.BOMService.BaseParameter.SearchString.toLowerCase();
      }
    }
    else {
      this.BOMService.IsShowLoading = true;
      this.BOMService.BaseParameter.ParentID = this.BOMService.BaseParameter.BaseModel.ID;
      this.BOMService.GetByParentIDToListAsync().subscribe(
        res => {
          this.BOMService.ListFilter = (res as BaseResult).List;
          this.BOMService.DataSourceFilter = new MatTableDataSource(this.BOMService.ListFilter);
          this.BOMService.DataSourceFilter.sort = this.BOMSortFilter;
          this.BOMService.DataSourceFilter.paginator = this.BOMPaginatorFilter;

          this.BOMService.BaseParameter.Count = 0;
          this.BOMService.BaseParameter.Sum = 0;
          let ListLeadNo = this.BOMService.ListFilter.filter(o => o.IsLeadNo == true);
          let ListSPST = this.BOMService.ListFilter.filter(o => o.IsSPST == true);
          if (ListLeadNo && ListLeadNo.length > 0) {
            this.BOMService.BaseParameter.Count = ListLeadNo.length;
          }
          if (ListSPST && ListSPST.length > 0) {
            this.BOMService.BaseParameter.Sum = ListSPST.length;
          }
        },
        err => {
        },
        () => {
          this.BOMService.IsShowLoading = false;
        }
      );
    }
  }
  Add(ID: number) {
    this.Router.navigateByUrl("BOMInfo/" + ID);
    this.BOMService.BaseParameter.ID = ID;
    this.BOMSearch();
  }
  BOMChange(event, files: FileList) {
    if (files) {
      this.BOMService.FileToUpload = files;
      this.BOMService.BaseParameter.Active = true;
      this.BOMService.BaseParameter.Event = event;
    }
  }
  BOMSemiFinishedChange(event, files: FileList) {
    if (files) {
      this.BOMService.FileToUpload = files;
      this.BOMService.BaseParameter.Active = false;
      this.BOMService.BaseParameter.Event = event;
    }
  }
  BOMSave(element: BOM) {
    this.BOMService.IsShowLoading = true;
    this.BOMService.BaseResult.BaseModel = this.BOMService.BaseParameter.BaseModel;
    this.BOMService.BaseParameter.BaseModel = element;
    this.BOMService.SaveAsync().subscribe(
      res => {
        this.NotificationService.warn(environment.SaveSuccess);
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.BOMService.IsShowLoading = false;
      }
    );
    this.BOMService.BaseParameter.BaseModel = this.BOMService.BaseResult.BaseModel;
  }
  Save() {
    this.BOMService.IsShowLoading = true;
    this.BOMService.SaveAndUploadFileAsync().subscribe(
      res => {
        this.BOMService.BaseParameter.BaseModel = (res as BaseResult).BaseModel;
        this.BOMService.FileToUpload = null;
        this.BOMService.BaseParameter.Event = null;
        this.Add(this.BOMService.BaseParameter.BaseModel.ID);
        this.NotificationService.warn(environment.SaveSuccess);
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.BOMService.IsShowLoading = false;
      }
    );
  }
  Copy() {
    this.BOMService.IsShowLoading = true;
    this.BOMService.CopyAsync().subscribe(
      res => {
        this.BOMService.BaseParameter.BaseModel = (res as BaseResult).BaseModel;
        this.Add(this.BOMService.BaseParameter.BaseModel.ID);
        this.NotificationService.warn(environment.SaveSuccess);
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.BOMService.IsShowLoading = false;
      }
    );
  }
  BOMDownload() {
    this.BOMService.IsShowLoading = true;
    this.BOMService.ExportBOMLeadByIDToExcelAsync().subscribe(
      res => {
        let url = (res as BaseResult).Message;
        window.open(url, "_blank");
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.BOMService.IsShowLoading = false;
      }
    );
  }
  BOMDetailSearch() {
    if (this.BOMDetailService.BaseParameter.SearchString.length > 0) {
      this.BOMDetailService.BaseParameter.SearchString = this.BOMDetailService.BaseParameter.SearchString.trim();
      if (this.BOMDetailService.DataSource) {
        this.BOMDetailService.DataSource.filter = this.BOMDetailService.BaseParameter.SearchString.toLowerCase();
      }
    }
    else {
      this.BOMDetailService.BaseParameter.ParentID = this.BOMService.BaseParameter.BaseModel.ID;
      this.BOMService.IsShowLoading = true;
      this.BOMDetailService.GetByParentIDAndEmptyToListAsync().subscribe(
        res => {
          this.BOMDetailService.List = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
          this.BOMDetailService.DataSource = new MatTableDataSource(this.BOMDetailService.List);
          this.BOMDetailService.DataSource.sort = this.BOMDetailSort;
          this.BOMDetailService.DataSource.paginator = this.BOMDetailPaginator;
        },
        err => {
        },
        () => {
          this.BOMService.IsShowLoading = false;
        }
      );
    }
  }
  BOMDetailDelete(element: BOMDetail) {
    this.BOMService.IsShowLoading = true;
    this.BOMDetailService.BaseParameter.ID = element.ID;
    if (confirm(environment.DeleteConfirm)) {
      this.BOMService.IsShowLoading = true;
      this.BOMDetailService.RemoveAsync().subscribe(
        res => {
          this.BOMDetailSearch();
          this.NotificationService.warn(environment.DeleteSuccess);
        },
        err => {
          this.NotificationService.warn(environment.DeleteNotSuccess);
        },
        () => {
          this.BOMService.IsShowLoading = false;
        }
      );
    }
  }
  BOMDetailSave(element: BOMDetail) {
    this.BOMService.IsShowLoading = true;
    element.ParentID = this.BOMService.BaseParameter.BaseModel.ID;
    this.BOMDetailService.BaseParameter.BaseModel = element;
    this.BOMDetailService.SaveAsync().subscribe(
      res => {
        this.BOMDetailSearch();
        this.NotificationService.warn(environment.SaveSuccess);
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.BOMService.IsShowLoading = false;
      }
    );
  }
  BOMTermSearch() {
    if (this.BOMTermService.BaseParameter.SearchString.length > 0) {
      this.BOMTermService.BaseParameter.SearchString = this.BOMTermService.BaseParameter.SearchString.trim();
      if (this.BOMTermService.DataSource) {
        this.BOMTermService.DataSource.filter = this.BOMTermService.BaseParameter.SearchString.toLowerCase();
      }
    }
    else {
      this.BOMTermService.BaseParameter.ParentID = this.BOMService.BaseParameter.BaseModel.ID;
      this.BOMService.IsShowLoading = true;
      this.BOMTermService.GetByParentIDAndEmptyToListAsync().subscribe(
        res => {
          this.BOMTermService.List = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
          this.BOMTermService.DataSource = new MatTableDataSource(this.BOMTermService.List);
          this.BOMTermService.DataSource.sort = this.BOMTermSort;
          this.BOMTermService.DataSource.paginator = this.BOMTermPaginator;
        },
        err => {
        },
        () => {
          this.BOMService.IsShowLoading = false;
        }
      );
    }
  }
  BOMTermDelete(element: BOMTerm) {
    this.BOMService.IsShowLoading = true;
    this.BOMTermService.BaseParameter.ID = element.ID;
    if (confirm(environment.DeleteConfirm)) {
      this.BOMService.IsShowLoading = true;
      this.BOMTermService.RemoveAsync().subscribe(
        res => {
          this.BOMTermSearch();
          this.NotificationService.warn(environment.DeleteSuccess);
        },
        err => {
          this.NotificationService.warn(environment.DeleteNotSuccess);
        },
        () => {
          this.BOMService.IsShowLoading = false;
        }
      );
    }
  }
  BOMTermSave(element: BOMTerm) {
    this.BOMService.IsShowLoading = true;
    element.ParentID = this.BOMService.BaseParameter.BaseModel.ID;
    this.BOMTermService.BaseParameter.BaseModel = element;
    this.BOMTermService.SaveAsync().subscribe(
      res => {
        this.BOMTermSearch();
        this.NotificationService.warn(environment.SaveSuccess);
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.BOMService.IsShowLoading = false;
      }
    );
  }
  BOMStageSearch() {
    if (this.BOMStageService.BaseParameter.SearchString.length > 0) {
      this.BOMStageService.BaseParameter.SearchString = this.BOMStageService.BaseParameter.SearchString.trim();
      if (this.BOMStageService.DataSource) {
        this.BOMStageService.DataSource.filter = this.BOMStageService.BaseParameter.SearchString.toLowerCase();
      }
    }
    else {
      this.BOMStageService.BaseParameter.ParentID = this.BOMService.BaseParameter.BaseModel.ID;
      this.BOMService.IsShowLoading = true;
      this.BOMStageService.GetByParentIDAndEmptyToListAsync().subscribe(
        res => {
          this.BOMStageService.List = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
          this.BOMStageService.DataSource = new MatTableDataSource(this.BOMStageService.List);
          this.BOMStageService.DataSource.sort = this.BOMStageSort;
          this.BOMStageService.DataSource.paginator = this.BOMStagePaginator;
        },
        err => {
        },
        () => {
          this.BOMService.IsShowLoading = false;
        }
      );
    }
  }
  BOMStageDelete(element: BOMStage) {
    this.BOMService.IsShowLoading = true;
    this.BOMStageService.BaseParameter.ID = element.ID;
    if (confirm(environment.DeleteConfirm)) {
      this.BOMService.IsShowLoading = true;
      this.BOMStageService.RemoveAsync().subscribe(
        res => {
          this.BOMStageSearch();
          this.NotificationService.warn(environment.DeleteSuccess);
        },
        err => {
          this.NotificationService.warn(environment.DeleteNotSuccess);
        },
        () => {
          this.BOMService.IsShowLoading = false;
        }
      );
    }
  }
  BOMStageSave(element: BOMStage) {
    this.BOMService.IsShowLoading = true;
    element.ParentID = this.BOMService.BaseParameter.BaseModel.ID;
    this.BOMStageService.BaseParameter.BaseModel = element;
    this.BOMStageService.SaveAsync().subscribe(
      res => {
        this.BOMStageSearch();
        this.NotificationService.warn(environment.SaveSuccess);
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.BOMService.IsShowLoading = false;
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
  MaterialModal(ID: number) {
    this.BOMService.IsShowLoading = true;
    this.MaterialService.BaseParameter.ID = ID;
    this.MaterialService.GetByIDAsync().subscribe(
      res => {
        this.MaterialService.BaseParameter.BaseModel = (res as BaseResult).BaseModel;
        this.MaterialModalOpen(this.MaterialService.BaseParameter.BaseModel);
      },
      err => {
      },
      () => {
        this.BOMService.IsShowLoading = false;
      }
    );
  }
  BOMFileSearch() {
    if (this.BOMService.BaseParameter.BaseModel.ID > 0) {
      if (this.BOMFileService.BaseParameter.SearchString && this.BOMFileService.BaseParameter.SearchString.length > 0) {
        this.BOMFileService.BaseParameter.SearchString = this.BOMFileService.BaseParameter.SearchString.trim();
        if (this.BOMFileService.DataSource) {
          this.BOMFileService.DataSource.filter = this.BOMFileService.BaseParameter.SearchString.toLowerCase();
        }
      }
      else {
        this.BOMFileService.List = [];
        this.BOMFileService.BaseParameter.ParentID = this.BOMService.BaseParameter.BaseModel.ID;
        this.BOMService.IsShowLoading = true;
        this.BOMFileService.GetByParentIDAndEmptyToListAsync().subscribe(
          res => {
            this.BOMFileService.List = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
            this.BOMFileService.DataSource = new MatTableDataSource(this.BOMFileService.List);
            this.BOMFileService.DataSource.sort = this.BOMFileSort;
            this.BOMFileService.DataSource.paginator = this.BOMFilePaginator;
          },
          err => {
          },
          () => {
            this.BOMService.IsShowLoading = false;
          }
        );
      }
    }
    else {
      this.BOMFileService.List = [];
      this.BOMFileService.DataSource = new MatTableDataSource(this.BOMFileService.List);
      this.BOMFileService.DataSource.sort = this.BOMFileSort;
      this.BOMFileService.DataSource.paginator = this.BOMFilePaginator;
    }
  }
  BOMFileDelete(element: BOMFile) {
    this.BOMFileService.BaseParameter.ID = element.ID;
    if (confirm(environment.DeleteConfirm)) {
      this.BOMService.IsShowLoading = true;
      this.BOMFileService.RemoveAsync().subscribe(
        res => {
          this.BOMFileSearch();
          this.NotificationService.warn(environment.DeleteSuccess);
        },
        err => {
          this.NotificationService.warn(environment.DeleteNotSuccess);
        },
        () => {
          this.BOMService.IsShowLoading = false;
        }
      );
    }
  }
  BOMFileSave(element: BOMFile) {
    this.BOMService.IsShowLoading = true;
    element.ParentID = this.BOMService.BaseParameter.BaseModel.ID;
    this.BOMFileService.BaseParameter.BaseModel = element;
    this.BOMFileService.SaveAndUploadFilesAsync().subscribe(
      res => {
        this.BOMFileSearch();
        this.NotificationService.warn(environment.SaveSuccess);
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.BOMService.IsShowLoading = false;
      }
    );
  }
}
