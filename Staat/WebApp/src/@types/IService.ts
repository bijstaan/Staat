import { IIncident } from "./IIncident"
import { IMaintenance } from "./IMaintenance"
import { IMonitor } from "./IMonitor"
import { IServiceGroup } from "./IServiceGroup"
import { IStatus } from "./IStatus"

export interface IService<
  Status extends Partial<IStatus>,
  Incident extends Partial<IIncident>,
  ServiceGroup extends Partial<IServiceGroup<any>>,
  ParentService extends Partial<IService<any, any, any, any, any, any, any>>,
  ChildService extends Partial<IService<any, any, any, any, any, any, any>>[],
  Monitor extends Partial<IMonitor>,
  Maintenance extends Partial<IMaintenance>
> {
  id: number

  createdAt: number
  updatedAt: number

  name: string
  description: string
  url: string
  status: Status
  incidents: Incident[]
  group: ServiceGroup
  parent: ParentService
  children: ChildService[]
  monitors: Monitor[]
  maintenance: Maintenance[]
}
