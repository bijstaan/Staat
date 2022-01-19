import React from "react"
import { makeStyles } from "@mui/styles"

import { HeaderNavLinks } from "../../data/HeaderNavigation"
import Link from "../Link"

import coverImg from "../../img/placeholder-cover.jpg"

const useStyles = makeStyles({
  headerRoot: {
    width: "100%",
    overflow: "hidden",
  },
  coverImage: {
    width: "100%",
    height: 200,
    objectFit: "cover",
    overflow: "hidden",
    borderBottom: "1px solid #e0e0e0",
  },
  // title: {
  //   margin: 0,
  //   fontWeight: "normal",
  // },
  // navContainer: {
  //   marginLeft: 'auto',
  // },
  // navList: {
  //   listStyle: "none",
  //   margin: 0,
  //   padding: 0,
  // },
})

export default function Header() {
  const classes = useStyles()

  return (
    <header className={classes.headerRoot}>
      <img src={coverImg} className={classes.coverImage} />
    </header>
  )
}
