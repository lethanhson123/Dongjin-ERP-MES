import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { MatTableDataSource } from '@angular/material/table';
import { environment } from 'src/environments/environment';
import { CategoryProduct } from './CategoryProduct.model';
import { BaseService } from './Base.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

@Injectable({
    providedIn: 'root'
})
export class CategoryProductService extends BaseService{
 DisplayColumns001: string[] = ['No', 'ID', 'ParentID', 'ParentName', 'CreateDate', 'CreateUserID', 'CreateUserCode', 'CreateUserName', 'UpdateDate', 'UpdateUserID', 'UpdateUserCode', 'UpdateUserName', 'RowVersion', 'SortOrder', 'Active', 'Code', 'Name', 'Display', 'Description', 'Note', 'FileName', 'Save'];

    List: CategoryProduct[] | undefined;
    ListFilter: CategoryProduct[] | undefined;
    FormData!: CategoryProduct;
    constructor(public httpClient: HttpClient) {
        super(httpClient);
        this.Controller = "CategoryProduct";
    }
}

