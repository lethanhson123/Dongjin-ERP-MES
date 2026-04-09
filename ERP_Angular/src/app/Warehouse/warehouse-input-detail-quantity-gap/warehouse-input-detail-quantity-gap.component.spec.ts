import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WarehouseInputDetailQuantityGAPComponent } from './warehouse-input-detail-quantity-gap.component';

describe('WarehouseInputDetailQuantityGAPComponent', () => {
  let component: WarehouseInputDetailQuantityGAPComponent;
  let fixture: ComponentFixture<WarehouseInputDetailQuantityGAPComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WarehouseInputDetailQuantityGAPComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(WarehouseInputDetailQuantityGAPComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
