import { useQuery, gql } from "@apollo/client"
import React, { useState } from "react"
import { IServiceGroup } from "../../@types/IServiceGroup"
import { ServiceData } from "./ServiceCard"

import { IconButton } from "@mui/material"
import { ExpandMoreOutlined, ExpandLessOutlined } from "@mui/icons-material"

interface IProps {
  group: Pick<IServiceGroup<never>, "id" | "name" | "description">
  services: ServiceData[] | null
  defaultOpen: boolean
}

export default function ServiceGroup({ group, services, defaultOpen }: IProps) {
  const [open, setOpen] = useState(defaultOpen)
  const [time, setTime] = useState(new Date())

  const { data, loading, error } = useQuery(
    gql`
      query ($groupId: Int!, $nowTime: DateTime!) {
        serviceGroup: serviceGroups(where: { id: { eq: $groupId } }) {
          nodes {
            services {
              id
              name
              description
              url

              status {
                id
                name
                description
                color
              }

              incidents(
                order: { startedAt: ASC }
                where: { endedAt: { eq: null } }
              ) {
                id
                title
                startedAt
                description
                descriptionHtml
              }

              maintenance(
                order: { startedAt: ASC }
                where: { endedAt: { gt: $nowTime } }
              ) {
                id
                title
                description
                startedAt
                endedAt
              }
            }
          }
        }
      }
    `,
    {
      variables: { groupId: group.id, nowTime: time.toISOString() },
      skip: (open && !defaultOpen) || !open,
    }
  )

  return (
    <section>
      <button onClick={() => setOpen(!open)} className="button-reset">
        <div>
          <div>
            <h2>{group.name}</h2>
            <p>{group.description}</p>
          </div>

          <IconButton component="div">
            {open ? <ExpandLessOutlined /> : <ExpandMoreOutlined />}
          </IconButton>
        </div>
      </button>
    </section>
  )
}
