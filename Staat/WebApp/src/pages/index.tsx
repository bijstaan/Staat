import React from "react"

import { Link } from "gatsby"
import PageLayout from "../components/Boilerplate/PageLayout"
import OverallStatus from "../components/OverallStatus"
import { makeStyles } from "@mui/styles"
import AllServicesList from "../components/Services/AllServicesList"

const useStyles = makeStyles({
  overallStatus: {
    width: "calc(100% - 32px)",
    margin: "0 auto",
    marginTop: -16,
    zIndex: 1,
    position: "relative",
  },
})

export default function Home() {
  const classes = useStyles()

  return (
    <PageLayout>
      <OverallStatus className={classes.overallStatus} />

      <AllServicesList />
    </PageLayout>
  )
}
