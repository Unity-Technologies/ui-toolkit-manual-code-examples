package com.unity.template.cats

import io.ktor.application.Application
import io.ktor.routing.routing
import org.koin.core.context.loadKoinModules
import org.koin.dsl.module

val catModule = module {
    single { CatRepository() }
    single { CatService(get()) }
}

fun Application.register() {
    loadKoinModules(catModule)

    routing {
        cats()
    }
}
