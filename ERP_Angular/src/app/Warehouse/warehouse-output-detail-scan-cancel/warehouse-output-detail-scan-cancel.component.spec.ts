import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WarehouseOutputDetailScanCancelComponent } from './warehouse-output-detail-scan-cancel.component';

describe('WarehouseOutputDetailScanCancelComponent', () => {
  let component: WarehouseOutputDetailScanCancelComponent;
  let fixture: ComponentFixture<WarehouseOutputDetailScanCancelComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WarehouseOutputDetailScanCancelComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(WarehouseOutputDetailScanCancelComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
