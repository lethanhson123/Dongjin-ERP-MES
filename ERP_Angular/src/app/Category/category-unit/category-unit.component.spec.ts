import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CategoryUnitComponent } from './category-unit.component';

describe('CategoryUnitComponent', () => {
  let component: CategoryUnitComponent;
  let fixture: ComponentFixture<CategoryUnitComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CategoryUnitComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CategoryUnitComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
