import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MemberShipModal03Component } from './member-ship-modal03.component';

describe('MemberShipModal03Component', () => {
  let component: MemberShipModal03Component;
  let fixture: ComponentFixture<MemberShipModal03Component>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MemberShipModal03Component ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(MemberShipModal03Component);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
