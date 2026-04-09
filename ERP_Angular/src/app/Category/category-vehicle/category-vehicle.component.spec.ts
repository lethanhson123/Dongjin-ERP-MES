import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CategoryVehicleComponent } from './category-vehicle.component';

describe('CategoryVehicleComponent', () => {
  let component: CategoryVehicleComponent;
  let fixture: ComponentFixture<CategoryVehicleComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CategoryVehicleComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CategoryVehicleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
