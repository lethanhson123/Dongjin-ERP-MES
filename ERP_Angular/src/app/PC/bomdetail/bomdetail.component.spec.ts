import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BOMDetailComponent } from './bomdetail.component';

describe('BOMDetailComponent', () => {
  let component: BOMDetailComponent;
  let fixture: ComponentFixture<BOMDetailComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ BOMDetailComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BOMDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
