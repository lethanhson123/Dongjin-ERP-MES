import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HrmThongKeKPIComponent } from './hrm-thong-ke-kpi.component';

describe('HrmThongKeKPIComponent', () => {
  let component: HrmThongKeKPIComponent;
  let fixture: ComponentFixture<HrmThongKeKPIComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ HrmThongKeKPIComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(HrmThongKeKPIComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
