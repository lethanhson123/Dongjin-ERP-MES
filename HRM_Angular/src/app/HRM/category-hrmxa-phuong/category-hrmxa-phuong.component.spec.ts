import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CategoryHRMXaPhuongComponent } from './category-hrmxa-phuong.component';

describe('CategoryHRMXaPhuongComponent', () => {
  let component: CategoryHRMXaPhuongComponent;
  let fixture: ComponentFixture<CategoryHRMXaPhuongComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CategoryHRMXaPhuongComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CategoryHRMXaPhuongComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
