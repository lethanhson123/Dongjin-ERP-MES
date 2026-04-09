import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CategoryDepartmentComponent } from './category-department.component';

describe('CategoryDepartmentComponent', () => {
  let component: CategoryDepartmentComponent;
  let fixture: ComponentFixture<CategoryDepartmentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CategoryDepartmentComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CategoryDepartmentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
