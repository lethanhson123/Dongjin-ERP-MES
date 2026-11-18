import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HRMChamCongLichSuModalComponent } from './hrmcham-cong-lich-su-modal.component';

describe('HRMChamCongLichSuModalComponent', () => {
  let component: HRMChamCongLichSuModalComponent;
  let fixture: ComponentFixture<HRMChamCongLichSuModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ HRMChamCongLichSuModalComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(HRMChamCongLichSuModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
