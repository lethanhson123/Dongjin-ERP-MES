import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WarehouseStockByInvoiceComponent } from './warehouse-stock-by-invoice.component';

describe('WarehouseStockByInvoiceComponent', () => {
  let component: WarehouseStockByInvoiceComponent;
  let fixture: ComponentFixture<WarehouseStockByInvoiceComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WarehouseStockByInvoiceComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(WarehouseStockByInvoiceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
