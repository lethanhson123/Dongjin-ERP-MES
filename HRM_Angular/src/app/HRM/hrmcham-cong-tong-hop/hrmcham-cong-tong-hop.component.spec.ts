import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HRMChamCongTongHopComponent } from './hrmcham-cong-tong-hop.component';

describe('HRMChamCongTongHopComponent', () => {
  let component: HRMChamCongTongHopComponent;
  let fixture: ComponentFixture<HRMChamCongTongHopComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ HRMChamCongTongHopComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(HRMChamCongTongHopComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
