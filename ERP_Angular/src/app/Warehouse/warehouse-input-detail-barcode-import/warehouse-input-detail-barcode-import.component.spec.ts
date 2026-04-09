import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WarehouseInputDetailBarcodeImportComponent } from './warehouse-input-detail-barcode-import.component';

describe('WarehouseInputDetailBarcodeImportComponent', () => {
  let component: WarehouseInputDetailBarcodeImportComponent;
  let fixture: ComponentFixture<WarehouseInputDetailBarcodeImportComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WarehouseInputDetailBarcodeImportComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(WarehouseInputDetailBarcodeImportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
