import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { NotificationService } from 'src/app/shared/Notification.service';
import { DownloadService } from 'src/app/shared/Download.service';

import { Module } from 'src/app/shared/ERP/Module.model';
import { ModuleService } from 'src/app/shared/ERP/Module.service';

@Component({
  selector: 'app-module',
  templateUrl: './module.component.html',
  styleUrls: ['./module.component.css']
})
export class ModuleComponent implements OnInit {

@ViewChild('ModuleSort') ModuleSort: MatSort;
  @ViewChild('ModulePaginator') ModulePaginator: MatPaginator;

  constructor(
    public Dialog: MatDialog,
    public NotificationService: NotificationService,
    public DownloadService: DownloadService,

    public ModuleService: ModuleService,
  ) { }

  ngOnInit(): void {

  }
  ngAfterViewInit() {
    this.ModuleSearch();
  }
  ModuleSearch() {
    this.ModuleService.SearchAll(this.ModuleSort, this.ModulePaginator);
  }
  ModuleSave(element: Module) {
    this.ModuleService.BaseParameter.BaseModel = element;
    this.NotificationService.warn(this.ModuleService.ComponentSaveAll(this.ModuleSort, this.ModulePaginator));
  }
  ModuleDelete(element: Module) {
    this.ModuleService.BaseParameter.ID = element.ID;
    this.NotificationService.warn(this.ModuleService.ComponentDeleteAll(this.ModuleSort, this.ModulePaginator));
  }
}