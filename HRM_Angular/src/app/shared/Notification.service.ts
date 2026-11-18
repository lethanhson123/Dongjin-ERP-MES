import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { MatSnackBar, MatSnackBarConfig } from '@angular/material/snack-bar';
import { environment } from 'src/environments/environment';
import { Notification } from 'src/app/shared/ERP/Notification.model';
import { BaseService } from 'src/app/shared/ERP/Base.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

@Injectable({
  providedIn: 'root'
})
export class NotificationService extends BaseService {

  IsSave: boolean = false;
  IsMobile: boolean = false;
  IsMobileDevice: boolean = false;
  ThanhVienID: number = 0;

  constructor(public httpClient: HttpClient, public snackBar: MatSnackBar) {
    super(httpClient);
    this.Controller = "Notification";
    let windowWidth = Math.floor(window.innerWidth);

    if (windowWidth < 700) {
      this.IsMobile = true;
    }
    this.IsMobileDevice = /iPhone|iPad|iPod|Android/i.test(navigator.userAgent);
    this.IsSave = false;
  }

  config: MatSnackBarConfig = {
    duration: 3000,
    horizontalPosition: 'left',
    verticalPosition: 'bottom'
  }
  success(message: string) {
    this.config['panelClass'] = ['notification', 'success'];
    this.snackBar.open(message, '', this.config);
  }

  warn(message: string) {
    this.config['panelClass'] = ['notification', 'warn'];
    this.snackBar.open(message, '', this.config);
    this.IsSave = true;
  }
  OpenWindowByURL(URLParamater, w = 100, h = 200) {
    var width = screen.width;
    var height = screen.height;
    var width01 = width - w;
    var height01 = height - h;
    var left = (width - width01) / 2;
    var top = (height - height01) / 3;
    var url = URLParamater;
    window.open(url, '_blank', 'location=yes,height=' + height01 + ',width=' + width01 + ',scrollbars=yes,status=yes,toolbar=yes,top=' + top + ',left=' + left + '');
  }
  OpenWindowByURL2025(URLParamater, w = 100, h = 200) {
    var width = screen.width;
    var height = screen.height;
    var width01 = width / 2;
    var height01 = height / 2;
    var left = (width - width01) / 2;
    var top = (height - height01) / 3;
    var url = URLParamater;
    window.open(url, '_blank', 'location=yes,height=' + height01 + ',width=' + width01 + ',scrollbars=yes,status=yes,toolbar=yes,top=' + top + ',left=' + left + '');
  }
  OpenWindowByURLMin(URLParamater, w = 100, h = 200) {
    var width = screen.width;
    var height = screen.height;
    var width01 = 1;
    var height01 = 1;
    var left = width;
    var top = height;
    var url = URLParamater;
    window.open(url, '_blank', 'location=yes,height=' + height01 + ',width=' + width01 + ',scrollbars=yes,status=yes,toolbar=yes,top=' + top + ',left=' + left + '');
  }

