import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HRMTangCaEmployeesComponent } from './hrm-tang-ca-employees.component';

describe('HRMTangCaEmployeesComponent', () => {
  let component: HRMTangCaEmployeesComponent;
  let fixture: ComponentFixture<HRMTangCaEmployeesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [HRMTangCaEmployeesComponent],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(HRMTangCaEmployeesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
