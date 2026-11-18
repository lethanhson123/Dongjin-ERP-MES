import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HrmCaLamViecKeHoachModalComponent } from './hrm-ca-lam-viec-ke-hoach-modal.component';

describe('HrmCaLamViecKeHoachModalComponent', () => {
  let component: HrmCaLamViecKeHoachModalComponent;
  let fixture: ComponentFixture<HrmCaLamViecKeHoachModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ HrmCaLamViecKeHoachModalComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(HrmCaLamViecKeHoachModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
