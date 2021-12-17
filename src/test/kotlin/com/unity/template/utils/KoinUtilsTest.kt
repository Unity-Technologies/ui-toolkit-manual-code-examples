package com.unity.template.utils

import com.unity.template.IntegrationTest
import org.hamcrest.CoreMatchers.equalTo
import org.hamcrest.MatcherAssert.assertThat
import org.junit.jupiter.api.BeforeEach
import org.junit.jupiter.api.Nested
import org.junit.jupiter.api.Test
import org.junit.jupiter.api.assertThrows
import org.koin.core.error.MissingPropertyException

class KoinUtilsTest : IntegrationTest() {
    @Nested
    inner class GetPropertyOrThrow {
        private val key = "foo"
        private val value = "bar"

        @BeforeEach
        internal fun setUp() {
            getKoin().setProperty(key, value)
        }

        @Test
        fun `retrieves property if available`() {
            val property: String = engine.application.getPropertyOrThrow(key)
            assertThat(property, equalTo(value))
        }

        @Test
        fun `throws if property is missing`() {
            assertThrows<MissingPropertyException> { engine.application.getPropertyOrThrow("bar") }
        }
    }
}
