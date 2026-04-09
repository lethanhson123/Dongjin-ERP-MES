import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WarehouseOutputDetailBarcodeHistoryModalComponent } from './warehouse-output-detail-barcode-history-modal.component';

describe('WarehouseOutputDetailBarcodeHistoryModalComponent', () => {
  let component: WarehouseOutputDetailBarcodeHistoryModalComponent;
  let fixture: ComponentFixture<WarehouseOutputDetailBarcodeHistoryModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WarehouseOutputDetailBarcodeHistoryModalComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(WarehouseOutputDetailBarcodeHistoryModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
