import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WarehouseCheckDJMComponent } from './warehouse-check-djm.component';

describe('WarehouseCheckDJMComponent', () => {
  let component: WarehouseCheckDJMComponent;
  let fixture: ComponentFixture<WarehouseCheckDJMComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WarehouseCheckDJMComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(WarehouseCheckDJMComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
