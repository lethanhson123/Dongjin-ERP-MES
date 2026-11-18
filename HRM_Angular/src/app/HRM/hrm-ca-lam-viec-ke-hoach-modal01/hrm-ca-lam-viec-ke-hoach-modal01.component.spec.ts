import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HrmCaLamViecKeHoachModal01Component } from './hrm-ca-lam-viec-ke-hoach-modal01.component';

describe('HrmCaLamViecKeHoachModal01Component', () => {
  let component: HrmCaLamViecKeHoachModal01Component;
  let fixture: ComponentFixture<HrmCaLamViecKeHoachModal01Component>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ HrmCaLamViecKeHoachModal01Component ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(HrmCaLamViecKeHoachModal01Component);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
