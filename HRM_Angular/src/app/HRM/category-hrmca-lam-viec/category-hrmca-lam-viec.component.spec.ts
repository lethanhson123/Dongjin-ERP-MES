import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CategoryHRMCaLamViecComponent } from './category-hrmca-lam-viec.component';

describe('CategoryHRMCaLamViecComponent', () => {
  let component: CategoryHRMCaLamViecComponent;
  let fixture: ComponentFixture<CategoryHRMCaLamViecComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CategoryHRMCaLamViecComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CategoryHRMCaLamViecComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
