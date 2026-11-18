import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HRMTangCaOrdersComponent } from './hrm-tang-ca-orders.component';

describe('HRMTangCaOrdersComponent', () => {
  let component: HRMTangCaOrdersComponent;
  let fixture: ComponentFixture<HRMTangCaOrdersComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [HRMTangCaOrdersComponent],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(HRMTangCaOrdersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
