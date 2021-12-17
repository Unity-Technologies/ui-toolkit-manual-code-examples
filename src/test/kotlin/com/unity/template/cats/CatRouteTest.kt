package com.unity.template.cats

import com.unity.template.IntegrationTest
import io.ktor.http.HttpMethod
import io.ktor.http.HttpStatusCode
import io.ktor.server.testing.handleRequest
import org.jetbrains.exposed.sql.insertAndGetId
import org.jetbrains.exposed.sql.transactions.transaction
import org.junit.jupiter.api.BeforeEach
import org.junit.jupiter.api.Test
import kotlin.test.assertEquals
import com.unity.template.cats.register as registerCats

class CatRouteTest : IntegrationTest() {

    @BeforeEach
    internal fun setUp() {
        engine.application.registerCats()
    }

    @Test
    fun `should respond with empty when no cats data exists`() {
        with(engine) {
            handleRequest(HttpMethod.Get, "/cats").apply {
                assertEquals(HttpStatusCode.OK, response.status())
                assertEquals("[ ]", response.content)
            }
        }
    }

    @Test
    fun `should respond with items when cats exist`() {
        val id = transaction {
            Cats.insertAndGetId {
                it[name] = "Pusheen"
                it[color] = "cray"
            }.value
        }
        with(engine) {
            handleRequest(HttpMethod.Get, "/cats").apply {
                val content =
                    """
                    |[ {
                    |  "id" : $id,
                    |  "name" : "Pusheen",
                    |  "color" : "cray"
                    |} ]""".trimMargin()
                assertEquals(HttpStatusCode.OK, response.status())
                println(response.content)
                assertEquals(content, response.content)
            }
        }
    }
}
