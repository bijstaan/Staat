import React from "react"
import { makeStyles } from "@mui/styles"
import clsx from "clsx"

const useStyles = makeStyles({
  root: {
    "--size": "16px",
    borderRadius: "50%",
    width: "var(--size)",
    height: "var(--size)",
    border: "2px solid currentColor",
    borderTopColor: "transparent",
    animation: "$spin 750ms linear infinite",
  },
  "@keyframes spin": {
    from: {
      transform: "rotate(0)",
    },
    to: {
      transform: "rotate(1turn)",
    },
  },
})

interface IProps {
  className?: string
  size?: number
}

export default function LoadingIndicator(props: IProps) {
  const classes = useStyles()

  return (
    <div
      className={clsx(classes.root, props.className)}
      role="status"
      aria-label="Loading..."
      style={{ "--size": props.size } as any}
    />
  )
}
