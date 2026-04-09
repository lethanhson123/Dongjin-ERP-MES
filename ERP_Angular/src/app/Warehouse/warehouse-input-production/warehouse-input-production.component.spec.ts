import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WarehouseInputProductionComponent } from './warehouse-input-production.component';

describe('WarehouseInputProductionComponent', () => {
  let component: WarehouseInputProductionComponent;
  let fixture: ComponentFixture<WarehouseInputProductionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WarehouseInputProductionComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(WarehouseInputProductionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
