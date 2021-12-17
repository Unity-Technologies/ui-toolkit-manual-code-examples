package com.unity.template.db

import com.zaxxer.hikari.HikariConfig
import com.zaxxer.hikari.HikariDataSource
import io.ktor.application.Application
import org.flywaydb.core.Flyway
import org.jetbrains.exposed.sql.Database
import org.koin.core.context.loadKoinModules
import org.koin.dsl.module
import org.koin.ktor.ext.inject

val dbModule = module {
    single { hikari(getProperty("db.url"), getProperty("db.username"), getProperty("db.password")) }
}

fun Application.register() {
    loadKoinModules(dbModule)
    val dbSource: HikariDataSource by inject()

    Database.connect(dbSource)
    Flyway.configure()
        .dataSource(dbSource)
        .load()
        .migrate()
}

private fun hikari(dbUrl: String, dbUser: String, dbPassword: String): HikariDataSource {
    val config = HikariConfig()
    config.driverClassName = "org.postgresql.Driver"
    config.jdbcUrl = dbUrl
    config.username = dbUser
    config.password = dbPassword
    config.maximumPoolSize = 3
    config.isAutoCommit = false
    config.validate()
    return HikariDataSource(config)
}
