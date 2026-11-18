import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HRMSoDoToChuc001Component } from './hrmso-do-to-chuc001.component';

describe('HRMSoDoToChuc001Component', () => {
  let component: HRMSoDoToChuc001Component;
  let fixture: ComponentFixture<HRMSoDoToChuc001Component>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ HRMSoDoToChuc001Component ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(HRMSoDoToChuc001Component);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
