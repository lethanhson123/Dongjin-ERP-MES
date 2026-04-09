import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MaterialUnitCovertComponent } from './material-unit-covert.component';

describe('MaterialUnitCovertComponent', () => {
  let component: MaterialUnitCovertComponent;
  let fixture: ComponentFixture<MaterialUnitCovertComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MaterialUnitCovertComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MaterialUnitCovertComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
