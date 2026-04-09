import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WarehouseOutputDetailBarcodeComponent } from './warehouse-output-detail-barcode.component';

describe('WarehouseOutputDetailBarcodeComponent', () => {
  let component: WarehouseOutputDetailBarcodeComponent;
  let fixture: ComponentFixture<WarehouseOutputDetailBarcodeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WarehouseOutputDetailBarcodeComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(WarehouseOutputDetailBarcodeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
