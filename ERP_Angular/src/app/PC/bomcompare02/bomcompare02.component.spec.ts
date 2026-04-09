import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BOMCompare02Component } from './bomcompare02.component';

describe('BOMCompare02Component', () => {
  let component: BOMCompare02Component;
  let fixture: ComponentFixture<BOMCompare02Component>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ BOMCompare02Component ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BOMCompare02Component);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
