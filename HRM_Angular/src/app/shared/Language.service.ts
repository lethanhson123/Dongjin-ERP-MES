import { Injectable } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { BehaviorSubject } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class LanguageService {
  // 1 -> en, 2 -> vi, 3 -> kr
  LanguageID: number = 1;
  Language: string = 'en';
  OnLanguageChange: BehaviorSubject<number> = new BehaviorSubject<number>(1);

  constructor(public TranslateService: TranslateService) {
    const saved = localStorage.getItem(environment.LanguageID);
    this.LanguageID = saved ? Number(saved) : 1;
    this.Language = this.GetLanguageCode(this.LanguageID);
    this.TranslateService.addLangs(['en', 'vi', 'kr']);
    this.TranslateService.setDefaultLang('en');
    this.TranslateService.use(this.Language);
    this.OnLanguageChange.next(this.LanguageID);
  }

  GetLanguageCode(languageID: number): string {
    switch (languageID) {
      case 2:
        return 'vi';
      case 3:
        return 'kr';
      default:
        return 'en';
    }
  }

  GetLanguageID(language: string): number {
    switch (language) {
      case 'vi':
        return 2;
      case 'kr':
        return 3;
      default:
        return 1;
    }
  }

  SetLanguage(languageID: number) {
    this.LanguageID = languageID;
    this.Language = this.GetLanguageCode(languageID);
    localStorage.setItem(environment.LanguageID, languageID.toString());
    this.TranslateService.use(this.Language);
    this.OnLanguageChange.next(languageID);
  }
}
