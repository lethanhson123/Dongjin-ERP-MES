import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CategoryRackComponent } from './category-rack.component';

describe('CategoryRackComponent', () => {
  let component: CategoryRackComponent;
  let fixture: ComponentFixture<CategoryRackComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CategoryRackComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CategoryRackComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
