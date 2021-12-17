package com.unity.template.utils

import com.typesafe.config.Config
import io.ktor.config.ApplicationConfig
import io.ktor.config.ApplicationConfigValue
import io.ktor.config.ApplicationConfigurationException

class CustomApplicationConfig(private val config: Config) : ApplicationConfig {
    override fun property(path: String): ApplicationConfigValue {
        if (!config.hasPath(path))
            throw ApplicationConfigurationException("Property $path not found.")
        return CustomApplicationConfigValue(config, path)
    }

    override fun propertyOrNull(path: String): ApplicationConfigValue? {
        if (!config.hasPath(path))
            return null
        return CustomApplicationConfigValue(config, path)
    }

    override fun configList(path: String): List<ApplicationConfig> {
        return config.getConfigList(path).map { CustomApplicationConfig(it) }
    }

    override fun config(path: String): ApplicationConfig = CustomApplicationConfig(config.getConfig(path))

    fun toMap(): Map<String, String> {
        return config.toMap()
    }

    private class CustomApplicationConfigValue(val config: Config, val path: String) : ApplicationConfigValue {
        override fun getString(): String = config.getString(path)
        override fun getList(): List<String> = config.getStringList(path)
    }
}

fun Config.toMap(): Map<String, String> {
    return entrySet().map { it.key to it.value.unwrapped().toString() }.toMap()
}
