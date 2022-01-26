import { IService } from "./IService"

export interface IServiceGroup<Service extends Partial<IService>> {
  id: number
  name: string
  description: string
  defaultOpen: boolean
  services: Service[]
  createdAt: number
  updatedAt: number
}
