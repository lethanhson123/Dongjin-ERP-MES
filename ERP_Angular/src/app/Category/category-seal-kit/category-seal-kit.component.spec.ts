import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CategorySealKitComponent } from './category-seal-kit.component';

describe('CategorySealKitComponent', () => {
  let component: CategorySealKitComponent;
  let fixture: ComponentFixture<CategorySealKitComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CategorySealKitComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CategorySealKitComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
