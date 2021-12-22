import {Component, Event, EventEmitter, h, Host, Prop, State, Watch} from '@stencil/core';
import {PagedList, VersionOptions, WorkflowDefinitionSummary} from "../../../models";
import {Container} from "typedi";
import {ElsaApiClientProvider, ElsaClient} from "../../../services";

@Component({
  tag: 'elsa-workflow-definition-browser',
  shadow: false,
})
export class WorkflowDefinitionBrowser {
  private elsaClient: ElsaClient;

  @Event() public workflowDefinitionSelected: EventEmitter<WorkflowDefinitionSummary>;
  @State() private workflowDefinitions: PagedList<WorkflowDefinitionSummary> = {items: [], totalCount: 0};
  @State() private publishedWorkflowDefinitions: Array<WorkflowDefinitionSummary> = [];

  public async componentWillLoad() {
    const elsaClientProvider = Container.get(ElsaApiClientProvider);
    this.elsaClient = await elsaClientProvider.getClient();
  }

  public async connectedCallback() {
    //this.workflowDefinitions = await this.elsaClient.workflows.list({});
    await this.loadWorkflowDefinitions();
  }

  private async onPublishClick(e: Event, workflowDefinition: WorkflowDefinitionSummary) {
    // const elsaClient = await this.createClient();
    // await elsaClient.workflowDefinitionsApi.publish(workflowDefinition.definitionId);
    // await this.loadWorkflowDefinitions();
  }

  private async onUnPublishClick(e: Event, workflowDefinition: WorkflowDefinitionSummary) {
    // const elsaClient = await this.createClient();
    // await elsaClient.workflowDefinitionsApi.retract(workflowDefinition.definitionId);
    // await this.loadWorkflowDefinitions();
  }

  private async onDeleteClick(e: Event, workflowDefinition: WorkflowDefinitionSummary) {

    // const result = await this.confirmDialog.show(t('DeleteConfirmationModel.Title'), t('DeleteConfirmationModel.Message'));
    //
    // if (!result)
    //   return;
    //
    // const elsaClient = await this.createClient();
    // await elsaClient.workflowDefinitionsApi.delete(workflowDefinition.definitionId);
    // await this.loadWorkflowDefinitions();
  }

  private onWorkflowDefinitionClick = (e: MouseEvent, workflowDefinition: WorkflowDefinitionSummary) => {
    e.preventDefault();
    this.workflowDefinitionSelected.emit(workflowDefinition);
  }

  private async loadWorkflowDefinitions() {
    const elsaClient = this.elsaClient;
    const page = 0;
    const pageSize = 50;
    const latestVersionOptions: VersionOptions = {isLatest: true};
    const publishedVersionOptions: VersionOptions = {isPublished: true};
    const latestWorkflowDefinitions = await elsaClient.workflows.list({page: page, pageSize: pageSize, versionOptions: {isLatest: true}});
    const unpublishedWorkflowDefinitionIds = latestWorkflowDefinitions.items.filter(x => !x.isPublished).map(x => x.definitionId);
    this.publishedWorkflowDefinitions = await elsaClient.workflows.getMany({definitionIds: unpublishedWorkflowDefinitionIds, versionOptions: publishedVersionOptions});
    this.workflowDefinitions = latestWorkflowDefinitions;
  }

