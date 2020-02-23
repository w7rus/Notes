import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PageNotesEditComponent } from './page-notes-edit.component';

describe('PageNotesEditComponent', () => {
  let component: PageNotesEditComponent;
  let fixture: ComponentFixture<PageNotesEditComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PageNotesEditComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PageNotesEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
