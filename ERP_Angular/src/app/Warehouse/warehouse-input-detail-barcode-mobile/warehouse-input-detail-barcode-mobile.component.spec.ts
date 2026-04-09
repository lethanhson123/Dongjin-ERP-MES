import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WarehouseInputDetailBarcodeMobileComponent } from './warehouse-input-detail-barcode-mobile.component';

describe('WarehouseInputDetailBarcodeMobileComponent', () => {
  let component: WarehouseInputDetailBarcodeMobileComponent;
  let fixture: ComponentFixture<WarehouseInputDetailBarcodeMobileComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WarehouseInputDetailBarcodeMobileComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(WarehouseInputDetailBarcodeMobileComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
