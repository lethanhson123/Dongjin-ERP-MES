import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WarehouseOutputDetailScanComponent } from './warehouse-output-detail-scan.component';

describe('WarehouseOutputDetailScanComponent', () => {
  let component: WarehouseOutputDetailScanComponent;
  let fixture: ComponentFixture<WarehouseOutputDetailScanComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WarehouseOutputDetailScanComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(WarehouseOutputDetailScanComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
