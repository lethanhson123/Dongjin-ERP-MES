import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MemberShipListModalComponent } from './member-ship-list-modal.component';

describe('MemberShipListModalComponent', () => {
  let component: MemberShipListModalComponent;
  let fixture: ComponentFixture<MemberShipListModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MemberShipListModalComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(MemberShipListModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
