package com.unity.template.live

import com.unity.template.IntegrationTest
import io.ktor.http.HttpMethod
import io.ktor.http.HttpStatusCode
import io.ktor.server.testing.handleRequest
import org.junit.jupiter.api.Assertions.assertEquals
import org.junit.jupiter.api.Test

class LiveTest : IntegrationTest() {
    @Test
    fun `returns OK to live endpoint`() {
        with(engine) {
            handleRequest(HttpMethod.Get, "/live").apply {
                assertEquals(HttpStatusCode.OK, response.status())
                assertEquals("Alive", response.content)
            }
        }
    }
}
