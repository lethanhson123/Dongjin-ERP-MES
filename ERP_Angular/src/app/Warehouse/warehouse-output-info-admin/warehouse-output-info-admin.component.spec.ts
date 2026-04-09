import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WarehouseOutputInfoAdminComponent } from './warehouse-output-info-admin.component';

describe('WarehouseOutputInfoAdminComponent', () => {
  let component: WarehouseOutputInfoAdminComponent;
  let fixture: ComponentFixture<WarehouseOutputInfoAdminComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WarehouseOutputInfoAdminComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(WarehouseOutputInfoAdminComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
