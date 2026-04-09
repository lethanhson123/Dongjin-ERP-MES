import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WarehouseOutputAdminComponent } from './warehouse-output-admin.component';

describe('WarehouseOutputAdminComponent', () => {
  let component: WarehouseOutputAdminComponent;
  let fixture: ComponentFixture<WarehouseOutputAdminComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WarehouseOutputAdminComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(WarehouseOutputAdminComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
