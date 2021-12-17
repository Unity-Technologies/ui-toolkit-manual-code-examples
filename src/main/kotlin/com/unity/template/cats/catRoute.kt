package com.unity.template.cats

import com.fasterxml.jackson.module.kotlin.MissingKotlinParameterException
import io.ktor.application.call
import io.ktor.http.HttpStatusCode
import io.ktor.request.receive
import io.ktor.response.respond
import io.ktor.routing.Routing
import io.ktor.routing.get
import io.ktor.routing.post
import mu.KotlinLogging
import org.jetbrains.exposed.exceptions.ExposedSQLException
import org.koin.ktor.ext.inject

private val LOGGER = KotlinLogging.logger {}

const val REST_ENDPOINT = "/cats"

fun Routing.cats() {
    // Lazy inject
    val service: CatService by inject()

    get(REST_ENDPOINT) {
        LOGGER.info { "Rest endpoint called to get all kitties" }
        call.respond(service.list())
    }

    get("$REST_ENDPOINT/{id}") {
        val id = call.parameters["id"]?.toInt() ?: throw IllegalArgumentException("Missing id!")
        LOGGER.debug("Get kitty with id: $id")

        try {
            call.respond(service.find(id))
        } catch (e: NoSuchElementException) {
            call.respond(HttpStatusCode.NotFound)
        }
    }

    post(REST_ENDPOINT) {
        try {
            val kitty = call.receive<Kitty>()
            call.respond(service.add(kitty))
        } catch (e: MissingKotlinParameterException) {
            call.respond(HttpStatusCode.BadRequest)
        } catch (e: ExposedSQLException) {
            call.respond(HttpStatusCode.Conflict)
        }
    }
}
