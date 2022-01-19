import React from "react"

import Header from "./Header"

import "normalize.css"
import "../../styles/baseline.css"

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
