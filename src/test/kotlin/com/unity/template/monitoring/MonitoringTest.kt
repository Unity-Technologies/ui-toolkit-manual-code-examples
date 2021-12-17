package com.unity.template.monitoring

import com.unity.template.IntegrationTest
import io.ktor.http.HttpMethod
import io.ktor.http.HttpStatusCode
import io.ktor.server.testing.handleRequest
import org.junit.jupiter.api.Assertions.assertEquals
import org.junit.jupiter.api.Assertions.assertFalse
import org.junit.jupiter.api.Test

class MonitoringTest : IntegrationTest() {
    @Test
    fun `returns success to metrics endpoint`() {
        with(engine) {
            handleRequest(HttpMethod.Get, "/metrics").apply {
                assertEquals(HttpStatusCode.OK, response.status())
                assertFalse(response.content.isNullOrBlank())
            }
        }
    }
}
