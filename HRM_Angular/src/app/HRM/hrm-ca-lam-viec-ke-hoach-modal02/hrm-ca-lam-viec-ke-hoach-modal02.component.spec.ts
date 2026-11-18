import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HrmCaLamViecKeHoachModal02Component } from './hrm-ca-lam-viec-ke-hoach-modal02.component';

describe('HrmCaLamViecKeHoachModal02Component', () => {
  let component: HrmCaLamViecKeHoachModal02Component;
  let fixture: ComponentFixture<HrmCaLamViecKeHoachModal02Component>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ HrmCaLamViecKeHoachModal02Component ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(HrmCaLamViecKeHoachModal02Component);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
