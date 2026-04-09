import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CategorySystemComponent } from './category-system.component';

describe('CategorySystemComponent', () => {
  let component: CategorySystemComponent;
  let fixture: ComponentFixture<CategorySystemComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CategorySystemComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CategorySystemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
