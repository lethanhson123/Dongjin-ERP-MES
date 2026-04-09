import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductionTrackingHookRackModalComponent } from './production-tracking-hook-rack-modal.component';

describe('ProductionTrackingHookRackModalComponent', () => {
  let component: ProductionTrackingHookRackModalComponent;
  let fixture: ComponentFixture<ProductionTrackingHookRackModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProductionTrackingHookRackModalComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProductionTrackingHookRackModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
