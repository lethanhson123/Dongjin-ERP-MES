import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CategoryHRMSuatAnComponent } from './category-hrmsuat-an.component';

describe('CategoryHRMSuatAnComponent', () => {
  let component: CategoryHRMSuatAnComponent;
  let fixture: ComponentFixture<CategoryHRMSuatAnComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CategoryHRMSuatAnComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CategoryHRMSuatAnComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
