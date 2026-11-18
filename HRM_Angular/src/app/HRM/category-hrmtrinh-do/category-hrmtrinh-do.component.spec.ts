import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CategoryHRMTrinhDoComponent } from './category-hrmtrinh-do.component';

describe('CategoryHRMTrinhDoComponent', () => {
  let component: CategoryHRMTrinhDoComponent;
  let fixture: ComponentFixture<CategoryHRMTrinhDoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CategoryHRMTrinhDoComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CategoryHRMTrinhDoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
