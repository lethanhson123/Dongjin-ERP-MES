import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WarehouseInputDetailScanComponent } from './warehouse-input-detail-scan.component';

describe('WarehouseInputDetailScanComponent', () => {
  let component: WarehouseInputDetailScanComponent;
  let fixture: ComponentFixture<WarehouseInputDetailScanComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WarehouseInputDetailScanComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(WarehouseInputDetailScanComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
