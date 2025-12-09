import { computed, signal, WritableSignal } from '@angular/core';
import { ConnectorEntity } from './connector.entity';
import { NodeEntity, NodeSnapshot } from './node.entity';
import { NodeType } from '../enumerations/node-type';

export interface ModelCallNodeSnapshot extends NodeSnapshot {
  modelName: string;
  instructions: string;
  prompt: string;
}

export class ModelCallNodeEntity extends NodeEntity<ModelCallNodeSnapshot> {
  public modelName: WritableSignal<string>;
  public instructions: WritableSignal<string>;
  public prompt: WritableSignal<string>;

  constructor(snapshot: ModelCallNodeSnapshot) {
    super(snapshot);

    this.modelName = signal(snapshot.modelName);
    this.instructions = signal(snapshot.instructions);
    this.prompt = signal(snapshot.prompt);

    const baseIsDirty = this.isDirty;
    this.isDirty = computed(() => {
      const snapshot = this.snapshot();

      // Must touch all property signals
      const currentIsDirty = baseIsDirty();
      const currentModelName = this.modelName();
      const currentInstructions = this.instructions();
      const currentPrompt = this.prompt();

      return currentIsDirty ||
        currentModelName !== snapshot.modelName ||
        currentInstructions !== snapshot.instructions ||
        currentPrompt !== snapshot.prompt;
    });
  }

  public override toSnapshot(): ModelCallNodeSnapshot {
    return {
      ...super.toNodeSnapshot(),
      modelName: this.modelName(),
      instructions: this.instructions(),
      prompt: this.prompt(),
    };
  }

  public static fromSnapshot(snapshot: ModelCallNodeSnapshot): ModelCallNodeEntity {
    return new ModelCallNodeEntity(snapshot);
  }

  public static override defaultSnapshot(): ModelCallNodeSnapshot {
    return {
      ...NodeEntity.defaultSnapshot(),
      nodeType: NodeType.ModelCall,
      title: 'Model Call',
      inputs: [ConnectorEntity.defaultSnapshot()],
      outputs: [ConnectorEntity.defaultSnapshot()],
      modelName: '',
      instructions: '',
      prompt: '',
    };
  }

  public static create(top: number, left: number): ModelCallNodeEntity {
    return new ModelCallNodeEntity({
      ...ModelCallNodeEntity.defaultSnapshot(),
      top,
      left,
    });
  }
}
