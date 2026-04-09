import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CategoryMaterialComponent } from './category-material.component';

describe('CategoryMaterialComponent', () => {
  let component: CategoryMaterialComponent;
  let fixture: ComponentFixture<CategoryMaterialComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CategoryMaterialComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CategoryMaterialComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
