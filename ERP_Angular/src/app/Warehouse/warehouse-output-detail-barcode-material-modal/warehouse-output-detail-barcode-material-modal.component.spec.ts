import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WarehouseOutputDetailBarcodeMaterialModalComponent } from './warehouse-output-detail-barcode-material-modal.component';

describe('WarehouseOutputDetailBarcodeMaterialModalComponent', () => {
  let component: WarehouseOutputDetailBarcodeMaterialModalComponent;
  let fixture: ComponentFixture<WarehouseOutputDetailBarcodeMaterialModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WarehouseOutputDetailBarcodeMaterialModalComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(WarehouseOutputDetailBarcodeMaterialModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
