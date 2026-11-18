import { HttpClient } from '@angular/common/http';
import { TranslateLoader } from '@ngx-translate/core';
import { forkJoin, Observable } from 'rxjs';
import { map } from 'rxjs/operators';


export class MultiTranslateHttpLoader implements TranslateLoader {
  constructor(
    private http: HttpClient,
    private resources: { prefix: string; suffix: string }[],
  ) {}

  getTranslation(lang: string): Observable<any> {
    const requests = this.resources.map((r) =>
      this.http.get(`${r.prefix}${lang}${r.suffix}`),
    );
    return forkJoin(requests).pipe(
      map((responses) => responses.reduce((acc, res) => ({ ...acc, ...res }), {})),
    );
  }
}

export function HttpLoaderFactory(http: HttpClient) {
  return new MultiTranslateHttpLoader(http, [
    { prefix: './assets/i18n/common/common-', suffix: '.json' },
    { prefix: './assets/i18n/membership/membership-', suffix: '.json' },
    { prefix: './assets/i18n/homepage/homepage-', suffix: '.json' },
    { prefix: './assets/i18n/hrm-quan-ly-cong-viec/hrm-quan-ly-cong-viec-', suffix: '.json' },
    { prefix: './assets/i18n/hrm-ca-lam-viec-ke-hoach/hrm-ca-lam-viec-ke-hoach-', suffix: '.json' },
    { prefix: './assets/i18n/hrm-tang-ca/hrm-tang-ca-', suffix: '.json' },
    { prefix: './assets/i18n/hrm-cham-cong-lich-su/hrm-cham-cong-lich-su-', suffix: '.json' },
    { prefix: './assets/i18n/hrm-cham-cong-tong-hop/hrm-cham-cong-tong-hop-', suffix: '.json' },
    { prefix: './assets/i18n/hrm-nghi-phep/hrm-nghi-phep-', suffix: '.json' },
  ]);
}
