package com.unity.template.utils

import com.typesafe.config.ConfigException
import com.typesafe.config.ConfigFactory
import com.typesafe.config.ConfigValueFactory
import io.ktor.config.ApplicationConfigurationException
import org.hamcrest.MatcherAssert.assertThat
import org.hamcrest.core.IsEqual.equalTo
import org.junit.jupiter.api.Assertions.assertNull
import org.junit.jupiter.api.Assertions.assertTrue
import org.junit.jupiter.api.BeforeEach
import org.junit.jupiter.api.Nested
import org.junit.jupiter.api.Test
import org.junit.jupiter.api.assertAll
import org.junit.jupiter.api.assertThrows
import kotlin.test.assertNotNull

class ConfigUtilsTest {
    lateinit var config: CustomApplicationConfig
    private val map1 = ConfigValueFactory.fromMap(mapOf("one" to 1, "two" to 2))
    private val map2 = ConfigValueFactory.fromMap(mapOf("three" to 3, "four" to 4))

    @BeforeEach
    internal fun setUp() {
        val baseConfig = ConfigFactory.empty()
            .withValue("foo", ConfigValueFactory.fromAnyRef("bar"))
            .withValue("number", ConfigValueFactory.fromAnyRef(1))
            .atKey("test")
            .withValue("key", ConfigValueFactory.fromAnyRef("value"))
            .withValue("bool", ConfigValueFactory.fromAnyRef("true"))
            .withValue("list", ConfigValueFactory.fromIterable(listOf(map1, map2)))

        config = CustomApplicationConfig(baseConfig)
    }

    @Nested
    inner class Property {
        @Test
        fun `returns value of existing config value`() {
            val value = config.property("key").getString()
            assertThat(value, equalTo("value"))
        }

        @Test
        fun `returns value of nested config value`() {
            val value = config.property("test.foo").getString()
            assertThat(value, equalTo("bar"))
        }

        @Test
        fun `throws if value is missing`() {
            assertThrows<ApplicationConfigurationException> { config.property("missing") }
        }
    }

    @Nested
    inner class PropertyOrNull {
        @Test
        fun `returns nullable value of existing config value`() {
            val value = config.propertyOrNull("key")
            assertNotNull(value)
            assertThat(value.getString(), equalTo("value"))
        }

        @Test
        fun `returns nullable value of nested config value`() {
            val value = config.propertyOrNull("test.foo")
            assertNotNull(value)
            assertThat(value.getString(), equalTo("bar"))
        }

        @Test
        fun `returns null if value is missing`() {
            val value = config.propertyOrNull("missing")
            assertNull(value)
        }
    }

    @Nested
    inner class ConfigList {
        @Test
        fun `returns list of configs`() {
            val configList = config.configList("list")
            assertAll(
                { assertThat(configList.size, equalTo(2)) },
                { assertThat(configList[0].property("one").getString(), equalTo("1")) },
                { assertThat(configList[0].property("two").getString(), equalTo("2")) },
                { assertThat(configList[1].property("three").getString(), equalTo("3")) },
                { assertThat(configList[1].property("four").getString(), equalTo("4")) }
            )
        }

        @Test
        fun `throws if list is missing`() {
            assertThrows<ConfigException.Missing> { config.configList("missing") }
        }

        @Test
        fun `throws if found value is no list`() {
            assertThrows<ConfigException.WrongType> { config.configList("key") }
        }
    }

    @Nested
    inner class Config {
        @Test
        fun `returns config if available`() {
            val foundConfig = config.config("test")
            assertThat(foundConfig.property("foo").getString(), equalTo("bar"))
        }

        @Test
        fun `throws if config is missing`() {
            assertThrows<ConfigException.Missing> { config.config("missing") }
        }

        @Test
        fun `throws if found value is no config`() {
            assertThrows<ConfigException.WrongType> { config.config("key") }
        }
    }

    @Nested
    inner class ToMap {
        lateinit var configMap: Map<String, Any>

        @BeforeEach
        internal fun setUp() {
            configMap = config.toMap()
        }

        @Test
        fun `returns map of config values`() {
            assertThat(configMap.size, equalTo(5))
        }

        @Test
        fun `includes nested values`() {
            assertAll(
                { assertThat(configMap["test.foo"] as String, equalTo("bar")) },
                { assertThat(configMap["test.number"] as String, equalTo("1")) }
            )
        }

        @Test
        fun `casts all to String value types`() {
            assertAll(
                { assertTrue(configMap["test.foo"] is String) },
                { assertTrue(configMap["test.number"] is String) },
                { assertTrue(configMap["list"] is String) },
                {
                    assertThat(configMap["list"] as String, equalTo("[{one=1, two=2}, {four=4, three=3}]"))
                }
            )
        }
    }
}
