import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductionTrackingHOOKRACKComponent } from './production-tracking-hookrack.component';

describe('ProductionTrackingHOOKRACKComponent', () => {
  let component: ProductionTrackingHOOKRACKComponent;
  let fixture: ComponentFixture<ProductionTrackingHOOKRACKComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProductionTrackingHOOKRACKComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProductionTrackingHOOKRACKComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
