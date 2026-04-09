import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CategoryLevelComponent } from './category-level.component';

describe('CategoryLevelComponent', () => {
  let component: CategoryLevelComponent;
  let fixture: ComponentFixture<CategoryLevelComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CategoryLevelComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CategoryLevelComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
