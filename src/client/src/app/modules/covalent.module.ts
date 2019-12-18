import { NgModule } from '@angular/core';

import {
  CovalentMediaModule, CovalentCommonModule, CovalentExpansionPanelModule, CovalentChipsModule, CovalentDialogsModule,
  CovalentFileModule, CovalentPagingModule, CovalentNotificationsModule, CovalentSearchModule, CovalentMenuModule,
  CovalentLoadingModule, TdLoadingService, CovalentLayoutModule, TdDialogService, CovalentDataTableModule, CovalentMessageModule
} from '@covalent/core';

@NgModule({
  exports: [
    CovalentLayoutModule,
    CovalentLoadingModule,
    CovalentMenuModule,
    CovalentSearchModule,
    CovalentNotificationsModule,
    CovalentPagingModule,
    CovalentFileModule,
    CovalentDialogsModule,
    CovalentMediaModule,
    CovalentCommonModule,
    CovalentExpansionPanelModule,
    CovalentChipsModule,
    CovalentDataTableModule,
    CovalentMessageModule,
  ],
  providers: [
    TdDialogService,
  ]
})
export class CovalentModule { }
