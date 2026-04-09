import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { NotificationService } from 'src/app/shared/Notification.service';
import { DownloadService } from 'src/app/shared/Download.service';

import { Factory } from 'src/app/shared/ERP/Factory.model';
import { FactoryService } from 'src/app/shared/ERP/Factory.service';

@Component({
  selector: 'app-factory',
  templateUrl: './factory.component.html',
  styleUrls: ['./factory.component.css']
})
export class FactoryComponent {

@ViewChild('FactorySort') FactorySort: MatSort;
  @ViewChild('FactoryPaginator') FactoryPaginator: MatPaginator;

  constructor(
    public Dialog: MatDialog,
    public NotificationService: NotificationService,
    public DownloadService: DownloadService,

    public FactoryService: FactoryService,
  ) { }

  ngOnInit(): void {

  }
  ngAfterViewInit() {
    this.FactorySearch();
  }
  FactorySearch() {
    this.FactoryService.SearchAll(this.FactorySort, this.FactoryPaginator);
  }
  FactorySave(element: Factory) {
    this.FactoryService.BaseParameter.BaseModel = element;
    this.NotificationService.warn(this.FactoryService.ComponentSaveAll(this.FactorySort, this.FactoryPaginator));
  }
  FactoryDelete(element: Factory) {
    this.FactoryService.BaseParameter.ID = element.ID;
    this.NotificationService.warn(this.FactoryService.ComponentDeleteAll(this.FactorySort, this.FactoryPaginator));
  }
}

