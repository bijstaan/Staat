import { IService } from "./IService"

export interface IServiceGroup {
  id: number
  name: string
  description: string
  defaultOpen: boolean
  services: IService[]
  createdAt: number
  updatedAt: number
}
