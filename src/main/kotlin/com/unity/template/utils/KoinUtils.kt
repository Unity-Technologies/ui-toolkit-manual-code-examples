package com.unity.template.utils

import io.ktor.application.Application
import org.koin.core.error.MissingPropertyException
import org.koin.ktor.ext.getProperty

fun Application.getPropertyOrThrow(key: String): String = getProperty(key) ?: throw MissingPropertyException("Property $key not found")
