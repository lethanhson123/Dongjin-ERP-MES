import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HrmDanhGiaTongTheHieuSuatComponent } from './hrm-danh-gia-tong-the-hieu-suat.component';

describe('HrmDanhGiaTongTheHieuSuatComponent', () => {
  let component: HrmDanhGiaTongTheHieuSuatComponent;
  let fixture: ComponentFixture<HrmDanhGiaTongTheHieuSuatComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ HrmDanhGiaTongTheHieuSuatComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(HrmDanhGiaTongTheHieuSuatComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
