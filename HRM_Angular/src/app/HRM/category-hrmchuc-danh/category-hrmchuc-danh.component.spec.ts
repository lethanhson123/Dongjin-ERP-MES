import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CategoryHRMChucDanhComponent } from './category-hrmchuc-danh.component';

describe('CategoryHRMChucDanhComponent', () => {
  let component: CategoryHRMChucDanhComponent;
  let fixture: ComponentFixture<CategoryHRMChucDanhComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CategoryHRMChucDanhComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CategoryHRMChucDanhComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
