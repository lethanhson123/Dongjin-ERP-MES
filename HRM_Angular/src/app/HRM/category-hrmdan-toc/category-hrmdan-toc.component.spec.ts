import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CategoryHRMDanTocComponent } from './category-hrmdan-toc.component';

describe('CategoryHRMDanTocComponent', () => {
  let component: CategoryHRMDanTocComponent;
  let fixture: ComponentFixture<CategoryHRMDanTocComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CategoryHRMDanTocComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CategoryHRMDanTocComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
