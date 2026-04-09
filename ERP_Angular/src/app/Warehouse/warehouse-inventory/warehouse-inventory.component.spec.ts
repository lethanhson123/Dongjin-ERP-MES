import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WarehouseInventoryComponent } from './warehouse-inventory.component';

describe('WarehouseInventoryComponent', () => {
  let component: WarehouseInventoryComponent;
  let fixture: ComponentFixture<WarehouseInventoryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WarehouseInventoryComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(WarehouseInventoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
