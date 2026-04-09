import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WarehouseStockLongTermComponent } from './warehouse-stock-long-term.component';

describe('WarehouseStockLongTermComponent', () => {
  let component: WarehouseStockLongTermComponent;
  let fixture: ComponentFixture<WarehouseStockLongTermComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WarehouseStockLongTermComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(WarehouseStockLongTermComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
