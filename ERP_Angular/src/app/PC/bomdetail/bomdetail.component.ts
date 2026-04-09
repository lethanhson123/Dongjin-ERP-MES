import { Component, OnInit, ViewChild } from '@angular/core';
import { Router, NavigationEnd, ActivatedRoute } from '@angular/router';
import { MatDialog, MatDialogConfig, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { NotificationService } from 'src/app/shared/Notification.service';
import { DownloadService } from 'src/app/shared/Download.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

import { BOM } from 'src/app/shared/ERP/BOM.model';
import { BOMService } from 'src/app/shared/ERP/BOM.service';

import { BOMDetail } from 'src/app/shared/ERP/BOMDetail.model';
import { BOMDetailService } from 'src/app/shared/ERP/BOMDetail.service';

import { CategoryUnit } from 'src/app/shared/ERP/CategoryUnit.model';
import { CategoryUnitService } from 'src/app/shared/ERP/CategoryUnit.service';
import { MaterialModalComponent } from 'src/app/PC/material-modal/material-modal.component';

import { Material } from 'src/app/shared/ERP/Material.model';
import { MaterialService } from 'src/app/shared/ERP/Material.service';

import { Company } from 'src/app/shared/ERP/Company.model';
import { CompanyService } from 'src/app/shared/ERP/Company.service';

@Component({
  selector: 'app-bomdetail',
  templateUrl: './bomdetail.component.html',
  styleUrls: ['./bomdetail.component.css']
})
export class BOMDetailComponent {

  @ViewChild('BOMDetailSort') BOMDetailSort: MatSort;
  @ViewChild('BOMDetailPaginator') BOMDetailPaginator: MatPaginator;

  constructor(
    public ActiveRouter: ActivatedRoute,
    public Router: Router,
    public Dialog: MatDialog,

    public NotificationService: NotificationService,
    public DownloadService: DownloadService,

    public BOMService: BOMService,
    public BOMDetailService: BOMDetailService,

    public CategoryUnitService: CategoryUnitService,
    public MaterialService: MaterialService,
    public CompanyService: CompanyService,
  ) {
    this.BOMDetailService.BaseParameter.Active = false;
    this.BOMDetailService.BaseParameter.SearchString = environment.InitializationString;
    this.BOMService.BaseParameter.SearchString = environment.InitializationString;
  }

  ngOnInit(): void {

  }
  ngAfterViewInit() {
    this.BOMDetailSearch();
  }

  BOMDetailSearch() {
    this.BOMDetailService.IsShowLoading = true;
    this.BOMDetailService.GetBySearchStringToListAsync().subscribe(
      res => {
        this.BOMDetailService.List = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
        this.BOMDetailService.DataSource = new MatTableDataSource(this.BOMDetailService.List);
        this.BOMDetailService.DataSource.sort = this.BOMDetailSort;
        this.BOMDetailService.DataSource.paginator = this.BOMDetailPaginator;
      },
      err => {
      },
      () => {
        this.BOMDetailService.IsShowLoading = false;
      }
    );
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
  MaterialModal(ID: number) {
    this.BOMService.IsShowLoading = true;
    this.MaterialService.BaseParameter.ID = ID;
    this.MaterialService.GetByIDAsync().subscribe(
      res => {
        this.MaterialService.BaseParameter.BaseModel = (res as BaseResult).BaseModel;
        this.MaterialModalOpen(this.MaterialService.BaseParameter.BaseModel);
      },
      err => {
      },
      () => {
        this.BOMService.IsShowLoading = false;
      }
    );
  }
}
