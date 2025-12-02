import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { catchError, map, Observable, of } from 'rxjs';
import { API_URL } from '../components/app/app.tokens';
import { WorkflowEntity, WorkflowSnapshot } from '../entities/definitions/workflow.entity';
import { WorkflowSummaryEntity, WorkflowSummarySnapshot } from '../entities/definitions/workflow.summary.entity';
import { RunProgressModel } from '../pages/workflow/interfaces/run-progress-model';
import { TraceProgressModel } from '../pages/workflow/interfaces/trace-progress-model';
import { ContextEntryListEntity } from '../entities/definitions/context-entry-list.entity';
import { ToastService } from './toast.service';

@Injectable({
  providedIn: 'root',
})
export class ServerRepositoryService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = inject(API_URL);
  private readonly toastService = inject(ToastService);

  public getWorkflows(): Observable<WorkflowSummaryEntity[]> {
    return this.http.get<WorkflowSummarySnapshot[]>(`${this.apiUrl}/api/workflow`).pipe(
      map(snapshots => snapshots.map(WorkflowSummaryEntity.fromSnapshot)),
      catchError((error) => {
        this.notifyError('Loading workflows', error);
        return of([]);
      })
    );
  }

  public getWorkflow(id: string): Observable<WorkflowEntity | null> {
    return this.http.get<WorkflowSnapshot>(`${this.apiUrl}/api/workflow/${id}`).pipe(
      map(snapshot => WorkflowEntity.fromSnapshot(snapshot)),
      catchError((error) => {
        this.notifyError('Loading workflow', error);
        return of(null);
      })
    );
  }

  public upsertWorkflow(workflow: WorkflowEntity): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/api/workflow`, workflow.toSnapshot()).pipe(
      catchError((error) => {
        this.notifyError('Saving workflow', error);
        return of(undefined);
      })
    );
  }

  public runWorkflow(id: string, entryList: ContextEntryListEntity): Observable<string | undefined>  {
    return this.http.post<string>(`${this.apiUrl}/api/workflow/run/${id}`, entryList.toSnapshot()).pipe(
      catchError((error) => {
        this.notifyError('Starting workflow run', error);
        return of(undefined);
      })
    );
  }

  public deleteWorkflow(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/api/workflow/${id}`).pipe(
      catchError((error) => {
        this.notifyError('Deleting workflow', error);
        return of(undefined);
      })
    );
  }

  public getLatestWorkflowRun(id: string): Observable<RunProgressModel | null> {
    return this.http.get<RunProgressModel>(`${this.apiUrl}/api/run/latestforworkflow/${id}`).pipe(
      catchError((error) => {
        this.notifyError('Loading latest workflow run', error);
        return of(null);
      })
    );
  }

  public getRunTraces(id: string): Observable<TraceProgressModel[] | null> {
    return this.http.get<TraceProgressModel[]>(`${this.apiUrl}/api/trace/forrun/${id}`).pipe(
      catchError((error) => {
        this.notifyError('Loading run traces', error);
        return of(null);
      })
    );
  }

  private notifyError(operation: string, error: unknown): void {
    const detail = this.toastService.extractErrorDetail(error);
    const message = detail ? `${operation} failed: ${detail}` : `${operation} failed.`;
    this.toastService.error(message);
  }
}
