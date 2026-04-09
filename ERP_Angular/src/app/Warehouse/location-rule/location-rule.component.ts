import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { NotificationService } from 'src/app/shared/Notification.service';
import { DownloadService } from 'src/app/shared/Download.service';

import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

import { CategoryLayer } from 'src/app/shared/ERP/CategoryLayer.model';
import { CategoryLayerService } from 'src/app/shared/ERP/CategoryLayer.service';

import { CategoryRack } from 'src/app/shared/ERP/CategoryRack.model';
import { CategoryRackService } from 'src/app/shared/ERP/CategoryRack.service';

import { CategoryLocation } from 'src/app/shared/ERP/CategoryLocation.model';
import { CategoryLocationService } from 'src/app/shared/ERP/CategoryLocation.service';

import { CategoryLocationMaterial } from 'src/app/shared/ERP/CategoryLocationMaterial.model';
import { CategoryLocationMaterialService } from 'src/app/shared/ERP/CategoryLocationMaterial.service';

import { CategoryLocationModalComponent } from 'src/app/Category/category-location-modal/category-location-modal.component';

@Component({
  selector: 'app-location-rule',
  templateUrl: './location-rule.component.html',
  styleUrls: ['./location-rule.component.css']
})
export class LocationRuleComponent {

  constructor(
    public Dialog: MatDialog,
    public NotificationService: NotificationService,
    public DownloadService: DownloadService,

    public CategoryLayerService: CategoryLayerService,
    public CategoryRackService: CategoryRackService,
    public CategoryLocationService: CategoryLocationService,
    public CategoryLocationMaterialService: CategoryLocationMaterialService,

  ) {
    this.CategoryLocationSearch();
    this.CategoryLayerSearch();
    this.CategoryRackSearch();
  }

  ngOnInit(): void {

  }
  ngAfterViewInit() {
  }
  CategoryLocationMaterialSearch() {
    this.CategoryLayerService.IsShowLoading = true;
    this.CategoryLocationMaterialService.BaseParameter.Active = true;
    this.CategoryLocationMaterialService.GetByActiveToListAsync().subscribe(
      res => {
        this.CategoryLocationMaterialService.ListFilter = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
      },
      err => {
      },
      () => {
        this.CategoryLayerService.IsShowLoading = false;
      }
    );
  }
  CategoryLocationSearch() {
    this.CategoryLayerService.IsShowLoading = true;
    this.CategoryLocationService.BaseParameter.Active = true;
    this.CategoryLocationService.GetByActiveToListAsync().subscribe(
      res => {
        this.CategoryLocationService.ListFilter = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
        if (this.CategoryLayerService.ListFilter) {
          if (this.CategoryLayerService.ListFilter.length > 0) {
            this.CategoryLocationMaterialSearch();
          }
        }
      },
      err => {
      },
      () => {
        this.CategoryLayerService.IsShowLoading = false;
      }
    );
  }
  CategoryLayerSearch() {
    this.CategoryLayerService.IsShowLoading = true;
    this.CategoryLayerService.BaseParameter.Active = true;
    this.CategoryLayerService.GetByActiveToListAsync().subscribe(
      res => {
        this.CategoryLayerService.ListFilter = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
        if (this.CategoryLayerService.ListFilter) {
          if (this.CategoryLayerService.ListFilter.length > 0) {
            for (let i = 0; i < this.CategoryLayerService.ListFilter.length; i++) {
              this.CategoryLayerService.ListFilter[i].Active = false;
            }
            this.CategoryLayerService.ListFilter[0].Active = true;
            this.CategoryLayerService.BaseParameter.BaseModel = this.CategoryLayerService.ListFilter[0];
          }
        }
      },
      err => {
      },
      () => {
        this.CategoryLayerService.IsShowLoading = false;
      }
    );
  }
  CategoryRackSearch() {
    this.CategoryLayerService.IsShowLoading = true;
    this.CategoryRackService.BaseParameter.Active = true;
    this.CategoryRackService.GetByActiveToListAsync().subscribe(
      res => {
        this.CategoryRackService.ListFilter = (res as BaseResult).List.sort((a, b) => (a.SortOrder < b.SortOrder ? 1 : -1));
      },
      err => {
      },
      () => {
        this.CategoryLayerService.IsShowLoading = false;
      }
    );
  }
  CategoryLayerView(CategoryLayer: CategoryLayer) {
    for (let i = 0; i < this.CategoryLayerService.ListFilter.length; i++) {
      this.CategoryLayerService.ListFilter[i].Active = false;
    }
    CategoryLayer.Active = true;
    this.CategoryLayerService.BaseParameter.BaseModel = CategoryLayer;
  }
  CategoryLocationModal(element: CategoryLocation) {
    this.CategoryLocationService.BaseParameter.BaseModel = element;
    const dialogConfig = new MatDialogConfig();
    dialogConfig.disableClose = true;
    dialogConfig.autoFocus = true;
    dialogConfig.width = environment.DialogConfigWidth;
    const dialog = this.Dialog.open(CategoryLocationModalComponent, dialogConfig);
    dialog.afterClosed().subscribe(() => {
      this.CategoryLocationSearch();

    });
  }
}
