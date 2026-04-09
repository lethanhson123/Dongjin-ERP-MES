import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
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

import { Material } from 'src/app/shared/ERP/Material.model';
import { MaterialService } from 'src/app/shared/ERP/Material.service';
import { MaterialModalComponent } from 'src/app/PC/material-modal/material-modal.component';

import { Company } from 'src/app/shared/ERP/Company.model';
import { CompanyService } from 'src/app/shared/ERP/Company.service';

@Component({
  selector: 'app-bomcompare01',
  templateUrl: './bomcompare01.component.html',
  styleUrls: ['./bomcompare01.component.css']
})
export class BOMCompare01Component {

@ViewChild('BOMDetailSort') BOMDetailSort: MatSort;
  @ViewChild('BOMDetailPaginator') BOMDetailPaginator: MatPaginator;

  @ViewChild('BOMDetailSort02') BOMDetailSort02: MatSort;
  @ViewChild('BOMDetailPaginator02') BOMDetailPaginator02: MatPaginator;

  constructor(
    public Dialog: MatDialog,
    public NotificationService: NotificationService,
    public DownloadService: DownloadService,

    public BOMService: BOMService,
    public BOMDetailService: BOMDetailService,
    public MaterialService: MaterialService,
    public CompanyService: CompanyService,
  ) { 
    this.CompanySearch();
  }

  ngOnInit(): void {

  }
  ngAfterViewInit() {
  }
  CompanySearch() {
    this.CompanyService.ComponentGetByMembershipID_ActiveToListAsync(this.BOMService);
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
  BOMSearch() {
    this.BOMService.IsShowLoading = true;
    this.BOMService.BaseParameter.Code = this.BOMService.BaseParameter.Code.trim();
    this.BOMService.BaseParameter.Name = this.BOMService.BaseParameter.Name.trim();
    this.BOMService.BaseParameter.Display = this.BOMService.BaseParameter.Display.trim();
    this.BOMService.BaseParameter.SearchString = this.BOMService.BaseParameter.Name;
    this.BOMService.GetByCompanyID_Code_MaterialCodeToListAsync().subscribe(
      res => {


        this.BOMDetailService.BaseParameter.ParentID = (res as BaseResult).BaseModel.ID;
        this.BOMDetailService.GetByParentIDToListAsync().subscribe(
          res => {
            this.BOMDetailService.List = (res as BaseResult).List.sort((a, b) => (a.MaterialID02 > b.MaterialID02 ? 1 : -1));

            this.BOMService.BaseParameter.SearchString = this.BOMService.BaseParameter.Display;
            this.BOMService.GetByCompanyID_Code_MaterialCodeToListAsync().subscribe(
              res => {


                this.BOMDetailService.BaseParameter.ParentID = (res as BaseResult).BaseModel.ID;
                this.BOMDetailService.GetByParentIDToListAsync().subscribe(
                  res => {
                    this.BOMDetailService.ListFilter = (res as BaseResult).List.sort((a, b) => (a.MaterialID02 > b.MaterialID02 ? 1 : -1));

                    for (let i = 0; i < this.BOMDetailService.List.length; i++) {
                      this.BOMDetailService.List[i].QuantityCompare = this.BOMDetailService.List[i].QuantityActual;
                      this.BOMDetailService.List[i].Percent = (this.BOMDetailService.List[i].QuantityCompare / this.BOMDetailService.List[i].QuantityActual) * 100;
                    }

                    for (let j = 0; j < this.BOMDetailService.ListFilter.length; j++) {
                      this.BOMDetailService.ListFilter[j].QuantityCompare = this.BOMDetailService.ListFilter[j].QuantityActual;
                      this.BOMDetailService.ListFilter[j].Percent = (this.BOMDetailService.ListFilter[j].QuantityCompare / this.BOMDetailService.ListFilter[j].QuantityActual) * 100;
                    }


                    for (let i = 0; i < this.BOMDetailService.List.length; i++) {
                      for (let j = 0; j < this.BOMDetailService.ListFilter.length; j++) {
                        if (this.BOMDetailService.List[i].MaterialID02 == this.BOMDetailService.ListFilter[j].MaterialID02) {
                          this.BOMDetailService.List[i].QuantityCompare = this.BOMDetailService.ListFilter[j].QuantityActual - this.BOMDetailService.List[i].QuantityActual;
                          this.BOMDetailService.List[i].Percent = (this.BOMDetailService.List[i].QuantityCompare / this.BOMDetailService.List[i].QuantityActual) * 100;

                          this.BOMDetailService.ListFilter[j].QuantityCompare = this.BOMDetailService.List[i].QuantityActual - this.BOMDetailService.ListFilter[j].QuantityActual;
                          this.BOMDetailService.ListFilter[j].Percent = (this.BOMDetailService.ListFilter[j].QuantityCompare / this.BOMDetailService.ListFilter[j].QuantityActual) * 100;
                        }
                      }
                    }

                    this.BOMDetailSearch();

                  },
                  err => {
                  },
                  () => {
                  }
                );
              },
              err => {
              },
              () => {
              }
            );

          },
          err => {
          },
          () => {
          }
        );
      },
      err => {
      },
      () => {
        this.BOMService.IsShowLoading = false;
      }
    );
  }
  BOMDetailSearch() {
    if (this.BOMDetailService.BaseParameter.SearchString.length > 0) {
      this.BOMDetailService.BaseParameter.SearchString = this.BOMDetailService.BaseParameter.SearchString.trim();
      if (this.BOMDetailService.DataSource) {
        this.BOMDetailService.DataSource.filter = this.BOMDetailService.BaseParameter.SearchString.toLowerCase();
      }
      if (this.BOMDetailService.DataSourceFilter) {
        this.BOMDetailService.DataSourceFilter.filter = this.BOMDetailService.BaseParameter.SearchString.toLowerCase();
      }
    }
    else {

      let List01 = this.BOMDetailService.List;
      let List02 = this.BOMDetailService.List;
      if (this.BOMDetailService.BaseParameter.Active == true) {
        List01 = List01.filter(o => o.QuantityCompare > 0);
        List02 = List02.filter(o => o.QuantityCompare > 0);
      }
      List01 = List01.sort((a, b) => (a.MaterialID02 > b.MaterialID02 ? 1 : -1));
      List02 = List02.sort((a, b) => (a.MaterialID02 > b.MaterialID02 ? 1 : -1));

      this.BOMDetailService.DataSource = new MatTableDataSource(List01);
      this.BOMDetailService.DataSource.sort = this.BOMDetailSort;
      this.BOMDetailService.DataSource.paginator = this.BOMDetailPaginator;

      this.BOMDetailService.DataSourceFilter = new MatTableDataSource(List02);
      this.BOMDetailService.DataSourceFilter.sort = this.BOMDetailSort02;
      this.BOMDetailService.DataSourceFilter.paginator = this.BOMDetailPaginator02;
    }
  }
}
