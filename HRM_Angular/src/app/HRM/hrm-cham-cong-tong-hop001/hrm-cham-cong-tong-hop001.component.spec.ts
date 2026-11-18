import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HrmChamCongTongHop001Component } from './hrm-cham-cong-tong-hop001.component';

describe('HrmChamCongTongHop001Component', () => {
  let component: HrmChamCongTongHop001Component;
  let fixture: ComponentFixture<HrmChamCongTongHop001Component>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ HrmChamCongTongHop001Component ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(HrmChamCongTongHop001Component);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
