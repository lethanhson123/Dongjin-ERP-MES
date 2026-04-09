import { ComponentFixture, TestBed } from '@angular/core/testing';

import { STOPLINEComponent } from './stopline.component';

describe('STOPLINEComponent', () => {
  let component: STOPLINEComponent;
  let fixture: ComponentFixture<STOPLINEComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ STOPLINEComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(STOPLINEComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
