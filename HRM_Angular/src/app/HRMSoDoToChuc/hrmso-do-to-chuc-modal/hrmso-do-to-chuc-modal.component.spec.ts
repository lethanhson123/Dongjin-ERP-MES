import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HRMSoDoToChucModalComponent } from './hrmso-do-to-chuc-modal.component';

describe('HRMSoDoToChucModalComponent', () => {
  let component: HRMSoDoToChucModalComponent;
  let fixture: ComponentFixture<HRMSoDoToChucModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ HRMSoDoToChucModalComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(HRMSoDoToChucModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
