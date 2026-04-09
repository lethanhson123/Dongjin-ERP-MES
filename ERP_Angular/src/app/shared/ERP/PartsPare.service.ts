import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from 'src/environments/environment';
import { PartsPare } from './PartsPare.model';
import { BaseService } from './Base.service';
@Injectable({
    providedIn: 'root'
})
export class PartsPareService extends BaseService{
 DisplayColumns001: string[] = ['No', 'ID', 'ParentID', 'ParentName', 'CreateDate', 'CreateUserID', 'CreateUserCode', 'CreateUserName', 'UpdateDate', 'UpdateUserID', 'UpdateUserCode', 'UpdateUserName', 'RowVersion', 'SortOrder', 'Active', 'Code', 'Name', 'Display', 'Description', 'Note', 'FileName', 'Price', 'QuantityRequired', 'SafetyStock', 'InventoryWarning', 'Inventory', 'TotalAmount', 'Save'];

    List: PartsPare[] | undefined;
    ListFilter: PartsPare[] | undefined;
    FormData!: PartsPare;
    constructor(public httpClient: HttpClient) {
        super(httpClient);
        this.Controller = "v1/PartsPare";
    }
}

