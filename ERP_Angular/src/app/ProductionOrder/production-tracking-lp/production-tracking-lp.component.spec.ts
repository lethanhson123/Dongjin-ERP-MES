import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductionTrackingLPComponent } from './production-tracking-lp.component';

describe('ProductionTrackingLPComponent', () => {
  let component: ProductionTrackingLPComponent;
  let fixture: ComponentFixture<ProductionTrackingLPComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProductionTrackingLPComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProductionTrackingLPComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
