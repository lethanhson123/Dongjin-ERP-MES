import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CategoryPositionComponent } from './category-position.component';

describe('CategoryPositionComponent', () => {
  let component: CategoryPositionComponent;
  let fixture: ComponentFixture<CategoryPositionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CategoryPositionComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CategoryPositionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
