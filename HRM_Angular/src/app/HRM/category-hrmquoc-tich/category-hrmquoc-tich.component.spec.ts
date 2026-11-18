import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CategoryHRMQuocTichComponent } from './category-hrmquoc-tich.component';

describe('CategoryHRMQuocTichComponent', () => {
  let component: CategoryHRMQuocTichComponent;
  let fixture: ComponentFixture<CategoryHRMQuocTichComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CategoryHRMQuocTichComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CategoryHRMQuocTichComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
