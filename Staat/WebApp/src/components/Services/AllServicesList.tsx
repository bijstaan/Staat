import React, { useState } from "react"
import { useQuery, gql } from "@apollo/client"
import LoadingIndicator from "../LoadingIndicator"
import ServiceGroup from "./ServiceGroup"

import { IServiceGroup } from "../../@types/IServiceGroup"
import { IService } from "../../@types/IService"
import { IStatus } from "../../@types/IStatus"
import { IIncident } from "../../@types/IIncident"
import { IMaintenance } from "../../@types/IMaintenance"

interface IAPIResponse {
  openServiceGroups: {
    nodes: Pick<
      IServiceGroup<
        Pick<
          IService<
            Pick<IStatus, "id" | "name" | "description" | "color">,
            Pick<
              IIncident,
              "id" | "title" | "startedAt" | "description" | "descriptionHtml"
            >,
            never,
            never,
            never,
            never,
            Pick<
              IMaintenance,
              "id" | "title" | "description" | "startedAt" | "endedAt"
            >
          >,
          | "id"
          | "name"
          | "description"
          | "url"
          | "status"
          | "incidents"
          | "maintenance"
        >
      >,
      "id" | "name" | "description" | "services"
    >[]
  }
  closedServiceGroups: {
    nodes: Pick<IServiceGroup<never>, "id" | "name" | "description">[]
  }
}

export default function AllServicesList() {
  const [hasErrored, setHasErrored] = useState(false)
  const [time, setTime] = useState(new Date())

  const { data, loading, error } = useQuery<IAPIResponse>(
    gql`
      query getGroups($nowTime: DateTime!) {
        openServiceGroups: serviceGroups(where: { defaultOpen: { eq: true } }) {
          nodes {
            id
            name
            description
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
                __typename
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
                __typename
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
                __typename
              }
              __typename
            }
            __typename
          }
          __typename
        }

        closedServiceGroups: serviceGroups(
          where: { defaultOpen: { eq: false } }
        ) {
          nodes {
            id
            name
            description
            __typename
          }
          __typename
        }
      }
    `,
    {
      variables: { nowTime: time.toISOString() },
      skip: hasErrored,
      onError: () => setHasErrored(true),
      errorPolicy: "all",
    }
  )

  if (loading) {
    return <LoadingIndicator />
  }

  if (!data) return null

  const { openServiceGroups, closedServiceGroups } = data

  return (
    <>
      <h1>Services</h1>

      <section>
        {openServiceGroups.nodes.map(group => {
          const { services, ...groupData } = group

          return (
            <ServiceGroup
              key={group.id}
              services={group.services}
              group={groupData}
              defaultOpen={true}
            />
          )
        })}
      </section>

      <section>
        {closedServiceGroups.nodes.map(group => {
          return (
            <ServiceGroup key={group.id} group={group} defaultOpen={false} />
          )
        })}
      </section>
    </>
  )
}
