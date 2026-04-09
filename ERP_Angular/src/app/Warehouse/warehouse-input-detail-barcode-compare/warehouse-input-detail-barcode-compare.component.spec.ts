import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WarehouseInputDetailBarcodeCompareComponent } from './warehouse-input-detail-barcode-compare.component';

describe('WarehouseInputDetailBarcodeCompareComponent', () => {
  let component: WarehouseInputDetailBarcodeCompareComponent;
  let fixture: ComponentFixture<WarehouseInputDetailBarcodeCompareComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WarehouseInputDetailBarcodeCompareComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(WarehouseInputDetailBarcodeCompareComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
