import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectTaskHistoryComponent } from './project-task-history.component';

describe('ProjectTaskHistoryComponent', () => {
  let component: ProjectTaskHistoryComponent;
  let fixture: ComponentFixture<ProjectTaskHistoryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProjectTaskHistoryComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ProjectTaskHistoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
