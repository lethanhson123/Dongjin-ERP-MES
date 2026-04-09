import { Injectable, ViewChild } from '@angular/core';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { environment } from 'src/environments/environment';
import { NotificationService } from 'src/app/shared/Notification.service';
import { Base } from './Base.model';
import { BaseParameter } from './BaseParameter.model';
import { BaseResult } from './BaseResult.model';
@Injectable({
    providedIn: 'root'
})
export class BaseService {
    DataSource: MatTableDataSource<any>;
    DataSourceFilter: MatTableDataSource<any>;
    DisplayColumns: string[] = ['No', 'ID', 'ParentID', 'ParentName', 'CreateDate', 'CreateUserID', 'CreateUserCode', 'CreateUserName', 'UpdateDate', 'UpdateUserID', 'UpdateUserCode', 'UpdateUserName', 'RowVersion', 'SortOrder', 'Active', 'Code', 'Name', 'Display', 'Description', 'Note', 'FileName', 'Save'];
    DisplayColumns01: string[] = ['No'];
    DisplayColumnsMobile: string[] = ['No'];
    List: Base[] | undefined;
    ListFilter: Base[] | undefined;
    ListFilter01: Base[] | undefined;
    ListFilter02: Base[] | undefined;
    ListFilter03: Base[] | undefined;
    ListYear: Base[] | undefined;
    ListMonth: Base[] | undefined;
    ListDay: Base[] | undefined;
    ListParent: Base[] | undefined;
    ListChild: Base[] | undefined;
    FormData!: Base;
    File: File;
    FileToUpload: FileList
    BaseParameter!: BaseParameter;
    BaseResult!: BaseResult;
    IsShowLoading: boolean = false;
    APIURL: string = environment.APIMasterDataURL;
    APIRootURL: string = environment.APIMasterDataRootURL;
    Controller: string = "Base";
    Headers: HttpHeaders = new HttpHeaders();


