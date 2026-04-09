import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WarehouseInventoryValueComponent } from './warehouse-inventory-value.component';

describe('WarehouseInventoryValueComponent', () => {
  let component: WarehouseInventoryValueComponent;
  let fixture: ComponentFixture<WarehouseInventoryValueComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WarehouseInventoryValueComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(WarehouseInventoryValueComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
