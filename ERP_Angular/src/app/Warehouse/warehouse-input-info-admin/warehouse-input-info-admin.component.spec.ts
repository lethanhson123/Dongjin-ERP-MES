import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WarehouseInputInfoAdminComponent } from './warehouse-input-info-admin.component';

describe('WarehouseInputInfoAdminComponent', () => {
  let component: WarehouseInputInfoAdminComponent;
  let fixture: ComponentFixture<WarehouseInputInfoAdminComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WarehouseInputInfoAdminComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(WarehouseInputInfoAdminComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
