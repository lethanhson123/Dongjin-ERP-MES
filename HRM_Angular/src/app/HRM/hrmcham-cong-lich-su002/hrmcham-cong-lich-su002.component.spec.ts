import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HRMChamCongLichSu002Component } from './hrmcham-cong-lich-su002.component';

describe('HRMChamCongLichSu002Component', () => {
  let component: HRMChamCongLichSu002Component;
  let fixture: ComponentFixture<HRMChamCongLichSu002Component>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ HRMChamCongLichSu002Component ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(HRMChamCongLichSu002Component);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
