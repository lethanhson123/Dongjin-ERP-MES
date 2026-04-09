import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MembershipHistoryURLComponent } from './membership-history-url.component';

describe('MembershipHistoryURLComponent', () => {
  let component: MembershipHistoryURLComponent;
  let fixture: ComponentFixture<MembershipHistoryURLComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MembershipHistoryURLComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MembershipHistoryURLComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
