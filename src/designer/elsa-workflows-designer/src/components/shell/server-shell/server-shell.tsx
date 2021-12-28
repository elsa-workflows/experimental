import {Component, Element, h, Listen, Prop, Watch} from '@stencil/core';
import 'reflect-metadata';
import {Container} from 'typedi';
import {ElsaApiClientProvider, ElsaClient, SaveWorkflowRequest, ServerSettings} from '../../../services';
import ShellTunnel, {ShellState} from './state';
import {ActivityDescriptor, TriggerDescriptor, Workflow, WorkflowDefinitionSummary} from '../../../models';
import {WorkflowUpdatedArgs} from '../../designer/workflow-editor/workflow-editor';
import {PublishClickedArgs} from "../../toolbar/workflow-publish-button/workflow-publish-button";

@Component({
  tag: 'elsa-server-shell'
})
export class ServerShell {
  private activityDescriptors: Array<ActivityDescriptor>;
  private triggerDescriptors: Array<TriggerDescriptor>;
  private elsaClient: ElsaClient;
  private workflowEditorElement?: HTMLElsaWorkflowEditorElement;

  @Element() private el: HTMLElsaServerShellElement;
  @Prop({attribute: 'server'}) public serverUrl: string;


  @Watch('serverUrl')
  private handleServerUrl(value: string) {
    const settings = Container.get(ServerSettings);
    settings.baseAddress = value;
  }

  @Listen('workflowUpdated')
  private async handleWorkflowUpdated(e: CustomEvent<WorkflowUpdatedArgs>) {
    const workflow = e.detail.workflow;
    await this.saveWorkflow(workflow, false);
  }

  @Listen('workflowDefinitionSelected')
  private async handleWorkflowDefinitionSelected(e: CustomEvent<WorkflowDefinitionSummary>) {
    const workflowEditorElement = this.workflowEditorElement;

    if (!workflowEditorElement)
      return;

    const definitionId = e.detail.definitionId;
    workflowEditorElement.workflow = await this.elsaClient.workflows.get({definitionId});
  }

  @Listen('publishClicked')
  private async handlePublishClicked(e: CustomEvent<PublishClickedArgs>) {
    const workflowEditorElement = this.workflowEditorElement;

    if (!workflowEditorElement)
      return;

    e.detail.begin();
    const workflow = await workflowEditorElement.getWorkflow();
    await this.saveWorkflow(workflow, true);
    e.detail.complete();
  }

  public async componentWillLoad() {
    this.handleServerUrl(this.serverUrl);

    const elsaClientProvider = Container.get(ElsaApiClientProvider);
    this.elsaClient = await elsaClientProvider.getClient();
    this.activityDescriptors = await this.elsaClient.descriptors.activities.list();
    this.triggerDescriptors = await this.elsaClient.descriptors.triggers.list();

    this.workflowEditorElement = this.el.getElementsByTagName('elsa-workflow-editor')[0] as HTMLElsaWorkflowEditorElement;
  }

  public render() {

    const tunnelState: ShellState = {
      activityDescriptors: this.activityDescriptors,
      triggerDescriptors: this.triggerDescriptors
    };

    return <ShellTunnel.Provider state={tunnelState}>
      <slot/>
    </ShellTunnel.Provider>;
  }

  private saveWorkflow = async (workflow: Workflow, publish: boolean): Promise<Workflow> => {
    const request: SaveWorkflowRequest = {
      definitionId: workflow.identity.definitionId,
      name: workflow.metadata.name,
      description: workflow.metadata.description,
      publish: publish,
      triggers: workflow.triggers,
      root: workflow.root
    };

    const updatedWorkflow = await this.elsaClient.workflows.post(request);
    this.workflowEditorElement.workflow = updatedWorkflow;
    return updatedWorkflow;
  }
}
