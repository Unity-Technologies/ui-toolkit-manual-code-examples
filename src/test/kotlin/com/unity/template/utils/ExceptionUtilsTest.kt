package com.unity.template.utils

import org.hamcrest.MatcherAssert.assertThat
import org.hamcrest.core.IsEqual.equalTo
import org.junit.jupiter.api.Nested
import org.junit.jupiter.api.Test

class ExceptionUtilsTest {
    @Nested
    inner class GetRootCause {
        @Test
        fun `returns throwable if it has no cause`() {
            val throwable = Exception("Hello")
            assertThat(throwable, equalTo(getRootCause(throwable)))
        }

        @Test
        fun `returns cause`() {
            val cause = Exception("Cause")
            val throwable = Exception("Hello", cause)
            assertThat(cause, equalTo(getRootCause(throwable)))
        }

        @Test
        fun `returns nested cause`() {
            val rootCause = Exception("RootCause")
            val cause = Exception("Cause", rootCause)
            val throwable = Exception("Hello", cause)
            assertThat(rootCause, equalTo(getRootCause(throwable)))
        }

        @Test
        fun `returns cyclic nested cause`() {
            val cyclicRoot = Exception("RootCause")
            val cause = Exception("Cause", cyclicRoot)
            cyclicRoot.initCause(cause)
            val throwable = Exception("Hello", cause)
            assertThat(cyclicRoot, equalTo(getRootCause(throwable)))
        }
    }
}
