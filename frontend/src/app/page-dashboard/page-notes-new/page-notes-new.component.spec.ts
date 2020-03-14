import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PageNotesNewComponent } from './page-notes-new.component';

describe('PageNotesNewComponent', () => {
  let component: PageNotesNewComponent;
  let fixture: ComponentFixture<PageNotesNewComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PageNotesNewComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PageNotesNewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
