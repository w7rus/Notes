import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PageNotesListComponent } from './page-notes-list.component';

describe('PageNotesListComponent', () => {
  let component: PageNotesListComponent;
  let fixture: ComponentFixture<PageNotesListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PageNotesListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PageNotesListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
