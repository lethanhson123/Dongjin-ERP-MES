import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WarehouseInputDetailBarcodeComponent } from './warehouse-input-detail-barcode.component';

describe('WarehouseInputDetailBarcodeComponent', () => {
  let component: WarehouseInputDetailBarcodeComponent;
  let fixture: ComponentFixture<WarehouseInputDetailBarcodeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WarehouseInputDetailBarcodeComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(WarehouseInputDetailBarcodeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
