import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WarehouseInputAdminComponent } from './warehouse-input-admin.component';

describe('WarehouseInputAdminComponent', () => {
  let component: WarehouseInputAdminComponent;
  let fixture: ComponentFixture<WarehouseInputAdminComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WarehouseInputAdminComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(WarehouseInputAdminComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
