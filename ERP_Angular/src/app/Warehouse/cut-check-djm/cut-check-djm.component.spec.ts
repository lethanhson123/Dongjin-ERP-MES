import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CutCheckDJMComponent } from './cut-check-djm.component';

describe('CutCheckDJMComponent', () => {
  let component: CutCheckDJMComponent;
  let fixture: ComponentFixture<CutCheckDJMComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CutCheckDJMComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CutCheckDJMComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
