import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductionOrderProductionPlanMaterial001Component } from './production-order-production-plan-material001.component';

describe('ProductionOrderProductionPlanMaterial001Component', () => {
  let component: ProductionOrderProductionPlanMaterial001Component;
  let fixture: ComponentFixture<ProductionOrderProductionPlanMaterial001Component>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProductionOrderProductionPlanMaterial001Component ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProductionOrderProductionPlanMaterial001Component);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
