import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { NotificationService } from 'src/app/shared/Notification.service';
import { DownloadService } from 'src/app/shared/Download.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

import { CategoryLocation } from 'src/app/shared/ERP/CategoryLocation.model';
import { CategoryLocationService } from 'src/app/shared/ERP/CategoryLocation.service';

import { CategoryLocationMaterial } from 'src/app/shared/ERP/CategoryLocationMaterial.model';
import { CategoryLocationMaterialService } from 'src/app/shared/ERP/CategoryLocationMaterial.service';

import { Company } from 'src/app/shared/ERP/Company.model';
import { CompanyService } from 'src/app/shared/ERP/Company.service';

import { CategoryDepartment } from 'src/app/shared/ERP/CategoryDepartment.model';
import { CategoryDepartmentService } from 'src/app/shared/ERP/CategoryDepartment.service';

import { CategoryLayer } from 'src/app/shared/ERP/CategoryLayer.model';
import { CategoryLayerService } from 'src/app/shared/ERP/CategoryLayer.service';

import { CategoryRack } from 'src/app/shared/ERP/CategoryRack.model';
import { CategoryRackService } from 'src/app/shared/ERP/CategoryRack.service';

import { Material } from 'src/app/shared/ERP/Material.model';
import { MaterialService } from 'src/app/shared/ERP/Material.service';
import { MaterialModalComponent } from 'src/app/PC/material-modal/material-modal.component';

@Component({
  selector: 'app-category-location-modal',
  templateUrl: './category-location-modal.component.html',
  styleUrls: ['./category-location-modal.component.css']
})
export class CategoryLocationModalComponent {

  @ViewChild('CategoryLocationMaterialSort') CategoryLocationMaterialSort: MatSort;
  @ViewChild('CategoryLocationMaterialPaginator') CategoryLocationMaterialPaginator: MatPaginator;

