import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WarehouseOutputComponent } from './warehouse-output.component';

describe('WarehouseOutputComponent', () => {
  let component: WarehouseOutputComponent;
  let fixture: ComponentFixture<WarehouseOutputComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WarehouseOutputComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(WarehouseOutputComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
