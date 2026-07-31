import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { BaseParameter } from './ERP/BaseParameter.model';
import { DomSanitizer } from '@angular/platform-browser';

@Injectable({
    providedIn: 'root'
})
export class DownloadService {

    BaseParameter!: BaseParameter;
    APIURL: string = environment.APIURL;
    Controller: string = "Download";
    Headers: HttpHeaders = new HttpHeaders();


    IPAddress: string = environment.InitializationString;


    constructor(
        private httpClient: HttpClient,
        public Sanitizer: DomSanitizer,
    ) {
        this.InitializationFormData();
        //this.GetIPAddress();
    }
    InitializationFormData() {
        this.BaseParameter = {
        };

        let token = localStorage.getItem(environment.Token);
        this.Headers = this.Headers.append('Authorization', 'Bearer ' + token);
    }
    HTMLTrusted(value: string) {
        return this.Sanitizer.bypassSecurityTrustHtml(value);
    }
    GetRandomColor(count) {
        var arr = [];
        for (var i = 0; i < count; i++) {
            var letters = '0123456789ABCDEF'.split('');
            var color = '#';
            for (var x = 0; x < 6; x++) {
                color += letters[Math.floor(Math.random() * 16)];
            }
        }
        return color;
    }
    GetPosition(): Promise<any> {
        return new Promise((resolve, reject) => {
            navigator.geolocation.getCurrentPosition(resp => {

                resolve({ lng: resp.coords.longitude, lat: resp.coords.latitude });
            },
                err => {
                    reject(err);
                });
        });
    }
    GetLongitude(Longitude: number) {
        if (Longitude > 180) {
            Longitude = 180;
        }
        if (Longitude < -180) {
            Longitude = -180;
        }
        return Longitude;
    }
    GetLatitude(Latitude: number) {
        if (Latitude > 180) {
            Latitude = 180;
        }
        if (Latitude < -180) {
            Latitude = -180;
        }
        if (Latitude > 90) {
            Latitude = 90;
        }
        if (Latitude < -90) {
            Latitude = -90;
        }
        return Latitude;
    }
    GetIPData() {
        return this.httpClient.get(environment.IPRegistryURL).toPromise();
    }


    // GetIPAddressLocal() {
    //     const os = require('os');
    //     const networkInterfaces = os.networkInterfaces();
    //     const addresses = [];
    //     for (const interfaceName in networkInterfaces) {
    //         const iface = networkInterfaces[interfaceName];
    //         for (const alias of iface) {
    //             if (alias.family === 'IPv4' && !alias.internal) {
    //                 addresses.push(alias.address);
    //             }
    //         }
    //     }
    //     return addresses;
    // }
}

