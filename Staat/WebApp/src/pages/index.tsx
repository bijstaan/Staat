import React from "react"

import { Link } from "gatsby"
import PageLayout from "../components/Boilerplate/PageLayout"

export default function Home() {
  return (
    <PageLayout>
      <div>Welcome to Staat!</div>
      <Link to="/page2">Page 2</Link>
    </PageLayout>
  )
}