  constructor(
    public Dialog: MatDialog,
    public DialogRef: MatDialogRef<CategoryLocationModalComponent>,
    public NotificationService: NotificationService,
    public DownloadService: DownloadService,

    public CategoryLocationService: CategoryLocationService,
    public CategoryLocationMaterialService: CategoryLocationMaterialService,

    public CompanyService: CompanyService,
    public CategoryDepartmentService: CategoryDepartmentService,
    public CategoryLayerService: CategoryLayerService,
    public CategoryRackService: CategoryRackService,
    public MaterialService: MaterialService,

  ) {
    this.MaterialSearch();
    this.CompanySearch();
  }
  ngOnInit(): void {
  }
  ngAfterViewInit() {
    this.CategoryLocationMaterialSearch();
  }
  Close() {
    this.DialogRef.close();
  }
  CompanySearch() {
    this.CompanyService.ComponentGetByMembershipID_ActiveToListAsync(this.CategoryLocationService);
    this.CompanyChange();
  }
  CompanyChange() {
    this.CategoryDepartmentSearch();
    this.CategoryLayerSearch();
    this.CategoryRackSearch();
  }
  CategoryDepartmentSearch() {
    this.CategoryDepartmentService.BaseParameter.CompanyID = this.CategoryLocationService.BaseParameter.BaseModel.CompanyID;
    this.CategoryDepartmentService.ComponentGetByMembershipID_CompanyID_ActiveToListAsync(this.CategoryLocationService);
    this.CategoryDepartmentService.ComponentGetByCompanyIDAndActiveToList(this.CategoryLocationService);
  }
  CategoryDepartmentChange() {
    this.CategoryRackSearch();
  }
  CategoryLayerSearch() {
    this.CategoryLayerService.BaseParameter.Active = true;
    this.CategoryLayerService.BaseParameter.CompanyID = this.CategoryLocationService.BaseParameter.BaseModel.CompanyID;
    this.CategoryLayerService.ComponentGetByCompanyIDAndActiveToListAsync(this.CategoryLocationService);
  }
  CategoryRackSearch() {    
    this.CategoryRackService.BaseParameter.Active = true;
    this.CategoryRackService.BaseParameter.CompanyID = this.CategoryLocationService.BaseParameter.BaseModel.CompanyID;
    this.CategoryRackService.BaseParameter.ParentID = this.CategoryLocationService.BaseParameter.BaseModel.CategoryDepartmentID;
    this.CategoryRackService.ComponentGetByParentIDAndCompanyIDAndActiveToListAsync(this.CategoryLocationService);
  }
  MaterialSearch() {
    this.MaterialService.BaseParameter.Active = true;
    this.MaterialService.BaseParameter.GeneralID = environment.CategoryMaterialID;
    this.MaterialService.ComponentGetByCategoryMaterialID_ActiveToListAsync(this.CategoryLocationService);
  }
  MaterialFilter(searchString: string) {
    this.MaterialService.Filter01(searchString);
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
  MaterialModal(element: CategoryLocationMaterial) {
    this.CategoryLocationService.IsShowLoading = true;
    this.MaterialService.BaseParameter.ID = element.MaterialID;
    this.MaterialService.GetByIDAsync().subscribe(
      res => {
        this.MaterialService.BaseParameter.BaseModel = (res as BaseResult).BaseModel;
        this.MaterialModalOpen(this.MaterialService.BaseParameter.BaseModel);
      },
      err => {
      },
      () => {
        this.CategoryLocationService.IsShowLoading = false;
      }
    );
  }
  Save() {
    this.CategoryLocationService.IsShowLoading = true;
    this.CategoryLocationService.SaveAsync().subscribe(
      res => {
        this.CategoryLocationService.BaseParameter.BaseModel = (res as BaseResult).BaseModel;
        this.NotificationService.warn(environment.SaveSuccess);
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.CategoryLocationService.IsShowLoading = false;
      }
    );
  }
  Print() {
    this.CategoryLocationService.IsShowLoading = true;
    this.CategoryLocationService.PrintAsync().subscribe(
      res => {
        this.CategoryLocationService.BaseResult = (res as BaseResult);
        this.NotificationService.OpenWindowByURL(this.CategoryLocationService.BaseResult.Message);
      },
      err => {
      },
      () => {
        this.CategoryLocationService.IsShowLoading = false;
      }
    );
  }
  Add() {
    this.CategoryLocationService.IsShowLoading = true;
    this.CategoryLocationService.BaseParameter.ID = environment.InitializationNumber;
    this.CategoryLocationService.GetByIDAsync().subscribe(
      res => {
        this.CategoryLocationService.BaseParameter.BaseModel = (res as BaseResult).BaseModel;
        this.CategoryLocationService.BaseParameter.BaseModel.Active = true;
        this.CategoryLocationMaterialSearch();
      },
      err => {
      },
      () => {
        this.CategoryLocationService.IsShowLoading = false;
      }
    );
  }
  Copy() {
    this.CategoryLocationService.IsShowLoading = true;
    this.CategoryLocationService.BaseParameter.BaseModel.ID = environment.InitializationNumber;
    this.CategoryLocationService.BaseParameter.BaseModel.Name = this.CategoryLocationService.BaseParameter.BaseModel.Name + "-Copy";
    this.CategoryLocationService.BaseParameter.BaseModel.SortOrder = this.CategoryLocationService.BaseParameter.BaseModel.SortOrder + 10;
    this.CategoryLocationService.SaveAsync().subscribe(
      res => {
        this.CategoryLocationService.BaseParameter.BaseModel = (res as BaseResult).BaseModel;
        this.NotificationService.warn(environment.SaveSuccess);
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.CategoryLocationService.IsShowLoading = false;
      }
    );
  }
  CategoryLocationMaterialSearch() {
    if (this.CategoryLocationMaterialService.BaseParameter.SearchString.length > 0) {
      this.CategoryLocationMaterialService.BaseParameter.SearchString = this.CategoryLocationMaterialService.BaseParameter.SearchString.trim();
      if (this.CategoryLocationMaterialService.DataSource) {
        this.CategoryLocationMaterialService.DataSource.filter = this.CategoryLocationMaterialService.BaseParameter.SearchString.toLowerCase();
      }
    }
    else {
      this.CategoryLocationMaterialService.BaseParameter.ParentID = this.CategoryLocationService.BaseParameter.BaseModel.ID;
      this.CategoryLocationService.IsShowLoading = true;
      this.CategoryLocationMaterialService.GetByParentIDAndEmptyToListAsync().subscribe(
        res => {
          this.CategoryLocationMaterialService.List = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
          this.CategoryLocationMaterialService.DataSource = new MatTableDataSource(this.CategoryLocationMaterialService.List);
          this.CategoryLocationMaterialService.DataSource.sort = this.CategoryLocationMaterialSort;
          this.CategoryLocationMaterialService.DataSource.paginator = this.CategoryLocationMaterialPaginator;
        },
        err => {
        },
        () => {
          this.CategoryLocationService.IsShowLoading = false;
        }
      );
    }
  }
  CategoryLocationMaterialDelete(element: CategoryLocationMaterial) {
    this.CategoryLocationMaterialService.BaseParameter.ID = element.ID;
    if (confirm(environment.DeleteConfirm)) {
      this.CategoryLocationService.IsShowLoading = true;
      this.CategoryLocationMaterialService.RemoveAsync().subscribe(
        res => {
          this.CategoryLocationMaterialSearch();
          this.NotificationService.warn(environment.DeleteSuccess);
        },
        err => {
          this.NotificationService.warn(environment.DeleteNotSuccess);
        },
        () => {
          this.CategoryLocationService.IsShowLoading = false;
        }
      );
    }
  }
  CategoryLocationMaterialSave(element: CategoryLocationMaterial) {
    this.CategoryLocationService.IsShowLoading = true;
    element.ParentID = this.CategoryLocationService.BaseParameter.BaseModel.ID;
    this.CategoryLocationMaterialService.BaseParameter.BaseModel = element;
    this.CategoryLocationMaterialService.SaveAsync().subscribe(
      res => {
        this.CategoryLocationMaterialSearch();
        this.NotificationService.warn(environment.SaveSuccess);
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.CategoryLocationService.IsShowLoading = false;
      }
    );
  }

}
