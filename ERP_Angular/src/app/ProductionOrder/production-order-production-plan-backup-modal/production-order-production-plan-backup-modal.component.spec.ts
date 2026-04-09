import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductionOrderProductionPlanBackupModalComponent } from './production-order-production-plan-backup-modal.component';

describe('ProductionOrderProductionPlanBackupModalComponent', () => {
  let component: ProductionOrderProductionPlanBackupModalComponent;
  let fixture: ComponentFixture<ProductionOrderProductionPlanBackupModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProductionOrderProductionPlanBackupModalComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProductionOrderProductionPlanBackupModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
