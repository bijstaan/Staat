import { makeStyles } from "@mui/styles"
import { IStatus } from "../../@types/IStatus"
import clsx from "clsx"
import { parse, darken, toRgb } from "micro-color"

interface IProps {
  status: Pick<IStatus, "color" | "name"> & Record<string, any>
  size: number | string
  className?: string
}

const useStyles = makeStyles({
  root: {
    width: "var(--size)",
    height: "var(--size)",
    borderRadius: "50%",
    backgroundColor: "var(--color)",
    display: "inline-block",
    border: "1px solid ",
  },
})

export default function StatusIndicator({
  status: { color, name },
  size,
  className,
}: IProps) {
  const classes = useStyles()

  return (
    <span
      className={clsx(classes.root, className)}
      data-tooltip
      aria-label={name}
      style={
        {
          "--size": typeof size === "number" ? `${size}px` : size,
          "--color": color,
          "--border": toRgb(darken(parse(color), 0.2)),
        } as any
      }
    />
  )
}
