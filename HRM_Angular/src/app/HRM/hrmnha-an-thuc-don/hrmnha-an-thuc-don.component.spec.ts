import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HRMNhaAnThucDonComponent } from './hrmnha-an-thuc-don.component';

describe('HRMNhaAnThucDonComponent', () => {
  let component: HRMNhaAnThucDonComponent;
  let fixture: ComponentFixture<HRMNhaAnThucDonComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ HRMNhaAnThucDonComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(HRMNhaAnThucDonComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
