import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PagePublicnotesEditComponent } from './page-publicnotes-edit.component';

describe('PagePublicnotesEditComponent', () => {
  let component: PagePublicnotesEditComponent;
  let fixture: ComponentFixture<PagePublicnotesEditComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PagePublicnotesEditComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PagePublicnotesEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
