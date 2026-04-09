import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WarehouseRequestAdminComponent } from './warehouse-request-admin.component';

describe('WarehouseRequestAdminComponent', () => {
  let component: WarehouseRequestAdminComponent;
  let fixture: ComponentFixture<WarehouseRequestAdminComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WarehouseRequestAdminComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(WarehouseRequestAdminComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
