import React from "react"

import Header from "./Header"

import "normalize.css"
import "../../styles/baseline.less"
import "../../styles/tooltips.less"

interface IProps {
  children: React.ReactChild | React.ReactChild[]
}

export default function PageLayout({ children }: IProps): JSX.Element {
  return (
    <>
      <Header />
      <main>{children}</main>
    </>
  )
}
