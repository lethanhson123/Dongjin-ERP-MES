import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductionTrackingSPSTComponent } from './production-tracking-spst.component';

describe('ProductionTrackingSPSTComponent', () => {
  let component: ProductionTrackingSPSTComponent;
  let fixture: ComponentFixture<ProductionTrackingSPSTComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProductionTrackingSPSTComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProductionTrackingSPSTComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
