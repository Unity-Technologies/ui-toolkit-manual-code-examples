package com.unity.template

import com.fasterxml.jackson.databind.SerializationFeature
import com.typesafe.config.ConfigFactory
import com.unity.template.live.live
import com.unity.template.utils.CustomApplicationConfig
import io.ktor.application.Application
import io.ktor.application.ApplicationStarted
import io.ktor.application.install
import io.ktor.features.CallLogging
import io.ktor.features.ContentNegotiation
import io.ktor.features.DefaultHeaders
import io.ktor.jackson.jackson
import io.ktor.routing.Routing
import io.ktor.server.engine.ApplicationEngineEnvironment
import io.ktor.server.engine.addShutdownHook
import io.ktor.server.engine.applicationEngineEnvironment
import io.ktor.server.engine.connector
import io.ktor.server.netty.NettyApplicationEngine
import mu.KotlinLogging
import org.koin.ktor.ext.Koin

private val LOGGER = KotlinLogging.logger {}

fun Application.mainModule() {
    install(DefaultHeaders)
    install(CallLogging)
    install(ContentNegotiation) {
        jackson {
            enable(SerializationFeature.INDENT_OUTPUT)
        }
    }

    val config = environment.config.config("template") as CustomApplicationConfig
    install(Koin) {
        properties(config.toMap())
    }

    install(Routing) {
        live()
    }

    environment.monitor.subscribe(ApplicationStarted) {
        LOGGER.info { "App is up and running!" }
    }
}

fun main() {
    val applicationEnvironment = buildApplicationEnvironment()
    val engine = NettyApplicationEngine(applicationEnvironment)
    engine.addShutdownHook {
        engine.stop(3_000, 5_000)
    }
    engine.start(true)
}

fun buildApplicationEnvironment(): ApplicationEngineEnvironment {
    val applicationConfig = ConfigFactory.load()
    val host = applicationConfig.getString("ktor.deployment.host")
    val port = applicationConfig.getInt("ktor.deployment.port")

    return applicationEngineEnvironment {
        config = CustomApplicationConfig(applicationConfig)
        connector {
            this.host = host
            this.port = port
        }
    }
}
