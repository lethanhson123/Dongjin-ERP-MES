import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WarehouseInputInfoComponent } from './warehouse-input-info.component';

describe('WarehouseInputInfoComponent', () => {
  let component: WarehouseInputInfoComponent;
  let fixture: ComponentFixture<WarehouseInputInfoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WarehouseInputInfoComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(WarehouseInputInfoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
