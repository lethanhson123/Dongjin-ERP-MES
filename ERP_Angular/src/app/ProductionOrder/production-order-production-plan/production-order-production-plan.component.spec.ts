import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductionOrderProductionPlanComponent } from './production-order-production-plan.component';

describe('ProductionOrderProductionPlanComponent', () => {
  let component: ProductionOrderProductionPlanComponent;
  let fixture: ComponentFixture<ProductionOrderProductionPlanComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProductionOrderProductionPlanComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProductionOrderProductionPlanComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
