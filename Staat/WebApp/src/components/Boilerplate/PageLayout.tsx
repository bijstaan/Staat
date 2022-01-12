import React from "react"

import Header from "./Header"

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
