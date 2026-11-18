import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HRMChamCongNhaAn001Component } from './hrmcham-cong-nha-an001.component';

describe('HRMChamCongNhaAn001Component', () => {
  let component: HRMChamCongNhaAn001Component;
  let fixture: ComponentFixture<HRMChamCongNhaAn001Component>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ HRMChamCongNhaAn001Component ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(HRMChamCongNhaAn001Component);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
