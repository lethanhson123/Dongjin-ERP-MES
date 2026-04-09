import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ZaloTokenComponent } from './zalo-token.component';

describe('ZaloTokenComponent', () => {
  let component: ZaloTokenComponent;
  let fixture: ComponentFixture<ZaloTokenComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ZaloTokenComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ZaloTokenComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
