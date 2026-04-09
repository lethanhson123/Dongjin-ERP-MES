import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { NotificationService } from 'src/app/shared/Notification.service';
import { DownloadService } from 'src/app/shared/Download.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

import { Material } from 'src/app/shared/ERP/Material.model';
import { MaterialService } from 'src/app/shared/ERP/Material.service';

import { MaterialConvert } from 'src/app/shared/ERP/MaterialConvert.model';
import { MaterialConvertService } from 'src/app/shared/ERP/MaterialConvert.service';

import { CategoryMaterial } from 'src/app/shared/ERP/CategoryMaterial.model';
import { CategoryMaterialService } from 'src/app/shared/ERP/CategoryMaterial.service';
import { CategoryLocation } from 'src/app/shared/ERP/CategoryLocation.model';
import { CategoryLocationService } from 'src/app/shared/ERP/CategoryLocation.service';
import { CategoryFamily } from 'src/app/shared/ERP/CategoryFamily.model';
import { CategoryFamilyService } from 'src/app/shared/ERP/CategoryFamily.service';

import { Company } from 'src/app/shared/ERP/Company.model';
import { CompanyService } from 'src/app/shared/ERP/Company.service';

@Component({
  selector: 'app-material-modal',
  templateUrl: './material-modal.component.html',
  styleUrls: ['./material-modal.component.css']
})
export class MaterialModalComponent {

  @ViewChild('MaterialSort01') MaterialSort01: MatSort;
  @ViewChild('MaterialPaginator01') MaterialPaginator01: MatPaginator;
  @ViewChild('MaterialConvertSort') MaterialConvertSort: MatSort;
  @ViewChild('MaterialConvertPaginator') MaterialConvertPaginator: MatPaginator;

