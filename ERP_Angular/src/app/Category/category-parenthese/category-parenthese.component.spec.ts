import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CategoryParentheseComponent } from './category-parenthese.component';

describe('CategoryParentheseComponent', () => {
  let component: CategoryParentheseComponent;
  let fixture: ComponentFixture<CategoryParentheseComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CategoryParentheseComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CategoryParentheseComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
