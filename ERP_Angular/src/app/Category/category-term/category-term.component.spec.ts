import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CategoryTermComponent } from './category-term.component';

describe('CategoryTermComponent', () => {
  let component: CategoryTermComponent;
  let fixture: ComponentFixture<CategoryTermComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CategoryTermComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CategoryTermComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
