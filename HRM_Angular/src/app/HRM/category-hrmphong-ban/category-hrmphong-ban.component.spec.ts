import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CategoryHRMPhongBanComponent } from './category-hrmphong-ban.component';

describe('CategoryHRMPhongBanComponent', () => {
  let component: CategoryHRMPhongBanComponent;
  let fixture: ComponentFixture<CategoryHRMPhongBanComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CategoryHRMPhongBanComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CategoryHRMPhongBanComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
