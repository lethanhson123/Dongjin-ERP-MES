import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WarehouseInventoryCompanyComponent } from './warehouse-inventory-company.component';

describe('WarehouseInventoryCompanyComponent', () => {
  let component: WarehouseInventoryCompanyComponent;
  let fixture: ComponentFixture<WarehouseInventoryCompanyComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WarehouseInventoryCompanyComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(WarehouseInventoryCompanyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
