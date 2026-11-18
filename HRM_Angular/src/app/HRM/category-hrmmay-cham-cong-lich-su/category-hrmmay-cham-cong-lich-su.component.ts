import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { NotificationService } from 'src/app/shared/Notification.service';
import { DownloadService } from 'src/app/shared/Download.service';
import { CategoryHRMMayChamCongLichSuService } from 'src/app/shared/ERP/CategoryHRMMayChamCongLichSu.service';
import { CategoryHRMMayChamCongLichSu } from 'src/app/shared/ERP/CategoryHRMMayChamCongLichSu.model';

@Component({
  selector: 'app-category-hrmmay-cham-cong-lich-su',
  templateUrl: './category-hrmmay-cham-cong-lich-su.component.html',
  styleUrls: ['./category-hrmmay-cham-cong-lich-su.component.css']
})
export class CategoryHRMMayChamCongLichSuComponent implements OnInit {

  @ViewChild('CategoryHRMMayChamCongLichSuSearchSort') CategoryHRMMayChamCongLichSuSearchSort: MatSort;
  @ViewChild('CategoryHRMMayChamCongLichSuSearchPaginator') CategoryHRMMayChamCongLichSuSearchPaginator: MatPaginator;

  constructor(
    public Dialog: MatDialog,
    public NotificationService: NotificationService,
    public DownloadService: DownloadService,

    public CategoryHRMMayChamCongLichSuService: CategoryHRMMayChamCongLichSuService,
  ) { }

  ngOnInit(): void {

  }
  ngAfterViewInit() {
    this.CategoryHRMMayChamCongLichSuSearch();
  }

  CategoryHRMMayChamCongLichSuSearch() {
    this.CategoryHRMMayChamCongLichSuService.SearchAll(this.CategoryHRMMayChamCongLichSuSearchSort, this.CategoryHRMMayChamCongLichSuSearchPaginator);
  }
  CategoryHRMMayChamCongLichSuSearchSave(element: CategoryHRMMayChamCongLichSu) {
    this.CategoryHRMMayChamCongLichSuService.BaseParameter.BaseModel = element;
    this.NotificationService.warn(this.CategoryHRMMayChamCongLichSuService.ComponentSaveAll(this.CategoryHRMMayChamCongLichSuSearchSort, this.CategoryHRMMayChamCongLichSuSearchPaginator));
  }
  CategoryHRMMayChamCongLichSuSearchDelete(element: CategoryHRMMayChamCongLichSu) {
    this.CategoryHRMMayChamCongLichSuService.BaseParameter.ID = element.ID;
    this.NotificationService.warn(this.CategoryHRMMayChamCongLichSuService.ComponentDeleteAll(this.CategoryHRMMayChamCongLichSuSearchSort, this.CategoryHRMMayChamCongLichSuSearchPaginator));
  }

}
