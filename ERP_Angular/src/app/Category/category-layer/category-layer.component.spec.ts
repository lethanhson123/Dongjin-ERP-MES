import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CategoryLayerComponent } from './category-layer.component';

describe('CategoryLayerComponent', () => {
  let component: CategoryLayerComponent;
  let fixture: ComponentFixture<CategoryLayerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CategoryLayerComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CategoryLayerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
