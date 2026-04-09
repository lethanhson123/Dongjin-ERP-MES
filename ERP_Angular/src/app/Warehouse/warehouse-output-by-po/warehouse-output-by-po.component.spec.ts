import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WarehouseOutputByPOComponent } from './warehouse-output-by-po.component';

describe('WarehouseOutputByPOComponent', () => {
  let component: WarehouseOutputByPOComponent;
  let fixture: ComponentFixture<WarehouseOutputByPOComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WarehouseOutputByPOComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(WarehouseOutputByPOComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
