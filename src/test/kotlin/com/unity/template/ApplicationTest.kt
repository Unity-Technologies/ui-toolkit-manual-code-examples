package com.unity.template

import io.ktor.application.call
import io.ktor.http.HttpMethod
import io.ktor.response.respondText
import io.ktor.routing.get
import io.ktor.routing.routing
import io.ktor.server.testing.handleRequest
import io.ktor.server.testing.withTestApplication
import org.junit.jupiter.api.Assertions.assertEquals
import org.junit.jupiter.api.Test

class ApplicationTest : KoinApplicationTest() {
    @Test
    fun `application smoke test`() {
        withTestApplication {
            application.routing {
                get("/") {
                    call.respondText { "OK" }
                }
            }

            handleRequest(HttpMethod.Get, "/").let { call ->
                assertEquals("OK", call.response.content)
            }
        }
    }
}
