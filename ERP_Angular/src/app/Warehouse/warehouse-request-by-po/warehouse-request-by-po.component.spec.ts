import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WarehouseRequestByPOComponent } from './warehouse-request-by-po.component';

describe('WarehouseRequestByPOComponent', () => {
  let component: WarehouseRequestByPOComponent;
  let fixture: ComponentFixture<WarehouseRequestByPOComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WarehouseRequestByPOComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(WarehouseRequestByPOComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
