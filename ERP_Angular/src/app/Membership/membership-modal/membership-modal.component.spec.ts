import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MembershipModalComponent } from './membership-modal.component';

describe('MembershipModalComponent', () => {
  let component: MembershipModalComponent;
  let fixture: ComponentFixture<MembershipModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MembershipModalComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MembershipModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
