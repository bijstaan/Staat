import { IFile } from "./IFile"
import { IMessage } from "./IMessage"
import { IService } from "./IService"
import { IUser } from "./IUser"

export interface IMaintenance {
  id: number
  createdAt: number
  updatedAt: number

  title: string
  description: string
  // descriptionHtml: string
  startedAt: number
  endedAt: number
  services: IService[]
  attachments: IFile[]
  author: IUser
  messages: IMessage[]
}
