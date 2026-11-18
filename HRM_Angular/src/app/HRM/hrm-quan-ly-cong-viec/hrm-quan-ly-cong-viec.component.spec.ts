import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HrmQuanLyCongViecComponent } from './hrm-quan-ly-cong-viec.component';

describe('HrmQuanLyCongViecComponent', () => {
  let component: HrmQuanLyCongViecComponent;
  let fixture: ComponentFixture<HrmQuanLyCongViecComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ HrmQuanLyCongViecComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(HrmQuanLyCongViecComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
