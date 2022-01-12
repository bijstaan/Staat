import React from "react"
import { HeaderNavLinks } from "../../data/HeaderNavigation"
import Link from "../Link"

export default function Header() {
  return (
    <header>
      <h1>Staat</h1>

      <nav>
        <ul>
          {HeaderNavLinks.map(link => (
            <li>
              <Link href={link.href} isExternal={link.isExternal}>
                {link.label}
              </Link>
            </li>
          ))}
        </ul>
      </nav>
    </header>
  )
}
