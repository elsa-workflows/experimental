import {ActivityDriver, ActivityDriverFactory} from "./activity-driver";
import {Service} from "typedi";
import {DefaultActivityDriver} from "../drivers/activity/default-activity-driver";

@Service()
export class ActivityDriverRegistry {
  private _driverMap: Map<string, ActivityDriverFactory> = new Map<string, ActivityDriverFactory>();
  private _defaultDriverFactory: ActivityDriverFactory = () => new DefaultActivityDriver();

  public add(activityType: string, driverFactory: ActivityDriverFactory) {
    this._driverMap.set(activityType, driverFactory);
  }

  public get(activityType: string): ActivityDriverFactory {
    return this._driverMap.get(activityType);
  }

  public createDriver(activityType: string): ActivityDriver {
    const driverFactory = this.get(activityType) ?? this._defaultDriverFactory;
    return driverFactory();
  }
}
