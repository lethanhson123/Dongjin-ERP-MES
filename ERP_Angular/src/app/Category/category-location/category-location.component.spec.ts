import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CategoryLocationComponent } from './category-location.component';

describe('CategoryLocationComponent', () => {
  let component: CategoryLocationComponent;
  let fixture: ComponentFixture<CategoryLocationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CategoryLocationComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CategoryLocationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
