import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WarehouseInputInfoStockComponent } from './warehouse-input-info-stock.component';

describe('WarehouseInputInfoStockComponent', () => {
  let component: WarehouseInputInfoStockComponent;
  let fixture: ComponentFixture<WarehouseInputInfoStockComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WarehouseInputInfoStockComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(WarehouseInputInfoStockComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
