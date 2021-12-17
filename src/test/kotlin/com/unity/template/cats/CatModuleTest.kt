package com.unity.template.cats

import com.unity.template.KoinApplicationTest
import io.ktor.application.install
import io.ktor.server.testing.withApplication
import org.junit.jupiter.api.Assertions.assertNotNull
import org.junit.jupiter.api.Test
import org.koin.ktor.ext.Koin
import org.koin.test.inject

class CatModuleTest : KoinApplicationTest() {

    @Test
    fun `initializes CatService with koin`() {
        withApplication {
            application.install(Koin) {
                modules(catModule)
            }

            val bean: CatService by inject()
            assertNotNull(bean)
        }
    }

    @Test
    fun `initializes CatRepository with koin`() {
        withApplication {
            application.install(Koin) {
                modules(catModule)
            }
            val bean: CatRepository by inject()
            assertNotNull(bean)
        }
    }
}
