import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CategoryHRMMayChamCongLichSuComponent } from './category-hrmmay-cham-cong-lich-su.component';

describe('CategoryHRMMayChamCongLichSuComponent', () => {
  let component: CategoryHRMMayChamCongLichSuComponent;
  let fixture: ComponentFixture<CategoryHRMMayChamCongLichSuComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CategoryHRMMayChamCongLichSuComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CategoryHRMMayChamCongLichSuComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
