import { CommonModule } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Connection } from '../../metadata/definitions/connection';
import { ConnectionConfig } from '../../metadata/definitions/connection-config';
import { FieldDescriptor } from '../../metadata/definitions/field-descriptor';
import { FieldDescriptorType } from '../../metadata/enumerations/field-descriptor-type';
import { ServerRepositoryService } from '../../services/server.repository.service';
import { MetadataService } from '../../services/metadata.service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-connection',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule
  ],
  templateUrl: './connection.component.html',
  styleUrls: ['./connection.component.scss'],
})
export class ConnectionComponent implements OnInit {
  private readonly route = inject(ActivatedRoute);
  private readonly serverRepository = inject(ServerRepositoryService);
  private readonly metadataService = inject(MetadataService);

  public connection: Connection = new Connection(Connection.defaultSnapshot());
  public connectionConfig: ConnectionConfig | null = null;
  public readonly fieldDescriptorType = FieldDescriptorType;

  ngOnInit(): void {
    const connectionId = this.route.snapshot.paramMap.get('id');
    if (connectionId) {
      this.serverRepository.getConnection(connectionId).subscribe(connection => {
        if (connection) {
          this.connection = connection;
          this.loadConnectionConfig(connection.configId());
        }
      });
    }
  }

  save(): void {
    this.serverRepository.upsertConnection(this.connection)
      .subscribe(() => {
        this.connection?.markClean();
    });
  }

  private loadConnectionConfig(configId: string): void {
    if (!configId) {
      this.connectionConfig = null;
      return;
    }

    const configs = this.metadataService.connectionConfigs();
    this.connectionConfig = configs.find(config => config.configId === configId) ?? null;

    const authModes = this.connectionConfig?.authModes ?? [];
    if (this.connection && !this.connection.authenticationModeId() && authModes.length > 0) {
      this.connection.authenticationModeId.set(authModes[0].id);
    }
  }

  public get selectedAuthMode() {
    const authModeId = this.connection.authenticationModeId();
    return this.connectionConfig?.authModes.find(mode => mode.id === authModeId);
  }

  public getFieldValue(field: FieldDescriptor): string {
    const value = this.connection.fieldValues().get(field.name);

    if (value != null) {
      return value;
    }

    if (field.defaultValue != null) {
      return String(field.defaultValue);
    }

    return '';
  }

  public onFieldValueChange(field: FieldDescriptor, value: string): void {
    this.connection.fieldValues.update(map => {
      const next = new Map(map);
      next.set(field.name, value ?? '');
      return next;
    });
  }

  public getFieldBooleanValue(field: FieldDescriptor): boolean {
    const value = this.connection.fieldValues().get(field.name);

    if (value != null) {
      return value.toLowerCase() === 'true';
    }

    return field.defaultValue === true;
  }

  public onFieldBooleanChange(field: FieldDescriptor, checked: boolean): void {
    this.connection.fieldValues.update(map => {
      const next = new Map(map);
      next.set(field.name, checked ? 'true' : 'false');
      return next;
    });
  }
}
