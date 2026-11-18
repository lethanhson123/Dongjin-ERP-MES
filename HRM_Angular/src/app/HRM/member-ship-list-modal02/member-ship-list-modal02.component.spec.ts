import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MemberShipListModal02Component } from './member-ship-list-modal02.component';

describe('MemberShipListModal02Component', () => {
  let component: MemberShipListModal02Component;
  let fixture: ComponentFixture<MemberShipListModal02Component>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MemberShipListModal02Component ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(MemberShipListModal02Component);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
