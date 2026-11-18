import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HrmQuanLyCongViec001Component } from './hrm-quan-ly-cong-viec001.component';

describe('HrmQuanLyCongViec001Component', () => {
  let component: HrmQuanLyCongViec001Component;
  let fixture: ComponentFixture<HrmQuanLyCongViec001Component>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ HrmQuanLyCongViec001Component ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(HrmQuanLyCongViec001Component);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
