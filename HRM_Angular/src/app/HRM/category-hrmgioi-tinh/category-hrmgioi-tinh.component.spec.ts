import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CategoryHRMGioiTinhComponent } from './category-hrmgioi-tinh.component';

describe('CategoryHRMGioiTinhComponent', () => {
  let component: CategoryHRMGioiTinhComponent;
  let fixture: ComponentFixture<CategoryHRMGioiTinhComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CategoryHRMGioiTinhComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CategoryHRMGioiTinhComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
