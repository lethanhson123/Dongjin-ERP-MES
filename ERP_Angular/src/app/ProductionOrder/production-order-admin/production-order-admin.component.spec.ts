import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductionOrderAdminComponent } from './production-order-admin.component';

describe('ProductionOrderAdminComponent', () => {
  let component: ProductionOrderAdminComponent;
  let fixture: ComponentFixture<ProductionOrderAdminComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProductionOrderAdminComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProductionOrderAdminComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
