import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductionOrderBOMComponent } from './production-order-bom.component';

describe('ProductionOrderBOMComponent', () => {
  let component: ProductionOrderBOMComponent;
  let fixture: ComponentFixture<ProductionOrderBOMComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProductionOrderBOMComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProductionOrderBOMComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
