import contrast from "contrast"

/**
 * Automatically calculates whether black or white is the best color to use
 * for text on the provided color to the function.
 */
export default function getTextColor(color: string): string {
  return contrast(color) === "light" ? "#000" : "#fff"
}
