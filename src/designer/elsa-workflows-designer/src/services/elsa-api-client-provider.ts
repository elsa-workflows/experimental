import axios, {AxiosInstance, AxiosRequestConfig} from "axios";
import {Service as MiddlewareService} from 'axios-middleware';
import {EventBus} from './event-bus';
import {
  ActivityDescriptor,
  ActivityDescriptorResponse,
  EventTypes,
  TriggerDescriptor,
  TriggerDescriptorResponse,
  Workflow
} from "../models";
import 'reflect-metadata';
import {Container, Service} from "typedi";
import {ServerSettings} from "./server-settings";

export async function createHttpClient(baseAddress: string): Promise<AxiosInstance> {
  const config: AxiosRequestConfig = {
    baseURL: baseAddress
  };

  const eventBus = Container.get(EventBus);
  await eventBus.emit(EventTypes.HttpClient.ConfigCreated, this, {config});

  const httpClient = axios.create(config);
  const service = new MiddlewareService(httpClient);

  await eventBus.emit(EventTypes.HttpClient.ClientCreated, this, {service, httpClient});

  return httpClient;
}

export async function createElsaClient(serverUrl: string): Promise<ElsaClient> {

  const httpClient: AxiosInstance = await createHttpClient(serverUrl);

  return {
    descriptors: {
      activities: {
        async list(): Promise<Array<ActivityDescriptor>> {
          const response = await httpClient.get<ActivityDescriptorResponse>('api/descriptors/activities');
          return response.data.activityDescriptors;
        }
      },
      triggers: {
        async list(): Promise<Array<TriggerDescriptor>> {
          const response = await httpClient.get<TriggerDescriptorResponse>('api/descriptors/triggers');
          return response.data.triggerDescriptors;
        }
      }
    },
    workflows: {
      async post(request: SaveWorkflowRequest): Promise<Workflow> {
        const response = await httpClient.post<Workflow>('api/workflows', request);
        return response.data;
      }
    }
  };
}

export interface ElsaClient {
  descriptors: DescriptorsApi;
  workflows: WorkflowsApi;
}

export interface DescriptorsApi {
  activities: ActivityDescriptorsApi;
  triggers: TriggerDescriptorsApi;
}

export interface ActivityDescriptorsApi {
  list(): Promise<Array<ActivityDescriptor>>;
}

export interface TriggerDescriptorsApi {
  list(): Promise<Array<TriggerDescriptor>>;
}

export interface WorkflowsApi {
  post(request: SaveWorkflowRequest): Promise<Workflow>;
}

export interface SaveWorkflowRequest {
  workflow: Workflow;
  publish: boolean;
}

@Service()
export class ElsaApiClientProvider {
  private elsaClient: ElsaClient;

  constructor(private serverSettings: ServerSettings) {
  }

  public async getClient(): Promise<ElsaClient> {
    if (!!this.elsaClient)
      return this.elsaClient;

    return this.elsaClient = await createElsaClient(this.serverSettings.baseAddress);
  }
}
