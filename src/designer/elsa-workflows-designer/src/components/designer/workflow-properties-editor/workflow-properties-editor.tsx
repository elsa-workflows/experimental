import {Component, Event, EventEmitter, h, Method, Prop, State} from '@stencil/core';
import {camelCase} from 'lodash';
import WorkflowEditorTunnel from '../state';
import {
  ActionDefinition,
  Activity,
  ActivityDescriptor,
  ActivityInput,
  DefaultActions,
  LiteralExpression, TabChangedArgs,
  TabDefinition, Workflow
} from '../../../models';

export interface WorkflowPropsUpdatedArgs {
  workflow: Workflow;
}

@Component({
  tag: 'elsa-workflow-properties-editor',
})
export class ActivityPropertiesEditor {
  private slideOverPanel: HTMLElsaSlideOverPanelElement;

  @Prop({mutable: true}) workflow?: Workflow;

  @Event() workflowPropsUpdated: EventEmitter<WorkflowPropsUpdatedArgs>;

  @State() private selectedTabIndex: number = 0;

  @Method()
  public async show(): Promise<void> {
    await this.slideOverPanel.show();
  }

  @Method()
  public async hide(): Promise<void> {
    await this.slideOverPanel.hide();
  }

  public render() {
    const workflow = this.workflow;
    const title = 'Workflow';

    const propertiesTab: TabDefinition = {
      displayText: 'Properties',
      content: () => this.renderPropertiesTab()
    };

    const variablesTab: TabDefinition = {
      displayText: 'Variables',
      content: () => this.renderVariablesTab()
    };

    const tabs = [propertiesTab, variablesTab];

    return (
      <elsa-form-panel
        headerText={title} tabs={tabs} selectedTabIndex={this.selectedTabIndex}
        onSelectedTabIndexChanged={e => this.onSelectedTabIndexChanged(e)}/>
    );
  }

  private onSelectedTabIndexChanged = (e: CustomEvent<TabChangedArgs>) => this.selectedTabIndex = e.detail.selectedTabIndex;

  private onPropertyEditorChanged = (apply: (w: Workflow) => void) => {
    const workflow = this.workflow;
    apply(workflow);
    return this.workflowPropsUpdated.emit({workflow});
  }

  private renderPropertiesTab = () => {
    const workflow = this.workflow;

    return <div>
      <div class="p-4">
        <label htmlFor="workflowName">
          Name
        </label>
        <div class="mt-1">
          <input type="text" name="workflowName" id="workflowName" value={workflow.metadata.name}
                 onChange={e => this.onPropertyEditorChanged(wf => wf.metadata.name = (e.target as HTMLInputElement).value)}
                 class="shadow-sm focus:ring-blue-500 focus:border-blue-500 block w-full sm:text-sm border-gray-300 rounded-md"/>
        </div>
        <p class="mt-2 text-sm text-gray-500">The name of the workflow</p>
      </div>
    </div>
  };

  private renderVariablesTab = () => {
    return <div>
      TODO: Variables editor
    </div>
  };
}

WorkflowEditorTunnel.injectProps(ActivityPropertiesEditor, ['activityDescriptors']);
