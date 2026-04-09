import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductionTracking2026Component } from './production-tracking2026.component';

describe('ProductionTracking2026Component', () => {
  let component: ProductionTracking2026Component;
  let fixture: ComponentFixture<ProductionTracking2026Component>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProductionTracking2026Component ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ProductionTracking2026Component);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