    constructor(
        public httpClient: HttpClient
    ) {
        this.FormData = {
        };
        this.BaseResult = {
        };
        this.BaseParameter = {
            ListID: [],
            SearchString: environment.InitializationString,
            SearchStringFilter: environment.InitializationString,
            UserID: environment.InitializationNumber,
            CategoryDepartmentID: environment.DepartmentID,
            CompanyID: environment.InitializationNumber,
            Active: true,
            Active001: true,
            Active002: true,
            Active003: true,
            Year: new Date().getFullYear(),
            Month: new Date().getMonth() + 1,
            Day: new Date().getDate(),
            Date: new Date(),
            DateBegin: new Date(),
            DateEnd: new Date(),
            Action: 1,
        };
        this.BaseParameter.DateEnd.setDate(this.BaseParameter.DateBegin.getDate() + 1);
        this.BaseParameter.BaseModel = {
            ID: environment.InitializationNumber,
        };
        this.List = [];
        this.ListFilter = [];
        this.ListYear = [];
        this.ListMonth = [];
        this.ListDay = [];
        this.ListParent = [];
        this.ListChild = [];
        let token = localStorage.getItem(environment.Token);
        this.Headers = this.Headers.append('Authorization', 'Bearer ' + token);
        this.Initialization();
    }
    InitializationHeaders() {
        if (this.Headers) {
            let Bearer = this.Headers.getAll("Authorization")[0];
            if (Bearer == environment.Bearer) {
                this.Headers = new HttpHeaders();
                this.Headers = this.Headers.append('Authorization', 'Bearer ' + this.FormData.Description);
            }
        }
    }
    Initialization() {
        var UserID = localStorage.getItem(environment.UserID);
        if (UserID) {
            if (this.BaseParameter.MembershipID == null) {
                this.BaseParameter.MembershipID = Number(UserID);
            }
            if (this.FormData) {
                this.FormData.UpdateUserID = Number(UserID);
            }
            if (this.BaseParameter) {
                this.BaseParameter.UpdateUserID = Number(UserID);
                if (this.BaseParameter.BaseModel) {
                    this.BaseParameter.BaseModel.UpdateUserID = Number(UserID);
                }
            }
        }
    }
    Filter(searchString: string) {
        this.List = this.ListFilter;
        if (searchString.length > 0) {
            searchString = searchString.trim();
            searchString = searchString.toLocaleLowerCase();
            this.ListFilter = this.List.filter((item: any) =>
                (item.Name && item.Name.length > 0 && item.Name.toLocaleLowerCase().indexOf(searchString) !== -1)
                || (item.Code && item.Code.length > 0 && item.Code.toLocaleLowerCase().indexOf(searchString) !== -1)
            );
        }
        else {
            this.ListFilter = this.List;
        }
    }
    Filter10(searchString: string) {
        this.ListFilter = [];
        if (searchString.length > 0) {
            searchString = searchString.trim();
            searchString = searchString.toLocaleLowerCase();
            this.ListFilter = this.List.filter((item: any) =>
                (item.Code && item.Code.length > 0 && item.Code.toLocaleLowerCase().indexOf(searchString) !== -1)
            );
        }
        else {
            this.List = this.List.sort((a, b) => (a.Code > b.Code ? 1 : -1));

            for (let i = 0; i < 10; i++) {
                this.ListFilter.push(this.List[i]);
            }
        }
        console.log(this.List);
        console.log(this.ListFilter);
    }
    Filter01(searchString: string) {
        this.List = this.ListFilter;
        if (searchString.length > 0) {
            searchString = searchString.trim();
            searchString = searchString.toLocaleLowerCase();
            this.ListFilter01 = this.List.filter((item: any) =>
                (item.Name && item.Name.length > 0 && item.Name.toLocaleLowerCase().indexOf(searchString) !== -1)
                || (item.Code && item.Code.length > 0 && item.Code.toLocaleLowerCase().indexOf(searchString) !== -1)
            );
        }
        else {
            this.ListFilter01 = this.List;
        }
    }
    Filter02(searchString: string) {
        this.List = this.ListFilter;
        if (searchString.length > 0) {
            searchString = searchString.trim();
            searchString = searchString.toLocaleLowerCase();
            this.ListFilter02 = this.List.filter((item: any) =>
                (item.Name && item.Name.length > 0 && item.Name.toLocaleLowerCase().indexOf(searchString) !== -1)
                || (item.Code && item.Code.length > 0 && item.Code.toLocaleLowerCase().indexOf(searchString) !== -1)
            );
        }
        else {
            this.ListFilter02 = this.List;
        }
    }
    Filter03(searchString: string) {
        this.List = this.ListFilter;
        if (searchString.length > 0) {
            searchString = searchString.trim();
            searchString = searchString.toLocaleLowerCase();
            this.ListFilter03 = this.List.filter((item: any) =>
                (item.Name && item.Name.length > 0 && item.Name.toLocaleLowerCase().indexOf(searchString) !== -1)
                || (item.Code && item.Code.length > 0 && item.Code.toLocaleLowerCase().indexOf(searchString) !== -1)
            );
        }
        else {
            this.ListFilter03 = this.List;
        }
    }
    SearchAll(sort: MatSort, paginator: MatPaginator) {
        if (this.BaseParameter.SearchString.length > 0) {
            this.BaseParameter.SearchString = this.BaseParameter.SearchString.trim();
            if (this.DataSource) {
                this.DataSource.filter = this.BaseParameter.SearchString.toLowerCase();
            }
        }
        else {
            this.SearchGetAllAndEmptyToListAsync(sort, paginator);
        }
    }
    Search(sort: MatSort, paginator: MatPaginator) {
        if (this.BaseParameter.SearchString.length > 0) {
            this.BaseParameter.SearchString = this.BaseParameter.SearchString.trim();
            if (this.DataSource) {
                this.DataSource.filter = this.BaseParameter.SearchString.toLowerCase();
            }
        }
        else {
            this.SearchGetAllToListAsync(sort, paginator);
        }
    }
    ComponentGetAllToListAsync(Service: BaseService) {
        if (this.ListFilter) {
            if (this.ListFilter.length == 0) {
                this.GetAllToListAsync().subscribe(
                    res => {
                        this.ListFilter = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
                    },
                    err => {
                    },
                    () => {
                    }
                );
            }
        }
    }
    ComponentGetByActiveToListAsync(Service: BaseService) {
        if (this.ListFilter) {
            if (this.ListFilter.length == 0) {
                this.GetByActiveToListAsync().subscribe(
                    res => {
                        this.ListFilter = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
                        if (this.ListFilter) {
                            if (this.ListFilter.length > 100) {
                                this.ListFilter01 = this.ListFilter.slice(0, 20);
                                this.ListFilter02 = this.ListFilter.slice(0, 20);
                                this.ListFilter03 = this.ListFilter.slice(0, 20);
                                let List = this.ListFilter.filter(o => o.ID == this.BaseParameter.ID);
                                if (List) {
                                    if (List.length > 0) {
                                        this.ListFilter01.push(List[0]);
                                        this.ListFilter02.push(List[0]);
                                        this.ListFilter03.push(List[0]);
                                    }
                                }
                            }
                            else {
                                this.ListFilter01 = this.ListFilter;
                                this.ListFilter02 = this.ListFilter;
                                this.ListFilter03 = this.ListFilter;
                            }
                        }
                    },
                    err => {
                    },
                    () => {
                    }
                );
            }
        }
    }
    ComponentGetByCompanyIDAndActiveToListAsync(Service: BaseService) {
        if (this.ListFilter) {
            if (this.ListFilter.length == 0) {
                this.GetByCompanyIDAndActiveToListAsync().subscribe(
                    res => {
                        this.ListFilter = (res as BaseResult).List.sort((a, b) => (a.Name > b.Name ? 1 : -1));
                        if (this.ListFilter) {
                            if (this.ListFilter.length > 100) {
                                this.ListFilter01 = this.ListFilter.slice(0, 20);
                                this.ListFilter02 = this.ListFilter.slice(0, 20);
                                this.ListFilter03 = this.ListFilter.slice(0, 20);
                                let List = this.ListFilter.filter(o => o.ID == this.BaseParameter.ID);
                                if (List) {
                                    if (List.length > 0) {
                                        this.ListFilter01.push(List[0]);
                                        this.ListFilter02.push(List[0]);
                                        this.ListFilter03.push(List[0]);
                                    }
                                }
                            }
                            else {
                                this.ListFilter01 = this.ListFilter;
                                this.ListFilter02 = this.ListFilter;
                                this.ListFilter03 = this.ListFilter;
                            }
                        }
                    },
                    err => {
                    },
                    () => {
                    }
                );
            }
        }
    }
    SearchGetAllToListAsync(sort: MatSort, paginator: MatPaginator) {
        this.IsShowLoading = true;
        this.GetAllToListAsync().subscribe(
            res => {
                this.List = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
                this.DataSource = new MatTableDataSource(this.List);
                this.DataSource.sort = sort;
                this.DataSource.paginator = paginator;
            },
            err => {
            },
            () => {
                this.IsShowLoading = false;
            }
        );
    }
    SearchGetAllAndEmptyToListAsync(sort: MatSort, paginator: MatPaginator) {
        this.IsShowLoading = true;
        this.GetAllAndEmptyToListAsync().subscribe(
            res => {
                this.List = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
                console.log(this.List);
                this.ListFilter = this.List;
                this.DataSource = new MatTableDataSource(this.List);
                this.DataSource.sort = sort;
                this.DataSource.paginator = paginator;
            },
            err => {
            },
            () => {
                this.IsShowLoading = false;
            }
        );
    }
    ComponentSaveAll(sort: MatSort, paginator: MatPaginator) {
        this.IsShowLoading = true;
        this.SaveAsync().subscribe(
            res => {
                this.SearchAll(sort, paginator);
                return environment.SaveSuccess;
            },
            err => {
                return environment.SaveNotSuccess;
            },
            () => {
                this.IsShowLoading = false;
            }
        );
        return environment.SaveSuccess;
    }
    ComponentSave(sort: MatSort, paginator: MatPaginator) {
        this.IsShowLoading = true;
        this.SaveAsync().subscribe(
            res => {
                this.Search(sort, paginator);
                return environment.SaveSuccess;
            },
            err => {
                return environment.SaveNotSuccess;
            },
            () => {
                this.IsShowLoading = false;
            }
        );
        return environment.SaveSuccess;
    }
    ComponentDeleteAll(sort: MatSort, paginator: MatPaginator) {
        if (confirm(environment.DeleteConfirm)) {
            this.IsShowLoading = true;
            this.RemoveAsync().subscribe(
                res => {
                    this.SearchAll(sort, paginator);
                    return environment.DeleteSuccess;
                },
                err => {
                    return environment.DeleteNotSuccess;
                },
                () => {
                    this.IsShowLoading = false;
                }
            );
            return environment.DeleteSuccess;
        }
    }
    ComponentDelete(sort: MatSort, paginator: MatPaginator) {
        if (confirm(environment.DeleteConfirm)) {
            this.IsShowLoading = true;
            this.RemoveAsync().subscribe(
                res => {
                    this.Search(sort, paginator);
                    return environment.DeleteSuccess;
                },
                err => {
                    return environment.DeleteNotSuccess;
                },
                () => {
                    this.IsShowLoading = false;
                }
            );
            return environment.DeleteSuccess;
        }
    }
    ComponentGetYearList() {
        if (this.ListYear) {
            if (this.ListYear.length == 0) {
                this.GetYearList().subscribe(
                    res => {
                        this.ListYear = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
                    },
                    err => {
                    },
                    () => {
                    }
                );
            }
            else {
            }
        }
        else {
        }
    }
    ComponentGetMonthList() {
        if (this.ListMonth) {
            if (this.ListMonth.length == 0) {
                this.GetMonthList().subscribe(
                    res => {
                        this.ListMonth = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
                    },
                    err => {
                    },
                    () => {
                    }
                );
            }
            else {
            }
        }
        else {
        }
    }
    ComponentGetDayList() {
        if (this.ListDay) {
            if (this.ListDay.length == 0) {
                this.GetDayList().subscribe(
                    res => {
                        this.ListDay = (res as BaseResult).List.sort((a, b) => (a.SortOrder > b.SortOrder ? 1 : -1));
                    },
                    err => {
                    },
                    () => {
                    }
                );
            }
            else {
            }
        }
        else {
        }
    }
    GetYearList() {
        let url = this.APIURL + this.Controller + '/GetYearList';
        const formUpload: FormData = new FormData();
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    GetMonthList() {
        let url = this.APIURL + this.Controller + '/GetMonthList';
        const formUpload: FormData = new FormData();
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    GetDayList() {
        let url = this.APIURL + this.Controller + '/GetDayList';
        const formUpload: FormData = new FormData();
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    Save() {
        this.Initialization();
        let url = this.APIURL + this.Controller + '/Save';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    SaveAsync() {
        this.Initialization();
        let url = this.APIURL + this.Controller + '/SaveAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    Copy() {
        this.Initialization();
        let url = this.APIURL + this.Controller + '/Copy';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    CopyAsync() {
        this.Initialization();
        let url = this.APIURL + this.Controller + '/CopyAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    Remove() {
        this.Initialization();
        let url = this.APIURL + this.Controller + '/Remove';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    RemoveAsync() {
        this.Initialization();
        let url = this.APIURL + this.Controller + '/RemoveAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    SaveList() {
        this.Initialization();
        let url = this.APIURL + this.Controller + '/SaveList';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    SaveListAsync() {
        this.Initialization();
        let url = this.APIURL + this.Controller + '/SaveListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    GetByID() {
        let url = this.APIURL + this.Controller + '/GetByID';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    GetByIDAsync() {
        let url = this.APIURL + this.Controller + '/GetByIDAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    GetByIDToList() {
        let url = this.APIURL + this.Controller + '/GetByIDToList';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    GetByIDToListAsync() {
        let url = this.APIURL + this.Controller + '/GetByIDToListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    GetByCode() {
        let url = this.APIURL + this.Controller + '/GetByCode';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    GetByCodeAsync() {
        let url = this.APIURL + this.Controller + '/GetByCodeAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    GetByCodeToList() {
        let url = this.APIURL + this.Controller + '/GetByCodeToList';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    GetByCodeToListAsync() {
        let url = this.APIURL + this.Controller + '/GetByCodeToListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }

    GetAllToList() {
        let url = this.APIURL + this.Controller + '/GetAllToList';
        const formUpload: FormData = new FormData();
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    GetAllToListAsync() {
        let url = this.APIURL + this.Controller + '/GetAllToListAsync';
        const formUpload: FormData = new FormData();
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    GetByParentIDToList() {
        let url = this.APIURL + this.Controller + '/GetByParentIDToList';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    GetByParentIDToListAsync() {
        let url = this.APIURL + this.Controller + '/GetByParentIDToListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    GetByParentIDAndActiveToList() {
        let url = this.APIURL + this.Controller + '/GetByParentIDAndActiveToList';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    GetByParentIDAndActiveToListAsync() {
        let url = this.APIURL + this.Controller + '/GetByParentIDAndActiveToListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    GetByCompanyIDToList() {
        let url = this.APIURL + this.Controller + '/GetByCompanyIDToList';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    GetByCompanyIDToListAsync() {
        let url = this.APIURL + this.Controller + '/GetByCompanyIDToListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    GetByCompanyIDAndActiveToList() {
        let url = this.APIURL + this.Controller + '/GetByCompanyIDAndActiveToList';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    GetByCompanyIDAndActiveToListAsync() {
        let url = this.APIURL + this.Controller + '/GetByCompanyIDAndActiveToListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    GetByActiveToList() {
        let url = this.APIURL + this.Controller + '/GetByActiveToList';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    GetByActiveToListAsync() {
        let url = this.APIURL + this.Controller + '/GetByActiveToListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    GetByParentIDAndEmptyToList() {
        let url = this.APIURL + this.Controller + '/GetByParentIDAndEmptyToList';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    GetByParentIDAndEmptyToListAsync() {
        let url = this.APIURL + this.Controller + '/GetByParentIDAndEmptyToListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    GetByCompanyIDAndEmptyToList() {
        let url = this.APIURL + this.Controller + '/GetByCompanyIDAndEmptyToList';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    GetByCompanyIDAndEmptyToListAsync() {
        let url = this.APIURL + this.Controller + '/GetByCompanyIDAndEmptyToListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    GetBySearchStringToList() {
        let url = this.APIURL + this.Controller + '/GetBySearchStringToList';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    GetBySearchStringToListAsync() {
        let url = this.APIURL + this.Controller + '/GetBySearchStringToListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    GetByPageAndPageSizeToList() {
        let url = this.APIURL + this.Controller + '/GetByPageAndPageSizeToList';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    GetByPageAndPageSizeToListAsync() {
        let url = this.APIURL + this.Controller + '/GetByPageAndPageSizeToListAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }

    GetAllAndEmptyToList() {
        let url = this.APIURL + this.Controller + '/GetAllAndEmptyToList';
        const formUpload: FormData = new FormData();
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    GetAllAndEmptyToListAsync() {
        let url = this.APIURL + this.Controller + '/GetAllAndEmptyToListAsync';
        const formUpload: FormData = new FormData();
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    SaveAndUploadFile() {
        this.Initialization();
        let url = this.APIURL + this.Controller + '/SaveAndUploadFile';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        if (this.FileToUpload) {
            if (this.FileToUpload.length > 0) {
                for (var i = 0; i < this.FileToUpload.length; i++) {
                    formUpload.append('file[]', this.FileToUpload[i]);
                }
            }
        }
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    SaveAndUploadFileAsync() {
        this.Initialization();
        let url = this.APIURL + this.Controller + '/SaveAndUploadFileAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        if (this.FileToUpload) {
            if (this.FileToUpload.length > 0) {
                for (var i = 0; i < this.FileToUpload.length; i++) {
                    formUpload.append('file[]', this.FileToUpload[i]);
                }
            }
        }
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    SaveAndUploadFiles() {
        this.Initialization();
        let url = this.APIURL + this.Controller + '/SaveAndUploadFiles';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        if (this.FileToUpload) {
            if (this.FileToUpload.length > 0) {
                for (var i = 0; i < this.FileToUpload.length; i++) {
                    formUpload.append('file[]', this.FileToUpload[i]);
                }
            }
        }
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    SaveAndUploadFilesAsync() {
        this.Initialization();
        let url = this.APIURL + this.Controller + '/SaveAndUploadFilesAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        if (this.FileToUpload) {
            if (this.FileToUpload.length > 0) {
                for (var i = 0; i < this.FileToUpload.length; i++) {
                    formUpload.append('file[]', this.FileToUpload[i]);
                }
            }
        }
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
    SaveAndUploadFilesByCategoryDepartmentAsync() {
        this.Initialization();
        let url = this.APIURL + this.Controller + '/SaveAndUploadFilesByCategoryDepartmentAsync';
        const formUpload: FormData = new FormData();
        formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
        if (this.FileToUpload) {
            if (this.FileToUpload.length > 0) {
                for (var i = 0; i < this.FileToUpload.length; i++) {
                    formUpload.append('file[]', this.FileToUpload[i]);
                }
            }
        }
        return this.httpClient.post(url, formUpload, { headers: this.Headers });
    }
}

