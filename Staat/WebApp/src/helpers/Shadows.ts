// Typescript number range types
type PrependNextNum<A extends Array<unknown>> = A["length"] extends infer T
  ? ((t: T, ...a: A) => void) extends (...x: infer X) => void
    ? X
    : never
  : never
type EnumerateInternal<A extends Array<unknown>, N extends number> = {
  0: A
  1: EnumerateInternal<PrependNextNum<A>, N>
}[N extends A["length"] ? 0 : 1]
export type Enumerate<N extends number> = EnumerateInternal<
  [],
  N
> extends (infer E)[]
  ? E
  : never
export type Range<FROM extends number, TO extends number> = Exclude<
  Enumerate<TO>,
  Enumerate<FROM>
>

// https://shadows.brumm.af/
const shadows = [
  `0px 0.3px 0.7px rgba(0, 0, 0, 0.017),
   0px 0.8px 1.7px rgba(0, 0, 0, 0.024),
   0px 1.5px 3.1px rgba(0, 0, 0, 0.03),
   0px 2.7px 5.6px rgba(0, 0, 0, 0.036),
   0px 5px 10.4px rgba(0, 0, 0, 0.043),
   0px 12px 25px rgba(0, 0, 0, 0.06)`,
  `0px 0.6px 2.2px rgba(0, 0, 0, 0.017),
   0px 1.3px 5.3px rgba(0, 0, 0, 0.024),
   0px 2.5px 10px rgba(0, 0, 0, 0.03),
   0px 4.5px 17.9px rgba(0, 0, 0, 0.036),
   0px 8.4px 33.4px rgba(0, 0, 0, 0.043),
   0px 20px 80px rgba(0, 0, 0, 0.06)`,
  `0px 1.1px 2.2px rgba(0, 0, 0, 0.017),
   0px 2.7px 5.3px rgba(0, 0, 0, 0.024),
   0px 5px 10px rgba(0, 0, 0, 0.03),
   0px 8.9px 17.9px rgba(0, 0, 0, 0.036),
   0px 16.7px 33.4px rgba(0, 0, 0, 0.043),
   0px 40px 80px rgba(0, 0, 0, 0.06)`,
  `0px 1.7px 2.2px rgba(0, 0, 0, 0.02),
   0px 4px 5.3px rgba(0, 0, 0, 0.028),
   0px 7.5px 10px rgba(0, 0, 0, 0.035),
   0px 13.4px 17.9px rgba(0, 0, 0, 0.042),
   0px 25.1px 33.4px rgba(0, 0, 0, 0.05),
   0px 60px 80px rgba(0, 0, 0, 0.07)`,
  `0px 2.8px 2.2px rgba(0, 0, 0, 0.02),
   0px 6.7px 5.3px rgba(0, 0, 0, 0.028),
   0px 12.5px 10px rgba(0, 0, 0, 0.035),
   0px 22.3px 17.9px rgba(0, 0, 0, 0.042),
   0px 41.8px 33.4px rgba(0, 0, 0, 0.05),
   0px 100px 80px rgba(0, 0, 0, 0.07)`,
]

export default function createShadow(elevation: Range<1, 8>) {
  if (elevation < 1 || elevation > 8) {
    throw new Error("Shadow elevation must be between 1 and 8")
  }

  return shadows[elevation - 1]
}
