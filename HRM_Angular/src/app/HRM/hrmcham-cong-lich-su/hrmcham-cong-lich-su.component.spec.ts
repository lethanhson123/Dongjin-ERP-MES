import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HRMChamCongLichSuComponent } from './hrmcham-cong-lich-su.component';

describe('HRMChamCongLichSuComponent', () => {
  let component: HRMChamCongLichSuComponent;
  let fixture: ComponentFixture<HRMChamCongLichSuComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ HRMChamCongLichSuComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(HRMChamCongLichSuComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
