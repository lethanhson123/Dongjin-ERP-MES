import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HrmCaLamViecKeHoachComponent } from './hrm-ca-lam-viec-ke-hoach.component';

describe('HrmCaLamViecKeHoachComponent', () => {
  let component: HrmCaLamViecKeHoachComponent;
  let fixture: ComponentFixture<HrmCaLamViecKeHoachComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ HrmCaLamViecKeHoachComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(HrmCaLamViecKeHoachComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
