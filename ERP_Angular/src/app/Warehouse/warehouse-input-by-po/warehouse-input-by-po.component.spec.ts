import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WarehouseInputByPOComponent } from './warehouse-input-by-po.component';

describe('WarehouseInputByPOComponent', () => {
  let component: WarehouseInputByPOComponent;
  let fixture: ComponentFixture<WarehouseInputByPOComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WarehouseInputByPOComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(WarehouseInputByPOComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
