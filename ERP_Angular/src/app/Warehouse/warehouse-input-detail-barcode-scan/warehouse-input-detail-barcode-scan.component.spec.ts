import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WarehouseInputDetailBarcodeScanComponent } from './warehouse-input-detail-barcode-scan.component';

describe('WarehouseInputDetailBarcodeScanComponent', () => {
  let component: WarehouseInputDetailBarcodeScanComponent;
  let fixture: ComponentFixture<WarehouseInputDetailBarcodeScanComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WarehouseInputDetailBarcodeScanComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(WarehouseInputDetailBarcodeScanComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
