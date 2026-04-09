import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WarehouseRequestByCategoryDepartmentComponent } from './warehouse-request-by-category-department.component';

describe('WarehouseRequestByCategoryDepartmentComponent', () => {
  let component: WarehouseRequestByCategoryDepartmentComponent;
  let fixture: ComponentFixture<WarehouseRequestByCategoryDepartmentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WarehouseRequestByCategoryDepartmentComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(WarehouseRequestByCategoryDepartmentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
