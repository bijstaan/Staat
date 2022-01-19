import { IService } from "./IService"
import { IIncident } from "./IIncident"

export interface IMonitor {
  id: number
  createdAt: number
  updatedAt: number

  service: IService
  monitorTypeId: number
  type: unknown
  host: string
  port: number
  validateSsl: boolean
  monitorCron: string
  nextRunTime: number
  lastRunTime: number
  currentIncident: IIncident
  monitorData
}
