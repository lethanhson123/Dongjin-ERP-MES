import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductionOrderProductionPlan001Component } from './production-order-production-plan001.component';

describe('ProductionOrderProductionPlan001Component', () => {
  let component: ProductionOrderProductionPlan001Component;
  let fixture: ComponentFixture<ProductionOrderProductionPlan001Component>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProductionOrderProductionPlan001Component ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProductionOrderProductionPlan001Component);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
