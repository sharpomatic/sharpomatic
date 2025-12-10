import { Routes } from '@angular/router';
import { WorkflowsComponent } from '../../pages/workflows/workflows.component';
import { SettingsComponent } from '../../pages/settings/settings.component';
import { WorkflowComponent } from '../../pages/workflow/components/workflow.component';
import { ConnectionsComponent } from '../../pages/connections/connections.component';
import { ConnectionComponent } from '../../pages/connection/connection.component';
import { ModelsComponent } from '../../pages/models/models.component';
import { ModelComponent } from '../../pages/model/model.component';
import { unsavedChangesGuard } from '../../guards/unsaved-changes.guard';

export const routes: Routes = [
  { path: '', redirectTo: '/workflows', pathMatch: 'full' },
  { path: 'workflows', component: WorkflowsComponent },
  { path: 'workflows/:id', component: WorkflowComponent, canDeactivate: [unsavedChangesGuard] },
  { path: 'models/:id', component: ModelComponent, canDeactivate: [unsavedChangesGuard] },
  { path: 'models', component: ModelsComponent },
  { path: 'connections/:id', component: ConnectionComponent, canDeactivate: [unsavedChangesGuard] },
  { path: 'connections', component: ConnectionsComponent },
  { path: 'settings', component: SettingsComponent },
];
