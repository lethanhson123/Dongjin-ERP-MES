import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WarehouseStockHOOKRACKComponent } from './warehouse-stock-hookrack.component';

describe('WarehouseStockHOOKRACKComponent', () => {
  let component: WarehouseStockHOOKRACKComponent;
  let fixture: ComponentFixture<WarehouseStockHOOKRACKComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WarehouseStockHOOKRACKComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(WarehouseStockHOOKRACKComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
