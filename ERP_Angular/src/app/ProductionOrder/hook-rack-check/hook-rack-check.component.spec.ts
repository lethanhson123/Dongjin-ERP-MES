import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HookRackCheckComponent } from './hook-rack-check.component';

describe('HookRackCheckComponent', () => {
  let component: HookRackCheckComponent;
  let fixture: ComponentFixture<HookRackCheckComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ HookRackCheckComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(HookRackCheckComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
