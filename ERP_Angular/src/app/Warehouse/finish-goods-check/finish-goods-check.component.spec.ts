import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FinishGoodsCheckComponent } from './finish-goods-check.component';

describe('FinishGoodsCheckComponent', () => {
  let component: FinishGoodsCheckComponent;
  let fixture: ComponentFixture<FinishGoodsCheckComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ FinishGoodsCheckComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FinishGoodsCheckComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
