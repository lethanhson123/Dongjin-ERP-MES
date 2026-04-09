import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WarehouseOutputModalComponent } from './warehouse-output-modal.component';

describe('WarehouseOutputModalComponent', () => {
  let component: WarehouseOutputModalComponent;
  let fixture: ComponentFixture<WarehouseOutputModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WarehouseOutputModalComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(WarehouseOutputModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
