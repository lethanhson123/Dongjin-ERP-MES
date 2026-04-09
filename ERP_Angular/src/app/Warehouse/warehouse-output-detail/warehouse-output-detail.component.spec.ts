import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WarehouseOutputDetailComponent } from './warehouse-output-detail.component';

describe('WarehouseOutputDetailComponent', () => {
  let component: WarehouseOutputDetailComponent;
  let fixture: ComponentFixture<WarehouseOutputDetailComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WarehouseOutputDetailComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(WarehouseOutputDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
