import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { NotificationService } from 'src/app/shared/Notification.service';
import { DownloadService } from 'src/app/shared/Download.service';

import { CategoryVehicle } from 'src/app/shared/ERP/CategoryVehicle.model';
import { CategoryVehicleService } from 'src/app/shared/ERP/CategoryVehicle.service';

@Component({
  selector: 'app-category-vehicle',
  templateUrl: './category-vehicle.component.html',
  styleUrls: ['./category-vehicle.component.css']
})
export class CategoryVehicleComponent {

@ViewChild('CategoryVehicleSort') CategoryVehicleSort: MatSort;
  @ViewChild('CategoryVehiclePaginator') CategoryVehiclePaginator: MatPaginator;

  constructor(
    public Dialog: MatDialog,
    public NotificationService: NotificationService,
    public DownloadService: DownloadService,

    public CategoryVehicleService: CategoryVehicleService,
  ) { }

  ngOnInit(): void {

  }
  ngAfterViewInit() {
    this.CategoryVehicleSearch();
  }
  CategoryVehicleSearch() {
    this.CategoryVehicleService.SearchAll(this.CategoryVehicleSort, this.CategoryVehiclePaginator);
  }
  CategoryVehicleSave(element: CategoryVehicle) {
    this.CategoryVehicleService.BaseParameter.BaseModel = element;
    this.NotificationService.warn(this.CategoryVehicleService.ComponentSaveAll(this.CategoryVehicleSort, this.CategoryVehiclePaginator));
  }
  CategoryVehicleDelete(element: CategoryVehicle) {
    this.CategoryVehicleService.BaseParameter.ID = element.ID;
    this.NotificationService.warn(this.CategoryVehicleService.ComponentDeleteAll(this.CategoryVehicleSort, this.CategoryVehiclePaginator));
  }
}