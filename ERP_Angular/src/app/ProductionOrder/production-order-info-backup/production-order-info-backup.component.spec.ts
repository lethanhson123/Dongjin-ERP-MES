import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductionOrderInfoBackupComponent } from './production-order-info-backup.component';

describe('ProductionOrderInfoBackupComponent', () => {
  let component: ProductionOrderInfoBackupComponent;
  let fixture: ComponentFixture<ProductionOrderInfoBackupComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProductionOrderInfoBackupComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProductionOrderInfoBackupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
