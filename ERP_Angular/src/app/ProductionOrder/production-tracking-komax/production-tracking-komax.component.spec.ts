import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductionTrackingKOMAXComponent } from './production-tracking-komax.component';

describe('ProductionTrackingKOMAXComponent', () => {
  let component: ProductionTrackingKOMAXComponent;
  let fixture: ComponentFixture<ProductionTrackingKOMAXComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProductionTrackingKOMAXComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProductionTrackingKOMAXComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
