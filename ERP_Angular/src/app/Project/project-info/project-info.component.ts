import { Component, OnInit, ViewChild } from '@angular/core';
import { ChartOptions, ChartType, ChartDataSets, Chart, ChartConfiguration, ChartData } from 'chart.js';
import { Color, Label, SingleDataSet, monkeyPatchChartJsLegend, monkeyPatchChartJsTooltip } from 'ng2-charts';

import { Router, NavigationEnd, ActivatedRoute } from '@angular/router';
import { MatDialog, MatDialogConfig, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { NotificationService } from 'src/app/shared/Notification.service';
import { DownloadService } from 'src/app/shared/Download.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

import { Company } from 'src/app/shared/ERP/Company.model';
import { CompanyService } from 'src/app/shared/ERP/Company.service';

import { CategoryDepartment } from 'src/app/shared/ERP/CategoryDepartment.model';
import { CategoryDepartmentService } from 'src/app/shared/ERP/CategoryDepartment.service';

import { CategorySystem } from 'src/app/shared/ERP/CategorySystem.model';
import { CategorySystemService } from 'src/app/shared/ERP/CategorySystem.service';

import { Module } from 'src/app/shared/ERP/Module.model';
import { ModuleService } from 'src/app/shared/ERP/Module.service';

import { CategoryType } from 'src/app/shared/ERP/CategoryType.model';
import { CategoryTypeService } from 'src/app/shared/ERP/CategoryType.service';

import { CategoryLevel } from 'src/app/shared/ERP/CategoryLevel.model';
import { CategoryLevelService } from 'src/app/shared/ERP/CategoryLevel.service';

import { CategoryStatus } from 'src/app/shared/ERP/CategoryStatus.model';
import { CategoryStatusService } from 'src/app/shared/ERP/CategoryStatus.service';

import { Project } from 'src/app/shared/ERP/Project.model';
import { ProjectService } from 'src/app/shared/ERP/Project.service';

import { ProjectTask } from 'src/app/shared/ERP/ProjectTask.model';
import { ProjectTaskService } from 'src/app/shared/ERP/ProjectTask.service';

import { ProjectFile } from 'src/app/shared/ERP/ProjectFile.model';
import { ProjectFileService } from 'src/app/shared/ERP/ProjectFile.service';

import { ProjectTaskMembership } from 'src/app/shared/ERP/ProjectTaskMembership.model';
import { ProjectTaskMembershipService } from 'src/app/shared/ERP/ProjectTaskMembership.service';

import { ProjectTaskHistory } from 'src/app/shared/ERP/ProjectTaskHistory.model';
import { ProjectTaskHistoryService } from 'src/app/shared/ERP/ProjectTaskHistory.service';


@Component({
  selector: 'app-project-info',
  templateUrl: './project-info.component.html',
  styleUrls: ['./project-info.component.css']
})
export class ProjectInfoComponent implements OnInit {

  @ViewChild('ProjectTaskSort') ProjectTaskSort: MatSort;
  @ViewChild('ProjectTaskPaginator') ProjectTaskPaginator: MatPaginator;

  @ViewChild('ProjectFileSort') ProjectFileSort: MatSort;
  @ViewChild('ProjectFilePaginator') ProjectFilePaginator: MatPaginator;

  @ViewChild('ProjectTaskMembershipSort') ProjectTaskMembershipSort: MatSort;
  @ViewChild('ProjectTaskMembershipPaginator') ProjectTaskMembershipPaginator: MatPaginator;

  @ViewChild('ProjectTaskHistorySort') ProjectTaskHistorySort: MatSort;
  @ViewChild('ProjectTaskHistoryPaginator') ProjectTaskHistoryPaginator: MatPaginator;

  constructor(
    public ActiveRouter: ActivatedRoute,
    public Router: Router,
    public Dialog: MatDialog,

    public NotificationService: NotificationService,
    public DownloadService: DownloadService,

    public CompanyService: CompanyService,
    public CategoryDepartmentService: CategoryDepartmentService,
    public CategorySystemService: CategorySystemService,
    public ModuleService: ModuleService,
    public CategoryTypeService: CategoryTypeService,
    public CategoryLevelService: CategoryLevelService,
    public CategoryStatusService: CategoryStatusService,

    public ProjectService: ProjectService,
    public ProjectTaskService: ProjectTaskService,
    public ProjectFileService: ProjectFileService,
    public ProjectTaskMembershipService: ProjectTaskMembershipService,
    public ProjectTaskHistoryService: ProjectTaskHistoryService,

  ) {
    this.CategorySystemSearch();
    this.ModuleSearch();
    this.CategoryTypeSearch();
    this.CategoryLevelSearch();
    this.CategoryStatusSearch();
    this.CompanySearch();
    this.Router.events.forEach((event) => {
      if (event instanceof NavigationEnd) {
        this.ProjectService.BaseParameter.ID = Number(this.ActiveRouter.snapshot.params.ID);
        this.ProjectSearch();
      }
    });
  }

  ngOnInit(): void {
  }

  ngAfterViewInit() {
  }
  CategorySystemSearch() {
    this.CategorySystemService.BaseParameter.Active = true;
    this.CategorySystemService.ComponentGetByActiveToListAsync(this.ProjectService);
  }
  ModuleSearch() {
    this.ModuleService.BaseParameter.Active = true;
    this.ModuleService.ComponentGetByActiveToListAsync(this.ProjectService);
  }
  CategoryTypeSearch() {
    this.CategoryTypeService.BaseParameter.Active = true;
    this.CategoryTypeService.ComponentGetByActiveToListAsync(this.ProjectService);
  }
  CategoryLevelSearch() {
    this.CategoryLevelService.BaseParameter.Active = true;
    this.CategoryLevelService.ComponentGetByActiveToListAsync(this.ProjectService);
  }
  CategoryStatusSearch() {
    this.CategoryStatusService.BaseParameter.Active = true;
    this.CategoryStatusService.ComponentGetByActiveToListAsync(this.ProjectService);
  }
  CategoryDepartmentSearch() {
    this.CategoryDepartmentService.BaseParameter.CompanyID = this.ProjectService.BaseParameter.BaseModel.CompanyID;
    this.CategoryDepartmentService.ComponentGetByCompanyIDAndActiveToList(this.ProjectService);
  }
  CompanySearch() {
    this.CompanyService.ComponentGetByMembershipID_ActiveToListAsync(this.ProjectService);
  }
  DateBegin(value) {
    this.ProjectService.BaseParameter.BaseModel.DateBegin = new Date(value);
  }
  DateEnd(value) {
    this.ProjectService.BaseParameter.BaseModel.DateEnd = new Date(value);
  }
  ProjectSearch() {
    this.ProjectService.IsShowLoading = true;
    this.ProjectService.GetByIDAsync().subscribe(
      res => {
        this.ProjectService.BaseParameter.BaseModel = (res as BaseResult).BaseModel;
        this.CategoryDepartmentSearch();
        this.ProjectTaskSearch();
        this.ProjectFileSearch();
        this.ProjectTaskMembershipSearch();
        this.ProjectTaskHistorySearch();
      },
      err => {
      },
      () => {
        this.ProjectService.IsShowLoading = false;
      }
    );
  }
  Save() {
    this.ProjectService.IsShowLoading = true;
    this.ProjectService.BaseParameter.ID = this.ProjectService.BaseParameter.BaseModel.ID;
    this.ProjectService.SaveAsync().subscribe(
      res => {
        this.ProjectService.BaseParameter.BaseModel = (res as BaseResult).BaseModel;
        this.ProjectService.FileToUpload = null;
        this.ProjectService.BaseParameter.Event = null;
        this.Add(this.ProjectService.BaseParameter.BaseModel.ID);
        this.NotificationService.warn(environment.SaveSuccess);
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.ProjectService.IsShowLoading = false;
      }
    );
  }
  Add(ID: number) {
    this.Router.navigateByUrl("ProjectInfo/" + ID);
    this.ProjectService.FileToUpload = null;
    this.ProjectService.BaseParameter.ID = ID;
    this.ProjectSearch();
  }

  ProjectTaskSearch() {
    if (this.ProjectService.BaseParameter.BaseModel.ID > 0) {
      if (this.ProjectTaskService.BaseParameter.SearchString && this.ProjectTaskService.BaseParameter.SearchString.length > 0) {
        this.ProjectTaskService.BaseParameter.SearchString = this.ProjectTaskService.BaseParameter.SearchString.trim();
        if (this.ProjectTaskService.DataSource) {
          this.ProjectTaskService.DataSource.filter = this.ProjectTaskService.BaseParameter.SearchString.toLowerCase();
        }
      }
      else {
        this.ProjectTaskService.List = [];
        this.ProjectTaskService.BaseParameter.ParentID = this.ProjectService.BaseParameter.BaseModel.ID;
        this.ProjectService.IsShowLoading = true;
        this.ProjectTaskService.GetByParentIDAndEmptyToListAsync().subscribe(
          res => {
            this.ProjectTaskService.List = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
            this.ProjectTaskService.DataSource = new MatTableDataSource(this.ProjectTaskService.List);
            this.ProjectTaskService.DataSource.sort = this.ProjectTaskSort;
            this.ProjectTaskService.DataSource.paginator = this.ProjectTaskPaginator;
          },
          err => {
          },
          () => {
            this.ProjectService.IsShowLoading = false;
          }
        );
      }
    }
    else {
      this.ProjectTaskService.List = [];
      this.ProjectTaskService.DataSource = new MatTableDataSource(this.ProjectTaskService.List);
      this.ProjectTaskService.DataSource.sort = this.ProjectTaskSort;
      this.ProjectTaskService.DataSource.paginator = this.ProjectTaskPaginator;
    }
  }
  ProjectTaskSave(element: ProjectTask) {
    this.ProjectService.IsShowLoading = true;
    element.ParentID = this.ProjectService.BaseParameter.BaseModel.ID;
    this.ProjectTaskService.BaseParameter.BaseModel = element;
    this.ProjectTaskService.SaveAsync().subscribe(
      res => {
        this.ProjectTaskSearch();
        this.NotificationService.warn(environment.SaveSuccess);
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.ProjectService.IsShowLoading = false;
      }
    );
  }
  ProjectTaskDelete(element: ProjectTask) {
    if (confirm(environment.DeleteConfirm)) {
      this.ProjectService.IsShowLoading = true;
      this.ProjectTaskService.BaseParameter.ID = element.ID;
      this.ProjectTaskService.RemoveAsync().subscribe(
        res => {
          this.ProjectTaskSearch();
          this.NotificationService.warn(environment.DeleteSuccess);
        },
        err => {
          this.NotificationService.warn(environment.DeleteNotSuccess);
        },
        () => {
          this.ProjectService.IsShowLoading = false;
        }
      );
    }
  }
  ProjectTaskDateBegin(value, element: ProjectTask) {
    element.DateBegin = new Date(value);
  }
  ProjectTaskDateEnd(value, element: ProjectTask) {
    element.DateEnd = new Date(value);
  }

  ProjectFileChange(event, files: FileList) {
    if (files) {
      this.ProjectFileService.FileToUpload = files;
      this.ProjectFileService.BaseParameter.Event = event;
    }
  }
  ProjectFileSearch() {
    if (this.ProjectService.BaseParameter.BaseModel.ID > 0) {
      if (this.ProjectFileService.BaseParameter.SearchString.length > 0) {
        this.ProjectFileService.BaseParameter.SearchString = this.ProjectFileService.BaseParameter.SearchString.trim();
        if (this.ProjectFileService.DataSource) {
          this.ProjectFileService.DataSource.filter = this.ProjectFileService.BaseParameter.SearchString.toLowerCase();
        }
      }
      else {
        this.ProjectFileService.BaseParameter.ParentID = this.ProjectService.BaseParameter.BaseModel.ID;
        this.ProjectService.IsShowLoading = true;
        this.ProjectFileService.GetByParentIDAndEmptyToListAsync().subscribe(
          res => {
            this.ProjectFileService.List = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
            this.ProjectFileService.DataSource = new MatTableDataSource(this.ProjectFileService.List);
            this.ProjectFileService.DataSource.sort = this.ProjectFileSort;
            this.ProjectFileService.DataSource.paginator = this.ProjectFilePaginator;
          },
          err => {
          },
          () => {
            this.ProjectService.IsShowLoading = false;
          }
        );
      }
    }
    else {
      this.ProjectFileService.List = [];
      this.ProjectFileService.DataSource = new MatTableDataSource(this.ProjectFileService.List);
      this.ProjectFileService.DataSource.sort = this.ProjectFileSort;
      this.ProjectFileService.DataSource.paginator = this.ProjectFilePaginator;
    }
  }
  ProjectFileDelete(element: ProjectFile) {
    this.ProjectFileService.BaseParameter.ID = element.ID;
    if (confirm(environment.DeleteConfirm)) {
      this.ProjectService.IsShowLoading = true;
      this.ProjectFileService.RemoveAsync().subscribe(
        res => {
          this.ProjectFileSearch();
          this.NotificationService.warn(environment.DeleteSuccess);
        },
        err => {
          this.NotificationService.warn(environment.DeleteNotSuccess);
        },
        () => {
          this.ProjectService.IsShowLoading = false;
        }
      );
    }
  }
  ProjectFileSave(element: ProjectFile) {
    this.ProjectService.IsShowLoading = true;
    element.ParentID = this.ProjectService.BaseParameter.BaseModel.ID;
    this.ProjectFileService.BaseParameter.BaseModel = element;
    this.ProjectFileService.SaveAndUploadFilesAsync().subscribe(
      res => {
        this.ProjectFileSearch();
        this.NotificationService.warn(environment.SaveSuccess);
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.ProjectService.IsShowLoading = false;
      }
    );
  }
  ProjectTaskMembershipSearch() {
    if (this.ProjectService.BaseParameter.BaseModel.ID > 0) {
      if (this.ProjectTaskMembershipService.BaseParameter.SearchString.length > 0) {
        this.ProjectTaskMembershipService.BaseParameter.SearchString = this.ProjectTaskMembershipService.BaseParameter.SearchString.trim();
        if (this.ProjectTaskMembershipService.DataSource) {
          this.ProjectTaskMembershipService.DataSource.filter = this.ProjectTaskMembershipService.BaseParameter.SearchString.toLowerCase();
        }
      }
      else {
        this.ProjectTaskMembershipService.BaseParameter.ParentID = this.ProjectService.BaseParameter.BaseModel.ID;
        this.ProjectService.IsShowLoading = true;
        this.ProjectTaskMembershipService.GetByParentIDAndEmptyToListAsync().subscribe(
          res => {
            this.ProjectTaskMembershipService.List = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
            this.ProjectTaskMembershipService.DataSource = new MatTableDataSource(this.ProjectTaskMembershipService.List);
            this.ProjectTaskMembershipService.DataSource.sort = this.ProjectFileSort;
            this.ProjectTaskMembershipService.DataSource.paginator = this.ProjectFilePaginator;
          },
          err => {
          },
          () => {
            this.ProjectService.IsShowLoading = false;
          }
        );
      }
    }
    else {
      this.ProjectTaskMembershipService.List = [];
      this.ProjectTaskMembershipService.DataSource = new MatTableDataSource(this.ProjectTaskMembershipService.List);
      this.ProjectTaskMembershipService.DataSource.sort = this.ProjectFileSort;
      this.ProjectTaskMembershipService.DataSource.paginator = this.ProjectFilePaginator;
    }
  }
  ProjectTaskMembershipDelete(element: ProjectTaskMembership) {
    this.ProjectTaskMembershipService.BaseParameter.ID = element.ID;
    if (confirm(environment.DeleteConfirm)) {
      this.ProjectService.IsShowLoading = true;
      this.ProjectTaskMembershipService.RemoveAsync().subscribe(
        res => {
          this.ProjectTaskMembershipSearch();
          this.NotificationService.warn(environment.DeleteSuccess);
        },
        err => {
          this.NotificationService.warn(environment.DeleteNotSuccess);
        },
        () => {
          this.ProjectService.IsShowLoading = false;
        }
      );
    }
  }
  ProjectTaskMembershipSave(element: ProjectTaskMembership) {
    this.ProjectService.IsShowLoading = true;
    element.ParentID = this.ProjectService.BaseParameter.BaseModel.ID;
    this.ProjectTaskMembershipService.BaseParameter.BaseModel = element;
    this.ProjectTaskMembershipService.SaveAsync().subscribe(
      res => {
        this.ProjectTaskMembershipSearch();
        this.NotificationService.warn(environment.SaveSuccess);
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.ProjectService.IsShowLoading = false;
      }
    );
  }

  ProjectTaskHistorySearch() {
    if (this.ProjectService.BaseParameter.BaseModel.ID > 0) {
      if (this.ProjectTaskHistoryService.BaseParameter.SearchString && this.ProjectTaskHistoryService.BaseParameter.SearchString.length > 0) {
        this.ProjectTaskHistoryService.BaseParameter.SearchString = this.ProjectTaskHistoryService.BaseParameter.SearchString.trim();
        if (this.ProjectTaskHistoryService.DataSource) {
          this.ProjectTaskHistoryService.DataSource.filter = this.ProjectTaskHistoryService.BaseParameter.SearchString.toLowerCase();
        }
      }
      else {
        this.ProjectTaskHistoryService.List = [];
        this.ProjectTaskHistoryService.BaseParameter.ParentID = this.ProjectService.BaseParameter.BaseModel.ID;
        this.ProjectService.IsShowLoading = true;
        this.ProjectTaskHistoryService.GetByParentIDAndEmptyToListAsync().subscribe(
          res => {
            this.ProjectTaskHistoryService.List = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
            this.ProjectTaskHistoryService.DataSource = new MatTableDataSource(this.ProjectTaskHistoryService.List);
            this.ProjectTaskHistoryService.DataSource.sort = this.ProjectTaskSort;
            this.ProjectTaskHistoryService.DataSource.paginator = this.ProjectTaskPaginator;
          },
          err => {
          },
          () => {
            this.ProjectService.IsShowLoading = false;
          }
        );
      }
    }
    else {
      this.ProjectTaskHistoryService.List = [];
      this.ProjectTaskHistoryService.DataSource = new MatTableDataSource(this.ProjectTaskHistoryService.List);
      this.ProjectTaskHistoryService.DataSource.sort = this.ProjectTaskSort;
      this.ProjectTaskHistoryService.DataSource.paginator = this.ProjectTaskPaginator;
    }
  }
  ProjectTaskHistorySave(element: ProjectTaskHistory) {
    this.ProjectService.IsShowLoading = true;
    element.ParentID = this.ProjectService.BaseParameter.BaseModel.ID;
    this.ProjectTaskHistoryService.BaseParameter.BaseModel = element;
    this.ProjectTaskHistoryService.SaveAsync().subscribe(
      res => {
        this.ProjectTaskHistorySearch();
        this.NotificationService.warn(environment.SaveSuccess);
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.ProjectService.IsShowLoading = false;
      }
    );
  }
  ProjectTaskHistoryDelete(element: ProjectTaskHistory) {
    if (confirm(environment.DeleteConfirm)) {
      this.ProjectService.IsShowLoading = true;
      this.ProjectTaskHistoryService.BaseParameter.ID = element.ID;
      this.ProjectTaskHistoryService.RemoveAsync().subscribe(
        res => {
          this.ProjectTaskHistorySearch();
          this.NotificationService.warn(environment.DeleteSuccess);
        },
        err => {
          this.NotificationService.warn(environment.DeleteNotSuccess);
        },
        () => {
          this.ProjectService.IsShowLoading = false;
        }
      );
    }
  }
  ProjectTaskHistoryDateBegin(value, element: ProjectTaskHistory) {
    element.DateBegin = new Date(value);
  }
  ProjectTaskHistoryDateEnd(value, element: ProjectTaskHistory) {
    element.DateEnd = new Date(value);
  }
}
