import {Component, Element, h, Listen, Prop, Watch} from '@stencil/core';
import 'reflect-metadata';
import {Container} from 'typedi';
import {ElsaApiClientProvider, ElsaClient, ServerSettings} from '../../../services';
import ShellTunnel, {ShellState} from './state';
import {ActivityDescriptor, TriggerDescriptor, WorkflowDefinitionSummary} from '../../../models';
import {WorkflowUpdatedArgs} from '../../designer/workflow-editor/workflow-editor';

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
    await this.elsaClient.workflows.post({
      workflow: e.detail.workflow,
      publish: false
    });
  }

  @Listen('workflowDefinitionSelected')
  private async handleWorkflowDefinitionSelected(e: CustomEvent<WorkflowDefinitionSummary>) {

    const workflowEditorElement = this.workflowEditorElement;

    if(!workflowEditorElement)
      return;

    const id = e.detail.id;
    workflowEditorElement.workflow = await this.elsaClient.workflows.get({id: id});
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
}
