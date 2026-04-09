import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WarehouseRequestByCategoryDepartmentInfoComponent } from './warehouse-request-by-category-department-info.component';

describe('WarehouseRequestByCategoryDepartmentInfoComponent', () => {
  let component: WarehouseRequestByCategoryDepartmentInfoComponent;
  let fixture: ComponentFixture<WarehouseRequestByCategoryDepartmentInfoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WarehouseRequestByCategoryDepartmentInfoComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(WarehouseRequestByCategoryDepartmentInfoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
