package com.unity.template

import org.junit.jupiter.api.AfterEach
import org.koin.core.context.stopKoin
import org.koin.test.KoinTest

open class KoinApplicationTest : KoinTest {

    /**
     * Koin AutoCloseKoinTest is using JUnit 4 and @After, that is not working with JUnit5.
     * That's why we need to override it here and extend tests that require injection with this class.
     */
    @AfterEach
    internal fun tearDown() {
        stopKoin()
    }
}
