import { IFile } from "./IFile"
import { IMessage } from "./IMessage"
import { IService } from "./IService"
import { IUser } from "./IUser"

export interface IIncident {
  id: number
  title: string
  description: string
  descriptionHtml: string
  service: IService
  messages: IMessage[]
  author: IUser
  startedAt: number
  endedAt: number | null
  files: IFile[]
}
