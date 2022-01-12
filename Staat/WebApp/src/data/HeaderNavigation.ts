interface NavigationLink {
  label: string
  href: string
}

interface NavigationLinkWithExternal extends NavigationLink {
  isExternal: boolean
}

const Links: NavigationLink[] = [
  {
    label: "GitHub",
    href: "https://github.com/bijstaan/Staat/",
  },
]

function isExternal(url: string) {
  if (
    url.startsWith("http://") ||
    url.startsWith("https://") ||
    url.startsWith("//")
  ) {
    return true
  }

  return false
}

export const HeaderNavLinks: NavigationLinkWithExternal[] = Links.map(
  (link: NavigationLink & { isExternal?: boolean }) => {
    link.isExternal = isExternal(link.href)
    return link as NavigationLinkWithExternal
  }
)
