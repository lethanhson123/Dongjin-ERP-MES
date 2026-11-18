import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HrmThongKeKpiModalComponent } from './hrm-thong-ke-kpi-modal.component';

describe('HrmThongKeKpiModalComponent', () => {
  let component: HrmThongKeKpiModalComponent;
  let fixture: ComponentFixture<HrmThongKeKpiModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ HrmThongKeKpiModalComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(HrmThongKeKpiModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
