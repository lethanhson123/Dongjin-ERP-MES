import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from 'src/environments/environment';
import { NotificationService } from 'src/app/shared/Notification.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';
import { CategoryHRMPhongBan } from 'src/app/shared/ERP/CategoryHRMPhongBan.model';
import { CategoryHRMPhongBanService } from 'src/app/shared/ERP/CategoryHRMPhongBan.service';

@Component({
  selector: 'app-category-hrmphong-ban',
  templateUrl: './category-hrmphong-ban.component.html',
  styleUrls: ['./category-hrmphong-ban.component.css']
})
export class CategoryHRMPhongBanComponent implements OnInit {
  @ViewChild('CategoryHRMPhongBanSort') CategoryHRMPhongBanSort: MatSort;
  @ViewChild('CategoryHRMPhongBanPaginator') CategoryHRMPhongBanPaginator: MatPaginator;

  constructor(
    public NotificationService: NotificationService,
    public CategoryHRMPhongBanService: CategoryHRMPhongBanService,
  ) { }

  ngOnInit(): void { }

  ngAfterViewInit(): void {
    setTimeout(() => {
      this.CategoryHRMPhongBanSearch();
    });
  }

  CategoryHRMPhongBanSearch(): void {
    this.CategoryHRMPhongBanService.IsShowLoading = true;
    this.CategoryHRMPhongBanService.GetAllAndEmptyToListAsync().subscribe(
      (res) => {
        const list = ((res as BaseResult).List || []) as CategoryHRMPhongBan[];
        this.CategoryHRMPhongBanService.List = list.sort((a, b) => {
          const aIsNew = !a.ID || a.ID === 0;
          const bIsNew = !b.ID || b.ID === 0;
          if (aIsNew && !bIsNew) return -1;
          if (!aIsNew && bIsNew) return 1;
          return (a.SortOrder || 0) - (b.SortOrder || 0);
        });
        this.CategoryHRMPhongBanService.ListFilter = this.CategoryHRMPhongBanService.List;
        this.CategoryHRMPhongBanService.DataSource = new MatTableDataSource(this.CategoryHRMPhongBanService.List);
        this.CategoryHRMPhongBanService.DataSource.sort = this.CategoryHRMPhongBanSort;
        this.CategoryHRMPhongBanService.DataSource.paginator = this.CategoryHRMPhongBanPaginator;
      },
      () => { },
      () => { this.CategoryHRMPhongBanService.IsShowLoading = false; },
    );
  }

  CategoryHRMPhongBanSearchFilter(): void {
    const value = (this.CategoryHRMPhongBanService.BaseParameter.SearchString || '').trim().toLowerCase();
    if (this.CategoryHRMPhongBanService.DataSource) {
      this.CategoryHRMPhongBanService.DataSource.filter = value;
    }
  }

  getParentOptions(element: CategoryHRMPhongBan): CategoryHRMPhongBan[] {
    const list = (this.CategoryHRMPhongBanService.List || [])
      .filter(item => item.ID && item.ID > 0 && item.ID !== element.ID);
    return list.sort((a, b) => this.getParentLabel(a).localeCompare(this.getParentLabel(b)));
  }

  getParentLabel(item: CategoryHRMPhongBan): string {
    const names: string[] = [];
    const visited: number[] = [];
    let current: CategoryHRMPhongBan | undefined = item;
    while (current && current.ID && !visited.includes(current.ID)) {
      visited.push(current.ID);
      names.unshift(current.Name || current.Display || current.Code || String(current.ID));
      current = (this.CategoryHRMPhongBanService.List || []).find(parent => parent.ID === current.ParentID);
    }
    return names.join(' / ');
  }

  CategoryHRMPhongBanParentChange(element: CategoryHRMPhongBan): void {
    const parentID = Number(element.ParentID || 0);
    const parent = (this.CategoryHRMPhongBanService.List || []).find(item => item.ID === parentID);
    element.ParentName = parent ? (parent.Name || parent.Display || parent.Code) : '';
  }

  getParentName(element: CategoryHRMPhongBan): string {
    const parent = (this.CategoryHRMPhongBanService.List || []).find(item => item.ID === element.ParentID);
    return parent ? this.getParentLabel(parent) : '';
  }

  CategoryHRMPhongBanSave(element: CategoryHRMPhongBan): void {
    this.CategoryHRMPhongBanParentChange(element);
    this.CategoryHRMPhongBanService.BaseParameter.BaseModel = { ...element };
    this.CategoryHRMPhongBanService.BaseParameter.BaseModel.ParentID = Number(element.ParentID || 0);
    this.CategoryHRMPhongBanService.IsShowLoading = true;
    this.CategoryHRMPhongBanService.SaveAsync().subscribe(
      () => {
        this.NotificationService.warn(environment.SaveSuccess);
        this.CategoryHRMPhongBanSearch();
      },
      () => { this.NotificationService.warn(environment.SaveNotSuccess); },
      () => { this.CategoryHRMPhongBanService.IsShowLoading = false; },
    );
  }

  CategoryHRMPhongBanDelete(element: CategoryHRMPhongBan): void {
    if (!confirm(environment.DeleteConfirm)) return;
    this.CategoryHRMPhongBanService.BaseParameter.ID = element.ID;
    this.CategoryHRMPhongBanService.IsShowLoading = true;
    this.CategoryHRMPhongBanService.RemoveAsync().subscribe(
      () => {
        this.NotificationService.warn(environment.DeleteSuccess);
        this.CategoryHRMPhongBanSearch();
      },
      () => { this.NotificationService.warn(environment.DeleteNotSuccess); },
      () => { this.CategoryHRMPhongBanService.IsShowLoading = false; },
    );
  }
}
