import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HrmQuanLyCongViecModalComponent } from './hrm-quan-ly-cong-viec-modal.component';

describe('HrmQuanLyCongViecModalComponent', () => {
  let component: HrmQuanLyCongViecModalComponent;
  let fixture: ComponentFixture<HrmQuanLyCongViecModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ HrmQuanLyCongViecModalComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(HrmQuanLyCongViecModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
