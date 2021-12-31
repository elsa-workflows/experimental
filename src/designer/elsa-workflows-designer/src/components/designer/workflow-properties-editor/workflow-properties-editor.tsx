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
import {Hint} from "../../forms/hint";

export interface WorkflowPropsUpdatedArgs {
  workflow: Workflow;
}

@Component({
  tag: 'elsa-workflow-properties-editor',
})
export class WorkflowPropertiesEditor {
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
    const metadata = workflow.metadata;
    const identity = workflow.identity;
    const publication = workflow.publication;

    return <div>
      <div class="p-4">
        <label htmlFor="workflowName">Name</label>
        <div class="mt-1">
          <input type="text" name="workflowName" id="workflowName" value={metadata.name}
                 onChange={e => this.onPropertyEditorChanged(wf => wf.metadata.name = (e.target as HTMLInputElement).value)}/>
        </div>
        <Hint text="The name of the workflow."/>
      </div>
      <div class="p-4">
        <label htmlFor="workflowDescription">Description</label>
        <div class="mt-1">
          <textarea name="workflowDescription" id="workflowDescription" value={metadata.description} rows={6}
                    onChange={e => this.onPropertyEditorChanged(wf => wf.metadata.description = (e.target as HTMLTextAreaElement).value)}/>
        </div>
        <Hint text="A brief description about the workflow."/>
      </div>
      <div class="p-4">
        <div class="max-w-4xl mx-auto">
          <div>
            <div>
              <h3 class="text-lg leading-6 font-medium text-gray-900">
                Workflow Information
              </h3>
            </div>
            <div class="mt-5 border-t border-gray-200">
              <dl class="sm:divide-y sm:divide-gray-200">
                <div class="py-4 sm:py-5 sm:grid sm:grid-cols-3 sm:gap-4">
                  <dt class="text-sm font-medium text-gray-500">
                    ID
                  </dt>
                  <dd class="mt-1 text-sm text-gray-900 sm:mt-0 sm:col-span-2">
                    {identity.id}
                  </dd>
                </div>
                <div class="py-4 sm:py-5 sm:grid sm:grid-cols-3 sm:gap-4">
                  <dt class="text-sm font-medium text-gray-500">
                    Definition ID
                  </dt>
                  <dd class="mt-1 text-sm text-gray-900 sm:mt-0 sm:col-span-2">
                    {identity.definitionId}
                  </dd>
                </div>
                <div class="py-4 sm:py-5 sm:grid sm:grid-cols-3 sm:gap-4">
                  <dt class="text-sm font-medium text-gray-500">
                    Version
                  </dt>
                  <dd class="mt-1 text-sm text-gray-900 sm:mt-0 sm:col-span-2">
                    {identity.version}
                  </dd>
                </div>
                <div class="py-4 sm:py-5 sm:grid sm:grid-cols-3 sm:gap-4">
                  <dt class="text-sm font-medium text-gray-500">
                    Status
                  </dt>
                  <dd class="mt-1 text-sm text-gray-900 sm:mt-0 sm:col-span-2">
                    {publication.isPublished ? 'Published' : 'Draft'}
                  </dd>
                </div>
              </dl>
            </div>
          </div>
        </div>
      </div>
    </div>
  };

  private renderVariablesTab = () => {
    return <div>
      TODO: Variables editor
    </div>
  };
}

WorkflowEditorTunnel.injectProps(WorkflowPropertiesEditor, ['activityDescriptors']);
