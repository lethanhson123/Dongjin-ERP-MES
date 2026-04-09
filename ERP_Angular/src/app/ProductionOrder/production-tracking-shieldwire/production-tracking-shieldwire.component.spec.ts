import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductionTrackingSHIELDWIREComponent } from './production-tracking-shieldwire.component';

describe('ProductionTrackingSHIELDWIREComponent', () => {
  let component: ProductionTrackingSHIELDWIREComponent;
  let fixture: ComponentFixture<ProductionTrackingSHIELDWIREComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProductionTrackingSHIELDWIREComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProductionTrackingSHIELDWIREComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