  constructor(
    public Dialog: MatDialog,
    public DialogRef: MatDialogRef<MaterialModalComponent>,
    public NotificationService: NotificationService,
    public DownloadService: DownloadService,


    public MaterialService: MaterialService,
    public MaterialConvertService: MaterialConvertService,
    public CategoryMaterialService: CategoryMaterialService,
    public CategoryLocationService: CategoryLocationService,
    public CategoryFamilyService: CategoryFamilyService,
    public CompanyService: CompanyService,
  ) {
    this.CompanySearch();
    this.CategoryMaterialSearch();
    this.CategoryLocationSearch();
    this.CategoryFamilySearch();
  }
  ngOnInit(): void {
  }
  ngAfterViewInit() {
    this.MaterialConvertSearch();
    //this.MaterialSearch();
  }
  Close() {
    this.DialogRef.close();
  }
  CompanySearch() {
    this.CompanyService.ComponentGetByMembershipID_ActiveToListAsync(this.MaterialService);    
  }
  CategoryMaterialSearch() {
    this.CategoryMaterialService.BaseParameter.Active = true;
    this.CategoryMaterialService.ComponentGetByActiveToListAsync(this.MaterialService);
  }
  CategoryLocationSearch() {
    this.CategoryLocationService.BaseParameter.Active = true;
    this.CategoryLocationService.ComponentGetByActiveToListAsync(this.MaterialService);
  }
  CategoryFamilySearch() {
    this.CategoryFamilyService.BaseParameter.Active = true;
    this.CategoryFamilyService.ComponentGetByActiveToListAsync(this.MaterialService);
  }
  MaterialSearch() {
    if (this.MaterialService.BaseParameter.SearchStringFilter.length > 0) {
      this.MaterialService.BaseParameter.SearchStringFilter = this.MaterialService.BaseParameter.SearchStringFilter.trim();
      if (this.MaterialService.DataSourceFilter) {
        this.MaterialService.DataSourceFilter.filter = this.MaterialService.BaseParameter.SearchStringFilter.toLowerCase();
      }
    }
    else {
      this.MaterialService.IsShowLoading = true;
      this.MaterialService.GetAllToListAsync().subscribe(
        res => {
          this.MaterialService.ListFilter = (res as BaseResult).List.sort((a, b) => (a.Code > b.Code ? 1 : -1));
          for (let i = 0; i < this.MaterialService.ListFilter.length; i++) {
            this.MaterialService.ListFilter[i].Active = false;
          }
          this.MaterialService.DataSourceFilter = new MatTableDataSource(this.MaterialService.ListFilter);
          this.MaterialService.DataSourceFilter.sort = this.MaterialSort01;
          this.MaterialService.DataSourceFilter.paginator = this.MaterialPaginator01;
        },
        err => {
        },
        () => {
          this.MaterialService.IsShowLoading = false;
        }
      );
    }
  }
  // MaterialConvertSave() {
  //   this.MaterialService.IsShowLoading = true;
  //   this.MaterialConvertService.BaseParameter.ID = this.MaterialService.BaseParameter.BaseModel.ID;
  //   this.MaterialConvertService.BaseParameter.ListID = [];
  //   for (let i = 0; i < this.MaterialService.ListFilter.length; i++) {
  //     if (this.MaterialService.ListFilter[i].Active == true) {
  //       this.MaterialConvertService.BaseParameter.ListID.push(this.MaterialService.ListFilter[i].ID);
  //     }
  //   }
  //   this.MaterialConvertService.CreateAutoAsync().subscribe(
  //     res => {
  //       this.MaterialConvertSearch();
  //     },
  //     err => {
  //     },
  //     () => {
  //       this.MaterialService.IsShowLoading = false;
  //     }
  //   );
  // }
  Save() {
    this.MaterialService.IsShowLoading = true;
    this.MaterialService.SaveAsync().subscribe(
      res => {

        this.NotificationService.warn(environment.SaveSuccess);
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.MaterialService.IsShowLoading = false;
      }
    );
  }
  MaterialConvertSearch() {
    if (this.MaterialConvertService.BaseParameter.SearchString.length > 0) {
      this.MaterialConvertService.BaseParameter.SearchString = this.MaterialConvertService.BaseParameter.SearchString.trim();
      if (this.MaterialConvertService.DataSource) {
        this.MaterialConvertService.DataSource.filter = this.MaterialConvertService.BaseParameter.SearchString.toLowerCase();
      }
    }
    else {
      this.MaterialConvertService.BaseParameter.ParentID = this.MaterialService.BaseParameter.BaseModel.ID;
      this.MaterialService.IsShowLoading = true;
      this.MaterialConvertService.GetByParentIDAndEmptyToListAsync().subscribe(
        res => {
          this.MaterialConvertService.List = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
          this.MaterialConvertService.DataSource = new MatTableDataSource(this.MaterialConvertService.List);
          this.MaterialConvertService.DataSource.sort = this.MaterialConvertSort;
          this.MaterialConvertService.DataSource.paginator = this.MaterialConvertPaginator;
        },
        err => {
        },
        () => {
          this.MaterialService.IsShowLoading = false;
        }
      );
    }
  }
  MaterialConvertSave(element: MaterialConvert) {
    this.MaterialService.IsShowLoading = true;
    element.ParentID = this.MaterialConvertService.BaseParameter.ParentID;
    this.MaterialConvertService.BaseParameter.BaseModel = element;
    this.MaterialConvertService.SaveAsync().subscribe(
      res => {
        this.MaterialConvertService.BaseResult = (res as BaseResult);
        this.MaterialConvertSearch();
        if (this.MaterialConvertService.BaseResult.IsCheck == true) {
          this.NotificationService.warn(environment.SaveSuccess);
        }
        else {
          this.NotificationService.warn(environment.DataExists);
        }
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.MaterialService.IsShowLoading = false;
      }
    );
  }
  MaterialConvertDelete(element: MaterialConvert) {
    this.MaterialService.IsShowLoading = true;
    this.MaterialConvertService.BaseParameter.ID = element.ID;
    if (confirm(environment.DeleteConfirm)) {
      this.MaterialConvertService.RemoveAsync().subscribe(
        res => {
          this.MaterialConvertSearch();
          this.NotificationService.warn(environment.DeleteSuccess);
        },
        err => {
          this.NotificationService.warn(environment.DeleteNotSuccess);
        },
        () => {
          this.MaterialService.IsShowLoading = false;
        }
      );
    }
  }
}
