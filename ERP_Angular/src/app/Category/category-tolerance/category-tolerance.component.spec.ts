import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CategoryToleranceComponent } from './category-tolerance.component';

describe('CategoryToleranceComponent', () => {
  let component: CategoryToleranceComponent;
  let fixture: ComponentFixture<CategoryToleranceComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CategoryToleranceComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CategoryToleranceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
