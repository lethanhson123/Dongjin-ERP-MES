import { Component, OnInit } from '@angular/core';
import { environment } from 'src/environments/environment';
import { NotificationService } from 'src/app/shared/Notification.service';
import { BaseResult } from 'src/app/shared/ERP/BaseResult.model';

import { CategoryMenu } from 'src/app/shared/ERP/CategoryMenu.model';
import { CategoryMenuService } from 'src/app/shared/ERP/CategoryMenu.service';

@Component({
  selector: 'app-homepage',
  templateUrl: './homepage.component.html',
  styleUrls: ['./homepage.component.css']
})
export class HomepageComponent implements OnInit {

  constructor(
    public NotificationService: NotificationService,
    public CategoryMenuService: CategoryMenuService,

  ) {

  }

  ngOnInit(): void {
  }

  MenuClick(itemParent: CategoryMenu) {
    itemParent.Active = !itemParent.Active;
  }
}
