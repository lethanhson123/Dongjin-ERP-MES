import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HRMNhaAnFileComponent } from './hrmnha-an-file.component';

describe('HRMNhaAnFileComponent', () => {
  let component: HRMNhaAnFileComponent;
  let fixture: ComponentFixture<HRMNhaAnFileComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ HRMNhaAnFileComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(HRMNhaAnFileComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
