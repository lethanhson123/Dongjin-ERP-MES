import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HrmChamCongLichSu001Component } from './hrm-cham-cong-lich-su001.component';

describe('HrmChamCongLichSu001Component', () => {
  let component: HrmChamCongLichSu001Component;
  let fixture: ComponentFixture<HrmChamCongLichSu001Component>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ HrmChamCongLichSu001Component ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(HrmChamCongLichSu001Component);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
