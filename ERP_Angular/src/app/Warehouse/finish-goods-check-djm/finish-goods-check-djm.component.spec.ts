import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FinishGoodsCheckDJMComponent } from './finish-goods-check-djm.component';

describe('FinishGoodsCheckDJMComponent', () => {
  let component: FinishGoodsCheckDJMComponent;
  let fixture: ComponentFixture<FinishGoodsCheckDJMComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ FinishGoodsCheckDJMComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FinishGoodsCheckDJMComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
