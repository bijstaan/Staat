import { IIncident } from "./IIncident"
import { IMaintenance } from "./IMaintenance"
import { IMonitor } from "./IMonitor"
import { IServiceGroup } from "./IServiceGroup"
import { IStatus } from "./IStatus"

export interface IService {
  id: number

  createdAt: number
  updatedAt: number

  name: string
  description: string
  url: string
  status: IStatus
  incidents: IIncident[]
  group: IServiceGroup
  parent: IService
  children: IService[]
  monitors: IMonitor[]
  maintenance: IMaintenance[]
}
