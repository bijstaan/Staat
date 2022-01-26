/**
 * Configure your Gatsby site with this file.
 *
 * See: https://www.gatsbyjs.com/docs/gatsby-config/
 */

const PROD_PLUGINS =
  process.env.NODE_ENV === "production" ? [`gatsby-plugin-preact`] : []

module.exports = {
  /* Your site config here */
  plugins: [
    ...PROD_PLUGINS,
    `gatsby-plugin-less`,
    `gatsby-plugin-material-ui`,
    {
      resolve: "gatsby-plugin-apollo",
      options: {
        uri: "/api/graphql",
      },
    },
  ],
}
