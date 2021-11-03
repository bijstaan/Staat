# Staat

### What is Staat

An open-source .NET Core C# Status Page and service monitoring tool. Includes:

- Public Status Page
- Monitoring of many service Types
  - HTTP
  - HTTPS
  - TCP
  - ICMP
  - SMTP
- Easy to use SPA Dashboard for IT Admins (Incident Reporting)
- Simple Deployment

# Installation

## Docker

Included with this repository is a `docker-compose.yml` file, and accompanying `Dockerfile` and `docker` folder.

The `docker` folder contains files relating to Staat, exclusive to Docker. This includes an example configuration file, updated for use with Docker.

> **You should edit the `docker/appsettings.json` file and modify the secret and URL to match your needs.**

After the config matches your setup, a simple `docker-compose up -d` will start Staat, and make it accessible on your PC and local network.

## Regular Install

### Storage Paths

There are 4 primary storage types you can use:

- Azure Blobs (`azure.blob://account=account_name;key=secret_value`) or (`azure.blob://development=true`)
- AWS S3 (`aws.s3://keyId=...;key=....;bucket=...;region=....;`)
- Google Cloud Storage (`google.storage://bucket=....;cred=....;`)
- Local Disk (`disk://path=C:\\Path\\To\\Directory`)

## Logging In

The default credentials are:

Email: admin@staat.local
Password: StaatIsAwesome!

# Why the AGPL v3 License

To make this decision clearer we first need to clear up some misconceptions that companies seem to have:

What AGPL does **NOT** stop:

- Commercial use
- Modification
- Distribution
- Private use

What AGPL does do:

- Force modifications to be available
- Force all copies to have the same license
- Makes network use (internet, lan, etc.) distribution

With that in mind this project is not a library, it will not be included in other projects. It is a standalone project, this means that strict open-source licensing is not going to cause major issues for companies trying to use it.
What it will do is force companies like Amazon, Google, etc. who decide that they want to use this project and make modifications to also make those modifications available to EVERYONE regardless of if it's the consumer or a competitor.
This means that if a large company makes a really good change I can copy that change into the main project, allowing everyone to benefit.
