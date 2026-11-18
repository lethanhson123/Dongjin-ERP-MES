import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HRMSoDoToChucComponent } from './hrmso-do-to-chuc.component';

describe('HRMSoDoToChucComponent', () => {
  let component: HRMSoDoToChucComponent;
  let fixture: ComponentFixture<HRMSoDoToChucComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ HRMSoDoToChucComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(HRMSoDoToChucComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