  GetByParentIDToListAsync() {
    this.BaseParameter.ParentID = Number(localStorage.getItem(environment.UserID));
    let url = this.APIURL + this.Controller + '/GetByParentIDToListAsync';
    const formUpload: FormData = new FormData();
    formUpload.append('BaseParameter', JSON.stringify(this.BaseParameter));
    return this.httpClient.post(url, formUpload, { headers: this.Headers });
  }
  SoundCallByCount(Count: number) {
    let audio = new Audio("/Media/Success.wav");
    audio.play();
    switch (Count) {
      case 1:
        audio = new Audio("/Media/1.mp3");
        audio.play();
        break;
      case 2:
        audio = new Audio("/Media/2.mp3");
        audio.play();
        break;
      case 3:
        audio = new Audio("/Media/3.mp3");
        audio.play();
        break;
      case 4:
        audio = new Audio("/Media/4.mp3");
        audio.play();
        break;
      case 5:
        audio = new Audio("/Media/5.mp3");
        audio.play();
        break;
      case 6:
        audio = new Audio("/Media/6.mp3");
        audio.play();
        break;
      case 7:
        audio = new Audio("/Media/7.mp3");
        audio.play();
        break;
      case 8:
        audio = new Audio("/Media/8.mp3");
        audio.play();
        break;
      case 9:
        audio = new Audio("/Media/9.mp3");
        audio.play();
        break;
      case 10:
        audio = new Audio("/Media/10.mp3");
        audio.play();
        break;
      case 11:
        audio = new Audio("/Media/11.mp3");
        audio.play();
        break;
      case 12:
        audio = new Audio("/Media/12.mp3");
        audio.play();
        break;
      case 13:
        audio = new Audio("/Media/13.mp3");
        audio.play();
        break;
      case 14:
        audio = new Audio("/Media/14.mp3");
        audio.play();
        break;
      case 15:
        audio = new Audio("/Media/15.mp3");
        audio.play();
        break;
      case 16:
        audio = new Audio("/Media/16.mp3");
        audio.play();
        break;
      case 17:
        audio = new Audio("/Media/17.mp3");
        audio.play();
        break;
      case 18:
        audio = new Audio("/Media/18.mp3");
        audio.play();
        break;
      case 19:
        audio = new Audio("/Media/19.mp3");
        audio.play();
        break;
      case 20:
        audio = new Audio("/Media/20.mp3");
        audio.play();
        break;
      case 21:
        audio = new Audio("/Media/21.mp3");
        audio.play();
        break;
      case 22:
        audio = new Audio("/Media/22.mp3");
        audio.play();
        break;
      case 23:
        audio = new Audio("/Media/23.mp3");
        audio.play();
        break;
      case 24:
        audio = new Audio("/Media/24.mp3");
        audio.play();
        break;
      case 25:
        audio = new Audio("/Media/25.mp3");
        audio.play();
        break;
      case 26:
        audio = new Audio("/Media/26.mp3");
        audio.play();
        break;
      case 27:
        audio = new Audio("/Media/27.mp3");
        audio.play();
        break;
      case 28:
        audio = new Audio("/Media/28.mp3");
        audio.play();
        break;
      case 29:
        audio = new Audio("/Media/29.mp3");
        audio.play();
        break;
      case 30:
        audio = new Audio("/Media/30.mp3");
        audio.play();
        break;
      case 31:
        audio = new Audio("/Media/31.mp3");
        audio.play();
        break;
      case 32:
        audio = new Audio("/Media/32.mp3");
        audio.play();
        break;
      case 33:
        audio = new Audio("/Media/33.mp3");
        audio.play();
        break;
      case 34:
        audio = new Audio("/Media/34.mp3");
        audio.play();
        break;
      case 35:
        audio = new Audio("/Media/35.mp3");
        audio.play();
        break;
      case 36:
        audio = new Audio("/Media/36.mp3");
        audio.play();
        break;
      case 37:
        audio = new Audio("/Media/37.mp3");
        audio.play();
        break;
      case 38:
        audio = new Audio("/Media/38.mp3");
        audio.play();
        break;
      case 39:
        audio = new Audio("/Media/39.mp3");
        audio.play();
        break;
      case 40:
        audio = new Audio("/Media/40.mp3");
        audio.play();
        break;
      case 41:
        audio = new Audio("/Media/41.mp3");
        audio.play();
        break;
      case 42:
        audio = new Audio("/Media/42.mp3");
        audio.play();
        break;
      case 43:
        audio = new Audio("/Media/43.mp3");
        audio.play();
        break;
      case 44:
        audio = new Audio("/Media/44.mp3");
        audio.play();
        break;
      case 45:
        audio = new Audio("/Media/45.mp3");
        audio.play();
        break;
      case 46:
        audio = new Audio("/Media/46.mp3");
        audio.play();
        break;
      case 47:
        audio = new Audio("/Media/47.mp3");
        audio.play();
        break;
      case 48:
        audio = new Audio("/Media/48.mp3");
        audio.play();
        break;
      case 49:
        audio = new Audio("/Media/49.mp3");
        audio.play();
        break;
      case 50:
        audio = new Audio("/Media/50.mp3");
        audio.play();
        break;
      case 51:
        audio = new Audio("/Media/51.mp3");
        audio.play();
        break;
      case 52:
        audio = new Audio("/Media/52.mp3");
        audio.play();
        break;
      case 53:
        audio = new Audio("/Media/53.mp3");
        audio.play();
        break;
      case 54:
        audio = new Audio("/Media/54.mp3");
        audio.play();
        break;
      case 55:
        audio = new Audio("/Media/55.mp3");
        audio.play();
        break;
      case 56:
        audio = new Audio("/Media/56.mp3");
        audio.play();
        break;
      case 57:
        audio = new Audio("/Media/57.mp3");
        audio.play();
        break;
      case 58:
        audio = new Audio("/Media/58.mp3");
        audio.play();
        break;
      case 59:
        audio = new Audio("/Media/59.mp3");
        audio.play();
        break;
      case 60:
        audio = new Audio("/Media/60.mp3");
        audio.play();
        break;
      default:
      // audio = new Audio("/Media/Success.wav");
      // audio.play();
    }
    return Count;
  }
}
