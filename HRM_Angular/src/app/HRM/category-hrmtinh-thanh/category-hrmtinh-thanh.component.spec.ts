import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CategoryHRMTinhThanhComponent } from './category-hrmtinh-thanh.component';

describe('CategoryHRMTinhThanhComponent', () => {
  let component: CategoryHRMTinhThanhComponent;
  let fixture: ComponentFixture<CategoryHRMTinhThanhComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CategoryHRMTinhThanhComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CategoryHRMTinhThanhComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
