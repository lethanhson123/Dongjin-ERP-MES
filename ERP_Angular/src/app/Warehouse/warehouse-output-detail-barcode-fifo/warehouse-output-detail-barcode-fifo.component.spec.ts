import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WarehouseOutputDetailBarcodeFIFOComponent } from './warehouse-output-detail-barcode-fifo.component';

describe('WarehouseOutputDetailBarcodeFIFOComponent', () => {
  let component: WarehouseOutputDetailBarcodeFIFOComponent;
  let fixture: ComponentFixture<WarehouseOutputDetailBarcodeFIFOComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WarehouseOutputDetailBarcodeFIFOComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(WarehouseOutputDetailBarcodeFIFOComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
