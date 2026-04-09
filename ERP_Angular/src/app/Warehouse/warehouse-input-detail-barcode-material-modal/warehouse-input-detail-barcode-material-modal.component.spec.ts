import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WarehouseInputDetailBarcodeMaterialModalComponent } from './warehouse-input-detail-barcode-material-modal.component';

describe('WarehouseInputDetailBarcodeMaterialModalComponent', () => {
  let component: WarehouseInputDetailBarcodeMaterialModalComponent;
  let fixture: ComponentFixture<WarehouseInputDetailBarcodeMaterialModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WarehouseInputDetailBarcodeMaterialModalComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(WarehouseInputDetailBarcodeMaterialModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
