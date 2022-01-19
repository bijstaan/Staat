import { IMonitor } from "./IMonitor"

export interface IMonitorData {
  id: number
  createdAt: number
  updatedAt: number

  pingTime: number
  available: boolean
  sslValid: boolean | null
  failureReason: string
  monitor: IMonitor
}
