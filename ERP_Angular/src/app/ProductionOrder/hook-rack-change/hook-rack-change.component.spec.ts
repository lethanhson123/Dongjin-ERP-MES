import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HookRackChangeComponent } from './hook-rack-change.component';

describe('HookRackChangeComponent', () => {
  let component: HookRackChangeComponent;
  let fixture: ComponentFixture<HookRackChangeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ HookRackChangeComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(HookRackChangeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
