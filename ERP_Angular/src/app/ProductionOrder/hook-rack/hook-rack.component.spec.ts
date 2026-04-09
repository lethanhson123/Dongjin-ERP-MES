import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HookRackComponent } from './hook-rack.component';

describe('HookRackComponent', () => {
  let component: HookRackComponent;
  let fixture: ComponentFixture<HookRackComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ HookRackComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(HookRackComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
