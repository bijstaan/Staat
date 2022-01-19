import { IIncident } from "./IIncident"
import { IMaintenance } from "./IMaintenance"
import { IMessage } from "./IMessage"

export interface IFile {
  id: number
  createdAt: number
  updatedAt: number

  name: string
  namespace: string
  hash: string
  mimeType: string

  incidents: IIncident[]
  incidentMessages: IMessage[]

  maintenances: IMaintenance[]
  maintenanceMessages: IMessage[]
}
