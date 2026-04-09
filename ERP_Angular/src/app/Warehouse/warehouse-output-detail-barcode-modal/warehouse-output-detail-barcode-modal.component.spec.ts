import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WarehouseOutputDetailBarcodeModalComponent } from './warehouse-output-detail-barcode-modal.component';

describe('WarehouseOutputDetailBarcodeModalComponent', () => {
  let component: WarehouseOutputDetailBarcodeModalComponent;
  let fixture: ComponentFixture<WarehouseOutputDetailBarcodeModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WarehouseOutputDetailBarcodeModalComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(WarehouseOutputDetailBarcodeModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
