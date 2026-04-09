import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WarehouseInventoryInvoiceComponent } from './warehouse-inventory-invoice.component';

describe('WarehouseInventoryInvoiceComponent', () => {
  let component: WarehouseInventoryInvoiceComponent;
  let fixture: ComponentFixture<WarehouseInventoryInvoiceComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WarehouseInventoryInvoiceComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(WarehouseInventoryInvoiceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
