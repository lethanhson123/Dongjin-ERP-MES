import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WarehouseInputModalComponent } from './warehouse-input-modal.component';

describe('WarehouseInputModalComponent', () => {
  let component: WarehouseInputModalComponent;
  let fixture: ComponentFixture<WarehouseInputModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WarehouseInputModalComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(WarehouseInputModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
