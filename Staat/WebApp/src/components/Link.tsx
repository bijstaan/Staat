import React from "react"
import { Link as GatsbyLink } from "gatsby"

interface IProps extends React.HTMLProps<HTMLAnchorElement> {
  children: React.ReactChild | React.ReactChild[]
  href: string
  isExternal: boolean
}

export default function Link({
  children,
  href,
  isExternal,
  ...props
}: IProps): JSX.Element {
  const commonProps = { ...props, children }

  if (isExternal) {
    return <a href={href} {...commonProps} />
  }

  return <GatsbyLink to={href} {...commonProps} />
}
