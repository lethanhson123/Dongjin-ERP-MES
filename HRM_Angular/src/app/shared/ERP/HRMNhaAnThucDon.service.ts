import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from 'src/environments/environment';
import { HRMNhaAnThucDon } from './HRMNhaAnThucDon.model';
import { BaseService } from './Base.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

@Injectable({
  providedIn: 'root',
})
export class HRMNhaAnThucDonService extends BaseService {
  DisplayColumns001: string[] = [
    'No',
    'ID',
    'UpdateUserCode',       
    'SortOrder', 
    'UpdateDate',         
    'FileName',        
    'Save',
  ];

  List: HRMNhaAnThucDon[] | undefined;
  ListFilter: HRMNhaAnThucDon[] | undefined;
  FormData!: HRMNhaAnThucDon;
  constructor(public httpClient: HttpClient) {
    super(httpClient);
    this.Controller = 'HRMNhaAnThucDon';
  }
}
