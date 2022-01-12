import React from "react"

import { Link } from "gatsby"
import PageLayout from "../components/Boilerplate/PageLayout"

export default function Home() {
  return (
    <PageLayout>
      <div>Hello world!!</div>
      <Link to="/">Page 1</Link>
    </PageLayout>
  )
}