  render() {

    const workflowDefinitions = this.workflowDefinitions;

    return (
      <Host class="block">

        <div class="pt-4">
          <h2 class="text-lg font-medium ml-4 mb-2">Workflow Definitions</h2>
          <div class="align-middle inline-block min-w-full border-b border-gray-200">
            <table class="min-w-full">
              <thead>
              <tr class="border-t border-gray-200">
                <th class="px-6 py-3 border-b border-gray-200 bg-gray-50 text-left text-xs leading-4 font-medium text-gray-500 uppercase tracking-wider">
                  <span class="lg:pl-2">Name</span>
                </th>
                <th class="px-6 py-3 border-b border-gray-200 bg-gray-50 text-left text-xs leading-4 font-medium text-gray-500 uppercase tracking-wider">
                  Instances
                </th>
                <th
                  class="hidden md:table-cell px-6 py-3 border-b border-gray-200 bg-gray-50 text-right text-xs leading-4 font-medium text-gray-500 uppercase tracking-wider">
                  Latest Version
                </th>
                <th
                  class="hidden md:table-cell px-6 py-3 border-b border-gray-200 bg-gray-50 text-right text-xs leading-4 font-medium text-gray-500 uppercase tracking-wider">
                  Published Version
                </th>
                <th class="pr-6 py-3 border-b border-gray-200 bg-gray-50 text-right text-xs leading-4 font-medium text-gray-500 uppercase tracking-wider"/>
              </tr>
              </thead>
              <tbody class="bg-white divide-y divide-gray-100">
              {workflowDefinitions.items.map(workflowDefinition => {
                const latestVersionNumber = workflowDefinition.version;
                const {isPublished} = workflowDefinition;
                const publishedVersion: WorkflowDefinitionSummary = isPublished ? workflowDefinition : this.publishedWorkflowDefinitions.find(x => x.definitionId == workflowDefinition.definitionId);
                const publishedVersionNumber = !!publishedVersion ? publishedVersion.version : '-';
                let workflowDisplayName = workflowDefinition.name;

                if (!workflowDisplayName || workflowDisplayName.trim().length == 0)
                  workflowDisplayName = 'Untitled';

                const editIcon = (
                  <svg class="h-5 w-5 text-gray-500" width="24" height="24" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round"
                       stroke-linejoin="round">
                    <path d="M11 4H4a2 2 0 0 0-2 2v14a2 2 0 0 0 2 2h14a2 2 0 0 0 2-2v-7"/>
                    <path d="M18.5 2.5a2.121 2.121 0 0 1 3 3L12 15l-4 1 1-4 9.5-9.5z"/>
                  </svg>
                );

                const deleteIcon = (
                  <svg class="h-5 w-5 text-gray-500" width="24" height="24" viewBox="0 0 24 24" stroke-width="2" stroke="currentColor" fill="none" stroke-linecap="round" stroke-linejoin="round">
                    <path stroke="none" d="M0 0h24v24H0z"/>
                    <line x1="4" y1="7" x2="20" y2="7"/>
                    <line x1="10" y1="11" x2="10" y2="17"/>
                    <line x1="14" y1="11" x2="14" y2="17"/>
                    <path d="M5 7l1 12a2 2 0 0 0 2 2h8a2 2 0 0 0 2 -2l1 -12"/>
                    <path d="M9 7v-3a1 1 0 0 1 1 -1h4a1 1 0 0 1 1 1v3"/>
                  </svg>
                );

                const publishIcon = (
                  <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5 text-gray-500" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M7 16a4 4 0 01-.88-7.903A5 5 0 1115.9 6L16 6a5 5 0 011 9.9M15 13l-3-3m0 0l-3 3m3-3v12"/>
                  </svg>
                );

                const unPublishIcon = (
                  <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5 text-gray-500" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M18.364 18.364A9 9 0 005.636 5.636m12.728 12.728A9 9 0 015.636 5.636m12.728 12.728L5.636 5.636"/>
                  </svg>
                );

                return (
                  <tr>
                    <td class="px-6 py-3 whitespace-no-wrap text-sm leading-5 font-medium text-gray-900">
                      <div class="flex items-center space-x-3 lg:pl-2">
                        <a onClick={e => this.onWorkflowDefinitionClick(e, workflowDefinition)} href="#" class="truncate hover:text-gray-600"><span>{workflowDisplayName}</span></a>
                      </div>
                    </td>

                    <td class="px-6 py-3 text-sm leading-5 text-gray-500 font-medium">
                      <div class="flex items-center space-x-3 lg:pl-2">
                        <a href="#" class="truncate hover:text-gray-600">Instances</a>
                      </div>
                    </td>

                    <td class="hidden md:table-cell px-6 py-3 whitespace-no-wrap text-sm leading-5 text-gray-500 text-right">{latestVersionNumber}</td>
                    <td class="hidden md:table-cell px-6 py-3 whitespace-no-wrap text-sm leading-5 text-gray-500 text-right">{publishedVersionNumber}</td>
                    <td class="pr-6">
                      <elsa-context-menu menuItems={[
                        {text: 'Edit', anchorUrl: '#', icon: editIcon},
                        isPublished ? {text: 'Unpublish', clickHandler: e => this.onUnPublishClick(e, workflowDefinition), icon: unPublishIcon} : {
                          text: 'Publish',
                          clickHandler: e => this.onPublishClick(e, workflowDefinition),
                          icon: publishIcon
                        },
                        {text: 'Delete', clickHandler: e => this.onDeleteClick(e, workflowDefinition), icon: deleteIcon}
                      ]}/>
                    </td>
                  </tr>
                );
              })}
              </tbody>
            </table>
          </div>

          {/*<confirm-dialog ref={el => this.confirmDialog = el} culture={this.culture}/>*/}
        </div>

      </Host>
    );
  }
}
