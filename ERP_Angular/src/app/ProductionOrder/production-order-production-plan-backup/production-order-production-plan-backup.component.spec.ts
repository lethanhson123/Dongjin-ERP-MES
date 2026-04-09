import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductionOrderProductionPlanBackupComponent } from './production-order-production-plan-backup.component';

describe('ProductionOrderProductionPlanBackupComponent', () => {
  let component: ProductionOrderProductionPlanBackupComponent;
  let fixture: ComponentFixture<ProductionOrderProductionPlanBackupComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProductionOrderProductionPlanBackupComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ProductionOrderProductionPlanBackupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
