import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CategoryLocationModalComponent } from './category-location-modal.component';

describe('CategoryLocationModalComponent', () => {
  let component: CategoryLocationModalComponent;
  let fixture: ComponentFixture<CategoryLocationModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CategoryLocationModalComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CategoryLocationModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
