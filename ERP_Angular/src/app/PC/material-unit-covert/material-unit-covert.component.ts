import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { NotificationService } from 'src/app/shared/Notification.service';
import { DownloadService } from 'src/app/shared/Download.service';

import { MaterialUnitCovert } from 'src/app/shared/ERP/MaterialUnitCovert.model';
import { MaterialUnitCovertService } from 'src/app/shared/ERP/MaterialUnitCovert.service';

import { Company } from 'src/app/shared/ERP/Company.model';
import { CompanyService } from 'src/app/shared/ERP/Company.service';

import { CategoryUnit } from 'src/app/shared/ERP/CategoryUnit.model';
import { CategoryUnitService } from 'src/app/shared/ERP/CategoryUnit.service';
import { combineAll } from 'rxjs/operators';

@Component({
  selector: 'app-material-unit-covert',
  templateUrl: './material-unit-covert.component.html',
  styleUrls: ['./material-unit-covert.component.css']
})
export class MaterialUnitCovertComponent {

  @ViewChild('MaterialUnitCovertSort') MaterialUnitCovertSort: MatSort;
  @ViewChild('MaterialUnitCovertPaginator') MaterialUnitCovertPaginator: MatPaginator;

  URLTemplate: string;

  constructor(
    public Dialog: MatDialog,
    public NotificationService: NotificationService,
    public DownloadService: DownloadService,

    public MaterialUnitCovertService: MaterialUnitCovertService,
    public CompanyService: CompanyService,
    public CategoryUnitService: CategoryUnitService,
  ) {
    this.MaterialUnitCovertService.BaseParameter.CompanyID=17;
    this.URLTemplate = this.MaterialUnitCovertService.APIRootURL + "Download/MaterialUnitCovert.xlsx";
    this.CompanySearch();
    this.CategoryUnitSearch();
  }

  ngOnInit(): void {

  }
  ngAfterViewInit() {
    this.MaterialUnitCovertSearch();
  }
  CategoryUnitSearch() {
    this.CategoryUnitService.BaseParameter.Active = true;
    this.CategoryUnitService.ComponentGetByActiveToListAsync(this.MaterialUnitCovertService);
  }
  CompanySearch() {
    this.CompanyService.BaseParameter.Active = true;
    this.CompanyService.ComponentGetByActiveToListAsync(this.MaterialUnitCovertService);
  }
  MaterialUnitCovertSearch() {
    this.MaterialUnitCovertService.SearchAll(this.MaterialUnitCovertSort, this.MaterialUnitCovertPaginator);
  }
  MaterialUnitCovertSave(element: MaterialUnitCovert) {
    this.MaterialUnitCovertService.BaseParameter.BaseModel = element;
    this.NotificationService.warn(this.MaterialUnitCovertService.ComponentSaveAll(this.MaterialUnitCovertSort, this.MaterialUnitCovertPaginator));
  }
  MaterialUnitCovertDelete(element: MaterialUnitCovert) {
    this.MaterialUnitCovertService.BaseParameter.ID = element.ID;
    this.NotificationService.warn(this.MaterialUnitCovertService.ComponentDeleteAll(this.MaterialUnitCovertSort, this.MaterialUnitCovertPaginator));
  }
  MaterialUnitCovertFileChange(event, files: FileList) {
    if (files) {
      this.MaterialUnitCovertService.FileToUpload = files;
      this.MaterialUnitCovertService.BaseParameter.Event = event;
    }
  }
  MaterialUnitCovertSaveAndUploadFile() {
    this.MaterialUnitCovertService.IsShowLoading = true;    
    this.MaterialUnitCovertService.SaveAndUploadFileAsync().subscribe(
      res => {
        this.MaterialUnitCovertService.FileToUpload = null;
        this.MaterialUnitCovertService.BaseParameter.Event = null;
        this.MaterialUnitCovertSearch();
        this.NotificationService.warn(environment.SaveSuccess);
      },
      err => {
        this.NotificationService.warn(environment.SaveNotSuccess);
      },
      () => {
        this.MaterialUnitCovertService.IsShowLoading = false;
      }
    );
  }
}
