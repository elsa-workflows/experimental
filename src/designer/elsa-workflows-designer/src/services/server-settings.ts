import {Service} from "typedi";

@Service()
export class ServerSettings {
  baseAddress: string;
}
