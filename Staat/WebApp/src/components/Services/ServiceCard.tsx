import React from "react"

import { makeStyles } from "@mui/styles"
import { IService } from "../../@types/IService"
import { useQuery, gql, ApolloError } from "@apollo/client"
import { IStatus } from "../../@types/IStatus"
import { IIncident } from "../../@types/IIncident"
import { IMaintenance } from "../../@types/IMaintenance"
import StatusIndicator from "./StatusIndicator"

export type ServiceData = Pick<
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
    Pick<IMaintenance, "id" | "title" | "description" | "startedAt" | "endedAt">
  >,
  "id" | "name" | "description" | "url" | "status" | "incidents" | "maintenance"
>

interface IProps {
  serviceId: number
  serviceData?: ServiceData
}

interface IAPIResponse {
  services: { nodes: ServiceData[] }
}

export default function ServiceCard({ serviceId, serviceData }: IProps) {
  const { data, loading, error } = useQuery<IAPIResponse>(
    gql`
      {
        services(where: { id: { eq: $serviceId } }, first: 1) {
          nodes {
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
              where: { endedAt: { gt: $now } }
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
    `,
    {
      variables: { serviceId, now: new Date().toISOString() },
      skip: !!serviceData,
    }
  )

  if (serviceData) {
    return (
      <ServiceCardInner
        serviceData={serviceData}
        loading={false}
        error={undefined}
      />
    )
  }

  return (
    <ServiceCardInner
      serviceData={data.services.nodes[0]}
      loading={loading}
      error={error}
    />
  )
}

interface InnerProps {
  serviceData: ServiceData
  loading: boolean
  error: ApolloError
}

const useStyles = makeStyles({
  root: {
    borderRadius: 8,
    padding: 16,
  },
  name: {
    display: "inline-block",
    marginLeft: 8,
  },
})

function ServiceCardInner({ serviceData, loading, error }: InnerProps) {
  const classes = useStyles()

  return (
    <section className={classes.root}>
      <StatusIndicator size="1em" status={serviceData.status} />
      <h3 className={classes.name}>{serviceData.name}</h3>
    </section>
  )
}
