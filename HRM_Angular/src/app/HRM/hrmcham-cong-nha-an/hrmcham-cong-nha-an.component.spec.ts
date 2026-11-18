import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HRMChamCongNhaAnComponent } from './hrmcham-cong-nha-an.component';

describe('HRMChamCongNhaAnComponent', () => {
  let component: HRMChamCongNhaAnComponent;
  let fixture: ComponentFixture<HRMChamCongNhaAnComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ HRMChamCongNhaAnComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(HRMChamCongNhaAnComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
