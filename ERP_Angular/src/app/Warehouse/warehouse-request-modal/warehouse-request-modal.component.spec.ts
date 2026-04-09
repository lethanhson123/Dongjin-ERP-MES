import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WarehouseRequestModalComponent } from './warehouse-request-modal.component';

describe('WarehouseRequestModalComponent', () => {
  let component: WarehouseRequestModalComponent;
  let fixture: ComponentFixture<WarehouseRequestModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WarehouseRequestModalComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(WarehouseRequestModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
