# ads-kotlin-service-template

Service template for new kotlin based services. Currently used by Wallet team. 

This template aims to be as lightweight as reasonably possible and using available Kotlin frameworks (instead of Java). 

# Frameworks
This service has uses:
- Ktor web framework https://ktor.io/
- Koin dependency injection framework https://insert-koin.io/
- Exposed SQL framework https://github.com/JetBrains/Exposed
- Nats 
- Flyway
- Kotlin-logging https://github.com/MicroUtils/kotlin-logging
- Junit 5 Jupiter and Mockk for testing

# Plugins
- Renovate
- Sonar

# Using template
How to use this as a template when creating a new repository: 
https://help.github.com/en/articles/creating-a-repository-from-a-template

## TODO
- Rename `ads-kotlin-service-template` to your service name
- Replace `todo_change_me` with correct values
- Create Terraform setup for database and staging/production environments
- Create secrets in Vault and update them to staging and production setup files in `helm` folder
- Add code owners file, if you want to restrict who can approve pull requests https://help.github.com/articles/about-codeowners/

# Local development

Start docker-compose to have Nats and database running 

`docker-compose up nats db`

Build

`./gradlew build`

Checkstyle

`./gradlew ktlintCheck`

Test

`./gradlew test`

Run

`./gradlew run`

## Nats

To test nats calls, sample ruby script is in `helpers/nats-test-scripts` folder

`ruby helpers/nats-test-scripts/find-cat.rb` 

## Rest

To test rest endpoints first add a new entity

`curl -d '{"name":"Felix", "color":"black"}' -H "Content-Type: application/json" -X POST http://localhost:8080/cats`

.. and then query 

`curl http://localhost:8080/cats`


# Sonar

https://sonarqube.internal.unity3d.com/dashboard?id=ads-kotlin-service-template

# Converting to public repository
Any and all Unity software of any description (including components) (1) whose source is to be made available other than under a Unity source code license or (2) in respect of which a public announcement is to be made concerning its inner workings, may be licensed and released only upon the prior approval of Legal.
The process for that is to access, complete, and submit this [FORM](https://docs.google.com/forms/d/e/1FAIpQLSe3H6PARLPIkWVjdB_zMvuIuIVtrqNiGlEt1yshkMCmCMirvA/viewform).
