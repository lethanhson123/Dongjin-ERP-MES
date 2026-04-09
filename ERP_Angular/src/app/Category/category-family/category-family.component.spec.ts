import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CategoryFamilyComponent } from './category-family.component';

describe('CategoryFamilyComponent', () => {
  let component: CategoryFamilyComponent;
  let fixture: ComponentFixture<CategoryFamilyComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CategoryFamilyComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CategoryFamilyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
