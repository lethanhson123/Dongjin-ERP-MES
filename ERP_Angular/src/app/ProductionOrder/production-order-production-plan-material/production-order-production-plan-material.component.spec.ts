import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductionOrderProductionPlanMaterialComponent } from './production-order-production-plan-material.component';

describe('ProductionOrderProductionPlanMaterialComponent', () => {
  let component: ProductionOrderProductionPlanMaterialComponent;
  let fixture: ComponentFixture<ProductionOrderProductionPlanMaterialComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProductionOrderProductionPlanMaterialComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProductionOrderProductionPlanMaterialComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
