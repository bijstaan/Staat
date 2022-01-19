import { IFile } from "./IFile";
import { IUser } from "./IUser"

export interface IMessage {
  id: number
  createdAt: number
  updatedAt: number
  message: string
  messageHtml: string
  author: IUser
  attachments: IFile[]
}
