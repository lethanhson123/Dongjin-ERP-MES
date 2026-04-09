import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductionOrderProductionPlanSemiComponent } from './production-order-production-plan-semi.component';

describe('ProductionOrderProductionPlanSemiComponent', () => {
  let component: ProductionOrderProductionPlanSemiComponent;
  let fixture: ComponentFixture<ProductionOrderProductionPlanSemiComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProductionOrderProductionPlanSemiComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProductionOrderProductionPlanSemiComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
