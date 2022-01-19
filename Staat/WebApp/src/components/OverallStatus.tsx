import React from "react"

import { useQuery, gql } from "@apollo/client"
import { makeStyles } from "@mui/styles"
import LoadingIndicator from "./LoadingIndicator"
import clsx from "clsx"
import getTextColor from "../helpers/getTextColor"

import { parse, darken, toRgb } from "micro-color"

import SuccessIcon from "mdi-react/CheckCircleOutlineIcon"
import WarningIcon from "mdi-react/AlertOutlineIcon"

interface IProps {
  className?: string
}

const useStyles = makeStyles({
  root: {
    "--bg-color": "#f5f5f5",
    "--border-color": "#e0e0e0",
    "--text-color": "#000",

    border: "1px solid var(--border-color)",
    padding: "12px 16px",
    borderRadius: 8,
    backgroundColor: "var(--bg-color)",
    color: "var(--text-color)",

    display: "flex",
    alignItems: "center",

    "& p": {
      margin: 0,
      marginLeft: 8,
    },
  },
  icon: {
    gridRow: "1 / span 2",
  },
})

export default function OverallStatus(props: IProps) {
  // Query all active incidents from the API, which have no end time
  // (i.e. currently in progress).
  const { data, loading, error } = useQuery(gql`
    {
      incidents(where: { endedAt: { eq: null } }) {
        nodes {
          id

          service {
            id
            name

            status {
              id
              name
              description
              color
            }
          }
        }
      }
    }
  `)

  /**
   * Array of active incidents, or `undefined` if loading or errored.
   */
  const activeIncidents = loading || error ? undefined : data.incidents.nodes

  const classes = useStyles()

  if (error) {
    return (
      <section className={clsx(classes.root, props.className)}>
        <WarningIcon className={classes.icon} />
        <p>Uh oh, an error occurred while loading data.</p>
      </section>
    )
  }

  if (loading) {
    return (
      <section className={clsx(classes.root, props.className)}>
        <LoadingIndicator />
        <p>Just a moment...</p>
      </section>
    )
  }

  if (activeIncidents?.length === 0) {
    return (
      <section className={clsx(classes.root, props.className)}>
        <SuccessIcon className={classes.icon} />
        <p>{"Everything's working normally."}</p>
      </section>
    )
  }

  // Find the most severe incident via service status ID
  const highestSeverity = activeIncidents.reduce((acc, curr) => {
    return acc?.id > curr.service.status.id ? acc : curr.service.status
  }, null)

  return (
    <section
      className={clsx(classes.root, classes.rootLoaded, props.className)}
      style={
        {
          "--bg-color": highestSeverity?.color,
          "--border-color": toRgb(darken(parse(highestSeverity?.color), 0.2)),
          "--text-color": getTextColor(highestSeverity?.color),
        } as any
      }
    >
      <WarningIcon className={classes.icon} />
      <p>{highestSeverity.name}</p>
    </section>
  )
}
