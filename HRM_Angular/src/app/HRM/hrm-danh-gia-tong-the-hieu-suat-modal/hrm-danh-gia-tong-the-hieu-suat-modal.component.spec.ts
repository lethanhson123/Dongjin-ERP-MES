import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HrmDanhGiaTongTheHieuSuatModalComponent } from './hrm-danh-gia-tong-the-hieu-suat-modal.component';

describe('HrmDanhGiaTongTheHieuSuatModalComponent', () => {
  let component: HrmDanhGiaTongTheHieuSuatModalComponent;
  let fixture: ComponentFixture<HrmDanhGiaTongTheHieuSuatModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ HrmDanhGiaTongTheHieuSuatModalComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(HrmDanhGiaTongTheHieuSuatModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
