import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WarehouseModalComponent } from './warehouse-modal.component';

describe('WarehouseModalComponent', () => {
  let component: WarehouseModalComponent;
  let fixture: ComponentFixture<WarehouseModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WarehouseModalComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(WarehouseModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
