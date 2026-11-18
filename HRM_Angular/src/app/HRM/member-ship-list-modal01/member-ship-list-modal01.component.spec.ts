import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MemberShipListModal01Component } from './member-ship-list-modal01.component';

describe('MemberShipListModal01Component', () => {
  let component: MemberShipListModal01Component;
  let fixture: ComponentFixture<MemberShipListModal01Component>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MemberShipListModal01Component ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(MemberShipListModal01Component);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
