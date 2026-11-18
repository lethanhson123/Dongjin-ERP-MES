import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CategoryHRMChuyenNganhComponent } from './category-hrmchuyen-nganh.component';

describe('CategoryHRMChuyenNganhComponent', () => {
  let component: CategoryHRMChuyenNganhComponent;
  let fixture: ComponentFixture<CategoryHRMChuyenNganhComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CategoryHRMChuyenNganhComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CategoryHRMChuyenNganhComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
