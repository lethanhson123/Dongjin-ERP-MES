import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WarehouseOutputReturnComponent } from './warehouse-output-return.component';

describe('WarehouseOutputReturnComponent', () => {
  let component: WarehouseOutputReturnComponent;
  let fixture: ComponentFixture<WarehouseOutputReturnComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WarehouseOutputReturnComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(WarehouseOutputReturnComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
