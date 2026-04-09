import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WarehouseInputDetailBarcodeModalComponent } from './warehouse-input-detail-barcode-modal.component';

describe('WarehouseInputDetailBarcodeModalComponent', () => {
  let component: WarehouseInputDetailBarcodeModalComponent;
  let fixture: ComponentFixture<WarehouseInputDetailBarcodeModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WarehouseInputDetailBarcodeModalComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(WarehouseInputDetailBarcodeModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
