import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WarehouseInputDetailBarcodeComparePartNoComponent } from './warehouse-input-detail-barcode-compare-part-no.component';

describe('WarehouseInputDetailBarcodeComparePartNoComponent', () => {
  let component: WarehouseInputDetailBarcodeComparePartNoComponent;
  let fixture: ComponentFixture<WarehouseInputDetailBarcodeComparePartNoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WarehouseInputDetailBarcodeComparePartNoComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(WarehouseInputDetailBarcodeComparePartNoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
