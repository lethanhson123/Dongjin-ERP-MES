import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CategoryDeliveryComponent } from './category-delivery.component';

describe('CategoryDeliveryComponent', () => {
  let component: CategoryDeliveryComponent;
  let fixture: ComponentFixture<CategoryDeliveryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CategoryDeliveryComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CategoryDeliveryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
