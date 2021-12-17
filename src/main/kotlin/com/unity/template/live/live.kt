package com.unity.template.live

import io.ktor.application.call
import io.ktor.http.ContentType
import io.ktor.response.respondText
import io.ktor.routing.Routing
import io.ktor.routing.get

fun Routing.live() {
    get("/live") {
        call.respondText("Alive", contentType = ContentType.Text.Plain)
    }
}
