import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CategoryCompanyComponent } from './category-company.component';

describe('CategoryCompanyComponent', () => {
  let component: CategoryCompanyComponent;
  let fixture: ComponentFixture<CategoryCompanyComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CategoryCompanyComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CategoryCompanyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
